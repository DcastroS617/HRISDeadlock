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
    [Category("Propositos")]
    public class TC003CaracteresEspecialesTiposFormacionTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly string urlScreen = "/Training/Maintenances/TypeTraining.aspx";

        private readonly SeleniumEntity seleniumEntities = new SeleniumEntity(@"TFCscript/>test!@#$%^*()__++{}:<>?<script />;:,.[L¨P¨P*ñ{09'987:Ñ[*?P>.><<!@#$%^&*((", @"TFCscript/>test!@#$%^*()__++{}:<>?<script />;:,.[L¨P¨P*ñ{09'987:Ñ[*?P>.><<!@#$%^&*((");

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
        protected void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                myCommons.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name);
            }

            driver.Quit();
        }

        [Test]
        public async Task TC003CaracteresEspecialesTiposFormacionCreacion()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);

            // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            Thread.Sleep(500);
            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de Tipo de Formación.");
            myCommons.TypeTrainingCharacters(wait, driver, js, new List<SeleniumEntity> { seleniumEntities });
                       

            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingCodeFilter")));
            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeFilter")).SendKeys(seleniumEntities.Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Buscar el Código" + seleniumEntities.Code);


             Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTypeTrainingCode")));
            string displayedCode = driver.FindElement(By.Id("dvTypeTrainingCode")).Text;
            Assert.AreEqual("TFCscriptt", displayedCode, "La código del rorpósito no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert en el campo de Código  " + displayedCode);


            string displayedDescription = driver.FindElement(By.Id("dvTypeTrainingName")).Text;
            Assert.AreEqual("TFCscripttest@__:script :.LPPñ09987:ÑP.@", displayedDescription, "La descripción del propósito no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert en el campo de Descripción  " + displayedDescription);


        }
    }
}