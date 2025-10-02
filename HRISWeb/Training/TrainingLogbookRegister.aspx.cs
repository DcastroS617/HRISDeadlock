using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.Training
{
    public partial class TrainingLogbookRegister : Page
    {
        [Dependency]
        public IClassroomsBll<ClassroomEntity> ObjClassroomsBll { get; set; }

        [Dependency]
        public ITrainingCentersBll<TrainingCenterEntity> ObjTrainingCentersBll { get; set; }

        [Dependency]
        public LogbooksFilesBll ObjLogbooksFilesBll { get; set; }

        [Dependency]
        public ITrainersBll<TrainerEntity> ObjTrainersBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjCoursesBll { get; set; }

        [Dependency]
        public IEmployeesBll<EmployeeEntity> ObjEmployeesBll { get; set; }

        [Dependency]
        public ILogbooksBll ObjLogbooksBll { get; set; }

        [Dependency]
        public IMatrixTargetBll ObjMatrixTargetBll { get; set; }

        [Dependency]
        public CycleTrainingBll ObjCycleTrainingBll { get; set; }

        [Dependency]
        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }

        [Dependency]
        public IAdminUsersByModulesBll<AdminUserByModuleEntity> ObjAdminUsersByModulesBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjClasificacionCourse { get; set; }

        [Dependency]
        public IDivisionsByUsersBll<DivisionByUserEntity> ObjIDivisionsByUsersBll { get; set; }

        readonly string sessionKeyCoursesResults = "TrainingLogbookRegister-CoursesResults";
        readonly string sessionKeyCyleTrainingResults = "TrainingLogbookRegister-CyleTrainingResults";
        readonly string sessionKeyTrainersResults = "TrainingLogbookRegister-TrainersResults";
        readonly string sessionKeyTrainingCentersResults = "TrainingLogbookRegister-TrainingCentersResults";
        readonly string sessionKeyClassroomsResults = "TrainingLogbookRegister-ClassroomsResults";
        readonly string sessionKeyClassificationCourseResults = "TrainingLogbookRegister-ClassificationCourseResults";

        readonly string sessionKeyEmployeesResults = "TrainingLogbookRegister-EmployeesResults";
        readonly string sessionKeyInactiveEmployeesResults = "TrainingLogbookRegister-InactiveEmployeesResults";

        readonly string sessionKeyAdvancedSearchResults = "TrainingLogbookRegister-AdvancedSearchResults";
        readonly string sessionKeyAdvancedSearchEmployeeResults = "TrainingLogbookRegister-AdvancedSearchEmployeeResults";
        readonly string sessionKeyAdvancedSearchEmployeeSave = "TrainingLogbookRegister-AdvancedSearchEmployeeSave";
        readonly string sessionKeyAdvancedSelectedAllEmployees = "TrainingLogbookRegister-AdvancedSelectedAllEmployees";
        readonly string sessionKeyAdvancedSelectedPageEmployees = "TrainingLogbookRegister-AdvancedSelectedPageEmployees";

        readonly string sessionKeyDivisionsList = "StructBy-DivisionsListResults";
        readonly string sessionKeyCompaniesList = "StructBy-CompaniesListResults";
        readonly string sessionKeyCostZoneList = "StructBy-CostZoneListResults";
        readonly string sessionKeyCostMiniZoneList = "StructBy-CostMiniZoneListResults";
        readonly string sessionKeyCostFarmList = "StructBy-CostFarmListResults";
        readonly string sessionKeyNominalClassList = "StructBy-NominalClassListResults";
        readonly string sessionKeyCostCenterList = "StructBy-CostCenterListResults";

        readonly string sessionKeyParticipantsResults = "TrainingLogbookRegister-ParticipantsResults";
        readonly string sessionKeyTypeLogbookResults = "TrainingLogbooks-LogbookNumberResults";
        readonly string sessionKeyLogbookNumberResults = "TrainingLogbooks-LogbookTypeResults";
        readonly string sessionKeyStatusLogbookResults = "TrainingLogbooks-LogbookStatusResults";
        readonly string sessionKeyStatusLogbookDelete = "TrainingLogbooks-LogbookStatusDelete";

        //Variable that indicates whether the logbook is new or from another logbook
        readonly string sessionIsNewLogbook = "true";
        readonly string[] sessionNominalClass = { "91", "93", "O1", "O2", "EA", "EF" };

        #region Properties

        /// <summary>
        /// Get or set divisions.
        /// </summary>
        public List<MatrixTargetByDivisionsEntity> Divisions
        {
            get { return Session[sessionKeyDivisionsList] as List<MatrixTargetByDivisionsEntity>; }
            set { Session[sessionKeyDivisionsList] = value; }
        }

        /// <summary>
        /// Get or set divisions multiple.
        /// </summary>
        public string DivisionsMultiple => string.Join(",", Divisions.Select(r => r.DivisionCode));

        #endregion

        #region Events

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
            try
            {
                if (Request.Form["__EVENTTARGET"] != null)
                {
                    if (Request.Form["__EVENTTARGET"].Equals("ctl00$cntBody$btnAccept"))
                    {
                        if (VerifyControlsGeneral(sender) && VerifyDatesTime(sender))
                        {
                            return;
                        }
                    }

                    if (Request.Form["__EVENTTARGET"].Equals("ctl00$cntBody$txtSearchEmployees"))
                    {
                        TxtSearchEmployees_TextChanged(sender, e);
                    }

                    if (Request.Form["__EVENTTARGET"].Equals("ctl00$cntBody$grvAdvancedSearchEmployees$ctl01$chkAdvancedSearchSelectedAllEmployee"))
                    {
                        ChkAdvancedSearchSelectedAllEmployee_CheckedChanged(sender, e);
                    }
                }

                if (Request.QueryString.Count > 0)
                {
                    Session[sessionKeyStatusLogbookResults] = Request.QueryString.Get("status");
                    DisableCookie();
                }

                if (!IsPostBack)
                {
                    UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                    string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');
                    hdfUserAccount.Value = userAccount[0] + "$" + userAccount[1];

                    hdfGeographicDivisionCode.Value = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                    hdfTypeEmployeesSearch.Value = "A";
                    hdfTypeLogbook.Value = Session[sessionKeyTypeLogbookResults] as string;

                    StructByEdit.Items.Add(new ListItem() { Value = "1", Text = GetLocalResourceObject("StructByFarm").ToString() });
                    StructByEdit.Items.Add(new ListItem() { Value = "2", Text = GetLocalResourceObject("StructByNominalClass").ToString() });

                    BtnStructByEdit_ServerClick(sender, e);
                    PrepareFormForNewLogbook();

                    if (Session[sessionKeyLogbookNumberResults] != null && int.TryParse(Session[sessionKeyLogbookNumberResults] as string, out int tmpLogbookNumber))
                    {
                        txtLogbookNumber.Text = Convert.ToString(tmpLogbookNumber);

                        Session[sessionKeyLogbookNumberResults] = string.Empty;
                        System.Web.HttpContext.Current.Session["loogbook"] = tmpLogbookNumber.ToString();

                        SearchLogbook();
                    }
                    else
                    {
                        if (System.Web.HttpContext.Current.Session["loogbook"] != null && int.TryParse(System.Web.HttpContext.Current.Session["loogbook"] as string, out int tmpLogbookNumberChange))
                        {
                            txtLogbookNumber.Text = Convert.ToString(tmpLogbookNumberChange);

                            Session[sessionKeyLogbookNumberResults] = string.Empty;
                            System.Web.HttpContext.Current.Session["loogbook"] = tmpLogbookNumberChange.ToString();

                            SearchLogbook();
                        }
                    }
                }

                if (Request.QueryString.Count < 0 && Session[sessionKeyTypeLogbookResults].Equals("H"))
                {
                    btnPrint.Visible = true;
                    btnPrint.Disabled = false;

                    return;
                }

                ApplyExceptionsInButtons();

                //activate the pager
                if (Session[sessionKeyAdvancedSearchResults] != null)
                {
                    PageHelper<EmployeeEntity> pageHelper = Session[sessionKeyAdvancedSearchResults] as PageHelper<EmployeeEntity>;
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnNew click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnNew_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session[sessionKeyStatusLogbookResults] = "New";

                DisableCookie();

                Response.Redirect("TrainingLogbookRegister.aspx?status=New", false);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnNewFromThis click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnNewFromThis_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session[sessionKeyStatusLogbookResults] = "NewFromThis";

                DisableCookie();
                PrepareFormForNewFromThis();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session[sessionKeyStatusLogbookResults] = "Edit";

                PrepareFormForEdit();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDraft click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDraft_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (VerifyDatesTime(sender))
                {
                    return;
                }

                SaveLogbook(false);

                if (hdfIsLogbookClosed.Value.Contains("false"))
                {
                    DisableButtons(true);
                    DisableLinkButtons(false);

                    DisableButtonsExceptionDraft();
                    DisableLinkButtonsDraft();

                    Session[sessionIsNewLogbook] = "false";
                    SetFormDisabled();

                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        string.Format(Convert.ToString(GetLocalResourceObject("msgDraftCompleted")), txtLogbookNumber.Text), "ReturnBitacoraPage");
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnSave click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSave_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (VerifyDatesTime(sender))
                {
                    return;
                }

                SaveLogbook(true);

                if (hdfIsLogbookClosed.Value.Contains("true"))
                {
                    DisableButtons(true);
                    DisableLinkButtons(false);

                    DisableButtonsExceptionClosed();
                    DisableLinkButtonsClosed();

                    SetFormDisabled();

                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        string.Format(Convert.ToString(GetLocalResourceObject("msgSaveCompleted")), txtLogbookNumber.Text), "ReturnBitacoraPage");
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnSearchLogbook click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearchLogbook_ServerClick(object sender, EventArgs e)
        {
            try
            {
                SearchLogbook();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnSave click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DeleteLogbook();

                Session[sessionKeyStatusLogbookDelete] = "true";

                DisableCookie();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        newEx.Message);
                }
            }
        }

        #region Logbook

        /// <summary>
        /// Handles the cboTrainingCenter selected index changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void CboTrainingCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadClassroomsBySelectedTrainingCenter();

                FilterSearchedEmployees();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the cboCourse index changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void CboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateCoursePanel((Control)sender);
                PopulateCycleTrainingByCourse();
                PopulateTrainersByCourse();

                FilterSearchedEmployees();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the CboCycleTranining index changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void CboCycleTranining_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateCycleTrainingPanel((Control)sender);

                FilterSearchedEmployees();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the cboClassroom index changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>am>
        protected void CboClassroom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateClassroomPanel((Control)sender);

                FilterSearchedEmployees();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }

        }

        #endregion

        #region Participants

        /// <summary>
        /// Handles the btnAddParticipant click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddParticipant_ServerClick(object sender, EventArgs e)
        {
            try
            {
                AddParticipant(sender);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }

        }

        /// <summary>
        /// Handles the txtSearchEmployees text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchEmployees_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedEmployees();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppEmployees
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppEmployees
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnRefreshParticipants click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshParticipants_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshParticipants();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppParticipants
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppParticipants
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnRemoveParticipant click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRemoveParticipant_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveParticipant(sender);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #endregion

        #region Grade


        protected void TxtGrade_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SaveAllGrades();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        newEx.Message);
                }
            }
        }

        #endregion

        #region AdvancedSearch

        /// <summary>
        /// Handles the BtnStructByEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnStructByEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session[sessionKeyAdvancedSearchResults] = null;
                Session[sessionKeyAdvancedSearchEmployeeSave] = null;

                Session[sessionKeyAdvancedSelectedAllEmployees] = false;
                Session[sessionKeyAdvancedSelectedPageEmployees] = new List<string>();

                int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                Divisions = new List<MatrixTargetByDivisionsEntity>() { new MatrixTargetByDivisionsEntity() { DivisionCode = divisionCode } };

                LoadCompanies();
                LoadNominalClass();

                LoadCostZone();
                LoadCostMiniZone();
                LoadCostFarms();

                LoadCostCenterByStruct();

                ClearResultSearch();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnCostZoneIdEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        public void BtnCostZoneIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CostZoneIdEdit.Value))
                {
                    List<MatrixTargetByCostMiniZonesEntity> costMiniZoneList = Session[sessionKeyCostMiniZoneList] as List<MatrixTargetByCostMiniZonesEntity>;

                    costMiniZoneList = costMiniZoneList.Where(w => CostZoneIdEdit.Value.Equals(w.CostZoneID)).ToList();

                    var options = costMiniZoneList.AsEnumerable().Select(fr => new ListItem
                    {
                        Value = fr.CostMiniZoneId,
                        Text = fr.CostMiniZoneName
                    }).ToArray();

                    CostMiniZoneIdEdit.Items.Clear();
                    CostMiniZoneIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
                    CostMiniZoneIdEdit.Items.AddRange(options);
                    CostMiniZoneIdEdit.SelectedIndex = 0;

                    CostFarmsIdEdit.Items.Clear();
                    CostCenterIdEdit.Items.Clear();

                    ClearResultSearch();
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        public void BtnCostMiniZoneIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CostZoneIdEdit.Value) && !string.IsNullOrEmpty(CostMiniZoneIdEdit.Value))
                {
                    List<MatrixTargetByCostFarmsEntity> costFarmList = Session[sessionKeyCostFarmList] as List<MatrixTargetByCostFarmsEntity>;

                    costFarmList = costFarmList.Where(w =>
                        CostZoneIdEdit.Value.Equals(w.CostZoneID) &&
                        CostMiniZoneIdEdit.Value.Equals(w.CostMiniZoneID)).ToList();

                    var options = costFarmList.AsEnumerable().Select(fr => new ListItem
                    {
                        Value = fr.CostFarmId,
                        Text = fr.CostFarmName
                    }).ToArray();

                    CostFarmsIdEdit.Items.Clear();
                    CostFarmsIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
                    CostFarmsIdEdit.Items.AddRange(options);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the ddlOnchange click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        public void BtnCompanyIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyIdEdit.Value))
                {
                    List<MatrixTargetByNominalClassEntity> costNominalClassList = Session[sessionKeyNominalClassList] as List<MatrixTargetByNominalClassEntity>;

                    costNominalClassList = costNominalClassList.Where(w => CompanyIdEdit.Value.Equals(w.CompanyCode)).ToList();

                    var options = costNominalClassList.AsEnumerable().Select(fr => new ListItem
                    {
                        Value = fr.NominalClassId,
                        Text = fr.NominalClassName
                    }).ToArray();

                    NominalClassIdEdit.Items.Clear();
                    NominalClassIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
                    NominalClassIdEdit.Items.AddRange(options);
                    NominalClassIdEdit.SelectedIndex = 0;

                    CostCenterIdEdit.Items.Clear();

                    ClearResultSearch();
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the BtnCostCenterByStructByNominalClass click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnCostCenterByStructByNominalClass_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyIdEdit.Value) && !string.IsNullOrEmpty(NominalClassIdEdit.Value))
                {
                    List<MatrixTargetByCostCentresEntity> costCentresEntities = Session[sessionKeyCostCenterList] as List<MatrixTargetByCostCentresEntity>;

                    costCentresEntities = costCentresEntities.Where(w =>
                        !string.IsNullOrEmpty(w.CompanyCode) &&
                        !string.IsNullOrEmpty(w.NominalClassId)).ToList();

                    costCentresEntities = costCentresEntities.Where(w =>
                        CompanyIdEdit.Value.Equals(w.CompanyCode) &&
                        NominalClassIdEdit.Value.Equals(w.NominalClassId)).ToList();

                    costCentresEntities = costCentresEntities.GroupBy(gb => gb.CostCenterName).Select(s => s.First()).ToList();

                    var options = costCentresEntities.AsEnumerable().Select(fr => new ListItem
                    {
                        Value = fr.CostCenterID,
                        Text = fr.CostCenterName
                    }).ToArray();

                    CostCenterIdEdit.Items.Clear();
                    CostCenterIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
                    CostCenterIdEdit.Items.AddRange(options);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the BtnCostCenterByStructByFarm click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnCostCenterByStructByFarm_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CostZoneIdEdit.Value) && !string.IsNullOrEmpty(CostMiniZoneIdEdit.Value) && !string.IsNullOrEmpty(CostFarmsIdEdit.Value))
                {
                    List<MatrixTargetByCostCentresEntity> costCentresEntities = Session[sessionKeyCostCenterList] as List<MatrixTargetByCostCentresEntity>;

                    costCentresEntities = costCentresEntities.Where(w =>
                        !string.IsNullOrEmpty(w.CostZoneID) &&
                        !string.IsNullOrEmpty(w.CostMiniZoneID) &&
                        !string.IsNullOrEmpty(w.CostFarmID)).ToList();

                    costCentresEntities = costCentresEntities.Where(w =>
                        CostZoneIdEdit.Value.Equals(w.CostZoneID) &&
                        CostMiniZoneIdEdit.Value.Equals(w.CostMiniZoneID) &&
                        CostFarmsIdEdit.Value.Equals(w.CostFarmID)).ToList();

                    costCentresEntities = costCentresEntities.GroupBy(gb => gb.CostCenterName).Select(s => s.First()).ToList();

                    var options = costCentresEntities.AsEnumerable().Select(fr => new ListItem
                    {
                        Value = fr.CostCenterID,
                        Text = fr.CostCenterName
                    }).ToArray();

                    CostCenterIdEdit.Items.Clear();
                    CostCenterIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
                    CostCenterIdEdit.Items.AddRange(options);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdvancedSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DataTable employees = LoadEmptyEmployees();
                Session[sessionKeyAdvancedSearchEmployeeResults] = employees;

                if (Session[sessionKeyAdvancedSearchEmployeeSave] == null)
                {
                    Session[sessionKeyAdvancedSearchEmployeeSave] = employees;
                }

                SearchResults(1);

                CommonFunctions.ResetSortDirection(Page.ClientID, grvAdvancedSearchEmployees.ClientID);

                DisplayResults();
                DisplayRowsChecked();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the BtnRefreshAdvancedSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAdvancedSearch_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAdvancedSearch();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the txtAdvancedSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtAdvancedSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtAdvancedSearchEmployees.Text))
                {
                    SearchResults(1);
                    DisplayResults();
                }

                else
                {
                    BtnAdvancedSearch_ServerClick(sender, e);
                }

                DisplayResults();

                CommonFunctions.ResetSortDirection(Page.ClientID, grvAdvancedSearchEmployees.ClientID);

                DisplayRowsChecked();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the chkAdvancedSearchSelectedAllEmployee click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void ChkAdvancedSearchSelectedAllEmployee_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk;
            try
            {
                if (sender.GetType().Name == "training_traininglogbookregister_aspx")
                {
                    chk = (CheckBox)((System.Web.UI.Page)sender).Form.FindControl("ctl00$cntBody$grvAdvancedSearchEmployees$ctl01$chkAdvancedSearchSelectedAllEmployee");
                }
                else
                {
                    chk = sender as CheckBox;
                }

                PageHelper<EmployeeEntity> pageHelper = (PageHelper<EmployeeEntity>)Session[sessionKeyAdvancedSearchResults];

                List<string> pages = Session[sessionKeyAdvancedSelectedPageEmployees] as List<string>;
                string seletedPage = pages.FirstOrDefault(w => w.Contains($"P{pageHelper.CurrentPage}|"));

                if (!string.IsNullOrEmpty(seletedPage))
                {
                    pages.Remove(seletedPage);
                    if (chk.Checked)
                    {
                        var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                        seletedPage = $"P{pageHelper.CurrentPage}|{chk.Checked}|{pageSizeValue}";
                        pages.Add(seletedPage);
                    }

                    else
                    {
                        var options = seletedPage.Split('|');
                        seletedPage = $"{options[0]}|{chk.Checked}|{1}";

                        pages.Add(seletedPage);
                    }
                }

                Session[sessionKeyAdvancedSelectedPageEmployees] = pages;

                ProcessSelectedRows(chk.Checked);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the chkAdvancedSearchSelectedEmployee click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void ChkAdvancedSearchSelectedEmployee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                var parent = chk.Parent;

                DataTable employees = Session[sessionKeyAdvancedSearchEmployeeSave] as DataTable;

                ControlCheckedIndividual(chk.Checked);

                if (chk.Checked)
                {
                    CreateEmployeeBySelectedRow(parent, employees);
                }

                else
                {
                    DeleteEmployeeBySelectedRow(parent, employees);
                }

                Session[sessionKeyAdvancedSearchEmployeeSave] = employees;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DataTable employees = Session[sessionKeyAdvancedSearchEmployeeSave] as DataTable;

                if (employees == null)
                {
                    return;
                }

                if (employees.Rows.Count <= 0)
                {
                    return;
                }

                DataTable participants = LoadParticipants();

                int capacity = int.Parse(txtClassroomCapacity.InnerText);

                var dif = (participants.Rows.Count - capacity) * -1;
                if (employees.Rows.Count > dif)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Validacion,
                        Convert.ToString(GetLocalResourceObject("msjClassroomCapacityExceeded")));

                    PageHelper<EmployeeEntity> pageHelper = (PageHelper<EmployeeEntity>)Session[sessionKeyAdvancedSearchResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                    return;
                }

                if (capacity == 0 || participants.Rows.Count > capacity)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Validacion,
                        Convert.ToString(GetLocalResourceObject("msjClassroomCapacityExceeded")));

                    PageHelper<EmployeeEntity> pageHelper = (PageHelper<EmployeeEntity>)Session[sessionKeyAdvancedSearchResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                    return;
                }

                employees.AsEnumerable().ToList().ForEach(pt =>
                {
                    participants.Rows.Add(pt.Field<string>("EmployeeCode"),
                    pt.Field<string>("employeeName"),
                    pt.Field<string>("CostCenter"),
                    pt.Field<string>("NominalClassId"),
                    GetNominalClassByParticipant(pt.Field<string>("NominalClassId")), false, null, 0, null);
                });

                rptParticipants.DataSource = participants;
                rptParticipants.DataBind();
                uppParticipants.Update();

                lblSearchParticipantsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults")), participants.DefaultView.Count);

                UpdateCountNominalClass();

                RegisterStartupScript(Page, "ReturnPostBackAcceptClickSave");
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BlstPager_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    int page = Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value);
                    PagerUtil.SetActivePage(blstPager, page);

                    SearchResults(page);
                    DisplayResults();

                    DisplayRowsChecked();

                    KeepSelectedEmployee(page);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvAdvancedSearchEmployees_PreRender(object sender, EventArgs e)
        {
            if ((grvAdvancedSearchEmployees.ShowHeader && grvAdvancedSearchEmployees.Rows.Count > 0) || (grvAdvancedSearchEmployees.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvAdvancedSearchEmployees.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvAdvancedSearchEmployees.ShowFooter && grvAdvancedSearchEmployees.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvAdvancedSearchEmployees.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvAdvancedSearchEmployees_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyAdvancedSearchResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvAdvancedSearchEmployees.ClientID, e.SortExpression);

                PageHelper<EmployeeEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
                DisplayRowsChecked();
            }
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Create logbook entity by logbook number
        /// </summary>
        private LogbookEntity CreateLogbookEntity()
        {
            LogbookEntity logbook = null;
            if (!string.IsNullOrEmpty(hdfTypeLogbook.Value))
            {
                if (hdfTypeLogbook.Value.Equals("H"))
                {
                    logbook = ObjLogbooksBll.ListHistoryByKey(
                        Convert.ToInt32(txtLogbookNumber.Text),
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    );
                }

                else if (hdfTypeLogbook.Value.Equals("N"))
                {
                    logbook = ObjLogbooksBll.ListByKey(
                       Convert.ToInt32(txtLogbookNumber.Text),
                       SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                       SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                   );
                }
            }

            return logbook;
        }

        /// <summary>
        /// Disable the cookie
        /// </summary>
        private void DisableCookie()
        {
            System.Web.HttpContext.Current.Session["loogbook"] = string.Empty;
        }

        /// <summary>
        /// Disable all buttons for the maintenice
        /// </summary>
        private void DisableButtons(bool state)
        {
            btnNew.Visible = !state;
            btnNew.Disabled = state;

            btnNewFromThis.Visible = !state;
            btnNewFromThis.Disabled = state;

            btnEdit.Visible = !state;
            btnEdit.Disabled = state;

            btnDraft.Visible = !state;
            btnDraft.Disabled = state;

            btnSave.Visible = !state;
            btnSave.Disabled = state;

            btnDelete.Visible = !state;
            btnDelete.Disabled = state;

            btnPrint.Visible = !state;
            btnPrint.Disabled = state;

            btnSearchLogbook.Visible = !state;
            btnSearchLogbook.Disabled = state;

            RegisterStartupScript(Page, "setTimeout(function () {{ DisableButtons('disabled'); }}, 10);", true);
        }

        /// <summary>
        /// Prepare the buttons for logbook
        /// </summary>
        private void DisableButtonsExceptionNew()
        {
            btnNew.Visible = true;
            btnNew.Disabled = false;

            btnDraft.Visible = true;
            btnDraft.Disabled = false;

            btnSave.Visible = true;
            btnSave.Disabled = false;
        }

        /// <summary>
        /// Prepare the buttons for logbook
        /// </summary>
        private void DisableButtonsExceptionDraft()
        {
            btnNew.Visible = true;
            btnNew.Disabled = false;

            btnNewFromThis.Visible = true;
            btnNewFromThis.Disabled = false;

            btnEdit.Visible = true;
            btnEdit.Disabled = false;

            btnDelete.Visible = true;
            btnDelete.Disabled = false;

            btnPrint.Visible = true;
            btnPrint.Disabled = false;

            RegisterStartupScript(Page, "setTimeout(function () {{ DisableButtons('enabled'); }}, 200);", true);
        }

        /// <summary>
        /// Prepare the buttons for logbook
        /// </summary>
        private void DisableButtonsExceptionEdit()
        {
            btnNew.Visible = true;
            btnNew.Disabled = false;

            btnNewFromThis.Visible = true;
            btnNewFromThis.Disabled = false;

            btnDraft.Visible = true;
            btnDraft.Disabled = false;

            btnSave.Visible = true;
            btnSave.Disabled = false;

            btnDelete.Visible = true;
            btnDelete.Disabled = false;

            btnPrint.Visible = true;
            btnPrint.Disabled = false;

            RegisterStartupScript(Page, "setTimeout(function () {{ DisableButtons('disabled'); }}, 200);", true);
        }

        /// <summary>
        /// Prepare the buttons for logbook
        /// </summary>
        private void DisableButtonsExceptionClosed()
        {
            if (hdfTypeLogbook.Value.Equals("H"))
            {
                btnNew.Visible = false;
                btnNew.Disabled = true;

                btnNewFromThis.Visible = false;
                btnNewFromThis.Disabled = true;

                btnDelete.Visible = false;
                btnDelete.Disabled = true;
            }

            else
            {
                btnNew.Visible = true;
                btnNew.Disabled = false;

                btnNewFromThis.Visible = true;
                btnNewFromThis.Disabled = false;

                btnDelete.Visible = true;
                btnDelete.Disabled = false;
            }

            btnPrint.Visible = true;
            btnPrint.Disabled = false;
            RegisterStartupScript(Page, "setTimeout(function () {{ DisableButtons('enabled'); }}, 200);", true);
        }

        /// <summary>
        /// Disable all link buttons for the maintenice
        /// </summary>
        private void DisableLinkButtons(bool state)
        {
            lnkbtnNewLogbook.Visible = state;
            lnkbtnNewLogbook.Enabled = state;

            lnkbtnNewLogbookFromThis.Visible = state;
            lnkbtnNewLogbookFromThis.Enabled = state;

            lnkbtnEditLogbook.Visible = state;
            lnkbtnEditLogbook.Enabled = state;

            lnkbtnDraftLogbook.Visible = state;
            lnkbtnDraftLogbook.Enabled = state;

            lnkbtnSaveLogbook.Visible = state;
            lnkbtnSaveLogbook.Enabled = state;

            lnkbtnDeleteLogbook.Visible = state;
            lnkbtnDeleteLogbook.Enabled = state;
        }

        /// <summary>
        /// Disable all link buttons for the maintenice
        /// </summary>
        private void DisableLinkButtonsDraft()
        {
            lnkbtnNewLogbook.Visible = true;
            lnkbtnNewLogbook.Enabled = true;

            lnkbtnNewLogbookFromThis.Visible = true;
            lnkbtnNewLogbookFromThis.Enabled = true;

            lnkbtnEditLogbook.Visible = true;
            lnkbtnEditLogbook.Enabled = true;

            lnkbtnDeleteLogbook.Visible = true;
            lnkbtnDeleteLogbook.Enabled = true;
        }

        /// <summary>
        /// Disable all link buttons for the maintenice
        /// </summary>
        private void DisableLinkButtonsClosed()
        {
            lnkbtnNewLogbook.Visible = true;
            lnkbtnNewLogbook.Enabled = true;

            lnkbtnNewLogbookFromThis.Visible = true;
            lnkbtnNewLogbookFromThis.Enabled = true;

            lnkbtnDeleteLogbook.Visible = true;
            lnkbtnDeleteLogbook.Enabled = true;
        }

        /// <summary>
        /// Apply exceptions in buttons depend status
        /// </summary>
        private void ApplyExceptionsInButtons()
        {
            DisableButtons(true);
            DisableLinkButtons(false);

            if (Session[sessionKeyStatusLogbookResults] == null)
            {
                Session[sessionKeyStatusLogbookResults] = "New";
            }

            if (Session[sessionKeyStatusLogbookResults].Equals("New") || Session[sessionKeyStatusLogbookResults].Equals("NewFromThis"))
            {
                DisableButtonsExceptionNew();
            }

            if (Session[sessionKeyStatusLogbookResults].Equals("Edit"))
            {
                DisableButtonsExceptionEdit();
            }

            if (Session[sessionKeyStatusLogbookResults].Equals("Draft"))
            {
                DisableButtonsExceptionDraft();
                DisableLinkButtonsDraft();
            }

            if (Session[sessionKeyStatusLogbookResults].Equals("Closed"))
            {
                DisableButtonsExceptionClosed();
                DisableLinkButtonsClosed();
            }
        }

        /// <summary>
        /// Verify controls general the are logbook
        /// </summary>
        private bool VerifyControlsGeneral(object sender)
        {
            bool retorno = false;

            if (cboCourse.SelectedValue.Equals("0"))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + cboCourse.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            if (cboTrainer.SelectedValue.Equals("0"))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + cboTrainer.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            if (cboClassificationCourseId.Value.Equals("0"))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + cboClassificationCourseId.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            if (cboTrainingCenter.SelectedValue.Equals("0"))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + cboTrainingCenter.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            if (string.IsNullOrEmpty(cboClassroom.SelectedValue))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + cboClassroom.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            return retorno;
        }

        /// <summary>
        /// Verify DatesTime the are logbook
        /// </summary>
        private bool VerifyDatesTime(object sender)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(dtpStartDate.Text))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + dtpStartDate.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            if (string.IsNullOrEmpty(tpcStarTime.Text))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + tpcStarTime.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            if (string.IsNullOrEmpty(dtpEndDate.Text))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + dtpEndDate.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                retorno = true;
            }

            return retorno;
        }

        /// <summary>
        /// Prepare the logbook
        /// </summary>
        private void PrepareLogbook(string isEnabled)
        {
            Session[sessionIsNewLogbook] = isEnabled;

            txtLogbookNumber.ReadOnly = false;
            txtLogbookNumber.Text = string.Empty;

            hdfCreatedBy.Value = string.Empty;

            txtLogbookStatus.Text = GetLogbookStatusLocalizatedDescription(LogbookStatus.New);
        }

        /// <summary>
        /// Prepare the controls for logbook
        /// </summary>
        private void PrepareControlsForLogbook(bool isNewLogbook)
        {
            dtpStartDate.Enabled = true;
            tpcStarTime.Enabled = true;
            dtpEndDate.Enabled = true;

            if (isNewLogbook)
            {
                dtpStartDate.Text = string.Empty;
                tpcStarTime.Text = string.Empty;
                dtpEndDate.Text = string.Empty;
            }

            cboTrainingCenter.Enabled = true;
            cboClassroom.Enabled = true;

            cboTrainer.Enabled = true;
            cboCourse.Enabled = true;
            cboCycleTranining.Enabled = true;

            cboClassificationCourseId.Disabled = false;
        }

        /// <summary>
        /// Prepare UI for new logbook
        /// </summary>
        private void PrepareFormForNewLogbook()
        {
            LimpiarSession();
            ApplyExceptionsInButtons();
            PrepareLogbook("true");

            LoadMinGrade();

            PrepareCoursesForLogbook();
            PopulateCoursePanel(null);

            PrepareCycleTrainingForLogbook();
            PopulateCycleTrainingPanel(null);

            PrepareTrainersForLogbook();
            PrepareTrainingCenterForLogbook();
            PrepareClassroomsForLogbook();

            LoadClassroomsBySelectedTrainingCenter();

            PrepareClassificationCoursesForLogbook();

            PrepareParticipantsForLogbook();
            PrepareEmployeesForLogbook();

            PrepareControlsForLogbook(true);
        }

        /// <summary>
        /// Prepare the UI for new logbbook importing data
        /// </summary>
        private void PrepareFormForNewFromThis()
        {
            DataTable participantsSession = Session[sessionKeyParticipantsResults] as DataTable;

            lblSearchEmployeesResults.InnerHtml = Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults"));
            lblSearchParticipantsResults.InnerHtml = Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults"));
            lblAdministratives.InnerHtml = string.Format("{0} : 0", Convert.ToString(GetLocalResourceObject("lblAdministratives")));
            lblOperatives.InnerHtml = string.Format("{0} : 0", Convert.ToString(GetLocalResourceObject("lblOperatives")));

            string msj = string.Empty;

            LimpiarSession();
            PrepareLogbook("false");

            PrepareCoursesForLogbook(ref msj);
            PrepareTrainersForLogbook(ref msj);
            PrepareTrainingCenterForLogbook(ref msj);
            PrepareClassroomsForLogbook(ref msj);

            LoadClassroomsBySelectedTrainingCenter();

            Session[sessionKeyParticipantsResults] = participantsSession;
            PrepareParticipantsForLogbook();
            PrepareEmployeesForLogbook();

            lblSearchParticipantsResults.InnerHtml = string.Format("{0} {1}",
                Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults")), participantsSession.DefaultView.Count);

            UpdateCountNominalClass();

            PrepareControlsForLogbook(false);

            hdfIsFormEnabled.Value = "true";
            hdfIsLogbookClosed.Value = "false";

            DisableButtons(true);
            DisableLinkButtons(false);
            DisableButtonsExceptionNew();

            if (!string.IsNullOrWhiteSpace(msj))
            {
                msj = string.Format("{0}<br/><br/>{1}", Convert.ToString(GetLocalResourceObject("msjEntitiesNotActiveNotUsed")), msj);

                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Advertencia, msj);
            }
        }

        /// <summary>
        /// Prepare the UI for edit logbook
        /// </summary>
        private void PrepareFormForEdit()
        {
            bool.TryParse(hdfIsLogbookClosed.Value, out bool isLogbookClosed);

            RefreshTableParticipant();

            if (isLogbookClosed)
            {
                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Validacion,
                    Convert.ToString(GetLocalResourceObject("msjLogbookNotEditableClosed")));

                return;
            }

            if (!UserHelper.GetCurrentFullUserName.Equals(hdfCreatedBy.Value, StringComparison.InvariantCultureIgnoreCase) &&
                !ObjAdminUsersByModulesBll.IsUserAdmin(UserHelper.GetCurrentFullUserName, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, GeneralParameters.cTrainingModuleCode))
            {
                Session[sessionKeyStatusLogbookResults] = "Draft";
                ApplyExceptionsInButtons();
                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Validacion,
                    Convert.ToString(GetLocalResourceObject("msjLogbookNotEditableAccessDenied")));
                return;
            }

            txtLogbookNumber.ReadOnly = true;
            cboTrainer.Enabled = true;
            cboCourse.Enabled = true;
            cboCycleTranining.Enabled = true;
            dtpStartDate.Enabled = true;
            tpcStarTime.Enabled = true;
            dtpEndDate.Enabled = true;
            cboTrainingCenter.Enabled = true;
            cboClassroom.Enabled = true;
            cboClassificationCourseId.Disabled = false;

            txtSearchEmployees.Text = string.Empty;
            rptEmployees.DataSource = null;
            rptEmployees.DataBind();
            uppEmployees.Update();

            hdfIsFormEnabled.Value = "true";

            DisableButtons(true);
            DisableLinkButtons(false);
            DisableButtonsExceptionEdit();

            RegisterStartupScript(Page, "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
        }

        /// <summary>
        /// Set the form controls as disabled for a read-only logbook
        /// </summary>
        private void SetFormDisabled()
        {
            txtLogbookNumber.ReadOnly = true;
            cboTrainer.Enabled = false;
            cboCourse.Enabled = false;
            cboCycleTranining.Enabled = false;
            dtpStartDate.Enabled = false;
            tpcStarTime.Enabled = false;
            dtpEndDate.Enabled = false;
            cboTrainingCenter.Enabled = false;
            cboClassroom.Enabled = false;
            cboClassificationCourseId.Disabled = true;

            hdfIsFormEnabled.Value = "false";
        }

        /// <summary>
        /// Search logbook by logbook number
        /// </summary>
        private void SearchLogbook()
        {
            if (string.IsNullOrEmpty(txtLogbookNumber.Text))
            {
                return;
            }

            LogbookEntity logbook = CreateLogbookEntity();
            if (logbook != null)
            {
                txtLogbookNumber.Text = Convert.ToString(logbook.LogbookNumber);
                txtLogbookStatus.Text = logbook.IsClosed.Value ? GetLogbookStatusLocalizatedDescription(LogbookStatus.Closed) : GetLogbookStatusLocalizatedDescription(LogbookStatus.Draft);

                hdfCreatedBy.Value = logbook.CreatedBy;
                hdfIsLogbookClosed.Value = Convert.ToString(logbook.IsClosed);

                var isPresentProperty = logbook.IsPresentAll.HasValue ? logbook.IsPresentAll.Value.ToString().ToLower() : "false";

                RegisterStartupScript(Page, "setTimeout(function () {  IsPresentAll('" + isPresentProperty + "'); },200); ", true);

                Session[sessionKeyStatusLogbookResults] = logbook.IsClosed.Value ? Convert.ToString(LogbookStatus.Closed) : Convert.ToString(LogbookStatus.Draft);

                string msj = string.Empty;

                SearchCourseByLogbookNumber(logbook, ref msj);

                PopulateCoursePanel(null);

                SearchClassificationCourseByLogbookNumber(logbook, ref msj);

                PopulateCycleTrainingByCourse();

                SearchCycleTrainingByLogbookNumber(logbook, ref msj);

                PopulateTrainersByCourse();

                SearchTrainerByLogbookNumber(logbook, ref msj);

                dtpStartDate.Text = logbook.StartDateTime.Value.ToString("MM/dd/yyyy");
                tpcStarTime.Text = logbook.StartDateTime.Value.ToString("HH:mm");
                dtpEndDate.Text = logbook.EndDate.Value.ToString("MM/dd/yyyy");

                SearchClassroomByLogbookNumber(logbook, ref msj);
                SearchParticipantsByLogbookNumber(logbook);

                if (logbook.IsClosed.Value)
                {
                    txtCourseDuration.InnerText = string.Format("{0}", logbook.CourseDuration);
                    hdfCourseCostByParticipant.Value = logbook.CourseCostByParticipant.ToString();

                    hdfNoteRequired.Value = logbook.NoteRequired.ToString();
                }

                if (Session[sessionKeyStatusLogbookResults].Equals("Draft") || Session[sessionKeyStatusLogbookResults].Equals("Closed"))
                {
                    SetFormDisabled();
                }

                if (!string.IsNullOrWhiteSpace(msj))
                {
                    msj = string.Format("{0}<br/><br/>{1}", Convert.ToString(GetLocalResourceObject("msjEntitiesNotActive")), msj);

                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Advertencia, msj);
                }

                RegisterStartupScript(Page, "setTimeout(function () {{ ReturnFromSearchLogbookPostBack(); }}, 10);", true);
            }

            else
            {
                MensajeriaHelper.MostrarMensaje(Page,
                   TipoMensaje.Informacion,
                   Convert.ToString(GetLocalResourceObject("msjLogbookNotExist")));

                PrepareFormForNewLogbook();
            }
        }

        /// <summary>
        /// Save the logbook
        /// </summary>
        /// <param name="close">True if logbook must be closed. False is draft.</param>
        private void SaveLogbook(bool close)
        {
            int? logbookNumber = string.IsNullOrWhiteSpace(txtLogbookNumber.Text) ? (int?)null : Convert.ToInt32(txtLogbookNumber.Text);

            string[] trainerTokens = cboTrainer.SelectedValue.Split(new char[] { ',' }, 2);
            string selectedTrainerType = trainerTokens[0];
            string selectedTrainerCode = trainerTokens[1];

            LogbookEntity logbook = new LogbookEntity(logbookNumber,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                cboCourse.SelectedValue,
                cboClassroom.SelectedValue,
                HrisEnum.ParseEnumByName<TrainerType>(selectedTrainerType), selectedTrainerCode,
                DateTime.ParseExact(string.Format("{0} {1}", dtpStartDate.Text, tpcStarTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                DateTime.ParseExact(dtpEndDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                close,
                UserHelper.GetCurrentFullUserName,
                UserHelper.GetCurrentFullUserName,
                DateTime.Now)
            {
                ClassificationCourseId = cboClassificationCourseId.Value.ToInt32Null(),
                CourseDuration = Convert.ToDecimal(txtCourseDuration.InnerText),
                CourseCostByParticipant = Convert.ToDecimal(hdfCourseCostByParticipant.Value),
                CycleTrainingCode = cboCycleTranining.SelectedValue,
                CyclesRefreshment = bool.Parse(hdfCyclesRefreshment.Value),
                NoteRequired = bool.Parse(hdfNoteRequired.Value)
            };

            SaveAllGrades();

            if (close)
            {
                CloseGrades();

                rptParticipants.DataSource = LoadParticipants();
                rptParticipants.DataBind();
                uppParticipants.Update();
            }

            DataTable participants = LoadParticipants().Copy();
            participants.Columns.Remove("ParticipantName");
            participants.Columns.Remove("NominalClassType");
            participants.Columns.Remove("OriginalGrade");

            if (close && participants?.Rows.Count < 1)
            {
                hdfIsLogbookClosed.Value = "Error";

                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Validacion,
                    string.Format(Convert.ToString(GetLocalResourceObject("msjLogbookdontParticipants")), txtLogbookNumber.Text));

                return;
            }

            txtLogbookNumber.ReadOnly = true;
            btnSearchLogbook.Disabled = true;

            SaveLogbookData(logbook, participants);
        }

        /// <summary>
        /// Method that is responsible for inserting or modifying the records of a logbook
        /// </summary>
        /// <param name="logbook">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <param name="isNew">Variable that indicates if the logbook is new or edited</param>
        /// <param name="close">Variable that indicates if the logbook is closed</param>
        public void SaveLogbookData(LogbookEntity logbook, DataTable participants)
        {
            DbaEntity logbookNumberAssigned = ObjLogbooksBll.AddOrUpdate(logbook, participants);

            if (logbookNumberAssigned.ErrorMessage.Equals("Saved"))
            {
                //set the info
                txtLogbookNumber.Text = Convert.ToString(logbookNumberAssigned.ErrorNumber);

                LogbookStatus status = logbook.IsClosed.Value ? LogbookStatus.Closed : LogbookStatus.Draft;
                txtLogbookStatus.Text = GetLogbookStatusLocalizatedDescription(status);

                Session[sessionKeyStatusLogbookResults] = Convert.ToString(status);

                hdfCreatedBy.Value = logbook.CreatedBy;
                hdfIsLogbookClosed.Value = logbook.IsClosed.Value ? "true" : "false";

                RegisterStartupScript(Page, "setTimeout(function () {{ ReturnFromBtnSaveClickPostBack(); }}, 1000);", true);
                return;
            }

            if (logbookNumberAssigned.ErrorNumber.Equals(18))
            {
                hdfIsLogbookClosed.Value = "Error";

                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Informacion,
                    Convert.ToString(GetLocalResourceObject("msjTrainerTime")));

                return;
            }

            if (logbookNumberAssigned.ErrorNumber.Equals(19))
            {
                hdfIsLogbookClosed.Value = "Error";

                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Informacion,
                    Convert.ToString(GetLocalResourceObject("msjCourseTime")));

                return;
            }

            if (logbookNumberAssigned.ErrorNumber.Equals(20))
            {
                hdfIsLogbookClosed.Value = "Error";

                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Informacion,
                    Convert.ToString(GetLocalResourceObject("msjLogbookClosed")));

                return;
            }

            if (logbookNumberAssigned.ErrorNumber.Equals(21))
            {
                hdfIsLogbookClosed.Value = "Error";

                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Informacion,
                    Convert.ToString(GetLocalResourceObject("msjTrainerEndTime")));

                return;
            }
        }

        /// <summary>
        /// Delete the logbook
        /// </summary>        
        private void DeleteLogbook()
        {
            if (!string.IsNullOrWhiteSpace(txtLogbookNumber.Text))
            {
                RefreshTableParticipant();

                if (!UserHelper.GetCurrentFullUserName.Equals(hdfCreatedBy.Value, StringComparison.InvariantCultureIgnoreCase) &&
                    !ObjAdminUsersByModulesBll.IsUserAdmin(UserHelper.GetCurrentFullUserName, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, GeneralParameters.cTrainingModuleCode))
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Validacion,
                        Convert.ToString(GetLocalResourceObject("msjLogbookNotDeletableAccessDenied")));
                    return;
                }

                bool.TryParse(hdfIsLogbookClosed.Value, out bool isClosed);

                if (isClosed &&
                    !ObjAdminUsersByModulesBll.IsUserAdmin(UserHelper.GetCurrentFullUserName, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, GeneralParameters.cTrainingModuleCode))
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Validacion,
                        Convert.ToString(GetLocalResourceObject("msjLogbookNotDeletableClosed")));

                    return;
                }

                List<LogbooksFileEntity> logbooksFiles = LoadLogbookFilesList(txtLogbookNumber.Text);
                if (logbooksFiles.Count > 0)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Validacion,
                        Convert.ToString(GetLocalResourceObject("msjLogbookNotDeletableFile")));

                    return;
                }

                ObjLogbooksBll.Delete(
                    Convert.ToInt32(txtLogbookNumber.Text),
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                PrepareFormForNewLogbook();

                RegisterStartupScript(Page, "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
            }

            else
            {
                MensajeriaHelper.MostrarMensaje(Page,
                    TipoMensaje.Error,
                    GetLocalResourceObject("msgInvalidSelection").ToString());
            }
        }

        /// <summary>
        /// Load logbooks files from database
        /// </summary>
        /// <returns></returns>
        private List<LogbooksFileEntity> LoadLogbookFilesList(string selectedLogbookNumber)
        {
            List<LogbooksFileEntity> logbookFiles = ObjLogbooksFilesBll.LogbookFilesListByKey(new LogbooksFileEntity
            {
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                LogbookNumber = int.Parse(selectedLogbookNumber),
                DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
            });

            return logbookFiles;
        }

        /// <summary>
        /// Limpia los datos de session 
        /// </summary>
        private void LimpiarSession()
        {
            Session[sessionKeyCoursesResults] = null;
            Session[sessionKeyTrainersResults] = null;
            Session[sessionKeyCyleTrainingResults] = null;
            Session[sessionKeyTrainingCentersResults] = null;
            Session[sessionKeyClassroomsResults] = null;

            Session[sessionKeyEmployeesResults] = null;
            Session[sessionKeyInactiveEmployeesResults] = null;
            Session[sessionKeyAdvancedSearchResults] = null;

            Session[sessionKeyParticipantsResults] = null;
        }

        #region Classrooms

        /// <summary>
        /// Load empty data structure for classrooms
        /// </summary>
        /// <returns>Empty data structure for classrooms</returns>
        private DataTable LoadEmptyClassrooms()
        {
            DataTable classrooms = new DataTable();
            classrooms.Columns.Add("ClassroomCode", typeof(string));
            classrooms.Columns.Add("ClassroomDescription", typeof(string));
            classrooms.Columns.Add("Capacity", typeof(int));
            classrooms.Columns.Add("Comments", typeof(string));
            classrooms.Columns.Add("TrainingCenterCode", typeof(string));
            classrooms.Rows.Add("0", "", 0, "", "");

            return classrooms;
        }

        /// <summary>
        /// Load classrooms from database
        /// </summary>        
        private DataTable LoadClassrooms()
        {
            DataTable classrooms = Session[sessionKeyClassroomsResults] as DataTable;

            if (Session[sessionKeyClassroomsResults] == null)
            {
                classrooms = LoadEmptyClassrooms();

                List<ClassroomEntity> registeredClassrooms = ObjClassroomsBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredClassrooms.ForEach(x => classrooms.Rows.Add(
                    x.ClassroomCode, string.Format("{0} - {1}", x.ClassroomCode, x.ClassroomDescription), x.Capacity, x.Comments, x.TrainingCenter.TrainingCenterCode));

                Session[sessionKeyClassroomsResults] = classrooms;
            }

            return classrooms;
        }

        /// <summary>
        /// Prepare the classrooms for logbook
        /// </summary>
        private void PrepareClassroomsForLogbook()
        {
            DataTable classrooms = LoadClassrooms();

            cboClassroom.Enabled = true;
            cboClassroom.DataValueField = "ClassroomCode";
            cboClassroom.DataTextField = "ClassroomDescription";
            cboClassroom.DataSource = classrooms.DefaultView;
            cboClassroom.DataBind();
        }

        /// <summary>
        /// Prepare the classrooms for logbook
        /// </summary>
        private void PrepareClassroomsForLogbook(ref string msj)
        {
            string selectedPreviousClassroom = cboClassroom.SelectedValue;

            DataTable classrooms = LoadClassrooms();

            DataRowView selectedClassroom = classrooms.DefaultView.Cast<DataRowView>().FirstOrDefault(r => Convert.ToString(r["ClassroomCode"]) == selectedPreviousClassroom);
            if (selectedClassroom == null)
            {
                msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjClassroomNotActive")));
            }

            else
            {
                cboClassroom.SelectedValue = selectedPreviousClassroom;
            }

            PopulateClassroomPanel(null);
        }

        /// <summary>
        /// Populate classroom panel
        /// </summary>
        /// <param name="sender">UI object identifying the request object</param>
        private void PopulateClassroomPanel(Control sender)
        {
            if (Session[sessionKeyClassroomsResults] != null)
            {
                DataTable classrooms = (DataTable)Session[sessionKeyClassroomsResults];
                DataRow selectedRow = classrooms.AsEnumerable().FirstOrDefault(r => r.Field<string>("ClassroomCode") == cboClassroom.SelectedValue);
                if (selectedRow != null)
                {
                    txtClassroomCapacity.InnerText = string.Format("{0}", selectedRow.Field<int>("Capacity"));
                    txtClassroomComments.InnerText = selectedRow.Field<string>("Comments");
                }

                else
                {
                    txtClassroomCapacity.InnerText = "0";
                    txtClassroomComments.InnerText = string.Empty;
                }

                RegisterStartupScript(sender, "ReturnFromClassroomChangedPostBack");
            }
        }

        /// <summary>
        /// Search classroom by logbook number
        /// </summary>
        private void SearchClassroomByLogbookNumber(LogbookEntity logbook, ref string msj)
        {
            ClassroomEntity classroom = ObjClassroomsBll.ListByKey(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                logbook.ClassroomCode, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

            if (classroom != null)
            {
                SearchTrainingCenterByLogbookNumber(classroom, ref msj);

                DataTable classrooms = LoadClassrooms();
                DataRow selectedClassroom = classrooms.AsEnumerable().FirstOrDefault(r => r.Field<string>("ClassroomCode") == logbook.ClassroomCode);

                if (selectedClassroom == null)
                {
                    msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjClassroomNotActive")));

                    classrooms.Rows.Add(classroom.ClassroomCode, classroom.ClassroomDescription, classroom.Capacity, classroom.Comments, classroom.TrainingCenter.TrainingCenterCode);
                    Session[sessionKeyClassroomsResults] = classrooms;
                }

                LoadClassroomsBySelectedTrainingCenter();
                cboClassroom.SelectedValue = logbook.ClassroomCode;

                PopulateClassroomPanel(null);
            }

            else
            {
                cboTrainingCenter.Items.Clear();
                cboClassroom.Items.Clear();

                txtClassroomCapacity.InnerText = "0";
                txtClassroomComments.InnerText = string.Empty;
            }
        }

        #endregion

        #region Classification Course

        /// <summary>
        /// Load classification course from database
        /// </summary>        
        private List<ClassificationCourseEntity> LoadClassificationCourse()
        {
            List<ClassificationCourseEntity> classificationCourse = ObjClasificacionCourse.ClassificationCourseListGet(GetLocalResourceObject("Lang").ToString());
            Session[sessionKeyClassificationCourseResults] = classificationCourse;

            return classificationCourse;
        }

        /// <summary>
        /// Prepare the classification of courses for logbook
        /// </summary>
        private void PrepareClassificationCoursesForLogbook()
        {
            cboClassificationCourseId.Items.Clear();

            List<ClassificationCourseEntity> classificationCourses = LoadClassificationCourse();
            cboClassificationCourseId.Items.AddRange(classificationCourses.Select(R => new ListItem()
            {
                Text = R.ClassificationCourseDesEs,
                Value = R.ClassificationCourseId.ToString()
            }).ToArray());
        }

        /// <summary>
        /// Search classification course by logbook number
        /// </summary>
        private void SearchClassificationCourseByLogbookNumber(LogbookEntity logbook, ref string msj)
        {
            List<ClassificationCourseEntity> classificationCourses = LoadClassificationCourse();
            ClassificationCourseEntity selectedClassificationCourse = classificationCourses.FirstOrDefault(r => r.ClassificationCourseId == logbook.ClassificationCourseId);

            if (selectedClassificationCourse == null)
            {
                if (!hdfTypeLogbook.Value.Equals("H"))
                {
                    msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjClassificationNotActive")));
                }

                if (logbook.ClassificationCourseId != null)
                {
                    string lang = GetLocalResourceObject("Lang").ToString();
                    if (lang.Equals("ES"))
                    {
                        cboClassificationCourseId.Items.Add(new ListItem(logbook.ClassificationCourseDesEs, logbook.ClassificationCourseId.ToString()));
                    }

                    else
                    {
                        cboClassificationCourseId.Items.Add(new ListItem(logbook.ClassificationCourseDesEn, logbook.ClassificationCourseId.ToString()));
                    }
                }

                cboClassificationCourseId.Value = Convert.ToString(logbook.ClassificationCourseId);
            }

            else
            {
                cboClassificationCourseId.Value = Convert.ToString(logbook.ClassificationCourseId);
            }
        }

        #endregion

        #region Courses

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyCourses()
        {
            DataTable courses = new DataTable();
            courses.Columns.Add("CourseCode", typeof(string));
            courses.Columns.Add("CourseName", typeof(string));
            courses.Columns.Add("CourseDuration", typeof(decimal));
            courses.Columns.Add("CourseCostByParticipant", typeof(decimal));
            courses.Columns.Add("NoteRequired", typeof(bool));
            courses.Columns.Add("CyclesRefreshment", typeof(bool));

            courses.Rows.Add("0", "", "0", false, false, false);

            return courses;
        }

        /// <summary>
        /// Load courses from database
        /// </summary>        
        private DataTable LoadCourses()
        {
            DataTable courses = Session[sessionKeyCoursesResults] as DataTable;

            if (Session[sessionKeyCoursesResults] == null)
            {
                courses = LoadEmptyCourses();

                List<CourseEntity> registeredCourses = ObjCoursesBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredCourses.ForEach(x => courses.Rows.Add(
                    x.CourseCode, string.Format("{0} - {1}", x.CourseCode, x.CourseName), x.CourseDuration, x.CourseCostByParticipant, x.NoteRequired, x.CyclesRefreshment));

                Session[sessionKeyCoursesResults] = courses;
            }

            return courses;
        }

        /// <summary>
        /// Prepare the courses for logbook
        /// </summary>
        private void PrepareCoursesForLogbook()
        {
            DataTable courses = LoadCourses();

            cboCourse.Enabled = true;
            cboCourse.DataValueField = "CourseCode";
            cboCourse.DataTextField = "CourseName";
            cboCourse.DataSource = courses;
            cboCourse.DataBind();

            cboCourse.SelectedIndex = 0;
        }

        /// <summary>
        /// Prepare the courses for logbook
        /// </summary>
        private void PrepareCoursesForLogbook(ref string msj)
        {
            string selectedPreviousCourse = cboCourse.SelectedValue;

            PrepareCoursesForLogbook();
            DataTable courses = Session[sessionKeyCoursesResults] as DataTable;

            DataRow selectedCourse = courses.AsEnumerable().FirstOrDefault(r => r.Field<string>("CourseCode") == selectedPreviousCourse);
            if (selectedCourse == null)
            {
                msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjCourseNotActive")));
            }

            else
            {
                cboCourse.SelectedValue = selectedPreviousCourse;
            }

            PopulateCoursePanel(null);
        }

        /// <summary>
        /// Populate course panel
        /// </summary>
        /// <param name="sender">UI object identifying the request object</param>
        private void PopulateCoursePanel(Control sender)
        {
            if (Session[sessionKeyCoursesResults] != null)
            {
                DataTable courses = Session[sessionKeyCoursesResults] as DataTable;
                DataRow selectedRow = courses.AsEnumerable().FirstOrDefault(r => r.Field<string>("CourseCode") == cboCourse.SelectedValue);

                if (selectedRow != null)
                {
                    txtCourseCode.InnerText = selectedRow.Field<string>("CourseCode");
                    txtCourseName.InnerText = selectedRow.Field<string>("CourseName").Split('-')?.LastOrDefault();
                    txtCourseDuration.InnerText = string.Format("{0}", selectedRow.Field<decimal?>("CourseDuration"));
                    hdfCourseCostByParticipant.Value = selectedRow.Field<decimal?>("CourseCostByParticipant").ToString();
                    hdfNoteRequired.Value = selectedRow.Field<bool?>("NoteRequired").ToString();
                    hdfCyclesRefreshment.Value = selectedRow.Field<bool?>("CyclesRefreshment").ToString();
                }

                else
                {
                    txtCourseCode.InnerText = string.Empty;
                    txtCourseName.InnerText = string.Empty;
                    txtCourseDuration.InnerText = "0";

                    hdfCourseCostByParticipant.Value = "False";
                    hdfNoteRequired.Value = "False";
                    hdfCyclesRefreshment.Value = "False";
                }

                RegisterStartupScript(sender, "ReturnFromCourseChangedPostBack");
            }
        }

        /// <summary>
        /// Search course by logbook number
        /// </summary>
        private void SearchCourseByLogbookNumber(LogbookEntity logbook, ref string msj)
        {
            DataTable courses = LoadCourses();
            DataRow selectedCourse = courses.AsEnumerable().FirstOrDefault(r => r.Field<string>("CourseCode") == logbook.CourseCode);

            if (selectedCourse == null)
            {
                if (!hdfTypeLogbook.Value.Equals("H"))
                {
                    msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjCourseNotActive")));
                }

                CourseEntity course = ObjCoursesBll.ListByKey(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    logbook.CourseCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                if (course != null)
                {
                    courses.Rows.Add(course.CourseCode, course.CourseCode + " - " + course.CourseName, course.CourseDuration, course.CourseCostByParticipant, course.NoteRequired, course.CyclesRefreshment);
                    Session[sessionKeyCoursesResults] = courses;
                }
            }

            cboCourse.DataSource = courses;
            cboCourse.DataBind();

            cboCourse.SelectedValue = logbook.CourseCode;
        }

        #endregion

        #region CycleTraining

        /// <summary>
        /// Load empty data structure for cycle training
        /// </summary>
        /// <returns>Empty data structure for cycle training</returns>
        private DataTable LoadEmptyCycleTraining()
        {
            DataTable cycleTraining = new DataTable();
            cycleTraining.Columns.Add("CourseCode", typeof(string));
            cycleTraining.Columns.Add("CycleTrainingCode", typeof(string));
            cycleTraining.Columns.Add("CycleTrainingName", typeof(string));
            cycleTraining.Columns.Add("CycleTrainingStartDate", typeof(string));
            cycleTraining.Columns.Add("CycleTrainingEndDate", typeof(string));

            cycleTraining.Rows.Add("", "", "", "", "");

            return cycleTraining;
        }

        /// <summary>
        /// Load cycle training from database
        /// </summary>        
        private DataTable LoadCycleTraining()
        {
            DataTable cycleTraining = Session[sessionKeyCyleTrainingResults] as DataTable;

            if (Session[sessionKeyCyleTrainingResults] == null)
            {
                cycleTraining = LoadEmptyCycleTraining();

                List<CycleTrainingEntity> registeredCycleTraining = ObjCycleTrainingBll.CycleTrainingListByByMasterProgramByCourse(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredCycleTraining.ForEach(x => cycleTraining.Rows.Add(
                    x.CourseCode, x.CycleTrainingCode, string.Format("{0} - {1}", x.CycleTrainingCode, x.CycleTrainingName), string.Format("{0:dd/MM/yyyy}", x.CycleTrainingStartDate), string.Format("{0:dd/MM/yyyy}", x.CycleTrainingEndDate)));

                Session[sessionKeyCyleTrainingResults] = cycleTraining;
            }

            return cycleTraining;
        }

        /// <summary>
        /// Prepare the cycle training for logbook
        /// </summary>
        private void PrepareCycleTrainingForLogbook()
        {
            DataTable cycleTraining = LoadCycleTraining();

            cboCycleTranining.Enabled = true;
            cboCycleTranining.DataValueField = "CycleTrainingCode";
            cboCycleTranining.DataTextField = "CycleTrainingName";
            cboCycleTranining.DataSource = cycleTraining;
            cboCycleTranining.DataBind();
            cboCycleTranining.SelectedIndex = 0;

            PopulateCycleTrainingByCourse();
        }

        /// <summary>
        /// Populate cycle Training panel
        /// </summary>
        /// <param name="sender">UI object identifying the request object</param>
        private void PopulateCycleTrainingPanel(Control sender)
        {
            if (Session[sessionKeyCyleTrainingResults] != null)
            {
                DataTable cycleTraining = Session[sessionKeyCyleTrainingResults] as DataTable;
                DataRow selectedRow = cycleTraining.AsEnumerable().FirstOrDefault(r => r.Field<string>("CycleTrainingCode") == cboCycleTranining.SelectedValue);

                if (selectedRow != null)
                {
                    txtCycleTrainingName.InnerText = selectedRow.Field<string>("CycleTrainingName");
                    txtCycleTrainingStartDate.InnerText = selectedRow.Field<string>("CycleTrainingStartDate");
                    txtCycleTrainingEndDate.InnerText = selectedRow.Field<string>("CycleTrainingEndDate");
                }

                else
                {
                    txtCycleTrainingName.InnerText = string.Empty;
                    txtCycleTrainingStartDate.InnerText = string.Empty;
                    txtCycleTrainingEndDate.InnerText = string.Empty;
                }
            }
        }

        /// <summary>
        /// Search cycle training by logbook number
        /// </summary>
        private void SearchCycleTrainingByLogbookNumber(LogbookEntity logbook, ref string msj)
        {
            DataTable cyclesTraining = LoadCycleTraining();
            DataRow selectedCyclesTraining = cyclesTraining.AsEnumerable().FirstOrDefault(r => r.Field<string>("CycleTrainingCode") == logbook.CycleTrainingCode);

            if (selectedCyclesTraining == null && logbook.CyclesRefreshment)
            {
                msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjCycleTrainingCodeNotActive")));
            }

            cycleTranining.Visible = hdfCyclesRefreshment.Value.Equals(bool.TrueString);
            panelCycleTranining.Visible = hdfCyclesRefreshment.Value.Equals(bool.TrueString);

            cboCycleTranining.SelectedValue = logbook.CycleTrainingCode;

            PopulateCycleTrainingPanel(null);
        }

        #endregion

        #region Employees

        /// <summary>
        /// Load empty data structure for employees
        /// </summary>
        /// <returns>Empty data structure for employees</returns>
        private DataTable LoadEmptyEmployees()
        {
            DataTable employees = new DataTable();
            employees.Columns.Add("EmployeeCode", typeof(string));
            employees.Columns.Add("EmployeeName", typeof(string));
            employees.Columns.Add("CostCenter", typeof(string));
            employees.Columns.Add("NominalClassId", typeof(string));

            return employees;
        }

        /// <summary>
        /// Load employees from database
        /// </summary>        
        private DataTable LoadEmployees()
        {
            DataTable employees = Session[sessionKeyEmployeesResults] as DataTable;

            if (Session[sessionKeyEmployeesResults] == null)
            {
                employees = LoadEmptyEmployees();

                List<EmployeeEntity> registeredEmployees = ObjEmployeesBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredEmployees.ForEach(x => employees.Rows.Add(x.EmployeeCode, x.EmployeeName, x.CostCenter, x.NominalClassId));

                Session[sessionKeyEmployeesResults] = employees;
            }

            return employees;
        }

        /// <summary>
        /// Load inactive employees from database
        /// </summary>        
        private DataTable LoadInactiveEmployees()
        {
            DataTable employees = Session[sessionKeyInactiveEmployeesResults] as DataTable;

            if (Session[sessionKeyInactiveEmployeesResults] == null)
            {
                employees = LoadEmptyEmployees();

                List<EmployeeEntity> registeredEmployees = ObjEmployeesBll.ListByInactiveDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredEmployees.ForEach(x => employees.Rows.Add(x.EmployeeCode, x.EmployeeName, x.CostCenter, x.NominalClassId));

                Session[sessionKeyInactiveEmployeesResults] = employees;
            }

            return employees;
        }

        /// <summary>
        /// Prepare the participants for logbook
        /// </summary>
        private void PrepareEmployeesForLogbook()
        {
            txtSearchEmployees.Text = string.Empty;
            rptEmployees.DataSource = LoadEmptyEmployees();
            rptEmployees.DataBind();
            uppEmployees.Update();

            lblSearchEmployeesResults.InnerHtml = Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults"));
            lblSearchParticipantsResults.InnerHtml = Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults"));
            lblAdministratives.InnerHtml = string.Format("{0} : 0", Convert.ToString(GetLocalResourceObject("lblAdministratives")));
            lblOperatives.InnerHtml = string.Format("{0} : 0", Convert.ToString(GetLocalResourceObject("lblOperatives")));
        }

        /// <summary>
        /// Searched employees by type employee
        /// </summary>
        private DataTable SearchedEmployeeByTypeEmployee()
        {
            DataTable existentEmployees = null;
            if (hdfTypeEmployeesSearch.Value.Equals("I"))
            {
                existentEmployees = LoadInactiveEmployees();
            }

            else if (hdfTypeEmployeesSearch.Value.Equals("A"))
            {
                existentEmployees = LoadEmployees();
            }

            return existentEmployees;
        }

        /// <summary>
        /// Refresh the employee tables by rebinding data
        /// </summary>
        private void RefreshAdvancedSearch()
        {
            if (Session[sessionKeyAdvancedSearchResults] != null)
            {
                grvAdvancedSearchEmployees.DataSource = Session[sessionKeyAdvancedSearchResults];
                grvAdvancedSearchEmployees.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched employees by search tokens
        /// </summary>
        private void FilterSearchedEmployees()
        {
            DataTable employees = SearchedEmployeeByTypeEmployee().Copy();
            DataTable participants = LoadParticipants();

            participants.AsEnumerable().ToList().ForEach(pt =>
            {
                employees.AsEnumerable().Where(e =>
                    string.Equals(pt.Field<string>("ParticipantCode"), e.Field<string>("EmployeeCode"))).ToList().ForEach(e => employees.Rows.Remove(e));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchEmployees.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchEmployees.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                List<string> filters = new List<string>();
                foreach (string term in terms)
                {
                    // Escapar caracteres especiales
                    string escapedTerm = term.Replace("[", "[[]").Replace("]", "[]]").Replace("%", "[%]").Replace("_", "[_]").Replace("'", "''");
                    filters.Add($"(EmployeeCode LIKE '%{escapedTerm}%' OR EmployeeName LIKE '%{escapedTerm}%')");
                }

                string filter = string.Join(" AND ", filters);

                employees.DefaultView.RowFilter = filter;
                DataTable finalTopResults = LoadEmptyEmployees();

                int minRowCount = Math.Min(10, employees.DefaultView.Count);

                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(employees.DefaultView[i].Row);
                }

                if (!string.IsNullOrWhiteSpace(cboTrainer.SelectedValue))
                {
                    if (cboTrainer.SelectedValue != "0")
                    {
                        string[] trainerId = cboTrainer.SelectedValue.Split(',');
                        string expression = $"EmployeeCode = '{trainerId[1]}'";

                        DataRow[] trainerEmployeeDataRows = finalTopResults.Select(expression);

                        if (trainerEmployeeDataRows.Length > 0)
                        {
                            finalTopResults.Rows.Remove(trainerEmployeeDataRows[0]);
                        }
                    }
                }

                rptEmployees.DataSource = finalTopResults;
                rptEmployees.DataBind();

                uppEmployees.Update();

                lblSearchEmployeesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResultsCount")), minRowCount, employees.DefaultView.Count));
            }
            else
            {
                employees.DefaultView.RowFilter = "1 = 0";
                rptEmployees.DataSource = null;
                rptEmployees.DataBind();
                uppEmployees.Update();

                lblSearchEmployeesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResultsCount")), 0, 0));
            }

            RegisterStartupScript(uppEmployees, "setTimeout(function () { ReturnFromSearchEmployeesPostBack(); }, 200);", true);
        }


        #endregion

        #region Grades

        /// <summary>
        /// Close the grades and determines if is approved or not.
        /// </summary>
        private void CloseGrades()
        {
            if (Session[sessionKeyParticipantsResults] != null)
            {
                DataTable participants = LoadParticipants();
                int minGrade = Convert.ToInt32(hdfMinGrade.Value);

             participants.AsEnumerable()
            .Where(w => w.Field<bool>("IsPresent"))
            .ToList()
            .ForEach(p =>
                {
                    int? grade = p.Field<int?>("Grade");
                    p.SetField("Approved", grade.HasValue && grade.Value >= minGrade);
                });
                Session[sessionKeyParticipantsResults] = participants;
            }
        }

        /// <summary>
        /// Loads the min grade from database
        /// </summary>
        private void LoadMinGrade()
        {
            //this is the default value
            int minGradeValue = 70;

            try
            {
                string minGrade = ConfigurationManager.AppSettings["MinGradeOfApprovalForLogbooks"].ToString();

                if (minGrade != null && !string.IsNullOrWhiteSpace(minGrade))
                {
                    int.TryParse(minGrade, out minGradeValue);
                }
            }

            catch
            {
                minGradeValue = 70;
            }

            hdfMinGrade.Value = Convert.ToString(minGradeValue);
        }

        /// <summary>
        /// Save all grades
        /// </summary>
        private void SaveAllGrades()
        {
            if (Session[sessionKeyParticipantsResults] != null)
            {
                DataTable participants = LoadParticipants();

                foreach (RepeaterItem item in rptParticipants.Items)
                {
                    HiddenField hdfParticipantCode = (HiddenField)item.FindControl("hdfParticipantCode");

                    CheckBox chkIsPresent = (CheckBox)item.FindControl("chkIsPresent");

                    TextBox txtGrade = (TextBox)item.FindControl("txtGrade");

                    if (hdfParticipantCode != null)
                    {
                        string participantCode = hdfParticipantCode.Value;
                        DataRow selectedRow = participants.AsEnumerable().FirstOrDefault(r => r.Field<string>("ParticipantCode") == participantCode);

                        if (selectedRow != null)
                        {
                            selectedRow.SetField("IsPresent", chkIsPresent.Checked);
                            selectedRow.SetField("Grade", VerifyGrades(txtGrade.Text, chkIsPresent.Checked));
                            selectedRow.SetField("OriginalGrade", VerifyGrades(txtGrade.Text, chkIsPresent.Checked));

                            RegisterStartupScript(this, string.Format("ReturnFromSaveGradePostBack('{0}');", txtGrade.ClientID), true);
                        }
                    }
                }

                rptParticipants.DataSource = participants;
                rptParticipants.DataBind();
                uppParticipants.Update();
            }
        }

        /// <summary>
        /// Verify all gradess
        /// </summary>
        private int? VerifyGrades(string txtGrade, bool isPresent)
        {
            const int DefaultGrade = 100;
            const int AbsenceGrade = 0;

            int? grade = DefaultGrade;

            // Asegura que txtGrade no es nulo o vacío
            txtGrade = !string.IsNullOrEmpty(txtGrade) ? txtGrade : "0";

            // Convierte a minúsculas el valor de hdfNoteRequired una vez
            string noteRequired = hdfNoteRequired.Value.ToLower();

            // Verifica si se requiere una nota
            if (noteRequired.Equals("true"))
            {
                if (isPresent)
                {
                    try
                    {
                        if (txtGrade!=null)
                            grade = Convert.ToInt32(txtGrade);
                        else
                            grade = DefaultGrade;
                    }
                    catch (FormatException)
                    {
                        // Maneja el error si txtGrade no es un número válido
                        grade = null;
                    }
                }
                else
                {
                    grade = AbsenceGrade;
                }
            }
            else if (noteRequired.Equals("false"))
            {
                grade = isPresent ? DefaultGrade : AbsenceGrade;
            }
            

            return grade;
        }


        #endregion

        #region Trainers

        /// <summary>
        /// Load empty data structure for Trainers
        /// </summary>
        /// <returns>Empty data structure for Trainers</returns>
        private DataTable LoadEmptyTrainers()
        {
            DataTable trainers = new DataTable();
            trainers.Columns.Add("TrainerPK", typeof(string));
            trainers.Columns.Add("TrainerDisplayName", typeof(string));
            trainers.Columns.Add("TrainerCode", typeof(string));
            trainers.Columns.Add("TrainerType", typeof(string));
            trainers.Columns.Add("TrainerName", typeof(string));

            trainers.Rows.Add("0", "", "0", "", "");
            return trainers;
        }

        /// <summary>
        /// Load Trainers from database
        /// </summary>        
        private DataTable LoadTrainers()
        {
            DataTable trainers = Session[sessionKeyTrainersResults] as DataTable;

            trainers = LoadEmptyTrainers();

            List<TrainerEntity> registeredTrainers = ObjTrainersBll.ListByDivision(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

            registeredTrainers.ForEach(x => trainers.Rows.Add(
                string.Format("{0},{1}", x.TrainerType, x.TrainerCode), string.Format("{0}-{1}", x.TrainerCode, x.TrainerName), x.TrainerCode, x.TrainerType, x.TrainerName));

            Session[sessionKeyTrainersResults] = trainers;

            return trainers;
        }

        /// <summary>
        /// Prepare the trainers for logbook
        /// </summary>
        private void PrepareTrainersForLogbook()
        {
            DataTable trainers = LoadTrainers();

            cboTrainer.Enabled = true;
            cboTrainer.DataValueField = "TrainerPK";
            cboTrainer.DataTextField = "TrainerDisplayName";
            cboTrainer.DataSource = trainers;
            cboTrainer.DataBind();
            cboTrainer.SelectedIndex = 0;

            PopulateTrainersByCourse();
        }

        /// <summary>
        /// Prepare the trainers for logbook
        /// </summary>
        private void PrepareTrainersForLogbook(ref string msj)
        {
            string selectedPreviousTrainer = cboTrainer.SelectedValue;

            PrepareTrainersForLogbook();
            DataTable trainers = Session[sessionKeyTrainersResults] as DataTable;

            DataRow selectedTrainer = trainers.AsEnumerable().FirstOrDefault(r => r.Field<string>("TrainerPK").Trim().ToLower() == selectedPreviousTrainer?.Trim().ToLower());
            if (selectedTrainer == null)
            {
                msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjTrainerNotActive")));
            }

            else
            {
                cboTrainer.SelectedValue = selectedPreviousTrainer;
            }
        }

        /// <summary>
        /// Search trainer by logbook number
        /// </summary>
        private void SearchTrainerByLogbookNumber(LogbookEntity logbook, ref string msj)
        {
            DataTable trainers = LoadTrainers();
            DataRow selectedTrainer = trainers.AsEnumerable().FirstOrDefault(r => r.Field<string>("TrainerPK") == string.Format("{0},{1}", logbook.TrainerType, logbook.TrainerCode));

            if (selectedTrainer == null)
            {
                if (!hdfTypeLogbook.Value.Equals("H"))
                {
                    msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjTrainerNotActive")));
                }

                TrainerEntity trainer = ObjTrainersBll.ListByKey(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    logbook.TrainerType,
                    logbook.TrainerCode);

                if (trainer != null)
                {
                    trainers.Rows.Add(string.Format("{0},{1}", trainer.TrainerType, trainer.TrainerCode), string.Format("{0}-{1}", trainer.TrainerCode, trainer.TrainerName), trainer.TrainerCode, trainer.TrainerType, trainer.TrainerName);
                    Session[sessionKeyTrainersResults] = trainers;

                    txtTrainer.InnerText = string.Format(" {0} ", trainer.TrainerName);
                }
            }

            else
            {
                txtTrainer.InnerText = string.Format(" {0} ", selectedTrainer[4].ToString());
            }

            cboTrainer.DataSource = trainers;
            cboTrainer.DataBind();
            cboTrainer.SelectedValue = string.Format("{0},{1}", logbook.TrainerType, logbook.TrainerCode);
        }

        #endregion

        #region Training Centers

        /// <summary>
        /// Load empty data structure for training centers
        /// </summary>
        /// <returns>Empty data structure for Training centers</returns>
        private DataTable LoadEmptyTrainingCenters()
        {
            DataTable trainingCenters = new DataTable();
            trainingCenters.Columns.Add("TrainingCenterCode", typeof(string));
            trainingCenters.Columns.Add("TrainingCenterDescription", typeof(string));
            trainingCenters.Rows.Add("0", "");

            return trainingCenters;
        }

        /// <summary>
        /// Load training centers from database
        /// </summary>        
        private DataTable LoadTrainingCenters()
        {
            DataTable trainingCenters = Session[sessionKeyTrainingCentersResults] as DataTable;

            if (Session[sessionKeyTrainingCentersResults] == null)
            {
                trainingCenters = LoadEmptyTrainingCenters();

                List<TrainingCenterEntity> registeredTrainingCenters = ObjTrainingCentersBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainingCenters.ForEach(x => trainingCenters.Rows.Add(
                    x.TrainingCenterCode, string.Format("{0} - {1}", x.TrainingCenterCode, x.TrainingCenterDescription)));

                Session[sessionKeyTrainingCentersResults] = trainingCenters;
            }

            return trainingCenters;
        }

        /// <summary>
        /// Prepare the training center for logbook
        /// </summary>
        private void PrepareTrainingCenterForLogbook()
        {
            DataTable trainingCenters = LoadTrainingCenters();

            cboTrainingCenter.Enabled = true;
            cboTrainingCenter.DataValueField = "TrainingCenterCode";
            cboTrainingCenter.DataTextField = "TrainingCenterDescription";
            cboTrainingCenter.DataSource = trainingCenters;
            cboTrainingCenter.DataBind();
        }

        /// <summary>
        /// Prepare the training center for logbook
        /// </summary>
        private void PrepareTrainingCenterForLogbook(ref string msj)
        {
            string selectedPreviousTrainingCenter = cboTrainingCenter.SelectedValue;

            PrepareTrainingCenterForLogbook();
            DataTable trainingCenters = Session[sessionKeyTrainingCentersResults] as DataTable;

            DataRow selectedTrainingCenter = trainingCenters.AsEnumerable().FirstOrDefault(r => r.Field<string>("TrainingCenterCode") == selectedPreviousTrainingCenter);
            if (selectedTrainingCenter == null)
            {
                msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjTrainingCenterNotActive")));
            }

            else
            {
                cboTrainingCenter.SelectedValue = selectedPreviousTrainingCenter;
            }
        }

        /// <summary>
        /// Search training center by logbook number
        /// </summary>
        private void SearchTrainingCenterByLogbookNumber(ClassroomEntity classroom, ref string msj)
        {
            DataTable trainingCenters = LoadTrainingCenters();
            DataRow selectedTrainingCenter = trainingCenters.AsEnumerable().FirstOrDefault(r => r.Field<string>("TrainingCenterCode") == classroom.TrainingCenter.TrainingCenterCode);

            if (selectedTrainingCenter == null)
            {
                msj = string.Format("{0}<br/>- {1}", msj, Convert.ToString(GetLocalResourceObject("msjTrainingCenterNotActive")));

                TrainingCenterEntity trainingCenter = ObjTrainingCentersBll.ListByCode(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    classroom.TrainingCenter.TrainingCenterCode);

                if (trainingCenter != null)
                {
                    trainingCenters.Rows.Add(trainingCenter.TrainingCenterCode, trainingCenter.TrainingCenterDescription);
                    Session[sessionKeyTrainingCentersResults] = trainingCenters;
                }
            }

            cboTrainingCenter.DataSource = trainingCenters;
            cboTrainingCenter.DataBind();
            cboTrainingCenter.SelectedValue = classroom.TrainingCenter.TrainingCenterCode;
        }

        #endregion

        #region Participants

        /// <summary>
        /// Load empty data structure for participants
        /// </summary>
        /// <returns>Empty data structure for participants</returns>
        private DataTable LoadEmptyParticipants()
        {
            DataTable participants = new DataTable();
            participants.Columns.Add("ParticipantCode", typeof(string));
            participants.Columns.Add("ParticipantName", typeof(string));
            participants.Columns.Add("CostCenter", typeof(string));
            participants.Columns.Add("NominalClassId", typeof(string));
            participants.Columns.Add("NominalClassType", typeof(string));
            participants.Columns.Add("IsPresent", typeof(bool));

            participants.Columns.Add("Grade", typeof(int));
            participants.Columns["Grade"].AllowDBNull = true;

            participants.Columns.Add("OriginalGrade", typeof(int));
            participants.Columns["OriginalGrade"].AllowDBNull = true;

            participants.Columns.Add("Approved", typeof(bool));
            participants.Columns["Approved"].AllowDBNull = true;

            return participants;
        }

        /// <summary>
        /// Load empty for participants
        /// </summary>
        /// <returns></returns>
        private DataTable LoadEmptyParticpants()
        {
            DataTable particpants = new DataTable();
            particpants.Columns.Add("Id", typeof(string));
            particpants.Columns.Add("Code", typeof(string));
            particpants.Columns.Add("KeyValue1", typeof(string));
            particpants.Columns.Add("Selected", typeof(int));

            particpants.Rows.Add("0", "", "0");

            return particpants;
        }

        /// <summary>
        /// Load empty data structure for participants
        /// </summary>
        /// <returns>Empty data structure for participants</returns>
        private DataTable LoadParticipants()
        {
            DataTable participants = Session[sessionKeyParticipantsResults] as DataTable;

            if (Session[sessionKeyParticipantsResults] == null)
            {
                participants = LoadEmptyParticipants();
                Session[sessionKeyParticipantsResults] = participants;
            }

            return participants;
        }

        /// <summary>
        /// Refresh the participants
        /// </summary>   
        private void RefreshTableParticipant()
        {
            DataTable participants = LoadParticipants();
            rptParticipants.DataSource = participants;
            rptParticipants.DataBind();
            uppParticipants.Update();
        }
        /// <summary>
        /// Prepare the participants for logbook
        /// </summary>
        private void PrepareParticipantsForLogbook()
        {
            DataTable participants = LoadParticipants();

            participants.AsEnumerable().ToList().ForEach(r => { r.SetField("Grade", 0); r.SetField("OriginalGrade", 0); r.SetField<bool?>("Approved", null); });

            rptParticipants.DataSource = participants;
            rptParticipants.DataBind();
            uppParticipants.Update();
        }

        /// <summary>
        /// Add participant to logbook
        /// </summary>
        /// <param name="sender">UI object identifying the person to add</param>
        private void AddParticipant(object sender)
        {
            HtmlButton btnParticipant = (HtmlButton)sender;
            string employeeCode = ((HiddenField)btnParticipant.Parent.FindControl("hdfEmployeeCode")).Value;
            string employeeName = ((HiddenField)btnParticipant.Parent.FindControl("hdfEmployeeName")).Value;
            string employeeCostCenter = ((HiddenField)btnParticipant.Parent.FindControl("hdfEmployeeCostCenter")).Value;
            string employeeNominalClass = ((HiddenField)btnParticipant.Parent.FindControl("hdfEmployeeNominalClass")).Value;

            DataTable participants = LoadParticipants();

            int capacity = int.Parse(txtClassroomCapacity.InnerText);
            if (capacity == 0 || participants.Rows.Count > capacity)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(employeeCode) && string.IsNullOrWhiteSpace(employeeName))
            {
                return;
            }

            if (!participants.AsEnumerable().ToList().Exists(pt => string.Equals(pt.Field<string>("ParticipantCode"), employeeCode)))
            {
                SaveParticipantsState();

                participants.Rows.Add(employeeCode, employeeName, employeeCostCenter, employeeNominalClass, GetNominalClassByParticipant(employeeNominalClass), false, null, null, null);

                rptParticipants.DataSource = participants;
                rptParticipants.DataBind();
                uppParticipants.Update();
            }

            lblSearchParticipantsResults.InnerHtml = string.Format("{0} {1}",
                Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults")), participants.DefaultView.Count);

            UpdateCountNominalClass();

            RegisterStartupScript(btnParticipant, "ReturnFromAddParticipantPostBack");
        }

        /// <summary>
        /// Refresh the participant tables by rebinding data
        /// </summary>
        private void RefreshParticipants()
        {
            if (Session[sessionKeyParticipantsResults] != null)
            {
                rptParticipants.DataSource = Session[sessionKeyParticipantsResults];
                rptParticipants.DataBind();
                uppParticipants.Update();
            }
        }

        /// <summary>
        /// Remove participant from logbook
        /// </summary>
        /// <param name="sender">UI object identifying the participant to remove</param>
        private void RemoveParticipant(object sender)
        {
            if (Session[sessionKeyParticipantsResults] != null)
            {
                SaveParticipantsState();

                HtmlButton btnParticipant = (HtmlButton)sender;
                HiddenField hdfParticipantCode = (HiddenField)btnParticipant.Parent.FindControl("hdfParticipantCode");
                HiddenField hdfEmployeeNominalClass = (HiddenField)btnParticipant.Parent.FindControl("hdfParticipantNominalClass");

                if (hdfParticipantCode != null)
                {
                    string participantCode = hdfParticipantCode.Value;
                    DataTable participants = LoadParticipants();

                    DataRow selectedRow = participants.AsEnumerable().FirstOrDefault(r => r.Field<string>("ParticipantCode") == participantCode);
                    if (selectedRow != null)
                    {
                        participants.Rows.Remove(selectedRow);

                        rptParticipants.DataSource = participants;
                        rptParticipants.DataBind();
                        uppParticipants.Update();
                    }

                    lblSearchParticipantsResults.InnerHtml = string.Format("{0} {1}",
                        Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults")), participants.DefaultView.Count);

                    UpdateCountNominalClass();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromRemoveParticipantPostBack{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnFromRemoveParticipantPostBack('ctl00_cntBody_rptParticipants_ctl01_hdfParticipantCode') }, 1000);  ", true);
                }
            }
        }

        /// <summary>
        /// Get nominal class by participant
        /// </summary>
        /// <param name="nominalClassByParticipant"></param>
        /// <returns></returns>
        private string GetNominalClassByParticipant(string nominalClassId)
        {
            string nominalClassType = Convert.ToString(GetLocalResourceObject("lblOperatives"));

            if (sessionNominalClass.Contains(nominalClassId))
            {
                nominalClassType = Convert.ToString(GetLocalResourceObject("lblAdministratives"));
            }

            return nominalClassType;
        }

        /// <summary>
        /// Updated count nominal class by participant
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void UpdateCountNominalClass()
        {
            DataTable participants = LoadParticipants();
            int count = 0;

            count = participants.AsEnumerable().Where(row => row.Field<String>("NominalClassType") == GetLocalResourceObject("lblOperatives").ToString()).Count();

            lblOperatives.InnerHtml = string.Format("{0}: {1}",
                Convert.ToString(GetLocalResourceObject("lblOperatives")), count.ToString());

            count = participants.AsEnumerable().Where(row => row.Field<String>("NominalClassType") == GetLocalResourceObject("lblAdministratives").ToString()).Count();

            lblAdministratives.InnerHtml = string.Format("{0}: {1}",
                Convert.ToString(GetLocalResourceObject("lblAdministratives")), count.ToString());
        }

        /// <summary>
        /// Search participants by logbook number
        /// </summary>
        private void SearchParticipantsByLogbookNumber(LogbookEntity logbook)
        {
            DataTable tableParticipants = LoadEmptyParticipants();

            if (logbook.Participants != null)
            {
                logbook.Participants.ForEach(x =>
                {
                    if (!ParticipantExists(x.ParticipantCode, tableParticipants))
                    {
                        tableParticipants.Rows.Add(
                            x.ParticipantCode,
                            x.ParticipantName,
                            x.CostCenter,
                            x.NominalClassId,
                            GetNominalClassByParticipant(x.NominalClassId),
                            x.IsPresent,
                            Convert.ToInt32(Math.Round(x.Grade)),
                            Convert.ToInt32(Math.Round(x.Grade)),
                            x.Approved);
                    }
                });
                tableParticipants.AsEnumerable().Where(w => w.Field<int>("Grade").Equals(-1) && w.Field<int>("OriginalGrade").Equals(-1)).ToList().ForEach(sm =>
                {
                    sm.SetField<string>("Grade", null);
                    sm.SetField<string>("OriginalGrade", null);
                });

                tableParticipants = tableParticipants.AsEnumerable()
                 .Where(w => !string.IsNullOrEmpty(w.Field<string>("NominalClassId")))
                 .CopyToDataTable();
            }

            // Asignación del DataTable al control de la UI, manipulación de sesión, etc.
            Session[sessionKeyParticipantsResults] = tableParticipants;
            rptParticipants.DataSource = tableParticipants;
            rptParticipants.DataBind();
            uppParticipants.Update();

            lblSearchParticipantsResults.InnerHtml = string.Format("{0} {1}",
                Convert.ToString(GetLocalResourceObject("lblSearchParticipantsResults")), tableParticipants.DefaultView.Count);

            UpdateCountNominalClass();
            txtSearchEmployees.Text = string.Empty;
            rptEmployees.DataSource = null;
            rptEmployees.DataBind();
            uppEmployees.Update();
        }

        /// <summary>
        /// Check if a participant already exists in the DataTable.
        /// </summary>
        private bool ParticipantExists(string participantCode, DataTable table)
        {
            return table.AsEnumerable().Any(row => row.Field<string>("ParticipantCode") == participantCode);
        }


        /// <summary>
        /// Save participants states while retaining grades and original grades
        /// </summary>
        private void SaveParticipantsState()
        {
            if (Session[sessionKeyParticipantsResults] != null)
            {
                DataTable participants = Session[sessionKeyParticipantsResults] as DataTable;

                foreach (RepeaterItem item in rptParticipants.Items)
                {
                    HtmlButton btnParticipant = item.FindControl("btnRemoveParticipant") as HtmlButton;
                    HiddenField hdfParticipantCode = btnParticipant.Parent.FindControl("hdfParticipantCode") as HiddenField;

                    CheckBox chkIsPresent = item.FindControl("chkIsPresent") as CheckBox;

                    TextBox txtGrade = item.FindControl("txtGrade") as TextBox;
                    TextBox txtOriginalGrade = item.FindControl("txtOriginalGrade") as TextBox;

                    if (hdfParticipantCode != null)
                    {
                        string participantCode = hdfParticipantCode.Value;
                        DataRow selectedRow = participants.AsEnumerable().FirstOrDefault(r => r.Field<string>("ParticipantCode") == participantCode);

                        if (selectedRow != null)
                        {
                            selectedRow.SetField("IsPresent", chkIsPresent.Checked);

                            if (!string.IsNullOrEmpty(txtGrade.Text) && !string.IsNullOrEmpty(txtOriginalGrade.Text))
                            {
                                selectedRow.SetField("Grade", Convert.ToInt32(txtGrade.Text));
                                selectedRow.SetField("OriginalGrade", Convert.ToInt32(txtOriginalGrade.Text));
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region AdvancedSearch

        /// <summary>
        /// Valid the form about struct by
        /// </summary>
        /// <returns></returns>
        public bool IsValidStructBy()
        {
            bool isValidStructBy = false;

            string structBy = StructByEdit.Value;
            if (structBy.Equals("1") && (!string.IsNullOrEmpty(CostZoneIdEdit.Value) && !string.IsNullOrEmpty(CostMiniZoneIdEdit.Value) && !string.IsNullOrEmpty(CostFarmsIdEdit.Value)))
            {
                isValidStructBy = true;
            }

            if (structBy.Equals("2") && (!string.IsNullOrEmpty(CompanyIdEdit.Value) && !string.IsNullOrEmpty(NominalClassIdEdit.Value)))
            {
                isValidStructBy = true;
            }

            return isValidStructBy;
        }

        /// <summary>
        /// Laod cost zones
        /// </summary>
        public void LoadCostZone()
        {
            var dtDivision = Divisions.Select(r => new TypeTableMultipleIdDto { Id = r.DivisionCode }).ToList().ToDataTableGet();

            var costZones = ObjMatrixTargetBll.CostZonesListEnableByDivisions(dtDivision);
            Session[sessionKeyCostZoneList] = costZones;

            var options = costZones.AsEnumerable().Select(fr => new ListItem
            {
                Value = fr.CostZoneId,
                Text = fr.CostZoneName,
            }).ToArray();

            CostZoneIdEdit.Items.Clear();
            CostZoneIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
            CostZoneIdEdit.Items.AddRange(options);
        }

        /// <summary>
        /// Load cost mini zones
        /// </summary>
        public void LoadCostMiniZone()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            Session[sessionKeyCostMiniZoneList] = ObjMatrixTargetBll.CostMiniZonesListEnableByDivision(geographicDivisionCode, divisionCode);

            CostMiniZoneIdEdit.Items.Clear();
        }

        /// <summary>
        /// Load cost farms
        /// </summary>
        public void LoadCostFarms()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            Session[sessionKeyCostFarmList] = ObjMatrixTargetBll.CostFarmsListEnableByDivision(geographicDivisionCode);

            CostFarmsIdEdit.Items.Clear();
        }

        /// <summary>
        /// Load companies
        /// </summary>
        public void LoadCompanies()
        {
            var dtDivision = Divisions.Select(r => new TypeTableMultipleIdDto { Id = r.DivisionCode }).ToList().ToDataTableGet();

            var companies = ObjMatrixTargetBll.CompaniesListEnableByDivision(dtDivision);
            Session[sessionKeyCompaniesList] = companies;

            var options = companies.AsEnumerable().Select(fr => new ListItem
            {
                Value = fr.CompanyID,
                Text = fr.CompanyName
            }).ToArray();

            CompanyIdEdit.Items.Clear();
            CompanyIdEdit.Items.Add(new ListItem() { Value = "", Text = "" });
            CompanyIdEdit.Items.AddRange(options);
        }

        /// <summary>
        /// Load nominal classes
        /// </summary>
        public void LoadNominalClass()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            Session[sessionKeyNominalClassList] = ObjMatrixTargetBll.NominalClassListEnabledByCompanie(geographicDivisionCode);

            NominalClassIdEdit.Items.Clear();
        }

        /// <summary>
        /// Load cost center by struct by farm
        /// </summary>
        public void LoadCostCenterByStruct()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            Session[sessionKeyCostCenterList] = ObjMatrixTargetBll.CostCentersListByStruct(geographicDivisionCode);

            CostCenterIdEdit.Items.Clear();
            CostCenterIdEditMultiple.Value = string.Empty;
        }

        /// <summary>
        /// Gets the selected cost zones
        /// </summary>
        public DataTable GetSelectedCostZones()
        {
            var dtCostZones = CostZoneIdEdit.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostZones;
        }

        /// <summary>
        /// Gets the selected cost mini zones
        /// </summary>
        public DataTable GetSelectedCostMiniZones()
        {
            var dtCostMiniZones = CostMiniZoneIdEdit.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Gets the selected cost farms
        /// </summary>
        public DataTable GetSelectedCostFarms()
        {
            var dtCostMiniZones = CostFarmsIdEdit.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Gets the selected companies
        /// </summary>
        public DataTable GetSelectedCompanies()
        {
            var dtCompanies = CompanyIdEdit.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                if (string.IsNullOrEmpty(values.ElementAtOrDefault(0)))
                {
                    return new TypeTableMultipleIdDto();
                }
                return new TypeTableMultipleIdDto { Id = int.Parse(values.ElementAtOrDefault(0)), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCompanies;
        }

        /// <summary>
        /// Gets the selected nominal class
        /// </summary>
        public DataTable GetSelectedNominalClass()
        {
            var dtCostMiniZones = NominalClassIdEdit.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Gets the selected cost center
        /// </summary>
        public DataTable GetSelectedCostCenter()
        {
            var dtCostCenters = CostCenterIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostCenters;
        }

        /// <summary>
        /// Process all selected rows in the grid
        /// </summary>
        private void ProcessSelectedRows(bool status)
        {
            DataTable employees = Session[sessionKeyAdvancedSearchEmployeeSave] as DataTable;

            foreach (GridViewRow row in grvAdvancedSearchEmployees.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkAdvancedSearchSelectedEmployee") as CheckBox);
                    if (status)
                    {
                        CreateEmployeeBySelectedRow(row.Cells[0], employees);
                    }

                    else
                    {
                        DeleteEmployeeBySelectedRow(row.Cells[0], employees);
                    }

                    chkRow.Checked = status;
                }
            }

            Session[sessionKeyAdvancedSearchEmployeeSave] = employees;
        }

        /// <summary>
        /// Create employee by selected row
        /// </summary>
        private void CreateEmployeeBySelectedRow(Control control, DataTable employees)
        {
            HiddenField hdfEmployeeCode = (control.FindControl("hdfEmployeeCode") as HiddenField);
            HiddenField hdfEmployeeName = (control.FindControl("hdfEmployeeName") as HiddenField);
            HiddenField hdfEmployeeNominalClass = (control.FindControl("hdfEmployeeNominalClass") as HiddenField);
            HiddenField hdfEmployeeCostCenter = (control.FindControl("hdfEmployeeCostCenter") as HiddenField);

            DataRow employee = employees.AsEnumerable().FirstOrDefault(w => string.Equals(hdfEmployeeCode.Value, w.Field<string>("EmployeeCode")));
            if (employee == null)
            {
                employees.Rows.Add(hdfEmployeeCode.Value, hdfEmployeeName.Value, hdfEmployeeCostCenter.Value, hdfEmployeeNominalClass.Value);
            }
        }

        /// <summary>
        /// Delete employee by selected row
        /// </summary>
        private void DeleteEmployeeBySelectedRow(Control control, DataTable employees)
        {
            HiddenField hdfEmployeeCode = (control.FindControl("hdfEmployeeCode") as HiddenField);

            DataRow employee = employees.AsEnumerable().FirstOrDefault(w => string.Equals(hdfEmployeeCode.Value, w.Field<string>("EmployeeCode")));
            if (employee != null)
            {
                employees.Rows.Remove(employee);
            }
        }

        /// <summary>
        /// Keep selected employee
        /// </summary>
        private void KeepSelectedEmployee(int page)
        {
            CheckBox chk = grvAdvancedSearchEmployees.HeaderRow.FindControl("chkAdvancedSearchSelectedAllEmployee") as CheckBox;
            if (chk != null)
            {
                if (Session[sessionKeyAdvancedSelectedPageEmployees] == null)
                {
                    return;
                }

                List<string> pages = Session[sessionKeyAdvancedSelectedPageEmployees] as List<string>;
                string seletedPage = pages.FirstOrDefault(w => w.Contains($"P{page}"));

                if (seletedPage != null)
                {
                    var options = seletedPage.Split('|');
                    bool.TryParse(options[1], out bool status);

                    chk.Checked = status;
                }

                Session[sessionKeyAdvancedSelectedPageEmployees] = pages;
            }
        }

        /// <summary>
        /// Control checked individual
        /// </summary>
        /// <param name=""></param>
        private void ControlCheckedIndividual(bool status)
        {
            CheckBox chkAll = grvAdvancedSearchEmployees.HeaderRow.FindControl("chkAdvancedSearchSelectedAllEmployee") as CheckBox;
            chkAll.Checked = false;


            // Lista para almacenar los CheckBoxes seleccionados
            List<CheckBox> selectedCheckBoxes = grvAdvancedSearchEmployees.Rows
                .Cast<GridViewRow>()
                .Where(row => row.RowType == DataControlRowType.DataRow)
                .Select(row => row.Cells[0]. FindControl("chkAdvancedSearchSelectedEmployee") as CheckBox)
                .Where(chk => chk != null && chk.Checked)
                .ToList();

            // Obtén el número de CheckBoxes seleccionados con selectedCheckBoxes.Count
            int selectedCount = selectedCheckBoxes.Count;


            PageHelper<EmployeeEntity> pageHelper = (PageHelper<EmployeeEntity>)Session[sessionKeyAdvancedSearchResults];

            List<string> pages = Session[sessionKeyAdvancedSelectedPageEmployees] as List<string>;
            string seletedPage = pages.FirstOrDefault(w => w.Contains($"P{pageHelper.CurrentPage}"));

            if (string.IsNullOrEmpty(seletedPage))
            {
                seletedPage = $"P{pageHelper.CurrentPage}|{false}|{1}";
                pages.Add(seletedPage);

                return;
            }

            int pageCount = 0;

            string[] options = seletedPage.Split('|');
            options[1] = bool.FalseString;

            pageCount = status ? int.Parse(options[2]) - 1 : int.Parse(options[2]) + 1;

            if (pageCount < 0)
            {
                pageCount = 0;
            }

            options[2] = pageCount.ToString();
            string seletedPageNew = string.Join("|", options);

            pages.Remove(seletedPage);
            pages.Add(seletedPageNew);

            var pageSizeValue =  int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            pageSizeValue = pageHelper.ResultList.Count < pageSizeValue ? pageHelper.ResultList.Count : pageSizeValue;
            if ( pageSizeValue.Equals(selectedCount))
            {
                chkAll.Checked = true;
            }

            Session[sessionKeyAdvancedSelectedPageEmployees] = pages;
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<EmployeeEntity> SearchResults(int page)
        {
            int.TryParse(StructByEdit.Value, out int structSelect);

            #region Struct Farm

            var costZoneIdMultiple = GetSelectedCostZones();
            var costMiniZoneIdMultiple = GetSelectedCostMiniZones();
            var costFarmsIdMultiple = GetSelectedCostFarms();

            #endregion

            #region Struct Nominal Class

            var companyIdMultiple = GetSelectedCompanies();
            var nominalClassIdMultiple = GetSelectedNominalClass();

            #endregion

            var costCenterIdMultiple = GetSelectedCostCenter();

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvAdvancedSearchEmployees.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvAdvancedSearchEmployees.ClientID);

            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            DataTable participants = LoadParticipants();
            DataTable participantsLogBook = LoadEmptyParticpants();

            if (participants.Rows.Count > 0)
            {
                participants.AsEnumerable().ToList().ForEach(pt =>
                {
                    participantsLogBook.Rows.Add(
                        divisionCode,
                        pt.Field<string>("ParticipantCode"),
                        geographicDivisionCode,
                        0);
                });
            }

            PageHelper<EmployeeEntity> pageHelper = ObjEmployeesBll.ListByStruct(geographicDivisionCode, structSelect, participantsLogBook,
                costZoneIdMultiple, costMiniZoneIdMultiple, costFarmsIdMultiple, companyIdMultiple, nominalClassIdMultiple,
                costCenterIdMultiple, txtAdvancedSearchEmployees.Text,
                divisionCode, sortExpression, sortDirection, page);

            Session[sessionKeyAdvancedSearchResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyAdvancedSearchResults] != null)
            {
                var pageHelper = Session[sessionKeyAdvancedSearchResults] as PageHelper<EmployeeEntity>;

                // Filtrar la lista de resultados para eliminar duplicados por ParticipantCode
                var filteredResults = pageHelper.ResultList
                    .GroupBy(participant => participant.EmployeeCode)
                    .Select(group => group.First())
                    .ToList();

                grvAdvancedSearchEmployees.DataSource = filteredResults;
                grvAdvancedSearchEmployees.DataBind();

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                }
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), filteredResults.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            }
            else
            {
                htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }
        }

        /// <summary>
        /// Set the configuration for displaying check the rows
        /// </summary>
        private void DisplayRowsChecked()
        {
            if (Session[sessionKeyAdvancedSearchEmployeeSave] is DataTable employees && employees.Rows.Count > 0)
            {
                int contEmployeesSelected = 0;
                bool flag = false;
                employees.AsEnumerable().ToList().ForEach(pt =>
                {
                    for (int i = 0; i < grvAdvancedSearchEmployees.Rows.Count; ++i)
                    {
                        CheckBox chkRow = grvAdvancedSearchEmployees.Rows[i].Cells[0].FindControl("chkAdvancedSearchSelectedEmployee") as CheckBox;

                        string key = grvAdvancedSearchEmployees.DataKeys[i].Value.ToString();
                        if (pt.Field<string>("EmployeeCode") == key)
                        {
                            chkRow.Checked = true;
                            contEmployeesSelected++;
                        }
                    }
                });
                if (grvAdvancedSearchEmployees.Rows.Count == contEmployeesSelected)
                {
                    CheckBox chkRowAll = grvAdvancedSearchEmployees.HeaderRow.Cells[0].FindControl("chkAdvancedSearchSelectedAllEmployee") as CheckBox;
                    chkRowAll.Checked = true;
                    flag = true;
                }
                RegisterStartupScript(Page, "setTimeout(function () {{ IsPresentAll('" + flag + "'); }}, 10);", true);
            }
        }

        /// <summary>
        /// Clear results of search
        /// </summary>
        public void ClearResultSearch()
        {
            Session[sessionKeyAdvancedSearchResults] = null;

            grvAdvancedSearchEmployees.DataSource = null;
            grvAdvancedSearchEmployees.DataBind();

            PagerUtil.SetupPager(blstPager, 0, 0);

            htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
        }

        /// <summary>
        /// Clear all options selected
        /// </summary>
        public void ClearSelected()
        {
            CostZoneIdEdit.Value = "";
            CostMiniZoneIdEdit.Value = "";
            CostFarmsIdEdit.Value = "";

            CompanyIdEdit.Value = "";
            NominalClassIdEdit.Value = "";
        }

        #endregion

        #region Others

        /// <summary>
        /// Load classroom by selected training center
        /// </summary>
        /// <param name="sender">UI object identifying the request object</param>
        private void LoadClassroomsBySelectedTrainingCenter()
        {
            if (Session[this.sessionKeyClassroomsResults] != null)
            {
                DataTable classrooms = Session[sessionKeyClassroomsResults] as DataTable;
                classrooms.DefaultView.RowFilter = string.Format("TrainingCenterCode = '{0}'", cboTrainingCenter.SelectedValue);

                cboClassroom.Enabled = true;
                cboClassroom.DataValueField = "ClassroomCode";
                cboClassroom.DataTextField = "ClassroomDescription";
                cboClassroom.DataSource = classrooms.DefaultView;
                cboClassroom.DataBind();

                PopulateClassroomPanel(null);
            }
        }

        /// <summary>
        /// Populate trainers by course
        /// </summary>
        /// <param name="sender">UI object identifying the request object</param>
        private void PopulateTrainersByCourse()
        {
            if (Session[sessionKeyCoursesResults] != null)
            {
                DataTable courses = Session[sessionKeyCoursesResults] as DataTable;
                DataRow selectedRow = courses.AsEnumerable().FirstOrDefault(r => r.Field<string>("CourseCode") == cboCourse.SelectedValue);

                if (selectedRow != null)
                {
                    string courseCode = selectedRow.Field<string>("CourseCode");

                    List<TrainerEntity> trainersByCourse = ObjTrainersBll.ListByCourse(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        courseCode);

                    var TrainerTypeList = GetAllValuesAndLocalizatedDescriptions<TrainerType>();

                    DataTable trainers = LoadEmptyTrainers();
                    txtTrainer.InnerText = string.Empty;

                    trainersByCourse.ForEach(x => trainers.Rows.Add(
                        string.Format("{0},{1}", x.TrainerType, x.TrainerCode), string.Format("{0} - {1} - {2}", x.TrainerCode, x.TrainerName, TrainerTypeList[x.TrainerType.ToString()]), x.TrainerCode, x.TrainerType, x.TrainerName));

                    Session[sessionKeyTrainersResults] = trainers;

                    cboTrainer.Enabled = true;
                    cboTrainer.DataValueField = "TrainerPK";
                    cboTrainer.DataTextField = "TrainerDisplayName";
                    cboTrainer.DataSource = trainers;
                    cboTrainer.DataBind();
                }
            }
        }

        /// <summary>
        /// Populate cycle of training by course
        /// </summary>
        /// <param name="sender">UI object identifying the request object</param>
        private void PopulateCycleTrainingByCourse()
        {
            cycleTranining.Visible = hdfCyclesRefreshment.Value.Equals(bool.TrueString);
            panelCycleTranining.Visible = hdfCyclesRefreshment.Value.Equals(bool.TrueString);

            if (Session[sessionKeyCyleTrainingResults] != null)
            {
                DataTable cyclesTraining = Session[sessionKeyCyleTrainingResults] as DataTable;
                List<DataRow> registeredCycleTraining = cyclesTraining.AsEnumerable().Where(r => r.Field<string>("CourseCode") == cboCourse.SelectedValue).ToList();

                DataTable cycleTraining = LoadEmptyCycleTraining();

                registeredCycleTraining.ForEach(r => cycleTraining.Rows.Add(
                    cboCourse.SelectedValue, r.Field<string>("CycleTrainingCode"), r.Field<string>("CycleTrainingName"), r.Field<string>("CycleTrainingStartDate"), r.Field<string>("CycleTrainingEndDate")));

                txtCycleTrainingName.InnerText = string.Empty;
                txtCycleTrainingStartDate.InnerText = string.Empty;
                txtCycleTrainingEndDate.InnerText = string.Empty;

                cboCycleTranining.Enabled = true;
                cboCycleTranining.DataValueField = "CycleTrainingCode";
                cboCycleTranining.DataTextField = "CycleTrainingName";
                cboCycleTranining.DataSource = cycleTraining;
                cboCycleTranining.DataBind();
            }
        }

        #endregion

        /// <summary>
        /// Register startup script in the page
        /// </summary>
        private void RegisterStartupScript(Control sender, string script, bool timeOut = false)
        {
            if (sender != null && !timeOut)
            {
                ScriptManager.RegisterStartupScript(sender, sender.GetType(), $"{script}{Guid.NewGuid()}", $"{script}('{sender.ClientID}')", true);
            }

            if (sender != null && timeOut)
            {
                ScriptManager.RegisterStartupScript(sender, sender.GetType(), $"{script}{Guid.NewGuid()}", script, true);
            }
        }

        /// <summary>
        /// Get the localizated string of trainer type 
        /// </summary>
        /// <param name="enumerationValuee">Enumeration value</param>
        /// <returns>Localizated name</returns>
        public string GetLogbookStatusLocalizatedDescription(string enumerationValue)
        {
            return GetLogbookStatusLocalizatedDescription(HrisEnum.ParseEnumByName<LogbookStatus>(enumerationValue));
        }

        /// <summary>
        /// Get the localizated string of trainer type 
        /// </summary>
        /// <param name="enumerationValuee">Enumeration value</param>
        /// <returns>Localizated name</returns>
        public string GetLogbookStatusLocalizatedDescription(LogbookStatus enumerationValue)
        {
            return GetLocatizatedDescription(enumerationValue);
        }

        #endregion

    }
}