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



    }
}