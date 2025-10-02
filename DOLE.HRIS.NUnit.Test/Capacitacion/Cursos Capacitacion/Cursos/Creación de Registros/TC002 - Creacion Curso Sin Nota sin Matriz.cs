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
    [Category("Cursos")]
    public class TC001CreacionCursoConMatrizSinNOTASinMAtrizTest
    {
        private readonly string urlEntorno = Environment.GetEnvironmentVariable("UrlApplicationForTesting", EnvironmentVariableTarget.Process);
        private string urlTest = "";
        private readonly List<SeleniumEntity> seleniumEntities = new List<SeleniumEntity>();
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

            seleniumEntities.Add(new SeleniumEntity("CSNM001", "CU001_CourseCreateSinNotaSinMatriz"));
            seleniumEntities.Add(new SeleniumEntity("CSNM002", "CU002_CourseCreateSinNotaSinMatriz"));
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
        public async Task TC001CreacionDeCursoSinMatrizSinNota()
        {
            await myCommons.SeleniumSetup(wait, driver, urlTest, urlScreen);
            Random rnd = new Random();

            for (int i = 0; i < 2; i++)
            {
                cantidad = rnd.Next(1, 10);
                // Espera a que el botón con ID que contiene 'btnAdd' sea clickeable y haz clic en él
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAdd")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAdd"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Ingresando a la modal de Creación del Curso.");

                Thread.Sleep(1000);
               myCommons.CreateCourseWithoutAndMatrix(wait, driver, js, i, seleniumEntities);


                myCommons.CourseByCode(wait, driver, js, seleniumEntities[i].Code);
                Thread.Sleep(2000);
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
                myCommons.Log.Information($"{DateTime.Now}: Buscando el Curso creado  " + seleniumEntities[i].Code);

                // Los asserts validan que el test case, realmente inserto las aulas
                string displayedCode = driver.FindElement(By.Id("dvCourseCode")).Text;
                Assert.AreEqual(seleniumEntities[i].Code, displayedCode, "El código del Curso no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Código del Curso  " + seleniumEntities[i].Code);


                string displayedDescription = driver.FindElement(By.Id("dvCourseName")).Text;
                Assert.AreEqual(seleniumEntities[i].Description, displayedDescription, "Nombre del curso no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Nombre del Curso  " + seleniumEntities[i].Description);

              
                string displayedTypeTrainingName = driver.FindElement(By.Id("dvTypeTrainingName")).Text;
                Assert.AreEqual("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico. Duración al menos dos horas.", displayedTypeTrainingName, "Tipo de Formación no coincide.");
                myCommons.Log.Information($"{DateTime.Now}: Validando el Assert por Tipo de Formación " + seleniumEntities[i].Description);


                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();



                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnEdit")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnEdit"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Click en el botón Editar");
                Thread.Sleep(1000);

                // Asegurarse de que el toggle está presente
                var noteRequired = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));
                Assert.IsFalse(noteRequired.Selected, "El toggle '¿Requiere Nota?' debería estar en NO.");

                var cyclesRefreshment = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbCyclesRefreshment")));
                Assert.IsFalse(cyclesRefreshment.Selected, "El toggle '¿Refrescamiento de Ciclos?' debería estar en NO.");

                var forMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));
                Assert.IsFalse(forMatrix.Selected, "El toggle '¿Es para Matriz?' debería estar en NO.");


                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("btnCancel")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnCancel"))).Click();
                myCommons.Log.Information($"{DateTime.Now}: Click en el botón Cancelar");

            }
        }
    }
}