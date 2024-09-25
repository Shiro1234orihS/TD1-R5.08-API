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
        public Task<List<Produit>> GetSeriesAsync(string nomControleur);
        public Task<Produit> GetSerieAsync(string nomControleur, int serieId);
        public Task<bool> PostSerieAsync(string nomControleur, Produit serie);
    }
}
