using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.Models;

namespace Client.Services
{
    public class WSMarque
    {
        private static readonly HttpClient Client = new HttpClient { BaseAddress = new Uri("https://localhost:7132/api/") };

        public WSMarque()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Marque>> GetMarqueAsync(string nomControleur)
        {
            try
            {
                return await Client.GetFromJsonAsync<List<Marque>>(nomControleur);
            }
            catch (Exception ex)
            {
                // Log the exception or throw
                return null;
            }
        }
    }
}
