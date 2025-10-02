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
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Automation.Selenium
{
    [TestFixture]
    [Category("Matriz")]
    public class TC003CaracteresEspecialesMatrizAlcanceCreacionTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly string urlScreen = "/Training/Maintenances/MatrixTarget.aspx";

        private readonly SeleniumEntity seleniumEntities = new SeleniumEntity(@"MATscriptt", @"MATscripttest@__:script :.LPPñ09987:ÑP.@");

        private IWebDriver driver;
        WebDriverWait wait;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        //Actions actions;
        Commons myCommons;

        [SetUp]
        public void SetUp()
        {
            urlTest = !string.IsNullOrEmpty(urlEntorno) ? urlEntorno : ConfigurationManager.AppSettings.Get("UrlApplicationForTesting");
            driver = new EdgeDriver();
            myCommons = new Commons();
            myCommons.log();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));


            driver.Manage().Window.Maximize();
            // Inicializa el ejecutor de JavaScript y el diccionario de variables

            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
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
        public async Task TC003CaracteresEspecialesMatrizAlcanceCreacion()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
            //myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen).GetAwaiter().GetResult();
            // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            Thread.Sleep(500);
            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de Tipo de Matriz.");
            myCommons.MatrixTargetCharacters(wait, driver, js, new List<SeleniumEntity> { seleniumEntities });


            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetCodeFilter")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeFilter")).SendKeys("MATscriptt");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Buscar el Código" + seleniumEntities.Code);


            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvMatrixTargetCode")));
            string displayedCode = driver.FindElement(By.Id("dvMatrixTargetCode")).Text;
            Assert.AreEqual("MATscriptt", displayedCode, "La código de la Matriz no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert en el campo de Código  " + displayedCode);


            string displayedDescription = driver.FindElement(By.Id("dvMatrixTargetName")).Text;
            Assert.AreEqual("MATscripttest@__:script :.LPPñ09987:ÑP.@", displayedDescription, "La descripción de la Matriz no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert en el campo de Descripción  " + displayedDescription);
        }
    }
}