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
            var builder = new DbContextOptionsBuilder<DBContexte>().UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=postgres");
            DBContexte dbContext = new DBContexte(builder.Options);
            dataRepository = new ProduitManager(context);
            // Création du gestionnaire de données et du contrôleur à tester
            ProduitsController controller = new ProduitsController(dataRepository);
        }

        #region ProduitsControllerTest
        [TestMethod()]
        public void ProduitsControllerTest()
        {
            // Arrange : préparation des données attendues
            var builder = new DbContextOptionsBuilder<DbContext>().UseNpgsql("Server = 51.83.36.122; port = 5432; Database = sa25; uid = sa25; password = 1G1Nxb; SearchPath = bmw");
            context = new DBContexte(builder.Options);
            dataRepository = new ProduitManager(context);

            // Act : appel de la méthode à tester
            var option = new ProduitsController(dataRepository);

            // Assert : vérification que les données obtenues correspondent aux données attendues : vérification que les données obtenues correspondent aux données attendues
            Assert.IsNotNull(option, "L'instance de MaClasse ne devrait pas être null.");
        }
        #endregion


        [TestMethod]
        public void GetProduitByIdTest_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Produit>>();

            ICollection<Produit> AcquisC = new List<Produit>
            {
                new Produit { /* initialisez les propriétés de l'objet ici */ },
                new Produit { /* un autre objet Acquerir */ }
            };


            Produit catre = new Produit
            {
                IdProduit = 1,
                NomProduit = "testMoq"
            };
            // Act
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(catre);
            var userController = new ProduitsController(mockRepository.Object);

            var actionResult = userController.GetproduitById(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(catre, actionResult.Value as Produit);
        }




        [TestMethod]
        public void PutProduitTestAvecMoq()
        {

            // Arrange
            Produit ProduitToUpdate = new Produit
            {
                IdProduit = 200,
                NomProduit = "SVG"

            };
            Produit updatedProduit = new Produit
            {
                IdProduit = 201,
                NomProduit = "Svg"

            };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(200)).ReturnsAsync(ProduitToUpdate);
            mockRepository.Setup(repo => repo.UpdateAsync(ProduitToUpdate, updatedProduit)).Returns(Task.CompletedTask);


            var controller = new ProduitsController(mockRepository.Object);

            // Act
            var result = controller.PutProduit(200, updatedProduit).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "La réponse n'est pas du type attendu NoContentResult");
        }

        /// <summary>
        /// Test PostUtilisateur 
        /// </summary>
        /// 
        [TestMethod()]
        public void PostProduitTest()
        {
            //// Arrange

            //Produit carte = new Produit
            //{
            //    IdProduit = 100,
            //    NomCarte = "NUNES EMILIO Ricardo ",
            //};

            //// Act
            //var result = controller.PostProduit(carte).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            //// Assert
            //// On récupère l'utilisateur créé directement dans la BD grace à son mail unique
            //Produit? carteRecupere = context.Produits
            //    .Where(u => u.IdProduit == carte.IdProduit)
            //    .FirstOrDefault();

            //// On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            //// Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            //carte.IdProduit = carteRecupere.IdProduit;
            //Assert.AreEqual(carteRecupere, carte, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void PostProduitTest_Mok()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var userController = new ProduitsController(mockRepository.Object);



            // Arrange
            Produit catre = new Produit
            {
                IdProduit = 1,
                NomProduit = "SVG"
            };
            // Act
            var actionResult = userController.PostProduit(catre).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Produit>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Produit), "Pas un Utilisateur");
            catre.IdProduit = ((Produit)result.Value).IdProduit;
            Assert.AreEqual(catre, (Produit)result.Value, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void DeleteProduitTest_AvecMoq()
        {

            // Arrange
            Produit catre = new Produit
            {
                IdProduit = 200,
                NomProduit = "SVG"
            };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(2).Result).Returns(catre);
            var userController = new ProduitsController(mockRepository.Object);
            // Act
            var actionResult = userController.DeleteProduit(2).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}