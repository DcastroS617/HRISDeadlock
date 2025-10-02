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
    public class TC001AsociarEscuelaAlCursoTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();

        private readonly List<SeleniumEntity> seleniumEntitiesSchool = new List<SeleniumEntity>();

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

            seleniumEntities.Add(new SeleniumEntity("CUSHO001", "CUSHO001_CourseCreate"));

            seleniumEntitiesSchool.Add(new SeleniumEntity("SHOCUR001", "Escuela A Asociar"));


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
        public async Task TC001AsociarEscuelaAlCurso()
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
             

                driver.Navigate().GoToUrl(urlTest + "/Training/Maintenances/SchoolTraining.aspx");
                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de Escuela.");
              
                Thread.Sleep(1000);
                myCommons.SchoolTrainingCreateCharacters(wait, driver, js, seleniumEntitiesSchool);
            
          
              driver.Navigate().GoToUrl(urlTest + urlScreen);
              myCommons.Log.Information($"{DateTime.Now}: Ir nuevamente a la pantalla de Cursos.");

              myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
              Thread.Sleep(2000);
              wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
              myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

              Thread.Sleep(1500);
              //wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));
              //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();

             wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));
             var element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode")));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
             element.Click();

            //Click en el botón de Propósitos
            //wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditSchoolsTraining")));
            //  wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditSchoolsTraining"))).Click();
            //  myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar la Escuela");

            try
            {
                // Esperar que el botón esté presente en el DOM
                var editButton = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditSchoolsTraining")));

                // Esperar que sea visible y clickeable
                wait.Until(ExpectedConditions.ElementToBeClickable(editButton));

                // Usar JavaScript para evitar errores por overlays u obstrucciones
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", editButton);

                myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Escuela");
            }
            catch (WebDriverTimeoutException ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Timeout esperando el botón de editar Escuela: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Error al hacer click en el botón de editar Escuela: {ex.Message}");
                throw;
            }


            Thread.Sleep(1500);

            // Buscar al instructor por código
            wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_txtSearchSchoolsTraining")));

            var inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchSchoolsTraining")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("SHOCUR001");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar SHOCUR001");

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[id*='divAddControlsSchoolsTraining'] button.btnAddSchoolTraining")));
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button.btnAddSchoolTraining")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddSchoolTraining"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar la Escuela");

            // Esperar que aparezca la tabla de resultados
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableBodySelectSchoolTraining")));
            myCommons.Log.Information($"{DateTime.Now}:Esperar que la Escuela sea visible en la tabla de Escuelas asociados");

                
            Thread.Sleep(1000);
         

            // Esperar el div con atributos específicos
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.data-sort-src[data-sort-code='SHOCUR001'][data-sort-name='Escuela A Asociar']")));

            // Reubicar el elemento para evitar StaleElementReferenceException
            var filaInstructor = driver.FindElement(
                By.CssSelector("div.data-sort-src[data-sort-code='SHOCUR001'][data-sort-name='Escuela A Asociar']"));

            var spans = filaInstructor.FindElements(By.TagName("span"));

            string codigo = spans[0].Text.Trim();
            string nombre = spans[1].Text.Trim();

            Assert.AreEqual("SHOCUR001", codigo, "El código de la Escuela no coincide."); 
            myCommons.Log.Information($"{DateTime.Now}: El código de la Escuela es el correcto" + codigo);

            Assert.AreEqual("Escuela A Asociar", nombre, "El nombre de la Escuela no coincide.");
             myCommons.Log.Information($"{DateTime.Now}: El nombre de la Escuela es el correcto" + nombre);





            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnSchoolsTrainingAccept"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar para cerrar la modal de Asociación de Escuelas ");




            myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el registro en el grid ");


            //Click en el botón de Propósitos
            wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditSchoolsTraining")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditSchoolsTraining"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar la Escuela");



            // Esperar que aparezca la tabla de resultados
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableBodySelectSchoolTraining")));
            myCommons.Log.Information($"{DateTime.Now}:Esperar que la Escuela sea visible en la tabla de Escuelas asociados");
         


            Thread.Sleep(1000);



            // Esperar el div con atributos específicos
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.data-sort-src[data-sort-code='SHOCUR001'][data-sort-name='Escuela A Asociar']")));

            // Reubicar el elemento para evitar StaleElementReferenceException
             filaInstructor = driver.FindElement(
                By.CssSelector("div.data-sort-src[data-sort-code='SHOCUR001'][data-sort-name='Escuela A Asociar']"));

             spans = filaInstructor.FindElements(By.TagName("span"));

             codigo = spans[0].Text.Trim();
             nombre = spans[1].Text.Trim();

            Assert.AreEqual("SHOCUR001", codigo, "El código de la Escuela no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: El código de la Escuela es el correcto" + codigo);

            Assert.AreEqual("Escuela A Asociar", nombre, "El nombre de la Escuela no coincide.");
            myCommons.Log.Information($"{DateTime.Now}: El nombre de la Escuela es el correcto" + nombre);




        }
    }
}