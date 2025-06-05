using BackEnd.Domain.Entities;

namespace BackEnd.Application.Fields.Queries.GetFields;

public class FieldDto
{
    public int Id { get; init; }
    public int SportId { get; init; }
    public string SportName { get; init; } = null!;
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
    public decimal? Rating { get; init; }
    public int ReviewCount { get; init; }
    public bool IsActive { get; init; }
    public string OwnerName { get; init; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Field, FieldDto>()
                .ForMember(d => d.SportName, opt => opt.MapFrom(s => s.Sport.SportName))
                .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner.FullName ?? s.Owner.UserName));
        }
    }
}
