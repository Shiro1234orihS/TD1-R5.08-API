using Microsoft.VisualStudio.TestTools.UnitTesting;
using TD1_code.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD1_code.Respository;
using TD1_code.Models;
using TD1_code.Models.EntityFramework;
using TD1_code.Models.DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TD1_code.Controllers.Tests
{
    [TestClass()]
    public class MarquesControllerTests
    {
        #region Private Fields
        // Déclaration des variables nécessaires pour les tests
        private MarquesController controller;
        private DBContexte context;
        private IDataRepository<Marque> dataRepository;
        #endregion

        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<DBContexte>().UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=postgres");
            DBContexte dbContext = new DBContexte(builder.Options);
            dataRepository = new MarqueManager(context);
            // Création du gestionnaire de données et du contrôleur à tester
            MarquesController controller = new MarquesController(dataRepository);
        }

        #region Test unitaires

        [TestMethod]
        public void GetMarques_ReturnsRightItems()
        {
            // Arrange
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.GetMarques();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<IEnumerable<Marque>>), "Pas un ActionResult");
            ActionResult<IEnumerable<Marque>> actionResult = result.Result as ActionResult<IEnumerable<Marque>>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            CollectionAssert.AreEqual(context.Marques.ToList(), (List<Marque>)actionResult.Value, "Pas les mêmes Marques");
        }

        [TestMethod]
        public void GetMarqueById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.GetMarqueById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Marque>), "Pas un ActionResult");

            var actionResult = result.Result as ActionResult<Marque>;

            // Assert
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            Assert.IsInstanceOfType(actionResult.Value, typeof(Marque), "Pas un Marque");
            Assert.AreEqual(context.Marques.Where(c => c.IdMarque == 1).FirstOrDefault(),
                (Marque)actionResult.Value, "Marques pas identiques");
        }

        [TestMethod]
        public void GetMarqueById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.GetMarqueById(0);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Marque>), "Pas un ActionResult");
            Assert.IsNull(result.Result.Value, "Marque pas null");
        }

        //[TestMethod]
        //public void GetMarqueByEmail_ExistingEmailPassed_ReturnsRightItem()
        //{
        //    // Arrange
        //    MarquesController controller = new MarquesController(dataRepository);

        //    // Act
        //    var result = controller.Getb("gdominguez0@washingtonpost.com");

        //    // Assert
        //    Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Marque>), "Pas un ActionResult");

        //    var actionResult = result.Result as ActionResult<Marque>;

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult.Value, typeof(Marque), "Pas un Marque");
        //    Assert.IsNotNull(actionResult.Value, "Valeur nulle");
        //    Assert.AreEqual(context.Marques.Where(c => c.Mail.ToLower() == "gdominguez0@washingtonpost.com").FirstOrDefault(),
        //        (Marque)actionResult.Value, "Marques pas identiques");
        //}

        //[TestMethod]
        //public void GetMarqueByEmail_UnknownEmailPassed_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    MarquesController controller = new MarquesController(dataRepository);

        //    // Act
        //    var result = controller.GetMarqueByEmail("123@free.fr");

        //    // Assert
        //    Assert.IsInstanceOfType(result.Result, typeof(ActionResult<Marque>), "Pas un ActionResult");
        //    Assert.IsNull(result.Result.Value, "Marque pas null");
        //}

        [TestMethod]
        public void PostMarque_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le Marque après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE du WS
            Marque MarqueAtester = new Marque()
            {
                NomMarque = "SVG",
            };
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.PostMarque(MarqueAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre la création

            // Assert
            Marque? MarqueRecupere = context.Marques.Where(u => u.NomMarque.ToUpper() == MarqueAtester.NomMarque.ToUpper()).FirstOrDefault(); //On récupère l'Marque créé directement dans la BD grace à son mail unique
                                                                                                                                                                            // On ne connait pas l'ID de l’Marque envoyé car numéro automatique.
                                                                                                                                                                            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 Marques
            MarqueAtester.IdMarque = MarqueRecupere.IdMarque;
            Assert.AreEqual(MarqueAtester, MarqueRecupere, "Marques pas identiques");
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostCompte_MailMissing_CreationFailed()
        {
            // Arrange
            Marque MarqueAtester = new Marque()
            {
                NomMarque = "SVG",
            };
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.PostMarque(MarqueAtester).Result;
        }

        //IDEM POUR LES AUTRES CHAMPS NOT NULL (Pwd)

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostCompte_EmailNotUnique_CreationFailed()
        {
            // Arrange
            Marque? Marque = context.Marques.Where(c => c.IdMarque == 1).FirstOrDefault();
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            // On ajoute un Marque existant
            var result = controller.PostMarque(Marque).Result;
        }




        //IDEM POUR LES AUTRES CHAMPS NON CONFORMES AU MODELE


        // IDEM POUR LE PUT.
        [TestMethod]
        public void PutMarque_ModelValidated_UpdateOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Marque Marque = new Marque
            {
                NomMarque = "SVG",
            };
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.PutMarque(Marque.IdMarque, Marque).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre la création

            // Assert
            Marque? MarqueRecupere = context.Marques.Where(u => u.NomMarque.ToUpper() == Marque.NomMarque.ToUpper()).FirstOrDefault(); //On récupère l'Marque créé directement dans la BD grace à son mail unique
                                                                                                                                                                     // On ne connait pas l'ID de l’Marque envoyé car numéro automatique.
                                                                                                                                                                     // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 Marques
            Assert.AreEqual(Marque, MarqueRecupere, "Marques pas identiques");
        }

        // Pareil pour les autres tests PUT


        [TestMethod()]
        public void DeleteMarqueTest()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Marque MarqueASuppr = new Marque()
            {
                NomMarque = "SVG",
            };
            context.Marques.Add(MarqueASuppr);
            context.SaveChanges();
            MarqueASuppr.IdMarque = context.Marques.Where(c => c.NomMarque.ToLower() == MarqueASuppr.NomMarque.ToLower()).FirstOrDefault().IdMarque;
            MarquesController controller = new MarquesController(dataRepository);

            // Act
            var result = controller.DeleteMarque(MarqueASuppr.IdMarque).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
            Assert.IsNull(context.Marques.Where(c => c.IdMarque == MarqueASuppr.IdMarque).FirstOrDefault());
        }

        #endregion


        #region Tests de substitution

        [TestMethod]
        public void GetMarqueById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Marque Marque = new Marque
            {
                NomMarque = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(Marque);

            var MarqueController = new MarquesController(mockRepository.Object);

            // Act
            var actionResult = MarqueController.GetMarqueById(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(Marque, actionResult.Value as Marque);
        }

        [TestMethod]
        public void GetMarqueById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var MarqueController = new MarquesController(mockRepository.Object);

            // Act
            var actionResult = MarqueController.GetMarqueById(0).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostMarque_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var userController = new MarquesController(mockRepository.Object);

            Marque user = new Marque
            {
                NomMarque = "SVG"
            };

            // Act
            var actionResult = userController.PostMarque(user).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Marque>), "Pas un ActionResult<Marque>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Marque), "Pas un Marque");
            user.IdMarque = ((Marque)result.Value).IdMarque;
            Assert.AreEqual(user, (Marque)result.Value, "Marques pas identiques");
        }

        [TestMethod]
        public void DeleteMarqueTest_AvecMoq()
        {
            // Arrange
            Marque user = new Marque
            {
                NomMarque = "SVG",
            };

            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new MarquesController(mockRepository.Object);

            // Act
            var actionResult = userController.DeleteMarque(1).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        [TestMethod]
        public void PutMarque_ModelValidated_UpdateOK_AvecMoq()
        {
            // Arrange
            Marque userAMaJ = new Marque
            {
                NomMarque = "SVG",
            };
            Marque userUpdated = new Marque
            {
                NomMarque = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(userAMaJ);
            var userController = new MarquesController(mockRepository.Object);

            // Act
            var actionResult = userController.PutMarque(userUpdated.IdMarque, userUpdated).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        #endregion
    }
}


