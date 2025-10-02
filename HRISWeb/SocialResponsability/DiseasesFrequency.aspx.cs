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
    public partial class DiseasesFrequency : System.Web.UI.Page
    {
        private List<SurveyAnswerEntity> employeeSavedAnswers = null;

        [Dependency]
        protected IDiseasesBll<DiseaseEntity> objDiseasesBll { get; set; }
       
        [Dependency]
        protected IDiseaseFrequenciesBll<DiseaseFrequencyEntity> objDiseaseFrequenciesBll { get; set; }
       
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
       
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
        
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }
        [Dependency]
        protected IChronicDiseasesBLL<ChronicDiseasesEntity> objChronicDiseasesBll { get; set; }

        private const string surveyVersionName = "SurveyVersion";

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
                cboQuantityWithoutFood.Items.AddRange(Enumerable.Range(1, 30).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray());
                cboQuantityWithoutFood.Items.Insert(0,new ListItem(GetLocalResourceObject("DontSelected").ToString(), "-1") {Selected=true });
                //
                GetTotalPeopleLivingWithEmployee();
                LoadSurveyAnswers();
                
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
            Response.Redirect("Disabilities.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("MaterialWellness.aspx", false);
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
        /// Gets the Academic degrees
        /// </summary>
        /// <returns>The academic degrees</returns>
        private List<ListItem> GetsChronicDiseases()
        {
            List<ListItem> listChronicDisease = new List<ListItem>();
            try
            {
                List<ChronicDiseasesEntity> chronicDiseasesEntityBDList;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogChronicDiseases] != null)
                {
                    chronicDiseasesEntityBDList = (List<ChronicDiseasesEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogChronicDiseases];
                }
                else
                {
                    objChronicDiseasesBll = objChronicDiseasesBll ?? Application.GetContainer().Resolve<IChronicDiseasesBLL<ChronicDiseasesEntity>>();
                    chronicDiseasesEntityBDList = objChronicDiseasesBll.ListEnabled();
                }
               
                Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogChronicDiseases] = chronicDiseasesEntityBDList;

                CultureInfo currentCulture = GetCurrentCulture();

                listChronicDisease.Add(new ListItem(Empty, "-1"));
                listChronicDisease.AddRange(chronicDiseasesEntityBDList.OrderBy(o => currentCulture.Name.Equals(Constants.cCultureEsCR)
                    ? o.ChronicDiseaseDescriptionSpanish : o.ChronicDiseaseDescriptionEnglish)
                    .Select(r => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? r.ChronicDiseaseDescriptionSpanish : r.ChronicDiseaseDescriptionEnglish
                        , r.ChronicDiseaseCode.ToString())));
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
            return listChronicDisease;
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

                    numberOfMenFamily = numberOfMen;
                    numberOfWomenFamily = numberOfWomen;

                    if (surveyAnswers.Item1.Gender.StartsWith("M"))
                    {
                        numberOfMenFamily += 1;
                    }
                    else
                    {
                        numberOfWomenFamily += 1;
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
            return numberOfMen + numberOfWomen;
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

        /// <returns>The employee answers</returns>
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

                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("28.b"));

                    cboMenNumber.Items.Add(new ListItem("0", "0"));
                    cboMenNumber.Items.AddRange(Enumerable.Range(1, numberOfMenFamily).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray());
                    ListItem liNumberMen = answer != null ? cboMenNumber.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberMen != null)
                    {
                        liNumberMen.Selected = true;
                    }
                    //
                    cboDiseaseChroniqueMen.Items.Clear();
                    cboDiseaseChroniqueMen.Items.AddRange(GetsChronicDiseases().ToArray());
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("28.c")))
                    {
                        hdfDiseaseSelectedMen.Value = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("28.c")).AnswerValue.Trim();
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("28.d"));
                    cboWomenNumber.Items.Add(new ListItem("0", "0"));
                    cboWomenNumber.Items.AddRange(Enumerable.Range(1, numberOfWomenFamily).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray());
                    ListItem liNumberWomen = answer != null ? cboWomenNumber.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberWomen != null)
                    {
                        liNumberWomen.Selected = true;
                    }
                    //
                    cboDiseaseChroniqueWomen.Items.Clear();
                    cboDiseaseChroniqueWomen.Items.AddRange(GetsChronicDiseases().ToArray());
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("28.e")))
                    {
                        hdfDiseaseSelectedWomen.Value = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("28.e")).AnswerValue.Trim();
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("29"));
                    chkWithoutFood.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    if (chkWithoutFood.Checked)
                    {
                        cboQuantityWithoutFood.Attributes.Remove("disabled");

                        //
                        cboQuantityWithoutFood.SelectedIndex = -1;
                        answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("29.1"));
                        ListItem liQuantityWithoutFood = answer != null ? cboQuantityWithoutFood.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                        if (liQuantityWithoutFood != null)
                        {
                            liQuantityWithoutFood.Selected = true;
                            cboQuantityWithoutFood.Attributes.Remove("disabled");
                        }
                    }
                    //
                    employeeSavedAnswers = surveyAnswers.Item2;
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
            finally {
                ScriptManager.RegisterStartupScript(this
                   , this.GetType()
                   , "ReturnLoadSurveyAnswers" + Guid.NewGuid().ToString()
                   , "setTimeout(function () { RestoreSelectedDiseases(); }, 200);", true);

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
                bool withoutFood = chkWithoutFood.Checked;
                string daysWithoutFood = cboQuantityWithoutFood.SelectedValue;
                string numberMen = cboMenNumber.SelectedValue;
                string diseasesMenSelected = hdfDiseaseSelectedMen.Value;
                string NumberWomen = cboWomenNumber.SelectedValue;
                string diseasesWomenSelected = hdfDiseaseSelectedWomen.Value;
                //
                List<Tuple<string, byte, string,int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();

                //
                if (!IsNullOrWhiteSpace(numberMen) && !numberMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("28.b", 1, numberMen, 4));
                }
                //
                if (!IsNullOrWhiteSpace(diseasesMenSelected) && !diseasesMenSelected.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("28.c", 1, diseasesMenSelected, 4));
                }
                //
                if (!IsNullOrWhiteSpace(NumberWomen) && !NumberWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("28.d", 1, NumberWomen, 4));
                }
                //
                if (!IsNullOrWhiteSpace(diseasesWomenSelected) && !diseasesWomenSelected.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("28.e", 1, diseasesWomenSelected, 4));
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int>("29", 1, withoutFood.ToString(),4));
                
                if (!chkWithoutFood.Checked)
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("29.1", 1, "0",4));
                }
                else
                {
                    if (!IsNullOrWhiteSpace(daysWithoutFood) && !daysWithoutFood.Equals("-1"))
                    {
                        employeeAnswers.Add(new Tuple<string, byte, string, int>("29.1", 1, daysWithoutFood,4));
                    }
                }
                if (withoutFood)
                {
                    cboQuantityWithoutFood.Attributes.Remove("disabled");
                }
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                if (surveyAnswers.Item2 != null)
                {
                    surveyAnswers.Item2.ForEach(a => a.LastModifiedUser = currentUser);
                }
                //
                surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("28."));
                surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("29."));

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
                            ,item.Item4) { SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH                
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.DiseasesFrequency);
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
                   , "ReturnSaveSurveyAnswers" + Guid.NewGuid().ToString()
                   , "setTimeout(function () { RestoreSelectedDiseases(); }, 200);", true);

            }
        }
    }
}