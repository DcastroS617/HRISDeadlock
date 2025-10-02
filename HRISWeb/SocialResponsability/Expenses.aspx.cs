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
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class Expenses : System.Web.UI.Page
    {
        [Dependency]
        protected ICurrenciesBll<CurrencyEntity> objCurrenciesBll { get; set; }
      
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
                LoadCurency();
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
            Response.Redirect(GetRedirectPage(), false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("FamilyWelfare.aspx", false);
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
        /// Load the Currency By Division
        /// </summary>
        private void LoadCurency()
        {
            try
            {
                List<CurrencyEntity> currenciesBDList = new List<CurrencyEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies] != null)
                {
                    currenciesBDList = (List<CurrencyEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies];
                }
                else
                {
                    objCurrenciesBll = objCurrenciesBll ?? Application.GetContainer().Resolve<ICurrenciesBll<CurrencyEntity>>();
                    currenciesBDList = objCurrenciesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                                
                List<CurrencyEntity> currencyByDivision = currenciesBDList.Where(r => r.DivisionCode.Equals(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode)).ToList();

                if(currencyByDivision != null && currencyByDivision.Any())
                {
                    lblCurrency.Text = Format(Convert.ToString(GetLocalResourceObject("lblCurrency.Text"))
                        , currencyByDivision.FirstOrDefault().CurrencyCode
                        , currentCulture.Name.Equals(Constants.cCultureEsCR)
                            ? currencyByDivision.FirstOrDefault().CurrencyNameSpanish
                            : currencyByDivision.FirstOrDefault().CurrencyNameEnglish);
                }
                else
                {
                    lblCurrency.Text = Format(Convert.ToString(GetLocalResourceObject("lblCurrency.Text"))
                        , Empty
                        , Empty);
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
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("22") && a.AnswerItem.Equals(1));
                    chkExtraIncome.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;                    
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("22.1") && a.AnswerItem.Equals(1));
                    txtExtraIncomeComes.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    if (chkExtraIncome.Checked)
                    {
                        txtExtraIncomeComes.Attributes.Remove("disabled");
                    }
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("22.2") && a.AnswerItem.Equals(1));
                    chkPrincipalProvider.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    //                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.1") && a.AnswerItem.Equals(1));
                    txtFeeding.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.2") && a.AnswerItem.Equals(1));
                    txtPhone.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.3") && a.AnswerItem.Equals(1));
                    txtGas.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.4") && a.AnswerItem.Equals(1));
                    txtDoctor.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.5.a") && a.AnswerItem.Equals(1));
                    txtLoan.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.5.b") && a.AnswerItem.Equals(1));
                    txtLoanDescription.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.6.a") && a.AnswerItem.Equals(1));
                    txtLegalDeductions.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.6.b") && a.AnswerItem.Equals(1));
                    txtLegalDeductionsDescription.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.7.a") && a.AnswerItem.Equals(1));
                    txtOtherExpenses.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.7.b") && a.AnswerItem.Equals(1));
                    txtOtherExpensesDescription.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.8") && a.AnswerItem.Equals(1));
                    txtElectricity.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.9") && a.AnswerItem.Equals(1));
                    txtCableTv.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.10.") && a.AnswerItem.Equals(1));
                    txtLivingPlace.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.11") && a.AnswerItem.Equals(1));
                    txtTransport.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.12") && a.AnswerItem.Equals(1));
                    txtInternet.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.13") && a.AnswerItem.Equals(1));
                    txtClothing.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.14") && a.AnswerItem.Equals(1));
                    txtWater.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("23.15") && a.AnswerItem.Equals(1));
                    txtEducation.Text = answer != null ? answer.AnswerValue.Trim() : Empty;
                   
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
                List<Tuple<string, byte, string,int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();

                bool haveExtraIncome = chkExtraIncome.Checked;
                string extraIncomeComesFrom = txtExtraIncomeComes.Text.Trim();
                bool principalProvider = chkPrincipalProvider.Checked;
                string feedingExpense = txtFeeding.Text.Trim();
                string electricityExpense = txtElectricity.Text.Trim();
                string cableTvExpense = txtCableTv.Text.Trim();
                string livingPlaceExpense = txtLivingPlace.Text.Trim();
                string transportExpense = txtTransport.Text.Trim();
                string internetExpense = txtInternet.Text.Trim();
                string clothingExpense = txtClothing.Text.Trim();
                string waterExpense = txtWater.Text.Trim();
                string educationExpense = txtEducation.Text.Trim();
                string telephoneExpense = txtPhone.Text.Trim();
                string gasForCookingExpense = txtGas.Text.Trim();
                string doctorExpense = txtDoctor.Text.Trim();
                string loansExpense = txtLoan.Text.Trim();
                string loansExpenseDescription = txtLoanDescription.Text.Trim();
                string legalDeductionsExpense = txtLegalDeductions.Text.Trim();
                string legalDeductionsExpenseDescription = txtLegalDeductionsDescription.Text.Trim();
                string otherExpenses = txtOtherExpenses.Text.Trim();
                string otherExpensesDescription = txtOtherExpensesDescription.Text.Trim();

                employeeAnswers.Add(new Tuple<string, byte, string, int>("22", 1, haveExtraIncome.ToString(),3));
                if (haveExtraIncome)
                {
                    txtExtraIncomeComes.Attributes.Remove("disabled");
                }
                if (!IsNullOrWhiteSpace(extraIncomeComesFrom))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("22.1", 1, extraIncomeComesFrom, 3));
                }                
                employeeAnswers.Add(new Tuple<string, byte, string, int>("22.2", 1, principalProvider.ToString(), 3));
                
                if (!IsNullOrWhiteSpace(feedingExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.1", 1, feedingExpense, 3));
                }
                if (!IsNullOrWhiteSpace(telephoneExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.2", 1, telephoneExpense, 3));
                }
                if (!IsNullOrWhiteSpace(gasForCookingExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.3", 1, gasForCookingExpense, 3));
                }
                if (!IsNullOrWhiteSpace(doctorExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.4", 1, doctorExpense, 3));
                }
                if (!IsNullOrWhiteSpace(loansExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.5.a", 1, loansExpense, 3));
                }
                if (!IsNullOrWhiteSpace(loansExpenseDescription))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.5.b", 1, loansExpenseDescription, 3));
                }
                if (!IsNullOrWhiteSpace(legalDeductionsExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.6.a", 1, legalDeductionsExpense, 3));
                }
                if (!IsNullOrWhiteSpace(legalDeductionsExpenseDescription))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.6.b", 1, legalDeductionsExpenseDescription, 3));
                }
                if (!IsNullOrWhiteSpace(otherExpenses))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.7.a", 1, otherExpenses, 3));
                }
                if (!IsNullOrWhiteSpace(otherExpensesDescription))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.7.b", 1, otherExpensesDescription, 3));
                }
                if (!IsNullOrWhiteSpace(electricityExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.8", 1, electricityExpense, 3));
                }
                if (!IsNullOrWhiteSpace(cableTvExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.9", 1, cableTvExpense, 3));
                }
                if (!IsNullOrWhiteSpace(livingPlaceExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.10.", 1, livingPlaceExpense, 3));
                }
                if (!IsNullOrWhiteSpace(transportExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.11", 1, transportExpense, 3));
                }
                if (!IsNullOrWhiteSpace(internetExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.12", 1, internetExpense, 3));
                }
                if (!IsNullOrWhiteSpace(clothingExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.13", 1, clothingExpense, 3));
                }
                if (!IsNullOrWhiteSpace(waterExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.14", 1, waterExpense, 3));
                }
                if (!IsNullOrWhiteSpace(educationExpense))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("23.15", 1, educationExpense, 3));
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
                            ,item.Item4) { SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.Expenses);
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
                return "FamilyWork.aspx";
            }
            else
            {
                if (totalMinorChildren > 0)
                {
                    return "MinorChildren.aspx";
                }
                else
                {
                    return "FamilyInformation.aspx";
                }
            }
        }
    }
}