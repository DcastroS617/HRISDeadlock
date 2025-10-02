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
using System.Web.Services;
using System.Web.Helpers;

namespace HRISWeb.SocialResponsability
{
    public partial class MinorChildren : System.Web.UI.Page
    {
        [Dependency]
        protected IMaritalStatusBll<MaritalStatusEntity> objMaritalStatusBll { get; set; }

        [Dependency]
        protected IAcademicDegreesBll<AcademicDegreeEntity> objAcademicDegreesBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }

        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }

        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }

        [Dependency]
        protected IFamilyRelationshipsBll<FamilyRelationshipEntity> objFamilyRelationshipsBll { get; set; }

        [Dependency]
        protected IYearAcademicDegreesBLL<YearAcademicDegreesEntity> objYearAcademicDegreesBll { get; set; }
        [Dependency]
        protected IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity> objHouseholdContributionRangesByDivisionsBll { get; set; }

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
                LoadNumberOfMinorChildren();
                LoadSurveyAnswersMainQuestion();
                ValidateWorkingDivisionVsEmployeeDivision();
                GetsHouseholdContributionRangesByDivisions();
            }
            else
            {
                ConfigureQuestionStudy();
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
            Response.Redirect(GetRedirectPage(false), false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect(GetRedirectPage(true), false);
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
        protected void rptMinorChildren_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LoadSurveyAnswers(e.Item);
            }
        }
        /// <summary>
        /// Handles the selected index change event for the control cboNumberOfMinorChildren
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboNumberOfMinorChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadsChildrenInformationGrid();
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
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("19.a"));
                    numberOfMen += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("19.b"));
                    numberOfWomen += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;

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

        /// <summary>
        /// Gets the marital status
        /// </summary>
        /// <returns>The marital status</returns>
        private List<ListItem> GetsMaritalStatus()
        {
            List<ListItem> maritalStatusList = new List<ListItem>();
            try
            {
                List<MaritalStatusEntity> maritalStatusBDList = new List<MaritalStatusEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogMaritalStatus] != null)
                {
                    maritalStatusBDList = (List<MaritalStatusEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogMaritalStatus];
                }
                else
                {
                    objMaritalStatusBll = objMaritalStatusBll ?? Application.GetContainer().Resolve<IMaritalStatusBll<MaritalStatusEntity>>();
                    maritalStatusBDList = objMaritalStatusBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                maritalStatusList.Add(new ListItem(Empty, "-1"));
                maritalStatusList.AddRange(maritalStatusBDList.Select(m => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR) ?
                    m.MaritalStatusDescriptionSpanish : m.MaritalStatusDescriptionEnglish
                    , m.MaritalStatusCode.ToString())));
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
            return maritalStatusList;
        }
        /// <summary>
        /// Gets the Academic degrees
        /// </summary>
        /// <returns>The academic degrees</returns>
        private List<AcademicDegreeEntity> GetsAcademicDegrees()
        {
            List<AcademicDegreeEntity> academicDegreesBDList = new List<AcademicDegreeEntity>();
            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] != null)
                {
                    academicDegreesBDList = (List<AcademicDegreeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees];
                }
                else
                {
                    objAcademicDegreesBll = objAcademicDegreesBll ?? Application.GetContainer().Resolve<IAcademicDegreesBll<AcademicDegreeEntity>>();
                    academicDegreesBDList = objAcademicDegreesBll.ListEnabled();
                    var listJson = Json.Encode(academicDegreesBDList);
                    string script = string.Format("localStorage.AcademicDegrees= '{0}';", listJson);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "AcademicDegrees", script, true);
                }

                CultureInfo currentCulture = GetCurrentCulture();

                academicDegreesBDList = academicDegreesBDList.Where(x => x.DegreeFormationTypeCode == 1).ToList();

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
            return academicDegreesBDList;
        }
        /// <summary>
        /// Gets a list of genders
        /// </summary>
        /// <returns>The genders</returns>
        private List<ListItem> GetGenders()
        {
            List<ListItem> gender = new List<ListItem>();
            gender.Add(new ListItem(Empty, "-1"));
            gender.Add(new ListItem("M", "M"));
            gender.Add(new ListItem("F", "F"));

            return gender;
        }
        /// <summary>
        /// Gets a list of ages
        /// </summary>
        /// <returns>The ages</returns>
        private List<ListItem> GetsAge()
        {
            List<ListItem> age = new List<ListItem>();
            age.Add(new ListItem(Empty, "-1"));
            // 21
            age.AddRange(Enumerable.Range(0, 22).Select(j => new ListItem(j.ToString(), j.ToString())));
            return age;
        }
        /// <summary>
        /// Gets the study years
        /// </summary>
        /// <returns>The study years</returns>
        /// 

        public List<YearAcademicDegreesEntity> GetsStudyYears(int academicDegree, bool coursing)
        {

            List<YearAcademicDegreesEntity> yearAcademicBDlist;

            if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogYearAcademicDegrees] != null)
            {
                yearAcademicBDlist = (List<YearAcademicDegreesEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogYearAcademicDegrees];
            }
            else
            {
                objYearAcademicDegreesBll = objYearAcademicDegreesBll ?? Application.GetContainer().Resolve<IYearAcademicDegreesBLL<YearAcademicDegreesEntity>>();
                yearAcademicBDlist = objYearAcademicDegreesBll.ListEnabled();
                var listJson = Json.Encode(yearAcademicBDlist);
                string script = string.Format("localStorage.YearDegrees= '{0}';", listJson);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "YearDegrees", script, true);
            }
            Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogYearAcademicDegrees] = yearAcademicBDlist;
            return yearAcademicBDlist.Where(x => x.AcademicDegreeCode == academicDegree && x.DivisionCode == SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode && x.Coursing == coursing).ToList();
        }

        private void ConfigureQuestionStudy()
        {
            foreach (RepeaterItem item in rptMinorChildren.Items)
            {
                //
                HtmlInputCheckBox cbCurrentlyStudying = item.FindControl("chkCurrentlyStudying") as HtmlInputCheckBox;
                if (cbCurrentlyStudying != null)
                {
                    if (cbCurrentlyStudying.Checked)
                    {
                        DropDownList cboAcademicDegreeItem = item.FindControl("cboAcademicDegree") as DropDownList;
                        DropDownList cboStudyYearItem = item.FindControl("cboStudyYear") as DropDownList;

                        cboAcademicDegreeItem?.Attributes?.Remove("disabled");
                        cboStudyYearItem?.Attributes?.Remove("disabled");
                    }
                }
            }
        }
        /// <summary>
        /// Loads the control for the available number of children under legal age who do not live with you
        /// </summary>
        private void LoadNumberOfMinorChildren()
        {
            cboNumberOfMinorChildren.Items.AddRange(Enumerable.Range(0, GetTotalPeopleLivingWithEmployee() + 1).Select(x => new ListItem(x.ToString(), x.ToString())).ToArray());
        }
        /// <summary>
        /// Loads the children information grid with the number of rows requested
        /// </summary>
        private void LoadsChildrenInformationGrid()
        {
            List<FamiliarEntity> relatives = null;

            if (cboNumberOfMinorChildren.SelectedItem != null
                && !IsNullOrWhiteSpace(cboNumberOfMinorChildren.SelectedValue)
                && !cboNumberOfMinorChildren.SelectedValue.Equals("-1"))
            {
                int numberOfChildren = Convert.ToInt32(cboNumberOfMinorChildren.SelectedValue);
                if (numberOfChildren > 0)
                {
                    relatives = new List<FamiliarEntity>();
                    relatives.AddRange(Enumerable.Range(1, numberOfChildren).Select(f => new FamiliarEntity(f)));
                }
            }
            rptMinorChildren.DataSource = relatives;
            rptMinorChildren.DataBind();
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
        /// Loads the answer value for the question 27, the number of total children under legal age who do not live with you
        /// </summary>
        private void LoadSurveyAnswersMainQuestion()
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    cboNumberOfMinorChildren.SelectedIndex = -1;
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2") && a.AnswerItem.Equals(1));
                    ListItem liNumberOfMinorChildren = answer != null ? cboNumberOfMinorChildren.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberOfMinorChildren != null)
                    {
                        liNumberOfMinorChildren.Selected = true;
                        cboNumberOfMinorChildren_SelectedIndexChanged(null, null);
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
        /// Load the survey answers for the current page
        /// </summary>
        /// <param name="item">The repeater item for the familiar</param>
        private void LoadSurveyAnswers(RepeaterItem item)
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                CultureInfo currentCulture = GetCurrentCulture();
                List<AcademicDegreeEntity> academicDegreesBDList;

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    byte familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                    SurveyAnswerEntity answer = null;
                    //
                    DropDownList cboRelationshipItem = item.FindControl("cboRelationship") as DropDownList;
                    if (cboRelationshipItem != null)
                    {
                        cboRelationshipItem.Items.AddRange(GetsFamilyRelationships().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.a") && a.AnswerItem.Equals(familyId));
                        cboRelationshipItem.SelectedIndex = -1;
                        ListItem liRelationship = answer != null ? cboRelationshipItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liRelationship != null)
                        {
                            liRelationship.Selected = true;
                        }
                    }
                    //
                    DropDownList cboGenderItem = item.FindControl("cboGender") as DropDownList;
                    if (cboGenderItem != null)
                    {
                        cboGenderItem.Items.AddRange(GetGenders().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.b") && a.AnswerItem.Equals(familyId));
                        cboGenderItem.SelectedIndex = -1;
                        ListItem liGender = answer != null ? cboGenderItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liGender != null)
                        {
                            liGender.Selected = true;
                        }
                    }
                    int cboAge = 0;
                    //
                    DropDownList cboAgeItem = item.FindControl("cboAge") as DropDownList;
                    if (cboAgeItem != null)
                    {
                        cboAgeItem.Items.AddRange(GetsAge().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.c") && a.AnswerItem.Equals(familyId));
                        cboAgeItem.SelectedIndex = -1;
                        ListItem liAge = answer != null ? cboAgeItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liAge != null)
                        {
                            liAge.Selected = true;
                        }
                        if (answer != null)
                        {
                            cboAge = Convert.ToInt32(answer.AnswerValue);
                        }
                    }
                    //
                    DropDownList cboMaritalStatusItem = item.FindControl("cboMaritalStatus") as DropDownList;
                    if (cboMaritalStatusItem != null)
                    {
                        cboMaritalStatusItem.Items.AddRange(GetsMaritalStatus().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.d") && a.AnswerItem.Equals(familyId));
                        cboMaritalStatusItem.SelectedIndex = -1;
                        ListItem liMaritalStatus = answer != null ? cboMaritalStatusItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liMaritalStatus != null)
                        {
                            liMaritalStatus.Selected = true;
                        }
                    }
                    //
                    HtmlInputCheckBox cbReadWrite = item.FindControl("chkReadWrite") as HtmlInputCheckBox;
                    if (cbReadWrite != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.e") && a.AnswerItem.Equals(familyId));
                        cbReadWrite.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cboLastAcademicAprovedItem = item.FindControl("cboLastAcademicAproved") as DropDownList;
                    if (cboLastAcademicAprovedItem != null)
                    {
                        academicDegreesBDList = GetsAcademicDegrees();
                        cboLastAcademicAprovedItem.Items.Clear();
                        cboLastAcademicAprovedItem.Items.Add(new ListItem(Empty, "-1"));
                        cboLastAcademicAprovedItem.Items.AddRange(academicDegreesBDList.OrderBy(b => b.Orderlist)
                            .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                                ? g.AcademicDegreeDescriptionSpanish : g.AcademicDegreeDescriptionEnglish
                                , g.AcademicDegreeCode.ToString())).ToArray());

                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.f") && a.AnswerItem.Equals(familyId));
                        cboLastAcademicAprovedItem.SelectedIndex = -1;
                        ListItem liAcademicDegreeAproved = answer != null ? cboLastAcademicAprovedItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liAcademicDegreeAproved != null)
                        {
                            liAcademicDegreeAproved.Selected = true;
                        }
                        cboLastAcademicAprovedItem.Enabled = !(cboAge < 5);
                    }
                    //
                    DropDownList cboLastStudyYearItem = item.FindControl("cboLastStudyYearAproved") as DropDownList;
                    if (cboLastStudyYearItem != null)
                    {
                        var years = GetsStudyYears(Convert.ToInt32(cboLastAcademicAprovedItem.SelectedValue), false);
                        cboLastStudyYearItem.Items.Clear();
                        if (years.Count > 1)
                        {
                            cboLastStudyYearItem.Items.Add(new ListItem(Empty, "-1"));
                        }
                        cboLastStudyYearItem.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.g") && a.AnswerItem.Equals(familyId));
                        cboLastStudyYearItem.SelectedIndex = -1;
                        ListItem liStudyYearApproved = answer != null ? cboLastStudyYearItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liStudyYearApproved != null)
                        {
                            liStudyYearApproved.Selected = true;
                        }
                        cboLastStudyYearItem.Enabled = !(cboAge < 5);
                    }
                    //
                    HtmlInputCheckBox cbCurrentlyStudying = item.FindControl("chkCurrentlyStudying") as HtmlInputCheckBox;
                    if (cbCurrentlyStudying != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.h") && a.AnswerItem.Equals(familyId));
                        cbCurrentlyStudying.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cboAcademicDegreeItem = item.FindControl("cboAcademicDegree") as DropDownList;
                    if (cboAcademicDegreeItem != null)
                    {
                        if (cbCurrentlyStudying.Checked)
                        {
                            cboAcademicDegreeItem.Attributes.Remove("disabled");
                        }
                        academicDegreesBDList = GetsAcademicDegrees();
                        var degreeComplete = academicDegreesBDList.Where(f => f.AcademicDegreeDescriptionSpanish.Contains("Completo") || f.AcademicDegreeDescriptionSpanish.Contains("Completado")
                        || f.AcademicDegreeDescriptionEnglish.Contains("Completed") || f.AcademicDegreeDescriptionEnglish.Contains("Complete")
                        ).ToList();

                        foreach (var degree in degreeComplete)
                        {
                            academicDegreesBDList.Remove(degree);
                        }

                        cboAcademicDegreeItem.Items.Add(new ListItem(Empty, "-1"));
                        cboAcademicDegreeItem.Items.AddRange(academicDegreesBDList.OrderBy(b => b.Orderlist)
                            .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                                ? g.AcademicDegreeDescriptionSpanish : g.AcademicDegreeDescriptionEnglish
                                , g.AcademicDegreeCode.ToString())).ToArray());

                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.i") && a.AnswerItem.Equals(familyId));
                        cboAcademicDegreeItem.SelectedIndex = -1;
                        ListItem liAcademicDegree = answer != null ? cboAcademicDegreeItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liAcademicDegree != null)
                        {
                            liAcademicDegree.Selected = true;
                        }
                    }
                    //
                    DropDownList cboStudyYearItem = item.FindControl("cboStudyYear") as DropDownList;
                    if (cboStudyYearItem != null)
                    {

                        if (cbCurrentlyStudying.Checked)
                        {
                            cboStudyYearItem.Attributes.Remove("disabled");
                        }
                        var years = GetsStudyYears(Convert.ToInt32(cboAcademicDegreeItem.SelectedValue), true);
                        cboStudyYearItem.Items.Clear();
                        if (years.Count > 1)
                        {
                            cboStudyYearItem.Items.Add(new ListItem(Empty, "-1"));
                        }
                        cboStudyYearItem.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.j") && a.AnswerItem.Equals(familyId));
                        cboStudyYearItem.SelectedIndex = -1;
                        ListItem liStudyYear = answer != null ? cboStudyYearItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liStudyYear != null)
                        {
                            liStudyYear.Selected = true;
                        }
                        cboStudyYearItem.Enabled = !(cboAge < 5);
                    }                  
                    //
                    HtmlInputCheckBox cbSocialSecurity = item.FindControl("chkSocialSecurity") as HtmlInputCheckBox;
                    if (cbSocialSecurity != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.k") && a.AnswerItem.Equals(familyId));
                        cbSocialSecurity.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox cbHelps = item.FindControl("chkHelps") as HtmlInputCheckBox;
                    if (cbHelps != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.l") && a.AnswerItem.Equals(familyId));
                        cbHelps.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox cbWorks = item.FindControl("chkWorks") as HtmlInputCheckBox;
                    if (cbWorks != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.m") && a.AnswerItem.Equals(familyId));
                        cbWorks.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox chkHouseHelpt = item.FindControl("chkHouseHelpt") as HtmlInputCheckBox;
                    if (chkHouseHelpt != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.n") && a.AnswerItem.Equals(familyId));
                        chkHouseHelpt.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cboHouseHelpItem = item.FindControl("cboHouseHelp") as DropDownList;
                    if (chkHouseHelpt.Checked)
                    {
                        cboHouseHelpItem?.Attributes.Remove("disabled");
                    }
                    if (cboHouseHelpItem != null)
                    {
                        cboHouseHelpItem.Items.Clear();
                        cboHouseHelpItem.Items.AddRange(GetsHouseholdContributionRangesByDivisions().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.2.n.a") && a.AnswerItem.Equals(familyId));
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
                //
                string numberOfChildren = cboNumberOfMinorChildren.SelectedValue;
                if (!IsNullOrWhiteSpace(numberOfChildren) && !numberOfChildren.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2", 1, numberOfChildren, 3));
                }
                //
                byte familyId = 0;
                foreach (RepeaterItem item in rptMinorChildren.Items)
                {
                    if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                    {
                        familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                        //
                        DropDownList ddlRelationship = item.FindControl("cboRelationship") as DropDownList;
                        string relationship = ddlRelationship != null ? ddlRelationship.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(relationship) && !relationship.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.a", familyId, relationship, 3));
                        }
                        //
                        DropDownList ddlGender = item.FindControl("cboGender") as DropDownList;
                        string gender = ddlGender != null ? ddlGender.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(gender) && !gender.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.b", familyId, gender, 3));
                        }
                        //
                        DropDownList ddlAge = item.FindControl("cboAge") as DropDownList;
                        string age = ddlAge != null ? ddlAge.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(age) && !age.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.c", familyId, age, 3));
                        }
                        //
                        DropDownList ddlMaritalStatus = item.FindControl("cboMaritalStatus") as DropDownList;
                        string maritalStatus = ddlMaritalStatus != null ? ddlMaritalStatus.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(maritalStatus) && !maritalStatus.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.d", familyId, maritalStatus, 3));
                        }
                        //
                        HtmlInputCheckBox cbReadWrite = item.FindControl("chkReadWrite") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.e", familyId, cbReadWrite != null ? cbReadWrite.Checked.ToString() : bool.FalseString, 3));
                        //
                        DropDownList ddlLastAcademicAproved = item.FindControl("cboLastAcademicAproved") as DropDownList;                        
                        ddlLastAcademicAproved.Enabled = !(Convert.ToInt32(age) < 5);
                        string lastAcademicAproved = ddlLastAcademicAproved != null ? ddlLastAcademicAproved.SelectedValue : Empty;                        
                        if (!IsNullOrWhiteSpace(lastAcademicAproved) && !lastAcademicAproved.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.f", familyId, lastAcademicAproved, 3));
                        }
                        //
                        DropDownList ddlLastStudyYearAproved = item.FindControl("cboLastStudyYearAproved") as DropDownList;
                        HiddenField hdfLastYearAprovedItem = item.FindControl("hdfLastYearAproved") as HiddenField;
                        string lastStudyYearAproved = ddlLastStudyYearAproved != null ? ddlLastStudyYearAproved.SelectedValue : Empty;
                        lastStudyYearAproved = IsNullOrEmpty(lastStudyYearAproved) ? hdfLastYearAprovedItem.Value : lastStudyYearAproved;

                        ddlLastStudyYearAproved.Enabled = !(Convert.ToInt32(age) < 5);

                        var yearsAproved = GetsStudyYears(Convert.ToInt32(ddlLastAcademicAproved.SelectedValue), false);
                        ddlLastStudyYearAproved.Items.Clear();
                        if (yearsAproved.Count > 1)
                        {
                            ddlLastStudyYearAproved.Items.Add(new ListItem(Empty, "-1"));
                        }
                        ddlLastStudyYearAproved.Items.AddRange(yearsAproved.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        ListItem liStudyYearAproved = ddlLastStudyYearAproved.Items.FindByValue(lastStudyYearAproved);
                        if (liStudyYearAproved != null)
                        {
                            liStudyYearAproved.Selected = true;
                        }
                       
                        if (!IsNullOrWhiteSpace(lastStudyYearAproved) && !lastStudyYearAproved.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.g", familyId, lastStudyYearAproved, 3));
                        }
                        //
                        HtmlInputCheckBox cbCurrentlyStudying = item.FindControl("chkCurrentlyStudying") as HtmlInputCheckBox;
                        bool currentlyStudying = cbCurrentlyStudying != null ? cbCurrentlyStudying.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.h", familyId, currentlyStudying.ToString(), 3));
                        //
                        DropDownList ddlAcademicDegree = item.FindControl("cboAcademicDegree") as DropDownList;
                        if (currentlyStudying)
                        {
                            ddlAcademicDegree?.Attributes.Remove("disabled");
                        }
                        ddlAcademicDegree.Enabled = !(Convert.ToInt32(age) < 5);
                        string academicDegree = ddlAcademicDegree != null ? ddlAcademicDegree.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(academicDegree) && !academicDegree.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.i", familyId, academicDegree, 3));
                        }
                        //
                        DropDownList ddlStudyYear = item.FindControl("cboStudyYear") as DropDownList;
                        HiddenField hdfStudyYearItem = item.FindControl("hdfStudyYear") as HiddenField;
                        if (currentlyStudying)
                        {
                            ddlStudyYear?.Attributes.Remove("disabled");
                            ddlStudyYear.Enabled = true;
                        }
                        string studyYear = ddlStudyYear != null ? ddlStudyYear.SelectedValue : Empty;
                        studyYear = IsNullOrEmpty(studyYear) ? hdfStudyYearItem.Value : studyYear;
                        if (!IsNullOrWhiteSpace(studyYear) && !studyYear.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.j", familyId, studyYear, 3));
                        }
                        var years = GetsStudyYears(Convert.ToInt32(ddlAcademicDegree.SelectedValue), true);
                        ddlStudyYear.Items.Clear();
                        if (years.Count > 1)
                        {
                            ddlStudyYear.Items.Add(new ListItem(Empty, "-1"));
                        }
                        ddlStudyYear.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        ListItem liStudyYear = ddlStudyYear.Items.FindByValue(studyYear);
                        if (liStudyYear != null)
                        {
                            liStudyYear.Selected = true;
                        }                        
                        //
                        HtmlInputCheckBox cbSocialSecurity = item.FindControl("chkSocialSecurity") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.k", familyId, cbSocialSecurity != null ? cbSocialSecurity.Checked.ToString() : bool.FalseString, 3));
                        //
                        HtmlInputCheckBox cbHelps = item.FindControl("chkHelps") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.l", familyId, cbHelps != null ? cbHelps.Checked.ToString() : bool.FalseString, 3));
                        //
                        HtmlInputCheckBox cbWorks = item.FindControl("chkWorks") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.m", familyId, cbWorks != null ? cbWorks.Checked.ToString() : bool.FalseString, 3));
                        //
                        HtmlInputCheckBox chkHouseHelpt = item.FindControl("chkHouseHelpt") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.n", familyId, cbWorks != null ? chkHouseHelpt.Checked.ToString() : bool.FalseString, 3));
                        //
                        DropDownList ddlHouseHelp = item.FindControl("cboHouseHelp") as DropDownList;
                        if (chkHouseHelpt.Checked)
                        {
                            ddlHouseHelp?.Attributes.Remove("disabled");
                            string houseHelp = ddlHouseHelp != null ? ddlHouseHelp.SelectedValue : Empty;
                            if (!IsNullOrWhiteSpace(houseHelp) && !houseHelp.Equals("-1"))
                            {
                                employeeAnswers.Add(new Tuple<string, byte, string, int>("20.2.n.a", familyId, houseHelp, 3));
                            }
                            ListItem liHouseHelpt = ddlHouseHelp.Items.FindByValue(houseHelp);
                            if (liHouseHelpt != null)
                            {
                                liHouseHelpt.Selected = true;
                            }
                        }
                    }
                }

                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                // Update last modified user for all answers
                surveyAnswers?.Item2?.ForEach(a => a.LastModifiedUser = currentUser);
                // Remove all answers for the questions 27
                surveyAnswers?.Item2?.RemoveAll(r => r.QuestionID.Contains("20.2"));
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
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.MinorChildren);
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

        /// <summary>
        /// Gets the family relationships
        /// </summary>
        /// <returns>The family relationships</returns>
        private List<ListItem> GetsFamilyRelationships()
        {
            List<ListItem> familyRelationship = new List<ListItem>();
            try
            {
                List<FamilyRelationshipEntity> familyRelationshipsBDList;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships] != null)
                {
                    familyRelationshipsBDList = (List<FamilyRelationshipEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships];
                }
                else
                {
                    objFamilyRelationshipsBll = objFamilyRelationshipsBll ?? Application.GetContainer().Resolve<IFamilyRelationshipsBll<FamilyRelationshipEntity>>();
                    familyRelationshipsBDList = objFamilyRelationshipsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                familyRelationshipsBDList = familyRelationshipsBDList.Where(x => x.FamilyRelationshipCode == 8).ToList();

                familyRelationship.Add(new ListItem(Empty, "-1"));
                familyRelationship.AddRange(familyRelationshipsBDList.OrderBy(o => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? o.FamilyRelationshipDescriptionSpanish : o.FamilyRelationshipDescriptionEnglish)
                    .Select(r => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? r.FamilyRelationshipDescriptionSpanish : r.FamilyRelationshipDescriptionEnglish
                        , r.FamilyRelationshipCode.ToString())));
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
            return familyRelationship;
        }
        /// <summary>
        /// Gets the redirect page according to the response value of the questions 20 and 21
        /// </summary>
        /// <returns>The page to redirect</returns>
        private string GetRedirectPage(bool next)
        {
            int totalPeopleLivingWithEmployee = 0;
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.a"));
                    totalPeopleLivingWithEmployee += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.b"));
                    totalPeopleLivingWithEmployee += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
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
            if (next) {
                return totalPeopleLivingWithEmployee > 0 ? "FamilyWork.aspx" : "Expenses.aspx";
            } else {
                return totalPeopleLivingWithEmployee > 0 ? "FamilyGroup.aspx" : "FamilyInformation.aspx";
            }
            
        }

    }
}