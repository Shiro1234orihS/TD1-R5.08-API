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
public class ProduitTest : PageTest
{
    [TestMethod]
    public async Task LoadProducts_ShouldDisplayProducts_WhenProductsExist()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

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
        await page.WaitForTimeoutAsync(1000);

        // Recompter les produits
        var newRowCount = await page.Locator("table.table tbody tr").CountAsync();
        Assert.AreEqual(initialRowCount - 1, newRowCount, "Un produit devrait être supprimé.");
    }

    [TestMethod]
    public async Task EditProduct_SaveChanges_ShouldUpdateProductDetails()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

        // Ouvrir le modal d'édition pour le premier produit
        await page.Locator("button.btn-warning:has-text('Modifier')").First.ClickAsync();

        // Vérifier que le modal est visible
        var modalVisible = await page.Locator("div.modal.show").IsVisibleAsync();
        Assert.IsTrue(modalVisible, "La modal d'édition devrait s'afficher.");

        // Modifier des informations dans le formulaire
        await page.FillAsync("#nomProduit", "Nouveau Nom");
        await page.FillAsync("#description", "Nouvelle Description");
        await page.FillAsync("#stockReel", "100");
        await page.FillAsync("#stockMin", "10");
        await page.FillAsync("#stockMax", "200");

        // Sauvegarder les modifications
        await page.Locator("button:has-text('Sauvegarder')").ClickAsync();

        // Vérifier que le modal se ferme
        modalVisible = await page.Locator("div.modal.show").IsVisibleAsync();
        Assert.IsFalse(modalVisible, "La modal d'édition devrait être fermée après sauvegarde.");
    }

    [TestMethod]
    public async Task DisplayNoProductMessage_WhenNoProductExists()
    {
        var page = await Browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7016/produitbydpo");

        // Simuler une liste de produits vide (vous devrez peut-être adapter selon le contexte)
        await page.EvaluateAsync("window['produits'] = [];");

        // Vérifier le message d'absence de produit
        await page.WaitForSelectorAsync("td.text-center:has-text('Aucun produit trouvé')");
        Assert.IsTrue(await page.Locator("td.text-center:has-text('Aucun produit trouvé')").IsVisibleAsync(),
                      "Le message 'Aucun produit trouvé' devrait être affiché.");
    }


}

