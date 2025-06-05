using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.Common.Mappings;
using BackEnd.Application.Common.Models;

namespace BackEnd.Application.Fields.Queries.GetFields;

public record GetFieldsQuery : IRequest<PaginatedList<FieldDto>>
{
    public int? SportId { get; init; }
    public string? City { get; init; }
    public string? District { get; init; }
    public string? Location { get; init; }
    public string? FieldType { get; init; }
    public bool? HasLighting { get; init; }
    public bool? HasParking { get; init; }
    public bool? HasRestrooms { get; init; }
    public bool? HasWater { get; init; }
    public bool? HasSoundSystem { get; init; }
    public bool? IsActive { get; init; } = true;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFieldsQueryHandler : IRequestHandler<GetFieldsQuery, PaginatedList<FieldDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFieldsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FieldDto>> Handle(GetFieldsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Fields
            .Include(f => f.Sport)
            .Include(f => f.Owner)
            .AsQueryable();

        if (request.SportId.HasValue)
            query = query.Where(f => f.SportId == request.SportId.Value);

        if (!string.IsNullOrEmpty(request.City))
            query = query.Where(f => f.City != null && f.City.Contains(request.City));

        if (!string.IsNullOrEmpty(request.District))
            query = query.Where(f => f.District != null && f.District.Contains(request.District));

        if (!string.IsNullOrEmpty(request.Location))
            query = query.Where(f => f.Location != null && f.Location.Contains(request.Location));

        if (!string.IsNullOrEmpty(request.FieldType))
            query = query.Where(f => f.FieldType == request.FieldType);

        if (request.HasLighting.HasValue)
            query = query.Where(f => f.HasLighting == request.HasLighting.Value);

        if (request.HasParking.HasValue)
            query = query.Where(f => f.HasParking == request.HasParking.Value);

        if (request.HasRestrooms.HasValue)
            query = query.Where(f => f.HasRestrooms == request.HasRestrooms.Value);

        if (request.HasWater.HasValue)
            query = query.Where(f => f.HasWater == request.HasWater.Value);

        if (request.HasSoundSystem.HasValue)
            query = query.Where(f => f.HasSoundSystem == request.HasSoundSystem.Value);

        if (request.IsActive.HasValue)
            query = query.Where(f => f.IsActive == request.IsActive.Value);

        return await query
            .OrderBy(f => f.City)
            .ThenBy(f => f.District)
            .ThenBy(f => f.FieldName)
            .ProjectTo<FieldDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
