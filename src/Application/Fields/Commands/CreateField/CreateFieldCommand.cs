using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Fields.Commands.CreateField;

public record CreateFieldCommand : IRequest<int>
{
    public int SportId { get; init; }
    public string FieldName { get; init; } = null!;
    public string? Location { get; init; }
    public string? City { get; init; }
    public string? District { get; init; }
    public string? Address { get; init; }
    public int? Capacity { get; init; }
    public string? Description { get; init; }
    public string? FieldType { get; init; }
    public bool HasLighting { get; init; }
    public bool HasParking { get; init; }
    public bool HasRestrooms { get; init; }
    public bool HasWater { get; init; }
    public bool HasSoundSystem { get; init; }
    public string? ImageUrl { get; init; }
    public int OwnerId { get; init; }
}

public class CreateFieldCommandHandler : IRequestHandler<CreateFieldCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = new Field
        {
            SportId = request.SportId,
            FieldName = request.FieldName,
            Location = request.Location,
            City = request.City,
            District = request.District,
            Address = request.Address,
            Capacity = request.Capacity,
            Description = request.Description,
            FieldType = request.FieldType,
            HasLighting = request.HasLighting,
            HasParking = request.HasParking,
            HasRestrooms = request.HasRestrooms,
            HasWater = request.HasWater,
            HasSoundSystem = request.HasSoundSystem,
            ImageUrl = request.ImageUrl,
            OwnerId = request.OwnerId
        };

        _context.Fields.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
