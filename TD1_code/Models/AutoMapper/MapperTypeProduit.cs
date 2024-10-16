using AutoMapper;
using TD1_code.Models.EntityFramework;
using TD1_code.Models.DTO;

namespace TD1_code.Models.AutoMapper
{
    public class MapperTypeProduit : Profile
    {
        public MapperTypeProduit()
        {
            CreateMap<TypeProduit, TypeProduitDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdTypeProduit))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomTypeProduit))
        }
    }
}
