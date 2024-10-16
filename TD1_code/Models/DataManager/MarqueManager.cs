using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.AutoMapper;
using TD1_code.Models.DTO;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Models.DataManager
{
    public class MarqueManager : IDataRepository<Marque> , IDataDtoMarque
    {
        readonly DBContexte? dBContext;
        private readonly IMapper _mapper;
        public MarqueManager() { }

        public MarqueManager(DBContexte context)
        {
            dBContext = context;
        }

        public async Task<ActionResult<IEnumerable<Marque>>> GetAllAsync()
        {
            return await dBContext.Marques.ToListAsync();
        }

        public async Task<ActionResult<Marque>> GetByIdAsync(int id)
        {
            return await dBContext.Marques.FirstOrDefaultAsync(u => u.IdMarque == id);
        }
        public async Task<ActionResult<Marque>> GetByStringAsync(string str)
        {
            return await dBContext.Marques.FirstOrDefaultAsync(u => u.NomMarque.ToUpper() == str.ToUpper());
        }
        public async Task AddAsync(Marque entity)
        {
            await dBContext.Marques.AddAsync(entity);
            await dBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Marque entityToUpdate, Marque entity)
        {
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;

            entityToUpdate.IdMarque = entity.IdMarque;
            entityToUpdate.NomMarque = entity.NomMarque;

            await dBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Marque entity)
        {
            dBContext.Marques.Remove(entity);
            await dBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MarqueDto>> GetAllAsyncMarqueDto()
        {
            var marques = await dBContext.Marques
                                          .Include(p => p.Produits)
                                          .ToListAsync();


            // Mapper une liste de produits vers une liste de ProduitDto
            IEnumerable<MarqueDto> marquesDtos = _mapper.Map<IEnumerable<MarqueDto>>(marques);

            return marquesDtos;
        }

        public async Task<MarqueDto> GetByIdAsyncMarqueDetailDto(int id)
        {
            var marque = await dBContext.Marques
                                     .Include(p => p.Produits)
                                     .FirstOrDefaultAsync(p => p.IdMarque == id);

            if (marque == null)
            {
                return null; // Produit non trouvé
            }

            MarqueDto marqueDto = _mapper.Map<MarqueDto>(marque);
            return marqueDto;
        }
    }
}
