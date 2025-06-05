using BackEnd.Application.Common.Exceptions;
using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Sports.Commands.DeleteSport;

public record DeleteSportCommand(int Id) : IRequest;

public class DeleteSportCommandHandler : IRequestHandler<DeleteSportCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSportCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Sports
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Sport), request.Id);
        }

        _context.Sports.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
