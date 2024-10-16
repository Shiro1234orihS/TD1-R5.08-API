using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DPO;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Models.DataManager
{
    public class TypeProduitManager : IDataRepository<TypeProduit>
    {
        readonly DBContexte? dBContext;

        public TypeProduitManager() { }

        public TypeProduitManager(DBContexte context)
        {
            dBContext = context;
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
    }
}
