using BackEnd.Domain.Entities;

namespace BackEnd.Application.Common.Models;

public class LookupDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Sport, LookupDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.SportName));
            CreateMap<Field, LookupDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.FieldName));
        }
    }
}
