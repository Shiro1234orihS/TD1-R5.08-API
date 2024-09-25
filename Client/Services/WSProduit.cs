using Client.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.Services
{
    public class WSProduit : IService
    {
        private HttpClient client;

        public HttpClient Client
        {
            get { return client; }
            set { client = value; }
        }
        public WSProduit(string uri)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(uri);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public WSProduit() : this("http://localhost:5185/api/") { }

        public async Task<Produit> GetproduitAsync(String nomControleur, Int32 produitId)
        {
            try
            {
                return await Client.GetFromJsonAsync<Produit>(String.Concat(nomControleur, "/", produitId));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Produit>> GetproduitsAsync(String nomControleur)
        {
            try
            {
                return await Client.GetFromJsonAsync<List<Produit>>(nomControleur);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Boolean> PostproduitAsync(String nomControleur, Produit produit)
        {
            var response = await Client.PostAsJsonAsync(nomControleur, produit);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteproduitAsync(string nomControler, Produit produit)
        {
            var response = await Client.DeleteAsync(String.Concat(nomControler, "/", produit.IdProduit));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditproduitAsync(string nomControler, int idToEdit, Produit produit)
        {
            var response = await Client.PutAsJsonAsync(String.Concat(nomControler, "/", idToEdit), produit);
            return response.IsSuccessStatusCode;
        }
    }
}
