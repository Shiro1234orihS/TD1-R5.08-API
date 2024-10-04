using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DPO;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Models.DataManager
{
    public class MarqueManager : IDataRepository<Marque>
    {
        readonly DBContexte? dBContext;

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

        public Task<ProduitDetailDto> GetByIdAsyncProduitDetailDto(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProduitDto> GetByIdAsyncProduitDto(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<ProduitDto>> GetProduitDtoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<ProduitDto>> GetAllAsyncProduitDto()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProduitDto>> IDataRepository<Marque>.GetAllAsyncProduitDto()
        {
            throw new NotImplementedException();
        }
    }
}
