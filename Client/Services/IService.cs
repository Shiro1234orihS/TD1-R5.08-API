using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IService
    {
        Task<List<Produit>> GetProduitsAsync(string nomControleur);
        Task<Produit> GetProduitAsync(string nomControleur, int produitId);
        Task<bool> PostProduitAsync(string nomControleur, Produit produit);
    }
}
