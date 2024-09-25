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
    public class TypeProduitsControllerTests
    {
        #region Private Fields
        // Déclaration des variables nécessaires pour les tests
        private TypeProduitsController controller;
        private DBContexte context;
        private IDataRepository<TypeProduit> dataRepository;
        #endregion

        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<DBContexte>().UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=postgres");
            DBContexte dbContext = new DBContexte(builder.Options);
            dataRepository = new TypeProduitManager(context);
            // Création du gestionnaire de données et du contrôleur à tester
            TypeProduitsController controller = new TypeProduitsController(dataRepository);
        }

        #region TypeProduitsControllerTest
        [TestMethod()]
        public void TypeProduitsControllerTest()
        {
            // Arrange : préparation des données attendues
            var builder = new DbContextOptionsBuilder<DbContext>().UseNpgsql("Server = 51.83.36.122; port = 5432; Database = sa25; uid = sa25; password = 1G1Nxb; SearchPath = bmw");
            context = new DBContexte(builder.Options);
            dataRepository = new TypeProduitManager(context);

            // Act : appel de la méthode à tester
            var option = new TypeProduitsController(dataRepository);

            // Assert : vérification que les données obtenues correspondent aux données attendues : vérification que les données obtenues correspondent aux données attendues
            Assert.IsNotNull(option, "L'instance de MaClasse ne devrait pas être null.");
        }
        #endregion


        [TestMethod]
        public void GetTypeProduitByIdTest_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();

            ICollection<TypeProduit> AcquisC = new List<TypeProduit>
     {
         new TypeProduit { /* initialisez les propriétés de l'objet ici */ },
         new TypeProduit { /* un autre objet Acquerir */ }
     };


            TypeProduit catre = new TypeProduit
            {
                IdTypeProduit = 1,
                NomTypeProduit = "testMoq"
            };
            // Act
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(catre);
            var userController = new TypeProduitsController(mockRepository.Object);

            var actionResult = userController.GettypeProduitById(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(catre, actionResult.Value as TypeProduit);
        }




        [TestMethod]
        public void PutTypeProduitTestAvecMoq()
        {

            // Arrange
            TypeProduit TypeProduitToUpdate = new TypeProduit
            {
                IdTypeProduit = 200,
                NomTypeProduit = "SVG"

            };
            TypeProduit updatedTypeProduit = new TypeProduit
            {
                IdTypeProduit = 201,
                NomTypeProduit = "Svg"

            };
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(200)).ReturnsAsync(TypeProduitToUpdate);
            mockRepository.Setup(repo => repo.UpdateAsync(TypeProduitToUpdate, updatedTypeProduit)).Returns(Task.CompletedTask);


            var controller = new TypeProduitsController(mockRepository.Object);

            // Act
            var result = controller.PutTypeProduit(200, updatedTypeProduit).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "La réponse n'est pas du type attendu NoContentResult");
        }

        /// <summary>
        /// Test PostUtilisateur 
        /// </summary>
        /// 
        [TestMethod()]
        public void PostTypeProduitTest()
        {
            //// Arrange

            //TypeProduit carte = new TypeProduit
            //{
            //    IdTypeProduit = 100,
            //    NomCarte = "NUNES EMILIO Ricardo ",
            //};

            //// Act
            //var result = controller.PostTypeProduit(carte).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            //// Assert
            //// On récupère l'utilisateur créé directement dans la BD grace à son mail unique
            //TypeProduit? carteRecupere = context.TypeProduits
            //    .Where(u => u.IdTypeProduit == carte.IdTypeProduit)
            //    .FirstOrDefault();

            //// On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            //// Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            //carte.IdTypeProduit = carteRecupere.IdTypeProduit;
            //Assert.AreEqual(carteRecupere, carte, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void PostTypeProduitTest_Mok()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            var userController = new TypeProduitsController(mockRepository.Object);



            // Arrange
            TypeProduit catre = new TypeProduit
            {
                IdTypeProduit = 1,
                NomTypeProduit = "SVG"
            };
            // Act
            var actionResult = userController.PostTypeProduit(catre).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeProduit>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(TypeProduit), "Pas un Utilisateur");
            catre.IdTypeProduit = ((TypeProduit)result.Value).IdTypeProduit;
            Assert.AreEqual(catre, (TypeProduit)result.Value, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void DeleteTypeProduitTest_AvecMoq()
        {

            // Arrange
            TypeProduit catre = new TypeProduit
            {
                IdTypeProduit = 200,
                NomTypeProduit = "SVG"
            };
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(2).Result).Returns(catre);
            var userController = new TypeProduitsController(mockRepository.Object);
            // Act
            var actionResult = userController.DeleteTypeProduit(2).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}