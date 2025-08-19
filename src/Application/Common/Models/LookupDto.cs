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
            CreateMap<Field, LookupDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.FieldName));
            CreateMap<User, LookupDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.FullName));
        }
    }
}
