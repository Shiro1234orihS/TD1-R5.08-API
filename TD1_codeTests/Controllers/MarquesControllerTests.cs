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

        #region MarquesControllerTest
        [TestMethod()]
        public void MarquesControllerTest()
        {
            // Arrange : préparation des données attendues
            var builder = new DbContextOptionsBuilder<DbContext>().UseNpgsql("Server = 51.83.36.122; port = 5432; Database = sa25; uid = sa25; password = 1G1Nxb; SearchPath = bmw");
            context = new DBContexte(builder.Options);
            dataRepository = new MarqueManager(context);

            // Act : appel de la méthode à tester
            var option = new MarquesController(dataRepository);

            // Assert : vérification que les données obtenues correspondent aux données attendues : vérification que les données obtenues correspondent aux données attendues
            Assert.IsNotNull(option, "L'instance de MaClasse ne devrait pas être null.");
        }
        #endregion

        /// <summary>
        /// Test GetMarquesTest 
        /// </summary>
        [TestMethod()]
        public void GetMarquesTest()
        {
            // Arrange
            List<Marque> expected = context.Marques.ToList();
            // Act
            var res = controller.GetMarques().Result;
            // Assert
            CollectionAssert.AreEqual(expected, res.Value.ToList(), "Les listes ne sont pas identiques");
        }
        /// <summary>
        /// Test GetMarqueById 
        /// </summary>

        [TestMethod()]
        public void GetMarqueByIdTest()
        {
            // Arrange
            Marque expected = context.Marques.Find(1);
            // Act
            var res = controller.GetMarqueById(1).Result;
            // Assert
            Assert.AreEqual(expected, res.Value);
        }

        [TestMethod]
        public void GetMarqueByIdTest_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Marque>>();

            ICollection<Marque> AcquisC = new List<Marque>
            {
                new Marque { /* initialisez les propriétés de l'objet ici */ },
                new Marque { /* un autre objet Acquerir */ }
            };


            Marque catre = new Marque
            {
               IdMarque = 1,
               NomMarque = "testMoq"
            };
            // Act
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(catre);
            var userController = new MarquesController(mockRepository.Object);

            var actionResult = userController.GetMarqueById(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(catre, actionResult.Value as Marque);
        }



        /// <summary>
        /// Test PutMarqueTest 
        /// </summary>
        [TestMethod()]
        public void PutMarqueTest()
        {
            // Arrange
            Marque userAtester = context.CartesBancaires.Find(1);

            // Act
            var res = controller.PutMarque(1, userAtester);

            // Arrange
            Marque nouvellecarte = context.CartesBancaires.Find(1);
            Assert.AreEqual(userAtester, nouvellecarte);
        }

        [TestMethod]
        public void PutMarqueTestAvecMoq()
        {

            // Arrange
            Marque carteToUpdate = new Marque
            {
                IdCb = 2000,
                NomCarte = "NUNES EMILIO Ricardo ",
                NumeroCb = "12345678901234",
                MoisExpiration = 02,
                AnneeExpiration = 25,
                CryptoCb = "093",

            };
            Marque updatedCarte = new Marque
            {
                IdCb = 21000,
                NomCarte = "NUNES  ",
                NumeroCb = "1234567890123fzeoizjf",
                MoisExpiration = 03,
                AnneeExpiration = 25,
                CryptoCb = "093",

            };
            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(21000)).ReturnsAsync(carteToUpdate);
            mockRepository.Setup(repo => repo.UpdateAsync(carteToUpdate, updatedCarte)).Returns(Task.CompletedTask);


            var controller = new MarqueController(mockRepository.Object);

            // Act
            var result = controller.PutMarque(21000, updatedCarte).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "La réponse n'est pas du type attendu NoContentResult");
        }

        /// <summary>
        /// Test PostUtilisateur 
        /// </summary>
        /// 
        [TestMethod()]
        public void PostMarqueTest()
        {
            //// Arrange

            //Marque carte = new Marque
            //{
            //    IdCb = 100,
            //    NomCarte = "NUNES EMILIO Ricardo ",
            //    NumeroCb = "12345678901234",
            //    MoisExpiration = 02,
            //    AnneeExpiration = 25,
            //    CryptoCb = "093",

            //};

            //// Act
            //var result = controller.PostMarque(carte).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            //// Assert
            //// On récupère l'utilisateur créé directement dans la BD grace à son mail unique
            //Marque? carteRecupere = context.CartesBancaires
            //    .Where(u => u.IdCb == carte.IdCb)
            //    .FirstOrDefault();

            //// On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            //// Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            //carte.IdCb = carteRecupere.IdCb;
            //Assert.AreEqual(carteRecupere, carte, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void PostMarqueTest_Mok()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var userController = new MarqueController(mockRepository.Object);



            // Arrange
            Marque catre = new Marque
            {
                IdCb = 1,
                NomCarte = "NUNES EMILIO Ricardo ",
                NumeroCb = "12345678901234",
                MoisExpiration = 02,
                AnneeExpiration = 25,
                CryptoCb = "093",

            };
            // Act
            var actionResult = userController.PostMarque(catre).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Marque>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Marque), "Pas un Utilisateur");
            catre.IdCb = ((Marque)result.Value).IdCb;
            Assert.AreEqual(catre, (Marque)result.Value, "Utilisateurs pas identiques");
        }



        /// <summary>
        /// Test Delete 
        /// </summary>

        [TestMethod()]
        public void DeleteMarqueTest()
        {
            // Arrange
            Marque catre = new Marque
            {
                IdCb = 200,
                NomCarte = "NUNES EMILIO Ricardo ",
                NumeroCb = "12345678901234",
                MoisExpiration = 02,
                AnneeExpiration = 25,
                CryptoCb = "093",

            };
            context.CartesBancaires.Add(catre);
            context.SaveChanges();

            // Act
            Marque deletedCarte = context.CartesBancaires.FirstOrDefault(u => u.IdCb == catre.IdCb);
            _ = controller.DeleteMarque(deletedCarte.IdCb).Result;

            // Arrange
            Marque res = context.CartesBancaires.FirstOrDefault(u => u.IdCb == catre.IdCb);
            Assert.IsNull(res, "utilisateur non supprimé");
        }


        [TestMethod]
        public void DeleteMarqueTest_AvecMoq()
        {
            ICollection<Acquerir> AcquisC = new List<Acquerir>
            {
                new Acquerir { /* initialisez les propriétés de l'objet ici */ },
                new Acquerir { /* un autre objet Acquerir */ }
            };

            // Arrange
            Marque catre = new Marque
            {
                IdCb = 1,
                NomCarte = "NUNES EMILIO Ricardo ",
                NumeroCb = "12345678901234",
                MoisExpiration = 02,
                AnneeExpiration = 25,
                CryptoCb = "093",
                AcquisCB = AcquisC
            };
            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(2).Result).Returns(catre);
            var userController = new MarqueController(mockRepository.Object);
            // Act
            var actionResult = userController.DeleteMarque(2).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}


