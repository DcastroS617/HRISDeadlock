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
    [Category("Matriz")]
    public class TC001FiltrosMatrizAlcanceTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly string urlScreen = "/Training/Maintenances/MatrixTarget.aspx";

        private readonly SeleniumEntity seleniumEntities = new SeleniumEntity("MAF001", "MAF001_matrizAFiltrar");

        private IWebDriver driver;
        WebDriverWait wait;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        Commons myCommons;

        [SetUp]
        public void SetUp()
        {
            urlTest = !string.IsNullOrEmpty(urlEntorno) ? urlEntorno : ConfigurationManager.AppSettings.Get("UrlApplicationForTesting");
            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();
            myCommons = new Commons();
            myCommons.log();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(80));

            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
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
        public async Task TC001FiltrosMatrizAlcance()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Esperando que el botón de Aceptar se muestre.");

            // Realiza acciones con el elemento <body>
            {
                var bodyElement = wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("body")));
                Actions builder = new Actions(driver);
                builder.MoveToElement(bodyElement, 0, 0).Perform();
            }


            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de Matriz.");

            Thread.Sleep(1000);
            myCommons.MatrixTargetCharacters(wait, driver, js, new List<SeleniumEntity> { seleniumEntities });
            myCommons.Log.Information($"{DateTime.Now}: Se crea el registro con los datos" + seleniumEntities.Code + seleniumEntities.Description);

            myCommons.MatrixTargetByCode(wait, driver, js, seleniumEntities.Code);
            myCommons.Log.Information($"{DateTime.Now}: Buscar el registro recién creado" + seleniumEntities.Code + seleniumEntities.Description);
            Thread.Sleep(1000);

            // Espera para ver si se actualiza
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvMatrixTargetCode")));
            string displayedCode = driver.FindElement(By.Id("dvMatrixTargetCode")).Text;
            myCommons.Log.Information($"Código del tipo de formación recuperado después de la búsqueda: " + seleniumEntities.Code);


            myCommons.Log.Information($"{DateTime.Now}: Se inicia la validación de los assert luego de filtar por ctl00_cntBody_MatrixTargetCodeFilter.");
            Assert.AreEqual(seleniumEntities.Code, displayedCode, "El código de la Matriz n no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por dvMatrixTargetCode coincida con {seleniumEntities.Code}.");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvMatrixTargetName")));
            string displayedDescription = driver.FindElement(By.Id("dvMatrixTargetName")).Text;
            Assert.AreEqual(seleniumEntities.Description, displayedDescription, "La descripción de la Matriz no coincide." + seleniumEntities.Description);
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por dvTypeTrainingName coincida con: " + seleniumEntities.Description);

            myCommons.MatrixTargetByName(wait, driver, js, seleniumEntities.Description);
            myCommons.Log.Information($"{DateTime.Now}: Buscar el registro recién creado" + seleniumEntities.Code + seleniumEntities.Description);


            myCommons.Log.Information($"{DateTime.Now}: Digitando valor a buscar en el filtro ctl00_cntBody_MatrixTargetNameFilter.  {seleniumEntities.Description}");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            myCommons.Log.Information($"Click en el botón de Buscar");

            //Espera que el elemento en el grid sea visble
            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvMatrixTargetName")));
            displayedDescription = driver.FindElement(By.Id("dvMatrixTargetName")).Text;
            Assert.AreEqual(seleniumEntities.Description, displayedDescription, "La descripción de la Matriz no coincide." + seleniumEntities.Description);
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por dvMatrixTargetName coincida con:" + seleniumEntities.Description);

            Thread.Sleep(1000);
           
            myCommons.MatrixTargetByStructure(wait, driver, js);
            myCommons.Log.Information($"{DateTime.Now}: Buscar solo por Estructura de Finca");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvStructBy")));
            string displayedStructure = driver.FindElement(By.Id("dvStructBy")).Text;
            Assert.AreEqual("Estructura de Finca", displayedStructure, "La estrucutra de la Matriz no coincide." + displayedStructure);
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por dvStructBy coincida con:" + displayedStructure);

            myCommons.MatrixTargetByPayrollClass(wait, driver, js);
            myCommons.Log.Information($"{DateTime.Now}: Buscar solo por Estructura Clase de Nómina");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvStructBy")));
            string displayedPayroll = driver.FindElement(By.Id("dvStructBy")).Text;
            Assert.AreEqual("Estructura de Clase de Nómina", displayedPayroll, "La Clase nómina de la Matriz no coincide." + displayedPayroll);
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por dvStructBy coincida con:" + displayedPayroll);


            myCommons.MatrixTargetByCodeAndStructure(wait, driver, js, seleniumEntities.Code);
            myCommons.Log.Information($"{DateTime.Now}: Buscar el registro por Estructura de Finca y Código");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvStructBy")));
            displayedStructure = driver.FindElement(By.Id("dvStructBy")).Text;
            Assert.AreEqual("Estructura de Finca", displayedStructure, "La estrucutra de la Matriz no coincide." + displayedStructure);
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por dvStructBy coincida con:" + displayedStructure);

           

        }

    }
}