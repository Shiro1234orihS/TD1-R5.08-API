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
    public class ProduitsControllerTests
    {
        #region Private Fields
        private ProduitsController controller;
        private DBContexte context;
        private IDataRepository<Produit> dataRepository;
        private IDataDtoProduit dataDPO;
        private MapperConfiguration config;
        private IMapper _mapper;
        #endregion

        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<DBContexte>()
                .UseNpgsql("Server=localhost;port=5432;Database=TD1_cod; uid=postgres; password=postgres");
            context = new DBContexte(builder.Options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProduit());
                cfg.AddProfile(new MapperMarque());
            });

            _mapper = config.CreateMapper();
            dataRepository = new ProduitManager(context, _mapper);

            // Instanciation correcte de dataDPO
            dataDPO = new ProduitManager(context, _mapper);

            controller = new ProduitsController(dataRepository, dataDPO);
        }

        #region Test unitaires

        [TestMethod]
        public async Task GetProduits_ReturnsRightItems()
        {
            // Act
            var result = await controller.Getproduits();

            var expected = context.Produits.ToList();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Produit>>), "Pas un ActionResult");
            var actionResult = result as ActionResult<IEnumerable<Produit>>;
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            CollectionAssert.AreEqual(expected, (List<Produit>)actionResult.Value, "Pas les mêmes Produits");
        }

        [TestMethod]
        public async Task GetProduitById_ExistingIdPassed_ReturnsRightItem()
        {
            var result = await controller.GetproduitById(1);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Produit>), "Pas un ActionResult");
            var actionResult = result as ActionResult<Produit>;
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(context.Produits.FirstOrDefault(c => c.IdProduit == 1),
                actionResult.Value, "Produits pas identiques");
        }

        [TestMethod]
        public async Task GetProduitById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            var result = await controller.GetproduitById(0);

            Assert.IsInstanceOfType(result, typeof(ActionResult<Produit>), "Pas un ActionResult");
            Assert.IsNull(result.Value, "Produit pas null");
        }

        [TestMethod]
        public async Task GetDpoProduit_ReturnsRightItem()
        {
            // Arrange
            var produits = await context.Produits
                                         .Include(p => p.IdTypeProduitNavigation)
                                         .Include(p => p.IdMarqueNavigation)
                                         .ToListAsync();

            IEnumerable<ProduitDto> produitDtos = _mapper.Map<IEnumerable<ProduitDto>>(produits);

            // Act
            var result = await controller.GetDpoProduit();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult), "Le résultat n'est pas un OkObjectResult.");
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value, "La liste des produits renvoyée est null.");

            var actual = okResult.Value as IEnumerable<ProduitDto>;
            Assert.IsNotNull(actual, "La liste des produits renvoyée est null.");

            // Comparaison
            var expected = produitDtos.Select(dto => new { dto.Id, dto.Nom }).ToList();
            var actualList = actual.Select(dto => new { dto.Id, dto.Nom }).ToList();
            CollectionAssert.AreEqual(expected, actualList, "Les produits ne correspondent pas.");
        }

        [TestMethod]
        public async Task GetDpoProduit_ReturnsNotFound_MOQ()
        {
            // Arrange
            var mockDataDPO = new Mock<IDataDtoProduit>();
            mockDataDPO.Setup(dpo => dpo.GetAllAsyncProduitDto()).ReturnsAsync(new List<ProduitDto>()); // Renvoie une liste vide

            var controller = new ProduitsController(dataRepository, mockDataDPO.Object);

            // Act
            var result = await controller.GetDpoProduit();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Le résultat n'est pas un NotFoundResult.");
        }

        [TestMethod]
        public async Task GetProduitDetailById_ReturnsRightItem()
        {
            // Arrange
            int id = 1;

            // Obtenir le produit attendu depuis le contexte, en incluant les relations nécessaires
            var produit = await context.Produits
                                        .Include(p => p.IdTypeProduitNavigation)
                                        .Include(p => p.IdMarqueNavigation)
                                        .FirstOrDefaultAsync(p => p.IdProduit == id);

            // S'assurer qu'il y a un produit correspondant
            Assert.IsNotNull(produit, "Le produit avec l'ID spécifié n'a pas été trouvé.");

            // Mapper le produit vers un DTO attendu
            ProduitDetailDto expectedProduitDetailDto = new ProduitDetailDto
            {
                Id = produit.IdProduit,
                Nom = produit.NomProduit,
                Description = produit.Description,
                Nomphoto = produit.NomPhoto,
                Uriphoto = produit.UriPhoto,
                Type = produit.IdTypeProduitNavigation?.NomTypeProduit,  // Assurez-vous que NomTypeProduit est une propriété dans TypeProduit
                Marque = produit.IdMarqueNavigation?.NomMarque,  // Assurez-vous que NomMarque est une propriété dans Marque
                Stock = produit.StockReel,
                EnReappro = produit.StockReel < produit.StockMin
            };

            // Act
            var result = await controller.GetProduitDetailById(id);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult), "Le résultat n'est pas du bon type.");

            // Récupérer l'objet du résultat
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "Le résultat OkObjectResult est null.");

            var actualProduitDetailDto = okResult.Value as ProduitDetailDto;
            Assert.IsNotNull(actualProduitDetailDto, "Le DTO du produit est null.");

            // Vérifier les valeurs de chaque propriété dans l'objet retourné
            Assert.AreEqual(expectedProduitDetailDto.Id, actualProduitDetailDto.Id, "L'ID ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Nom, actualProduitDetailDto.Nom, "Le nom du produit ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Description, actualProduitDetailDto.Description, "La description ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Nomphoto, actualProduitDetailDto.Nomphoto, "Le nom de la photo ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Uriphoto, actualProduitDetailDto.Uriphoto, "L'URI de la photo ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Type, actualProduitDetailDto.Type, "Le type de produit ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Marque, actualProduitDetailDto.Marque, "La marque du produit ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.Stock, actualProduitDetailDto.Stock, "Le stock ne correspond pas.");
            Assert.AreEqual(expectedProduitDetailDto.EnReappro, actualProduitDetailDto.EnReappro, "L'état de réapprovisionnement ne correspond pas.");
        }


        [TestMethod]
        public async Task PostProduit_ModelValidated_CreationOK()
        {
            var rnd = new Random();
            int id = rnd.Next(200, 1000);

            var ProduitAtester = new Produit
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

            var postResult = await controller.PostProduit(ProduitAtester);
            await context.SaveChangesAsync();

            var ProduitRecupere = context.Produits.FirstOrDefault(u => u.IdProduit == ProduitAtester.IdProduit);
            Assert.IsNotNull(ProduitRecupere, "Le produit n'a pas été trouvé dans la base de données.");
            Assert.AreEqual(ProduitAtester, ProduitRecupere, "Les produits ne correspondent pas.");
            await controller.DeleteProduit(id);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public async Task PostProduit_CreationFailed()
        {
            var rnd = new Random();
            int id = rnd.Next(200, 1000);

            var ProduitAtester = new Produit
            {
                IdProduit = id,
                NomProduit = null,
                Description = "test",
                NomPhoto = "test",
                UriPhoto = null,
                IdTypeProduit = 0,
                IdMarque = 0,
                StockReel = 0,
                StockMin = 0,
                StockMax = 0
            };

            await controller.PostProduit(ProduitAtester);
        }

        [TestMethod]
        public async Task PutProduit_ModelValidated_UpdateOK()
        {
            var rnd = new Random();
            int id = rnd.Next(200, 1000);

            var ProduitInitial = new Produit
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

            await controller.PostProduit(ProduitInitial);
            await context.SaveChangesAsync();

            var ProduitUpdated = new Produit
            {
                IdProduit = id,
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

            var result = await controller.PutProduit(ProduitUpdated.IdProduit, ProduitUpdated);
            await context.SaveChangesAsync();

            var ProduitRecupere = context.Produits.FirstOrDefault(c => c.IdProduit == ProduitUpdated.IdProduit);
            Assert.IsNotNull(ProduitRecupere, "Le produit mis à jour n'a pas été trouvé dans la base de données.");

            Assert.AreEqual(ProduitUpdated.NomProduit, ProduitRecupere.NomProduit, "Le nom du produit n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.Description, ProduitRecupere.Description, "La description du produit n'a pas été mise à jour.");
            Assert.AreEqual(ProduitUpdated.NomPhoto, ProduitRecupere.NomPhoto, "Le nom de la photo n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.UriPhoto, ProduitRecupere.UriPhoto, "L'URI de la photo n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.IdTypeProduit, ProduitRecupere.IdTypeProduit, "Le type de produit n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.IdMarque, ProduitRecupere.IdMarque, "La marque du produit n'a pas été mise à jour.");
            Assert.AreEqual(ProduitUpdated.StockReel, ProduitRecupere.StockReel, "Le stock réel n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.StockMin, ProduitRecupere.StockMin, "Le stock minimum n'a pas été mis à jour.");
            Assert.AreEqual(ProduitUpdated.StockMax, ProduitRecupere.StockMax, "Le stock maximum n'a pas été mis à jour.");

            await controller.DeleteProduit(id);
        }

        [TestMethod]
        public async Task DeleteProduitTest()
        {
            var rnd = new Random();
            int id = rnd.Next(200, 1000);
            var ProduitASuppr = new Produit
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

            context.Produits.Add(ProduitASuppr);
            await context.SaveChangesAsync();
            ProduitASuppr.IdProduit = context.Produits.FirstOrDefault(c => c.IdProduit == ProduitASuppr.IdProduit).IdProduit;

            var result = await controller.DeleteProduit(ProduitASuppr.IdProduit);

            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
            Assert.IsNull(context.Produits.FirstOrDefault(c => c.IdProduit == ProduitASuppr.IdProduit));
        }

        #endregion

        #region Tests de substitution

        [TestMethod]
        public void GetProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            var Produit = new Produit { NomProduit = "SVG" };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(Produit);

            var ProduitController = new ProduitsController(mockRepository.Object, null);

            var actionResult = ProduitController.GetproduitById(1).Result;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(Produit, actionResult.Value as Produit);
        }

        [TestMethod]
        public void GetProduitById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var ProduitController = new ProduitsController(mockRepository.Object, null);

            var actionResult = ProduitController.GetproduitById(0).Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostProduit_ModelValidated_CreationOK_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var userController = new ProduitsController(mockRepository.Object, null);

            var user = new Produit { NomProduit = "SVG" };

            var actionResult = userController.PostProduit(user).Result;

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
            var user = new Produit { NomProduit = "SVG" };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);

            var userController = new ProduitsController(mockRepository.Object, null);

            var actionResult = userController.DeleteProduit(1).Result;

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }

        [TestMethod]
        public async Task PutProduit_ModelValidated_UpdateOK_AvecMoq()
        {
            var userAMaJ = new Produit { IdProduit = 1, NomProduit = "SVG", Description = "Ancienne description" };
            var userUpdated = new Produit { IdProduit = 1, NomProduit = "SVG", Description = "Nouvelle description" };

            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(userAMaJ.IdProduit)).ReturnsAsync(userAMaJ);
            mockRepository.Setup(x => x.UpdateAsync(userAMaJ, userUpdated)).Returns(Task.CompletedTask);

            var userController = new ProduitsController(mockRepository.Object, null);

            var actionResult = await userController.PutProduit(userUpdated.IdProduit, userUpdated);

            mockRepository.Verify(x => x.UpdateAsync(userAMaJ, userUpdated), Times.Once);
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }

        #endregion
    }
}
