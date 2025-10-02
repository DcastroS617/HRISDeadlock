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
    public class TC002AsociarInstructorAlCursoPermiteVariosTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();

        private readonly List<SeleniumEntity> seleniumEntitiesTrainer = new List<SeleniumEntity>();

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

            seleniumEntities.Add(new SeleniumEntity("COINST01", "COINST01_CourseCreate"));

            seleniumEntitiesTrainer.Add(new SeleniumEntity("PEA001", "Persona Externa Carlos"));
            seleniumEntitiesTrainer.Add(new SeleniumEntity("PEA002", "Persona Externa Marcos"));



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
        public async Task TC002AsociarInstructorAlCursoSoloPermiteUnRegistro()
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


              driver.Navigate().GoToUrl(urlTest + "/Training/Maintenances/Trainers.aspx");
              
              Thread.Sleep(1000);

            for (int i = 0; i < 2; i++)
            {

                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación del Instructor."+ seleniumEntitiesTrainer[i]);

                Thread.Sleep(1000);
                //myCommons.CreateVariousExternalPersons(wait, driver, js, i, seleniumEntitiesTrainer);
                myCommons.CreateVariousExternalPersons(wait, driver, js,i, seleniumEntitiesTrainer);

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

            //Click en el botón de Instructor
            //wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditTrainers")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditTrainers"))).Click();
            //myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Instructor");

            try
            {
                // Esperar que el botón esté presente en el DOM
                var editButton = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditTrainers")));

                // Esperar que sea visible y clickeable
                wait.Until(ExpectedConditions.ElementToBeClickable(editButton));

                // Usar JavaScript para evitar errores por overlays u obstrucciones
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", editButton);

                myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Instructor");
            }
            catch (WebDriverTimeoutException ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Timeout esperando el botón de editar Instructor: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Error al hacer click en el botón de editar Instructor: {ex.Message}");
                throw;
            }


            // Buscar al instructor por código
            var inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchTrainers")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("PEA001");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar PEA001");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_rptTrainers_ctl01_btnAddTrainer"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar el instructor PEA001");

            // Esperar que aparezca la tabla de resultados
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableBodySelectTrainer")));
            myCommons.Log.Information($"{DateTime.Now}:Esperar que el instructor sea visible en la tabla de Instructores asociados");

            Thread.Sleep(1000);


            // Esperar el div con atributos específicos
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.data-sort-src[data-sort-code='PEA001'][data-sort-name='Persona Externa Carlos'][data-sort-type='EP']")));

            // Reubicar el elemento para evitar StaleElementReferenceException
            var filaInstructor = driver.FindElement(By.CssSelector("div.data-sort-src[data-sort-code='PEA001'][data-sort-name='Persona Externa Carlos'][data-sort-type='EP']"));

            var spans = filaInstructor.FindElements(By.TagName("span"));

            string tipo = spans[0].Text.Trim();
            string codigo = spans[1].Text.Trim();
            string nombre = spans[2].Text.Trim();

            Assert.AreEqual("Persona externa", tipo, "El tipo del instructor no coincide.");
            Assert.AreEqual("PEA001", codigo, "El código del instructor no coincide.");
            Assert.AreEqual("Persona Externa Carlos", nombre, "El nombre del instructor no coincide.");




            // Buscar al escuela por código
            inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchTrainers")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("PEA002");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar PEA002");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_rptTrainers_ctl01_btnAddTrainer"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar el instructor PEA002");

            

            // Esperar que aparezca la tabla de resultados
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableBodySelectTrainer")));
            myCommons.Log.Information($"{DateTime.Now}:Esperar que el instructor sea visible en la tabla de Instructores asociados");


            Thread.Sleep(1000);


            // Esperar el div con atributos específicos
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.data-sort-src[data-sort-code='PEA002'][data-sort-name='Persona Externa Marcos'][data-sort-type='EP']")));

          
            var filaSelector = By.CssSelector("div.data-sort-src[data-sort-code='PEA002'][data-sort-name='Persona Externa Marcos'][data-sort-type='EP']");
            wait.Until(ExpectedConditions.ElementIsVisible(filaSelector));
            filaInstructor = driver.FindElement(filaSelector);  // Refresca la referencia



             wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement tabla = wait.Until(driver => driver.FindElement(By.Id("tableSelectAssociatedTrainer")));

            // Obtener las filas del tbody
            var filas = tabla.FindElements(By.CssSelector("#tableBodySelectTrainer tr"));

            Assert.AreEqual(2, filas.Count, "Se esperaban 2 filas en la tabla de entrenadores asociados.");

            // Validar primera fila
            var columnasFila1 = filas[0].FindElements(By.TagName("span")).Select(e => e.Text.Trim()).ToList();
            Assert.Contains("Persona externa", columnasFila1);
            Assert.Contains("PEA001", columnasFila1);
            Assert.Contains("Persona Externa Carlos", columnasFila1);

            // Validar segunda fila
            var columnasFila2 = filas[1].FindElements(By.TagName("span")).Select(e => e.Text.Trim()).ToList();
            Assert.Contains("Persona externa", columnasFila2);
            Assert.Contains("PEA002", columnasFila2);
            Assert.Contains("Persona Externa Marcos", columnasFila2);




            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnTrainersAccept"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar para cerrar la modal de Asociación de Escuelas ");

            


            myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();


            //CLick en el botón de Instructores
            wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditTrainers")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditTrainers"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el campo de buscar Instructor");

            Thread.Sleep(1000);
            // Esperar que aparezca la tabla de resultados

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.table.table-hover.table-striped")));


            myCommons.Log.Information($"{DateTime.Now}:Esperar que el instructor sea visible en la tabla de Instructores asociados");

              tabla = wait.Until(driver => driver.FindElement(By.Id("tableSelectAssociatedTrainer")));

            // Obtener las filas del tbody
            filas = tabla.FindElements(By.CssSelector("#tableBodySelectTrainer tr"));

            Assert.AreEqual(2, filas.Count, "Se esperaban 2 filas en la tabla de entrenadores asociados."); 
            
            Thread.Sleep(1000);
            // Validar primera fila
            columnasFila1 = filas[0].FindElements(By.TagName("span")).Select(e => e.Text.Trim()).ToList();
            Assert.Contains("Persona externa", columnasFila1);
            Assert.Contains("PEA001", columnasFila1);
            Assert.Contains("Persona Externa Carlos", columnasFila1);

            // Validar segunda fila
              columnasFila2 = filas[1].FindElements(By.TagName("span")).Select(e => e.Text.Trim()).ToList();
            Assert.Contains("Persona externa", columnasFila2);
            Assert.Contains("PEA002", columnasFila2);
            Assert.Contains("Persona Externa Marcos", columnasFila2);




        }
    }
}