using BackEnd.Application.Common.Exceptions;
using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Sports.Queries.GetSport;

public record GetSportQuery(int Id) : IRequest<SportDetailDto>;

public class GetSportQueryHandler : IRequestHandler<GetSportQuery, SportDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSportQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SportDetailDto> Handle(GetSportQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Sports
            .Include(s => s.Fields)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Sport), request.Id);
        }

        return _mapper.Map<SportDetailDto>(entity);
    }
}
