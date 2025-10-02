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
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static System.String;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class PersonalProfile : System.Web.UI.Page
    {
        [Dependency]
        protected IMaritalStatusBll<MaritalStatusEntity> objMaritalStatusBll { get; set; }

        [Dependency]
        protected IEmployeeTaskBll objIEmployeeTaskBll { get; set; }

        [Dependency]
        protected IEmployeeWorkBll objIEmployeeWorkBll { get; set; }
        [Dependency]
        protected ILanguagesBll<LanguageEntity> objLanguagesBll { get; set; }
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }

        private readonly int SurveyVersion = 2;

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


        public string DataListcboSpouseWorkplace => "PersonalProfile-DataListcboSpouseWorkplace";

        public string DataListcboWhatWorkDoes => "PersonalProfile-DataListcboWhatWorkDoes";
        /// <summary>
        /// Handles the load of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMaritalStatus();


                cboChildrenNumber.Items.Add(new ListItem(String.Empty, "-1"));
                cboChildrenLivingOutsideHome.Items.Add(new ListItem(String.Empty, "-1"));
                #region cbos 
                cboSpouseWorkplace.Items.Add(new ListItem(String.Empty, ""));
                cboWhatWorkDoes.Items.Add(new ListItem(String.Empty, ""));

                var ListSpuseWorkplace = objIEmployeeWorkBll.EmployeeWorkListByEnable();
                var listcboWhatWorkDoes = objIEmployeeTaskBll.EmployeeTaskListByEnabled();

                Session[DataListcboSpouseWorkplace] = ListSpuseWorkplace;
                Session[DataListcboWhatWorkDoes] = listcboWhatWorkDoes;
                cboSpouseWorkplace.Items.AddRange(ListSpuseWorkplace);
                cboWhatWorkDoes.Items.AddRange(listcboWhatWorkDoes);
                #endregion


                cboChildrenNumber.Items.AddRange(Enumerable.Range(0, 16).Select(a => new ListItem(a.ToString(), a.ToString())).ToArray());
                cboChildrenLivingOutsideHome.Items.AddRange(Enumerable.Range(0, 16).Select(a => new ListItem(a.ToString(), a.ToString())).ToArray());

                LoadLanguages();

                ValidateWorkingDivisionVsEmployeeDivision();
                LoadSurveyAnswers();
            }            
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
            Response.Redirect("AcademicProfile.aspx", false);
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
        /// Load the marital status
        /// </summary>
        private void LoadMaritalStatus()
        {
            try
            {
                List<MaritalStatusEntity> maritalStatusList = new List<MaritalStatusEntity>();

                if(Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogMaritalStatus] != null)
                {
                    maritalStatusList = (List<MaritalStatusEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogMaritalStatus];
                }
                else
                {
                    objMaritalStatusBll = objMaritalStatusBll ?? Application.GetContainer().Resolve<IMaritalStatusBll<MaritalStatusEntity>>();
                    maritalStatusList = objMaritalStatusBll.ListEnabled();
                }

                rdbMaritalStatus.DataTextField = GetCurrentCulture().Name.Equals(Constants.cCultureEsCR) ? "MaritalStatusDescriptionSpanish" : "MaritalStatusDescriptionEnglish";
                rdbMaritalStatus.DataValueField = "MaritalStatusCode";
                rdbMaritalStatus.DataSource = maritalStatusList;
                rdbMaritalStatus.DataBind();
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
                
                cboNativeLanguage.SelectedIndex = -1;

                cboNativeLanguage.Items.Add(new ListItem(String.Empty, "-1"));
                cboNativeLanguage.Items.AddRange(languagesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR) 
                        ? f.LanguageNameSpanish : f.LanguageNameEnglish)
                    .Select(s => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? s.LanguageNameSpanish : s.LanguageNameEnglish,  s.LanguageCode.ToString())).ToArray());

                ListItem liNativeLanguage = cboNativeLanguage.Items.FindByText(GetCurrentCulture().Name.Equals(Constants.cCultureEsCR) ? "Español" : "Spanish");
                if(liNativeLanguage != null)
                {
                    liNativeLanguage.Selected = true;
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
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            return Session[Constants.cCulture] != null ? new CultureInfo(Convert.ToString(Session[Constants.cCulture])) : new CultureInfo(Constants.cCultureEsCR);
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
            if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
            {
                currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];
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
                if (surveyAnswers?.Item2.Any() ?? false)
                {                    
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("8"));
                    rdbMaritalStatus.SelectedValue = answer?.AnswerValue ?? "-1";
                    //                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("9"));
                    chkSpouseWorks.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("9.1.a"));

                    var valSpousework = answer?.AnswerValue ?? Empty;
                    if (!(Session[DataListcboSpouseWorkplace] as ListItem[]  ).Where(r=>r.Value.Equals(valSpousework)).Any())
                    {
                        int val;

                        bool success = int.TryParse(valSpousework, out val);
                        EmployeeWorksEntity work = null;
                        if (success) 
                             work= objIEmployeeWorkBll.EmployeeWorkDetail(new EmployeeWorksEntity { EmployeeWorksCode = val });

                        if (work!=null)
                        {
                            cboSpouseWorkplace.Items.Add(new ListItem(work.EmployeeWorksCode.ToString(), work.EmployeeWorksDescription));
                        }
                        else
                        {
                            cboSpouseWorkplace.Items.Add(new ListItem(valSpousework, valSpousework));
                        }
                        

                      }

                    cboSpouseWorkplace.Value = valSpousework;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("9.1.b"));

                    var valWhatWorkDoes = answer?.AnswerValue ?? Empty;
                    if (!(Session[DataListcboWhatWorkDoes] as ListItem[]).Where(r => r.Value.Equals(valWhatWorkDoes)).Any())
                    {

                        int val;

                        bool success = int.TryParse(valWhatWorkDoes, out val);
                        EmployeeTaskEntity task = null;
                        if (success)
                            task = objIEmployeeTaskBll.EmployeeTaskDetail(new EmployeeTaskEntity { EmployeeTaskCode = val });

                        if (task != null)
                        {
                            cboWhatWorkDoes.Items.Add(new ListItem(task.EmployeeTaskCode.ToString(), task.EmployeeTaskDescription));
                        }else if(!String.IsNullOrEmpty(valWhatWorkDoes))
                        {
                            cboWhatWorkDoes.Items.Add(new ListItem(valWhatWorkDoes, valWhatWorkDoes));
                        }
                       
                    }

                    cboWhatWorkDoes.Value = valWhatWorkDoes;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("9.2"));
                    txtWhatDoForLiving.Text = answer?.AnswerValue ?? Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("10"));
                    chkMainSupplier.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    //
                    cboChildrenNumber.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("11"));
                    ListItem liChildrenNum = answer != null ? cboChildrenNumber.Items.FindByValue(answer.AnswerValue) : null;
                    if (liChildrenNum != null)
                    {
                        liChildrenNum.Selected = true;
                    }
                    //
                    cboChildrenLivingOutsideHome.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("12"));
                    ListItem liChildrenOutsideHome = answer != null ? cboChildrenLivingOutsideHome.Items.FindByValue(answer.AnswerValue) : null;
                    if (liChildrenOutsideHome != null)
                    {
                        liChildrenOutsideHome.Selected = true;
                    }
                    //
                    cboNativeLanguage.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("13"));
                    ListItem liNativeLanguage = answer != null ? cboNativeLanguage.Items.FindByValue(answer.AnswerValue) 
                        : cboNativeLanguage.Items.FindByText(GetCurrentCulture().Name.Equals(Constants.cCultureEsCR) ? "Español" : "Spanish");
                    if (liNativeLanguage != null)
                    {
                        liNativeLanguage.Selected = true;
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
                    , "setTimeout(function () { ConfigureQuestionSpouseWorks(); ConfigureByMaritalStatus(); ConfigureNumberOfChildren();}, 200);", true);
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
                string maritalStatus = rdbMaritalStatus.SelectedValue;
                bool spouseWorks = chkSpouseWorks.Checked;
                string spouseWorkplace = cboSpouseWorkplace.Value?.Trim();
                string whatWorkDoes = cboWhatWorkDoes.Value?.Trim();
                string whatDoForLiving = txtWhatDoForLiving.Text.Trim();
                bool mainSupplier = chkMainSupplier.Checked;
                string childrenNumber = cboChildrenNumber.SelectedValue;
                string childrenLivingOutsideHome = cboChildrenLivingOutsideHome.SelectedValue;
                string nativeLanguage = cboNativeLanguage.SelectedValue;
                
                List<Tuple<string, byte, string>> employeeAnswers = new List<Tuple<string, byte, string>>();
                if (!String.IsNullOrWhiteSpace(maritalStatus))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string>("8", 1, maritalStatus));
                }
                employeeAnswers.Add(new Tuple<string, byte, string>("9", 1, spouseWorks.ToString()));
               
                if (rdbMaritalStatus.Items[1].Selected || rdbMaritalStatus.Items[5].Selected)
                {
                    if (spouseWorks)
                    {
                        if (!String.IsNullOrWhiteSpace(spouseWorkplace))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string>("9.1.a", 1, spouseWorkplace));
                        }
                        if (!String.IsNullOrWhiteSpace(whatWorkDoes))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string>("9.1.b", 1, whatWorkDoes));
                        }
                        if (String.IsNullOrWhiteSpace(whatDoForLiving))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string>("9.2", 1, ""));
                        }
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(spouseWorkplace))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string>("9.1.a", 1, "0"));
                        }
                        if (String.IsNullOrWhiteSpace(whatWorkDoes))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string>("9.1.b", 1, "0"));
                        }
                        if (!String.IsNullOrWhiteSpace(whatDoForLiving))
                        {
                            employeeAnswers.Add(new Tuple<string, byte, string>("9.2", 1, whatDoForLiving));
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(whatDoForLiving))
                    {
                        employeeAnswers.Add(new Tuple<string, byte, string>("9.2", 1, ""));
                    }
                    if (String.IsNullOrWhiteSpace(spouseWorkplace))
                    {
                        employeeAnswers.Add(new Tuple<string, byte, string>("9.1.a", 1, "0"));
                    }
                    if (String.IsNullOrWhiteSpace(whatWorkDoes))
                    {
                        employeeAnswers.Add(new Tuple<string, byte, string>("9.1.b", 1, "0"));
                    }
                }

                employeeAnswers.Add(new Tuple<string, byte, string>("10", 1, mainSupplier.ToString()));
                if (!String.IsNullOrWhiteSpace(childrenNumber) && !childrenNumber.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string>("11", 1, childrenNumber));
                }
                if (!String.IsNullOrWhiteSpace(childrenLivingOutsideHome) && !childrenLivingOutsideHome.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string>("12", 1, childrenLivingOutsideHome));
                }
                if (!String.IsNullOrWhiteSpace(nativeLanguage) && !nativeLanguage.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string>("13", 1, nativeLanguage));
                }
                //
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                surveyAnswers?.Item2?.ForEach(a => a.LastModifiedUser = currentUser);

                foreach (var item in employeeAnswers)
                {
                    var answer = surveyAnswers.Item2.Find(v => v.QuestionID.Equals(item.Item1) && v.AnswerItem.Equals(item.Item2));
                    if (answer != null)
                    {
                        answer.AnswerValue = item.Item3;
                        answer.LastModifiedUser = currentUser;
                        answer.SurveyVersion = this.SurveyVersion;
                    }
                    else
                    {                        
                        surveyAnswers.Item2.Add(new SurveyAnswerEntity(surveyAnswers.Item1.SurveyCode
                            , surveyQuestions.Find(q => q.QuestionID.Equals(item.Item1))?.QuestionCode ?? 0
                            , item.Item1
                            , item.Item2
                            , item.Item3
                            , currentUser) { SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH
                //
                // Remove invalid(not applicable by answers flow configuration) answers
                //if (spouseWorks)
                //{
                //    surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Equals("9.2"));
                //}
                //else
                //{
                //    surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Equals("9.1.a") || r.QuestionID.Equals("9.1.b"));
                //}
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.PersonalProfile);
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
                    , "setTimeout(function () { ConfigureQuestionSpouseWorks(); }, 200);", true);
            }
        }
    }
}