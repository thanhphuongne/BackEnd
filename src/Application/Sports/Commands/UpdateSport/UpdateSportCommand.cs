using BackEnd.Application.Common.Exceptions;
using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Sports.Commands.UpdateSport;

public record UpdateSportCommand : IRequest
{
    public int Id { get; init; }
    public string SportName { get; init; } = null!;
    public string? Description { get; init; }
}

public class UpdateSportCommandHandler : IRequestHandler<UpdateSportCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSportCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Sports
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Sport), request.Id);
        }

        entity.SportName = request.SportName;
        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
