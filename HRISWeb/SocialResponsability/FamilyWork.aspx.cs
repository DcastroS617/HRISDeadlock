using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using System.Threading;
using System.Globalization;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class FamilyWork : System.Web.UI.Page
    {
        [Dependency]
        protected IMaritalStatusBll<MaritalStatusEntity> objMaritalStatusBll { get; set; }
      
        [Dependency]
        protected IFamilyRelationshipsBll<FamilyRelationshipEntity> objFamilyRelationshipsBll { get; set; }
      
        [Dependency]
        protected IAcademicDegreesBll<AcademicDegreeEntity> objAcademicDegreesBll { get; set; }
      
        [Dependency]
        protected IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity> objHouseholdContributionRangesByDivisionsBll { get; set; }
       
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
        
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
       
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }
       
        [Dependency]
        protected IReasonNotWorkBLL<ReasonNotWorkEntity> objReasonNotWorkBll { get; set; }
        
        [Dependency]
        protected IPrincipalProfesionBLL<PrincipalProfesionEntity> objPrincipalProfesionBll { get; set; }
    
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
                ReloadTable();
                GetsHouseholdContributionRangesByDivisions();
                ValidateWorkingDivisionVsEmployeeDivision();
            }
        }

        /// <summary>
        /// Handles the event to refresh de table of family menber
        /// </summary>
        private void ReloadTable()
        {
            int totalPeopleLivingWithEmployee = GetTotalPeopleLivingWithEmployee();
            List<FamiliarEntity> relatives = new List<FamiliarEntity>();
            relatives.AddRange(Enumerable.Range(1, totalPeopleLivingWithEmployee).Select(r => new FamiliarEntity(r)));
            //
            rptFamilyWork.DataSource = relatives;
            rptFamilyWork.DataBind();
        }

        /// <summary>
        /// Handles the lbtnBack click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("MinorChildren.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("Expenses.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnSaveAsDraft click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnSaveAsDraft_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(true);
            ReloadTable();
        }
        /// <summary>
        /// Handles the item data bound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void rptFamilyWork_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LoadSurveyAnswers(e.Item);
            }
        }

        /// <summary>
        /// Gets the Rwason of not work
        /// </summary>
        /// <returns>The academic degrees</returns>
        private List<ListItem> GetsReasonNotWork()
        {
            List<ListItem> reasonNotWork = new List<ListItem>();
            try
            {
                List<ReasonNotWorkEntity> reasonNotWorksBDList;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotWorks] != null)
                {
                    reasonNotWorksBDList = (List<ReasonNotWorkEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotWorks];
                }
                else
                {
                    objReasonNotWorkBll = objReasonNotWorkBll ?? Application.GetContainer().Resolve<IReasonNotWorkBLL<ReasonNotWorkEntity>>();
                    reasonNotWorksBDList = objReasonNotWorkBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                reasonNotWork.Add(new ListItem(Empty, "-1"));
                reasonNotWork.AddRange(reasonNotWorksBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? f.ReasonNotWorkDescriptionSpanish : f.ReasonNotWorkDescriptionEnglish)
                    .Select(a => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? a.ReasonNotWorkDescriptionSpanish : a.ReasonNotWorkDescriptionEnglish
                        , a.ReasonNotWorkCode.ToString())));
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
            return reasonNotWork;
        }
        /// <summary>
        /// Gets the Academic degrees
        /// </summary>
        /// <returns>The academic degrees</returns>
        private List<ListItem> GetsPrincipalProfesion()
        {
            List<ListItem> principalProfesion = new List<ListItem>();
            try
            {
                List<PrincipalProfesionEntity> principalProfesionBDList;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPrincipalProfesion] != null)
                {
                    principalProfesionBDList = (List<PrincipalProfesionEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPrincipalProfesion];
                }
                else
                {
                    objPrincipalProfesionBll = objPrincipalProfesionBll ?? Application.GetContainer().Resolve<IPrincipalProfesionBLL<PrincipalProfesionEntity>>();
                    principalProfesionBDList = objPrincipalProfesionBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                principalProfesion.Add(new ListItem(Empty, "-1"));
                principalProfesion.AddRange(principalProfesionBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? f.PrincipalProfesionDescriptionSpanish : f.PrincipalProfesionDescriptionEnglish)
                    .Select(a => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? a.PrincipalProfesionDescriptionSpanish : a.PrincipalProfesionDescriptionEnglish
                        , a.PrincipalProfesionCode.ToString())));
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
            return principalProfesion;
        }

        /// <summary>
        /// Load the Family Relationship
        /// </summary>
        public string GetsFamilyRelationShip(int number)
        {
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

            if (currentCulture.Name.Equals(Constants.cCultureEsCR))
            {
                return familyRelationships.Find(r => r.FamilyRelationshipCode == number).FamilyRelationshipDescriptionSpanish;
            }
            else
            {
                return familyRelationships.Find(r => r.FamilyRelationshipCode == number).FamilyRelationshipDescriptionEnglish;

            }
        }
        /// <summary>
        /// Gets the Household Contribution Ranges By Divisions
        /// </summary>
        /// <returns>The household contribution ranges</returns>
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
                    householdContributionRangeByDivisionBDList = objHouseholdContributionRangesByDivisionsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                int workingDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                List<HouseHoldContributionRangeByDivisionEntity> rangesByDivision = householdContributionRangeByDivisionBDList
                    .Where(r => r.DivisionCode.Equals(workingDivisionCode) && r.SearchEnabled && r.Deleted == false).ToList();

                howMuch.Add(new ListItem(Empty, "-1"));
                howMuch.AddRange(rangesByDivision.Select(m => new ListItem(m.RangeOrder.ToString(), m.RangeOrder.ToString())));

                // Set middle ranges
                rangesByDivision.ForEach(r =>
                {
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
        /// <param name="item">The repeater item for the familiar</param>
        private void LoadSurveyAnswers(RepeaterItem item)
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                SurveyAnswerEntity answer = null;

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    byte familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                    //

                    Label lblGuideInformative = item.FindControl("lblGuide") as Label;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.a") && a.AnswerItem.Equals(familyId));
                    lblGuideInformative.Text = GetsFamilyRelationShip(Convert.ToInt32(answer.AnswerValue.Trim())) + ", "
                                    + surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.c") && a.AnswerItem.Equals(familyId)).AnswerValue;
                    //
                    HtmlInputCheckBox chWork = item.FindControl("chkWork") as HtmlInputCheckBox;
                    if (chWork != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.b") && a.AnswerItem.Equals(familyId));
                        chWork.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;                        
                    }
                    //
                    HtmlInputCheckBox chWorkDole = item.FindControl("chkDole") as HtmlInputCheckBox;
                    chWorkDole?.Attributes?.Remove("disabled");
                    chWorkDole?.Attributes?.Remove("readOnly");
                    if (!chWork.Checked)
                    {
                        chWorkDole?.Attributes?.Add("disabled", "disabled");
                        chWorkDole?.Attributes?.Add("readOnly", "readOnly");
                    }
                    if (chWorkDole != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.c") && a.AnswerItem.Equals(familyId));
                        chWorkDole.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chSocialMedic = item.FindControl("chkSocialMedic") as HtmlInputCheckBox;
                    if (chSocialMedic != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.d") && a.AnswerItem.Equals(familyId));
                        chSocialMedic.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chHaveHelp = item.FindControl("chkHaveHelp") as HtmlInputCheckBox;
                    if (chHaveHelp != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.e") && a.AnswerItem.Equals(familyId));
                        chHaveHelp.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chCurrentlyPaying = item.FindControl("chkCurrentlyPaying") as HtmlInputCheckBox;
                    if (chCurrentlyPaying != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.f") && a.AnswerItem.Equals(familyId));
                        chCurrentlyPaying.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chRetired = item.FindControl("chkRetired") as HtmlInputCheckBox;
                    if (chRetired != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.g") && a.AnswerItem.Equals(familyId));
                        chRetired.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chHouseHelp = item.FindControl("chkHouseHelp") as HtmlInputCheckBox;
                    if (chHouseHelp != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.h") && a.AnswerItem.Equals(familyId));
                        chHouseHelp.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cboHouseHelpItem = item.FindControl("cboHouseHelp") as DropDownList;
                    if (chHouseHelp.Checked)
                    {
                        cboHouseHelpItem?.Attributes.Remove("disabled");
                    }
                    if (cboHouseHelpItem != null)
                    {
                        cboHouseHelpItem.Items.Clear();
                        cboHouseHelpItem.Items.AddRange(GetsHouseholdContributionRangesByDivisions().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.h.a") && a.AnswerItem.Equals(familyId));
                        if (answer != null)
                        {
                            cboHouseHelpItem.SelectedIndex = -1;
                            ListItem liHowMuch = cboHouseHelpItem.Items.FindByValue(answer.AnswerValue.Trim());
                            if (liHowMuch != null)
                            {
                                liHowMuch.Selected = true;
                            }
                        }
                    }
                    //
                    DropDownList cbprofesion = item.FindControl("cboProfession") as DropDownList;
                    cbprofesion.Enabled = chWork != null ? chWork.Checked : false;
                    if (cbprofesion != null)
                    {
                        cbprofesion.Items.AddRange(GetsPrincipalProfesion().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.i") && a.AnswerItem.Equals(familyId));
                        cbprofesion.SelectedIndex = -1;
                        ListItem liProfesion = answer != null ? cbprofesion.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liProfesion != null)
                        {
                            liProfesion.Selected = true;
                        }
                    }
                    //
                    HtmlInputCheckBox chDoBussines = item.FindControl("chkDoBussines") as HtmlInputCheckBox;
                    chDoBussines?.Attributes?.Remove("disabled");
                    chDoBussines?.Attributes?.Remove("readOnly");
                    if (chWork.Checked)
                    {
                        chDoBussines?.Attributes?.Add("disabled", "disabled");
                        chDoBussines?.Attributes?.Add("readOnly", "readOnly");
                    }
                    if (chDoBussines != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.j") && a.AnswerItem.Equals(familyId));
                        chDoBussines.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cbReasonNotwork = item.FindControl("cboReasonNotwork") as DropDownList;
                    if (!chWork.Checked)
                    {
                        cbReasonNotwork.Enabled = !chDoBussines.Checked;
                    }
                    else {
                        cbReasonNotwork.Enabled = false;
                    }
                    if (cbReasonNotwork != null)
                    {
                        cbReasonNotwork.Items.AddRange(GetsReasonNotWork().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("21.k") && a.AnswerItem.Equals(familyId));
                        cbReasonNotwork.SelectedIndex = -1;
                        ListItem liReason = answer != null ? cbReasonNotwork.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liReason != null)
                        {
                            liReason.Selected = true;
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
                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();

                byte familyId = 0;
                foreach (RepeaterItem item in rptFamilyWork.Items)
                {
                    if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                    {
                        familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                        //
                        HtmlInputCheckBox chWork = item.FindControl("chkWork") as HtmlInputCheckBox;
                        bool work = chWork != null ? chWork.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.b", familyId, work.ToString(), 3));
                        //
                        HtmlInputCheckBox chWorkDole = item.FindControl("chkDole") as HtmlInputCheckBox;
                        chWorkDole?.Attributes?.Remove("disabled");
                        chWorkDole?.Attributes?.Remove("readOnly");
                        if (!chWork.Checked)
                        {
                            chWorkDole?.Attributes?.Add("disabled", "disabled");
                            chWorkDole?.Attributes?.Add("readOnly", "readOnly");
                        }
                        bool workDole = chWorkDole != null ? chWorkDole.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.c", familyId, workDole.ToString(), 3));
                        //
                        HtmlInputCheckBox chSocialMedic = item.FindControl("chkSocialMedic") as HtmlInputCheckBox;
                        bool socialMedic = chSocialMedic != null ? chSocialMedic.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.d", familyId, socialMedic.ToString(), 3));
                        //
                        HtmlInputCheckBox chHaveHelp = item.FindControl("chkHaveHelp") as HtmlInputCheckBox;
                        bool haveHelp = chHaveHelp != null ? chHaveHelp.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.e", familyId, haveHelp.ToString(), 3));
                        //
                        HtmlInputCheckBox chCurrentlyPaying = item.FindControl("chkCurrentlyPaying") as HtmlInputCheckBox;
                        bool currentlyPaying = chCurrentlyPaying != null ? chCurrentlyPaying.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.f", familyId, currentlyPaying.ToString(), 3));
                        //
                        HtmlInputCheckBox chRetired = item.FindControl("chkRetired") as HtmlInputCheckBox;
                        bool retired = chRetired != null ? chRetired.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.g", familyId, retired.ToString(), 3));
                        //
                        HtmlInputCheckBox chkHouseHelp = item.FindControl("chkHouseHelp") as HtmlInputCheckBox;
                        bool houseHelp = chkHouseHelp != null ? chkHouseHelp.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.h", familyId, houseHelp.ToString(), 3));
                        //
                        DropDownList ddlHouseHelp = item.FindControl("cboHouseHelp") as DropDownList;
                        if (chkHouseHelp.Checked)
                        {
                            ddlHouseHelp?.Attributes.Remove("disabled");
                            string houseHelpMount = ddlHouseHelp != null ? ddlHouseHelp.SelectedValue : Empty;
                            if (!IsNullOrWhiteSpace(houseHelpMount) && !houseHelp.Equals("-1"))
                            {
                                employeeAnswers.Add(new Tuple<string, byte, string, int>("21.h.a", familyId, houseHelpMount, 3));
                            }
                            ListItem liHouseHelpt = ddlHouseHelp.Items.FindByValue(houseHelpMount);
                            if (liHouseHelpt != null)
                            {
                                liHouseHelpt.Selected = true;
                            }
                        }
                        //
                        DropDownList ddlProfession = item.FindControl("cboProfession") as DropDownList;
                        ddlProfession.Enabled = chWork.Checked;
                        string actualProfession = ddlProfession != null ? ddlProfession.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(actualProfession) && !actualProfession.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("21.i", familyId, actualProfession, 3));
                        }
                        //
                        HtmlInputCheckBox chDoBussines = item.FindControl("chkDoBussines") as HtmlInputCheckBox;
                        chDoBussines?.Attributes?.Remove("disabled");
                        chDoBussines?.Attributes?.Remove("readOnly");
                        if (chWork.Checked)
                        {
                            chDoBussines?.Attributes?.Add("disabled", "disabled");
                            chDoBussines?.Attributes?.Add("readOnly", "readOnly");
                        }
                        bool doBussines = chDoBussines != null ? chDoBussines.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("21.j", familyId, doBussines.ToString(), 3));
                        //
                        DropDownList ddlReasonNotwork = item.FindControl("cboReasonNotwork") as DropDownList;
                        ddlReasonNotwork.Enabled = chDoBussines.Checked;
                        string reasonNotwork = ddlReasonNotwork != null ? ddlReasonNotwork.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(reasonNotwork) && !reasonNotwork.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("21.k", familyId, reasonNotwork, 3));
                        }
                    }
                }

                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                // Update last modified user for all answers
                surveyAnswers?.Item2?.ForEach(a => a.LastModifiedUser = currentUser);
                // Remove all answers for the questions 21
                surveyAnswers?.Item2?.RemoveAll(r => r.QuestionID.Contains("21."));
                //
                foreach (var item in employeeAnswers)
                {
                    var answer = surveyAnswers.Item2.Find(v => v.QuestionID.Equals(item.Item1) && v.AnswerItem.Equals(item.Item2));
                    if (answer != null)
                    {
                        answer.AnswerValue = item.Item3;
                        answer.LastModifiedUser = currentUser;
                        answer.SurveyVersion = this.SurveyVersion;
                        answer.IdSurveyModule = 3;
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
                            , item.Item4)
                        { SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.FamilyGroup);
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
                        , "ReturnFromProcessSaveAsDraftResponse{0}" + Guid.NewGuid().ToString()
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
    }
}