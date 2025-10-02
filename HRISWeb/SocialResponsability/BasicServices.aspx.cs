using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using DOLE.HRIS.Exceptions;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class BasicServices : System.Web.UI.Page
    {
        private List<SurveyAnswerEntity> employeeSavedAnswers = null;

        [Dependency]
        protected IBasicServicesBll<BasicServiceEntity> objBasicServicesBll { get; set; }
      
        [Dependency]
        protected IServicesAvailabilityBll<ServicesAvailabilityEntity> objServicesAvailabilityBll { get; set; }
      
        [Dependency]
        protected IOtherServicesBll<OtherServiceEntity> objOtherServicesBll { get; set; }
      
        [Dependency]
        protected IGarbageDisposalTypesBll<GarbageDisposalTypeEntity> objGarbageDisposalTypesBll { get; set; }
      
        [Dependency]
        protected IWaterSuppliesBll<WaterSupplyEntity> objWaterSuppliesBll { get; set; }
       
        [Dependency]
        protected ICookEnergyTypesBll<CookEnergyTypeEntity> objCookEnergyTypesBll { get; set; }
      
        [Dependency]
        protected IToiletTypesBll<ToiletTypeEntity> objToiletTypesBll { get; set; }
       
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
       
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
       
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll objGeneralConfigurationsBll { get; set; }
    
        private const string surveyVersionName = "SurveyVersion";

        private int SurveyVersion;

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
            this.SurveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());
            if (!IsPostBack)
            {
                LoadBasicServices();
                LoadOtherServices();
                LoadGarbageDisposalTypes();
                LoadWaterSupplies();
                LoadToiletTypes();
                LoadCookEnergyTypes();
                //
                LoadSurveyAnswers();
                //
                ValidateWorkingDivisionVsEmployeeDivision();
            }
        }
        /// <summary>
        /// Handles the lbtnBack click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false, HrisEnum.SurveyStates.Draft);
            Response.Redirect("HousingDistribution.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnFinish_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false, HrisEnum.SurveyStates.Completed);            
        }
        /// <summary>
        /// Handles the lbtnSaveAsDraft click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnSaveAsDraft_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(true, HrisEnum.SurveyStates.Draft);
        }
        /// <summary>
        /// Handles the rptServicesAvailability item data bound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void rptServicesAvailability_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList cboServicesAvailabilityItem = e.Item.FindControl("cboServicesAvailability") as DropDownList;
                if (cboServicesAvailabilityItem != null)
                {
                    //cboServicesAvailabilityItem.Items.Add(new ListItem { Value = "", Text = GetLocalResourceObject("NoHaySelec").ToString() });
                    cboServicesAvailabilityItem.Items.AddRange(GetsServicesAvailability().ToArray());
                    List<SurveyAnswerEntity> answers = GetsLocalSurveyAnswer();
                    if(answers != null)
                    {
                        Label lblServiceCode = e.Item.FindControl("lblBasicServiceCode") as Label;
                        // Finde the answer item for the current service code
                        SurveyAnswerEntity answer = lblServiceCode != null ? answers.Find(a => a.QuestionID.Equals("41.1.a") && a.AnswerValue.Equals(lblServiceCode.Text.Trim())) : null;
                        byte answerItem = answer != null ? answer.AnswerItem : (byte)0;

                        answer = answers.Find(a => a.QuestionID.Equals("41.1.b") && a.AnswerItem.Equals(answerItem));
                        ListItem liServicesAvailabilityItem = answer != null ? cboServicesAvailabilityItem.Items.FindByValue(answer.AnswerValue) : null;
                        if (liServicesAvailabilityItem != null)
                        {
                            liServicesAvailabilityItem.Selected = true;
                        }                        
                    }
                }
            }
        }

        /// <summary>
        /// Load the enabled Basic Services and the service for water and elctricity on the repeater
        /// </summary>
        private void LoadBasicServices()
        {
            List<BasicServiceEntity> basicServicesBDList = new List<BasicServiceEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogBasicServices] != null)
                {
                    basicServicesBDList = (List<BasicServiceEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogBasicServices];
                }
                else
                {
                    objBasicServicesBll = objBasicServicesBll ?? Application.GetContainer().Resolve<IBasicServicesBll<BasicServiceEntity>>();
                    basicServicesBDList = objBasicServicesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                basicServicesBDList.ForEach(b => b.CurrentCulture = currentCulture.Name);

                cboBasicServices.DataTextField = "BasicServiceDescriptionByCurrentCulture";
                cboBasicServices.DataValueField = "BasicServiceCode";
                cboBasicServices.DataSource = basicServicesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                     ? f.BasicServiceDescriptionSpanish : f.BasicServiceDescriptionEnglish);
                cboBasicServices.DataBind();

                // Just water and electricity
                rptServicesAvailability.DataSource = basicServicesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.BasicServiceDescriptionSpanish : f.BasicServiceDescriptionEnglish)
                     .Where(s => s.BasicServiceCode.Equals(1) || s.BasicServiceCode.Equals(2)).ToList();
                rptServicesAvailability.DataBind();
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
        /// Gets the enabled Services Availability
        /// </summary>
        /// <returns>The services availability</returns>
        private List<ListItem> GetsServicesAvailability()
        {
            List<ServicesAvailabilityEntity> servicesAvailabilityBDList = new List<ServicesAvailabilityEntity>();
            List<ListItem> servicesAvailability = new List<ListItem>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogServicesAvailability] != null)
                {
                    servicesAvailabilityBDList = (List<ServicesAvailabilityEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogServicesAvailability];
                }
                else
                {
                    objServicesAvailabilityBll = objServicesAvailabilityBll ?? Application.GetContainer().Resolve<IServicesAvailabilityBll<ServicesAvailabilityEntity>>();
                    servicesAvailabilityBDList = objServicesAvailabilityBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                servicesAvailability.Add(new ListItem(GetLocalResourceObject("NoHaySelec").ToString(), "-1"));
                servicesAvailability.AddRange(servicesAvailabilityBDList.Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.ServicesAvailabilityDescriptionSpanish : s.ServicesAvailabilityDescriptionEnglish
                        , s.ServicesAvailabilityCode.ToString())).ToArray());
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
            return servicesAvailability;
        }
        /// <summary>
        /// Gets the enabled Other Services
        /// </summary>
        private void LoadOtherServices()
        {
            List<OtherServiceEntity> otherServicesBDList = new List<OtherServiceEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogOtherServices] != null)
                {
                    otherServicesBDList = (List<OtherServiceEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogOtherServices];
                }
                else
                {
                    objOtherServicesBll = objOtherServicesBll ?? Application.GetContainer().Resolve<IOtherServicesBll<OtherServiceEntity>>();
                    otherServicesBDList = objOtherServicesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboOtherServices.Items.AddRange(otherServicesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR) 
                        ? f.OtherServiceDescriptionSpanish : f.OtherServiceDescriptionEnglish)
                    .Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.OtherServiceDescriptionSpanish : s.OtherServiceDescriptionEnglish
                        , s.OtherServiceCode.ToString())).ToArray());
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
        /// Gets the enabled Garbage Disposal Types
        /// </summary>
        private void LoadGarbageDisposalTypes()
        {
            List<GarbageDisposalTypeEntity> garbageDisposalTypesBDList = new List<GarbageDisposalTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogGarbageDisposalTypes] != null)
                {
                    garbageDisposalTypesBDList = (List<GarbageDisposalTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogGarbageDisposalTypes];
                }
                else
                {
                    objGarbageDisposalTypesBll = objGarbageDisposalTypesBll ?? Application.GetContainer().Resolve<IGarbageDisposalTypesBll<GarbageDisposalTypeEntity>>();
                    garbageDisposalTypesBDList = objGarbageDisposalTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboGarbageDisposal.Items.AddRange(garbageDisposalTypesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.GarbageDisposalTypeDescriptionSpanish : f.GarbageDisposalTypeDescriptionEnglish)
                    .Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.GarbageDisposalTypeDescriptionSpanish : s.GarbageDisposalTypeDescriptionEnglish
                        , s.GarbageDisposalTypeCode.ToString())).ToArray());
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
        /// Gets the enabled Water Supplies
        /// </summary>
        private void LoadWaterSupplies()
        {
            List<WaterSupplyEntity> waterSuppliesBDList = new List<WaterSupplyEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogWaterSupplies] != null)
                {
                    waterSuppliesBDList = (List<WaterSupplyEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogWaterSupplies];
                }
                else
                {
                    objWaterSuppliesBll = objWaterSuppliesBll ?? Application.GetContainer().Resolve<IWaterSuppliesBll<WaterSupplyEntity>>();
                    waterSuppliesBDList = objWaterSuppliesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboWaterSupply.Items.AddRange(waterSuppliesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.WaterSupplyDescriptionSpanish : f.WaterSupplyDescriptionEnglish)
                    .Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.WaterSupplyDescriptionSpanish : s.WaterSupplyDescriptionEnglish
                        , s.WaterSupplyCode.ToString())).ToArray());
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
        /// Gets the enabled Cook Energy Types
        /// </summary>
        private void LoadCookEnergyTypes()
        {
            List<CookEnergyTypeEntity> cookEnergyTypeBDList = new List<CookEnergyTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCookEnergyTypes] != null)
                {
                    cookEnergyTypeBDList = (List<CookEnergyTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCookEnergyTypes];
                }
                else
                {
                    objCookEnergyTypesBll = objCookEnergyTypesBll ?? Application.GetContainer().Resolve<ICookEnergyTypesBll<CookEnergyTypeEntity>>();
                    cookEnergyTypeBDList = objCookEnergyTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboCookEnergyType.Items.AddRange(cookEnergyTypeBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.CookEnergyTypeDescriptionSpanish : f.CookEnergyTypeDescriptionEnglish)
                    .Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.CookEnergyTypeDescriptionSpanish : s.CookEnergyTypeDescriptionEnglish
                        , s.CookEnergyTypeCode.ToString())).ToArray());
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
        /// Gets the enabled Toilet Types
        /// </summary>
        private void LoadToiletTypes()
        {
            List<ToiletTypeEntity> toiletTypeBDList = new List<ToiletTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogToiletTypes] != null)
                {
                    toiletTypeBDList = (List<ToiletTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogToiletTypes];
                }
                else
                {
                    objToiletTypesBll = objToiletTypesBll ?? Application.GetContainer().Resolve<IToiletTypesBll<ToiletTypeEntity>>();
                    toiletTypeBDList = objToiletTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboToilet.Items.Add(new ListItem(Empty, "-1"));
                cboToilet.Items.AddRange(toiletTypeBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.ToiletTypeDescriptionSpanish : f.ToiletTypeDescriptionEnglish)
                    .Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.ToiletTypeDescriptionSpanish : s.ToiletTypeDescriptionEnglish
                        , s.ToiletTypeCode.ToString())).ToArray());
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
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            if (Session[Constants.cCulture] != null)
            {
                return new CultureInfo(Convert.ToString(Session[Constants.cCulture]));
            }
            return new CultureInfo(Constants.cCultureEsCR);
        }
        /// <summary>
        /// Validates that the working division is the same of the current employee survey
        /// </summary>
        private void ValidateWorkingDivisionVsEmployeeDivision()
        {
            if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];

                if (!SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    .Equals(currentSurvey.Item1.DivisionCode))
                {
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard);
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisions);
                    Response.Redirect("StartSurvey.aspx", false);
                }
            }
            else
            {
                Response.Redirect("StartSurvey.aspx", false);
            }
        }
        /// <summary>
        /// Gets the survey questions
        /// </summary>
        /// <returns>The survey questions</returns>
        private List<SurveyQuestionEntity> GetSurveyQuestions()
        {
            List<SurveyQuestionEntity> surveyQuestions = null;

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilitySurveyQuestions] != null)
                {
                    surveyQuestions = (List<SurveyQuestionEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilitySurveyQuestions];
                }
                else
                {
                    objSurveyQuestionsBll = objSurveyQuestionsBll ?? Application.GetContainer().Resolve<ISurveyQuestionsBll<SurveyQuestionEntity>>();
                    surveyQuestions = objSurveyQuestionsBll.ListAll(Convert.ToInt32(ConfigurationManager.AppSettings["SurveyVersion"]));                    
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
            return surveyQuestions;
        }
        /// <summary>
        /// Gets the survey header and answers values
        /// </summary>
        /// <returns>The survey values</returns>
        private Tuple<SurveyEntity, List<SurveyAnswerEntity>> GetSurveyAnswers()
        {
            Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = null;
            try
            {
                if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
                {
                    currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];
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
            return currentSurvey;
        }
        /// <summary>
        /// Gets the employee answers
        /// </summary>
        /// <returns>The employee answers</returns>
        private List<SurveyAnswerEntity> GetsLocalSurveyAnswer()
        {
            if (employeeSavedAnswers == null)
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = GetSurveyAnswers();
                employeeSavedAnswers = currentSurvey != null ? currentSurvey.Item2 : null;
            }
            return employeeSavedAnswers;
        }
        /// <summary>
        /// Load the survey answers for the current page
        /// </summary>
        private void LoadSurveyAnswers()
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    cboBasicServices.SelectedIndex = -1;
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("41"));
                    hdfSelectdBasicServices.Value = answer != null ? answer.AnswerValue : Empty;
                    //
                    cboOtherServices.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("42"));
                    hdfSelectedOtherServices.Value = answer != null ? answer.AnswerValue : Empty;
                    //
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("42.1")))
                    {
                        chkUseInternet.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("42.1")).AnswerValue);
                    }
                    //
                    cboGarbageDisposal.SelectedIndex = -1;                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("43"));
                    hdfSelectedGarbageDisposal.Value = answer != null ? answer.AnswerValue : Empty;
                    //
                    cboWaterSupply.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("44"));
                    hdfSelectedWaterSuppliers.Value = answer != null ? answer.AnswerValue : Empty;
                    //
                    cboToilet.SelectedIndex = -1;                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("45"));
                    ListItem liToilet = answer != null ? cboToilet.Items.FindByValue(answer.AnswerValue) : null;
                    if (liToilet != null)
                    {
                        liToilet.Selected = true;
                    }
                    //
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("45.1")))
                    {
                        chkToiletShare.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("45.1")).AnswerValue);
                    }
                    //
                    cboCookEnergyType.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("46"));
                    hdfSelectedCookEnergyTypes.Value = answer != null ? answer.AnswerValue : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("46.a"));
                    txtOtherCookEnergyType.Text = answer != null ? answer.AnswerValue : Empty;
                    if (answer != null)
                    {
                        txtOtherCookEnergyType.Attributes.Remove("disabled");
                    }
                    //
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("46.1")))
                    {
                        chkCoilAir.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("46.1")).AnswerValue);
                    }
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
                    , "ReturnLoadSurveyAnswers" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { RestoreMultiSelectedValues(); }, 200);", true);
            }
        }
        /// <summary>
        /// Save the survey answers of the employee
        /// </summary>
        /// <param name="saveAsDraft">Indicates if the survey must saved as draft</param>
        /// <param name="state">The survey state</param>
        private void SaveSurveyAnswers(bool saveAsDraft, HrisEnum.SurveyStates state)
        {
            try
            {
                // get answers from screen
                string basicServices = hdfSelectdBasicServices.Value.Trim();
                string otherServices = hdfSelectedOtherServices.Value.Trim();
                string garbageDisposal = hdfSelectedGarbageDisposal.Value.Trim();
                string waterSuppliers = hdfSelectedWaterSuppliers.Value.Trim();
                string toiletType = cboToilet.SelectedValue;
                string cookingEnergyTypes = hdfSelectedCookEnergyTypes.Value.Trim();
                string otherCookEnergyType = txtOtherCookEnergyType.Text.Trim();
                bool useInternet = chkUseInternet.Checked;
                bool toiletShare = chkToiletShare.Checked;
                bool coilAir = chkCoilAir.Checked;
                
                //
                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                if (!IsNullOrWhiteSpace(basicServices))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("41", 1, basicServices,6));
                }
                //
                byte serviceCounter = 1;
                foreach (RepeaterItem item in rptServicesAvailability.Items)
                {
                    if(item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                    {
                        Label lblServiceCode = item.FindControl("lblBasicServiceCode") as Label;
                        DropDownList cboAvailability = item.FindControl("cboServicesAvailability") as DropDownList;
                        if(lblServiceCode != null && cboAvailability != null)
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("41.1.a", serviceCounter, lblServiceCode.Text.Trim(), 6));
                            string availability = cboAvailability.SelectedValue;
                            if (!IsNullOrWhiteSpace(availability) && !availability.Equals("-1"))
                            {
                                employeeAnswers.Add(new Tuple<string, byte, string, int>("41.1.b", serviceCounter, availability, 6));
                            }
                            serviceCounter++;
                        }
                    }
                }
                //
                if (!IsNullOrWhiteSpace(otherServices))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("42", 1, otherServices, 6));
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("42.1", 1, useInternet.ToString(), 6));
                //
                if (!IsNullOrWhiteSpace(garbageDisposal))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("43", 1, garbageDisposal, 6));
                }
                //
                if (!IsNullOrWhiteSpace(waterSuppliers))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("44", 1, waterSuppliers, 6));
                }
                //
                if (!IsNullOrWhiteSpace(toiletType) && !toiletType.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("45", 1, toiletType, 6));
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("45.1", 1, toiletShare.ToString(), 6));
                if (!IsNullOrWhiteSpace(cookingEnergyTypes))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("46", 1, cookingEnergyTypes, 6));
                }
                if (!IsNullOrWhiteSpace(otherCookEnergyType))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("46.a", 1, otherCookEnergyType, 6));
                    txtOtherCookEnergyType.Attributes.Remove("disabled");
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("46.1", 1, coilAir.ToString(), 6));
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                if (surveyAnswers.Item2 != null)
                {
                    surveyAnswers.Item2.ForEach(a => a.LastModifiedUser = currentUser);
                }

                surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("41.1") || a.QuestionID.Contains("46.a"));

                foreach (var item in employeeAnswers)
                {
                    var answer = surveyAnswers.Item2.Find(v => v.QuestionID.Equals(item.Item1) && v.AnswerItem.Equals(item.Item2));
                    if (answer != null)
                    {
                        answer.AnswerValue = item.Item3;
                        answer.LastModifiedUser = currentUser;
                        answer.SurveyVersion = this.SurveyVersion;
                        answer.IdSurveyModule = 6;
                    }
                    else
                    {
                        SurveyQuestionEntity question = surveyQuestions.Find(q => q.QuestionID.Equals(item.Item1));
                        surveyAnswers.Item2.Add(new SurveyAnswerEntity(surveyAnswers.Item1.SurveyCode
                            , question != null ? question.QuestionCode : 0
                            , item.Item1
                            , item.Item2
                            , item.Item3
                            , currentUser
                            , item.Item4) { SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH                
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(state);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.BasicServices);
                surveyAnswers.Item1.SurveyCompletedBy = currentUser;
                surveyAnswers.Item1.LastModifiedUser = currentUser;
                //
                // Save in data base
                if (saveAsDraft || state.Equals(HrisEnum.SurveyStates.Completed))
                {
                    objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
                    objSurveysBll.SaveCurrentState(surveyAnswers.Item1.SurveyCode
                        , surveyAnswers.Item1.SurveyStateCode
                        , surveyAnswers.Item1.SurveyCurrentPageCode
                        , surveyAnswers.Item1.SurveyCompletedBy
                        , state.Equals(HrisEnum.SurveyStates.Completed) ? DateTime.Now : (DateTime?)null
                        , surveyAnswers.Item1.LastModifiedUser
                        , this.SurveyVersion);

                    objSurveyAnswersBll = objSurveyAnswersBll ?? Application.GetContainer().Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();
                    objSurveyAnswersBll.Save(surveyAnswers.Item2);
                    //Realizar la syncronizacion con la sede principial
                    #region Sync With Serve HRISK
                    //if (state.Equals(HrisEnum.SurveyStates.Completed))
                    //{

                    //    //syncronizar
                    //    GeneralConfiguration ConfigEqual0 = objGeneralConfigurationsBll.
                    //        ListByCode(HrisEnum.GeneralConfigurations.SocioeconomicCardAppLocal);

                    //    if (ConfigEqual0.GeneralConfigurationValue == "0")
                    //    {
                    //        var result = SynchronizePendingSurveys(surveyAnswers.Item1.SurveyCode);

                    //        if (!result)
                    //        {

                    //            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "La syncronizacion  no se puede realizar");
                    //        }
                    //    }

                    //}
                    #endregion


                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromProcessResponse" + Guid.NewGuid().ToString()
                        , state.Equals(HrisEnum.SurveyStates.Draft) ? "ProcessSaveAsDraftResponse();" : "ProcessFinishResponse()"
                        , true);
                }
                //
                // Save in session
                if (state.Equals(HrisEnum.SurveyStates.Draft))
                {
                    Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, surveyAnswers);
                }
                else
                {
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard);
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisions);
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
                    , "ReturnLoadSurveyAnswers" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { RestoreMultiSelectedValues(); }, 200);", true);
            }
        }


        private bool SynchronizePendingSurveys(long SurveyCode)
        {
            try
            {
                objGeneralConfigurationsBll = objGeneralConfigurationsBll ?? Application.GetContainer().Resolve<IGeneralConfigurationsBll>();
                GeneralConfigurationEntity configurationBaseAddress = objGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.SocioeconomicCardApiBaseAddress);

                List<SurveyEntity> pendingSurveys = objSurveysBll.ListPendingSynchronization(SurveyCode) ;



                HttpClientHandler handler = new HttpClientHandler()
                {
                    UseDefaultCredentials = true
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
                    }
                }
                if (!pendingSurveys.Where(s => s.PendingSynchronization.Equals(true)).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            
        }

    }
}