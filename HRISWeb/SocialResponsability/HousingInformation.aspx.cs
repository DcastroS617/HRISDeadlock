using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using System.Threading;
using System.Globalization;
using DOLE.HRIS.Exceptions;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class HousingInformation : System.Web.UI.Page
    {        
        [Dependency]
        protected IHousingTypesBll<HousingTypeEntity> objHousingTypesBll { get; set; }
       
        [Dependency]
        protected IHousingTenuresBll<HousingTenureEntity> objHousingTenuresBll { get; set; }
       
        [Dependency]
        protected IHousingAcquisitionWaysBll<HousingAcquisitionWayEntity> objHousingAcquisitionWaysBll { get; set; }
      
        [Dependency]
        protected ISectorsBll<SectorEntity> objSectorsBll { get; set; }
      
        [Dependency]
        protected IPoliticalDivisionsBll<PoliticalDivisionEntity> objPoliticalDivisionsBll { get; set; }
      
        [Dependency]
        protected IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity> objPoliticalDivisionsLabelsBll { get; set; }
       
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
       
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
        
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }
    
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
                LoadPoliticalDivisionsLabels();
                LoadPoliticalDivision(cboProvince, null, 1);                
                cboCanton.SelectedIndex = -1;
                cboCanton.Enabled = false;
                cboDistrict.SelectedIndex = -1;
                cboDistrict.Enabled = false;
                cboNeighborhood.SelectedIndex = -1;
                cboNeighborhood.Enabled = false;

                LoadSectors();
                LoadHousingTypes();
                LoadHousingTenure();
                LoadHousingAcquisitionWays();
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
            SaveSurveyAnswers(false);
            Response.Redirect("MaterialWellness.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("HousingDistribution.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnSaveAsDraft click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnSaveAsDraft_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(true);
        }
        /// <summary>
        /// Handles the click event for the cboProvince control
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboCanton.SelectedIndex = -1;
            string selectedValue = cboProvince.SelectedValue;

            if(!IsNullOrWhiteSpace(selectedValue) && !selectedValue.Equals("-1"))
            {
                LoadPoliticalDivision(cboCanton, Convert.ToInt32(selectedValue), 2);
                cboCanton.Enabled = true;
            }
            else
            {
                cboCanton.Enabled = false;                
            }
            cboDistrict.SelectedIndex = -1;
            cboDistrict.Enabled = false;
            cboNeighborhood.SelectedIndex = -1;
            cboNeighborhood.Enabled = false;
        }
        /// <summary>
        /// Handles the click event for the cboCanton control
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboCanton_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDistrict.SelectedIndex = -1;
            string selectedValue = cboCanton.SelectedValue;

            if (!IsNullOrWhiteSpace(selectedValue) && !selectedValue.Equals("-1"))
            {
                LoadPoliticalDivision(cboDistrict, Convert.ToInt32(selectedValue), 3);
                cboDistrict.Enabled = true;
            }
            else
            {
                cboDistrict.Enabled = false;                
            }
            cboNeighborhood.SelectedIndex = -1;
            cboNeighborhood.Enabled = false;
        }
        /// <summary>
        /// Handles the click event for the cboDistrict control
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboNeighborhood.SelectedIndex = -1;
            string selectedValue = cboDistrict.SelectedValue;

            if (!IsNullOrWhiteSpace(selectedValue) && !selectedValue.Equals("-1"))
            {
                LoadPoliticalDivision(cboNeighborhood, Convert.ToInt32(selectedValue), 4);
                cboNeighborhood.Enabled = true;
            }
            else
            {                
                cboNeighborhood.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the enabled Housing Types
        /// </summary>
        private void LoadHousingTypes()
        {
            List<HousingTypeEntity> housingTypesBDList = new List<HousingTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingTypes] != null)
                {
                    housingTypesBDList = (List<HousingTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingTypes];
                }
                else
                {
                    objHousingTypesBll = objHousingTypesBll ?? Application.GetContainer().Resolve<IHousingTypesBll<HousingTypeEntity>>();
                    housingTypesBDList = objHousingTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                cboHousingType.Items.Add(new ListItem(Empty, "-1"));
                cboHousingType.Items.AddRange(housingTypesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR) 
                        ? f.HousingTypeDescriptionSpanish : f.HousingTypeDescriptionEnglish)
                    .Select(h => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? h.HousingTypeDescriptionSpanish : h.HousingTypeDescriptionEnglish
                        , h.HousingTypeCode.ToString())).ToArray());
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
        /// Gets the enabled Housing Tenures
        /// </summary>
        private void LoadHousingTenure()
        {
            List<HousingTenureEntity> housingTenuresBDList = new List<HousingTenureEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingTenures] != null)
                {
                    housingTenuresBDList = (List<HousingTenureEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingTenures];
                }
                else
                {
                    objHousingTenuresBll = objHousingTenuresBll ?? Application.GetContainer().Resolve<IHousingTenuresBll<HousingTenureEntity>>();
                    housingTenuresBDList = objHousingTenuresBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboHousingTenure.Items.Add(new ListItem(Empty, "-1"));
                cboHousingTenure.Items.AddRange(housingTenuresBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.HousingTenureDescriptionSpanish : f.HousingTenureDescriptionEnglish)
                    .Select(h => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? h.HousingTenureDescriptionSpanish : h.HousingTenureDescriptionEnglish
                        , h.HousingTenureCode.ToString())).ToArray());
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
        /// Gets the enabled Housing Acquisition ways
        /// </summary>
        private void LoadHousingAcquisitionWays()
        {
            List<HousingAcquisitionWayEntity> housingAcquisitionWaysBDList = new List<HousingAcquisitionWayEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingAcquisitionWays] != null)
                {
                    housingAcquisitionWaysBDList = (List<HousingAcquisitionWayEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingAcquisitionWays];
                }
                else
                {
                    objHousingAcquisitionWaysBll = objHousingAcquisitionWaysBll ?? Application.GetContainer().Resolve<IHousingAcquisitionWaysBll<HousingAcquisitionWayEntity>>();
                    housingAcquisitionWaysBDList = objHousingAcquisitionWaysBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboAcquisitionWay.Items.Add(new ListItem(Empty, "-1"));
                cboAcquisitionWay.Items.AddRange(housingAcquisitionWaysBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.HousingAcquisitionWayDescriptionSpanish : f.HousingAcquisitionWayDescriptionEnglish)
                    .Select(h => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? h.HousingAcquisitionWayDescriptionSpanish : h.HousingAcquisitionWayDescriptionEnglish
                        , h.HousingAcquisitionWayCode.ToString())).ToArray());
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
        /// Gets the enabled Housing Acquisition ways
        /// </summary>
        private void LoadSectors()
        {
            List<SectorEntity> sectorsBDList = new List<SectorEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogSectors] != null)
                {
                    sectorsBDList = (List<SectorEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogSectors];
                }
                else
                {
                    objSectorsBll = objSectorsBll ?? Application.GetContainer().Resolve<ISectorsBll<SectorEntity>>();
                    sectorsBDList = objSectorsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboSector.Items.Add(new ListItem(Empty, "-1"));
                cboSector.Items.AddRange(sectorsBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.SectorDescriptionSpanish : f.SectorDescriptionEnglish)
                    .Select(h => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? h.SectorDescriptionSpanish : h.SectorDescriptionEnglish
                        , h.SectorCode.ToString())).ToArray());
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
        /// Loads the political division by level
        /// </summary>
        /// <param name="cboPoliticalDivision"></param>
        /// <param name="parentPoliticalDivision"></param>
        /// <param name="politicalDivisionLevelToLoad">The level to load in the poitical division hierarchy</param>
        private void LoadPoliticalDivision(DropDownList cboPoliticalDivision, int? parentPoliticalDivision, int politicalDivisionLevelToLoad)
        {
            List<PoliticalDivisionEntity> politicalDivisionBDList = new List<PoliticalDivisionEntity>();

            try
            {
                objPoliticalDivisionsBll = objPoliticalDivisionsBll ?? Application.GetContainer().Resolve<IPoliticalDivisionsBll<PoliticalDivisionEntity>>();
                politicalDivisionBDList = objPoliticalDivisionsBll.ListEnabledByCountryByParentPoliticalDivision(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).CountryID, parentPoliticalDivision);

                cboPoliticalDivision.DataTextField = "PoliticalDivisionName";
                cboPoliticalDivision.DataValueField = "PoliticalDivisionID";
                cboPoliticalDivision.SelectedIndex = -1;
                //bool noPoliticalDivision = false;
                if (!politicalDivisionBDList.Any() || politicalDivisionLevelToLoad == 4)
                {
                    politicalDivisionBDList.Add(new PoliticalDivisionEntity(0, parentPoliticalDivision, "N/A"));
                    //noPoliticalDivision = true;
                }
                cboPoliticalDivision.DataSource = politicalDivisionBDList.Where(p => p.ParentPoliticalDivisionID.Equals(parentPoliticalDivision)).OrderBy(o => o.PoliticalDivisionName);
                cboPoliticalDivision.DataBind();
                cboPoliticalDivision.Items.Insert(0, new ListItem(Empty, "-1"));

                if (chkAdditionalTerrain.Checked)
                {
                    txtDescriptionAdditionalTerrain.Attributes.Remove("disabled");
                }
                /*if (noPoliticalDivision)
                {
                    cboPoliticalDivision.SelectedIndex = -1;
                    ListItem liPoliticalDivision = cboPoliticalDivision.Items.FindByValue("0");
                    if(liPoliticalDivision != null)
                    {
                        liPoliticalDivision.Selected = true;
                    }
                }*/
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
        /// Gets the enabled Political Divisions Labels by division
        /// </summary>
        private void LoadPoliticalDivisionsLabels()
        {
            List<PoliticalDivisionLabelEntity> politicalDivisionsLabelsBDList = new List<PoliticalDivisionLabelEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisionsLabels] != null)
                {
                    politicalDivisionsLabelsBDList = (List<PoliticalDivisionLabelEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisionsLabels];
                }
                else
                {
                    objPoliticalDivisionsLabelsBll = objPoliticalDivisionsLabelsBll ?? Application.GetContainer().Resolve<IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity>>();
                    politicalDivisionsLabelsBDList = objPoliticalDivisionsLabelsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                var CountryId = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).CountryID;
                politicalDivisionsLabelsBDList = politicalDivisionsLabelsBDList.Where(p => p.CountryID.ToLower().Equals(CountryId.ToLower())).ToList();

                if (politicalDivisionsLabelsBDList.Any())
                {
                    lblProvince.Text = Format("32.1 {0}", currentCulture.Name.Equals(Constants.cCultureEsCR) ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(1)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(1)).FirstOrDefault().PoliticalDivisionLabelTextEnglish);

                    lblCanton.Text = currentCulture.Name.Equals(Constants.cCultureEsCR) ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(2)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(2)).FirstOrDefault().PoliticalDivisionLabelTextEnglish;

                    lblDistrict.Text = currentCulture.Name.Equals(Constants.cCultureEsCR) ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(3)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(3)).FirstOrDefault().PoliticalDivisionLabelTextEnglish;

                    lblNeighborhood.Text = currentCulture.Name.Equals(Constants.cCultureEsCR) ?
                        politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(4)).FirstOrDefault().PoliticalDivisionLabelTextSpanish
                        : politicalDivisionsLabelsBDList.Where(l => l.PoliticalDivisionLevel.Equals(4)).FirstOrDefault().PoliticalDivisionLabelTextEnglish;
                }
                else
                {
                    lblProvince.Text = Convert.ToString(GetLocalResourceObject("lblProvince.Text"));
                    lblCanton.Text = Convert.ToString(GetLocalResourceObject("lblCanton.Text"));
                    lblDistrict.Text = Convert.ToString(GetLocalResourceObject("lblDistrict.Text"));
                    lblNeighborhood.Text = Convert.ToString(GetLocalResourceObject("lblNeighborhood.Text"));
                }

                cboProvinceValidation.Attributes.Remove("data-content");
                cboProvinceValidation.Attributes.Add("data-content", Format(GetLocalResourceObject("msgProvinceValidation").ToString(), lblProvince.Text.Replace("38.1", Empty)));

                cboCantonValidation.Attributes.Remove("data-content");
                cboCantonValidation.Attributes.Add("data-content", Format(GetLocalResourceObject("msgCantonValidation").ToString(), lblCanton.Text));

                cboDistrictValidation.Attributes.Remove("data-content");
                cboDistrictValidation.Attributes.Add("data-content", Format(GetLocalResourceObject("msgDistrictValidation").ToString(), lblDistrict.Text));

                cboNeighborhoodValidation.Attributes.Remove("data-content");
                var SetTextNeighborhood = lblNeighborhood.Text.Equals("-") || string.IsNullOrEmpty(lblNeighborhood.Text)
                    ? GetLocalResourceObject("Value").ToString()
                    : lblNeighborhood.Text;


                cboNeighborhoodValidation.Attributes.Add("data-content", Format(GetLocalResourceObject("msgNeighborhoodValidation").ToString(), SetTextNeighborhood));
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
        /// Load the survey answers for the current page
        /// </summary>
        private void LoadSurveyAnswers()
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    cboProvince.SelectedIndex = -1;
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("32.1.a"));
                    ListItem liProvince = answer != null ? cboProvince.Items.FindByValue(answer.AnswerValue) : null;
                    if (liProvince != null)
                    {
                        liProvince.Selected = true;
                        LoadPoliticalDivision(cboCanton, Convert.ToInt32(liProvince.Value), 2);
                        cboCanton.Enabled = true;
                    }
                    //
                    cboCanton.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("32.1.b"));
                    ListItem liCanton = answer != null ? cboCanton.Items.FindByValue(answer.AnswerValue) : null;
                    if (liCanton != null)
                    {
                        liCanton.Selected = true;
                        LoadPoliticalDivision(cboDistrict, Convert.ToInt32(liCanton.Value), 3);
                        cboDistrict.Enabled = true;
                    }
                    //
                    cboDistrict.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("32.1.c"));
                    ListItem liDistrict = answer != null ? cboDistrict.Items.FindByValue(answer.AnswerValue) : null;
                    if (liDistrict != null)
                    {
                        liDistrict.Selected = true;
                        LoadPoliticalDivision(cboNeighborhood, Convert.ToInt32(liDistrict.Value), 4);
                        cboNeighborhood.Enabled = true;
                    }
                    //                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("32.1.d"));
                    ListItem liNeighborhood = answer != null ? cboNeighborhood.Items.FindByValue(answer.AnswerValue) : null;
                    if (liNeighborhood != null)
                    {
                        cboNeighborhood.SelectedIndex = -1;
                        liNeighborhood.Selected = true;
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("32.1.e"));
                    txtDirection.Text = answer != null ? answer.AnswerValue : Empty;
                    //
                    cboSector.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("32.2"));
                    ListItem liSector = answer != null ? cboSector.Items.FindByValue(answer.AnswerValue) : null;
                    if (liSector != null)
                    {
                        liSector.Selected = true;
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("33"));
                    chkWorkerLivesOnFarm.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    //
                    cboHousingType.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("34"));
                    ListItem liHousingType = answer != null ? cboHousingType.Items.FindByValue(answer.AnswerValue) : null;
                    if (liHousingType != null)
                    {
                        liHousingType.Selected = true;
                    }
                    //
                    cboHousingTenure.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("35"));
                    ListItem liHousingTenure = answer != null ? cboHousingTenure.Items.FindByValue(answer.AnswerValue) : null;
                    if (liHousingTenure != null)
                    {
                        liHousingTenure.Selected = true;
                    }
                    //
                    cboAcquisitionWay.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("36"));
                    ListItem liAcquisitionWay = answer != null ? cboAcquisitionWay.Items.FindByValue(answer.AnswerValue) : null;
                    if (liAcquisitionWay != null)
                    {
                        liAcquisitionWay.Selected = true;
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("37"));
                    chkLegalDocToItsName.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("38"));
                    chkAdditionalTerrain.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    if (chkAdditionalTerrain.Checked)
                    {
                        txtDescriptionAdditionalTerrain.Attributes.Remove("disabled");
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("38.1"));
                    txtDescriptionAdditionalTerrain.Text = answer != null ? answer.AnswerValue : Empty;
                    if (answer != null)
                    {                        
                        txtDescriptionAdditionalTerrain.Attributes.Remove("disabled");
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
        }
        /// <summary>
        /// Save the survey answers of the employee
        /// </summary>
        /// <param name="saveAsDraft">Indicates if the survey must saved as draft</param>
        private void SaveSurveyAnswers(bool saveAsDraft)
        {
            try
            {
                // get answers from screen
                string province = cboProvince.SelectedValue;
                string canton = cboCanton.SelectedValue;
                string district = cboDistrict.SelectedValue;
                string neighborhood = cboNeighborhood.SelectedValue;
                string houseDirection = txtDirection.Text;
                string sector = cboSector.SelectedValue;
                bool workerLivesOnFarm = chkWorkerLivesOnFarm.Checked;
                string housingType = cboHousingType.SelectedValue;
                string housingTenure = cboHousingTenure.SelectedValue;
                string acquisitionWay = cboAcquisitionWay.SelectedValue;
                bool legalDocToItsName = chkLegalDocToItsName.Checked;
                bool legalAdditionalTerrain = chkAdditionalTerrain.Checked;
                string descriptionAndUseAdditionalTerrain = txtDescriptionAdditionalTerrain.Text.Trim();                
                //
                List<Tuple<string, byte, string,int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                if (!IsNullOrWhiteSpace(province) && !province.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("32.1.a", 1, province,6));
                }
                if (!IsNullOrWhiteSpace(canton) && !canton.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("32.1.b", 1, canton, 6));
                    cboCanton.Enabled = true;
                }
                if (!IsNullOrWhiteSpace(district) && !district.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("32.1.c", 1, district, 6));
                    cboDistrict.Enabled = true;
                }
                if (!IsNullOrWhiteSpace(neighborhood) && !neighborhood.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("32.1.d", 1, neighborhood, 6));
                    cboNeighborhood.Enabled = true;
                }
                if (!IsNullOrWhiteSpace(houseDirection))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("32.1.e", 1, houseDirection, 6));
                }
                //
                if (!IsNullOrWhiteSpace(sector) && !sector.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("32.2", 1, sector, 6));
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("33", 1, workerLivesOnFarm.ToString(), 6));
                //
                if (!IsNullOrWhiteSpace(housingType) && !housingType.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("34", 1, housingType, 6));
                }
                //
                if (!IsNullOrWhiteSpace(housingTenure) && !housingTenure.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("35", 1, housingTenure, 6));
               }
                //
                //if (!IsNullOrWhiteSpace(acquisitionWay) && !acquisitionWay.Equals("-1"))
                //{
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("36", 1, acquisitionWay, 6));
              //  }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("37", 1, legalDocToItsName.ToString(), 6));
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("38", 1, legalAdditionalTerrain.ToString(), 6));
                if (legalAdditionalTerrain)
                {
                    txtDescriptionAdditionalTerrain.Attributes.Remove("disabled");
                }
                //
                if (!IsNullOrWhiteSpace(descriptionAndUseAdditionalTerrain))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("38.1", 1, descriptionAndUseAdditionalTerrain, 6));
                    txtDescriptionAdditionalTerrain.Attributes.Remove("disabled");
                }
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                if (surveyAnswers.Item2 != null)
                {
                    surveyAnswers.Item2.ForEach(a => a.LastModifiedUser = currentUser);
                }

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
                            , item.Item4) { SurveyVersion = this.SurveyVersion } );
                    }
                }// FOREACH                
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.HousingInformation);
                surveyAnswers.Item1.SurveyCompletedBy = currentUser;
                surveyAnswers.Item1.LastModifiedUser = currentUser;
                //
                // Save in data base
                if (saveAsDraft)
                {
                    objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
                    objSurveysBll.SaveCurrentState(surveyAnswers.Item1.SurveyCode
                        , surveyAnswers.Item1.SurveyStateCode
                        , surveyAnswers.Item1.SurveyCurrentPageCode
                        , surveyAnswers.Item1.SurveyCompletedBy
                        , null
                        , surveyAnswers.Item1.LastModifiedUser
                        , this.SurveyVersion);

                    objSurveyAnswersBll = objSurveyAnswersBll ?? Application.GetContainer().Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();
                    objSurveyAnswersBll.Save(surveyAnswers.Item2);

                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, GetLocalResourceObject("msjSurveySavedAsDraft").ToString());

                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromProcessSaveAsDraftResponse" + Guid.NewGuid().ToString()
                        , "ProcessSaveAsDraftResponse();", true);
                }
                //
                // Save in session
                Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, surveyAnswers);
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