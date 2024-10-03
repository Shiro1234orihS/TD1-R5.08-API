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
            var builder = new DbContextOptionsBuilder<DBContexte>().UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=Ricardo2003@");
            DBContexte dbContext = new DBContexte(builder.Options);
            dataRepository = new ProduitManager(context);
            // Création du gestionnaire de données et du contrôleur à tester
            ProduitsController controller = new ProduitsController(dataRepository);
        }

        #region Test unitaires

        [TestMethod]
        public void GetProduits_ReturnsRightItems()
        {
            // Arrange
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.Getproduits();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<IEnumerable<Produit>>), "Pas un ActionResult");
            ActionResult<IEnumerable<Produit>> actionResult = result.Result as ActionResult<IEnumerable<Produit>>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            CollectionAssert.AreEqual(context.Produits.ToList(), (List<Produit>)actionResult.Value, "Pas les mêmes Produits");
        }

        [TestMethod]
        public void GetProduitById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.GetproduitById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Produit>), "Pas un ActionResult");

            var actionResult = result.Result as ActionResult<Produit>;

            // Assert
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            Assert.IsInstanceOfType(actionResult.Value, typeof(Produit), "Pas un Produit");
            Assert.AreEqual(context.Produits.Where(c => c.IdProduit == 1).FirstOrDefault(),
                (Produit)actionResult.Value, "Produits pas identiques");
        }

        [TestMethod]
        public void GetProduitById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.GetproduitById(0);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Produit>), "Pas un ActionResult");
            Assert.IsNull(result.Result.Value, "Produit pas null");
        }

        //[TestMethod]
        //public void GetProduitByEmail_ExistingEmailPassed_ReturnsRightItem()
        //{
        //    // Arrange
        //    ProduitsController controller = new ProduitsController(dataRepository);

        //    // Act
        //    var result = controller.Getb("gdominguez0@washingtonpost.com");

        //    // Assert
        //    Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Produit>), "Pas un ActionResult");

        //    var actionResult = result.Result as ActionResult<Produit>;

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult.Value, typeof(Produit), "Pas un Produit");
        //    Assert.IsNotNull(actionResult.Value, "Valeur nulle");
        //    Assert.AreEqual(context.Produits.Where(c => c.Mail.ToLower() == "gdominguez0@washingtonpost.com").FirstOrDefault(),
        //        (Produit)actionResult.Value, "Produits pas identiques");
        //}

        //[TestMethod]
        //public void GetProduitByEmail_UnknownEmailPassed_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    ProduitsController controller = new ProduitsController(dataRepository);

        //    // Act
        //    var result = controller.GetProduitByEmail("123@free.fr");

        //    // Assert
        //    Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Produit>), "Pas un ActionResult");
        //    Assert.IsNull(result.Result.Value, "Produit pas null");
        //}

        [TestMethod]
        public void PostProduit_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le Produit après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE du WS
            Produit ProduitAtester = new Produit()
            {
                NomProduit = "SVG",
            };
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.PostProduit(ProduitAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre la création

            // Assert
            Produit? ProduitRecupere = context.Produits.Where(u => u.NomProduit.ToUpper() == ProduitAtester.NomProduit.ToUpper()).FirstOrDefault(); //On récupère l'Produit créé directement dans la BD grace à son mail unique
            // On ne connait pas l'ID de l’Produit envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 Produits
            ProduitAtester.IdProduit = ProduitRecupere.IdProduit;
            Assert.AreEqual(ProduitAtester, ProduitRecupere, "Produits pas identiques");
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostCompte_MailMissing_CreationFailed()
        {
            // Arrange
            Produit ProduitAtester = new Produit()
            {
                NomProduit = "SVG",
            };
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.PostProduit(ProduitAtester).Result;
        }

        //IDEM POUR LES AUTRES CHAMPS NOT NULL (Pwd)

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostCompte_EmailNotUnique_CreationFailed()
        {
            // Arrange
            Produit? Produit = context.Produits.Where(c => c.IdProduit == 1).FirstOrDefault();
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            // On ajoute un Produit existant
            var result = controller.PostProduit(Produit).Result;
        }




        //IDEM POUR LES AUTRES CHAMPS NON CONFORMES AU MODELE


        // IDEM POUR LE PUT.
        [TestMethod]
        public void PutProduit_ModelValidated_UpdateOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Produit Produit = new Produit
            {
                NomProduit = "SVG",
            };
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.PutProduit(Produit.IdProduit, Produit).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre la création

            // Assert
            Produit? ProduitRecupere = context.Produits.Where(u => u.NomProduit.ToUpper() == Produit.NomProduit.ToUpper()).FirstOrDefault(); //On récupère l'Produit créé directement dans la BD grace à son mail unique
            // On ne connait pas l'ID de l’Produit envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 Produits
            Assert.AreEqual(Produit, ProduitRecupere, "Produits pas identiques");
        }

        // Pareil pour les autres tests PUT


        [TestMethod()]
        public void DeleteProduitTest()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Produit ProduitASuppr = new Produit()
            {
                NomProduit = "SVG",
            };
            context.Produits.Add(ProduitASuppr);
            context.SaveChanges();
            ProduitASuppr.IdProduit = context.Produits.Where(c => c.NomProduit.ToLower() == ProduitASuppr.NomProduit.ToLower()).FirstOrDefault().IdProduit;
            ProduitsController controller = new ProduitsController(dataRepository);

            // Act
            var result = controller.DeleteProduit(ProduitASuppr.IdProduit).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
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
        public void PutProduit_ModelValidated_UpdateOK_AvecMoq()
        {
            // Arrange
            Produit userAMaJ = new Produit
            {
                NomProduit = "SVG",
            };
            Produit userUpdated = new Produit
            {
                NomProduit = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(userAMaJ);
            var userController = new ProduitsController(mockRepository.Object);

            // Act
            var actionResult = userController.PutProduit(userUpdated.IdProduit, userUpdated).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        #endregion
    }
}