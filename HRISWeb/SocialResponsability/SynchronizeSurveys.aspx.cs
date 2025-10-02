using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Web;
using Unity.Attributes;
using HRISWeb.Shared;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using System.Net;
using System.Configuration;
using System.IO;

namespace HRISWeb.SocialResponsability
{
    public partial class SynchronizeSurveys : System.Web.UI.Page
    {
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
        [Dependency]
        protected IGeneralConfigurationsBll objGeneralConfigurationsBll { get; set; }
        [Dependency]
        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }

        private const string dominioUsuarioConsultaAD = "DominioUsuarioConsultaAD";
        private const string claveConsultaAd = "ClaveConsultaAd";
        private const string usuarioConsultaAd = "UsuarioConsultaAd";

        /// <summary>
        /// Sets the culture information
        /// </summary>
        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            if (Session[Constants.cCulture] != null)
            {
                CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }
        /// <summary>
        /// Handles the load of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SearchResults(PagerUtil.GetActivePage(blstPager));
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
        }
        /// <summary>
        /// Handles the lbtnSynchronizeSurveys click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnSynchronizeSurveys_Click(object sender, EventArgs e)
        {
            SynchronizePendingSurveys();
        }
        /// <summary>
        /// Handles the grvPendingSurveys pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvPendingSurveys_PreRender(object sender, EventArgs e)
        {
            // Configure the grid to have thead and tbody sections explicitly declared in markup
            if ((grvPendingSurveys.ShowHeader && grvPendingSurveys.Rows.Count > 0) || (grvPendingSurveys.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> 
                grvPendingSurveys.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvPendingSurveys.ShowFooter && grvPendingSurveys.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> 
                grvPendingSurveys.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        /// <summary>
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void blstPager_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    PagerUtil.SetActivePage(blstPager, Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value));
                    SearchResults(PagerUtil.GetActivePage(blstPager));
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(upnList, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(upnList, TipoMensaje.Error, newEx.Message);
                }
            }
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<SurveyEntity> SearchResults(int page)
        {
            objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
            PageHelper<SurveyEntity> pageHelper = objSurveysBll.ListPendingSynchronizationByPage(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, page);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);

            return pageHelper;
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        /// <param name="pageHelper">The page helper that contains result information</param>
        private void DisplayResults(PageHelper<SurveyEntity> pageHelper)
        {
            grvPendingSurveys.DataSource = pageHelper.ResultList;
            grvPendingSurveys.DataBind();

            htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);

            if(pageHelper.TotalResults == 0)
            {
               // lbtnSynchronizeSurveys.Enabled = false;
                MensajeriaHelper.MostrarMensaje(upnList, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msgNoSurveysToSynchronize")));
            }
        }

        public static void LoggerSynFicha(string logMessage, StreamWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }
        /// <summary>
        /// Sends the completed surveys and pending to synchronize to central server
        /// </summary>
        private void SynchronizePendingSurveys()
        {
            try
            {                                                
                objGeneralConfigurationsBll = objGeneralConfigurationsBll ?? Application.GetContainer().Resolve<IGeneralConfigurationsBll>();
                GeneralConfigurationEntity configurationBaseAddress = objGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.SocioeconomicCardApiBaseAddress);

                List<SurveyEntity> pendingSurveys = objSurveysBll.ListPendingSynchronization(null);
                var uri= new Uri(configurationBaseAddress.GeneralConfigurationValue);



                var _UserName = ObjGeneralParametersBll.ListByFilter(usuarioConsultaAd); 
                var _PassWord = ObjGeneralParametersBll.ListByFilter(claveConsultaAd); 
                var _DomainName = ObjGeneralParametersBll.ListByFilter(dominioUsuarioConsultaAD); 
                var CRENDITIAL = new NetworkCredential(_DomainName + "\\"+_UserName, _PassWord);
                var credentialsCache = new CredentialCache { { uri, "NTLM", CRENDITIAL } };
                HttpClientHandler handler = new HttpClientHandler()
                {
                    Credentials= credentialsCache,
                    AllowAutoRedirect=true,
                    //UseDefaultCredentials = true,
                };
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(configurationBaseAddress.GeneralConfigurationValue);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    foreach (SurveyEntity item in pendingSurveys)
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = client.PostAsync("api/Surveys/Save", content).Result;
                        
                        if (response.IsSuccessStatusCode)
                        {                            
                            item.PendingSynchronization = false;
                            objSurveysBll.SetSynchronized(item.SurveyCode);
                        }
                        else
                        {
                            var pathlog = Server.MapPath("loghrisSync.txt");
                            using (StreamWriter w = File.AppendText(pathlog))
                            {
                                LoggerSynFicha(response.IsSuccessStatusCode.ToString(), w);
                                LoggerSynFicha(response.ReasonPhrase, w);
                                LoggerSynFicha(response.RequestMessage?.ToString(), w);
                            }
                        }
                    }                    
                }
                if(!pendingSurveys.Where(s => s.PendingSynchronization.Equals(true)).Any())
                {
                    MensajeriaHelper.MostrarMensaje(upnActions, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("AllSurveysSynchronized")));
                }
                else
                {

                    MensajeriaHelper.MostrarMensaje(upnActions, TipoMensaje.Advertencia, Convert.ToString(GetLocalResourceObject("NotAllSurveysWereSynchronized")));

                   
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(upnActions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(upnActions, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                ScriptManager.RegisterStartupScript(upnActions
                        , upnActions.GetType()
                        , "ProcessSynchronizeResponse" + Guid.NewGuid().ToString()
                        , "ProcessSynchronizeResponse();", true);

                SearchResults(PagerUtil.GetActivePage(blstPager));
            }
        }
    }
}