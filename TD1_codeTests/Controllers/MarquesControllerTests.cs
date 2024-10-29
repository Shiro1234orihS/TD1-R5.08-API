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
using AutoMapper;

namespace TD1_code.Controllers.Tests
{
    //bug avec les test  unitaires, je suis entrain de les faire 
    [TestClass()]
    public class MarquesControllerTests
    {
        //#region Private Fields
        //// Déclaration des variables nécessaires pour les tests
        //private MarquesController controller;
        //private DBContexte context;
        //private IDataRepository<Marque> dataRepository;
        //private IDataDtoProduit dataDtoProduit;
        //private IMapper mapper;
        //#endregion

        //[TestInitialize]
        //public void Init()
        //{
        //    // Utilisation d'une base de données en mémoire pour simplifier les tests unitaires
        //    var builder = new DbContextOptionsBuilder<DBContexte>()
        //        .UseInMemoryDatabase("TD1_cod");  // Utilisation d'une base de données en mémoire pour les tests

        //    context = new DBContexte(builder.Options);

        //    // Configuration de l'objet IMapper avec AutoMapper
        //    var mappingConfig = new MapperConfiguration(mc =>
        //    {
        //        mc.AddProfile(new MappingProfile()); // Ajoutez ici votre profil de mapping
        //    });
        //    mapper = mappingConfig.CreateMapper();

        //    // Initialisation des dépendances
        //    dataRepository = new MarqueManager(context, mapper);  // Initialiser MarqueManager avec le contexte
        //    dataDtoProduit = new ProduitManager(context, mapper); // Assurez-vous que ProduitManager est bien la classe correcte

        //    // Initialisation du contrôleur avec les dépendances
        //    controller = new MarquesController(dataRepository, dataDtoProduit);
        //}

        //#region Test unitaires

        //[TestMethod]
        //public async Task GetMarques_ReturnsRightItems()
        //{
        //    // Act
        //    var result = await controller.GetMarques();  // Attendre la réponse

        //    List<Marque> expected = context.Marques.ToList();  // Marques attendus dans la base

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Marque>>), "Pas un ActionResult");
        //    var actionResult = result as ActionResult<IEnumerable<Marque>>;
        //    Assert.IsNotNull(actionResult, "ActionResult null");
        //    Assert.IsNotNull(actionResult.Value, "Valeur nulle");
        //    CollectionAssert.AreEqual(expected, (List<Marque>)actionResult.Value, "Pas les mêmes Marques");
        //}


        //[TestMethod]
        //public async Task GetMarqueById_ExistingIdPassed_ReturnsRightItem()
        //{
        //    // Act
        //    var result = await controller.GetMarqueById(1);

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(ActionResult<Marque>), "Pas un ActionResult");
        //    var actionResult = result as ActionResult<Marque>;
        //    Assert.IsNotNull(actionResult, "ActionResult null");
        //    Assert.IsNotNull(actionResult.Value, "Valeur nulle");
        //    Assert.IsInstanceOfType(actionResult.Value, typeof(Marque), "Pas un Marque");
        //    Assert.AreEqual(context.Marques.Where(c => c.IdMarque == 1).FirstOrDefault(),
        //        (Marque)actionResult.Value, "Marques pas identiques");
        //}

        //[TestMethod]
        //public async Task GetMarqueById_UnknownIdPassed_ReturnsNotFoundResult()
        //{
        //    // Act
        //    var result = await controller.GetMarqueById(0);  // Attendre la réponse

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(ActionResult<Marque>), "Pas un ActionResult");
        //    Assert.IsNull(result.Value, "Marque pas null");
        //}

        //[TestMethod]
        //public async Task PostMarque_ModelValidated_CreationOK()
        //{
        //    // Arrange
        //    Random rnd = new Random();
        //    int id = rnd.Next(200, 1000);

        //    Marque MarqueAtester = new Marque()
        //    {
        //        IdMarque = id,
        //        NomMarque = "test"
        //    };

        //    // Act
        //    var postResult = await controller.PostMarque(MarqueAtester);

        //    // Sauvegarder les changements dans le contexte pour s'assurer que l'ajout est persistant
        //    await context.SaveChangesAsync();

        //    // Assert
        //    // Vérifier si le Marque a été correctement ajouté à la base de données
        //    Marque? MarqueRecupere = context.Marques.Where(u => u.IdMarque == MarqueAtester.IdMarque).FirstOrDefault();
        //    Assert.IsNotNull(MarqueRecupere, "Le Marque n'a pas été trouvé dans la base de données.");

        //    // Vérifier que les Marques sont identiques
        //    Assert.AreEqual(MarqueAtester, MarqueRecupere, "Les Marques ne correspondent pas.");
        //    // Ajouter d'autres comparaisons si nécessaire pour les autres propriétés du Marque.
        //    await controller.DeleteMarque(id);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        //public async Task PostMarque_CreationFailed()
        //{
        //    // Arrange
        //    Random rnd = new Random();
        //    int id = rnd.Next(200, 1000);

        //    // Créer un Marque invalide (par exemple, sans nom)
        //    Marque MarqueAtester = new Marque()
        //    {
        //        IdMarque = id,
        //        NomMarque = null,  // Cela devrait provoquer une exception car le nom est requis
        //    };

        //    // Act
        //    // Cette ligne devrait lancer une exception, car le Marque n'est pas valide
        //    await controller.PostMarque(MarqueAtester);
        //}


        //// IDEM POUR LE PUT.
        //[TestMethod]
        //public async Task PutMarque_ModelValidated_UpdateOK()
        //{
        //    // Arrange
        //    Random rnd = new Random();
        //    int id = rnd.Next(200, 1000);

        //    // Création d'un Marque initial
        //    Marque MarqueInitial = new Marque()
        //    {
        //        IdMarque = id,
        //        NomMarque = "MarqueInitial"
        //    };

        //    // Ajouter le Marque initial dans la base de données
        //    await controller.PostMarque(MarqueInitial);
        //    await context.SaveChangesAsync(); // Sauvegarder les changements

        //    // Création d'un Marque mis à jour avec les mêmes Id mais avec d'autres valeurs
        //    Marque MarqueUpdated = new Marque()
        //    {
        //        IdMarque = id,  // Utilisation du même Id
        //        NomMarque = "MarqueMisAJour"
              
        //    };

        //    // Act
        //    var result = await controller.PutMarque(MarqueUpdated.IdMarque, MarqueUpdated); // Mettre à jour le Marque
        //    await context.SaveChangesAsync(); // Sauvegarder les changements après la mise à jour

        //    // Assert
        //    Marque? MarqueRecupere = context.Marques.Where(c => c.IdMarque == MarqueUpdated.IdMarque).FirstOrDefault();
        //    Assert.IsNotNull(MarqueRecupere, "Le Marque mis à jour n'a pas été trouvé dans la base de données.");

        //    // Vérifier que les valeurs mises à jour correspondent
        //    Assert.AreEqual(MarqueUpdated.NomMarque, MarqueRecupere.NomMarque, "Le nom du Marque n'a pas été mis à jour.");
        //    await controller.DeleteMarque(id);
        //}


        //// Pareil pour les autres tests PUT

        //[TestMethod]
        //public async Task DeleteMarqueTest()
        //{
        //    // Arrange
        //    Random rnd = new Random();
        //    int id = rnd.Next(200, 1000);
        //    Marque MarqueASuppr = new Marque()
        //    {
        //        IdMarque = id,
        //        NomMarque = "test"
               
        //    };

        //    context.Marques.Add(MarqueASuppr);
        //    await context.SaveChangesAsync();
        //    MarqueASuppr.IdMarque = context.Marques.Where(c => c.IdMarque == MarqueASuppr.IdMarque).FirstOrDefault().IdMarque;

        //    // Act
        //    var result = await controller.DeleteMarque(MarqueASuppr.IdMarque);

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
        //    Assert.IsNull(context.Marques.Where(c => c.IdMarque == MarqueASuppr.IdMarque).FirstOrDefault());
        //}

        //#endregion


        //#region Tests de substitution

        //[TestMethod]
        //public void GetMarqueById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        //{
        //    // Arrange
        //    Marque Marque = new Marque
        //    {
        //        NomMarque = "SVG",
        //    };
        //    var mockRepository = new Mock<IDataRepository<Marque>>();
        //    mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(Marque);

        //    var MarqueController = new MarquesController(mockRepository.Object);

        //    // Act
        //    var actionResult = MarqueController.GetMarqueById(1).Result;

        //    // Assert
        //    Assert.IsNotNull(actionResult);
        //    Assert.IsNotNull(actionResult.Value);
        //    Assert.AreEqual(Marque, actionResult.Value as Marque);
        //}

        //[TestMethod]
        //public void GetMarqueById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        //{
        //    var mockRepository = new Mock<IDataRepository<Marque>>();
        //    var MarqueController = new MarquesController(mockRepository.Object);

        //    // Act
        //    var actionResult = MarqueController.GetMarqueById(0).Result;

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        //}

        //[TestMethod]
        //public void PostMarque_ModelValidated_CreationOK_AvecMoq()
        //{
        //    // Arrange
        //    var mockRepository = new Mock<IDataRepository<Marque>>();
        //    var userController = new MarquesController(mockRepository.Object);

        //    Marque user = new Marque
        //    {
        //        NomMarque = "SVG"
        //    };

        //    // Act
        //    var actionResult = userController.PostMarque(user).Result;

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Marque>), "Pas un ActionResult<Marque>");
        //    Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
        //    var result = actionResult.Result as CreatedAtActionResult;
        //    Assert.IsInstanceOfType(result.Value, typeof(Marque), "Pas un Marque");
        //    user.IdMarque = ((Marque)result.Value).IdMarque;
        //    Assert.AreEqual(user, (Marque)result.Value, "Marques pas identiques");
        //}

        //[TestMethod]
        //public void DeleteMarqueTest_AvecMoq()
        //{
        //    // Arrange
        //    Marque user = new Marque
        //    {
        //        NomMarque = "SVG",
        //    };

        //    var mockRepository = new Mock<IDataRepository<Marque>>();
        //    mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
        //    var userController = new MarquesController(mockRepository.Object);

        //    // Act
        //    var actionResult = userController.DeleteMarque(1).Result;

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        //}

        //[TestMethod]
        //public async Task PutMarque_ModelValidated_UpdateOK_AvecMoq()
        //{
        //    // Arrange
        //    Marque userAMaJ = new Marque
        //    {
        //        IdMarque = 1,   // Initialiser l'ID du Marque à mettre à jour
        //        NomMarque = "SVG",
        
        //    };

        //    Marque userUpdated = new Marque
        //    {
        //        IdMarque = 1,   // Assurez-vous que l'ID correspond à celui que vous souhaitez mettre à jour
        //        NomMarque = "SV",
               
        //    };

        //    // Simuler le repository avec Moq
        //    var mockRepository = new Mock<IDataRepository<Marque>>();

        //    // Simuler la méthode GetByIdAsync pour retourner le Marque à mettre à jour
        //    mockRepository.Setup(x => x.GetByIdAsync(userAMaJ.IdMarque)).ReturnsAsync(userAMaJ);

        //    // Simuler la méthode UpdateAsync avec deux paramètres
        //    mockRepository.Setup(x => x.UpdateAsync(userAMaJ, userUpdated)).Returns(Task.CompletedTask);

        //    // Créer le contrôleur avec le mock du repository
        //    var userController = new MarquesController(mockRepository.Object);

        //    // Act
        //    var actionResult = await userController.PutMarque(userUpdated.IdMarque, userUpdated);

        //    // Assert
        //    // Vérifier que la méthode UpdateAsync a bien été appelée avec les bons paramètres
        //    mockRepository.Verify(x => x.UpdateAsync(userAMaJ, userUpdated), Times.Once);

        //    // Vérifier que le retour est bien un NoContentResult, ce qui signifie que la mise à jour a réussi
        //    Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        //}



        //#endregion

        //#region Test de DTO


        //#endregion
    }
}