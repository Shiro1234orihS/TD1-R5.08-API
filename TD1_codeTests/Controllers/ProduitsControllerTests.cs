using Microsoft.VisualStudio.TestTools.UnitTesting;
using TD1_code.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TD1_code.Models.DataManager;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Controllers.Tests
{
    [TestClass()]
    public class ProduitsControllerTests
    {
        #region Private Fields
        // Déclaration des variables nécessaires pour les tests
        private ProduitsController controller;
        private DBContexte context;
        private IDataRepository<Produit> dataRepository;
        #endregion

        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<DBContexte>()
                .UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=postgres");
            context = new DBContexte(builder.Options);  // Assurer que le context est bien initialisé
            dataRepository = new ProduitManager(context);  // Initialiser ProduitManager avec le context
            controller = new ProduitsController(dataRepository);  // Utiliser le repository dans le contrôleur
        }

        #region Test unitaires

        [TestMethod]
        public async Task GetProduits_ReturnsRightItems()
        {
            // Act
            var result = await controller.Getproduits();  // Attendre la réponse

            List<Produit> expected = context.Produits.ToList();  // Produits attendus dans la base

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Produit>>), "Pas un ActionResult");
            var actionResult = result as ActionResult<IEnumerable<Produit>>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            CollectionAssert.AreEqual(expected, (List<Produit>)actionResult.Value, "Pas les mêmes Produits");
        }


        [TestMethod]
        public async Task GetProduitById_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var result = await controller.GetproduitById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Produit>), "Pas un ActionResult");
            var actionResult = result as ActionResult<Produit>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            Assert.IsInstanceOfType(actionResult.Value, typeof(Produit), "Pas un Produit");
            Assert.AreEqual(context.Produits.Where(c => c.IdProduit == 1).FirstOrDefault(),
                (Produit)actionResult.Value, "Produits pas identiques");
        }

        [TestMethod]
        public async Task GetProduitById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var result = await controller.GetproduitById(0);  // Attendre la réponse

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Produit>), "Pas un ActionResult");
            Assert.IsNull(result.Value, "Produit pas null");
        }

        [TestMethod]
        public async Task PostProduit_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);

            Produit ProduitAtester = new Produit()
            {
                IdProduit = id,
                NomProduit = "test",
                Description = "test",
                NomPhoto = "test",
                UriPhoto = "test",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 0,
                StockMin = 0,
                StockMax = 0
            };

            // Act
            var postResult = await controller.PostProduit(ProduitAtester);

            // Sauvegarder les changements dans le contexte pour s'assurer que l'ajout est persistant
            await context.SaveChangesAsync();

            // Assert
            // Vérifier si le produit a été correctement ajouté à la base de données
            Produit? ProduitRecupere = context.Produits.Where(u => u.IdProduit == ProduitAtester.IdProduit).FirstOrDefault();
            Assert.IsNotNull(ProduitRecupere, "Le produit n'a pas été trouvé dans la base de données.");

            // Vérifier que les produits sont identiques
            Assert.AreEqual(ProduitAtester, ProduitRecupere, "Les produits ne correspondent pas.");
            // Ajouter d'autres comparaisons si nécessaire pour les autres propriétés du produit.
            await controller.DeleteProduit(id);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public async Task PostProduit_CreationFailed()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);

            // Créer un produit invalide (par exemple, sans nom)
            Produit ProduitAtester = new Produit()
            {
                IdProduit = id,
                NomProduit = null,  // Cela devrait provoquer une exception car le nom est requis
                Description = "test",
                NomPhoto = "test",
                UriPhoto = null,
                IdTypeProduit = 0,
                IdMarque = 0,
                StockReel = 0,
                StockMin = 0,
                StockMax = 0
            };

            // Act
            // Cette ligne devrait lancer une exception, car le produit n'est pas valide
            await controller.PostProduit(ProduitAtester);
        }


        // IDEM POUR LE PUT.
        [TestMethod]
        public async Task PutProduit_ModelValidated_UpdateOK()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);

            // Création d'un produit initial
            Produit ProduitInitial = new Produit()
            {
                IdProduit = id,
                NomProduit = "ProduitInitial",
                Description = "DescriptionInitiale",
                NomPhoto = "PhotoInitiale",
                UriPhoto = "UriInitiale",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 0,
                StockMin = 0,
                StockMax = 0
            };

            // Ajouter le produit initial dans la base de données
            await controller.PostProduit(ProduitInitial);
            await context.SaveChangesAsync(); // Sauvegarder les changements

            // Création d'un produit mis à jour avec les mêmes Id mais avec d'autres valeurs
            Produit ProduitUpdated = new Produit()
            {
                IdProduit = id,  // Utilisation du même Id
                NomProduit = "ProduitMisAJour",
                Description = "DescriptionMisAJour",
                NomPhoto = "PhotoMisAJour",
                UriPhoto = "UriMisAJour",
                IdTypeProduit = 2,
                IdMarque = 2,
                StockReel = 10,
                StockMin = 5,
                StockMax = 15
            };

            // Act
            var result = await controller.PutProduit(ProduitUpdated.IdProduit, ProduitUpdated); // Mettre à jour le produit
            await context.SaveChangesAsync(); // Sauvegarder les changements après la mise à jour

            // Assert
            Produit? ProduitRecupere = context.Produits.Where(c => c.IdProduit == ProduitUpdated.IdProduit).FirstOrDefault();
            Assert.IsNotNull(ProduitRecupere, "Le produit mis à jour n'a pas été trouvé dans la base de données.");

            // Vérifier que les valeurs mises à jour correspondent
            Assert.AreEqual(ProduitUpdated.NomProduit, ProduitRecupere.NomProduit, "Le nom du produit n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.Description, ProduitRecupere.Description, "La description du produit n'a pas été mise à jour.");
            Assert.AreEqual(ProduitUpdated.NomPhoto, ProduitRecupere.NomPhoto, "Le nom de la photo n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.UriPhoto, ProduitRecupere.UriPhoto, "L'URI de la photo n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.IdTypeProduit, ProduitRecupere.IdTypeProduit, "Le type de produit n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.IdMarque, ProduitRecupere.IdMarque, "La marque du produit n'a pas été mise à jour.");
            Assert.AreEqual(ProduitUpdated.StockReel, ProduitRecupere.StockReel, "Le stock réel n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.StockMin, ProduitRecupere.StockMin, "Le stock minimum n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.StockMax, ProduitRecupere.StockMax, "Le stock maximum n'a pas été mis à jour.");
        }


        // Pareil pour les autres tests PUT

        [TestMethod]
        public async Task DeleteProduitTest()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);
            Produit ProduitASuppr = new Produit()
            {
                IdProduit = id,
                NomProduit = "test",
                Description = "test",
                NomPhoto = "test",
                UriPhoto  = "test",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 0,
                StockMin = 0,
                StockMax = 0
            };

            context.Produits.Add(ProduitASuppr);
            await context.SaveChangesAsync();
            ProduitASuppr.IdProduit = context.Produits.Where(c => c.IdProduit == ProduitASuppr.IdProduit).FirstOrDefault().IdProduit;

            // Act
            var result = await controller.DeleteProduit(ProduitASuppr.IdProduit);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
            Assert.IsNull(context.Produits.Where(c => c.IdProduit == ProduitASuppr.IdProduit).FirstOrDefault());
        }

        #endregion


        #region Tests de substitution

        [TestMethod]
        public void GetProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Produit Produit = new Produit
            {
                NomProduit = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(Produit);

            var ProduitController = new ProduitsController(mockRepository.Object);

            // Act
            var actionResult = ProduitController.GetproduitById(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(Produit, actionResult.Value as Produit);
        }

        [TestMethod]
        public void GetProduitById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var ProduitController = new ProduitsController(mockRepository.Object);

            // Act
            var actionResult = ProduitController.GetproduitById(0).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var userController = new ProduitsController(mockRepository.Object);

            Produit user = new Produit
            {
                NomProduit = "SVG"
            };

            // Act
            var actionResult = userController.PostProduit(user).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Produit>), "Pas un ActionResult<Produit>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Produit), "Pas un Produit");
            user.IdProduit = ((Produit)result.Value).IdProduit;
            Assert.AreEqual(user, (Produit)result.Value, "Produits pas identiques");
        }

        [TestMethod]
        public void DeleteProduitTest_AvecMoq()
        {
            // Arrange
            Produit user = new Produit
            {
                NomProduit = "SVG",
            };

            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new ProduitsController(mockRepository.Object);

            // Act
            var actionResult = userController.DeleteProduit(1).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        [TestMethod]
        public async Task PutProduit_ModelValidated_UpdateOK_AvecMoq()
        {
            // Arrange
            Produit userAMaJ = new Produit
            {
                IdProduit = 1,   // Initialiser l'ID du produit à mettre à jour
                NomProduit = "SVG",
                Description = "Ancienne description"
            };

            Produit userUpdated = new Produit
            {
                IdProduit = 1,   // Assurez-vous que l'ID correspond à celui que vous souhaitez mettre à jour
                NomProduit = "SVG",
                Description = "Nouvelle description"   // Modifiez une propriété pour simuler la mise à jour
            };

            // Simuler le repository avec Moq
            var mockRepository = new Mock<IDataRepository<Produit>>();

            // Simuler la méthode GetByIdAsync pour retourner le produit à mettre à jour
            mockRepository.Setup(x => x.GetByIdAsync(userAMaJ.IdProduit)).ReturnsAsync(userAMaJ);

            // Simuler la méthode UpdateAsync avec deux paramètres
            mockRepository.Setup(x => x.UpdateAsync(userAMaJ, userUpdated)).Returns(Task.CompletedTask);

            // Créer le contrôleur avec le mock du repository
            var userController = new ProduitsController(mockRepository.Object);

            // Act
            var actionResult = await userController.PutProduit(userUpdated.IdProduit, userUpdated);

            // Assert
            // Vérifier que la méthode UpdateAsync a bien été appelée avec les bons paramètres
            mockRepository.Verify(x => x.UpdateAsync(userAMaJ, userUpdated), Times.Once);

            // Vérifier que le retour est bien un NoContentResult, ce qui signifie que la mise à jour a réussi
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }



        #endregion
    }
}