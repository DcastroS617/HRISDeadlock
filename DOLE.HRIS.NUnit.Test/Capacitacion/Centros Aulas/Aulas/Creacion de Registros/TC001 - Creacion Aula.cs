using DOLE.HRIS.NUnit.Test;
using DOLE.HRIS.NUnit.Test.Utilities;
using NUnit.Framework;
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
using NUnit.Framework.Interfaces;


namespace DOLE.HRIS.Application.Automation.Selenium
{
    [TestFixture]
    [Category("Aulas")]
    public class TC001CreacionAulaTest
    {

        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private  string urlTest = "" ;
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();
        private readonly string urlScreen = "/Training/Maintenances/Classrooms.aspx";

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        WebDriverWait wait;
        Commons myCommons;
        int cantidad;
        private object ScreenshotHelper;

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
           

            // Agrega entidades de Selenium
            seleniumEntities.Add(new SeleniumEntity("099S1", "TC001_001"));
            seleniumEntities.Add(new SeleniumEntity("099S2", "TC001_002"));
       
        
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
        public async Task TC001CreacionAula()
        {

            Random rnd = new Random();
           
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
            
     
            for (int i = 0; i < 2; i++)
            {
                cantidad = rnd.Next(1, 10);

               // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de Aulas.");

                //Los pasos para completar el formulario de creación de una aula
                AgregarDatosAula(i);
                myCommons.Log.Information($"{DateTime.Now}: Ingresando los datos respectivos para la creación del Aula.");

                //Permite buscar las aulas creadas
                myCommons.BuscarAula(wait, driver, js, seleniumEntities[i].Code);
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvClassroomDescription")));
                myCommons.Log.Information($"{DateTime.Now}: Buscando el Aula creada  " + seleniumEntities[i].Code);

                // Los asserts validan que el test case, realmente inserto las aulas
                string displayedCode = driver.FindElement(By.Id("dvClassroomCode")).Text;
                Assert.AreEqual(seleniumEntities[i].Code, displayedCode, "La código del aula no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Codigo de Aula  " + seleniumEntities[i].Code);

                string displayedDescription = driver.FindElement(By.Id("dvClassroomDescription")).Text;
                Assert.AreEqual(seleniumEntities[i].Description, displayedDescription, "La descripción del aula no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Descripción de Aula  " + seleniumEntities[i].Description);

                string displayedTrainingCenterCode = driver.FindElement(By.Id("dvTrainingCenterDescription")).Text;
                Assert.AreEqual("Oficinas Guápiles - Interno", displayedTrainingCenterCode, "El código del centro de capacitación no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert para el Centro de Capacitación Aula  " + displayedTrainingCenterCode);

                string displayedCapacity = driver.FindElement(By.Id("dvCapacity")).Text;
                Assert.AreEqual(driver.FindElement(By.Id("dvCapacity")).Text, cantidad.ToString(), $"La cantidad no es igual, deberia ser: {cantidad}");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert para Capacidade del Aula  " + displayedCapacity);

            }
        }

        private void AgregarDatosAula(int posicion)
        {
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter")));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Oficinas Guápiles - Interno");
            myCommons.Log.Information($"{DateTime.Now}:Selecionado el Centro de Capacitación: Oficinas Guápiles - Interno" );


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomCode");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode")).SendKeys(seleniumEntities[posicion].Code);
            myCommons.Log.Information($"{DateTime.Now}:Ingresando el código del Aula {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomDescription");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).SendKeys(seleniumEntities[posicion].Description);
            myCommons.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Aula {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtCapacity"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtCapacity");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).SendKeys(cantidad.ToString());
            myCommons.Log.Information($"{DateTime.Now}:Ingresando la capacidad del Aula {cantidad.ToString()}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtComments"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtComments");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).SendKeys("Selenium");
            myCommons.Log.Information($"{DateTime.Now}:Ingresando la Facilidad Disponible del  Aula : Selenium");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-6 .active"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el boton de activar el aula");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nueva Aula ");

            Thread.Sleep(1500);

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Thread.Sleep(1500);
        }
                    
    }
}
