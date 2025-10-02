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
    [Category("Escuelas")]
    public class TC001CreacionEscuelaEntrenamientoTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();
        private readonly string urlScreen = "/Training/Maintenances/SchoolTraining.aspx";


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

            seleniumEntities.Add(new SeleniumEntity("SH001", "SH001_SchoolCreate"));
            seleniumEntities.Add(new SeleniumEntity("SH002", "SH002_SchoolCreate"));
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
        public async Task TC001CreacionEscuelaEntrenamiento()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);


            for (int i = 0; i < 2; i++)
            {
                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de la Escuela.");

                Thread.Sleep(1000);
                myCommons.SchoolTrainingCreate(wait, driver, js, i, seleniumEntities);


                myCommons.SearchSchoolByCode(wait, driver, js, seleniumEntities[i].Code);
                Thread.Sleep(1000);

                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvSchoolTrainingCode")));
                myCommons.Log.Information($"{DateTime.Now}: Buscando La Escuela de Entrenamiento creado  " + seleniumEntities[i].Code);

                // Los asserts validan que el test case, realmente inserto las aulas
                string displayedCode = driver.FindElement(By.Id("dvSchoolTrainingCode")).Text;
                Assert.AreEqual(seleniumEntities[i].Code, displayedCode, "La código de la Escuela no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Código de la Escuela  " + seleniumEntities[i].Code);


                string displayedDescription = driver.FindElement(By.Id("dvSchoolTrainingName")).Text;
                Assert.AreEqual(seleniumEntities[i].Description, displayedDescription, "La descripción de la Escuela no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Descripción de la Escuela  " + seleniumEntities[i].Description);
            }

        }
    }
}