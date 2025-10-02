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
    public class TC002AsociarEscuelaAlCursoSoloPermiteURegistroTest
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

            seleniumEntities.Add(new SeleniumEntity("COOUno01", "COOUno01_CourseCreate"));

            seleniumEntitiesSchool.Add(new SeleniumEntity("SHOUNO001", "Escuela Asociar1 1 a 1"));
            seleniumEntitiesSchool.Add(new SeleniumEntity("SHOUNO002", "Escuela Asociar2 1 a 1"));



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
        public async Task TC002AsociarEscuelaAlCursoSoloPermiteUnRegistro()
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
              
              Thread.Sleep(1000);

            for (int i = 0; i < 2; i++)
            {

                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación de la Escuela.");

                Thread.Sleep(1000);
                myCommons.SchoolTrainingCreate(wait, driver, js, i, seleniumEntitiesSchool);
                myCommons.Log.Information($"{DateTime.Now}: Se crea la escuela con el código" + seleniumEntitiesSchool[0].Code);

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

            //Click en el botón de Escuelas
            //wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditSchoolsTraining")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditSchoolsTraining"))).Click();
            //myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar la Escuela");

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


            // Buscar al instructor por código

            var inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchSchoolsTraining")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("SHOUNO001");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar SHOUNO001");


           // wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_rptSchoolsTraining_ctl01_btnAddSchoolTraining"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[id*='divAddControlsSchoolsTraining'] button.btnAddSchoolTraining")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddSchoolTraining"))).Click();

            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar la Escuela");




            //// --- INICIO DE LA SECCIÓN CRÍTICA PARA EL STALE ELEMENT REFERENCE ---

            //// Define el localizador para el div de la escuela asociada.
            //// Lo usamos para esperar su visibilidad y para re-localizarlo si se vuelve stale.
            //By schoolDivLocator = By.CssSelector("div.data-sort-src[data-sort-code='SHOUNO001'][data-sort-name='Escuela Asociar1 1 a 1']");

            //// Esperar que el div de la escuela asociada esté visible en la tabla.
            //// Esto asegura que el DOM se ha actualizado y el elemento está presente y visible.
            //myCommons.Log.Information($"{DateTime.Now}: Esperando que la Escuela sea visible en la tabla de Escuelas asociados");
            //wait.Until(ExpectedConditions.ElementIsVisible(schoolDivLocator));

            //// Ahora, vamos a localizar el elemento principal de la fila de la escuela.
            //// Usamos una variable auxiliar para que pueda ser re-asignada si se vuelve stale.
            //IWebElement filaEscuela = null;

            //// Esta espera es la que contiene la lógica para re-localizar el elemento si se vuelve stale.
            //wait.Until(driver =>
            //{
            //    try
            //    {
            //        // Intentar encontrar el elemento. Si ya está localizado, FindElement dentro de un elemento stale
            //        // podría lanzar StaleElementReferenceException, por eso re-localizamos `filaEscuela` al principio del loop.
            //        filaEscuela = driver.FindElement(schoolDivLocator);

            //        // Una vez que tenemos el elemento fresco, intentamos encontrar sus spans y verificar el texto.
            //        var spans = filaEscuela.FindElements(By.TagName("span"));

            //        // Verificamos que haya al menos 2 spans y que su texto no esté vacío.
            //        return spans.Count >= 2 &&
            //               !string.IsNullOrWhiteSpace(spans[0].Text) &&
            //               !string.IsNullOrWhiteSpace(spans[1].Text);
            //    }
            //    catch (StaleElementReferenceException)
            //    {
            //        // Si ocurre un StaleElementReferenceException, significa que el DOM ha cambiado.
            //        // Devuelve false para que la espera continúe y re-intente la localización en la siguiente iteración.
            //        // El `filaEscuela = driver.FindElement(schoolDivLocator);` al inicio del try-block
            //        // se encargará de obtener el elemento fresco en la próxima iteración.
            //        return false;
            //    }
            //    catch (NoSuchElementException)
            //    {
            //        // Si el elemento no se encuentra, también esperamos y reintentamos.
            //        return false;
            //    }
            //});


            //filaEscuela = driver.FindElement(schoolDivLocator);
            //var spansConfirmados = filaEscuela.FindElements(By.TagName("span"));

            //// Validar existencia de los spans
            //Assert.That(spansConfirmados.Count, Is.GreaterThanOrEqualTo(2), "No se encontraron los spans esperados dentro del div de escuela");


            //filaEscuela = driver.FindElement(schoolDivLocator);

            //// Extraer el código y el nombre usando selectores claros
            //string codigo = filaEscuela.FindElement(By.CssSelector("div.col-xs-4 span")).Text.Trim();
            //string nombre = filaEscuela.FindElement(By.CssSelector("div.col-xs-6 span")).Text.Trim();

            //Assert.That(codigo, Is.EqualTo("SHOUNO001"), "El código de la escuela no es el esperado");
            //Assert.That(nombre, Is.EqualTo("Escuela Asociar1 1 a 1"), "El nombre de la escuela no es el esperado");

            //// --- FIN DE LA SECCIÓN CRÍTICA PARA EL STALE ELEMENT REFERENCE ---
            ///

            // --- INICIO DE LA NUEVA SECCIÓN ROBUSTA CONTRA STALE ELEMENT ---

            // Define el locator de la escuela asociada
            By schoolDivLocator = By.CssSelector("div.data-sort-src[data-sort-code='SHOUNO001'][data-sort-name='Escuela Asociar1 1 a 1']");

            // Esperar que el div de la escuela aparezca visible
            myCommons.Log.Information($"{DateTime.Now}: Esperando que la Escuela sea visible en la tabla de Escuelas asociadas");

            wait.Until(ExpectedConditions.ElementIsVisible(schoolDivLocator));

            // Método auxiliar con reintento para extraer código y nombre
            (string Codigo, string Nombre) GetSchoolInfo(IWebDriver drv, By locator)
            {
                int reintentos = 3;
                for (int i = 0; i < reintentos; i++)
                {
                    try
                    {
                        var el = drv.FindElement(locator);
                        string codigo = el.FindElement(By.CssSelector("div.col-xs-4 span")).Text.Trim();
                        string nombre = el.FindElement(By.CssSelector("div.col-xs-6 span")).Text.Trim();
                        return (codigo, nombre);
                    }
                    catch (StaleElementReferenceException)
                    {
                        if (i == reintentos - 1)
                            throw;
                        Thread.Sleep(300);
                    }
                }
                throw new Exception("No se pudo obtener la información de la escuela.");
            }

            // Recolectar datos frescos
            var info = GetSchoolInfo(driver, schoolDivLocator);

            // Validar
            Assert.That(info.Codigo, Is.EqualTo("SHOUNO001"), "El código de la escuela no es el esperado");
            Assert.That(info.Nombre, Is.EqualTo("Escuela Asociar1 1 a 1"), "El nombre de la escuela no es el esperado");

            // --- FIN DE LA NUEVA SECCIÓN ROBUSTA CONTRA STALE ELEMENT ---



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnSchoolsTrainingAccept"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar para cerrar la modal de Asociación de Escuelas ");




            myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();

            //Click en el botón de Escuela
            wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditSchoolsTraining")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditSchoolsTraining"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar la Escuela");



            // Buscar al escuela por código
            inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchSchoolsTraining")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("SHOUNO002");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar SHOUNO002");


            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[id*='divAddControlsSchoolsTraining'] button.btnAddSchoolTraining")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddSchoolTraining"))).Click();


           // wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_rptSchoolsTraining_ctl01_btnAddSchoolTraining"))).Click();

            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar la Escuela");
           

            try
            {
                // Espera a que aparezca el contenedor de mensajería
                var contenedor = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector(".contenedor-mensajeria.panel.panel-default")));

                // Espera y valida el título "Advertencia"
                var tituloAdvertencia = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//h3[contains(@class,'panel-title') and contains(text(),'Advertencia')]")));

                Assert.AreEqual("Advertencia", tituloAdvertencia.Text.Trim(),
                    "El título del mensaje no coincide con 'Advertencia'.");

                // Espera y valida el contenido del mensaje
                var mensajeAdvertencia = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[contains(@class,'panel-body')]//p")));

                Assert.AreEqual("Ya existe una escuela asociada a este curso. No se puede asociar más de una.",
                    mensajeAdvertencia.Text.Trim(),
                    "El mensaje de advertencia no es el esperado.");

                myCommons.Log.Information("Mensaje de advertencia mostrado correctamente: " + mensajeAdvertencia.Text);
            }
            catch (WebDriverTimeoutException ex)
            {
                myCommons.Log.Error("No se mostró el mensaje de advertencia esperado.");
                throw;
            }





        }
    }
}