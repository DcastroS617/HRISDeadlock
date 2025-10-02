using HRISWeb.Shared;
using System.Threading;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Application.Business.Interfaces;
using System.Web.Services;
using DOLE.HRIS.Application.Business;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class SurveySummary : System.Web.UI.Page
    {
        [Dependency]
        protected IEmployeesBll<EmployeeEntity> objEmployeesBll { get; set; }
       
        [Dependency]
        protected IPoliticalDivisionsBll<PoliticalDivisionEntity> objPoliticalDivisionsBll { get; set; }
       
        [Dependency]
        protected IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity> objPoliticalDivisionsLabelsBll { get; set; }

        [Dependency]
        protected IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity> objHouseholdContributionRangesByDivisionsBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
       
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
        
        [Dependency]
        protected IAdminUsersByModulesBll<AdminUserByModuleEntity> objAdminUsersByModulesBll { get; set; }
      
        [Dependency]
        protected IGeneralConfigurationsBll objGeneralConfigurationsBll { get; set; }

        [Dependency]
        protected ICurrenciesBll<CurrencyEntity> objCurrenciesBll { get; set; }
  
        private const string surveyVersionName = "SurveyVersion";
        public int SurveyVersion;

        public int DivisionCodeGlobal { get; set; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SurveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());
            if (!IsPostBack)
            {
                if (objEmployeesBll == null)
                {
                    objEmployeesBll = Application.GetContainer().Resolve<IEmployeesBll<EmployeeEntity>>();
                }
                if (objSurveysBll == null)
                {
                    objSurveysBll = Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
                }

                ConfigureStartSurveyByUser();
                Currency();
                grvEmployees.DataSource = new List<EmployeeEntity>();
                grvEmployees.DataBind();
                DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                GetsHouseholdContributionRangesByDivisions();
                LoadPoliticalDivisionsLabels();

                if (DivisionCodeGlobal.Equals(4) || DivisionCodeGlobal.Equals(14)) //HND GTM
                {
                    Question_25_2.Visible = false;
                }
                else
                {
                    Question_25_2.Visible = true;
                }

            }
        }
        private CultureInfo GetCurrentCulture()
        {
            if (Session[Constants.cCulture] != null)
            {
                return new CultureInfo(Convert.ToString(Session[Constants.cCulture]));
            }
            return new CultureInfo(Constants.cCultureEsCR);
        }
        public void Currency() {

            List<CurrencyEntity> currenciesBDList = new List<CurrencyEntity>();
            if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies] != null)
            {
                currenciesBDList = (List<CurrencyEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies];
            }
            else
            {
                objCurrenciesBll = objCurrenciesBll ?? Application.GetContainer().Resolve<ICurrenciesBll<CurrencyEntity>>();
                currenciesBDList = objCurrenciesBll.ListEnabled();
            }
            CultureInfo currentCulture = GetCurrentCulture();

            List<CurrencyEntity> currencyByDivision = currenciesBDList.Where(r => r.DivisionCode.Equals(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode)).ToList();

            if (currencyByDivision != null && currencyByDivision.Any())
            {
                lblCurrency.InnerText = Format(Convert.ToString(GetLocalResourceObject("lblCurrency"))
                    , currencyByDivision.FirstOrDefault().CurrencyCode
                    , currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? currencyByDivision.FirstOrDefault().CurrencyNameSpanish
                        : currencyByDivision.FirstOrDefault().CurrencyNameEnglish);
            }
            else
            {
                lblCurrency.InnerText = Format(Convert.ToString(GetLocalResourceObject("lblCurrency"))
                    , Empty
                    , Empty);
            }
        }
        /// <summary>
        /// Configures the survey start screen, determines if the current user is administrator of the social module, if yes then show the search option, otherwise show the user information
        /// </summary>
        private void ConfigureStartSurveyByUser()
        {
            try
            {
                ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromSurveySummary" + Guid.NewGuid().ToString()
                        , "setTimeout(function () {{ ShowEmployeeSearchDialog(); }}, 200);", true);
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
        /// Handles the btnEmployeeSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnEmployeeSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string employeeCode = txtEmployeeCodeSearch.Text.Trim();

                List<EmployeeEntity> employees = new List<EmployeeEntity>();


                if (!IsNullOrWhiteSpace(employeeCode))
                {
                    employees = objEmployeesBll.FilterByGeographicDivisionAndEmployeeCode(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , employeeCode);
                }
                grvEmployees.DataSource = employees;
                grvEmployees.DataBind();

                //Si no se obtuvieron resultados con la cédula suministrada
                //mostrar un mensaje al usuario.
                if (employees.Count == 0)
                {
                    string message = String.Format(Convert.ToString(GetLocalResourceObject("msgEmployeeSearchedNotFound")), SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionName);
                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, message);
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
            finally
            {
                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnFromProcessEmployeeSearchResponse" + Guid.NewGuid().ToString()
                    , "ProcessEmployeeSearchResponse();", true);
            }
        }
        /// <summary>
        /// Handles the grvEmployees pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvEmployees_PreRender(object sender, EventArgs e)
        {
            if ((grvEmployees.ShowHeader && grvEmployees.Rows.Count > 0) || (grvEmployees.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvEmployees.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvEmployees.ShowFooter && grvEmployees.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvEmployees.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the lbtnAcceptSelectedEmployee click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnAcceptSelectedEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll = Application.GetContainer().Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [WebMethod(EnableSession = true)]
        public static SurveySummaryEntity SurveySummaryGet(EmployeeEntity entity,string Lang, int SurveyVersion)
        {
            try
            {
                var CultureGet = Lang;

                var GeographicDivisionCode= SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll = HttpContext.Current.Application.GetContainer().
                    Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();

                entity.GeographicDivisionCode = GeographicDivisionCode;

                var result = objSurveyAnswersBll.SurveySummaryGet(entity, CultureGet);

                return result;
            }
            catch (Exception ex)
            {

                return new SurveySummaryEntity { Error=-99, Msg= ex.Message };
            }
        }

        
        private List<ListItem> GetsHouseholdContributionRangesByDivisions()
        {
            List<ListItem> howMuch = new List<ListItem>();
            try
            {
                List<HouseHoldContributionRangeByDivisionEntity> householdContributionRangeByDivisionBDList = new List<HouseHoldContributionRangeByDivisionEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions] != null)
                {
                    householdContributionRangeByDivisionBDList = (List<HouseHoldContributionRangeByDivisionEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions];
                }
                else
                {
                    objHouseholdContributionRangesByDivisionsBll = objHouseholdContributionRangesByDivisionsBll ?? Application.GetContainer().Resolve<IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity>>();
                    householdContributionRangeByDivisionBDList = objHouseholdContributionRangesByDivisionsBll.ListEnabled(false);
                }

                CultureInfo currentCulture = GetCurrentCulture();

                int workingDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                List<HouseHoldContributionRangeByDivisionEntity> rangesByDivision = householdContributionRangeByDivisionBDList.Where(r => r.DivisionCode.Equals(workingDivisionCode)).ToList();

                howMuch.Add(new ListItem(Empty, "-1"));
                howMuch.AddRange(Enumerable.Range(1, rangesByDivision.Count).Select(m => new ListItem(m.ToString(), m.ToString())));

                // Set middle ranges
                rangesByDivision.ForEach(r => {
                    r.CurrentCulture = currentCulture.Name;
                    r.TextForRange = Convert.ToString(GetLocalResourceObject("TextForRangeMiddle"));
                    r.IsFirstRange = false;
                    r.IsLastRange = false;
                });
                // Set first range
                HouseHoldContributionRangeByDivisionEntity firstRange = rangesByDivision.OrderBy(or => or.RangeOrder).FirstOrDefault();
                if (firstRange != null)
                {
                    firstRange.TextForRange = Convert.ToString(GetLocalResourceObject("TextForRangeFirst"));
                    firstRange.IsFirstRange = true;
                }
                // Set last range
                HouseHoldContributionRangeByDivisionEntity lastRange = rangesByDivision.OrderByDescending(or => or.RangeOrder).FirstOrDefault();
                if (lastRange != null)
                {
                    lastRange.TextForRange = Convert.ToString(GetLocalResourceObject("TextForRangeLast"));
                    lastRange.IsLastRange = true;
                }

                lvHouseholdContributionRanges.DataSource = rangesByDivision;
                lvHouseholdContributionRanges.DataBind();

                lvHouseholdContributionRangesCopy.DataSource = rangesByDivision;
                lvHouseholdContributionRangesCopy.DataBind();
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
            return howMuch;
        }
        

        /// <summary>
        /// Handles the lbtnCancelEmployeeSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnCancelEmployeeSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx", false);
        }



        private void LoadPoliticalDivisionsLabels()
        {
            List<PoliticalDivisionLabelEntity> politicalDivisionsLabelsBDList = new List<PoliticalDivisionLabelEntity>();

            try
            {
                objPoliticalDivisionsLabelsBll = objPoliticalDivisionsLabelsBll ?? Application.GetContainer().Resolve<IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity>>();

                politicalDivisionsLabelsBDList = objPoliticalDivisionsLabelsBll.ListEnabled();

                CultureInfo currentCulture = GetCurrentCulture();

                var CulturaES = currentCulture.Name.Equals(Constants.cCultureEsCR);

                var CountryId = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).CountryID;
                politicalDivisionsLabelsBDList = politicalDivisionsLabelsBDList.Where(p => p.CountryID.ToLower().Equals(CountryId.ToLower())).ToList();

                if (politicalDivisionsLabelsBDList.Any())
                {
                    var lblProvincetxtget = Format("32.1.a {0}", CulturaES ?
                       politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(1)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                       : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(1)).FirstOrDefault().PoliticalDivisionLabelTextEnglish);

                    lblProvince.InnerHtml = lblProvincetxtget;

                    var lblProvincetxtget2 = Format("6. {0}", CulturaES ?
                       politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(1)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                       : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(1)).FirstOrDefault().PoliticalDivisionLabelTextEnglish);
                    BirthProvinceNamelbl.InnerHtml = lblProvincetxtget2;



                    lblCanton.InnerHtml = CulturaES ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(2)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(2)).FirstOrDefault().PoliticalDivisionLabelTextEnglish;

                    lblDistrict.InnerHtml = CulturaES ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(3)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(3)).FirstOrDefault().PoliticalDivisionLabelTextEnglish;

                    lblNeighborhood.InnerHtml = CulturaES ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(4)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(4)).FirstOrDefault().PoliticalDivisionLabelTextEnglish;
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


    }
}