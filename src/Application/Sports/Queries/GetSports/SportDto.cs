using BackEnd.Domain.Entities;

namespace BackEnd.Application.Sports.Queries.GetSports;

public class SportDto
{
    public int Id { get; init; }
    public string SportName { get; init; } = null!;
    public string? Description { get; init; }
    public int FieldCount { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Sport, SportDto>()
                .ForMember(d => d.FieldCount, opt => opt.MapFrom(s => s.Fields.Count));
        }
    }
}
