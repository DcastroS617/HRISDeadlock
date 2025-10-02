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
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Automation.Selenium
{
    [TestFixture]
    [Category("Matriz")]
    public class TC001CreacionMatrizAlcanceConSelecciónPorDivisionTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();
        private readonly string urlScreen = "/Training/Maintenances/MatrixTarget.aspx";


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

            seleniumEntities.Add(new SeleniumEntity("MPD001", "MPD001_MatrixCreate"));
            seleniumEntities.Add(new SeleniumEntity("MPD002", "MPD002_MatrixCreate"));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                myCommons.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name);
            }

            driver.Quit();
            driver.Dispose();
        }

        [Test]
        public async Task TC001CreacionMatrizAlcanceConSeleccionMultiplePorDivision()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
            //myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen).GetAwaiter().GetResult();

            //for (int i = 0; i < 2; i++)
            //{
                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de la Matriz.");

                Thread.Sleep(1500);
                myCommons.MatrixWithMultipleSelectionByDivision(wait, driver, js, seleniumEntities);


                myCommons.MatrixTargetByCode(wait, driver, js, seleniumEntities[0].Code);
                Thread.Sleep(1000);

                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvMatrixTargetCode")));
                myCommons.Log.Information($"{DateTime.Now}: Buscando la Matriz creado  " + seleniumEntities[0].Code);

                // Los asserts validan que el test case, realmente inserto las aulas
                string displayedCode = driver.FindElement(By.Id("dvMatrixTargetCode")).Text;
                Assert.AreEqual(seleniumEntities[0].Code, displayedCode, "La código de la Matriz no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Código de la Matriz  " + seleniumEntities[0].Code);


                string displayedDescription = driver.FindElement(By.Id("dvMatrixTargetName")).Text;
                Assert.AreEqual(seleniumEntities[0].Description, displayedDescription, "La descripción de la Matriz no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Descripción de la Matriz  " + seleniumEntities[0].Description);

                string displayedDivision = driver.FindElement(By.Id("dvDivisionName")).Text;
                Assert.AreEqual("1-División de Costa Rica", displayedDivision, "LA División no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert para la División seleccionada   " + displayedDivision);

                string displayedStructBy = driver.FindElement(By.Id("dvStructBy")).Text;
                Assert.AreEqual("Estructura de Finca", displayedStructBy, "La Estructura por no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert para la Estructura por   " + displayedStructBy);


                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvMatrixTargetCode"))).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnEdit")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEdit"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Click en el botón Editar");

                


                Thread.Sleep(1000);
                // 1. Esperar hasta que el <h3> sea visible
                var titleElement = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("h3#dialogTitle.modal-title.text-primary")));

                // 2. Aserción de que contiene el texto esperado
                Assert.AreEqual("Editar", titleElement.Text.Trim(),
                    "El título del modal no es el esperado.");


                // Ahora validas que las zonas que marcaste antes sigan seleccionadas:
                string[] zonas = {"HND - ACA - Acarsa","HND - ASI - Asisa", "CRC - Z01 - Zona Estrella", "CRC - Z02 - Zona Rio Frio"
};
                myCommons.AssertBootstrapMultiOptionsSelected(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zonas);

                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnCancel"))).Click();

                


            //}
        }
    }
}