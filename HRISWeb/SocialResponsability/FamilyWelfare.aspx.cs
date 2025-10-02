using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using System.Threading;
using System.Globalization;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class FamilyWelfare : System.Web.UI.Page
    {
        [Dependency]
        protected IDiseaseCarePlacesBll<DiseaseCarePlaceEntity> objDiseaseCarePlacesBll { get; set; }
       
        [Dependency]
        protected IDisabilityTypesBll<DisabilityTypeEntity> objDisabilityTypesBll { get; set; }
      
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
       
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
      
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }
      
        [Dependency]
        public IReasonNotMedicalBLL<ReasonNotMedicalEntity> objReasonNotMedicalBll { get; set; }
       
        [Dependency]
        public IReasonNotAttentionBLL<ReasonNotAttentionEntity> objReasonNotAttentionBll { get; set; }
  
        [Dependency]
        protected IFamilyRelationshipsBll<FamilyRelationshipEntity> objFamilyRelationshipsBll { get; set; }

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
                LoadDisabilityTypes();

                rptFamilyWelfareUpdate();

                LoadSurveyAnswersDishabilies();


                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                if (workingDivision.DivisionCode.Equals(4) || workingDivision.DivisionCode.Equals(14) ) //HND GTM
                {
                    Question25_2Text.Visible = false;
                }
                else
                {
                    Question25_2Text.Visible = true;
                }

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
            Response.Redirect("Expenses.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("Disabilities.aspx", false);
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
        /// Handles the item data bound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void rptFamilyWelfare_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LoadSurveyAnswers(e.Item);
            }
        }

        /// <summary>
        /// rptFamilyWelfareUpdate
        /// </summary>
        public void rptFamilyWelfareUpdate()
        {

            int totalPeopleLivingWithEmployee = GetTotalPeopleLivingWithEmployee();
            List<FamiliarEntity> relatives = new List<FamiliarEntity>();
            relatives.Add(new FamiliarEntity(0));
            relatives.AddRange(Enumerable.Range(1, totalPeopleLivingWithEmployee).Select(r => new FamiliarEntity(r)));
            //
            rptFamilyWelfare.DataSource = relatives;
            rptFamilyWelfare.DataBind();
        }


        /// <summary>
        /// Load the Family Relationship
        /// </summary>
        public string GetsFamilyRelationShip(int number) {

            List<FamilyRelationshipEntity> familyRelationships;
            
            if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships] != null)
            {
                familyRelationships = (List<FamilyRelationshipEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships];
            }
            else
            {
                objFamilyRelationshipsBll = objFamilyRelationshipsBll ?? Application.GetContainer().Resolve<IFamilyRelationshipsBll<FamilyRelationshipEntity>>();
                familyRelationships = objFamilyRelationshipsBll.ListEnabled();
            }

            CultureInfo currentCulture = GetCurrentCulture();

            if(currentCulture.Name.Equals(Constants.cCultureEsCR))
            {
               return familyRelationships.Find(r => r.FamilyRelationshipCode == number).FamilyRelationshipDescriptionSpanish;
            }
            else {
                return familyRelationships.Find(r => r.FamilyRelationshipCode == number).FamilyRelationshipDescriptionEnglish;
            
            }
        }

        /// <summary>
        /// Load the Disease Care Places
        /// </summary>
        private List<ListItem> GetsDiseaseCarePlaces()
        {
            List<ListItem> diseaseCarePlaceList = new List<ListItem>();
            List<DiseaseCarePlaceEntity> diseaseCarePlaceBDList = new List<DiseaseCarePlaceEntity>();
            try
            {

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDiseaseCarePlaces] != null)
                {
                    diseaseCarePlaceBDList = (List<DiseaseCarePlaceEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDiseaseCarePlaces];
                }
                else
                {
                    objDiseaseCarePlacesBll = objDiseaseCarePlacesBll ?? Application.GetContainer().Resolve<IDiseaseCarePlacesBll<DiseaseCarePlaceEntity>>();
                    diseaseCarePlaceBDList = objDiseaseCarePlacesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                diseaseCarePlaceList.Add(new ListItem(Empty, "-1"));
                diseaseCarePlaceList.AddRange(diseaseCarePlaceBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? f.DiseaseCarePlaceDescriptionSpanish : f.DiseaseCarePlaceDescriptionEnglish)
                    .Select(a => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? a.DiseaseCarePlaceDescriptionSpanish : a.DiseaseCarePlaceDescriptionEnglish
                        , a.DiseaseCarePlaceCode.ToString())));


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
            return diseaseCarePlaceList;
        }

        /// <summary>
        /// Load the Reason of not Medical Attention
        /// </summary>
        private List<ListItem> GetsReasonNotMedical()
        {
            List<ListItem> reasonNotMedicalList = new List<ListItem>();
            List<ReasonNotMedicalEntity> reasonNotMedicalBDList = new List<ReasonNotMedicalEntity>();
            try
            {

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical] != null)
                {
                    reasonNotMedicalBDList = (List<ReasonNotMedicalEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical];
                }
                else
                {
                    objReasonNotMedicalBll = objReasonNotMedicalBll ?? Application.GetContainer().Resolve<IReasonNotMedicalBLL<ReasonNotMedicalEntity>>();
                    reasonNotMedicalBDList = objReasonNotMedicalBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                reasonNotMedicalList.Add(new ListItem(Empty, "-1"));
                reasonNotMedicalList.AddRange(reasonNotMedicalBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? f.ReasonNotMedicalDescriptionSpanish : f.ReasonNotMedicalDescriptionEnglish)
                    .Select(a => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? a.ReasonNotMedicalDescriptionSpanish : a.ReasonNotMedicalDescriptionEnglish
                        , a.ReasonNotMedicalCode.ToString())));


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
            return reasonNotMedicalList;
        }

        /// <summary>
        /// Load the Reason of not Attention
        /// </summary>
        private List<ListItem> GetsReasonAttention()
        {
            List<ListItem> reasonNotAtentionList = new List<ListItem>();
            List<ReasonNotAttentionEntity> reasonNotAtentionBDList = new List<ReasonNotAttentionEntity>();
            try
            {

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention] != null)
                {
                    reasonNotAtentionBDList = (List<ReasonNotAttentionEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention];
                }
                else
                {
                    objReasonNotAttentionBll = objReasonNotAttentionBll ?? Application.GetContainer().Resolve<IReasonNotAttentionBLL<ReasonNotAttentionEntity>>();
                    reasonNotAtentionBDList = objReasonNotAttentionBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                reasonNotAtentionList.Add(new ListItem(Empty, "-1"));
                reasonNotAtentionList.AddRange(reasonNotAtentionBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? f.ReasonNotAttentionDescriptionSpanish : f.ReasonNotAttentionDescriptionEnglish)
                    .Select(a => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? a.ReasonNotAttentionDescriptionSpanish : a.ReasonNotAttentionDescriptionEnglish
                        , a.ReasonNotAttentionCode.ToString())));


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
            return reasonNotAtentionList;
        }
        /// <summary>
        /// Load the Disability Types
        /// </summary>
        private void LoadDisabilityTypes()
        {
            try
            {
                List<DisabilityTypeEntity> disabilityTypesBDList = new List<DisabilityTypeEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDisabilityTypes] != null)
                {
                    disabilityTypesBDList = (List<DisabilityTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDisabilityTypes];
                }
                else
                {
                    objDisabilityTypesBll = objDisabilityTypesBll ?? Application.GetContainer().Resolve<IDisabilityTypesBll<DisabilityTypeEntity>>();
                    disabilityTypesBDList = objDisabilityTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                                
                cboTypeDisability.Items.Add(new ListItem(Empty, "-1"));
                cboTypeDisability.Items.AddRange(disabilityTypesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.DisabilityTypeDescriptionSpanish : f.DisabilityTypeDescriptionEnglish)
                    .Select(d => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? d.DisabilityTypeDescriptionSpanish : d.DisabilityTypeDescriptionEnglish
                        , d.DisabilityTypeCode.ToString())).ToArray());
                
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
        /// Gets the total people living with the employee according to the response value of the questions 20 and 21
        /// </summary>
        /// <returns>The total people living with the employee</returns>
        private int GetTotalPeopleLivingWithEmployee()
        {
            int numberOfMen = 0, numberOfWomen = 0;
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.a"));
                    numberOfMen += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.b"));
                    numberOfWomen += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    //
                    hdfNumberOfMen.Value = Convert.ToString(numberOfMen);
                    hdfNumberOfWomen.Value = Convert.ToString(numberOfWomen);
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
            return numberOfMen + numberOfWomen;
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
        private void LoadSurveyAnswers(RepeaterItem item)
        {
            try
            {
                CultureInfo currentCulture = GetCurrentCulture();

                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                SurveyAnswerEntity answer = null;

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    byte familyId = 0;
                    if ( ((Label)item.FindControl("lblFamilyId")).Text == "COLAB"  || ((Label)item.FindControl("lblFamilyId")).Text == "0")
                    {
                        ((Label)item.FindControl("lblFamilyId")).Text = "0";
                        Label lblGuideInformative = item.FindControl("lblGuide") as Label;
                        lblGuideInformative.Text = GetLocalResourceObject("collaborator").ToString();
                    }
                    else
                    {
                        familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                        Label lblGuideInformative = item.FindControl("lblGuide") as Label;
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.a") && a.AnswerItem.Equals(familyId));
                        lblGuideInformative.Text = GetsFamilyRelationShip(Convert.ToInt32(answer.AnswerValue.Trim()))+", "
                                    + surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.c") && a.AnswerItem.Equals(familyId)).AnswerValue;
                        
                    }
                    

                    //
                    DropDownList cbTreatmentPlaces = item.FindControl("cboTreatmentPlacesCare") as DropDownList;
                    if (cbTreatmentPlaces != null)
                    {
                        cbTreatmentPlaces.Items.AddRange(GetsDiseaseCarePlaces().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("24.2") && a.AnswerItem.Equals(familyId));
                        cbTreatmentPlaces.SelectedIndex = -1;
                        ListItem liTreatmentPlace = answer != null ? cbTreatmentPlaces.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liTreatmentPlace != null)
                        {
                            liTreatmentPlace.Selected = true;
                        }
                    }
                    //
                    HtmlInputCheckBox chVisitMedic = item.FindControl("chkVisitMedic") as HtmlInputCheckBox;
                    if (chVisitMedic != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("24.3") && a.AnswerItem.Equals(familyId));
                        chVisitMedic.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cbReasonNotMedical = item.FindControl("cboReasonNotMedic") as DropDownList;
                    cbReasonNotMedical.Enabled = chVisitMedic != null ? !chVisitMedic.Checked : false;
                    if (cbReasonNotMedical != null)
                    {
                        cbReasonNotMedical.Items.AddRange(GetsReasonNotMedical().ToArray());
                        if (!chVisitMedic.Checked) {
                            answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("24.4") && a.AnswerItem.Equals(familyId));
                            cbReasonNotMedical.SelectedIndex = -1;
                            ListItem liReasonNotMedical = answer != null ? cbReasonNotMedical.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                            if (liReasonNotMedical != null)
                            {
                                liReasonNotMedical.Selected = true;
                            }
                        }                        
                    }
                    //
                    HtmlInputCheckBox chHAveAccident = item.FindControl("chkHAveAccident") as HtmlInputCheckBox;
                    if (chHAveAccident != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("24.5") && a.AnswerItem.Equals(familyId));
                        chHAveAccident.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chHAveAttention = item.FindControl("chkHAveAttention") as HtmlInputCheckBox;
                    if (chHAveAttention != null)
                    {
                        if (chHAveAccident.Checked) {
                            answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("24.6") && a.AnswerItem.Equals(familyId));
                            chHAveAttention.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                            chHAveAttention.Attributes.Remove("disabled");

                        }
                        
                    }
                    //
                    DropDownList cbReasonNotAttention = item.FindControl("cboReasonNotAttention") as DropDownList;
                    cbReasonNotAttention.Enabled = chHAveAttention != null ? !chHAveAttention.Checked : false;

                    if (cbReasonNotAttention != null)
                    {
                        if (!chHAveAccident.Checked)
                        {
                            cbReasonNotAttention.Enabled = false;                            
                        }
                            cbReasonNotAttention.Items.AddRange(GetsReasonAttention().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("24.7") && a.AnswerItem.Equals(familyId));
                        cbReasonNotAttention.SelectedIndex = -1;
                        ListItem liReasonNotAtention = answer != null ? cbReasonNotAttention.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liReasonNotAtention != null)
                        {
                            liReasonNotAtention.Selected = true;
                        }
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
                    , "setTimeout(function () { }, 200);", true);
            }
        }
        /// <summary>
        /// Load the survey answers for the current page
        /// </summary>
        private void LoadSurveyAnswersDishabilies()
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                SurveyAnswerEntity answer;
                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {

                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("25") && a.AnswerItem.Equals(1));
                    chkDisability.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    if (chkDisability.Checked)
                    {
                        cboTypeDisability.Attributes.Remove("disabled");
                    }
                    //
                    cboTypeDisability.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("25.1") && a.AnswerItem.Equals(1));
                    ListItem liTypeDisability = answer != null ? cboTypeDisability.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liTypeDisability != null)
                    {
                        liTypeDisability.Selected = true;
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("25.2") && a.AnswerItem.Equals(1));
                    chkDiscapacityGrade.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;

                    DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                    if (workingDivision.DivisionCode.Equals(5))//ECU
                    {
                        chkDiscapacityGrade.Attributes.Remove("disabled");
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
                    , "setTimeout(function () { }, 200);", true);
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
                // QuestionID, AnswerItem, AnswerValue
                List<Tuple<string, byte, string,int>> employeeAnswers = new List<Tuple<string, byte, string,int>>();

                byte familyId = 0;
                foreach (RepeaterItem item in rptFamilyWelfare.Items)
                {
                    if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                    {
                        if (((Label)item.FindControl("lblFamilyId")).Text == "COLAB")
                        {
                            familyId = 0;
                        }
                        else {
                            familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                        }

                        //
                        DropDownList ddlcbTreatmenPlace = item.FindControl("cboTreatmentPlacesCare") as DropDownList;
                        string treatmentPlace = ddlcbTreatmenPlace != null ? ddlcbTreatmenPlace.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(treatmentPlace) && !treatmentPlace.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("24.2", familyId, treatmentPlace, 4));
                        }
                        //
                        HtmlInputCheckBox chVisitMedic = item.FindControl("chkVisitMedic") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("24.3", familyId, chVisitMedic != null ? chVisitMedic.Checked.ToString() : bool.FalseString, 4));
                        //
                        DropDownList ddlcbReasonNotMedic = item.FindControl("cboReasonNotMedic") as DropDownList;
                        string reasonNotMedic = ddlcbReasonNotMedic != null ? ddlcbReasonNotMedic.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(reasonNotMedic) && !reasonNotMedic.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("24.4", familyId, reasonNotMedic, 4));
                        }
                        //
                        HtmlInputCheckBox chHAveAccident = item.FindControl("chkHAveAccident") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("24.5", familyId, chHAveAccident != null ? chHAveAccident.Checked.ToString() : bool.FalseString, 4));
                        //
                        HtmlInputCheckBox chHAveAttention = item.FindControl("chkHAveAttention") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("24.6", familyId, chHAveAttention != null ? chHAveAttention.Checked.ToString() : bool.FalseString, 4));

                        //
                        DropDownList ddlcbReasonNotAttention = item.FindControl("cboReasonNotAttention") as DropDownList;
                        string reasonNotAttention = ddlcbReasonNotAttention != null ? ddlcbReasonNotAttention.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(reasonNotAttention) && !reasonNotAttention.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("24.7", familyId, reasonNotAttention, 4));
                        }
                    }
               }
                bool haveAnyDisability = chkDisability.Checked;
                string disabilityType = cboTypeDisability.SelectedValue;
                string disabilityDegreeGreaterThan75 = Convert.ToString(chkDiscapacityGrade.Checked);

                employeeAnswers.Add(new Tuple<string, byte, string,int>("25", 1, haveAnyDisability.ToString(),4));
                if (haveAnyDisability)
                {
                    cboTypeDisability.Attributes.Remove("disabled");
                }
                if (!IsNullOrWhiteSpace(disabilityType) && !disabilityType.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("25.1", 1, disabilityType,4));
                }
                if (disabilityType.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("25.1", 1, "-1",4));
                }
               // DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                if (!IsNullOrWhiteSpace(disabilityDegreeGreaterThan75) )//ECU
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("25.2", 1, disabilityDegreeGreaterThan75,4));
                }

                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                // Update last modified user for all answers
                if (surveyAnswers.Item2 != null)
                {
                    surveyAnswers.Item2.ForEach(a => a.LastModifiedUser = currentUser);
                }
                ////
                surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Contains("24."));
                surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Contains("25."));
                //
                foreach (var item in employeeAnswers)
                {
                    var answer = surveyAnswers.Item2.Find(v => v.QuestionID.Equals(item.Item1) && v.AnswerItem.Equals(item.Item2));
                    if (answer != null)
                    {
                        answer.AnswerValue = item.Item3;
                        answer.LastModifiedUser = currentUser;
                        answer.SurveyVersion = this.SurveyVersion;
                        answer.IdSurveyModule = 4;
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
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.FamilyWelfare);
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
                    rptFamilyWelfareUpdate();

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
            finally
            {
                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnLoadSurveyAnswers" + Guid.NewGuid().ToString()
                    , "setTimeout(function () {  }, 200);", true);
            }
        }
    }
}