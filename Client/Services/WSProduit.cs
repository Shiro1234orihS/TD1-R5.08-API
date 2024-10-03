using Client.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.Models;
public class WSProduit : IService
{
    private static readonly HttpClient Client = new HttpClient { BaseAddress = new Uri("https://localhost:7132/api/") };

    public WSProduit()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<Produit> GetProduitAsync(string nomControleur, int produitId)
    {
        try
        {
            return await Client.GetFromJsonAsync<Produit>(String.Concat(nomControleur, "/", produitId));
        }
        catch (Exception ex)
        {
            // Log the exception or throw
            return null;
        }
    }

    public async Task<List<Produit>> GetProduitsAsync(string nomControleur)
    {
        try
        {
            return await Client.GetFromJsonAsync<List<Produit>>(nomControleur);
        }
        catch (Exception ex)
        {
            // Log the exception or throw
            return null;
        }
    }

    public async Task<bool> PostProduitAsync(string nomControleur, Produit produit)
    {
        
        var response = await Client.PostAsJsonAsync(nomControleur, produit);
        if (!response.IsSuccessStatusCode)
        {
            // Optionally handle error
            var error = await response.Content.ReadAsStringAsync();
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteProduitAsync(string nomControler, int produitId)
    {
        var response = await Client.DeleteAsync(String.Concat(nomControler, "/", produitId));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EditProduitAsync(string nomControler, int idToEdit, Produit produit)
    {
        var response = await Client.PutAsJsonAsync(String.Concat(nomControler, "/", idToEdit), produit);
        return response.IsSuccessStatusCode;
    }
}
