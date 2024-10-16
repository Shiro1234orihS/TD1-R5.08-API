using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DPO;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;


namespace TD1_code.Models.DataManager
{
    public class ProduitManager : IDataRepository<Produit>, IDataDpoProduit
    {
        readonly DBContexte? dBContext;
        private readonly IMapper _mapper;


        public ProduitManager() { }

        public ProduitManager(DBContexte context , IMapper mapper)
        {
            dBContext = context;
            _mapper = mapper;
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

        public async Task<IEnumerable<ProduitDto>> GetAllAsyncProduitDto()
        {
            var produits = await dBContext.Produits
                                          .Include(p => p.IdTypeProduitNavigation)
                                          .Include(p => p.IdMarqueNavigation)
                                          .ToListAsync();

            
            // Mapper une liste de produits vers une liste de ProduitDto
            IEnumerable<ProduitDto> produitDtos = _mapper.Map<IEnumerable<ProduitDto>>(produits);

            return produitDtos;
        }

        public async Task<ProduitDetailDto> GetByIdAsyncProduitDetailDto(int id)
        {
            var produit = await dBContext.Produits
                                         .Include(p => p.IdTypeProduitNavigation) 
                                         .Include(p => p.IdMarqueNavigation)      
                                         .FirstOrDefaultAsync(p => p.IdProduit == id);

            if (produit == null)
            {
                return null; // Produit non trouvé
            }

            ProduitDetailDto produitDetailDto = _mapper.Map<ProduitDetailDto>(produit);
            return produitDetailDto;
        }

        

        public async Task<ActionResult<Produit>> GetByIdAsync(int id)
        {
            return await dBContext.Produits.FirstOrDefaultAsync(p => p.IdProduit == id);
        }

        public async Task<ActionResult<ProduitDto>> GetProduitDtoAsync(int id) 
        {
            Produit produitrecherche = await dBContext.Produits.FirstOrDefaultAsync(p => p.IdMarque == id);

            ProduitDto DPOproduit = new ProduitDto
            {
                Id = produitrecherche.IdProduit,
                Nom = produitrecherche.NomProduit
            };

            return DPOproduit;
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

        public Task<ActionResult<Produit>> GetByStringAsync(string str)
        {
            throw new NotImplementedException();
        }

       
       
    }
}
