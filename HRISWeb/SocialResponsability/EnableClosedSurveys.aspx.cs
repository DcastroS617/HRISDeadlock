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
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.SocialResponsability
{
    public partial class EnableClosedSurveys : System.Web.UI.Page
    {
        [Dependency]
        protected IEmployeesBll<EmployeeEntity> objEmployeesBll { get; set; }
        [Dependency]
        protected IAdminUsersByModulesBll<AdminUserByModuleEntity> objAdminUsersByModulesBll { get; set; }
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }

        private readonly int SurveyVersion = 3;

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
                grvEmployees.DataSource = new List<EmployeeEntity>();
                grvEmployees.DataBind();
                hdfEmployeeCode.Value = Empty;
                lbtnEnableSurvey.Enabled = false;
                lbtnSearchEmployee.Enabled = false;
                hdfIsCancelEmployeeSearchEnabled.Value = "0";
                ShowSearchModal();
            }
        }
        /// <summary>
        /// Handles the lbtnCancelEmployeeSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnCancelEmployeeSearch_Click(object sender, EventArgs e)
        {
            // Nunca deberia ejecutarse este evento, solo esta en caso que el js falle
            Response.Redirect("../Default.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnAcceptSelectedEmployee click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnAcceptSelectedEmployee_Click(object sender, EventArgs e)
        {
            bool isValidSearch = true;
            try
            {
                string selectedEmployeeIndex = hdfSelectedRowIndex.Value.Trim();
                int selectedIndex = -1;

                if (!IsNullOrWhiteSpace(selectedEmployeeIndex) && int.TryParse(selectedEmployeeIndex, out selectedIndex))
                {
                    string employeeCode = Convert.ToString(grvEmployees.DataKeys[selectedIndex].Values[0]);
                    string geographicDivisionCode = Convert.ToString(grvEmployees.DataKeys[selectedIndex].Values[1]);
                    
                    hdfEmployeeCode.Value = employeeCode;
                    LoadEmployeeSurveys(employeeCode, geographicDivisionCode, this.SurveyVersion);
                }
                else
                {                    
                    MensajeriaHelper.MostrarMensaje(upnEmployeeSearch, TipoMensaje.Informacion, GetLocalResourceObject("EmployeeCodeRequired").ToString());
                    isValidSearch = false;
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
                lbtnEnableSurvey.Enabled = isValidSearch;
                lbtnSearchEmployee.Enabled = isValidSearch;
                hdfSelectedRowIndex.Value = "-1";

                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnFromProcessAcceptSelectedEmployeeResponse" + Guid.NewGuid().ToString()
                    , $"ProcessAcceptSelectedEmployeeResponse({isValidSearch.GetHashCode()});", true);
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

                if (!IsNullOrWhiteSpace(employeeCode))
                {
                    objEmployeesBll = objEmployeesBll ?? Application.GetContainer().Resolve<IEmployeesBll<EmployeeEntity>>();
                    employees = objEmployeesBll.FilterByGeographicDivisionAndEmployeeCode(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , employeeCode);
                }
                grvEmployees.DataSource = employees;
                grvEmployees.DataBind();

                if (employees.Count==0)
                {
                    string message = String.Format(Convert.ToString(GetLocalResourceObject("msgEmployeeSearchedNotFound")), SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionName);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, message);
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
        /// Handles the grvEmployeeSurveys pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvEmployeeSurveys_PreRender(object sender, EventArgs e)
        {
            if ((grvEmployeeSurveys.ShowHeader && grvEmployeeSurveys.Rows.Count > 0) || (grvEmployeeSurveys.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvEmployeeSurveys.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvEmployeeSurveys.ShowFooter && grvEmployeeSurveys.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvEmployeeSurveys.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        /// <summary>
        /// Handles the lbtnEnableSurvey click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnEnableSurvey_Click(object sender, EventArgs e)
        {
            bool isEnabledSuccessful = false;
            try
            {
                string selectedEmployeeSurveyIndex = hdfEmployeeSurveysSelectedRowIndex.Value.Trim();
                int selectedIndex = -1;

                if (!IsNullOrWhiteSpace(selectedEmployeeSurveyIndex) && !selectedEmployeeSurveyIndex.Equals("-1") && int.TryParse(selectedEmployeeSurveyIndex, out selectedIndex))
                {
                    long surveyCode = Convert.ToInt64(grvEmployeeSurveys.DataKeys[selectedIndex].Values[0]);
                    string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                    string employeeCode = hdfEmployeeCode.Value;

                    EnableEmployeeSurvey(surveyCode);
                    LoadEmployeeSurveys(employeeCode, geographicDivisionCode, this.SurveyVersion);
                                        
                    isEnabledSuccessful = true;
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(upnEmployeeSearch, TipoMensaje.Informacion, GetLocalResourceObject("msgInvalidSelection").ToString());
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
                hdfEmployeeSurveysSelectedRowIndex.Value = "-1";

                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnFromProcessAcceptSelectedEmployeeResponse" + Guid.NewGuid().ToString()
                    , $"ProcessEnableEmployeeSurveyResponse({isEnabledSuccessful.GetHashCode()});", true);
            }
        }
        /// <summary>
        /// Handles the lbtnSearchEmployee click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnSearchEmployee_Click(object sender, EventArgs e)
        {
            grvEmployees.DataSource = new List<EmployeeEntity>(); 
            grvEmployees.DataBind();

            ScriptManager.RegisterStartupScript(this
                , this.GetType()
                , "ReturnFromSearchEmployeeResponse" + Guid.NewGuid().ToString()
                , "ProcessSearchEmployeeResponse();", true);
        }

        /// <summary>
        /// Show the search modal for admin users
        /// </summary>
        private void ShowSearchModal()
        {
            try
            {
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                objAdminUsersByModulesBll = objAdminUsersByModulesBll ?? Application.GetContainer().Resolve<IAdminUsersByModulesBll<AdminUserByModuleEntity>>();
                // Determines if the current user is administrator of the social module
                bool isAdminUser = objAdminUsersByModulesBll.IsUserAdmin(currentUser.ActiveDirectoryUserAccount
                    , workingDivision.DivisionCode
                    , Convert.ToInt32(HrisEnum.Modules.ResponsabilidadSocial));

                lbtnEnableSurvey.Enabled = isAdminUser;
                lbtnSearchEmployee.Enabled = isAdminUser;

                ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , "ReturnFromShowSearchModalResponse" + Guid.NewGuid().ToString()
                        , $"showSearchModal({isAdminUser.GetHashCode().ToString()});", true);
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
        /// Load the last completed employee survey
        /// </summary>
        /// <param name="employeeCode">The employee code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        private void LoadEmployeeSurveys(string employeeCode, string geographicDivisionCode, int surveyVersion)
        {
            objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
            SurveyEntity lastSurvey = objSurveysBll.ListLastByEmployeeCodeGeographicDivision(employeeCode, geographicDivisionCode, surveyVersion);
            List<SurveyEntity> surveys = new List<SurveyEntity>();
            if (lastSurvey != null)
            {
                surveys.Add(lastSurvey);
            }
            grvEmployeeSurveys.DataSource = surveys.Where(s => s.SurveyStateCode.Equals((byte)SurveyStates.Completed));
            grvEmployeeSurveys.DataBind();
        }
        /// <summary>
        /// Enable the employee survey, set its state as draft
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        private void EnableEmployeeSurvey(long surveyCode)
        {
            objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
            string currentUser = UserHelper.GetCurrentFullUserName;

            objSurveysBll.SaveCurrentState(surveyCode
                , (byte)SurveyStates.Draft
                , (byte)SurveyPages.PersonalProfile
                , currentUser
                , null
                , currentUser
                , this.SurveyVersion);
        }                
    }
}