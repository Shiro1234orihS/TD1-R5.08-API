using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PlaywrightTests;

[TestClass]
public class ProduitByDPOTests : PageTest
{
    [TestMethod]
    public async Task LoadProducts_ShouldDisplayProducts_WhenProductsExist()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

        // Log avant le chargement
        Console.WriteLine("Attente du chargement de la table");

        // Attendre la visibilité de la table et vérifier les lignes
        await page.WaitForSelectorAsync("table.table tbody tr");
        Console.WriteLine("Table détectée");

        var rows = await page.Locator("table.table tbody tr").CountAsync();
        Console.WriteLine($"Nombre de lignes trouvées : {rows}");

        Assert.IsTrue(rows > 0, "La liste des produits devrait s'afficher");
    }
    [TestMethod]
    public async Task EditProduct_ShouldOpenModal_WhenEditButtonIsClicked()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

        // Cliquer sur le bouton "Modifier" pour le premier produit
        await page.Locator("button.btn-warning:has-text('Modifier')").First.ClickAsync();

        // Vérifier que la modal de modification est affichée
        var modalVisible = await page.Locator("div.modal.show").IsVisibleAsync();
        Assert.IsTrue(modalVisible, "La modal d'édition devrait s'afficher.");
    }
    [TestMethod]
    public async Task DeleteProduct_ShouldRemoveProductFromList_WhenDeleteButtonIsClicked()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

        // Compter le nombre initial de produits
        var initialRowCount = await page.Locator("table.table tbody tr").CountAsync();

        // Cliquer sur le bouton "Supprimer" pour le premier produit
        await page.Locator("button.btn-danger:has-text('Supprimer')").First.ClickAsync();

        // Attendre que le chargement ou la mise à jour soit terminée
        await page.WaitForTimeoutAsync(1000); // Augmentez le délai si nécessaire

        // Recompter les produits
        var newRowCount = await page.Locator("table.table tbody tr").CountAsync();
        Assert.AreEqual(initialRowCount - 1, newRowCount, "Un produit devrait être supprimé.");
    }

    [TestMethod]
    public async Task ViewProductDetails_ShouldNavigateToDetailsPage_WhenViewDetailsButtonIsClicked()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

        // Cliquer sur le bouton "Voir détails" pour le premier produit
        await page.Locator("button:has-text('Voir détails')").First.ClickAsync();

        // Vérifier que l'URL change pour la page de détails
        await page.WaitForURLAsync(url => url.Contains("/produitDetails/"));
        Assert.IsTrue(page.Url.Contains("/produitDetails/"), "L'URL devrait contenir '/produitDetails/' après navigation.");
    }
}

