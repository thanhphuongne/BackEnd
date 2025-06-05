using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Sports.Commands.CreateSport;

public record CreateSportCommand : IRequest<int>
{
    public string SportName { get; init; } = null!;
    public string? Description { get; init; }
}

public class CreateSportCommandHandler : IRequestHandler<CreateSportCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSportCommand request, CancellationToken cancellationToken)
    {
        var entity = new Sport
        {
            SportName = request.SportName,
            Description = request.Description
        };

        _context.Sports.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
