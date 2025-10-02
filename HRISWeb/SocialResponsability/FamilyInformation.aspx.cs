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

namespace HRISWeb.SocialResponsability
{
    public partial class FamilyInformation : System.Web.UI.Page
    {
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
            if (!IsPostBack)
            {
                List<DropDownList> formDropdownList = new List<DropDownList>();
                formDropdownList.Add(cboFamilyMembersLivingWithYouMen);
                formDropdownList.Add(cboFamilyMembersLivingWithYouWomen);
                formDropdownList.Add(cboNumberOtherPeopleLivingWithYouMen);
                formDropdownList.Add(cboNumberOtherPeopleLivingWithYouWomen);
                formDropdownList.Add(cboPeopleDependEconomicallyOnYouMen);
                formDropdownList.Add(cboPeopleDependEconomicallyOnYouWomen);
                formDropdownList.Add(cboNumberOtherPeopleLivingWithOutYouMen);
                formDropdownList.Add(cboNumberOtherPeopleLivingWithOutYouWomen);
                formDropdownList.Add(cboMinorsWhoAreParentMen);
                formDropdownList.Add(cboMinorsWhoAreParentWomen);
                formDropdownList.Add(cboMinorsWithEmploymentMen);
                formDropdownList.Add(cboMinorsWithEmploymentWomen);

                formDropdownList.ForEach(d => d.Items.AddRange(Enumerable.Range(0, 16).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray()));
                                
                ValidateWorkingDivisionVsEmployeeDivision();
                LoadSurveyAnswers();
            }
            this.SurveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());
        }
        /// <summary>
        /// Handles the lbtnBack click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("AcademicProfile.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect(GetRedirectPage(), false);
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
                    cboFamilyMembersLivingWithYouMen.SelectedIndex = -1;
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.a"));
                    ListItem liFamilyMembersLivingWithYouMen = answer != null ? cboFamilyMembersLivingWithYouMen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liFamilyMembersLivingWithYouMen != null)
                    {
                        liFamilyMembersLivingWithYouMen.Selected = true;
                    }
                    //
                    cboFamilyMembersLivingWithYouWomen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.b"));
                    ListItem liFamilyMembersLivingWithYouWomen = answer != null ? cboFamilyMembersLivingWithYouWomen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liFamilyMembersLivingWithYouWomen != null)
                    {
                        liFamilyMembersLivingWithYouWomen.Selected = true;
                    }
                    //
                    cboPeopleDependEconomicallyOnYouMen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("15.a"));
                    ListItem liPeopleDependEconomicallyOnYouMen = answer != null ? cboPeopleDependEconomicallyOnYouMen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liPeopleDependEconomicallyOnYouMen != null)
                    {
                        liPeopleDependEconomicallyOnYouMen.Selected = true;
                    }
                    //
                    cboPeopleDependEconomicallyOnYouWomen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("15.b"));
                    ListItem liPeopleDependEconomicallyOnYouWomen = answer != null ? cboPeopleDependEconomicallyOnYouWomen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liPeopleDependEconomicallyOnYouWomen != null)
                    {
                        liPeopleDependEconomicallyOnYouWomen.Selected = true;
                    }
                    //
                    cboMinorsWhoAreParentMen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("16.a"));
                    ListItem liMinorsWhoAreParentMen = answer != null ? cboMinorsWhoAreParentMen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liMinorsWhoAreParentMen != null)
                    {
                        liMinorsWhoAreParentMen.Selected = true;
                    }
                    //
                    cboMinorsWhoAreParentWomen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("16.b"));
                    ListItem liMinorsWhoAreParentWomen = answer != null ? cboMinorsWhoAreParentWomen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liMinorsWhoAreParentWomen != null)
                    {
                        liMinorsWhoAreParentWomen.Selected = true;
                    }
                    //
                    cboMinorsWithEmploymentMen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("17.a"));
                    ListItem liMinorsWithEmploymentMen = answer != null ? cboMinorsWithEmploymentMen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liMinorsWithEmploymentMen != null)
                    {
                        liMinorsWithEmploymentMen.Selected = true;
                    }
                    //
                    cboMinorsWithEmploymentWomen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("17.b"));
                    ListItem liMinorsWithEmploymentWomen = answer != null ? cboMinorsWithEmploymentWomen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liMinorsWithEmploymentWomen != null)
                    {
                        liMinorsWithEmploymentWomen.Selected = true;
                    }
                    //
                    cboNumberOtherPeopleLivingWithYouMen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("18.a"));
                    ListItem liNumberOtherPeopleLivingWithYouMen = answer != null ? cboNumberOtherPeopleLivingWithYouMen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberOtherPeopleLivingWithYouMen != null)
                    {
                        liNumberOtherPeopleLivingWithYouMen.Selected = true;
                    }
                    //
                    cboNumberOtherPeopleLivingWithYouWomen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("18.b"));
                    ListItem liNumberOtherPeopleLivingWithYouWomen = answer != null ? cboNumberOtherPeopleLivingWithYouWomen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberOtherPeopleLivingWithYouWomen != null)
                    {
                        liNumberOtherPeopleLivingWithYouWomen.Selected = true;
                    }
                    //
                    cboNumberOtherPeopleLivingWithOutYouMen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("19.a"));
                    ListItem liNumberOtherPeopleLivingWithOutYouMen = answer != null ? cboNumberOtherPeopleLivingWithOutYouMen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberOtherPeopleLivingWithOutYouMen != null)
                    {
                        liNumberOtherPeopleLivingWithOutYouMen.Selected = true;
                    }
                    //
                    cboNumberOtherPeopleLivingWithOutYouWomen.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("19.b"));
                    ListItem liNumberOtherPeopleLivingWithoutYouWomen = answer != null ? cboNumberOtherPeopleLivingWithOutYouWomen.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberOtherPeopleLivingWithoutYouWomen != null)
                    {
                        liNumberOtherPeopleLivingWithoutYouWomen.Selected = true;
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
                string familyMembersLivingWithYouMen = cboFamilyMembersLivingWithYouMen.SelectedValue;
                string familyMembersLivingWithYouWomen = cboFamilyMembersLivingWithYouWomen.SelectedValue;

                string peopleDependEconomicallyOnYouMen = cboPeopleDependEconomicallyOnYouMen.SelectedValue;
                string peopleDependEconomicallyOnYouWomen = cboPeopleDependEconomicallyOnYouWomen.SelectedValue;

                string minorsWhoAreParentMen = cboMinorsWhoAreParentMen.SelectedValue;
                string minorsWhoAreParentWomen = cboMinorsWhoAreParentWomen.SelectedValue;

                string minorsWithEmploymentMen = cboMinorsWithEmploymentMen.SelectedValue;
                string minorsWithEmploymentWomen = cboMinorsWithEmploymentWomen.SelectedValue;

                string numberOtherPeopleLivingWithYouMen = cboNumberOtherPeopleLivingWithYouMen.SelectedValue;
                string numberOtherPeopleLivingWithYouWomen = cboNumberOtherPeopleLivingWithYouWomen.SelectedValue;

                string numberOtherPeopleLivingWithOutYouMen = cboNumberOtherPeopleLivingWithOutYouMen.SelectedValue;
                string numberOtherPeopleLivingWithOutYouWomen = cboNumberOtherPeopleLivingWithOutYouWomen.SelectedValue;

                //
                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                if (!IsNullOrWhiteSpace(familyMembersLivingWithYouMen) && !familyMembersLivingWithYouMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string,int>("14.a", 1, familyMembersLivingWithYouMen,3));
                }
                if (!IsNullOrWhiteSpace(familyMembersLivingWithYouWomen) && !familyMembersLivingWithYouWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("14.b", 1, familyMembersLivingWithYouWomen, 3));
                }
                //
                if (!IsNullOrWhiteSpace(peopleDependEconomicallyOnYouMen) && !peopleDependEconomicallyOnYouMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("15.a", 1, peopleDependEconomicallyOnYouMen, 3));
                }
                if (!IsNullOrWhiteSpace(peopleDependEconomicallyOnYouWomen) && !peopleDependEconomicallyOnYouWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("15.b", 1, peopleDependEconomicallyOnYouWomen, 3));
                }
                //
                if (!IsNullOrWhiteSpace(minorsWhoAreParentMen) && !minorsWhoAreParentMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("16.a", 1, minorsWhoAreParentMen, 3));
                }
                if (!IsNullOrWhiteSpace(minorsWhoAreParentWomen) && !minorsWhoAreParentWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("16.b", 1, minorsWhoAreParentWomen, 3));
                }
                //
                if (!IsNullOrWhiteSpace(minorsWithEmploymentMen) && !minorsWithEmploymentMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("17.a", 1, minorsWithEmploymentMen, 3));
                }
                if (!IsNullOrWhiteSpace(minorsWithEmploymentWomen) && !minorsWithEmploymentWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("17.b", 1, minorsWithEmploymentWomen, 3));
                }
                //
                if (!IsNullOrWhiteSpace(numberOtherPeopleLivingWithYouMen) && !numberOtherPeopleLivingWithYouMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("18.a", 1, numberOtherPeopleLivingWithYouMen, 3));
                }
                if (!IsNullOrWhiteSpace(numberOtherPeopleLivingWithYouWomen) && !numberOtherPeopleLivingWithYouWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("18.b", 1, numberOtherPeopleLivingWithYouWomen, 3));
                }
                //
                if (!IsNullOrWhiteSpace(numberOtherPeopleLivingWithOutYouMen) && !numberOtherPeopleLivingWithOutYouMen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("19.a", 1, numberOtherPeopleLivingWithOutYouMen, 3));
                }
                if (!IsNullOrWhiteSpace(numberOtherPeopleLivingWithOutYouWomen) && !numberOtherPeopleLivingWithOutYouWomen.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("19.b", 1, numberOtherPeopleLivingWithOutYouWomen, 3));
                }
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                if(surveyAnswers.Item2 != null)
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
                            ,item.Item4){ SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH                
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.FamilyInformation);
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
        /// Gets the redirect page according to the response value of the questions 20 and 21
        /// </summary>
        /// <returns>The page to redirect</returns>
        private string GetRedirectPage()
        {            
            int totalPeopleLivingWithEmployee = 0;
            int totalMinorChildren = 0;
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                
                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.a"));
                    totalPeopleLivingWithEmployee += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    //
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("14.b")))
                    {
                        totalPeopleLivingWithEmployee += Convert.ToInt32(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.b")).AnswerValue);
                    }
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("19.a")))
                    {
                        totalMinorChildren += Convert.ToInt32(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("19.a")).AnswerValue);
                    }
                    if (surveyAnswers.Item2.Exists(a => a.QuestionID.Equals("19.b")))
                    {
                        totalMinorChildren += Convert.ToInt32(surveyAnswers.Item2.Find(a => a.QuestionID.Equals("19.b")).AnswerValue);
                    }
                    /*
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.b"));
                    totalPeopleLivingWithEmployee += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    */

                    if (totalPeopleLivingWithEmployee.Equals(0))
                    {
                        // Remove all answers for the questions 26
                        surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Contains("20.1"));
                        surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Contains("20.2"));
                        surveyAnswers.Item2.RemoveAll(r => r.QuestionID.Contains("21"));
                    }
                    // Save in session
                    Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, surveyAnswers);
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
            if (totalPeopleLivingWithEmployee > 0)
            {
                return "FamilyGroup.aspx";
            }
            else {
                if (totalMinorChildren > 0)
                {
                    return "MinorChildren.aspx";
                }
                else {
                    return "Expenses.aspx";
                }
            }
        }
    }
}