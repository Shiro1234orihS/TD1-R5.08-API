using Client.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.Services
{
    public class WSTypeProduit
    {
        private static readonly HttpClient Client = new HttpClient { BaseAddress = new Uri("https://localhost:7132/api/") };

        public WSTypeProduit()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<TypeProduit>> GetMarqueAsync(string nomControleur)
        {
            try
            {
                return await Client.GetFromJsonAsync<List<TypeProduit>>(nomControleur);
            }
            catch (Exception ex)
            {
                // Log the exception or throw
                return null;
            }
        }
    }
}
