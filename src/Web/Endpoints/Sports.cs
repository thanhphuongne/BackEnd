using BackEnd.Application.Common.Models;
using BackEnd.Application.Sports.Commands.CreateSport;
using BackEnd.Application.Sports.Commands.DeleteSport;
using BackEnd.Application.Sports.Commands.UpdateSport;
using BackEnd.Application.Sports.Queries.GetSport;
using BackEnd.Application.Sports.Queries.GetSports;
using BackEnd.Web.Infrastructure;

namespace BackEnd.Web.Endpoints;

public class Sports : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetSports)
            .MapGet(GetSport, "{id}")
            .MapPost(CreateSport)
            .MapPut(UpdateSport, "{id}")
            .MapDelete(DeleteSport, "{id}");
    }

    public async Task<PaginatedList<SportDto>> GetSports(ISender sender, [AsParameters] GetSportsQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<SportDetailDto> GetSport(ISender sender, int id)
    {
        return await sender.Send(new GetSportQuery(id));
    }

    public async Task<int> CreateSport(ISender sender, CreateSportCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<IResult> UpdateSport(ISender sender, int id, UpdateSportCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteSport(ISender sender, int id)
    {
        await sender.Send(new DeleteSportCommand(id));
        return Results.NoContent();
    }
}
