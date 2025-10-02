using DOLE.HRIS.NUnit.Test;
using DOLE.HRIS.NUnit.Test.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Automation.Selenium
{
    [TestFixture]
    [Category("Instructor")]
    public class TC001CreacionInstructorCompañiaExternaTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();
        private readonly string urlScreen = "/Training/Maintenances/Trainers.aspx";


        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        WebDriverWait wait;
        Commons myCommons;


        [SetUp]
        public void SetUp()
        {
            urlTest = !string.IsNullOrEmpty(urlEntorno) ? urlEntorno : ConfigurationManager.AppSettings.Get("UrlApplicationForTesting");
            // Inicializa el driver de Edge
            driver = new EdgeDriver();
            myCommons = new Commons();
            myCommons.log();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            driver.Manage().Window.Maximize();

            // Inicializa el ejecutor de JavaScript y el diccionario de variables
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();

            seleniumEntities.Add(new SeleniumEntity("CEX01", "Compañia Externa CEX"));
        }

        [TearDown]
        protected void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                myCommons.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name);
            }

            driver.Quit();
        }

        [Test]
        public async Task TC001CreacionInstructorCompañiaExterna()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);


           
            // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación del Instructor.");

            Thread.Sleep(1000);
            myCommons.CreateTrainer(wait, driver, js, seleniumEntities, "Compañía externa");


            myCommons.TrainerEmployeeByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTrainerCode")));
            myCommons.Log.Information($"{DateTime.Now}: Buscando El instructor creado {seleniumEntities[0].Code}");

            // Validación robusta sin displayedCode
            IWebElement visibleElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTrainerCode")));
            string displayedCode = visibleElement.Text;

            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Código del Instructor {seleniumEntities[0].Code}");
            Assert.AreEqual(seleniumEntities[0].Code, displayedCode, "El código del instructor no coincide.");

            IWebElement nameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTrainerName")));
            string displayedDescription = nameElement.Text;

            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Nombre de instructor {seleniumEntities[0].Description}");
            Assert.AreEqual(seleniumEntities[0].Description, displayedDescription, "El nombre no coincide.");
        }
    }
}