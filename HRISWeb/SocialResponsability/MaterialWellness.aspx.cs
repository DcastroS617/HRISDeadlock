using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;

namespace HRISWeb.SocialResponsability
{
    public partial class MaterialWellness : System.Web.UI.Page
    {
        [Dependency]
        protected ITransportsBll<TransportEntity> objTransportsBll { get; set; }

        [Dependency]
        protected ITransportsBll<TransportEntity> objTransportationsToWorkBll { get; set; }

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
                LoadTransports();
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
            Response.Redirect("DiseasesFrequency.aspx", false);
        }

        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("HousingInformation.aspx", false);
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
        /// Load the Transport Means
        /// </summary>
        private void LoadTransports()
        {
            try
            {
                List<TransportEntity> transportBDList = new List<TransportEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports] != null)
                {
                    transportBDList = (List<TransportEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports];
                }

                else
                {
                    objTransportsBll = objTransportsBll ?? Application.GetContainer().Resolve<ITransportsBll<TransportEntity>>();
                    transportBDList = objTransportsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                cboTransportMeansThatHas.Items.AddRange(transportBDList.Where(t => t.TransportTypeCode.Equals(Convert.ToByte(HrisEnum.TransportType.OwnTransport)) || t.TransportTypeCode.Equals(Convert.ToByte(HrisEnum.TransportType.Both)))
                    .OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.TransportDescriptionSpanish : f.TransportDescriptionEnglish)
                    .Select(t => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? t.TransportDescriptionSpanish : t.TransportDescriptionEnglish
                        , t.TransportCode.ToString())).ToArray());

                cboTransportationForWorkplace.Items.AddRange(transportBDList.Where(t => t.TransportTypeCode.Equals(Convert.ToByte(HrisEnum.TransportType.TransportToGoWork)) || t.TransportTypeCode.Equals(Convert.ToByte(HrisEnum.TransportType.Both)))
                    .OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.TransportDescriptionSpanish : f.TransportDescriptionEnglish)
                    .Select(t => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? t.TransportDescriptionSpanish : t.TransportDescriptionEnglish
                        , t.TransportCode.ToString())).ToArray());
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
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilitySurveyQuestions, surveyQuestions);
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
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("30"));
                    hdfTransportMeansThatEmployeeHas.Value = answer != null ? answer.AnswerValue : Empty;
                    //                    
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("31"));
                    hdfTransportationForWorkplace.Value = answer != null ? answer.AnswerValue : Empty;
                    //

                    //answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("31.a"));
                    //txtOtherTransportMean.Text = answer != null ? answer.AnswerValue : Empty;
                    //if (answer != null)
                    //{
                    //    var values = hdfTransportationForWorkplace.Value.Split(',').Where(r => r == "4").FirstOrDefault();
                    //    var HasOther = cboTransportationForWorkplace.Items.FindByValue(values)?.Value == "4";

                    //    if (!HasOther)
                    //    {
                    //        txtOtherTransportMean.Text = string.Empty;
                    //    }
                    //    else
                    //    {
                    //        txtOtherTransportMean.Attributes.Remove("disabled");
                    //    }
                    //}
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
                    , "setTimeout(function () { RestoreSelectedTransports(); }, 200);", true);
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
                string transportMeansThatEmployeeHas = hdfTransportMeansThatEmployeeHas.Value.Trim();
                string transportsForWorkplace = hdfTransportationForWorkplace.Value.Trim();
                //string otherTransport = txtOtherTransportMean.Text.Trim();

                //
                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                if (!IsNullOrWhiteSpace(transportMeansThatEmployeeHas))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("30", 1, transportMeansThatEmployeeHas, 5));
                }

                //
                if (!IsNullOrWhiteSpace(transportsForWorkplace))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("31", 1, transportsForWorkplace, 5));
                }

                //
                //if (!IsNullOrWhiteSpace(otherTransport))
                //{
                //    employeeAnswers.Add(new Tuple<string, byte, string, int>("31.a", 1, otherTransport, 5));
                //    txtOtherTransportMean.Attributes.Remove("disabled");
                //}
                //else
                //{
                employeeAnswers.Add(new Tuple<string, byte, string, int>("31.a", 1, "", 5));
                //}

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
                        answer.IdSurveyModule = 5;
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
                 //
                 // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.MaterialWellness);
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
                    , "setTimeout(function () { RestoreSelectedTransports(); }, 200);", true);
            }
        }
    }
}