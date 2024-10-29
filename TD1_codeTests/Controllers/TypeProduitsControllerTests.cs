using Microsoft.VisualStudio.TestTools.UnitTesting;
using TD1_code.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TD1_code.Models.DataManager;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;
using TD1_code.Models.AutoMapper;
using AutoMapper;
using TD1_code.Models.DTO;

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
        private IDataDtoTypeProduit dataDPO;
        private IMapper _mapper;
        #endregion

        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<DBContexte>()
               .UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=Ricardo2003@");
            context = new DBContexte(builder.Options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProduit());
                cfg.AddProfile(new MapperMarque());
                cfg.AddProfile(new MapperTypeProduit());
            });

            _mapper = config.CreateMapper();
            dataRepository = new TypeProduitManager(context, _mapper);

            // Instanciation correcte de dataDPO
            dataDPO = new TypeProduitManager(context, _mapper);

            controller = new TypeProduitsController(dataRepository, dataDPO);
        }

        #region Test unitaires

        [TestMethod]
        public async Task GetTypeProduits_ReturnsRightItems()
        {
            // Act
            var result = await controller.GettypeProduits();  // Attendre la réponse

            List<TypeProduit> expected = context.TypeProduits.ToList();  // TypeProduits attendus dans la base

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<TypeProduit>>), "Pas un ActionResult");
            var actionResult = result as ActionResult<IEnumerable<TypeProduit>>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            CollectionAssert.AreEqual(expected, (List<TypeProduit>)actionResult.Value, "Pas les mêmes TypeProduits");
        }


        [TestMethod]
        public async Task GetTypeProduitById_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var result = await controller.GettypeProduitById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<TypeProduit>), "Pas un ActionResult");
            var actionResult = result as ActionResult<TypeProduit>;
            Assert.IsNotNull(actionResult, "ActionResult null");
            Assert.IsNotNull(actionResult.Value, "Valeur nulle");
            Assert.IsInstanceOfType(actionResult.Value, typeof(TypeProduit), "Pas un TypeProduit");
            Assert.AreEqual(context.TypeProduits.Where(c => c.IdTypeProduit == 1).FirstOrDefault(),
                (TypeProduit)actionResult.Value, "TypeProduits pas identiques");
        }

        [TestMethod]
        public async Task GetTypeProduitById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var result = await controller.GettypeProduitById(0);  // Attendre la réponse

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<TypeProduit>), "Pas un ActionResult");
            Assert.IsNull(result.Value, "TypeProduit pas null");
        }

        [TestMethod]
        public async Task PostTypeProduit_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);

            TypeProduit TypeProduitAtester = new TypeProduit()
            {
                IdTypeProduit = id,
                NomTypeProduit = "test"
            };

            // Act
            var postResult = await controller.PostTypeProduit(TypeProduitAtester);

            // Sauvegarder les changements dans le contexte pour s'assurer que l'ajout est persistant
            await context.SaveChangesAsync();

            // Assert
            // Vérifier si le TypeProduit a été correctement ajouté à la base de données
            TypeProduit? TypeProduitRecupere = context.TypeProduits.Where(u => u.IdTypeProduit == TypeProduitAtester.IdTypeProduit).FirstOrDefault();
            Assert.IsNotNull(TypeProduitRecupere, "Le TypeProduit n'a pas été trouvé dans la base de données.");

            // Vérifier que les TypeProduits sont identiques
            Assert.AreEqual(TypeProduitAtester, TypeProduitRecupere, "Les TypeProduits ne correspondent pas.");
            // Ajouter d'autres comparaisons si nécessaire pour les autres propriétés du TypeProduit.
            await controller.DeleteTypeProduit(id);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public async Task PostTypeProduit_CreationFailed()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);

            // Créer un TypeProduit invalide (par exemple, sans nom)
            TypeProduit TypeProduitAtester = new TypeProduit()
            {
                IdTypeProduit = id,
                NomTypeProduit = null,  // Cela devrait provoquer une exception car le nom est requis
            };

            // Act
            // Cette ligne devrait lancer une exception, car le TypeProduit n'est pas valide
            await controller.PostTypeProduit(TypeProduitAtester);
        }


        // IDEM POUR LE PUT.
        [TestMethod]
        public async Task PutTypeProduit_ModelValidated_UpdateOK()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);

            // Création d'un TypeProduit initial
            TypeProduit TypeProduitInitial = new TypeProduit()
            {
                IdTypeProduit = id,
                NomTypeProduit = "TypeProduitInitial"
            };

            // Ajouter le TypeProduit initial dans la base de données
            await controller.PostTypeProduit(TypeProduitInitial);
            await context.SaveChangesAsync(); // Sauvegarder les changements

            // Création d'un TypeProduit mis à jour avec les mêmes Id mais avec d'autres valeurs
            TypeProduit TypeProduitUpdated = new TypeProduit()
            {
                IdTypeProduit = id,  // Utilisation du même Id
                NomTypeProduit = "TypeProduitMisAJour"

            };

            // Act
            var result = await controller.PutTypeProduit(TypeProduitUpdated.IdTypeProduit, TypeProduitUpdated); // Mettre à jour le TypeProduit
            await context.SaveChangesAsync(); // Sauvegarder les changements après la mise à jour

            // Assert
            TypeProduit? TypeProduitRecupere = context.TypeProduits.Where(c => c.IdTypeProduit == TypeProduitUpdated.IdTypeProduit).FirstOrDefault();
            Assert.IsNotNull(TypeProduitRecupere, "Le TypeProduit mis à jour n'a pas été trouvé dans la base de données.");

            // Vérifier que les valeurs mises à jour correspondent
            Assert.AreEqual(TypeProduitUpdated.NomTypeProduit, TypeProduitRecupere.NomTypeProduit, "Le nom du TypeProduit n'a pas été mis à jour.");

            await controller.DeleteTypeProduit(id);
        }


        // Pareil pour les autres tests PUT

        [TestMethod]
        public async Task DeleteTypeProduitTest()
        {
            // Arrange
            Random rnd = new Random();
            int id = rnd.Next(200, 1000);
            TypeProduit TypeProduitASuppr = new TypeProduit()
            {
                IdTypeProduit = id,
                NomTypeProduit = "test"

            };

            context.TypeProduits.Add(TypeProduitASuppr);
            await context.SaveChangesAsync();
            TypeProduitASuppr.IdTypeProduit = context.TypeProduits.Where(c => c.IdTypeProduit == TypeProduitASuppr.IdTypeProduit).FirstOrDefault().IdTypeProduit;

            // Act
            var result = await controller.DeleteTypeProduit(TypeProduitASuppr.IdTypeProduit);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
            Assert.IsNull(context.TypeProduits.Where(c => c.IdTypeProduit == TypeProduitASuppr.IdTypeProduit).FirstOrDefault());
        }

        #endregion


        #region Tests de substitution

        [TestMethod]
        public void GetTypeProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            TypeProduit TypeProduit = new TypeProduit
            {
                NomTypeProduit = "SVG",
            };
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(TypeProduit);

            var TypeProduitController = new TypeProduitsController(dataRepository, dataDPO);

            // Act
            var actionResult = TypeProduitController.GettypeProduitById(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(TypeProduit, actionResult.Value as TypeProduit);
        }

        [TestMethod]
        public void GetTypeProduitById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            var TypeProduitController = new TypeProduitsController(dataRepository, dataDPO);

            // Act
            var actionResult = TypeProduitController.GettypeProduitById(0).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostTypeProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            var userController = new TypeProduitsController(dataRepository, dataDPO);

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
            var userController = new TypeProduitsController(dataRepository, dataDPO);

            // Act
            var actionResult = userController.DeleteTypeProduit(1).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        [TestMethod]
        public async Task PutTypeProduit_ModelValidated_UpdateOK_AvecMoq()
        {
            // Arrange
            TypeProduit userAMaJ = new TypeProduit
            {
                IdTypeProduit = 1,   // Initialiser l'ID du TypeProduit à mettre à jour
                NomTypeProduit = "SVG",

            };

            TypeProduit userUpdated = new TypeProduit
            {
                IdTypeProduit = 1,   // Assurez-vous que l'ID correspond à celui que vous souhaitez mettre à jour
                NomTypeProduit = "SV",

            };

            // Simuler le repository avec Moq
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();

            // Simuler la méthode GetByIdAsync pour retourner le TypeProduit à mettre à jour
            mockRepository.Setup(x => x.GetByIdAsync(userAMaJ.IdTypeProduit)).ReturnsAsync(userAMaJ);

            // Simuler la méthode UpdateAsync avec deux paramètres
            mockRepository.Setup(x => x.UpdateAsync(userAMaJ, userUpdated)).Returns(Task.CompletedTask);

            // Créer le contrôleur avec le mock du repository
            var userController = new TypeProduitsController(dataRepository, dataDPO);

            // Act
            var actionResult = await userController.PutTypeProduit(userUpdated.IdTypeProduit, userUpdated);

            // Assert
            // Vérifier que la méthode UpdateAsync a bien été appelée avec les bons paramètres
            mockRepository.Verify(x => x.UpdateAsync(userAMaJ, userUpdated), Times.Once);

            // Vérifier que le retour est bien un NoContentResult, ce qui signifie que la mise à jour a réussi
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }



        #endregion
    }
}