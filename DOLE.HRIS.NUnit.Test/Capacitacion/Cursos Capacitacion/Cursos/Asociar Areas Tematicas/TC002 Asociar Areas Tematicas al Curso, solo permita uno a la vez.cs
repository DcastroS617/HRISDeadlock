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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Automation.Selenium
{
    [TestFixture]
    [Category("Cursos")]
    public class TC002AsociarAreaTematicaAlCursoSoloPermiteURegistroTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();

        private readonly List<SeleniumEntity> seleniumEntitiesArea = new List<SeleniumEntity>();

        private readonly string urlScreen = "/Training/Maintenances/Courses.aspx";




        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        WebDriverWait wait;
        Commons myCommons;
        int cantidad;

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

            seleniumEntities.Add(new SeleniumEntity("CUAUno001", "CUAUno001_CourseCreate"));

            seleniumEntitiesArea.Add(new SeleniumEntity("AREUNO001", "Area Tematica Asociar1 1 a 1"));
            seleniumEntitiesArea.Add(new SeleniumEntity("AREUNO002", "Area Tematica2 Asociar2 1 a 1"));



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
        public async Task TC002AsociarAreaTematicaAlCursoSoloPermiteUnRegistro()
        {
           await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
          Random rnd = new Random();


       cantidad = rnd.Next(1, 10);
            // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
             wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación del Curso.");

            Thread.Sleep(1000);
            myCommons.CourseCreateCharacters(wait, driver, js, seleniumEntities);


           driver.Navigate().GoToUrl(urlTest + "/Training/Maintenances/ThematicAreas.aspx");
          for (int i = 0; i < 2; i++)
          {

              // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
              wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación del Area Temática.");

            Thread.Sleep(1000);
            myCommons.ThematicAreaCreate(wait, driver, js, i,seleniumEntitiesArea);
          }


            driver.Navigate().GoToUrl(urlTest + urlScreen);
            myCommons.Log.Information($"{DateTime.Now}: Ir nuevamente a la pantalla de Cursos.");

            myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();

            //Click en el botón de Propósitos
            //wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditThematicAreas")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditThematicAreas"))).Click();
            //myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Área Temática");
            try
            {
                // Esperar que el botón esté presente en el DOM
                var editButton = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditThematicAreas")));

                // Esperar que sea visible y clickeable
                wait.Until(ExpectedConditions.ElementToBeClickable(editButton));

                // Usar JavaScript para evitar errores por overlays u obstrucciones
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", editButton);

                myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Area Temática");
            }
            catch (WebDriverTimeoutException ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Timeout esperando el botón de editar Area Temática: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Error al hacer click en el botón de editar Area Temática: {ex.Message}");
                throw;
            }


            // Buscar al instructor por código
            var inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchThematicAreas")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("AREUNO001");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar AREUNO001");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableSelectThematicArea")));


            // Esperar visibilidad
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button.btnAddThematicArea")));
            // Luego esperar que sea clickeable
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddThematicArea"))).Click();





            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar el Área Temática");

            // Esperar que aparezca la tabla de resultados
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableBodySelectThematicArea")));
            myCommons.Log.Information($"{DateTime.Now}:Esperar que el Area sea visible en la tabla de Áreas Temáticas asociados");


            Thread.Sleep(2000);

            // Esperar el div con atributos específicos
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.data-sort-src[data-sort-code='AREUNO001'][data-sort-name='Area Tematica Asociar1 1 a 1']")));

            var filaInstructor = driver.FindElement(
                By.CssSelector("div.data-sort-src[data-sort-code='AREUNO001'][data-sort-name='Area Tematica Asociar1 1 a 1']"));

            

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnThematicAreasAccept"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar para cerrar la modal de Areas Temáticas ");



            myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();

            //Click en el botón de Propósitos
            wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditThematicAreas")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditThematicAreas"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Área Temática");


            // Buscar al instructor por código
             inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchThematicAreas")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("AREUNO002");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar AREUNO002");

            
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableSelectThematicArea")));

            // Esperar visibilidad
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button.btnAddThematicArea")));
            // Luego esperar que sea clickeable
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddThematicArea"))).Click();

            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar el Área Temática");

            // Esperar que aparezca el título con texto 'Error'
            var tituloError = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//h3[contains(@class,'panel-title') and contains(text(),'Error')]")));

            // Asegurar que el título sea correcto
            Assert.AreEqual("Error", tituloError.Text.Trim(), "El título del mensaje de error no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: Assert para validar que el Titulo sea visible en la modal:" + tituloError);


            // Esperar que aparezca el <p> con el mensaje de error
            var mensajeError = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[contains(@class,'panel-body')]//p")));

            // Validar el texto del mensaje
            Assert.AreEqual("Ya existe asociada una área temática con el curso seleccionado.",
                mensajeError.Text.Trim(),
                "El mensaje de error no es el esperado.");

            myCommons.Log.Information($"{DateTime.Now}: Assert para validar que mensaje de errro se muestre:" + mensajeError);


        }
    }
}