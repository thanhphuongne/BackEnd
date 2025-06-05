using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.Common.Mappings;
using BackEnd.Application.Common.Models;

namespace BackEnd.Application.Sports.Queries.GetSports;

public record GetSportsQuery : IRequest<PaginatedList<SportDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetSportsQueryHandler : IRequestHandler<GetSportsQuery, PaginatedList<SportDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSportsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SportDto>> Handle(GetSportsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sports
            .OrderBy(x => x.SportName)
            .ProjectTo<SportDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
