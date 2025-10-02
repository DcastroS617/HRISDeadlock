using HRISWeb.Shared;
using System.Threading;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Application.Business.Interfaces;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class StartSurvey : System.Web.UI.Page
    {
        [Dependency]
        protected IEmployeesBll<EmployeeEntity> objEmployeesBll { get; set; }

        [Dependency]
        protected IPoliticalDivisionsBll<PoliticalDivisionEntity> objPoliticalDivisionsBll { get; set; }

        [Dependency]
        protected IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity> objPoliticalDivisionsLabelsBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }

        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }

        [Dependency]
        protected IAdminUsersByModulesBll<AdminUserByModuleEntity> objAdminUsersByModulesBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll objGeneralConfigurationsBll { get; set; }

        [Dependency]
        protected IMaritalStatusBll<MaritalStatusEntity> objMaritalStatusBll { get; set; }

        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }

        private const string activarFichaInternet = "ActivarFichaInternet";
        private const string surveyVersionName = "SurveyVersion";
        private const string activeState = "Activo";

        private int surveyVersion;

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
            this.surveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());

            if (!IsPostBack)
            {
                if (objEmployeesBll == null)
                {
                    objEmployeesBll = Application.GetContainer().Resolve<IEmployeesBll<EmployeeEntity>>();
                }

                if (objSurveysBll == null)
                {
                    objSurveysBll = Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
                }

                LoadMaritalStatus();

                lbtnStartSurvey.Enabled = false;

                lblBirthProvince.Text = Convert.ToString(GetLocalResourceObject("lblBirthProvince.Text"));
                LoadPoliticalDivision(cboProvince, null);

                ConfigureStartSurveyByUser();

                grvEmployees.DataSource = new List<EmployeeEntity>();
                grvEmployees.DataBind();

                chkInformedConsent.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkInformedConsent.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

            }
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
                    , "setTimeout(function () {  }, 200);", true);
            }
        }

        /// <summary>
        /// Handles the lbtnStartSurvey click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnStartSurvey_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
                {
                    Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];

                    currentSurvey.Item1.BirthProvince = Convert.ToInt32(cboProvince.SelectedValue);
                    currentSurvey.Item1.BirthProvinceName = cboProvince.SelectedItem.Text.Trim();
                    currentSurvey.Item1.LastModifiedUser = UserHelper.GetCurrentFullUserName;

                    objGeneralConfigurationsBll = objGeneralConfigurationsBll ?? Application.GetContainer().Resolve<IGeneralConfigurationsBll>();
                    GeneralConfigurationEntity configuration = objGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.SocioeconomicCardAppLocal);
                    currentSurvey.Item1.PendingSynchronization = Convert.ToBoolean(Convert.ToInt32(configuration.GeneralConfigurationValue));

                    if (currentSurvey.Item1.IsNewSurvey)
                    {
                        currentSurvey.Item1.SurveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());
                        currentSurvey.Item1.SurveyCode = objSurveysBll.Add(currentSurvey.Item1);
                        currentSurvey.Item1.IsNewSurvey = false;
                    }
                    else if (currentSurvey.Item1.SurveyStateCode.Equals(Convert.ToByte(HrisEnum.SurveyStates.Draft)))
                    {
                        objSurveysBll.Edit(currentSurvey.Item1);
                    }

                    Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, currentSurvey);

                    SaveSurveyAnswers(true);

                    Response.Redirect("AcademicProfile.aspx", false);
                }

                else
                {
                    hdfSelectedRowIndex.Value = "-1";
                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Validacion, GetLocalResourceObject("msjErrorSurveySession").ToString());
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
        /// Handles the lbtnCancel click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx", false);
        }
       
        /// <summary>
        /// Handles the lbtnCancelEmployeeSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnCancelEmployeeSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx", false);
        }
       
        /// <summary>
        /// Handles the lbtnAcceptSelectedEmployee click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnAcceptSelectedEmployee_Click(object sender, EventArgs e)
        {
            bool surveyHasValidTimePeriod = true;
            var employeeValidate = 0;
            string currentState = "";

            try
            {
                string selectedEmployeeIndex = hdfSelectedRowIndex.Value.Trim();
                int selectedIndex = -1;

                if (!IsNullOrWhiteSpace(selectedEmployeeIndex) && int.TryParse(selectedEmployeeIndex, out selectedIndex))
                {
                    string employeeCode = Convert.ToString(grvEmployees.DataKeys[selectedIndex].Values[0]);
                    string geographicDivisionCode = Convert.ToString(grvEmployees.DataKeys[selectedIndex].Values[1]);
                    currentState = Convert.ToString(grvEmployees.DataKeys[selectedIndex].Values[2]);
                   
                    objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
                    if (currentState == activeState)
                    {
                        employeeValidate = objSurveysBll.SurveyEmployeeInactive(employeeCode, geographicDivisionCode);
                        if (employeeValidate == 0)
                        {
                            surveyHasValidTimePeriod = GetSurveyForEmployee(employeeCode, geographicDivisionCode, true);
                            LoadSurveyAnswers();
                        }

                        else
                        {
                            lbtnStartSurvey.Enabled = false;
                            hdfSelectedRowIndex.Value = "-1";
                            
                            var msg = GetLocalResourceObject(employeeValidate == -1 ? "MsgEmployeIsDelete" : "MsgEmployeIsDelete2").ToString();
                            MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, msg);
                        }
                    }

                    else {
                        lbtnStartSurvey.Enabled = false;
                        hdfSelectedRowIndex.Value = "-1";
                        MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, GetLocalResourceObject("deactiveEmployee").ToString());
                    }
                }

                else
                {
                    lbtnStartSurvey.Enabled = false;
                    hdfSelectedRowIndex.Value = "-1";
                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, GetLocalResourceObject("EmployeeCodeRequired").ToString());
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
                if (employeeValidate == 0 && currentState == activeState)
                {
                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromProcessAcceptSelectedEmployeeResponse" + Guid.NewGuid().ToString()
                        , $"ProcessAcceptSelectedEmployeeResponse({surveyHasValidTimePeriod.GetHashCode()});", true);
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromProcessAcceptSelectedEmployeeResponse" + Guid.NewGuid().ToString()
                        , $"DisableModalSearchEmployes();", true);
                }
            }
        }
        
        /// <summary>
        /// Handles the btnEmployeeSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnEmployeeSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string employeeCode = txtEmployeeCodeSearch.Text.Trim();

                List<EmployeeEntity> employees = new List<EmployeeEntity>();
                SurveyEmployeeCampaign employeeScope = null;
                var middleText = "";
                var finalText = "";
                var campaignName = "";

                if (!IsNullOrWhiteSpace(employeeCode))
                {
                    employees = objEmployeesBll.FilterByGeographicDivisionAndEmployeeCode(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , employeeCode);
                    
                    employeeScope = objSurveysBll.SurveyScopeEmployee(employeeCode, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                }   

                grvEmployees.DataSource = employees;
                grvEmployees.DataBind();

                //Si no se obtuvieron resultados con la cédula suministrada
                //mostrar un mensaje al usuario.
                if (employees.Count == 0)
                {
                    string message = String.Format(Convert.ToString(GetLocalResourceObject("msgEmployeeSearchedNotFound")), SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionName);
                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, message);
                }

                else {
                    if (employeeScope != null && employeeScope.IdSurveyScope > 0)
                    {
                        middleText = Convert.ToString(GetLocalResourceObject("msjInSurveyScopeMiddle"));
                        finalText = Convert.ToString(GetLocalResourceObject("msjInSurveyEmployeeScopeFinal")) + " ";
                    }

                    else
                    {
                        middleText = Convert.ToString(GetLocalResourceObject("msjOutSurveyScopeMiddle"));
                        finalText = Convert.ToString(GetLocalResourceObject("msjOutSurveyEmployeeScopeFinal")) + " ";
                    }

                    campaignName = employeeScope.SurveyScopeName;

                    lblScopeEmployee.Text = string.Empty;

                    if (!string.IsNullOrEmpty(campaignName))
                    {
                        lblScopeEmployee.Text = String.Format("{0} <b>{1} </b>{2} <b>{3} </b>", Convert.ToString(GetLocalResourceObject("msjSurveyEmployeeScopeInitial")), middleText, finalText, campaignName);
                    }
          
                    lblScopeEmployee.Visible = true;
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
                    , "ReturnFromProcessEmployeeSearchResponse" + Guid.NewGuid().ToString()
                    , "ProcessEmployeeSearchResponse();", true);
            }
        }
       
        /// <summary>
        /// Handles the grvEmployees pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvEmployees_PreRender(object sender, EventArgs e)
        {
            if ((grvEmployees.ShowHeader && grvEmployees.Rows.Count > 0) || (grvEmployees.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvEmployees.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvEmployees.ShowFooter && grvEmployees.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvEmployees.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
       
        /// <summary>
        /// Loads the political division by level
        /// </summary>
        /// <param name="cboPoliticalDivision"></param>
        /// <param name="parentPoliticalDivision"></param>  
        private void LoadPoliticalDivision(DropDownList cboPoliticalDivision, int? parentPoliticalDivision)
        {
            List<PoliticalDivisionEntity> politicalDivisionBDList = new List<PoliticalDivisionEntity>();

            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];
                objPoliticalDivisionsBll = objPoliticalDivisionsBll ?? Application.GetContainer().Resolve<IPoliticalDivisionsBll<PoliticalDivisionEntity>>();
                politicalDivisionBDList = objPoliticalDivisionsBll.ListEnabledByCountryByParentPoliticalDivision(currentSurvey?.Item1?.NationalityAlpha3Code == null ? 
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).CountryID : currentSurvey.Item1?.NationalityAlpha3Code, parentPoliticalDivision);

                cboPoliticalDivision.DataTextField = "PoliticalDivisionName";
                cboPoliticalDivision.DataValueField = "PoliticalDivisionID";
                cboPoliticalDivision.SelectedIndex = -1;
                
                if (!politicalDivisionBDList.Any())
                {
                    politicalDivisionBDList.Add(new PoliticalDivisionEntity(0, parentPoliticalDivision, "N/A"));
                }

                cboPoliticalDivision.DataSource = politicalDivisionBDList;
                cboPoliticalDivision.DataBind();
                cboPoliticalDivision.Items.Insert(0, new ListItem(Empty, "-1"));
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
       
        private void LoadMaritalStatus()
        {
            try
            {
                List<MaritalStatusEntity> maritalStatusList = new List<MaritalStatusEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogMaritalStatus] != null)
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
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            return Session[Constants.cCulture] != null ? new CultureInfo(Convert.ToString(Session[Constants.cCulture])) : new CultureInfo(Constants.cCultureEsCR);
        }
       
        /// <summary>
        /// Load the survey information for the page
        /// </summary>
        /// <param name="currentSurvey">The survey entity</param>
        private void LoadSurveyPageInformation(SurveyEntity currentSurvey)
        {
            txtStartDateTime.Text = currentSurvey.SurveyStartDateTimeFormatted;
            txtCompany.Text = currentSurvey.CompanyName;
            txtFarm.Text = currentSurvey.CostFarmName;
            txtDepartment.Text = currentSurvey.DepartmentName;
            txtPosition.Text = currentSurvey.PositionName;
            txtHireDate.Text = currentSurvey.HireDateFormatted;
            txtTelephone.Text = currentSurvey.FullOfficePhone;

            txtIdentificationDocumentNumber.Text = currentSurvey.EmployeeID;
            txtName.Text = currentSurvey.EmployeeName;
            txtBirthdate.Text = currentSurvey.BirthDateFormatted;
            txtAge.Text = currentSurvey.Age;
            txtGender.Text = currentSurvey.Gender;
            txtNationality.Text = currentSurvey.NationalityName;
           
            ListItem liBirthProvince = cboProvince.Items.FindByValue(Convert.ToString(currentSurvey.BirthProvince));
            cboProvince.SelectedIndex = -1;

            if (liBirthProvince != null)
            {
                liBirthProvince.Selected = true;
            }

            Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
            if (surveyAnswers?.Item2.Any() ?? false)
            {
                SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("8"));
                rdbMaritalStatus.SelectedValue = answer?.AnswerValue ?? "-1";

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
        /// Gets the current survey for the employee if the survey is not complete, or creates a new one if the establish period of time has elapsed
        /// </summary>
        /// <param name="employeeCode">The employee code</param>
        /// <param name="geographicDivisionCode">The geographic division of the employee</param>
        /// <param name="showSaveAsDraft">Show the information message for sabe the survey as draft?</param>
        /// <returns>Does the employee's last survey have a valid time period?</returns>
        private bool GetSurveyForEmployee(string employeeCode, string geographicDivisionCode, bool showSaveAsDraft)
        {
            bool surveyHasValidTimePeriod = false;
            try
            {
                SurveyEntity currentSurvey = null;
                List<SurveyAnswerEntity> surveyAnswers = null;
                bool isNewSurvey = true;
                int surveyVersionparam = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());

                currentSurvey = objSurveysBll.ListLastByEmployeeCodeGeographicDivision(employeeCode, geographicDivisionCode, surveyVersionparam);

                int surveryPeriodTime = GetSurveyTimePeriod();

                surveyHasValidTimePeriod = currentSurvey == null ? true
                    : currentSurvey.SurveyStateCode.Equals((byte)HrisEnum.SurveyStates.Draft) ? true
                        : currentSurvey.SurveyStartDateTime.Year < currentSurvey.SurveyStartDateTime.AddMonths(surveryPeriodTime).Year
                            && currentSurvey.SurveyStartDateTime.AddMonths(surveryPeriodTime).CompareTo(DateTime.Today) <= 0
                            ? true : false;

                if (surveyHasValidTimePeriod)
                {                    
                    if (currentSurvey == null
                        || (currentSurvey?.SurveyStateCode.Equals((byte)HrisEnum.SurveyStates.Completed) ?? false))
                    {
                        GeneralConfigurationEntity ecuadorDvisionCodeParameter = objGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.EcuadorDivisionCode);
                        GeneralConfigurationEntity peruDvisionCodeParameter = objGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.PeruDivisionCode);

                        EmployeeEntity selectedEmployee = objEmployeesBll.ListByEmployeeCodeGeographicDivision(employeeCode, geographicDivisionCode);

                        currentSurvey = new SurveyEntity(employeeCode
                            , geographicDivisionCode
                            , selectedEmployee.AccountingGeographicDivisionCode
                            , selectedEmployee.DivisionCode
                            , DateTime.Now
                            , selectedEmployee.ID
                            , selectedEmployee.EmployeeName
                            , selectedEmployee.BirthDate
                            , selectedEmployee.Alpha3Code
                            , selectedEmployee.Nationality
                            , selectedEmployee.Gender
                            , 0 // Birth Province
                            , selectedEmployee.CompanyID
                            , selectedEmployee.CompanyName
                            , selectedEmployee.CostFarmID
                            , selectedEmployee.CostFarmName // Farm name
                            , selectedEmployee.DepartmentCode
                            , selectedEmployee.DepartmentName
                            , selectedEmployee.PositionName
                            , selectedEmployee.DivisionCode == Convert.ToInt32(ecuadorDvisionCodeParameter.GeneralConfigurationValue)
                               || selectedEmployee.DivisionCode == Convert.ToInt32(peruDvisionCodeParameter.GeneralConfigurationValue) ? selectedEmployee.SeniorityDate : selectedEmployee.RecruitmentDate
                            , selectedEmployee.PhoneOffice
                            , selectedEmployee.TelephoneExtension
                            , true
                            , Convert.ToByte(HrisEnum.SurveyStates.Draft)
                            , Convert.ToByte(HrisEnum.SurveyPages.PersonalProfile)
                            , UserHelper.GetCurrentFullUserName
                            , null
                            , selectedEmployee.Seat);
                    }

                    else if (currentSurvey.SurveyStateCode.Equals((byte)HrisEnum.SurveyStates.Draft))
                    {
                        isNewSurvey = false;
                        objSurveyAnswersBll = objSurveyAnswersBll ?? Application.GetContainer().Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();
                        surveyAnswers = objSurveyAnswersBll.ListBySurveyCode(currentSurvey.SurveyCode, this.surveyVersion);
                    }  
                    
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard);
                    if (SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.Equals(currentSurvey.DivisionCode))
                    {
                        currentSurvey.IsNewSurvey = isNewSurvey;
                        surveyAnswers = surveyAnswers ?? new List<SurveyAnswerEntity>();
                        Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, new Tuple<SurveyEntity, List<SurveyAnswerEntity>>(currentSurvey, surveyAnswers));
                      
                        LoadPoliticalDivision(cboProvince, null);
                        LoadSurveyPageInformation(currentSurvey);
                        lbtnStartSurvey.Enabled = true;
                       
                        if (showSaveAsDraft)
                        {
                            MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, GetLocalResourceObject("SaveAsDraftText").ToString());
                        }
                    }

                    else
                    {
                        Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisions);
                    }
                }
                
                // IF surveyHasValidTimePeriod
                else
                {
                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgSurveyCarriedOut")));
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

            return surveyHasValidTimePeriod;
        }
       
        /// <summary>
        /// Configures the survey start screen, determines if the current user is administrator of the social module, if yes then show the search option, otherwise show the user information
        /// </summary>
        private void ConfigureStartSurveyByUser()
        {
            try
            {
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                if (currentUser == null || string.IsNullOrEmpty(currentUser?.ActiveDirectoryUserAccount))
                {
                    Response.Redirect("~/Default.aspx", true);

                }

                objAdminUsersByModulesBll = objAdminUsersByModulesBll ?? Application.GetContainer().Resolve<IAdminUsersByModulesBll<AdminUserByModuleEntity>>();
                
                // Determines if the current user is administrator of the social module
                bool isAdminUser = objAdminUsersByModulesBll.IsUserAdmin(currentUser.ActiveDirectoryUserAccount
                    , workingDivision.DivisionCode
                    , Convert.ToInt32(HrisEnum.Modules.ResponsabilidadSocial));
                
                isAdminUser = ConfigurationManager.AppSettings["activarFichaInternet"].ToString().Equals("true") ? false : isAdminUser;

                hdfCurrentUserIsModuleAdmin.Value = isAdminUser.GetHashCode().ToString();
               
                bool isActiveSurvey = false;
                if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
                {
                    Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];

                    isActiveSurvey = currentSurvey.Item1.IsPageCalledByBackButton;
                    if (isActiveSurvey || (!isAdminUser && currentSurvey.Item1.DivisionCode.Equals(workingDivision.DivisionCode)))
                    {
                        LoadSurveyPageInformation(currentSurvey.Item1);
                        lbtnStartSurvey.Enabled = true;
                       
                        currentSurvey.Item1.IsPageCalledByBackButton = false;
                        Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, currentSurvey);
                    }

                    else
                    {
                        Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard);
                        
                        // no mostrar consentimiento informado cuando no es admin y la división es diferente 
                        if (!isAdminUser && !currentSurvey.Item1.DivisionCode.Equals(workingDivision.DivisionCode))
                        {
                            isActiveSurvey = true;
                            lbtnStartSurvey.Enabled = false;
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgEmployeeNotFound")));
                        }
                    }
                }

                else
                {
                    if (!isAdminUser)
                    {
                        EmployeeEntity currentEmployee = objEmployeesBll.ListEmployeeByActiveDirectoryUserAccount(currentUser.ActiveDirectoryUserAccount,currentUser.EmailAddress);
                        bool isSameDivision = currentEmployee?.DivisionCode.Equals(workingDivision.DivisionCode) ?? false;
                        if (isSameDivision)
                        {
                            isActiveSurvey = !GetSurveyForEmployee(currentEmployee.EmployeeCode, currentEmployee.GeographicDivisionCode, false);
                            lbtnStartSurvey.Enabled = !isActiveSurvey;
                            cboProvince.Enabled = !isActiveSurvey;
                        }

                        else
                        {
                            // Se coloca la bandera de 'encuesta activa' para no mostrar el consentimiento informado
                            isActiveSurvey = true;
                            lbtnStartSurvey.Enabled = false;
                            cboProvince.Enabled = false;
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgEmployeeNotFound")));
                        }
                    }
                }

                if (!isActiveSurvey)
                {
                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromStartSurvey" + Guid.NewGuid().ToString()
                        , "setTimeout(function () {{ ShowInformedConsentDialog(); }}, 200);", true);
                }
            }

            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    Response.Redirect("~/Default.aspx", true);
                }

                else
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
        }
       
        /// <summary>
        /// Gets the socio-economic card period of time in which an employee can perform the survey again
        /// </summary>
        /// <returns>Months in witch an employee can perform the survey again</returns>
        private int GetSurveyTimePeriod()
        {
            objGeneralConfigurationsBll = objGeneralConfigurationsBll ?? Application.GetContainer().Resolve<IGeneralConfigurationsBll>();
            GeneralConfigurationEntity configuration = objGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.SocioeconomicCardRegistrationTimePeriod);
           
            int months = 0;
            int.TryParse(configuration?.GeneralConfigurationValue ?? Empty, out months);
           
            return months;
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

                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                if (!String.IsNullOrWhiteSpace(maritalStatus))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("8", 1, maritalStatus, 1));
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
                        answer.SurveyVersion = this.surveyVersion;
                        answer.IdSurveyModule = item.Item4;
                    }

                    else
                    {
                        surveyAnswers.Item2.Add(new SurveyAnswerEntity(surveyAnswers.Item1.SurveyCode
                            , surveyQuestions.Find(q => q.QuestionID.Equals(item.Item1))?.QuestionCode ?? 0
                            , item.Item1
                            , item.Item2
                            , item.Item3
                            , currentUser
                            , item.Item4)
                        { SurveyVersion = this.surveyVersion });
                    }
                }

                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.PersonalProfile);
                surveyAnswers.Item1.SurveyCompletedBy = currentUser;
                surveyAnswers.Item1.LastModifiedUser = currentUser;

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
                        , this.surveyVersion);

                    objSurveyAnswersBll = objSurveyAnswersBll ?? Application.GetContainer().Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();
                    objSurveyAnswersBll.Save(surveyAnswers.Item2);

                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, GetLocalResourceObject("msjSurveySavedAsDraft").ToString());

                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromProcessSaveAsDraftResponse" + Guid.NewGuid().ToString()
                        , "ProcessSaveAsDraftResponse();", true);
                }

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