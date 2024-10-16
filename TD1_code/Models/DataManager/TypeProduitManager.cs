using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DTO;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Models.DataManager
{
    public class TypeProduitManager : IDataRepository<TypeProduit> , IDataDtoTypeProduit
    {
        readonly DBContexte? dBContext;
        private readonly IMapper _mapper;

        public TypeProduitManager() { }

        public TypeProduitManager(DBContexte context, IMapper mapper)
        {
            dBContext = context;
            _mapper = mapper;
        }

        public async Task AddAsync(TypeProduit entity)
        {
            await dBContext.TypeProduits.AddAsync(entity);
            await dBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TypeProduit entity)
        {
            dBContext.TypeProduits.Remove(entity);
            await dBContext.SaveChangesAsync();
        }

        public async Task<ActionResult<IEnumerable<TypeProduit>>> GetAllAsync()
        {
            return await dBContext.TypeProduits.ToListAsync();
        }

      

        public async Task<ActionResult<TypeProduit>> GetByIdAsync(int id)
        {
            return await dBContext.TypeProduits.FirstOrDefaultAsync(p => p.IdTypeProduit == id);
        }


        public async Task<ActionResult<TypeProduit>> GetByStringAsync(string str)
        {
            return await dBContext.TypeProduits.FirstOrDefaultAsync(p => p.NomTypeProduit.ToUpper() == str.ToUpper());
        }

       

        public async Task UpdateAsync(TypeProduit entityToUpdate, TypeProduit entity)
        {
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;

            entityToUpdate.IdTypeProduit = entity.IdTypeProduit;
            entityToUpdate.NomTypeProduit = entity.NomTypeProduit;

            await dBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TypeProduitDto>> GetAllAsyncTypeProduitDto()
        {
            var typeProduits = await dBContext.TypeProduits
                                       .Include(p => p.Produits)
                                       .ToListAsync();


            // Mapper une liste de produits vers une liste de ProduitDto
            IEnumerable<TypeProduitDto> typeProduiDtos = _mapper.Map<IEnumerable<TypeProduitDto>>(typeProduits);

            return typeProduiDtos;
        }

        public async Task<TypeProduitDto> GetByIdAsyncTypeProduitDetailDto(int id)
        {
            var typeProduit = await dBContext.TypeProduits
                                      .Include(p => p.Produits)
                                      .FirstOrDefaultAsync(p => p.IdTypeProduit == id);

            if (typeProduit == null)
            {
                return null; // Produit non trouvé
            }

            TypeProduitDto produitDetailDto = _mapper.Map<TypeProduitDto>(typeProduit);
            return produitDetailDto;
        }
    }
}
