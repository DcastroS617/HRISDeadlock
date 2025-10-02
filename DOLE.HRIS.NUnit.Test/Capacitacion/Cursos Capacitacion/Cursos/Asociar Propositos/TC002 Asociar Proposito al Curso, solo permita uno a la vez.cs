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
using static System.Net.WebRequestMethods;

namespace DOLE.HRIS.Application.Automation.Selenium
{
    [TestFixture]
    [Category("Cursos")]
    public class TC001AsociarPropositoAlCursoSoloPermiteURegistroTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();

        private readonly List<SeleniumEntity> seleniumEntitiesPurpose = new List<SeleniumEntity>();

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

            seleniumEntities.Add(new SeleniumEntity("CUPUno001", "CUPUno001_CourseCreate"));

            seleniumEntitiesPurpose.Add(new SeleniumEntity("PROUNO001", "Propósito Asociar1 1 a 1"));
            seleniumEntitiesPurpose.Add(new SeleniumEntity("PROUNO002", "Propósito Asociar2 1 a 1"));



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
        public async Task TC001AsociarPropositoAlCursoSoloPermiteUnRegistro()
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


              driver.Navigate().GoToUrl(urlTest + "/Training/Maintenances/TrainingPurpose.aspx");

            for (int i = 0; i < 2; i++)
            {
                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
              wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
              myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación del Proposito." + seleniumEntitiesPurpose[i].Code);

              Thread.Sleep(1000);
              myCommons.AgregarPropositos(wait, driver, js, i, seleniumEntitiesPurpose);
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
            //wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditTrainingPrograms")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditTrainingPrograms"))).Click();
            //myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Proposito");
            try
            {
                // Esperar que el botón esté presente en el DOM
                var editButton = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditTrainingPrograms")));

                // Esperar que sea visible y clickeable
                wait.Until(ExpectedConditions.ElementToBeClickable(editButton));

                // Usar JavaScript para evitar errores por overlays u obstrucciones
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", editButton);

                myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Propósito");
            }
            catch (WebDriverTimeoutException ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Timeout esperando el botón de editar programas: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                myCommons.Log.Error($"[{DateTime.Now}] Error al hacer click en el botón de editar programas: {ex.Message}");
                throw;
            }


            // Buscar al instructor por código
            var inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchTrainingPrograms")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("PROUNO001");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar PROUNO001");

            // Esperar que el botón sea clickeable y hacer clic


            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button.btnAddTrainingProgram")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddTrainingProgram"))).Click();
                
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar Programas");

            // Esperar que aparezca la tabla de resultados
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableSelectAssociatedTrainingPrograms")));
            myCommons.Log.Information($"{DateTime.Now}:Esperar que el Propósito sea visible en la tabla de Propósito asociados");


            Thread.Sleep(2000);

            // Esperar el div con atributos específicos
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.data-sort-src[data-sort-code='PROUNO001'][data-sort-name='Propósito Asociar1 1 a 1']")));

            var filaInstructor = driver.FindElement(
                By.CssSelector("div.data-sort-src[data-sort-code='PROUNO001'][data-sort-name='Propósito Asociar1 1 a 1']"));

            

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnTrainingProgramsAccept"))).Click();
            myCommons.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar para cerrar la modal de Propósitos ");



            myCommons.CourseByCode(wait, driver, js, seleniumEntities[0].Code);
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            wait.Until(ExpectedConditions.ElementExists(By.Id("dvCourseCode")));

            myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  por " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();

            //Click en el botón de Propósitos
            wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_btnEditTrainingPrograms")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEditTrainingPrograms"))).Click();
            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de buscar Proposito");

            Thread.Sleep(1000);

            // Buscar al instructor por código
            inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchTrainingPrograms")));
            inputBuscar.Clear();
            inputBuscar.SendKeys("PROUNO002");
            myCommons.Log.Information($"{DateTime.Now}: Ingresando valor a Buscar PROUNO002");


            // Luego click en el icono de agregar
            // Buscar el botón dentro de esa fila

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button.btnAddTrainingProgram")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btnAddTrainingProgram"))).Click();

            myCommons.Log.Information($"{DateTime.Now}: Click en el botón de + para asociar Programas PROUNO002");


            // Esperar a que aparezca el panel de error (usamos el título como referencia)
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h3.panel-title.text-center")));

            // Una vez visible, obtenemos el mensaje de error
            var mensajeError = driver.FindElement(By.CssSelector(".contenedor-mensajeria .panel-body p"));

            // Validar el contenido del mensaje
            string textoEsperado = "No se puede agregar más de un propósito al curso";
            Assert.AreEqual(textoEsperado, mensajeError.Text.Trim(), "El mensaje de error no es el esperado.");

            // Esperar hasta que aparezca el mensaje de error
            var mensajeError2 = wait.Until(d =>
            {
                var contenedor = d.FindElement(By.CssSelector(".contenedor-mensajeria .panel-body p"));
                return contenedor.Displayed ? contenedor : null;
            });

            // Validar el texto del mensaje
             textoEsperado = "No se puede agregar más de un propósito al curso";
            Assert.AreEqual(textoEsperado, mensajeError2.Text.Trim(), "El mensaje de error no es el esperado.");


        }
    }
}