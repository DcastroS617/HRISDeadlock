using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using System.Configuration;
using System.Web.Helpers;

namespace HRISWeb.SocialResponsability
{
    public partial class AcademicProfile : System.Web.UI.Page
    {
        [Dependency]
        protected IAcademicDegreesBll<AcademicDegreeEntity> objAcademicDegreesBll { get; set; }
        [Dependency]
        protected IDegreeFormationTypeBll<DegreeFormationTypeEntity> objDegreeFormationTypeBll { get; set; }

        [Dependency]
        protected ILanguagesBll<LanguageEntity> objLanguagesBll { get; set; }

        [Dependency]
        protected IProfessionsBll<ProfessionEntity> objProfessionsBll { get; set; }

        [Dependency]
        protected IEmployeeTaskBll objOccupationsBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }

        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }

        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }

        [Dependency]
        protected IYearAcademicDegreesBLL<YearAcademicDegreesEntity> objYearAcademicDegreesBll { get; set; }

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
            if (!IsPostBack)
            {
                LoadAcademicDegrees();
                LoadLanguages();
                ValidateWorkingDivisionVsEmployeeDivision();
                LoadDegreeFormationType();
                LoadSurveyAnswers();
            }

            this.SurveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());
            string script = string.Format("localStorage.DivisionCode= '{0}';", SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DivisionCode", script, true);

        }
        /// <summary>
        /// Handles the lbtnBack click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];

                currentSurvey.Item1.IsPageCalledByBackButton = true;

                Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, currentSurvey);
            }
            SaveSurveyAnswers(false);
            Response.Redirect("StartSurvey.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("FamilyInformation.aspx", false);
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

        protected void cboDegreeFormationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboOtherEducation.Items.Clear();
                cboStudyYearCarrer.Items.Clear();
                if (cboDegreeFormationType.SelectedValue != "-1") { 
                List<AcademicDegreeEntity> academicDegreesBDList = GetAcademicDegrees().Where(x => x.DegreeFormationTypeCode == Convert.ToByte(cboDegreeFormationType.SelectedValue)).ToList();

                CultureInfo currentCulture = GetCurrentCulture();
                

                var degreeComplete = academicDegreesBDList.Where(f => f.AcademicDegreeDescriptionSpanish.Contains("Completo") || f.AcademicDegreeDescriptionSpanish.Contains("Completado")
                        || f.AcademicDegreeDescriptionEnglish.Contains("Completed") || f.AcademicDegreeDescriptionEnglish.Contains("Complete")
                        ).ToList();

                foreach (var item in degreeComplete) {
                    academicDegreesBDList.Remove(item);
                }

                cboOtherEducation.Items.Add(new ListItem(Empty, "-1"));

                cboOtherEducation.Items.AddRange(academicDegreesBDList.OrderBy(b => b.Orderlist)
                    .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? g.AcademicDegreeDescriptionSpanish : g.AcademicDegreeDescriptionEnglish
                        , g.AcademicDegreeCode.ToString())).ToArray());
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
                    , "cboDegreeFormationType_SelectedIndexChanged" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { ConfigureQuestionStudy(); }, 50);", true);
            }

        }

        protected void cboGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int degreeSelect = Convert.ToInt32(cboGrade.SelectedValue);
                cboStudyYears.Items.Clear();
                cboOtherEducation.SelectedIndex = -1;
                cboStudyYearCarrer.Items.Clear();
                bool loadYearFlag = true;
                var years = GetsStudyYears(degreeSelect, false);

                if (years.Count > 1)
                {
                    cboStudyYears.Items.Add(new ListItem(Empty, "-1"));
                }
                else {
                    if (years.Count == 0) {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjAcademicDegreeNotYears").ToString());
                        return;
                    }
                    if (chkRead.Checked && chkWrite.Checked)
                    {
                        if (!years.FirstOrDefault().ReadAndWrite)
                        {
                            loadYearFlag = false;
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjAcademicDegreeInvalidRead").ToString());
                            cboGrade.SelectedIndex = -1;
                        }
                    }
                    else {
                        if (years.FirstOrDefault().ReadAndWrite)
                        {
                            loadYearFlag = false;
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjAcademicDegreeInvalidUnRead").ToString());
                            cboGrade.SelectedIndex = -1;
                        }
                    }
                }
                if (loadYearFlag)
                {
                    cboStudyYears.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());
                    cboStudyYears.SelectedIndex = -1;
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
                    , "cboGrade_SelectedIndexChanged" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { ConfigureQuestionStudy(); }, 50);", true);
            }
        }
        protected void cboOtherEducation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboStudyYearCarrer.Items.Clear();
                int degreeSelect = Convert.ToInt32(cboOtherEducation.SelectedValue);
                int lastAcademicValidation = Convert.ToInt32(ConfigurationManager.AppSettings["LastValidationAcademicDegree"].ToString());
                if (cboGrade.SelectedValue != "-1" && cboOtherEducation.SelectedValue != "-1")
                {         
                    var listDegrees = GetAcademicDegrees();
                    var degree = listDegrees.FirstOrDefault(x => x.AcademicDegreeCode == Convert.ToByte(cboGrade.SelectedValue));
                    var actualdegree = listDegrees.FirstOrDefault(x => x.AcademicDegreeCode == Convert.ToByte(cboOtherEducation.SelectedValue));
                    if (degree != null && actualdegree != null && cboDegreeFormationType.SelectedValue == "1")
                    {
                        if (degree.Orderlist <= lastAcademicValidation)
                        {
                            if (degree.Orderlist > actualdegree.Orderlist)
                            {
                                MensajeriaHelper.MostrarMensaje(this, TipoMensaje.Validacion, GetLocalResourceObject("msjActualDegreeInvalid").ToString());
                                cboOtherEducation.SelectedIndex = -1;
                            }
                        }
                        else
                        {
                            if (actualdegree.Orderlist <= lastAcademicValidation)
                            {
                                MensajeriaHelper.MostrarMensaje(this, TipoMensaje.Validacion, GetLocalResourceObject("msjActualDegreeInvalid").ToString());
                                cboOtherEducation.SelectedIndex = -1;
                            }
                        }
                    }
                }
                else {
                    if (cboGrade.SelectedValue == "-1")
                    {
                        MensajeriaHelper.MostrarMensaje(this, TipoMensaje.Validacion, GetLocalResourceObject("msjSelectDegreeValid").ToString());
                        cboOtherEducation.SelectedIndex = -1;
                    }
                }

                if (cboOtherEducation.SelectedIndex > 0) {
                    var years = GetsStudyYears(degreeSelect, true);
                    years = years.Where(x => x.AcademicDegreeCode == degreeSelect && x.DivisionCode == SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode).ToList();

                    if (years.Count > 1)
                    {
                        cboStudyYearCarrer.Items.Add(new ListItem(Empty, "-1"));
                    }
                    if (years.Count == 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjAcademicDegreeNotYears").ToString());
                        return;
                    }
                    cboStudyYearCarrer.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());
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
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType()
                    , "cboOtherEducation_SelectedIndexChanged" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { ConfigureQuestionStudy(); }, 50);", true);
            }

        }
        protected void cboStudyYearCarrer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboStudyYears.SelectedValue == "" || cboStudyYearCarrer.SelectedValue == ""  ) {
                    MensajeriaHelper.MostrarMensaje(this, TipoMensaje.Validacion, GetLocalResourceObject("msjAcademicDegreeNotYears").ToString());
                    return;
                }

                int actualDegree = Convert.ToInt32(cboOtherEducation.SelectedValue);
                int completeDegree = Convert.ToInt32(cboGrade.SelectedValue);
                int actualYear = Convert.ToInt32(cboStudyYearCarrer.SelectedValue);
                int completeYear = Convert.ToInt32(cboStudyYears.SelectedValue);
                int lastAcademicValidation = Convert.ToInt32(ConfigurationManager.AppSettings["LastValidationAcademicDegree"].ToString());
                var listDegrees = GetAcademicDegrees();
                var degree = listDegrees.FirstOrDefault(x => x.AcademicDegreeCode == Convert.ToByte(actualDegree));

                if (degree.Orderlist < lastAcademicValidation) {
                    if (actualDegree == completeDegree) {
                        if (completeYear > actualYear) {
                            MensajeriaHelper.MostrarMensaje(this, TipoMensaje.Validacion, GetLocalResourceObject("msjActualYearInvalid").ToString());
                            cboStudyYearCarrer.SelectedIndex = -1;
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
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType()
                    , "cboOtherEducation_SelectedIndexChanged" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { ConfigureQuestionStudy(); }, 50);", true);
            }

        }
        private List<AcademicDegreeEntity> GetAcademicDegrees() {

            List<AcademicDegreeEntity> academicDegreesBDList;

                objAcademicDegreesBll = objAcademicDegreesBll ?? Application.GetContainer().Resolve<IAcademicDegreesBll<AcademicDegreeEntity>>();
                academicDegreesBDList = objAcademicDegreesBll.ListEnabled();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] = academicDegreesBDList;


            return academicDegreesBDList;
        }

        /// <summary>
        /// Load the Academic degrees
        /// </summary>
        private void LoadAcademicDegrees()
        {
            try
            {
                List<AcademicDegreeEntity> academicDegreesBDList = GetAcademicDegrees().Where(x => x.DegreeFormationTypeCode == Convert.ToByte(1)).ToList();


                CultureInfo currentCulture = GetCurrentCulture();

                cboGrade.Items.Add(new ListItem(Empty, "-1"));

                cboGrade.Items.AddRange(academicDegreesBDList.OrderBy(b => b.Orderlist)
                    .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? g.AcademicDegreeDescriptionSpanish : g.AcademicDegreeDescriptionEnglish
                        , g.AcademicDegreeCode.ToString())).ToArray());

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
        /// Load the DegreeFormationType
        /// </summary>
        private void LoadDegreeFormationType()
        {
            try
            {
                List<DegreeFormationTypeEntity> degreeFormationTypeList;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDegreeFormationType] == null)
                {
                    objDegreeFormationTypeBll = objDegreeFormationTypeBll ?? Application.GetContainer().Resolve<IDegreeFormationTypeBll<DegreeFormationTypeEntity>>();
                    degreeFormationTypeList = objDegreeFormationTypeBll.ListEnabled();
                    Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDegreeFormationType] = degreeFormationTypeList;
                }

                degreeFormationTypeList = (List<DegreeFormationTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDegreeFormationType];

                CultureInfo currentCulture = GetCurrentCulture();
                cboDegreeFormationType.Items.Add(new ListItem(Empty, "-1"));

                cboDegreeFormationType.Items.AddRange(degreeFormationTypeList.Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? g.DegreeFormationTypeDescriptionSpanish : g.DegreeFormationTypeDescriptionEnglish
                        , g.DegreeFormationTypeCode.ToString())).ToArray());


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

        private void LoadAcademicDegreeSurveyAnswer(string academicDegree)
        {

            try
            {
                List<AcademicDegreeEntity> academicDegreesBDList = GetAcademicDegrees();
                if (academicDegree != "-1") {
                    var selectedDegree = academicDegreesBDList.Find(x => x.AcademicDegreeCode == Convert.ToByte(academicDegree));

                    ListItem liDegreeFormationCode = cboDegreeFormationType.Items.FindByValue(selectedDegree.DegreeFormationTypeCode.ToString());
                    if (liDegreeFormationCode != null)
                    {
                        liDegreeFormationCode.Selected = true;
                    }

                    this.cboDegreeFormationType_SelectedIndexChanged(new object(), new EventArgs());


                    ListItem liOtherEducation = cboOtherEducation.Items.FindByValue(academicDegree);
                    if (liOtherEducation != null)
                    {
                        liOtherEducation.Selected = true;
                    }

                    int degreee = Convert.ToInt32(academicDegree);
                    var years = GetsStudyYears(degreee, true);
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
        /// Gets the study years
        /// </summary>
        /// <returns>The study years</returns>
        private List<YearAcademicDegreesEntity> GetsStudyYears(int academicDegree, bool coursing)
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
            }
            var listJson = Json.Encode(yearAcademicBDlist);
            string script = string.Format("localStorage.YearDegrees= '{0}';", listJson);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "YearDegrees", script, true);
            return yearAcademicBDlist.Where(x => x.AcademicDegreeCode == academicDegree && x.DivisionCode == SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode && x.Coursing == coursing).ToList();
        }
        /// <summary>
        /// Load the Languages
        /// </summary>
        private void LoadLanguages()
        {
            try
            {
                List<LanguageEntity> languagesBDList = new List<LanguageEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogLanguages] != null)
                {
                    languagesBDList = (List<LanguageEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogLanguages];
                }
                else
                {
                    objLanguagesBll = objLanguagesBll ?? Application.GetContainer().Resolve<ILanguagesBll<LanguageEntity>>();
                    languagesBDList = objLanguagesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboLanguages.Items.Add(new ListItem(Empty, "-1"));
                cboLanguages.Items.AddRange(languagesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.LanguageNameSpanish : f.LanguageNameEnglish)
                    .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? g.LanguageNameSpanish : g.LanguageNameEnglish
                        , g.LanguageCode.ToString())).ToArray());

                cboNativeLanguages.Items.Add(new ListItem(Empty, "-1"));
                cboNativeLanguages.Items.AddRange(languagesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.LanguageNameSpanish : f.LanguageNameEnglish)
                    .Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? g.LanguageNameSpanish : g.LanguageNameEnglish
                        , g.LanguageCode.ToString())).ToArray());
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
                if (surveyAnswers != null && surveyAnswers.Item2.Count > 0)
                {
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("9.1.a")))
                    {
                        chkRead.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("9.1.a")).AnswerValue);
                    }
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("9.1.b")))
                    {
                        chkWrite.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("9.1.b")).AnswerValue);
                    }
                    //
                    //cboGrade.SelectedIndex = -1;
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("10")))
                    {
                        ListItem liLastGrade = cboGrade.Items.FindByValue(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("10")).AnswerValue.Trim());
                        if (liLastGrade != null)
                        {
                            liLastGrade.Selected = true;
                        }
                    }
                    //
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("10.1")))
                    {
                        int degreee = Convert.ToInt32(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("10")).AnswerValue.Trim());
                        var years = GetsStudyYears(degreee,false);

                        if (years.Count > 1)
                        {
                            cboStudyYears.Items.Add(new ListItem(Empty, "-1"));
                        }
                        cboStudyYears.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());

                        ListItem liStudyYears = cboStudyYears.Items.FindByValue(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("10.1")).AnswerValue.Trim());
                        if (liStudyYears != null)
                        {
                            liStudyYears.Selected = true;
                        }
                    }
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("11")))
                    {
                        chkGraduate.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("11")).AnswerValue);
                    }
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("12")))
                    {
                        chkCurrentlyStudying.Checked = Convert.ToBoolean(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("12")).AnswerValue);
                    }
                    cboOtherEducation.SelectedIndex = -1;

                    if (chkCurrentlyStudying.Checked)
                    {
                        cboStudyYearCarrer.Enabled = true;
                        cboOtherEducation.Enabled = true;
                        if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("12.1")))
                        {
                            LoadAcademicDegreeSurveyAnswer(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("12.1")).AnswerValue.Trim());

                        }
                        if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("12.2")))
                        {
                            int degreeeType = Convert.ToInt32(cboDegreeFormationType.SelectedValue);
                            if (degreeeType == 1)
                            {
                                var years = GetsStudyYears(Convert.ToInt32(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("12.1")).AnswerValue.Trim()),true);

                                if (years.Count > 1)
                                {
                                    cboStudyYearCarrer.Items.Add(new ListItem(Empty, "-1"));
                                }
                                cboStudyYearCarrer.Items.AddRange(years.Select(a => new ListItem(a.AcademicYear.ToString(), a.AcademicYear.ToString())).ToArray());
                            }
                            else
                            {
                                List<ListItem> studyYear = new List<ListItem>();
                                studyYear.Add(new ListItem(Empty, "-1"));
                                studyYear.AddRange(Enumerable.Range(1, 12).Select(s => new ListItem(s.ToString(), s.ToString())));
                                cboStudyYearCarrer.Items.AddRange(studyYear.ToArray());
                            }

                            ListItem liStudyYearsCarrer = cboStudyYearCarrer.Items.FindByValue(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("12.2")).AnswerValue.Trim());
                            if (liStudyYearsCarrer != null)
                            {
                                liStudyYearsCarrer.Selected = true;
                            }
                        }
                    }
                    cboNativeLanguages.SelectedIndex = -1;
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("13")))
                    {
                        ListItem liNativeLanguage = cboNativeLanguages.Items.FindByValue(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("13")).AnswerValue.Trim());
                        if (liNativeLanguage != null)
                        {
                            liNativeLanguage.Selected = true;
                        }
                    }

                    cboLanguages.SelectedIndex = -1;
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("13.1")))
                    {
                        hdfEmployeeSelectedLanguages.Value = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("13.1")).AnswerValue.Trim();
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
                    , "setTimeout(function () { RestoreSelectedLanguages(); }, 200);", true);
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
                bool doYouRead = chkRead.Checked;
                bool doYouWrite = chkWrite.Checked;
                string studyYear = cboStudyYears.SelectedValue;
                string maxSchoolDegree = cboGrade.SelectedValue;
                bool highSchoolGraduate = chkGraduate.Checked;//chkHighSchoolGraduate.Checked;
                bool currentlyStudying = chkCurrentlyStudying.Checked;
                string otherEducations = cboOtherEducation.SelectedValue;
                string studyYearCarrer = cboStudyYearCarrer.SelectedValue;//cboStudyYears.SelectedValue;
                string nativeLanguages = cboNativeLanguages.SelectedValue;
                string languages = hdfEmployeeSelectedLanguages.Value;

                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("9.1.a", 1, doYouRead.ToString(), 2));
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("9.1.b", 1, doYouWrite.ToString(), 2));
                //
                if (!IsNullOrWhiteSpace(maxSchoolDegree) && !maxSchoolDegree.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("10", 1, maxSchoolDegree, 2));
                }
                //
                if (!IsNullOrWhiteSpace(studyYear))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("10.1", 1, studyYear, 2));
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("11", 1, highSchoolGraduate.ToString(), 2));
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("12", 1, currentlyStudying.ToString(), 2));
                //
                if (currentlyStudying)
                {
                    if (!IsNullOrWhiteSpace(otherEducations))
                    {
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("12.1", 1, otherEducations, 2));
                    }
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("12.2", 1, studyYearCarrer, 2));
                }
                //
                if (!IsNullOrWhiteSpace(nativeLanguages) && !nativeLanguages.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("13", 1, nativeLanguages, 2));
                }
                //
                if (!IsNullOrWhiteSpace(languages))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("13.1", 1, languages, 2));
                }
                //
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
                        answer.IdSurveyModule = item.Item4;
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
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.AcademicProfile);
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
            finally
            {
                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnLoadSurveyAnswers" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { RestoreSelectedLanguages(); }, 200);", true);
            }
        }

        
    }
}