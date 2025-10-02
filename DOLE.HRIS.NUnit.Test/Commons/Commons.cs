using DOLE.HRIS.NUnit.Test;
using DOLE.HRIS.NUnit.Test.Utilities;
using DOLE.HRIS.Shared.Entity.ADAM;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DOLE.HRIS.Application.Automation.Selenium
{
    public class Commons
    {
        private object ScreenshotImageFormat;

        //Permite hacer el login e ingresar la división de Costa Rica y luego a la pantalla respectiva a validar 


        public ILogger Log { get; private set; }

        public Commons()
        {
            Log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            string logPath = Path.GetFullPath("logs/test-log.txt");

        }

        public void log()
        {
            Log.Information("Log inicializado correctamente.");
        }



        public void TakeScreenshot(IWebDriver driver, string nombreArchivo)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string nombreConFecha = $"{nombreArchivo}_{timestamp}.png";

                // 🗂 Ruta fija en red o local
                string rutaFija = @"\\DTP-MEU-DEV023\Screenshots";
                Directory.CreateDirectory(rutaFija);
                string rutaCompletaFija = Path.Combine(rutaFija, nombreConFecha);
                screenshot.SaveAsFile(rutaCompletaFija);
                Log.Information($"✅ Screenshot guardado en ruta fija: {rutaCompletaFija}");

                // 📦 Ruta para Azure DevOps (artefactos)
                string rutaDevOps = Environment.GetEnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY");
                if (string.IsNullOrEmpty(rutaDevOps))
                {
                    // Si está corriendo localmente o fuera de DevOps, usar una ruta temporal
                    rutaDevOps = Path.Combine(Path.GetTempPath(), "Screenshots");
                }
                else
                {
                    // En DevOps, crear subcarpeta "Screenshots"
                    rutaDevOps = Path.Combine(rutaDevOps, "Screenshots");
                }

                Directory.CreateDirectory(rutaDevOps);
                string rutaCompletaDevOps = Path.Combine(rutaDevOps, nombreConFecha);
                screenshot.SaveAsFile(rutaCompletaDevOps);
                Log.Information($"✅ Screenshot guardado para DevOps: {rutaCompletaDevOps}");
            }
            catch (Exception ex)
            {
                Log.Error($"❌ Error al tomar el screenshot: {ex.Message}");
            }
        }






        public async Task SeleniumSetup(WebDriverWait wait, IWebDriver driver, string urlTest, string URLScreen)
        {
            Logger log;
            var colorOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Inicio");
            log = new LoggerConfiguration()
                   .WriteTo.Console()
                   .CreateLogger();
            log.Information("Inicia el test");
            string logPath = Path.GetFullPath("logs/test-log.txt");
            string TESTScreen = URLScreen;
            string fullUrl = urlTest;
            if (!await this.IsUrlAccessible(fullUrl))
            {
                log.Error($"{DateTime.Now}: La URL {fullUrl} no está accesible.");

                return;
            }

            // Navega a la URL de prueba
            driver.Navigate().GoToUrl(fullUrl);


            // Obtener y registrar el título de la página
            string pageTitle = driver.Title;
            log.Information($"{DateTime.Now}: El título de la página es: {pageTitle}");

            driver.Manage().Window.Size = new System.Drawing.Size(1945, 1020);



            // wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            // Verificar la existencia del elemento 'workingdivisionmodal'
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("workingdivisionmodal")));



            bool isWorkingDivisionModalPresent = false;
            try
            {
                isWorkingDivisionModalPresent = driver.FindElement(By.Id("workingdivisionmodal")).Displayed;
            }
            catch (NoSuchElementException)
            {
                isWorkingDivisionModalPresent = false;
            }

            if (isWorkingDivisionModalPresent)
            {
                // Si el modal de división de trabajo está presente
                try
                {
                    Log.Information($"{DateTime.Now}: La página Default.aspx cargó correctamente y el modal de división de trabajo está activo.");

                    wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".filter-option-inner-inner")));
                    log.Information($"{DateTime.Now}: La página Default.aspx cargó correctamente.");


                }
                catch (WebDriverTimeoutException ex)
                {

                    log.Information($"{DateTime.Now}: La página Default.aspx no cargó correctamente. Excepción: {ex.Message}");
                    return;
                }

                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".filter-option-inner-inner"))).Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("li:nth-child(2) .text"))).Click();
                {
                    var dropdown = driver.FindElement(By.Id("ctl00_cntBody_cboUserDivisions"));
                    wait.Until(ExpectedConditions.ElementToBeClickable(dropdown.FindElement(By.XPath("//option[. = 'División de Costa Rica']")))).Click();
                    log.Information($"{DateTime.Now}: Seleccionar la División de Costa Rica.");


                }

                Thread.Sleep(500);

                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[class='modal-content']")));

                wait.Until(drivr =>
                {
                    try
                    {
                        return driver.FindElement(By.CssSelector("a.btn.btn-primary.btnAjaxAction")).Displayed;

                    }
                    catch
                    {
                        return false;
                    }
                });

                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='btn btn-primary btnAjaxAction']"))).Click();


                driver.Navigate().GoToUrl(urlTest + URLScreen);

            }
            else
            {
                // Si el modal de división de trabajo no está presente
                try
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[class='modal-content']")));
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='btn btn-primary btnAjaxAction']"))).Click();
                    driver.Navigate().GoToUrl(urlTest + URLScreen);

                    log.Information($"{DateTime.Now}: Ingresando a la pantalla respectiva." + URLScreen);

                }
                catch (WebDriverTimeoutException ex)
                {
                    log.Error($"{DateTime.Now}: No se pudo encontrar el botón de selección de división de trabajo. Excepción: {ex.Message}");
                    Assert.Fail("No se pudo encontrar el botón de selección de división de trabajo.");
                    return;
                }
            }
        }



        public async Task<bool> IsUrlAccessible(string url)
        {
            var log = new LoggerConfiguration()
                           .WriteTo.Console()
                           .CreateLogger();
            var handler = new HttpClientHandler
            {
                Credentials = CredentialCache.DefaultCredentials // Usa las credenciales del sistema
            };

            using (HttpClient client = new HttpClient(handler))
            {

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    log.Information($"{DateTime.Now}: Código de estado HTTP para {url}: {response.StatusCode}, {response.ReasonPhrase}");
                    return response.IsSuccessStatusCode;
                }
                catch (HttpRequestException e)
                {
                    log.Error($"{DateTime.Now}: Error al acceder a la URL: {e.Message}");
                    return false;
                }
            }
        }

        //Permite la Creación de Registros de Aulas 
        public void AgregarDatosAula(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, int cantidad, List<SeleniumEntity> seleniumEntities)
        {

            Serilog.Log.Information($"{DateTime.Now}: Ingresando modal de creación de Aulas");
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter")));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Oficinas Guápiles - Interno");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomCode");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode")).SendKeys(seleniumEntities[posicion].Code);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomDescription");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).SendKeys(seleniumEntities[posicion].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtCapacity"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtCapacity");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).SendKeys(cantidad.ToString());


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtComments"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtComments");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).SendKeys("Selenium");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-6 .active"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Creando el registro de aulas exitosamente");

            Thread.Sleep(1500);


            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));



            Thread.Sleep(1500);
        }

        public void UpdateClassroom(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, int cantidad, List<SeleniumEntity> seleniumEntitiesEdited)
        {
            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter"))));
            IWebElement fieldElement = driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter"));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "El Bosque - Interno");
            Serilog.Log.Information($"{DateTime.Now}: Modificando el Centro de Capacitación  \"El Bosque - Interno ");


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomDescription");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).SendKeys(seleniumEntitiesEdited[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}: Modificando la Descripción   " + seleniumEntitiesEdited[posicion].Description);


            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtCapacity");

            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).SendKeys(cantidad.ToString());
            Serilog.Log.Information($"{DateTime.Now}: Modificando la Capacidad   " + cantidad.ToString());


            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtComments");

            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).SendKeys("Selenium");
            Serilog.Log.Information($"{DateTime.Now}: Modificando el campo de Comentario Selenium ");


            driver.FindElement(By.Id("ctl00_cntBody_btnAccept")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botón de Aceptar para aplicar la modificación ");

            Thread.Sleep(1500);



            wait.Until(drv =>
            {
                try
                {
                    var elementMessage = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return elementMessage.Displayed && elementMessage.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
        }

        public void AgregarAula(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int cantidad, List<SeleniumEntity> seleniumEntities)
        {

            Serilog.Log.Information($"{DateTime.Now}: Ingresando modal de creación de Aulas");
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter")));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Oficinas Guápiles - Interno");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomCode");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode")).SendKeys(seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomDescription");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).SendKeys(seleniumEntities[0].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtCapacity"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtCapacity");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).SendKeys(cantidad.ToString());


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtComments"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtComments");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).SendKeys("Selenium");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-6 .active"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Creando el registro de aulas exitosamente");

            Thread.Sleep(1500);


            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));



            Thread.Sleep(1500);
        }


        public void ReactivarAula(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int cantidad, List<SeleniumEntity> seleniumEntities)
        {

            Serilog.Log.Information($"{DateTime.Now}: Ingresando modal de creación de Aulas");
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ucClassroom_cboTrainingCenter")));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Oficinas Guápiles - Interno");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomCode");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomCode")).SendKeys(seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtClassroomDescription");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtClassroomDescription")).SendKeys(seleniumEntities[0].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtCapacity"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtCapacity");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtCapacity")).SendKeys(cantidad.ToString());


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ucClassroom_txtComments"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_ucClassroom_txtComments");
            driver.FindElement(By.Id("ctl00_cntBody_ucClassroom_txtComments")).SendKeys("Selenium");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-6 .active"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Creando el registro de aulas exitosamente");


        }

        //Permite buscar  Registros de Aulas en el grid 
        public void BuscarAula(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            Serilog.Log.Information($"{DateTime.Now}:Filtrando el aula a consultar");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtClassroomCodeFilter")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtClassroomCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtClassroomCodeFilter");
            driver.FindElement(By.Id("ctl00_cntBody_txtClassroomCodeFilter")).SendKeys(valor);



            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Valor buscado" + valor);


            //Thread.Sleep(1500);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }
        public void DeletedClassroom(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvClassroomCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvClassroomCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));

            Thread.Sleep(1500);


        }

        public void AgregarDatosCentros(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            Serilog.Log.Information($"{DateTime.Now}: Ingresando modal de creación de Centros");
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingCenterCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterCode"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código del Centro " + seleniumEntities[posicion].Code);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingCenterDescription")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterDescription"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando la descripción del centro " + seleniumEntities[posicion].Description);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var locationDropdown = new SelectElement(wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_cboPlaceLocation"))));
            locationDropdown.SelectByText("Interno");
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando Lugar" + "Interno");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando la opción de Habilitado con SI");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Clcik en en boton de Aceptar para guardar el registro");


            Thread.Sleep(1500);


            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));



            Thread.Sleep(1500);
        }



        public void AddTrainginCenters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            Serilog.Log.Information($"{DateTime.Now}: Ingresando modal de creación de Centros");
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingCenterCode")));
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código del Centro " + seleniumEntities[0].Code);


            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando la descripción del centro " + seleniumEntities[0].Description);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //IWebElement fieldElement = driver.FindElement(By.Id("ctl00_cntBody_cboPlaceLocation"));           
            //Utilities.SelectOptionByDropDownList(js, fieldElement, "Internal");
            var locationDropdown = new SelectElement(wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_cboPlaceLocation"))));
            locationDropdown.SelectByText("Interno");
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando Lugar" + "Interno");


            driver.FindElement(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal")).Click();
            driver.FindElement(By.CssSelector(".col-sm-8 .active")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando la opción de Habilitado con SI");



            driver.FindElement(By.Id("ctl00_cntBody_btnAccept")).Click();
            Serilog.Log.Information($"{DateTime.Now}:Clcik en en boton de Aceptar para guardar el registro");


            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div[class='alert alert-autocloseable-msg alert-success'")));

            Thread.Sleep(1500);
        }



        public void EditDataWithCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntitiesEdited)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),  // Tiempo máximo de espera
                PollingInterval = TimeSpan.FromMilliseconds(500)  // Cada cuánto revisa la condición
            };
            fluentWait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));  // Ignora errores de referencia obsoleta

            Thread.Sleep(1000);

            IWebElement element = fluentWait.Until(drv => drv.FindElement(By.Id("dvTrainingCenterCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvTrainingCenterCode"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Se espera que aparezca el elemento" + element);

            Thread.Sleep(1000);

            //driver.FindElement(By.CssSelector("tr:nth-child(1) > .ItemSelector:nth-child(2)")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_btnEdit")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el boton Editar");


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingCenterDescription"))));
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingCenterDescription");
            //driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).SendKeys(seleniumEntitiesEdited.Description);
            js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = arguments[1];",
                driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")),
                "Escripttest@__:script:LPPñ09987:ÑP@__:script:.LPPñ:ÑP");

            Serilog.Log.Information($"{DateTime.Now}: Modificando la Descripción con caracteres.  " + seleniumEntitiesEdited[0].Description);

            IWebElement fieldElement = driver.FindElement(By.Id("ctl00_cntBody_cboPlaceLocation"));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Interno");
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando Lugar" + "Interno");


            driver.FindElement(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal")).Click();
            driver.FindElement(By.CssSelector(".col-sm-8 .active")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Seleccionar la opción de Habilitar registro");


            driver.FindElement(By.Id("ctl00_cntBody_btnAccept")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botón de Aceptar");


            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            var alert = driver.FindElements(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
            if (alert.Count > 0)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            }
            else
            {
                Serilog.Log.Information("No se encontró la alerta de éxito");
            }
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Thread.Sleep(1000);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        public void UpdateCenters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntitiesEdited)
        {
            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingCenterDescription"))));
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingCenterDescription");

            //driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).Click();
            //driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).SendKeys(seleniumEntitiesEdited[posicion].Description);
            //Serilog.Log.Information($"{DateTime.Now}: Modificando Descripción del Centro:.  " + seleniumEntitiesEdited[posicion].Description);


            // Obtener la referencia del elemento
            var descripcionInput = driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription"));

            // Forzar foco si quieres
            descripcionInput.Click();

            // Usar JavaScript para asignar el valor
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].value = arguments[1];",
                descripcionInput,
                seleniumEntitiesEdited[posicion].Description
            );

            Serilog.Log.Information($"{DateTime.Now}: Modificando Descripción del Centro: {seleniumEntitiesEdited[posicion].Description}");







            IWebElement fieldElement = driver.FindElement(By.Id("ctl00_cntBody_cboPlaceLocation"));
            Utilities.SelectOptionByDropDownList(js, fieldElement, "Interno");

            driver.FindElement(By.Id("ctl00_cntBody_btnAccept")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botón de Aceptar  ");





            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            var alert = driver.FindElements(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
            if (alert.Count > 0)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            }
            else
            {
                Serilog.Log.Information("No se encontró la alerta de éxito");
            }
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

            Thread.Sleep(1500);
        }


        public void BuscarCentros(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            Serilog.Log.Information($"{DateTime.Now}:Filtrando el centro a consultar");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingCenterCodeFilter")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingCenterCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterCodeFilter")));
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterCodeFilter")).SendKeys(valor);
            Serilog.Log.Information($"{DateTime.Now}: Buscar el Código del Centro " + valor);


            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Valor buscado" + valor);


            //Thread.Sleep(1500);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void BuscarCentroDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingCenterDescriptionFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterDescriptionFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingCenterCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingCenterDescriptionFilter");
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescriptionFilter")).SendKeys(valor);

            //driver.FindElement(By.Id("ctl00_cntBody_btnSearch")).Click();

            //Thread.Sleep(1000);


            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void BorrarCentroCapacitacion(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTrainingCenterCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvTrainingCenterCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnDelete")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);


            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));



            Thread.Sleep(1500);
        }

        public void AgregarCentro(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            // driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterCode")).SendKeys(seleniumEntities.Code);

            Serilog.Log.Information($"{DateTime.Now}: Ingresando modal de creación de Centros");
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingCenterCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterCode"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código del Centro " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingCenterDescription"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingCenterDescription")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Cambiando Lugar" + "Externo");


            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var locationDropdown = new SelectElement(wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_cboPlaceLocation"))));
            locationDropdown.SelectByText("Interno");
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando Lugar" + "Interno");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando la opción de Habilitado con SI");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Clcik en en boton de Aceptar para guardar el registro");

            Thread.Sleep(1500);


            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));



            Thread.Sleep(1500);


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }


        public void AgregarPropositosEdicion(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Propósito {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Propósito {seleniumEntities[0].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Propósito");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Propósito ");


            Thread.Sleep(1500);


            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
            wait.Until(drv => drv.PageSource.Contains("La operación fue realizada exitosamente"));



            Thread.Sleep(1500);

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }

        public void AgregarPropositos(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Propósito {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Propósito {seleniumEntities[posicion].Description}");


            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chkSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            // Confirmar toggle activado
            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
                Thread.Sleep(300);
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");

            // Confirmar que el botón está habilitado
            var acceptBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept")));
            Assert.IsTrue(acceptBtn.Enabled, "El botón Aceptar no está habilitado.");

            // Forzar clic con JavaScript
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", acceptBtn);
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para guardar el nuevo propósito.");

            // Esperar a que el div aparezca (con texto correcto)
            // TEMPORAL: Espera forzada por comportamiento intermitente en carga de alertas (revisar en futuro)
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));




            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }
        public void AgregarPropositoaDuplicar(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Propósito {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Propósito {seleniumEntities[posicion].Description}");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Propósito");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Propósito ");



            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }


        public void BuscarPropositoPorCodigo(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingProgramCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingProgramCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingProgramNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramCodeFilter")).SendKeys(valor);


            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando propósito " + valor);

            //Thread.Sleep(1000);
            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);


        }

        public void SearchPurposeByDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingProgramNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingProgramNameFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingProgramNameFilter");
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramNameFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);


            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }



        public void AgregarProposito(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramCode"))).Click();


            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingProgramName")));
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramName")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando la Descripción.  " + seleniumEntities[0].Description);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            // TEMPORAL: Espera forzada por comportamiento intermitente en carga de alertas (revisar en futuro)
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));




            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);


        }

        public void EditPurposes(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingProgramName")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingProgramName"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingProgramName");
            Thread.Sleep(500);
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingProgramName")).SendKeys(seleniumEntities[0].Description);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }


        public void DeletePurpose(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTrainingProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvTrainingProgramCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



            Thread.Sleep(1500);

        }


        public void TypeTrainingCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Tipo de Formación {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Tipo de Formación {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Tipo de Formación");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Tipo de Formación ");



            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }

        public void TypeTrainingCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Tipo de Formación {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Tipo de Formación {seleniumEntities[0].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Tipo de Formación");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Tipo de Formación ");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));




            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }

        public void ReactivedTypeTraining(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Tipo de Formación {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Tipo de Formación {seleniumEntities[0].Description}");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Tipo de Formación");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Tipo de Formación ");


        }

        public void EditTrainingTypeOverwriteDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingCodeEdit");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Igresando el Código " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingNameEdit");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Igresando la Descripción " + seleniumEntities[0].Description);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar la creación ");



        }



        public void TypeTrainingDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeEdit")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Tipo de Formación {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Tipo de Formación {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Tipo de Formación");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Tipo de Formación ");

        }

        public void SearchTrainingTypeByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_TypeTrainingCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingCodeFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void SearchTrainingTypeByName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_TypeTrainingCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameFilter")).SendKeys(valor);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            Thread.Sleep(1000);
        }
        public void EditTrainingType(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_TypeTrainingNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_TypeTrainingNameEdit");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_TypeTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_TypeTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void DeleteTrainingType(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvTypeTrainingCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvTypeTrainingCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();


            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        }

        public void SchoolTrainingCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Escuela de Entrenamiento {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de  la Escuela de Entrenamiento {seleniumEntities[posicion].Description}");



            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");





            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        }

        public void SchoolTrainingCreateCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit"))).Click();


            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")));
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el nombre de la Escuela.  " + seleniumEntities[0].Description);


            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void SearchSchoolByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_SchoolTrainingCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingCodeFilter")).SendKeys(valor);



            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Escuela " + valor);

            //Thread.Sleep(2000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void EditSchoolTraining(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingNameEdit"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingNameEdit");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingNameEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }


        public void SchoolTrainingDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Tipo de Formación {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción de la Escuela {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar la Escuela");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nueva Escuela ");

        }

        public void EditSchoolTrainingOverwriteName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingCodeEdit");
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Igresando el Código " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")));
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingNameEdit");
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Igresando el nombre " + seleniumEntities[0].Description);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar la creación ");



        }


        public void SearchSchoolByDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_SchoolTrainingNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingNameFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_SchoolTrainingNameFilter");
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);


            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por descripción " + valor);

            Thread.Sleep(1000);


        }


        public void DeleteSchoolTraining(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvSchoolTrainingCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvSchoolTrainingCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnDelete")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

        }


        public void ReactivedSchoolTraining(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Escuela {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_SchoolTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_SchoolTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción de la Escuela {seleniumEntities[0].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Tipo de Formación");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de la Escuela ");


        }

        public void ThematicAreaCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Area de Temática {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre del Area temática {seleniumEntities[posicion].Description}");


            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        }


        public void ThematicArealByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtThematicAreaCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaCodeFilter");
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCodeFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Area Temática " + valor);

            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por código " + valor);

            Thread.Sleep(1000);
        }


        public void ThematicAreaDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Area Temática {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Area Tematica {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Area");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nueva Area ");
            Thread.Sleep(1000);

        }

        public void ThematicAreaCreateCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCode"))).Click();


            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaName")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaName"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el nombre del Area.  " + seleniumEntities[0].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            Thread.Sleep(1500);


            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
        }
        public void SearchThematicAreaByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtThematicAreaCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCodeFilter")));

            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCodeFilter")).SendKeys(valor);

            Thread.Sleep(1000);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por código " + valor);

            Thread.Sleep(1000);









        }

        public void EditThematicArea(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaName")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaName"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaName");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).SendKeys(seleniumEntities[0].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);


            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void SearchThematicAreaByDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtThematicAreaNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaNameFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaNameFilter");
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaNameFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);


            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por descripción " + valor);

            Thread.Sleep(1000);
        }


        public void DeleteThematicArea(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvThematicAreaCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvThematicAreaCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void ReactivedThematicArea(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Area {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Area {seleniumEntities[0].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#ctl00_cntBody_uppDialogControl .form-horizontal"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".col-sm-8 .active"))).Click();
            Serilog.Log.Information($"{DateTime.Now}:Click en el boton de activar el Tipo de Formación");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de la Escuela ");


        }

        public void EditThematicAreaOverwriteName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtThematicAreaCode"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaCode");
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Igresando el Código " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtThematicAreaName")));
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtThematicAreaName");
            driver.FindElement(By.Id("ctl00_cntBody_txtThematicAreaName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Igresando el nombre " + seleniumEntities[0].Description);

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar la creación ");



        }

        public void CourseCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre del curso {seleniumEntities[posicion].Description}");


            // Esperar a que el select sea visible
            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));

            // Verificar si el toggle ya está en "Sí" (checked)
            if (!inputCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbNoteRequired + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));
            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("5");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("20");


            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");
            Thread.Sleep(1500);



            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        }

        public void CourseCreateWithGradeMatrixCycle(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre del curso {seleniumEntities[posicion].Description}");


            // Esperar a que el select sea visible
            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));

            // Verificar si el toggle ya está en "Sí" (checked)
            if (!inputCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbNoteRequired + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }


            var toggleCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbCyclesRefreshment")));
            if (!toggleCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbCyclesRefreshment + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            //Selección del check de ¿Es para matriz? en SI
            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));
            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("5");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("20");



            //Seleccionar  SI en ¿Habilitado?
            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");
            Thread.Sleep(1500);



            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        }


        public void CourseCreateWithoutGrade(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre del curso {seleniumEntities[posicion].Description}");


            // Esperar a que el select sea visible
            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));


            // Esperar y obtener el div principal del toggle
            var toggleDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.toggle")));

            // Obtener el atributo "class"
            string toggleClass = toggleDiv.GetAttribute("class");

            // Validar que incluya "off" (lo cual indica que está en estado "No")
            Assert.IsTrue(toggleClass.Contains("off"), "El toggle no está en estado 'No'.");
            Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'NO' " + toggleClass);



            var toggleCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbCyclesRefreshment")));
            if (!toggleCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbCyclesRefreshment + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            //Selección del check de ¿Es para matriz? en SI
            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));
            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("5");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("20");



            //Seleccionar  SI en ¿Habilitado?
            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbSearchEnabled"));
                toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");
            Thread.Sleep(1500);



            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        }

        public void CourseByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtCourseCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCodeFilter")));

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCodeFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Curso " + valor);

            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por código " + valor);

            Thread.Sleep(1000);
        }

        public void SearchCourseByDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtCourseNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseNameFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseNameFilter");
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseNameFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);


            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por descripción " + valor);

            Thread.Sleep(1000);
        }


        public void CourseDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre del curso {seleniumEntities[posicion].Description}");

            // Esperar a que el select sea visible
            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));

            // Verificar si el toggle ya está en "Sí" (checked)
            if (!inputCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbNoteRequired + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));
            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("5");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("20");


            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");
            Thread.Sleep(1000);


        }
        public void CourseCreateCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();


            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseName")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el nombre del Curso.  " + seleniumEntities[0].Description);

            // Esperar a que el select sea visible
            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));

            // Verificar si el toggle ya está en "Sí" (checked)
            if (!inputCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbNoteRequired + div .toggle-on")
                ));

                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);

                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));

            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));

                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);

                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("20");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("50");


            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
           By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }


        public void EditCourse(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseName")));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseName");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el nombre del Curso.  " + seleniumEntities[0].Description);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTypeTrainingEdit"))).Click();

            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("5");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("5", select.SelectedOption.GetAttribute("value"));

            }


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseCostByParticipant");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("80");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseDuration");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("90");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));

            // Verificar si el toggle ya está en "Sí" (checked)
            if (!inputCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbNoteRequired + div .toggle-on")
                ));

                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);

                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));

            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));

                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);

                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtMaxDaysTrainEdit");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("25");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtDaysRenewCourseEdit");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("55");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }

        public void DeleteCourse(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCourseCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();


            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);


            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void ReactivedCourse(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Curso {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Curso {seleniumEntities[0].Description}");


            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");

            Thread.Sleep(500);
            // Asegurarse de que el toggle está presente
            var inputCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbNoteRequired")));

            // Verificar si el toggle ya está en "Sí" (checked)
            if (!inputCheckbox.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbNoteRequired + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }

            var inputCheckboxMatrix = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chbForMatrix")));
            if (!inputCheckboxMatrix.Selected)
            {
                // Encontrar el botón "Sí"
                var botonSi = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#ctl00_cntBody_chbForMatrix + div .toggle-on")
                ));
                // Hacer clic mediante JavaScript por si Selenium no puede
                js.ExecuteScript("arguments[0].click();", botonSi);
                Serilog.Log.Information($"{DateTime.Now}: Toggle 'Nota Requerida' activado en 'Sí' con JavaScript");
            }



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtMaxDaysTrainEdit")).SendKeys("5");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtDaysRenewCourseEdit")).SendKeys("20");


            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo el Curso  ");
            Thread.Sleep(1500);


        }


        public void CreateCourseWithoutAndMatrix(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre del curso {seleniumEntities[posicion].Description}");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTypeTrainingEdit"))).Click();


            var dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTypeTrainingEdit")));

            {

                // Crear el objeto SelectElement para manejar el <select>
                var select = new SelectElement(dropdownElement);

                // Seleccionar por valor
                select.SelectByValue("4");
                //select.SelectByText("Seminario o Curso: Sesión de interacción aprendiz-facilitador sobre tema específico.  Duración al menos dos horas.");

                // Validar que fue seleccionado correctamente
                Assert.AreEqual("4", select.SelectedOption.GetAttribute("value"));

            }

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseCostByParticipant")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseCostByParticipant"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseCostByParticipant")).SendKeys("60");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtCourseDuration")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseDuration"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtCourseDuration")).SendKeys("50");


            // Clic sobre el handle
            IWebElement toggleHandle = driver.FindElement(By.CssSelector("#ctl00_cntBody_chbSearchEnabled ~ div.toggle-group button.toggle-handle"));

            toggleHandle.Click();


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro de Nuevo de Curso");

            Thread.Sleep(1500);

            try
            {
                // Espera a que aparezca el mensaje de éxito (alert-success)
                var successAlert = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

                Serilog.Log.Information($"{DateTime.Now}: Mensaje de éxito visible: {successAlert.Text}");

                // Espera a que desaparezca
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

                Serilog.Log.Information($"{DateTime.Now}: El mensaje de éxito desapareció correctamente.");
            }
            catch (WebDriverTimeoutException ex)
            {
                Serilog.Log.Error($"{DateTime.Now}: No se encontró o no desapareció el mensaje de éxito. Detalle: {ex.Message}");
                Assert.Fail("No se encontró o no desapareció el mensaje de éxito después de guardar el curso.");
            }
            Thread.Sleep(1500);

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        }
        public void SearchCourseByState(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtCourseNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtCourseNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseNameFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtCourseNameFilter");
            //driver.FindElement(By.Id("ctl00_cntBody_txtCourseNameFilter")).SendKeys(valor);

            var estadoCursoDropdown = new SelectElement(
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_cboCourseState"))));

            // Selecciona "Habilitado"
            estadoCursoDropdown.SelectByText("Habilitado");

            // Assert para validar que fue correctamente seleccionada
            Assert.AreEqual("Habilitado", estadoCursoDropdown.SelectedOption.Text, "El filtro de estado no se aplicó correctamente.");


            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre + mas filtro de Estado" + valor);


            //Thread.Sleep(1000);
            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por código " + valor);

            Thread.Sleep(1000);
        }

        public IWebElement GetToggleHandle(WebDriverWait wait)
        {
            return wait.Until(driver =>
                        driver.FindElement(By.CssSelector("input#ctl00_cntBody_chbExternalsSearchEnabled"))
                               .FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]//button[contains(@class, 'toggle-handle')]"))
                    );
        }


        public void CreateVariousExternalPersons(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {
            SelectBootstrapTrainerOptionByText(driver, wait, "ctl00_cntBody_cboTrainerType", "Persona externa");


            Thread.Sleep(1000);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerCode")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[posicion].Code);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerName")));
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el Nombre.  " + seleniumEntities[posicion].Description);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")).SendKeys("60452623");

            // Solo hacer clic si aún está desactivado

            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbExternalsSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        }


        public void CreateTrainer(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities, string valor)
        {

            SelectBootstrapTrainerOptionByText(driver, wait, "ctl00_cntBody_cboTrainerType", valor);


            Thread.Sleep(1000);



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[0].Code);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerName")));
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el Nombre.  " + seleniumEntities[0].Description);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")).SendKeys("60452623");

            // Solo hacer clic si aún está desactivado

            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbExternalsSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        }

        public void EditTrainer(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerName")));
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_txtExternalsTrainerName");


            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el Nombre.  " + seleniumEntities[0].Description);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")));
            Utilities.CleanFields(js, "ctl00_cntBody_txtExternalsTrainerTelephone");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")).SendKeys("78901245");

            // Solo hacer clic si aún está desactivado

            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbExternalsSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }






        public void DuplicateTrainer(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities, string valor)
        {

            SelectBootstrapTrainerOptionByText(driver, wait, "ctl00_cntBody_cboTrainerType", valor);


            Thread.Sleep(1000);



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el código.  " + seleniumEntities[0].Code);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerName")));
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando el Nombre.  " + seleniumEntities[0].Description);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtExternalsTrainerTelephone")).SendKeys("60452623");

            // Solo hacer clic si aún está desactivado

            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbExternalsSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            Thread.Sleep(1500);





        }

        //public void CreateTrainerEmployee(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        //{


        //    SelectBootstrapTrainerOptionByText(driver, wait, "ctl00_cntBody_cboTrainerType", "Empleado");


        //    Thread.Sleep(1000);

        //    // Esperar visible
        //    var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchEmployees")));

        //    // Esperar clickeable y dar foco
        //    wait.Until(ExpectedConditions.ElementToBeClickable(searchInput)).Click();

        //    // Usar JavaScript para poner el texto
        //    ((IJavaScriptExecutor)driver).ExecuteScript(
        //        "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input'));",
        //        searchInput,
        //        seleniumEntities[0].Description
        //    );
        //    searchInput.SendKeys(Keys.Enter);


        //    // Log
        //    Serilog.Log.Information($"{DateTime.Now}: Buscando empleados: {seleniumEntities[0].Description}");

        //    Thread.Sleep(2000);
        //    var buttonclick = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_rptEmployees_ctl01_btnAddEmployee")));
        //    wait.Until(ExpectedConditions.ElementToBeClickable(buttonclick)).Click();

        //    //driver.FindElement(By.Id("ctl00_cntBody_rptEmployees_ctl01_btnAddEmployee")).Click();
        //    Serilog.Log.Information($"{DateTime.Now}: Seleccionando al Empleado ");


        //    // Valida el label de Nombre
        //    var labelNombre = driver.FindElement(By.CssSelector("label[for='ctl00_cntBody_txtInternalEmployeeTrainerName']"));
        //    Assert.AreEqual("Nombre:", labelNombre.Text.Trim(), "El label de nombre no coincide.");

        //    // Valida el label de Código
        //    var labelCodigo = driver.FindElement(By.CssSelector("label[for='ctl00_cntBody_txtInternalEmployeeTrainerCode']"));
        //    Assert.AreEqual("Código de instructor:", labelCodigo.Text.Trim(), "El label de código de instructor no coincide.");


        //    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtInternalEmployeeTrainerTelephone")));
        //    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtInternalEmployeeTrainerTelephone"))).Click();
        //    driver.FindElement(By.Id("ctl00_cntBody_txtInternalEmployeeTrainerTelephone")).SendKeys("60452623");

        //    // Solo hacer clic si aún está desactivado

        //    var toggleHandle = wait.Until(d =>
        //    {
        //        var input = d.FindElement(By.Id("ctl00_cntBody_chbInternalEmployeeSearchEnabled"));
        //        var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
        //        return toggleDiv.FindElement(By.ClassName("toggle-handle"));
        //    });

        //    if (toggleHandle.GetAttribute("aria-checked") == "false")
        //    {
        //        toggleHandle.Click();
        //    }
        //    Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");



        //    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
        //    Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
        //    Thread.Sleep(1500);

        //    wait.Until(drv =>
        //    {
        //        try
        //        {
        //            var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
        //            return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
        //        }
        //        catch (NoSuchElementException)
        //        {
        //            return false;
        //        }
        //    });

        //    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
        //        By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        //}


        public void CreateTrainerEmployee(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {


            SelectBootstrapTrainerOptionByText(driver, wait, "ctl00_cntBody_cboTrainerType", "Empleado");


            Thread.Sleep(1000);

            //var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchEmployees")));

            //// Darle foco
            //wait.Until(ExpectedConditions.ElementToBeClickable(searchInput)).Click();

            //// Limpiar y escribir
            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = '';", searchInput);
            //searchInput.SendKeys(seleniumEntities[0].Description);


            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtSearchEmployees")));

            // Click para enfocar
            searchInput.Click();

            // Borrar contenido con JavaScript (más confiable)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = '';", searchInput);

            // Escribir el texto uno por uno (simula usuario real)
            string valor = seleniumEntities[0].Description;
            foreach (char c in valor)
            {
                searchInput.SendKeys(c.ToString());
                Thread.Sleep(50); // pequeña pausa para simular tipeo humano
            }

            // Disparar eventos necesarios (esto es crítico para que funcione en campos dinámicos)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].dispatchEvent(new Event('input'));", searchInput);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", searchInput);

            // Alternativamente, presionar Enter si el campo lo requiere
            searchInput.SendKeys(Keys.Enter);

            // Esperar a que cargue resultados (puedes ajustar esto según lo que ves en la interfaz)
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tableBodySelectEmployee")));


            // Opción A - Probar Enter
            //searchInput.SendKeys(Keys.Enter);
            js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
    var input = arguments[0];
    var event = new KeyboardEvent('keydown', { key: 'Enter', code: 'Enter', keyCode: 13, which: 13 });
    input.dispatchEvent(event);", searchInput);


            // Pausar un poco a que cargue
            Thread.Sleep(1000);

            // Verificar si se llenó la tabla
            bool tablaVisible = driver.FindElements(By.Id("ctl00_cntBody_rptEmployees_ctl01_btnAddEmployee")).Count > 0;

            if (!tablaVisible)
            {
                // Opción B - Probar pérdida de foco
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].blur();", searchInput);
                Thread.Sleep(1000);
            }

            // Verificar otra vez si apareció la tabla
            tablaVisible = driver.FindElements(By.Id("ctl00_cntBody_rptEmployees_ctl01_btnAddEmployee")).Count > 0;

            if (!tablaVisible)
            {
                // Opción C - Forzar __doPostBack (muy común en ASP.NET)
                ((IJavaScriptExecutor)driver).ExecuteScript("__doPostBack('ctl00$cntBody$txtSearchEmployees','');");
                Thread.Sleep(2000);
            }

            // Ahora sí esperar el botón
            var buttonclick = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_rptEmployees_ctl01_btnAddEmployee")));
            wait.Until(ExpectedConditions.ElementToBeClickable(buttonclick)).Click();

            //driver.FindElement(By.Id("ctl00_cntBody_rptEmployees_ctl01_btnAddEmployee")).Click();
            Serilog.Log.Information($"{DateTime.Now}: Seleccionando al Empleado ");


            // Valida el label de Nombre
            var labelNombre = driver.FindElement(By.CssSelector("label[for='ctl00_cntBody_txtInternalEmployeeTrainerName']"));
            Assert.AreEqual("Nombre:", labelNombre.Text.Trim(), "El label de nombre no coincide.");

            // Valida el label de Código
            var labelCodigo = driver.FindElement(By.CssSelector("label[for='ctl00_cntBody_txtInternalEmployeeTrainerCode']"));
            Assert.AreEqual("Código de instructor:", labelCodigo.Text.Trim(), "El label de código de instructor no coincide.");


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtInternalEmployeeTrainerTelephone")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtInternalEmployeeTrainerTelephone"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtInternalEmployeeTrainerTelephone")).SendKeys("60452623");

            // Solo hacer clic si aún está desactivado

            var toggleHandle = wait.Until(d =>
            {
                var input = d.FindElement(By.Id("ctl00_cntBody_chbInternalEmployeeSearchEnabled"));
                var toggleDiv = input.FindElement(By.XPath("./ancestor::div[contains(@class, 'toggle')]"));
                return toggleDiv.FindElement(By.ClassName("toggle-handle"));
            });

            if (toggleHandle.GetAttribute("aria-checked") == "false")
            {
                toggleHandle.Click();
            }
            Assert.AreEqual("true", toggleHandle.GetAttribute("aria-checked"), "El toggle no quedó activado.");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en el botén de Aceptar");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        }
        public void TrainerEmployeeByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainerCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainerCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainerCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainerNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainerCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainerCodeFilter")).SendKeys(valor);
            // Esperar visible

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por código " + valor);

            Thread.Sleep(1000);
        }


        public void TrainerEmployeeByName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainerNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainerNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainerCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainerNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainerNameFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainerNameFilter")).SendKeys(valor);


            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);

            Thread.Sleep(1000);
        }

        public void SearchTrainerByCodeAndType(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainerCodeFilter")));
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainerCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainerNameFilter");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainerCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainerCodeFilter")).SendKeys(valor);
            Log.Information($"{DateTime.Now}: Digitando valor a buscar en el filtro txtClassroomCodeFilter.  {valor}");

            driver.FindElement(By.Id("ctl00_cntBody_cboTrainerTypeFilter")).Click();
            {
                var dropdown = driver.FindElement(By.Id("ctl00_cntBody_cboTrainerTypeFilter"));
                dropdown.FindElement(By.XPath("//option[. = 'Persona externa']")).Click();
            }



            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);

            Thread.Sleep(1000);
        }



        public void SelectBootstrapTrainerOptionByText(IWebDriver driver, WebDriverWait wait, string selectDataId, string texto)
        {
            // 1. Click en el botón del dropdown
            Thread.Sleep(1500);
            var buttonSelector = $"button[data-id='{selectDataId}']";
            var comboBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(buttonSelector)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboBtn);

            // 2. Esperar que el UL del dropdown esté presente (con timeout extendido si es lento)
            wait.Until(drivr =>
            {
                var lista = driver.FindElements(By.CssSelector("ul.dropdown-menu"));
                return lista.Any(l => l.Displayed && l.Text.Contains(texto));
            });

            // 3. Buscar el span por texto normalizado (mejor que exacto)
            var xpathOpcion = $"//ul[contains(@class,'dropdown-menu')]/li/a/span[contains(normalize-space(.), '{texto}')]";
            var opcionSpan = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathOpcion)));

            // 4. Subir al <a> padre
            var opcionAnchor = opcionSpan.FindElement(By.XPath("./parent::a"));

            // 5. Hacer scroll
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", opcionAnchor);

            // 6. Click
            opcionAnchor.Click();

            // 7. Confirmar que el botón ya muestra el texto
            wait.Until(drivr =>
            {
                var btnTexto = driver.FindElement(By.CssSelector(buttonSelector)).Text.Trim();
                return btnTexto.Contains(texto);
            });

            Thread.Sleep(500); // opcional si sigue habiendo race condition
        }


        public void ClassificationCourseCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso de Clasificación {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en español {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en inglés {seleniumEntities[posicion].Description}");

            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);

        }


        public void ClassificationCourseDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")).SendKeys(seleniumEntities[posicion].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso de Clasificación {seleniumEntities[posicion].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en español {seleniumEntities[posicion].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit")).SendKeys(seleniumEntities[posicion].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en inglés {seleniumEntities[posicion].Description}");

            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

        }

        public void ClassificationCourseCreateCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Curso de Clasificación {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en español {seleniumEntities[0].Description}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en inglés {seleniumEntities[0].Description}");

            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);

        }



        public void ClassificationCourseByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_ClassCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_ClassCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_ClassDesESFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassCodeFilter"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassCodeFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Curso de Clasificación " + valor);

            //Thread.Sleep(1000);
            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por código " + valor);

            Thread.Sleep(1000);
        }









        public void ClassificationCourseByDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_ClassDesESFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassDesESFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_ClassDesESFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_ClassDesESFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassDesESFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_ClassDesESFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando por nombre " + valor);


            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por Descripción " + valor);

            Thread.Sleep(1000);
        }



        public void EditClassificationCourse(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            string descripcion = seleniumEntities[0].Description;

            // Español
            var inputEs = wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_ClassificationCourseDescriptionEsEdit");

            // Asignar valor por JavaScript
            js.ExecuteScript("arguments[0].value = arguments[1];", inputEs, descripcion);

            // Lanzar evento change si tu app lo requiere
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", inputEs);

            Serilog.Log.Information($"{DateTime.Now}: Ingresando Descripción en español con JS: {descripcion}");

            // Inglés
            var inputEn = wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_ClassificationCourseDescriptionEnEdit");

            js.ExecuteScript("arguments[0].value = arguments[1];", inputEn, descripcion);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", inputEn);

            Serilog.Log.Information($"{DateTime.Now}: Ingresando Descripción en inglés con JS: {descripcion}");

            // Click en Aceptar
            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.Id("ctl00_cntBody_btnAccept"))).Click();

            Thread.Sleep(1500);

            // Validar mensaje
            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed &&
                           element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            // Esperar que desaparezca
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
        }


        public void DeleteClassificationCourse(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvClassificationCourseCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvClassificationCourseCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnDelete")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

        }


        public void ReactivedClassificationCourse(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            //IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")));
            // wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit"))).Click();

            var CodeElement = driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit"));
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")));

            wait.Until(ExpectedConditions.ElementToBeClickable((CodeElement))).Click();


            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Curso de Clasificación {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEsEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción de la Curso de Clasificación {seleniumEntities[0].Description}");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_ClassificationCourseDescriptionEnEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando Descripción en inglés {seleniumEntities[0].Description}");

            // Esperar a que el botón toggle esté presente y visible
            var toggleButton = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("div.toggle-group > button[role='switch']")));

            // Verificar el estado actual ("true" = Sí, "false" = No)
            string estadoActual = toggleButton.GetAttribute("aria-checked");

            // Si el estado es "false", hacer clic para cambiar a "Sí"
            if (estadoActual == "false")
            {
                toggleButton.Click();

                // Esperar opcional a que el cambio se refleje
                wait.Until(drive =>
                    toggleButton.GetAttribute("aria-checked") == "true");
            }
            Assert.That(toggleButton.GetAttribute("aria-checked"), Is.EqualTo("true"), "El toggle no quedó en 'Sí'");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Curso de Clasificación ");

            Thread.Sleep(1500);


            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));
        }
        public void MatrixTargetEdited(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {


            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            // wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameEdit"))).Click();

            var CodeElement = driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameEdit"));
            //IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            //wait.Until(ExpectedConditions.ElementToBeClickable((CodeElement))).Click();
            //Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameEdit"))).Click();
            //driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameEdit")).SendKeys(seleniumEntities[0].Description);

            var inputEs = wait.Until(ExpectedConditions.ElementIsVisible(
        By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");

            // Asignar valor por JavaScript
            js.ExecuteScript("arguments[0].value = arguments[1];", inputEs, seleniumEntities[0].Description);

            // Lanzar evento change si tu app lo requiere
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", inputEs);

            Serilog.Log.Information($"{DateTime.Now}: Ingresando Descripción en español con JS: {seleniumEntities[0].Description}");


            DeselectItem(wait, driver);

            Thread.Sleep(1500);

            string[] zonas = { "CRC - Z02 - Zona Rio Frio" };

            foreach (string zona in zonas)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zona);

                Thread.Sleep(1500);
            }



            Thread.Sleep(1500);


            string[] minizonas = { "CRC - M03 - Mini Zona Rio Frio" };

            foreach (string zona in minizonas)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostMiniZoneIdEdit", zona);
                Thread.Sleep(1500);
            }

            // Esperar que cargue el combo de fincas con al menos N opciones
            wait.Until(drivr =>
            {
                try
                {
                    var selectFincas = driver.FindElement(By.Id("ctl00_cntBody_CostFarmsIdEdit"));
                    var opciones = selectFincas.FindElements(By.TagName("option"));
                    return opciones.Count > 2; // ajustar según el mínimo esperado
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            string[] fincas = { "CRC - G03 - Generales Mini Zona Rio Frio" };

            foreach (string finca in fincas)
            {
                // SeleccionarFarmConBootstrapSelect(driver, wait, finca);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostFarmsIdEdit", finca);

                Thread.Sleep(1500);
            }


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void DeselectItem(WebDriverWait wait, IWebDriver driver)
        {
            // 1) Click al botón del dropdown
            var dropdownBtnZona = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector("button[data-id='ctl00_cntBody_CostZoneIdEdit']")));
            dropdownBtnZona.Click();
            Thread.Sleep(1500);
            // 2) Esperar que el dropdown quede abierto (aria-expanded="true")
            wait.Until(d =>
            {
                var btn = d.FindElement(By.CssSelector("button[data-id='ctl00_cntBody_CostZoneIdEdit']"));
                return btn.GetAttribute("aria-expanded") == "true";
            });

            // 3) Localizar el <a> de la opción
            var opcionZonaEstrella = driver.FindElement(
                By.XPath("//ul[contains(@class,'dropdown-menu') and contains(@class,'inner')]/li/a[.//span[contains(text(),'CRC - Z01 - Zona Estrella')]]"));

            // 4) Verificar si está seleccionada
            Console.WriteLine("¿Seleccionada?: " + opcionZonaEstrella.GetAttribute("aria-selected"));

            // 5) Click para desmarcarla
            Actions actions = new Actions(driver);
            actions.MoveToElement(opcionZonaEstrella).Click().Perform();
        }

        public void MatrixTargetCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            var inputEs = wait.Until(ExpectedConditions.ElementIsVisible(
    By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeEdit");

            // Obtener el código a ingresar
            string codeToEnter = seleniumEntities[0].Code;
            string descriptionToEnter = seleniumEntities[0].Description;

            // Asignar valor por JavaScript
            js.ExecuteScript("arguments[0].value = arguments[1];", inputEs, codeToEnter);

            // Lanzar evento change si tu app lo requiere
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", inputEs);

            Serilog.Log.Information($"{DateTime.Now}: Ingresando Descripción en español con JS: {codeToEnter}");

            // Ingresar Descripción
            var descriptionInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");

            js.ExecuteScript("arguments[0].value = arguments[1];", descriptionInput, descriptionToEnter);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", descriptionInput);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando descripción con JS: {descriptionToEnter}");




            var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));



            string textoVisible = divisionBtn.Text.Trim();
            Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");


            //------ Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura-----
            // Hacer clic para abrir el dropdown visual
            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
            // Envuelve en SelectElement
            var select = new SelectElement(selectElem);
            // — Para validar que está en “Estructura de Finca”:
            string selected = select.SelectedOption.Text.Trim();
            Assert.AreEqual("Estructura de Finca", selected,
                "La estructura seleccionada no es la esperada.");
            // — Para cambiar la selección a “Estructura de Finca” (si lo necesitaras):
            select.SelectByText("Estructura de Finca");


            Thread.Sleep(1500);



            string[] zonas = { "CRC - Z01 - Zona Estrella" };

            foreach (string zona in zonas)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zona);

                Thread.Sleep(1500);
            }



            Thread.Sleep(500);


            string[] minizonas = { "CRC - M01 - Mini Zona Atalanta" };

            foreach (string zona in minizonas)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostMiniZoneIdEdit", zona);
                Thread.Sleep(1500);
            }

            // Esperar que cargue el combo de fincas con al menos N opciones
            wait.Until(drivr =>
            {
                try
                {
                    var selectFincas = driver.FindElement(By.Id("ctl00_cntBody_CostFarmsIdEdit"));
                    var opciones = selectFincas.FindElements(By.TagName("option"));
                    return opciones.Count > 2; // ajustar según el mínimo esperado
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            string[] fincas = { "CRC - FDU - Finca Duruy" };

            foreach (string finca in fincas)
            {
                // SeleccionarFarmConBootstrapSelect(driver, wait, finca);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostFarmsIdEdit", finca);

                Thread.Sleep(1500);
            }

            Thread.Sleep(1500);
            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }
        //    public void MatrixTargetCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, int posicion, List<SeleniumEntity> seleniumEntities)
        //    {

        //        var inputEs = wait.Until(ExpectedConditions.ElementIsVisible(
        //By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));
        //        Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeEdit");

        //        // Obtener el código a ingresar
        //        string codeToEnter = seleniumEntities[posicion].Code;
        //        string descriptionToEnter = seleniumEntities[posicion].Description;

        //        // Asignar valor por JavaScript
        //        js.ExecuteScript("arguments[0].value = arguments[1];", inputEs, codeToEnter);

        //        // Lanzar evento change si tu app lo requiere
        //        js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", inputEs);

        //        Serilog.Log.Information($"{DateTime.Now}: Ingresando Descripción en español con JS: {codeToEnter}");

        //        // Ingresar Descripción
        //        var descriptionInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
        //        Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");

        //        js.ExecuteScript("arguments[0].value = arguments[1];", descriptionInput, descriptionToEnter);
        //        js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", descriptionInput);
        //        Serilog.Log.Information($"{DateTime.Now}: Ingresando descripción con JS: {descriptionToEnter}");




        //        var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));



        //        string textoVisible = divisionBtn.Text.Trim();
        //        Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");


        //        //------ Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura-----
        //        // Hacer clic para abrir el dropdown visual
        //        var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
        //        // Envuelve en SelectElement
        //        var select = new SelectElement(selectElem);
        //        // — Para validar que está en “Estructura de Finca”:
        //        string selected = select.SelectedOption.Text.Trim();
        //        Assert.AreEqual("Estructura de Finca", selected,
        //            "La estructura seleccionada no es la esperada.");
        //        // — Para cambiar la selección a “Estructura de Finca” (si lo necesitaras):
        //        select.SelectByText("Estructura de Finca");


        //        Thread.Sleep(1500);



        //        string[] zonas = { "CRC - Z01 - Zona Estrella" };

        //        foreach (string zona in zonas)
        //        {
        //            //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
        //            SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zona);

        //            Thread.Sleep(1500);
        //        }



        //        Thread.Sleep(500);


        //        string[] minizonas = { "CRC - M01 - Mini Zona Atalanta" };

        //        foreach (string zona in minizonas)
        //        {
        //            //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
        //            SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostMiniZoneIdEdit", zona);
        //            Thread.Sleep(1500);
        //        }

        //        // Esperar que cargue el combo de fincas con al menos N opciones
        //        wait.Until(drivr =>
        //        {
        //            try
        //            {
        //                var selectFincas = driver.FindElement(By.Id("ctl00_cntBody_CostFarmsIdEdit"));
        //                var opciones = selectFincas.FindElements(By.TagName("option"));
        //                return opciones.Count > 2; // ajustar según el mínimo esperado
        //            }
        //            catch (NoSuchElementException)
        //            {
        //                return false;
        //            }
        //        });

        //        string[] fincas = { "CRC - FDU - Finca Duruy" };

        //        foreach (string finca in fincas)
        //        {
        //            // SeleccionarFarmConBootstrapSelect(driver, wait, finca);
        //            SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostFarmsIdEdit", finca);

        //            Thread.Sleep(1500);
        //        }

        //        Thread.Sleep(1500);
        //        //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
        //        // Esperar que el toggle esté presente
        //        var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
        //        if (!toggleInput.Selected)
        //        {
        //            var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

        //            // Forzar clic vía JavaScript
        //            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

        //            // Re-obtener después del clic
        //            toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
        //        }
        //        Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


        //        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
        //        Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

        //        Thread.Sleep(1500);

        //        wait.Until(drv =>
        //        {
        //            try
        //            {
        //                var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
        //                return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
        //            }
        //            catch (NoSuchElementException)
        //            {
        //                return false;
        //            }
        //        });

        //        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
        //            By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

        //        Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[posicion].Code + seleniumEntities[posicion].Description);
        //    }

        public void MatrixTargetByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeFilter")).SendKeys(valor);



            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void MatrixTargetByName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_MatrixTargetNameFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameFilter")).SendKeys(valor);

            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void MatrixTargetByStructure(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js)
        {



            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_StructByFilter"))).Click();

            Thread.Sleep(1000);

            string[] structures = { "Estructura de Finca" };

            foreach (string structure in structures)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapStructureByText(driver, wait, "ctl00_cntBody_StructByFilter", structure);

                Thread.Sleep(1500);
            }
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);

            Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + structures);

            Thread.Sleep(1000);
        }

        public void MatrixTargetByCodeAndStructure(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {

            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_StructByFilter"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeFilter")).SendKeys(valor);

            Thread.Sleep(1000);

            string[] structures = { "Estructura de Finca" };

            foreach (string structure in structures)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapStructureByText(driver, wait, "ctl00_cntBody_StructByFilter", structure);

                Thread.Sleep(1500);
            }

            //Thread.Sleep(1000);
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por Estructura ");




            Thread.Sleep(1000);
        }

        public void MatrixTargetByCodeAndPayrollClass(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {

            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_StructByFilter"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeFilter")).SendKeys(valor);

            Thread.Sleep(1000);

            string[] structures = { "Estructura de Clase de Nómina" };

            foreach (string structure in structures)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapStructureByText(driver, wait, "ctl00_cntBody_StructByFilter", structure);

                Thread.Sleep(1500);
            }
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + structures);

            //Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por Estructura ");
            Thread.Sleep(1000);

        }





        public void MatrixTargetByPayrollClass(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js)
        {



            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameFilter");
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_StructByFilter"))).Click();


            Thread.Sleep(1000);

            string[] structures = { "Estructura de Clase de Nómina" };

            foreach (string structure in structures)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapStructureByText(driver, wait, "ctl00_cntBody_StructByFilter", structure);

                Thread.Sleep(1500);
            }
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            //Serilog.Log.Information($"{DateTime.Now}: Buscandopor estructura " + structures);

            Thread.Sleep(1000);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);
            Serilog.Log.Information($"{DateTime.Now}: Buscando por estructura " + structures);




            Thread.Sleep(1000);



        }

        public void MatrixTargetDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            //        // 1) Espera que esté visible y habilitado
            //IWebElement codeInput = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));

            //// 2) Click por JavaScript para asegurar foco si es necesario
            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", codeInput);

            //// 3) Limpiar el campo si trae valor por defecto
            //codeInput.Clear();

            //// 4) Verificar que esté habilitado
            //if (!codeInput.Enabled)
            //{
            //    throw new Exception("El campo de código no está habilitado para escritura.");
            //}

            //// 5) Escribir
            //codeInput.SendKeys(seleniumEntities[posicion].Code);

            //Serilog.Log.Information($"{DateTime.Now}: Ingresando el código de la Matriz de Entrenamiento {seleniumEntities[posicion].Code}");

            //var fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameEdit"))).Click();

            //driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameEdit")).SendKeys(seleniumEntities[posicion].Description);
            //Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre de la Matriz {seleniumEntities[posicion].Description}");

            // Obtener valores actuales de la lista
            string codeToEnter = seleniumEntities[0].Code;
            string descriptionToEnter = seleniumEntities[0].Description;

            // Ingresar Código
            var codeInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeEdit");
            codeInput.Click();

            js.ExecuteScript("arguments[0].value = arguments[1];", codeInput, codeToEnter);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", codeInput);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando código con JS: {codeToEnter}");

            // Ingresar Descripción

            Thread.Sleep(1500);
            var descriptionInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");
            descriptionInput.Click();

            js.ExecuteScript("arguments[0].value = arguments[1];", descriptionInput, descriptionToEnter);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", descriptionInput);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando descripción con JS: {descriptionToEnter}");




            var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));

            string textoVisible = divisionBtn.Text.Trim();
            Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");


            //------ Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura-----
            // Hacer clic para abrir el dropdown visual
            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
            // Envuelve en SelectElement
            var select = new SelectElement(selectElem);
            // — Para validar que está en “Estructura de Finca”:
            string selected = select.SelectedOption.Text.Trim();
            Assert.AreEqual("Estructura de Finca", selected,
                "La estructura seleccionada no es la esperada.");
            // — Para cambiar la selección a “Estructura de Finca” (si lo necesitaras):
            select.SelectByText("Estructura de Finca");


            Thread.Sleep(1500);



            string[] zonas = { "CRC - Z01 - Zona Estrella" };

            foreach (string zona in zonas)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zona);

                Thread.Sleep(1500);
            }



            string[] minizonas = { "CRC - M01 - Mini Zona Atalanta" };

            foreach (string zona in minizonas)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostMiniZoneIdEdit", zona);
                Thread.Sleep(1500);
            }

            // Esperar que cargue el combo de fincas con al menos N opciones
            wait.Until(drivr =>
            {
                try
                {
                    var selectFincas = driver.FindElement(By.Id("ctl00_cntBody_CostFarmsIdEdit"));
                    var opciones = selectFincas.FindElements(By.TagName("option"));
                    return opciones.Count > 2; // ajustar según el mínimo esperado
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            string[] fincas = { "CRC - FDU - Finca Duruy" };

            foreach (string finca in fincas)
            {
                // SeleccionarFarmConBootstrapSelect(driver, wait, finca);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostFarmsIdEdit", finca);

                Thread.Sleep(1500);
            }


            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");

            Thread.Sleep(1500);

        }

        public void MatrixWithMultipleSelectionByDivision(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            //IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeEdit"))).Click();

            var CodeElement = driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeEdit"));
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));

            wait.Until(ExpectedConditions.ElementToBeClickable((CodeElement))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Matriz de Entrenamiento {seleniumEntities[0].Code}");


            fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre de la Matriz {seleniumEntities[0].Description}");


            var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));

            string textoVisible = divisionBtn.Text.Trim();
            Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");

            // Abrir el combo de divisiones
            divisionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));
            divisionBtn.Click();

            // Esperar y seleccionar "División de Honduras"
            var hondurasOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//ul[@class='dropdown-menu inner ']/li/a/span[text()='División de Honduras']")));
            hondurasOption.Click();


            // Verificar que los valores estén reflejados en el título del botón
            string selectedText = divisionBtn.GetAttribute("title");
            Assert.IsTrue(selectedText.Contains("División de Honduras"), "No se seleccionó Honduras.");

            //------Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura---- -
            //Hacer clic para abrir el dropdown visual
            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
            // Envuelve en SelectElement
            var select = new SelectElement(selectElem);
            // — Para validar que está en “Estructura de Finca”:
            string selected = select.SelectedOption.Text.Trim();
            Assert.AreEqual("Estructura de Finca", selected,
                "La estructura seleccionada no es la esperada.");
            // — Para cambiar la selección a “Estructura de Finca” (si lo necesitaras):
            select.SelectByText("Estructura de Finca");
            Thread.Sleep(2000);



            string[] zonas = { "HND - ACA - Acarsa", "HND - ASI - Asisa", "CRC - Z01 - Zona Estrella", "CRC - Z02 - Zona Rio Frio" };

            foreach (string zona in zonas)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zona);

                Thread.Sleep(1500);
            }



            Thread.Sleep(500);


            string[] minizonas = { "HND - ACR - Acarsa", "HND - CGA - Costos Generales Asisa", "HND - ILC - Isletas Linea Corta", "CRC - M01 - Mini Zona Atalanta" };

            foreach (string zona in minizonas)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostMiniZoneIdEdit", zona);
                Thread.Sleep(1500);
            }

            // Esperar que cargue el combo de fincas con al menos N opciones
            wait.Until(drivr =>
            {
                try
                {
                    var selectFincas = driver.FindElement(By.Id("ctl00_cntBody_CostFarmsIdEdit"));
                    var opciones = selectFincas.FindElements(By.TagName("option"));
                    return opciones.Count > 2; // ajustar según el mínimo esperado
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            string[] fincas = { "HND - A17 - Finca Acarsa", "HND - A69 - Costos Generales Asisa", "CRC - FDU - Finca Duruy", "CRC - G01 - Generales Mini Zona Atalanta" };

            foreach (string finca in fincas)
            {
                // SeleccionarFarmConBootstrapSelect(driver, wait, finca);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostFarmsIdEdit", finca);

                Thread.Sleep(1500);
            }

            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }


        public void MatrixWithMultipleSelectionByPayrollClass(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {


            var CodeElement = driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeEdit"));
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));

            wait.Until(ExpectedConditions.ElementToBeClickable((CodeElement))).Click();



            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Matriz de Entrenamiento {seleniumEntities[0].Code}");


            fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre de la Matriz {seleniumEntities[0].Description}");


            var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));

            string textoVisible = divisionBtn.Text.Trim();
            Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");

            // Abrir el combo de divisiones
            divisionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));
            divisionBtn.Click();

            // Esperar y seleccionar "División de Honduras"
            var hondurasOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//ul[@class='dropdown-menu inner ']/li/a/span[text()='División de Honduras']")));
            hondurasOption.Click();


            // Verificar que los valores estén reflejados en el título del botón
            string selectedText = divisionBtn.GetAttribute("title");
            Assert.IsTrue(selectedText.Contains("División de Honduras"), "No se seleccionó Honduras.");

            //------ Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura-----


            Thread.Sleep(1500);
            // 1. Encuentra el <select> y envuélvelo en un SelectElement
            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
            var select = new SelectElement(selectElem);

            // 2. Selecciona por índice (0-basado, así que el child(2) es el índice 1)
            select.SelectByIndex(1);

            // 3. Si necesitas disparar postback manual (botón oculto con guiones bajos)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver.FindElement(By.Id("ctl00_cntBody_btnStructByEdit")));

            // 4. Espera a que se refleje
            wait.Until(d =>
                new SelectElement(d.FindElement(By.Id("ctl00_cntBody_StructByEdit")))
                    .SelectedOption.Text
                    .Trim() ==
                "Estructura de Clase de Nómina" // o el texto que tenga esa opción
            );

            Thread.Sleep(1500);

            string[] companies = {"HND - 2000 - CONTRATISTAS PINAS I",
                "CRC - 3300 - Standard Fruit Company de Costa Rica S.A",
                "CRC - 3332 - AGROINDUSTRIAL PIÑAS DEL BOSQUE S.A.",
                "HND - 3536 - CLINICAS MEDICAS DEL AGUAN, S.A." };


            foreach (string company in companies)
            {

                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CompanyIdEdit", company);

                Thread.Sleep(1500);
            }


            Thread.Sleep(2000);

            string[] payrollClass = {
                "CRC - 3300 - 09-Finca Atalanta",
               "CRC - 3300 - 11-Finca Duruy",
                "HND - 2000 - C1.Emdem/Conlov/Cotralsa",
                "HND - 2000 - P1.Esam/Esmat/Sharp" };



            foreach (string payroll in payrollClass)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_NominalClassIdEdit", payroll);
                Thread.Sleep(1500);
            }



            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }

        public void MatrixWithSelectionByPayrollClass(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            //IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetCodeEdit"))).Click();

            var CodeElement = driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeEdit"));
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));

            wait.Until(ExpectedConditions.ElementToBeClickable((CodeElement))).Click();


            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código de la Matriz de Entrenamiento {seleniumEntities[0].Code}");


            fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_MatrixTargetNameEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_MatrixTargetNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el nombre de la Matriz {seleniumEntities[0].Description}");


            var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));

            string textoVisible = divisionBtn.Text.Trim();
            Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");

            // Abrir el combo de divisiones
            divisionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));
            divisionBtn.Click();

            //------ Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura-----

            // 1. Encuentra el <select> y envuélvelo en un SelectElement
            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
            var select = new SelectElement(selectElem);

            // 2. Selecciona por índice (0-basado, así que el child(2) es el índice 1)
            select.SelectByIndex(1);

            // 3. Si necesitas disparar postback manual (botón oculto con guiones bajos)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver.FindElement(By.Id("ctl00_cntBody_btnStructByEdit")));

            // 4. Espera a que se refleje
            wait.Until(d =>
                new SelectElement(d.FindElement(By.Id("ctl00_cntBody_StructByEdit")))
                    .SelectedOption.Text
                    .Trim() ==
                "Estructura de Clase de Nómina" // o el texto que tenga esa opción
            );

            Thread.Sleep(1500);




            string[] companies = { "CRC - 3300 - Standard Fruit Company de Costa Rica S.A" };

            foreach (string company in companies)
            {

                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CompanyIdEdit", company);

                Thread.Sleep(1500);
            }



            Thread.Sleep(500);


            string[] payrollClass = { "CRC - 3300 - 09-Finca Atalanta" };

            foreach (string payroll in payrollClass)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_NominalClassIdEdit", payroll);
                Thread.Sleep(1500);
            }



            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");
            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));

            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }


        public void SelectBootstrapMultiOptionByText(IWebDriver driver, WebDriverWait wait, string selectDataId, string texto)
        {
            // 1. Abrir el dropdown visual (toggle de bootstrap-select)
            var buttonSelector = $"button[data-id='{selectDataId}']";
            var comboBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(buttonSelector)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboBtn);
            Thread.Sleep(1500);

            // wait.Until(ExpectedConditions.ElementIsVisible(
            // By.CssSelector("div.dropdown-menu.open")));

            // 2. Buscar el <li> que contiene el texto deseado
            var liOpcion = wait.Until(ExpectedConditions.ElementExists(
                By.XPath($"//ul[contains(@class,'dropdown-menu')]/li[a/span[normalize-space(text())='{texto}']]")));

            // 3. Obtener el <a> padre del <span>
            var anchor = liOpcion.FindElement(By.XPath("./a"));

            // 4. Scroll hasta la opción para asegurar visibilidad
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", anchor);
            Thread.Sleep(1500);

            // 5. Si no está ya seleccionada, hacer click
            if (!liOpcion.GetAttribute("class").Contains("selected"))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", anchor);
                Thread.Sleep(1500);
            }
        }




        public void SelectBootstrapStructureByText(IWebDriver driver, WebDriverWait wait, string selectDataId, string texto)
        {
            var buttonSelector = $"button[data-id='{selectDataId}']";

            // 1. Abrir el dropdown
            var comboBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(buttonSelector)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboBtn);
            Thread.Sleep(1500);

            // 2. XPath CORRECTO
            var xpath = $"//ul[contains(@class,'dropdown-menu')]/li/a/span[@class='text' and normalize-space()='{texto}']/parent::a/parent::li";

            // 3. Buscar el <li>
            var liOpcion = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));

            // 4. Scroll
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", liOpcion);
            Thread.Sleep(1300);

            // 5. Verificar si ya está seleccionado
            bool isSelected = liOpcion.GetAttribute("class").Contains("selected");
            if (!isSelected)
            {
                var anchor = liOpcion.FindElement(By.XPath("./a"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", anchor);
                Thread.Sleep(1500);
            }
        }




        // En tu clase Commons:
        public void AssertBootstrapMultiOptionsSelected(IWebDriver driver, WebDriverWait wait, string selectId, params string[] expectedTexts)
        {
            foreach (var texto in expectedTexts)
            {
                // Espera a que el <option> exista en el <select>
                var option = wait.Until(drv =>
                    drv.FindElement(By.XPath(
                        $"//select[@id='{selectId}']/option[normalize-space(text())='{texto}']")));

                // Aserta que está seleccionado
                Assert.IsTrue(option.Selected, $"La opción '{texto}' no quedó seleccionada en el select '{selectId}'.");
            }
        }


        public void MatrixTargetCharacters(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {


            var inputEs = wait.Until(ExpectedConditions.ElementIsVisible(
    By.Id("ctl00_cntBody_MatrixTargetCodeEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetCodeEdit");

            // Obtener el código a ingresar
            string codeToEnter = seleniumEntities[0].Code;
            string descriptionToEnter = seleniumEntities[0].Description;

            // Asignar valor por JavaScript
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("ctl00_cntBody_MatrixTargetCodeEdit"))).Click();
            js.ExecuteScript("arguments[0].value = arguments[1];", inputEs, codeToEnter);

            // Lanzar evento change si tu app lo requiere
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", inputEs);

            Serilog.Log.Information($"{DateTime.Now}: Ingresando Descripción en español con JS: {codeToEnter}");

            // Ingresar Descripción
            var descriptionInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");

            js.ExecuteScript("arguments[0].value = arguments[1];", descriptionInput, descriptionToEnter);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", descriptionInput);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando descripción con JS: {descriptionToEnter}");

            var divisionBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-id='ctl00_cntBody_DivisionCodeEdit']")));

            string textoVisible = divisionBtn.Text.Trim();
            Assert.AreEqual("División de Costa Rica", textoVisible, "La división seleccionada no es la esperada.");


            //------ Esperar que el dropdown esté visible y habilitado para la Selección de la Estructura-----
            // Hacer clic para abrir el dropdown visual
            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_StructByEdit")));
            // Envuelve en SelectElement
            var select = new SelectElement(selectElem);
            // — Para validar que está en “Estructura de Finca”:
            string selected = select.SelectedOption.Text.Trim();
            Assert.AreEqual("Estructura de Finca", selected,
                "La estructura seleccionada no es la esperada.");
            // — Para cambiar la selección a “Estructura de Finca” (si lo necesitaras):
            select.SelectByText("Estructura de Finca");


            Thread.Sleep(1500);

            string[] zonas = { "CRC - Z01 - Zona Estrella" };

            foreach (string zona in zonas)
            {
                //SelectBootstrapMultiOptionByTextZone(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostZoneIdEdit", zona);

                Thread.Sleep(1500);
            }


            string[] minizonas = { "CRC - M01 - Mini Zona Atalanta" };

            foreach (string zona in minizonas)
            {
                //SeleccionarMiniZonaConBootstrapSelect(driver, wait, zona);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostMiniZoneIdEdit", zona);
                Thread.Sleep(1500);
            }


            //Selección de FINCA
            wait.Until(drivr =>
            {
                try
                {
                    var selectFincas = driver.FindElement(By.Id("ctl00_cntBody_CostFarmsIdEdit"));
                    var opciones = selectFincas.FindElements(By.TagName("option"));
                    return opciones.Count > 2; // ajustar según el mínimo esperado
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            string[] fincas = { "CRC - FDU - Finca Duruy" };

            foreach (string finca in fincas)
            {
                // SeleccionarFarmConBootstrapSelect(driver, wait, finca);
                SelectBootstrapMultiOptionByText(driver, wait, "ctl00_cntBody_CostFarmsIdEdit", finca);

                Thread.Sleep(1500);
            }


            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }



        public void MatrixTargetCharactersEdited(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            string descriptionToEnter = seleniumEntities[0].Description;

            // Ingresar Descripción
            var descriptionInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("ctl00_cntBody_MatrixTargetNameEdit")));
            Utilities.CleanFields(js, "ctl00_cntBody_MatrixTargetNameEdit");

            js.ExecuteScript("arguments[0].value = arguments[1];", descriptionInput, descriptionToEnter);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", descriptionInput);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando descripción con JS: {descriptionToEnter}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo de Escuela");


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }

        public void DeleteMatrixTarget(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvMatrixTargetCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvMatrixTargetCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();


            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        }

        public void TrainingCycleCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Ciclo {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Ciclo {seleniumEntities[0].Description}");


            js.ExecuteScript("document.getElementById('ctl00_cntBody_FromDateEdit').value = '01/01/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '01/01/2025'");

            js.ExecuteScript("document.getElementById('ctl00_cntBody_ToDateEdit').value = '12/31/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '12/31/2025'");




            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro del Ciclo");



            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }
        public void SearchTrainingCycleByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_CycleTrainingCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingCodeFilter")).SendKeys(valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }


        public void SearchTrainingCycleByName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_CycleTrainingCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingNameFilter")).SendKeys(valor);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            Thread.Sleep(1000);
        }

        public void TrainingCycleDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Ciclo {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del Ciclo {seleniumEntities[0].Description}");


            js.ExecuteScript("document.getElementById('ctl00_cntBody_FromDateEdit').value = '01/01/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '01/01/2025'");

            js.ExecuteScript("document.getElementById('ctl00_cntBody_ToDateEdit').value = '12/31/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '12/31/2025'");




            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro del Ciclo");
        }

        public void EditTrainingCycle(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_CycleTrainingNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingNameEdit");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);

            js.ExecuteScript("document.getElementById('ctl00_cntBody_FromDateEdit').value = '01/01/2026';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '01/01/2026'");

            js.ExecuteScript("document.getElementById('ctl00_cntBody_ToDateEdit').value = '12/31/2026';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '12/31/2026'");




            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void DeleteTrainingCycle(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dvCycleTrainingCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dvCycleTrainingCode"))).Click();

            Serilog.Log.Information($"{DateTime.Now}: Click sobre el registro  " + valor);


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnDelete"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Delete ");



            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("boton-mensajeria-uno")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("boton-mensajeria-uno"))).Click();


            Serilog.Log.Information($"{DateTime.Now}: Confirmación de borrado  " + valor);

            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));



        }

        public void ReactivedTrainingCycle(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Ciclo de Entrenamiento {seleniumEntities[0].Code}");


            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción delCiclo de Entrenamiento {seleniumEntities[0].Description}");



            js.ExecuteScript("document.getElementById('ctl00_cntBody_FromDateEdit').value = '01/01/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '01/01/2025'");

            js.ExecuteScript("document.getElementById('ctl00_cntBody_ToDateEdit').value = '12/31/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '12/31/2025'");




            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_SearchEnabledEdit")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro de Nuevo Tipo de Formación ");


        }

        public void EditTrainingCycleOverwriteDescription(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            Thread.Sleep(1000);
            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeEdit"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingCodeEdit");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingCodeEdit"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingCodeEdit")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Igresando el Código " + seleniumEntities[0].Code);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_CycleTrainingNameEdit")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();
            Utilities.CleanFields(js, "ctl00_cntBody_CycleTrainingNameEdit");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_CycleTrainingNameEdit"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_CycleTrainingNameEdit")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Igresando la Descripción " + seleniumEntities[0].Description);

            js.ExecuteScript("document.getElementById('ctl00_cntBody_FromDateEdit').value = '01/01/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '01/01/2025'");

            js.ExecuteScript("document.getElementById('ctl00_cntBody_ToDateEdit').value = '12/31/2025';");
            Serilog.Log.Information($"{DateTime.Now}: Ingresando fecha desde en español con JS: '12/31/2025'");

            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnAccept")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();

            Serilog.Log.Information($"{DateTime.Now}:Click en el botón de Aceptar la creación ");



        }



        public void TrainingPlanProgramsCreate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingPlanProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingPlanProgramCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Programa {seleniumEntities[0].Code}");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingPlanProgramName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del programa {seleniumEntities[0].Description}");



            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chkSearchEnabled")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chkSearchEnabled")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Acpeptar para Guardar el Registro del Ciclo");



            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


            Serilog.Log.Information($"{DateTime.Now}: Registro creado.   " + seleniumEntities[0].Code + seleniumEntities[0].Description);
        }



        public void TrainingPlanProgramsByCode(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingPlanProgramCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingPlanProgramCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingPlanProgramCodeFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramCodeFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingPlanProgramCodeFilter")).SendKeys(valor);

            var searchBtn = driver.FindElement(By.Id("ctl00_cntBody_btnSearch"));

            // Clic
            searchBtn.Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Buscar");

            // Esperar que el botón vuelva a habilitarse
            wait.Until(drv => drv.FindElement(By.Id("ctl00_cntBody_btnSearch")).Enabled);



            Thread.Sleep(1000);
        }

        public void TrainingPlanProgramsByName(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {


            wait.Until(ExpectedConditions.ElementIsVisible((By.Id("ctl00_cntBody_txtTrainingPlanProgramCodeFilter"))));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramCodeFilter"))).Click();

            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingPlanProgramCodeFilter");
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingPlanProgramNameFilter");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramNameFilter"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingPlanProgramNameFilter")).SendKeys(valor);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_btnSearch")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnSearch"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Buscando Tipo de formación " + valor);

            Thread.Sleep(1000);
        }

        public void TrainingPlanProgramsDuplicate(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            IWebElement fieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_cntBody_txtTrainingPlanProgramCode")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramCode"))).Click();

            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingPlanProgramCode")).SendKeys(seleniumEntities[0].Code);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando el código del Programa {seleniumEntities[0].Code}");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_txtTrainingPlanProgramName"))).Click();
            driver.FindElement(By.Id("ctl00_cntBody_txtTrainingPlanProgramName")).SendKeys(seleniumEntities[0].Description);
            Serilog.Log.Information($"{DateTime.Now}:Ingresando de la descripción del programa {seleniumEntities[0].Description}");


            //----- Esperar a que el botón toggle esté presente y visible para Habilitar el registro----
            // Esperar que el toggle esté presente
            // Esperar a que el toggle esté presente
            var toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chkSearchEnabled")));
            if (!toggleInput.Selected)
            {
                var toggleVisual = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.toggle")));

                // Forzar clic vía JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", toggleVisual);

                // Re-obtener después del clic
                toggleInput = wait.Until(ExpectedConditions.ElementExists(By.Id("ctl00_cntBody_chkSearchEnabled")));
            }
            Assert.IsTrue(toggleInput.Selected, "El toggle no está en 'Sí' como se esperaba.");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();
            Serilog.Log.Information($"{DateTime.Now}: Click en Aceptar para Guardar el Registro del Ciclo");
        }



        public void EditTrainingPlanPrograms(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, List<SeleniumEntity> seleniumEntities)
        {

            Thread.Sleep(1000);
                      
            string descriptionToEnter = seleniumEntities[0].Description;
            // Ingresar Descripción
            var descriptionInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("ctl00_cntBody_txtTrainingPlanProgramName")));
            Utilities.CleanFields(js, "ctl00_cntBody_txtTrainingPlanProgramName");

            js.ExecuteScript("arguments[0].value = arguments[1];", descriptionInput, descriptionToEnter);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", descriptionInput);
            Serilog.Log.Information($"{DateTime.Now}: Ingresando descripción con JS: {descriptionToEnter}");



            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_cntBody_btnAccept"))).Click();


            Thread.Sleep(1500);

            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(By.CssSelector("div.alert.alert-autocloseable-msg.alert-success"));
                    return element.Displayed && element.Text.Contains("La operación fue realizada exitosamente");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                By.CssSelector("div.alert.alert-autocloseable-msg.alert-success")));


        }

        public void ClearSessionAndCache(WebDriverWait wait, IWebDriver driver, IJavaScriptExecutor js, string valor)
        {

            // Limpiar cookies
            driver.Manage().Cookies.DeleteAllCookies();

            // Limpiar almacenamiento local y de sesión solo si no es una URL de datos
            string currentUrl = driver.Url;
            if (!currentUrl.StartsWith("data:"))
            {
                js.ExecuteScript("window.localStorage.clear();");
                js.ExecuteScript("window.sessionStorage.clear();");
            }
        }

        public void SaveClassroomsPageHtmlToLog(WebDriverWait wait, IWebDriver driver)
        {

            var log = new LoggerConfiguration()
                           .WriteTo.Console()
                           .CreateLogger();
            try
            {
                string pageSource = driver.PageSource;
                log.Information($"{DateTime.Now}: HTML de la página de aulas:\n{pageSource}");
                // logWriter.WriteLine($"{DateTime.Now}: HTML de la página de aulas:\n{pageSource}");
            }
            catch (Exception ex)
            {
                log.Error($"{DateTime.Now}: Error al guardar el HTML de la página de aulas. Excepción: {ex.Message}");
                // logWriter.WriteLine($"{DateTime.Now}: Error al guardar el HTML de la página de aulas. Excepción: {ex.Message}");
            }
        }

        internal void SaveClassroomsPageHtmlToLog()
        {
            throw new NotImplementedException();
        }

    }


}
