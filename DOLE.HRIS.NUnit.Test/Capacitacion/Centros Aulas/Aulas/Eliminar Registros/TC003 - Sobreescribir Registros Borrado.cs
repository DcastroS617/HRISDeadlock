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
using System.Xml.Linq;

namespace DOLE.HRIS.Application.Automation.Selenium


{
    [TestFixture]
    [Category("Aulas")]
    public class TC003SobreescribirRegistroBorradoAulaTest
    {

        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly string urlScreen = "/Training/Maintenances/Classrooms.aspx";

        private readonly SeleniumEntity seleniumEntities = new SeleniumEntity("AUS001", "AUS001_SobreescribirBorrado");
        private readonly SeleniumEntity seleniumEntitiesEdited = new SeleniumEntity("AUS001", "AUS001_SobreescribirBorradoAula");

        private IWebDriver driver;
        WebDriverWait wait;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        Commons myCommons;
        int cantidad;
        string expectedErrorMessage = "Los datos ingresados provocan un conflicto con otro registro por duplicidad de código.\nLos datos del registro existente son los siguientes:";

        [SetUp]
        public void SetUp()
        {
            urlTest = !string.IsNullOrEmpty(urlEntorno) ? urlEntorno : ConfigurationManager.AppSettings.Get("UrlApplicationForTesting");
            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();
            myCommons = new Commons();
            myCommons.log();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

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
        public async Task TC003SobreescribirRegistroBorradoAula()
        {
          
            Random rnd = new Random();

            cantidad = rnd.Next(1, 10);

            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de Centros.");

            //AgregarCentro();
            myCommons.AgregarAula(wait, driver, js, cantidad, new List<SeleniumEntity> { seleniumEntities });
            myCommons.Log.Information($"{DateTime.Now}: Creando Registro." + seleniumEntities.Code);



            myCommons.BuscarAula(wait, driver, js, seleniumEntities.Code);
            myCommons.Log.Information($"{DateTime.Now}: Buscar el registro." + seleniumEntities.Code);

            myCommons.DeletedClassroom(wait, driver, js, seleniumEntities.Code);
            myCommons.Log.Information($"{DateTime.Now}:Se elimina el registro." + seleniumEntities.Code);
            Utilities.CleanFields(js, "ctl00_cntBody_txtClassroomCodeFilter");
            myCommons.Log.Information($"{DateTime.Now}:limpiar el filtro de Código." + seleniumEntities.Code);


            //Thread.Sleep(1000);
   
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Ingresando nuevamente en la modal de Creación de Aula.");
            SobreescribirAulaBorrada();


 
            //Assert para validar la modal de Registro Desactivado
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_divActivateDeletedDialog")));
            string errorMessage = driver.FindElement(By.Id("ctl00_cntBody_divActivateDeletedDialog")).Text;
            errorMessage = errorMessage.Replace("\r", "");
            Assert.AreEqual(expectedErrorMessage, errorMessage, "El mensaje de error no coincide con el esperado.");
            myCommons.Log.Information($"{DateTime.Now}:Despliegue de modal de Registro desactivado con el mensaje " + expectedErrorMessage);

            // Esperar hasta que el segundo toggle sea visible
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbUpdateActivateDeleted ~ .toggle-group .toggle-handle")));
                   

            IWebElement secondToggle = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='ctl00_cntBody_chbUpdateActivateDeleted']/following-sibling::div[contains(@class, 'toggle-group')]//button[contains(@class, 'toggle-handle')]")));
            secondToggle.Click();




            myCommons.Log.Information($"{DateTime.Now}:Seleccionando Si para Activar registro encontrado actualizando con los nuevos datos ");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnActivateDeletedAccept")));
            driver.FindElement(By.Id("ctl00_cntBody_btnActivateDeletedAccept")).Click(); 
            myCommons.Log.Information($"{DateTime.Now}: Botón de Aceptar para agregar nuevamente el registro Desactivado con el cambio en la descripción CAC001CentroReactivarModificar");


            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            var alert = driver.FindElements(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
            if (alert.Count > 0)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            }
            else
            {
                myCommons.Log.Information("No se encontró la alerta de éxito");
            }

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            myCommons.Log.Information($"{DateTime.Now}:Seleccionando Si para Activar registro encontrado actualizando con los nuevos datos ");


            



            myCommons.BuscarAula(wait, driver, js, seleniumEntitiesEdited.Code);
            myCommons.Log.Information($"{DateTime.Now}: Buscar el Aula a modificar   " + seleniumEntities.Code);

           

            // Los asserts validan que el test case, realmente inserto las aulas

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvClassroomCode")));

          
            // Los asserts validan que el test case, realmente inserto las aulas
            string displayedCode = driver.FindElement(By.Id("dvClassroomCode")).Text;
            Assert.AreEqual(seleniumEntities.Code, displayedCode, "La código del aula no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Codigo de Aula  " + seleniumEntitiesEdited.Code);

            string displayedDescription = driver.FindElement(By.Id("dvClassroomDescription")).Text;
            Assert.AreEqual(seleniumEntitiesEdited.Description, displayedDescription, "La descripción del aula no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Descripción de Aula  " + seleniumEntitiesEdited.Description);

            string displayedTrainingCenterCode = driver.FindElement(By.Id("dvTrainingCenterDescription")).Text;
            Assert.AreEqual("Bananito - Interno", displayedTrainingCenterCode, "El código del centro de capacitación no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert para el Centro de Capacitación Aula  " + displayedTrainingCenterCode);

            string displayedCapacity = driver.FindElement(By.Id("dvCapacity")).Text;
            Assert.AreEqual(driver.FindElement(By.Id("dvCapacity")).Text, cantidad.ToString(), $"La cantidad no es igual, deberia ser: {cantidad}");
            myCommons.Log.Information($"{DateTime.Now}: Validando el Assert para Capacidade del Aula  " + displayedCapacity);
        }
              

        private void SobreescribirAulaBorrada()
        {
            IWebElement fieldElement;
            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter"))));
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_cboTrainingCenter");
            fieldElement = driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter"));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Bananito - Interno");
            myCommons.Log.Information($"{DateTime.Now}: Modificando el Centro de Capacitación  \"Bananito - Interno ");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomCode");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode")).SendKeys(seleniumEntitiesEdited.Code);

            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))));
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomDescription");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).SendKeys(seleniumEntitiesEdited.Description);


            myCommons.Log.Information($"{DateTime.Now}: Modificando la Descripción   " + seleniumEntitiesEdited.Description);

            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtCapacity");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).SendKeys(cantidad.ToString());
            myCommons.Log.Information($"{DateTime.Now}: Modificando la Capacidad   " + cantidad.ToString());


            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtComments");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).SendKeys("Selenium");
            myCommons.Log.Information($"{DateTime.Now}: Modificando el campo de Comentario Selenium ");


            driver.FindElement(By.Id("ctl00_cntBody_btnAccept")).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de Aceptar para aplicar la modificación ");


            

        }
    }
}