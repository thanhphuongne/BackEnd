using BackEnd.Application.Common.Models;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Sports.Queries.GetSport;

public class SportDetailDto
{
    public int Id { get; init; }
    public string SportName { get; init; } = null!;
    public string? Description { get; init; }
    public IList<LookupDto> Fields { get; init; } = Array.Empty<LookupDto>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Sport, SportDetailDto>()
                .ForMember(d => d.Fields, opt => opt.MapFrom(s => s.Fields));
        }
    }
}
