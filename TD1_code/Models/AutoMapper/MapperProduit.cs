using AutoMapper;
using TD1_code.Models.EntityFramework;
using TD1_code.Models.DPO;

namespace TD1_code.Models.AutoMapper
{
    public class MapperProduit : Profile
    {
        public MapperProduit() {
            CreateMap<Produit, ProduitDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProduit))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.IdTypeProduitNavigation.NomTypeProduit))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.IdMarqueNavigation.NomMarque))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockReel))
                .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => src.StockReel < src.StockMin));

            CreateMap<Produit, ProduitDto>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProduit))
                   .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomProduit))
                   .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.IdTypeProduitNavigation.NomTypeProduit))
                   .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.IdMarqueNavigation.NomMarque));

        }
    }
}
