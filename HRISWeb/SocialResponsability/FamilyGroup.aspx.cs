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
using System.Web.Services;
using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.DataAccess;
using Hanssens.Net;
using System.Web.Helpers;

namespace HRISWeb.SocialResponsability
{
    public partial class FamilyGroup : System.Web.UI.Page
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
        protected IYearAcademicDegreesBLL<YearAcademicDegreesEntity> objYearAcademicDegreesBll { get; set; }

        private const string surveyVersionName = "SurveyVersion";
        private LocalStorage storage;
        private int SurveyVersion;

        private int numberOfMenFamily;
        private int numberOfWomenFamily;
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

                ValidateWorkingDivisionVsEmployeeDivision();

                string script = string.Format("localStorage.DivisionCode= '{0}';", SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "DivisionCode", script, true);

            }
        }
        /// <summary>
        /// Handles the event to refresh de table of family menber
        /// </summary>
        private void ReloadTable() {
            int totalPeopleLivingWithEmployee = GetTotalPeopleLivingWithEmployee();
            List<FamiliarEntity> relatives = new List<FamiliarEntity>();
            relatives.AddRange(Enumerable.Range(1, totalPeopleLivingWithEmployee).Select(r => new FamiliarEntity(r)));
            //
            rptFamilyGroup.DataSource = relatives;
            rptFamilyGroup.DataBind();

            foreach (RepeaterItem item in rptFamilyGroup.Items)
            {
                if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                {
                    DropDownList ddlStudyYear = item.FindControl("cboStudyYear") as DropDownList;
                }
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
            Response.Redirect("FamilyInformation.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("MinorChildren.aspx", false);
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
        protected void rptFamilyGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LoadSurveyAnswers(e.Item);
            }
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
                maritalStatusList.AddRange(maritalStatusBDList.Select(m => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? m.MaritalStatusDescriptionSpanish : m.MaritalStatusDescriptionEnglish
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

        /// <summary>
        /// Gets the family relationships
        /// </summary>
        /// <returns>The family relationships</returns>
        private List<ListItem> GetsFamilyRelationships()
        {
            List<ListItem> familyRelationship = new List<ListItem>();
            try
            {
                List<FamilyRelationshipEntity> familyRelationshipsBDList = new List<FamilyRelationshipEntity>();

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
        /// Gets the Academic degrees
        /// </summary>
        /// <returns>The academic degrees</returns>
        private List<AcademicDegreeEntity> GetsAcademicDegrees()
        {
            List<AcademicDegreeEntity> academicDegreesBDList = new List<AcademicDegreeEntity>();
            try
            {                
                
                objAcademicDegreesBll = objAcademicDegreesBll ?? Application.GetContainer().Resolve<IAcademicDegreesBll<AcademicDegreeEntity>>();
                academicDegreesBDList = objAcademicDegreesBll.ListEnabled();
                
                var listJson = Json.Encode(academicDegreesBDList);
                string script = string.Format("localStorage.AcademicDegrees= '{0}';", listJson);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "AcademicDegrees", script, true);
                Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] = academicDegreesBDList;

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
            age.AddRange(Enumerable.Range(0, 111).Select(a => new ListItem(a.ToString(), a.ToString())));
            return age;
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
        /// <param name="item">The repeater item for the familiar</param>
        private void LoadSurveyAnswers(RepeaterItem item)
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                SurveyAnswerEntity answer = null;
                CultureInfo currentCulture = GetCurrentCulture();
                List<AcademicDegreeEntity> academicDegreesBDList ;

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    byte familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                    //
                    DropDownList cboRelationshipItem = item.FindControl("cboRelationship") as DropDownList;
                    if (cboRelationshipItem != null)
                    {
                        cboRelationshipItem.Items.AddRange(GetsFamilyRelationships().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.a") && a.AnswerItem.Equals(familyId));
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
                        cboGenderItem.Enabled = false;
                        if (numberOfWomenFamily > 0)
                        {
                            cboGenderItem.SelectedValue = "F";
                            numberOfWomenFamily--;
                        }
                        else
                        {
                            cboGenderItem.SelectedValue = "M";
                            numberOfMenFamily--;
                        }
                    }
                    Label lblMsgGenderValidation = item.FindControl("cboGenderValidation") as Label;
                    if (lblMsgGenderValidation != null)
                    {
                        lblMsgGenderValidation.Attributes.Remove("data-content");
                        lblMsgGenderValidation.Attributes.Add("data-content", Convert.ToString(GetLocalResourceObject("msgGenderValidationNumberOfPeopleByGender")));
                    }
                    int cboAge = 0;
                    //
                    DropDownList cboAgeItem = item.FindControl("cboAge") as DropDownList;
                    if (cboAgeItem != null)
                    {
                        cboAgeItem.Items.AddRange(GetsAge().ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.c") && a.AnswerItem.Equals(familyId));
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
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.d") && a.AnswerItem.Equals(familyId));
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
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.e") && a.AnswerItem.Equals(familyId));
                        cbReadWrite.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cboAcademicDegreeItem = item.FindControl("cboAcademicDegree") as DropDownList;
                    if (cboAcademicDegreeItem != null)
                    {
                        academicDegreesBDList = GetsAcademicDegrees();
                        cboAcademicDegreeItem.Attributes.Remove("disabled");
                        cboAcademicDegreeItem.Items.Add(new ListItem(Empty, "-1"));
                        cboAcademicDegreeItem.Items.AddRange(academicDegreesBDList.OrderBy(b => b.Orderlist)
                            .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                                ? g.AcademicDegreeDescriptionSpanish : g.AcademicDegreeDescriptionEnglish
                                , g.AcademicDegreeCode.ToString())).ToArray());
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.f") && a.AnswerItem.Equals(familyId));
                        cboAcademicDegreeItem.SelectedIndex = -1;
                        ListItem liAcademicDegree = answer != null ? cboAcademicDegreeItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liAcademicDegree != null)
                        {
                            liAcademicDegree.Selected = true;
                        }

                        
                    }
                    //
                    DropDownList cboLastYearStudyItem = item.FindControl("cboLastYearStudy") as DropDownList;
                    if (cboLastYearStudyItem != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.g") && a.AnswerItem.Equals(familyId));
                        var years = GetsStudyYears(Convert.ToInt32(cboAcademicDegreeItem.SelectedValue), false);

                        if (years.Count > 1)
                        {
                            cboLastYearStudyItem.Items.Add(new ListItem(Empty, "-1"));
                        }
                        cboLastYearStudyItem.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        if (answer != null)
                        {
                            cboLastYearStudyItem.Attributes.Remove("disabled");
                            cboLastYearStudyItem.SelectedIndex = -1;
                            ListItem liStudyYear = cboLastYearStudyItem.Items.FindByValue(answer.AnswerValue.Trim());
                            if (liStudyYear != null)
                            {
                                liStudyYear.Selected = true;
                            }
                        }

                        
                    }
                    //
                    HtmlInputCheckBox cbhaveDegree = item.FindControl("chkHaveDegree") as HtmlInputCheckBox;
                    if (cbhaveDegree != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.h") && a.AnswerItem.Equals(familyId));
                        cbhaveDegree.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    HtmlInputCheckBox cbCurrentlyStudying = item.FindControl("chkCurrentlyStudying") as HtmlInputCheckBox;
                    if (cbCurrentlyStudying != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.i") && a.AnswerItem.Equals(familyId));
                        cbCurrentlyStudying.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    }
                    //
                    DropDownList cboActualAcademicDegreeItem = item.FindControl("cboActualAcademicDegree") as DropDownList;
                    if (cbCurrentlyStudying.Checked)
                    {
                        cboActualAcademicDegreeItem?.Attributes?.Remove("disabled");
                    }
                    if (cboActualAcademicDegreeItem != null)
                    {
                        academicDegreesBDList = GetsAcademicDegrees();
                        var degreeComplete = academicDegreesBDList.Where(f => f.AcademicDegreeDescriptionSpanish.Contains("Completo") || f.AcademicDegreeDescriptionSpanish.Contains("Completado")
                        || f.AcademicDegreeDescriptionEnglish.Contains("Completed") || f.AcademicDegreeDescriptionEnglish.Contains("Complete")
                        ).ToList();

                        foreach (var degree in degreeComplete)
                        {
                            academicDegreesBDList.Remove(degree);
                        }
                        cboActualAcademicDegreeItem.Items.Add(new ListItem(Empty, "-1"));
                        cboActualAcademicDegreeItem.Items.AddRange(academicDegreesBDList.OrderBy(b => b.Orderlist)
                            .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                                ? g.AcademicDegreeDescriptionSpanish : g.AcademicDegreeDescriptionEnglish
                                , g.AcademicDegreeCode.ToString())).ToArray());

                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.j") && a.AnswerItem.Equals(familyId));
                        cboActualAcademicDegreeItem.SelectedIndex = -1;
                        ListItem liAcademicDegree = answer != null ? cboActualAcademicDegreeItem.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liAcademicDegree != null)
                        {
                            liAcademicDegree.Selected = true;
                        }

                        cboActualAcademicDegreeItem.Enabled = !(cboAge < 5);
                    }
                    //
                    DropDownList cboStudyYearItem = item.FindControl("cboStudyYear") as DropDownList;
                    if (cbCurrentlyStudying.Checked)
                    {
                        cboStudyYearItem?.Attributes?.Remove("disabled");
                    }
                    if (cboStudyYearItem != null)
                    {
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.k") && a.AnswerItem.Equals(familyId));
                        var years = GetsStudyYears(Convert.ToInt32(cboActualAcademicDegreeItem.SelectedValue), true);

                        if (years.Count > 1)
                        {
                            cboStudyYearItem.Items.Add(new ListItem(Empty, "-1"));
                        }
                        cboStudyYearItem.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        if (answer != null)
                        {
                            cboStudyYearItem.Attributes.Remove("disabled");
                            cboStudyYearItem.SelectedIndex = -1;
                            ListItem licurrentStudyYear = cboStudyYearItem.Items.FindByValue(answer.AnswerValue.Trim());
                            if (licurrentStudyYear != null)
                            {
                                licurrentStudyYear.Selected = true;
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

                byte familyId = 0;
                foreach (RepeaterItem item in rptFamilyGroup.Items)
                {
                    if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                    {
                        familyId = Convert.ToByte(((Label)item.FindControl("lblFamilyId")).Text);
                        //
                        DropDownList ddlRelationship = item.FindControl("cboRelationship") as DropDownList;
                        string relationship = ddlRelationship != null ? ddlRelationship.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(relationship) && !relationship.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.a", familyId, relationship, 3));
                        }
                        //
                        DropDownList ddlGender = item.FindControl("cboGender") as DropDownList;
                        string gender = ddlGender != null ? ddlGender.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(gender) && !gender.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.b", familyId, gender, 3));
                        }
                        //
                        DropDownList ddlAge = item.FindControl("cboAge") as DropDownList;
                        string age = ddlAge != null ? ddlAge.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(age) && !age.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.c", familyId, age, 3));
                        }
                        //
                        DropDownList ddlMaritalStatus = item.FindControl("cboMaritalStatus") as DropDownList;
                        string maritalStatus = ddlMaritalStatus != null ? ddlMaritalStatus.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(maritalStatus) && !maritalStatus.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.d", familyId, maritalStatus, 3));
                        }
                        //
                        HtmlInputCheckBox cbReadWrite = item.FindControl("chkReadWrite") as HtmlInputCheckBox;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.e", familyId, cbReadWrite != null ? cbReadWrite.Checked.ToString() : bool.FalseString, 3));
                        //
                        DropDownList ddlAcademicDegree = item.FindControl("cboAcademicDegree") as DropDownList;                        
                        string academicDegree = ddlAcademicDegree != null ? ddlAcademicDegree.SelectedValue : Empty;
                        if (!IsNullOrWhiteSpace(academicDegree) && !academicDegree.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.f", familyId, academicDegree, 3));
                            ddlAcademicDegree.Attributes.Remove("disabled");
                            ddlAcademicDegree.Enabled = !(Convert.ToInt32(age) < 5);
                        }
                        //
                        DropDownList cbLastYearStudy = item.FindControl("cboLastYearStudy") as DropDownList;                        
                        HiddenField hdfLastYearStudyItem = item.FindControl("hdfLastYearStudy") as HiddenField;
                        string lastYearStudy = cbLastYearStudy != null ? cbLastYearStudy.SelectedValue : Empty;
                        lastYearStudy = IsNullOrEmpty(lastYearStudy) ? hdfLastYearStudyItem.Value : lastYearStudy;
                        if (!IsNullOrWhiteSpace(lastYearStudy) && !lastYearStudy.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.g", familyId, lastYearStudy, 3));

                            cbLastYearStudy.Attributes.Remove("disabled");
                            cbLastYearStudy.Enabled = !(Convert.ToInt32(age) < 5);
                            var years = GetsStudyYears(Convert.ToInt32(academicDegree), false);
                            cbLastYearStudy.Items.Clear();
                            if (years.Count > 1)
                            {
                                cbLastYearStudy.Items.Add(new ListItem(Empty, "-1"));
                            }
                            cbLastYearStudy.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());
                            ListItem liStudyYear = cbLastYearStudy.Items.FindByValue(lastYearStudy);
                            if (liStudyYear != null)
                            {
                                liStudyYear.Selected = true;
                            }
                        }
                        //
                        HtmlInputCheckBox chkHaveDegree = item.FindControl("chkHaveDegree") as HtmlInputCheckBox;
                        bool haveDegree = chkHaveDegree != null ? chkHaveDegree.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.h", familyId, haveDegree.ToString(), 3));
                        //
                        HtmlInputCheckBox cbCurrentlyStudying = item.FindControl("chkCurrentlyStudying") as HtmlInputCheckBox;
                        bool currentlyStudying = cbCurrentlyStudying != null ? cbCurrentlyStudying.Checked : false;
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.i", familyId, currentlyStudying.ToString(), 3));
                        //
                        DropDownList ddlActualAcademicDegree = item.FindControl("cboActualAcademicDegree") as DropDownList;
                        string actualAcademicDegree = ddlActualAcademicDegree != null ? ddlActualAcademicDegree.SelectedValue : Empty;
                        if (currentlyStudying)
                        {
                            ddlActualAcademicDegree?.Attributes?.Remove("disabled");
                            ddlActualAcademicDegree.Enabled = true;
                        }
                        if (!IsNullOrWhiteSpace(actualAcademicDegree) && !actualAcademicDegree.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.j", familyId, actualAcademicDegree, 3));
                        }
                        DropDownList ddlStudyYear = item.FindControl("cboStudyYear") as DropDownList;
                        HiddenField hdfStudyYearItem = item.FindControl("hdfStudyYearItem") as HiddenField;
                        string studyYear = ddlStudyYear != null ? ddlStudyYear.SelectedValue : Empty;
                        studyYear = IsNullOrEmpty(studyYear) ? hdfStudyYearItem.Value : studyYear;
                        if (currentlyStudying)
                        {
                            ddlStudyYear?.Attributes?.Remove("disabled");
                            var years = GetsStudyYears(Convert.ToInt32(actualAcademicDegree), true);
                            ddlStudyYear.Items.Clear();
                            if (years.Count > 1)
                            {
                                ddlStudyYear.Items.Add(new ListItem(Empty, "-1"));
                            }
                            ddlStudyYear.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        }
                        if (!IsNullOrWhiteSpace(studyYear) && !studyYear.Equals("-1"))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("20.1.k", familyId, studyYear, 3));
                            ListItem licurrentStudyYear = ddlStudyYear.Items.FindByValue(studyYear);
                            if (licurrentStudyYear != null)
                            {
                                licurrentStudyYear.Selected = true;
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
                // Remove all answers for the questions 26
                surveyAnswers?.Item2?.RemoveAll(r => r.QuestionID.Contains("20.1"));
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

                    this.numberOfMenFamily = numberOfMen;
                    this.numberOfWomenFamily = numberOfWomen;
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