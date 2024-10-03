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
using System.Text.RegularExpressions;

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
            var builder = new DbContextOptionsBuilder<DBContexte>().UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=Ricardo2003@");
            DBContexte dbContext = new DBContexte(builder.Options);
            dataRepository = new TypeProduitManager(context);
            // Création du gestionnaire de données et du contrôleur à tester
            TypeProduitsController controller = new TypeProduitsController(dataRepository);
        }

        #region Test unitaires

        [TestMethod]
        public void GetTypeProduits_ReturnsRightItems()
        {
            // Arrange
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.GettypeProduits();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<IEnumerable<TypeProduit>>), "Pas un ActionResult");
            ActionResult<IEnumerable<TypeProduit>> actionResult = result.Result as ActionResult<IEnumerable<TypeProduit>>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            CollectionAssert.AreEqual(context.TypeProduits.ToList(), (List<TypeProduit>)actionResult.Value, "Pas les mêmes TypeProduits");
        }

        [TestMethod]
        public void GetTypeProduitById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.GettypeProduitById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<TypeProduit>), "Pas un ActionResult");

            var actionResult = result.Result as ActionResult<TypeProduit>;

            // Assert
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            Assert.IsInstanceOfType(actionResult.Value, typeof(TypeProduit), "Pas un TypeProduit");
            Assert.AreEqual(context.TypeProduits.Where(c => c.IdTypeProduit == 1).FirstOrDefault(),
                (TypeProduit)actionResult.Value, "TypeProduits pas identiques");
        }

        [TestMethod]
        public void GetTypeProduitById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.GettypeProduitById(0);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<TypeProduit>), "Pas un ActionResult");
            Assert.IsNull(result.Result.Value, "TypeProduit pas null");
        }

        //[TestMethod]
        //public void GetTypeProduitByEmail_ExistingEmailPassed_ReturnsRightItem()
        //{
        //    // Arrange
        //    TypeProduitsController controller = new TypeProduitsController(dataRepository);

        //    // Act
        //    var result = controller.Getb("gdominguez0@washingtonpost.com");

        //    // Assert
        //    Assert.IsInstanceOfType(result.Result, typeof(ActionResult<TypeProduit>), "Pas un ActionResult");

        //    var actionResult = result.Result as ActionResult<TypeProduit>;

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult.Value, typeof(TypeProduit), "Pas un TypeProduit");
        //    Assert.IsNotNull(actionResult.Value, "Valeur nulle");
        //    Assert.AreEqual(context.TypeProduits.Where(c => c.Mail.ToLower() == "gdominguez0@washingtonpost.com").FirstOrDefault(),
        //        (TypeProduit)actionResult.Value, "TypeProduits pas identiques");
        //}

        //[TestMethod]
        //public void GetTypeProduitByEmail_UnknownEmailPassed_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    TypeProduitsController controller = new TypeProduitsController(dataRepository);

        //    // Act
        //    var result = controller.GetTypeProduitByEmail("123@free.fr");

        //    // Assert
        //    Assert.IsInstanceOfType(result.Result, typeof(ActionResult<TypeProduit>), "Pas un ActionResult");
        //    Assert.IsNull(result.Result.Value, "TypeProduit pas null");
        //}

        [TestMethod]
        public void PostTypeProduit_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le typeproduit après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE du WS
            TypeProduit typeproduitAtester = new TypeProduit()
            {
                NomTypeProduit = "SVG",
            };
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.PostTypeProduit(typeproduitAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre la création

            // Assert
            TypeProduit? typeproduitRecupere = context.TypeProduits.Where(u => u.NomTypeProduit.ToUpper() == typeproduitAtester.NomTypeProduit.ToUpper()).FirstOrDefault(); //On récupère l'TypeProduit créé directement dans la BD grace à son mail unique
            // On ne connait pas l'ID de l’TypeProduit envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 typeproduits
            typeproduitAtester.IdTypeProduit = typeproduitRecupere.IdTypeProduit;
            Assert.AreEqual(typeproduitAtester, typeproduitRecupere, "TypeProduits pas identiques");
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostCompte_MailMissing_CreationFailed()
        {
            // Arrange
            TypeProduit typeproduitAtester = new TypeProduit()
            {
                NomTypeProduit = "SVG",
            };
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.PostTypeProduit(typeproduitAtester).Result;
        }

        //IDEM POUR LES AUTRES CHAMPS NOT NULL (Pwd)

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostCompte_EmailNotUnique_CreationFailed()
        {
            // Arrange
            TypeProduit? typeproduit = context.TypeProduits.Where(c => c.IdTypeProduit == 1).FirstOrDefault();
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            // On ajoute un TypeProduit existant
            var result = controller.PostTypeProduit(typeproduit).Result;
        }

    


        //IDEM POUR LES AUTRES CHAMPS NON CONFORMES AU MODELE


        // IDEM POUR LE PUT.
        [TestMethod]
        public void PutTypeProduit_ModelValidated_UpdateOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            TypeProduit typeproduit = new TypeProduit
            {
                NomTypeProduit = "SVG",
            };
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.PutTypeProduit(typeproduit.IdTypeProduit, typeproduit).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre la création

            // Assert
            TypeProduit? typeproduitRecupere = context.TypeProduits.Where(u => u.NomTypeProduit.ToUpper() == typeproduit.NomTypeProduit.ToUpper()).FirstOrDefault(); //On récupère l'TypeProduit créé directement dans la BD grace à son mail unique
            // On ne connait pas l'ID de l’TypeProduit envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 typeproduits
            Assert.AreEqual(typeproduit, typeproduitRecupere, "TypeProduits pas identiques");
        }

        // Pareil pour les autres tests PUT


        [TestMethod()]
        public void DeleteTypeProduitTest()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            TypeProduit typeproduitASuppr = new TypeProduit()
            {
                NomTypeProduit = "SVG",
            };
            context.TypeProduits.Add(typeproduitASuppr);
            context.SaveChanges();
            typeproduitASuppr.IdTypeProduit = context.TypeProduits.Where(c => c.NomTypeProduit.ToLower() == typeproduitASuppr.NomTypeProduit.ToLower()).FirstOrDefault().IdTypeProduit;
            TypeProduitsController controller = new TypeProduitsController(dataRepository);

            // Act
            var result = controller.DeleteTypeProduit(typeproduitASuppr.IdTypeProduit).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
            Assert.IsNull(context.TypeProduits.Where(c => c.IdTypeProduit == typeproduitASuppr.IdTypeProduit).FirstOrDefault());
        }

        #endregion


        #region Tests de substitution

        [TestMethod]
        public void GetTypeProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            TypeProduit typeproduit = new TypeProduit
            {
                NomTypeProduit = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(typeproduit);

            var typeproduitController = new TypeProduitsController(mockRepository.Object);

            // Act
            var actionResult = typeproduitController.GettypeProduitById(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(typeproduit, actionResult.Value as TypeProduit);
        }

        [TestMethod]
        public void GetTypeProduitById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            var typeproduitController = new TypeProduitsController(mockRepository.Object);

            // Act
            var actionResult = typeproduitController.GettypeProduitById(0).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostTypeProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            var userController = new TypeProduitsController(mockRepository.Object);

            TypeProduit user = new TypeProduit
            {
                NomTypeProduit = "SVG"
            };

            // Act
            var actionResult = userController.PostTypeProduit(user).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeProduit>), "Pas un ActionResult<TypeProduit>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(TypeProduit), "Pas un TypeProduit");
            user.IdTypeProduit = ((TypeProduit)result.Value).IdTypeProduit;
            Assert.AreEqual(user, (TypeProduit)result.Value, "TypeProduits pas identiques");
        }

        [TestMethod]
        public void DeleteTypeProduitTest_AvecMoq()
        {
            // Arrange
            TypeProduit user = new TypeProduit
            {
                NomTypeProduit = "SVG",
            };

            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new TypeProduitsController(mockRepository.Object);

            // Act
            var actionResult = userController.DeleteTypeProduit(1).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        [TestMethod]
        public void PutTypeProduit_ModelValidated_UpdateOK_AvecMoq()
        {
            // Arrange
            TypeProduit userAMaJ = new TypeProduit
            {
                NomTypeProduit = "SVG",
            };
            TypeProduit userUpdated = new TypeProduit
            {
                NomTypeProduit = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(userAMaJ);
            var userController = new TypeProduitsController(mockRepository.Object);

            // Act
            var actionResult = userController.PutTypeProduit(userUpdated.IdTypeProduit, userUpdated).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        #endregion
    }
}