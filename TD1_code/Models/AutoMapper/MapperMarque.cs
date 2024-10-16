using AutoMapper;
using TD1_code.Models.EntityFramework;
using TD1_code.Models.DTO;

namespace TD1_code.Models.AutoMapper
{
    public class MapperMarque : Profile
    {
        public MapperMarque()
        {
            CreateMap<Marque, MarqueDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdMarque))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomMarque))
                .ForMember(dest => dest.NbProduits, opt => opt.MapFrom(src => src.Produits.Count));
                
        }
    }
}
