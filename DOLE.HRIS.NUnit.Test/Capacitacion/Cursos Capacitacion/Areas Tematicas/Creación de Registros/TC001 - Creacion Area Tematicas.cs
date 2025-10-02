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
    [Category("AreaTematicas")]
    public class TC001CreacionAreaTematicaTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();
        private readonly string urlScreen = "/Training/Maintenances/ThematicAreas.aspx";


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

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            driver.Manage().Window.Maximize();

            // Inicializa el ejecutor de JavaScript y el diccionario de variables
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();

            seleniumEntities.Add(new SeleniumEntity("ATH001", "AT001_AreaCreate"));
            seleniumEntities.Add(new SeleniumEntity("ATH002", "AT002_AreaCreate"));
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
        public async Task TC001CreacionAreaTematica()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);


            for (int i = 0; i < 2; i++)
            {
                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de la Escuela.");

                Thread.Sleep(1000);
                myCommons.ThematicAreaCreate(wait, driver, js, i, seleniumEntities);


                myCommons.ThematicArealByCode(wait, driver, js, seleniumEntities[i].Code);
                Thread.Sleep(2000);
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvThematicAreaCode")));
                myCommons.Log.Information($"{DateTime.Now}: Buscando el Area Temática creado  " + seleniumEntities[i].Code);

                // Los asserts validan que el test case, realmente inserto las aulas

                string displayedCode = driver.FindElement(By.Id("dvThematicAreaCode")).Text;
                Assert.AreEqual(seleniumEntities[i].Code, displayedCode, "El códigodel Area no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Código del Area  " + seleniumEntities[i].Code);


                string displayedDescription = driver.FindElement(By.Id("dvThematicAreaName")).Text;
                Assert.AreEqual(seleniumEntities[i].Description, displayedDescription, "La descripcióndel Area no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Descripción del Area " + seleniumEntities[i].Description);
            }
        }
    }
}