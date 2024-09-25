using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;


namespace TD1_code.Models.DataManager
{
    public class ProduitManager : IDataRepository<Produit>
    {
        readonly DBContexte? dBContext;

        public ProduitManager() { }

        public ProduitManager(DBContexte context)
        {
            dBContext = context;
        }

        public async Task AddAsync(Produit entity)
        {
            await dBContext.Produits.AddAsync(entity);
            await dBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Produit entity)
        {
            dBContext.Produits.Remove(entity);
            await dBContext.SaveChangesAsync();
        }

        public async Task<ActionResult<IEnumerable<Produit>>> GetAllAsync()
        {
            return await dBContext.Produits.ToListAsync();
        }

        public async Task<ActionResult<Produit>> GetByIdAsync(int id)
        {

            return await dBContext.Produits.FirstOrDefaultAsync(p => p.IdMarque == id);
        }

        public async Task<ActionResult<Produit>> GetByStringAsync(string str)
        {
            return await dBContext.Produits.FirstOrDefaultAsync(p => p.NomProduit.ToUpper() == str.ToUpper());
        }

        public async Task UpdateAsync(Produit entityToUpdate, Produit entity)
        {
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;

            entityToUpdate.IdProduit = entity.IdProduit;
            entityToUpdate.NomProduit = entity.NomProduit;
            entityToUpdate.Description = entity.Description;
            entityToUpdate.NomPhoto = entity.NomPhoto;
            entityToUpdate.UriPhoto = entity.UriPhoto;
            entityToUpdate.IdTypeProduit = entity.IdTypeProduit;
            entityToUpdate.IdMarque = entity.IdMarque;
            entityToUpdate.StockReel = entity.StockReel;
            entityToUpdate.StockMin = entity.StockMin;
            entityToUpdate.StockMax = entity.StockMax;

            await dBContext.SaveChangesAsync();
        }
    }
}
