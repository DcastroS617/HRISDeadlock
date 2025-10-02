using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.SocialResponsability.ClosingDeprivations
{
    public partial class DeprivationManagement : System.Web.UI.Page
    {
        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionBll { get; set; }

        [Dependency]
        public IIndicatorsBll<IndicatorEntity> ObjIndicatorBll { get; set; }

        [Dependency]
        public IInitiativeCoordinatorsBLL<InitiativeCoordinatorEntity> ObjInitiativeCoordinatorBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveyBll { get; set; }

        [Dependency]
        protected IInitiativesBLL<InitiativeEntity> objInitiativesBLL { get; set; }

        [Dependency]
        protected IInitiativeBeneficiariesBLL<InitiativeBeneficiaries> objInitiativeBeneficiariesBLL { get; set; }

        [Dependency]
        protected IDeprivationsBLL<DeprivationEntity> objDeprivationsBLL { get; set; }

        [Dependency]
        protected IDeprivationManagementBLL<DeprivationManagementEntity> objDeprivationManagementBLL { get; set; }

        [Dependency]
        protected IDeprivationStatusBLL<DeprivationStatusEntity> objDeprivationStatusBLL { get; set; }

        [Dependency]
        protected IDeprivationProcessBLL<DeprivationProcessEntity> objDeprivationProcessBLL { get; set; }

        [Dependency]
        protected IDeprivationInstitutionBLL<DeprivationInstitutionEntity> objDeprivationInstitutionBLL { get; set; }


        [Dependency]
        public IMatrixTargetBll ObjMatrixTargetBll { get; set; }

        //session key for the results
        readonly string sessionKeyInitiativesResults = "TrainingInitiatives-InitiativesDeprivationHouseResults";
        readonly string sessionKeyInitiativesResultsHousehold = "TrainingInitiatives-InitiativesResultsHousehold";
        readonly string sessionKeyInitiativeCodeResults = "TrainingInitiatives-InitiativeCodeResults";
        readonly string sessionKeyEmployeeCodeResults = "TrainingInitiatives-EmployeeCodeResults";
        readonly string sessionKeyDivisionResults = "Initiatives-DivisionResults";
        readonly string sessionKeyIndicatorsResults = "Initiatives-IndicatorsResults";
        readonly string sessionKeyCoordinatorsResults = "Initiatives-CoordinatorsResults";
        readonly string sessionKeyDeprivationManagementResults = "Initiatives-DeprivationManagementResults";

        readonly string sessionKeyDeprivationStatusResults = "Initiatives-DeprivationStatusResults";
        readonly string sessionKeyDeprivationProcessResults = "Initiatives-DeprivationProcessResults";
        readonly string sessionKeyDeprivationInstitutionResults = "Initiatives-DeprivationInstitutionResults";
        readonly string sessionKeyDeprivationOrigin = "TrainingInitiatives-DeprivationOrigin";
        readonly string sessionKeyDeprivationName = "TrainingInitiatives-DeprivationName";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');

                Session[sessionKeyDeprivationInstitutionResults] = null;
                Session[sessionKeyDeprivationProcessResults] = null;
                Session[sessionKeyDeprivationStatusResults] = null;


                LoadList();
                LoadListProcedure();
                if (Session[sessionKeyEmployeeCodeResults] != null)
                {
                    hndInitiativeCode.Value = Convert.ToString(Session[sessionKeyEmployeeCodeResults]);
                    SearchHouseholdDeprivations();
                    //SearchInitiative();
                    DisableControls();
                    SearchResults(1);
                    DisplayResults();

                    SearchResultsHousehold(1);
                    DisplayResultsHousehold();
                }
            }

        }

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
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //Response.Redirect("Initiatives.aspx");
                //llenar grid de procedures
                if (Session[sessionKeyInitiativesResults] != null)
                {
                    PageHelper<IndividualsDeprivations> pageHelper = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResults];

                    var result = pageHelper.ResultList.FirstOrDefault(ind => ind.DeprivationCode == int.Parse(hdfDeprivationCode.Value));

                    txtEmployeeCodeDlg.Text = result.EmployeeCode;
                    txtEmployeeNameDlg.Text = result.EmployeeName;
                    txtIndicatorNameDlg.Text = result.IndicatorName;
                    txtIndividualNameDlg.Text = result.IndividualName;
                    txtStatusDlg.Text = result.Deprived == 1 ? "Cerrada" : "En progreso";

                    txtEmployeeCodeDlg.Enabled = false;
                    txtEmployeeNameDlg.Enabled = false;
                    txtIndicatorNameDlg.Enabled = false;
                    txtIndividualNameDlg.Enabled = false;
                    txtStatusDlg.Enabled = false;
                }

                Session[sessionKeyDeprivationManagementResults] = null;
                this.SearchResultsDeprivationManagement(1);
                this.DisplayResultsDeprivationManagement();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEdit_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(); },200);  ", true);
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

        //btnEditHousehold_ServerClick
        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnEditHousehold_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //Response.Redirect("Initiatives.aspx");
                //llenar grid de procedures
                if (Session[sessionKeyInitiativesResultsHousehold] != null)
                {
                    PageHelper<IndividualsDeprivations> pageHelper = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResultsHousehold];

                    var result = pageHelper.ResultList.FirstOrDefault(ind => ind.DeprivationCode == int.Parse(hdfDeprivationCode.Value));

                    txtEmployeeCodeDlg.Text = result.EmployeeCode;
                    txtEmployeeNameDlg.Text = result.EmployeeName;
                    txtIndicatorNameDlg.Text = result.IndicatorName;
                    txtIndividualNameDlg.Text = result.IndividualName;
                    txtStatusDlg.Text = "En progreso";

                    txtEmployeeCodeDlg.Enabled = false;
                    txtEmployeeNameDlg.Enabled = false;
                    txtIndicatorNameDlg.Enabled = false;
                    txtIndividualNameDlg.Enabled = false;
                    txtStatusDlg.Enabled = false;
                }

                Session[sessionKeyDeprivationManagementResults] = null;
                this.SearchResultsDeprivationManagement(1);
                this.DisplayResultsDeprivationManagement();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEdit_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(); },200);  ", true);
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
        protected void btnEditProcedure_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (Session[sessionKeyDeprivationManagementResults] != null)
                {
                    PageHelper<DeprivationManagementEntity> pageHelper = (PageHelper<DeprivationManagementEntity>)Session[sessionKeyDeprivationManagementResults];

                    var result = pageHelper.ResultList.FirstOrDefault(ind => ind.DeprivationManagementCode == int.Parse(hdfDeprivationManagementCode.Value));

                    cboDeprivationStatus.SelectedValue = result.DeprivationStatusCode.ToString();
                    cboDeprivationProcess.SelectedValue = result.DeprivationProcessCode.ToString();
                    cboDeprivationInstitution.SelectedValue = result.DeprivationInstitutionCode.ToString();
                    dtpRegisterDate.Text = result.RegisterDate.ToString("MM/dd/yyyy");
                    txtManagementNotes.Text = result.Notes;
                    txtInvestedHours.Text = result.InvestedHours.ToString();

                    if (txtStatusDlg.Text == "Cerrada")
                    {
                        btnAcceptManagement.Disabled = true;
                        cboDeprivationStatus.Enabled = false;
                        cboDeprivationProcess.Enabled = false;
                        cboDeprivationInstitution.Enabled = false;
                        dtpRegisterDate.Enabled = false;
                        txtManagementNotes.Enabled = false;
                        txtInvestedHours.Enabled = false;
                    }
                }

                Session[sessionKeyDeprivationManagementResults] = null;
                this.SearchResultsDeprivationManagement(1);
                this.DisplayResultsDeprivationManagement();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEditProcedure_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditProcedureOpen(); },200);  ", true);
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
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtManagementNotes.Text = "";
                dtpRegisterDate.Text = "";
                cboDeprivationInstitution.SelectedValue = "-1";
                cboDeprivationStatus.SelectedValue = "-1";
                cboDeprivationProcess.SelectedValue = "-1";
                txtInvestedHours.Text = "";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnRequestbtnAccept{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestbtnAccept(); },200);  ", true);
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
        protected void btnAcceptManagement_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var deprivationManagementCode = hdfDeprivationManagementCode.Value == "-1" || string.IsNullOrEmpty(hdfDeprivationManagementCode.Value) ? 0 : int.Parse(hdfDeprivationManagementCode.Value);
                var deprivationCode = hdfDeprivationCode.Value == "-1" || string.IsNullOrEmpty(hdfDeprivationCode.Value) ? 0 : int.Parse(hdfDeprivationCode.Value);
                PageHelper<IndividualsDeprivations> pageHelper = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResults];

                PageHelper<IndividualsDeprivations> pageHelperHousehold = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResultsHousehold];

                var DeprivationResult = pageHelper.ResultList.FirstOrDefault(ind => ind.DeprivationCode == int.Parse(hdfDeprivationCode.Value));

                if (DeprivationResult == null)
                {
                    DeprivationResult = pageHelperHousehold.ResultList.FirstOrDefault(ind => ind.DeprivationCode == int.Parse(hdfDeprivationCode.Value));
                }

                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');

                var entity = new DeprivationManagementEntity()
                {
                    DeprivationCode = deprivationCode,
                    CreatedBy = hdfCurrentUser.Value,
                    EmployeeCode = DeprivationResult.EmployeeCode,
                    IndicatorCode = DeprivationResult.IndicatorCode,
                    SurveyCode = 0,
                    DeprivationManagementCode = deprivationManagementCode,
                    IndividualCode = DeprivationResult.IndividualCode,
                    DeprivationStatusCode = int.Parse(cboDeprivationStatus.SelectedValue),
                    DeprivationProcessCode = int.Parse(cboDeprivationProcess.SelectedValue),
                    DeprivationInstitutionCode = int.Parse(cboDeprivationInstitution.SelectedValue),
                    Notes = txtManagementNotes.Text,
                    RegisterDate = DateTime.ParseExact(dtpRegisterDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    DeprivationStatusDesSpanish = userAccount[0] + @"\" + userAccount[1],
                    InvestedHours = int.Parse(txtInvestedHours.Text)
                };

                DbaEntity result = null;
                result = objDeprivationManagementBLL.Save(entity);

                if (result.ErrorNumber == 0)
                {
                    Session[sessionKeyDeprivationManagementResults] = null;
                    this.SearchResultsDeprivationManagement(1);
                    this.DisplayResultsDeprivationManagement();

                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        string.Format(Convert.ToString(GetLocalResourceObject("msgDeprivationMgmSaveCompleted")), hdfDeprivationCode.Value), "ReturnDeprivationManagementResult");
                }

                else if (result.ErrorNumber == -3)
                {

                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Advertencia,
                        GetLocalResourceObject("msgorderduplicate").ToString());
                }
                else
                {
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("btnAcceptManagement_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnDeprivationManagementResult(); },200);  ", true);
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
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //Session[sessionKeyDeprivationOrigin] = "2"
                if (Session[sessionKeyDeprivationOrigin] != null && Session[sessionKeyDeprivationOrigin].ToString() == "2")
                {
                    Response.Redirect("DeprivationsList.aspx");
                }

                if (Session[sessionKeyDeprivationOrigin] != null && Session[sessionKeyDeprivationOrigin].ToString() == "1")
                {
                    Response.Redirect("InitiativeManagement.aspx");
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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                
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
        protected void GrvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0) || (grvList.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvList.ShowFooter && grvList.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyInitiativesResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<IndividualsDeprivations> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
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

        protected void grvList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

            // If multiple ButtonField column fields are used, use the
            // CommandName property to determine which button was clicked.
            /*if (e.CommandName == "ViewDetails")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                Session[sessionKeyInitiativeCodeResults] = e.CommandArgument;


                Response.Redirect("InitiativeManagement.aspx");

                // Get the last name of the selected author from the appropriate
                // cell in the GridView control.
                //GridViewRow selectedRow = grvList.Rows[index];


            }*/
        }

        //MG

        /// <summary>
        /// Handles the grvListHousehold pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvListHousehold_PreRender(object sender, EventArgs e)
        {
            if ((grvListHousehold.ShowHeader && grvListHousehold.Rows.Count > 0) || (grvListHousehold.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvListHousehold.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvListHousehold.ShowFooter && grvListHousehold.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvListHousehold.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvListHousehold sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvListHousehold_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyInitiativesResultsHousehold] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvListHousehold.ClientID, e.SortExpression);

                PageHelper<IndividualsDeprivations> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        /// <summary>
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BlstPagerHousehold_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    int page = Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value);
                    PagerUtil.SetActivePage(blstPager, page);

                    SearchResults(page);
                    DisplayResults();
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

        protected void grvListHousehold_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

            // If multiple ButtonField column fields are used, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "ViewDetails")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                Session[sessionKeyInitiativeCodeResults] = e.CommandArgument;


                Response.Redirect("InitiativeManagement.aspx");

                // Get the last name of the selected author from the appropriate
                // cell in the GridView control.
                //GridViewRow selectedRow = grvListHousehold.Rows[index];


            }
        }


        /// <summary>
        /// Handles the grvListHousehold pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvListDeprivationProcedures_PreRender(object sender, EventArgs e)
        {
            if ((grvListHousehold.ShowHeader && grvListHousehold.Rows.Count > 0) || (grvListHousehold.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvListHousehold.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvListHousehold.ShowFooter && grvListHousehold.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvListHousehold.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvListHousehold sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvListDeprivationProcedures_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyDeprivationManagementResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvListHousehold.ClientID, e.SortExpression);

                PageHelper<IndividualsDeprivations> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        /// <summary>
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BlstPagerDeprivationProcedure_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    int page = Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value);
                    PagerUtil.SetActivePage(blstPagerDeprivationProcedure, page);

                    SearchResults(page);
                    DisplayResults();
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

        protected void grvListDeprivationProcedures_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

            // If multiple ButtonField column fields are used, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "ViewDetails")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                Session[sessionKeyInitiativeCodeResults] = e.CommandArgument;


                Response.Redirect("InitiativeManagement.aspx");

                // Get the last name of the selected author from the appropriate
                // cell in the GridView control.
                //GridViewRow selectedRow = grvListHousehold.Rows[index];


            }
        }


        /// <summary>
        /// Handles the event selectedIndexChanged on cboGroupType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadCompanies();
                LoadFarms();
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

        #endregion

        #region Methods

        /// <summary>
        /// Load Lists from database
        /// </summary>
        private void LoadList()
        {
            /*DataTable divisions = LoadDivisions();
            cboDivision.Enabled = true;
            cboDivision.DataValueField = "DivisionCode";
            cboDivision.DataTextField = "DivisionName";
            cboDivision.DataSource = divisions;
            cboDivision.DataBind();

            DataTable indicators = LoadIndicators();
            cboIndicator.Enabled = true;
            cboIndicator.DataValueField = "IndicatorCode";
            cboIndicator.DataTextField = "IndicatorName";
            cboIndicator.DataSource = indicators;
            cboIndicator.DataBind();

            DataTable coordinators = LoadCoordinators();
            cboCoordinator.Enabled = true;
            cboCoordinator.DataValueField = "CoordinatorCode";
            cboCoordinator.DataTextField = "CoordinatorName";
            cboCoordinator.DataSource = coordinators;
            cboCoordinator.DataBind();

            LoadCompanies();*/
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyDivisions()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("DivisionCode", typeof(string));
            divisions.Columns.Add("DivisionName", typeof(string));

            return divisions;
        }

        /// <summary>
        /// Load empty data structure for Farms
        /// </summary>
        /// <returns>Empty data structure for Farms</returns>
        private DataTable LoadEmptyFarms()
        {
            DataTable farms = new DataTable();
            farms.Columns.Add("CostFarmId", typeof(string));
            farms.Columns.Add("CostFarmName", typeof(string));

            return farms;
        }

        /// <summary>
        /// Load empty data structure for Indicators
        /// </summary>
        /// <returns>Empty data structure for Indicators</returns>
        private DataTable LoadEmptyIndicators()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("IndicatorCode", typeof(string));
            divisions.Columns.Add("IndicatorName", typeof(string));

            return divisions;
        }

        /// <summary>
        /// Load empty data structure for coordinators
        /// </summary>
        /// <returns>Empty data structure for coordinators</returns>
        private DataTable LoadEmptyCoordinators()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("CoordinatorCode", typeof(string));
            divisions.Columns.Add("CoordinatorName", typeof(string));

            return divisions;
        }

        /// <summary>
        /// Load Divisions from database
        /// </summary>        
        private DataTable LoadDivisions()
        {
            DataTable divisions = (DataTable)Session[sessionKeyDivisionResults];

            if (Session[sessionKeyDivisionResults] == null)
            {
                divisions = LoadEmptyDivisions();

                List<DivisionEntity> indicatorsList = ObjDivisionBll.ListAll();

                indicatorsList.ForEach(x => divisions.Rows.Add(
                    x.DivisionCode, x.DivisionName));

                DataRow defaultRow = divisions.NewRow();

                defaultRow.SetField("DivisionCode", "-1");
                defaultRow.SetField("DivisionName", string.Empty);
                divisions.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyDivisionResults] = divisions;
            }

            return divisions;
        }

        /// <summary>
        /// Load Indicators from database
        /// </summary>        
        private DataTable LoadIndicators()
        {
            DataTable indicators = (DataTable)Session[sessionKeyIndicatorsResults];

            if (Session[sessionKeyIndicatorsResults] == null)
            {
                indicators = LoadEmptyIndicators();

                List<IndicatorEntity> registeredCourses = ObjIndicatorBll.ListAll();

                registeredCourses.ForEach(x => indicators.Rows.Add(
                    x.IndicatorCode, x.IndicatorName));

                DataRow defaultRow = indicators.NewRow();
                defaultRow.SetField("IndicatorCode", "-1");
                defaultRow.SetField("IndicatorName", string.Empty);
                indicators.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyIndicatorsResults] = indicators;
            }

            return indicators;
        }

        /// <summary>
        /// Load coordinators from database
        /// </summary>        
        private DataTable LoadCoordinators()
        {
            DataTable coordinators = (DataTable)Session[sessionKeyCoordinatorsResults];

            if (Session[sessionKeyCoordinatorsResults] == null)
            {
                coordinators = LoadEmptyCoordinators();

                List<InitiativeCoordinatorEntity> registeredCourses = ObjInitiativeCoordinatorBll.ListAll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                registeredCourses.ForEach(x => coordinators.Rows.Add(
                    x.CoordinatorCode, x.CoordinatorName));

                DataRow defaultRow = coordinators.NewRow();
                defaultRow.SetField("CoordinatorCode", "-1");
                defaultRow.SetField("CoordinatorName", string.Empty);
                coordinators.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyCoordinatorsResults] = coordinators;
            }

            return coordinators;
        }

        /// <summary>
        /// Load companies from database
        /// </summary>
        private void LoadCompanies()
        {
            /*var division = String.IsNullOrEmpty(cboDivision.SelectedValue) ? (int?)null : int.Parse(cboDivision.SelectedValue);
            UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
            var companyList = objSurveyBll.RptCboSurveyExportCompany(division, currentUser.UserCode);

            cboCompanies.DataValueField = "CompanyID";
            cboCompanies.DataTextField = "CompanyName";
            cboCompanies.DataSource = companyList;
            cboCompanies.DataBind();*/
        }

        /// <summary>
        /// Load farms from database
        /// </summary>
        private void LoadFarms()
        {
            /*var division = String.IsNullOrEmpty(cboDivision.SelectedValue) ? (int?)null : int.Parse(cboDivision.SelectedValue);
            List<KeyValuePair<DivisionEntity, string>> divisionsLst = ObjDivisionBll.ListAllWithGeographicDivision();

            DataTable farms = LoadEmptyFarms();


            var foundPair = divisionsLst.Find(pair => pair.Key.DivisionCode == division);

            string associatedValue = foundPair.Value;

            var farmList = ObjMatrixTargetBll.CostFarmsListEnableByDivisions(associatedValue);

            farmList.ForEach(x => farms.Rows.Add(
                x.CostFarmId, x.CostFarmName));

            DataRow defaultRow = farms.NewRow();

            defaultRow.SetField("CostFarmId", "-1");
            defaultRow.SetField("CostFarmName", string.Empty);
            farms.Rows.InsertAt(defaultRow, 0);

            cboFarm.DataValueField = "CostFarmId";
            cboFarm.DataTextField = "CostFarmName";
            cboFarm.DataSource = farms;
            cboFarm.DataBind();*/
        }

        private void LoadListProcedure()
        {
            DataTable deprivationStatus = LoadDeprivationStatus();
            cboDeprivationStatus.Enabled = true;
            cboDeprivationStatus.DataValueField = "DeprivationStatusCode";
            cboDeprivationStatus.DataTextField = "DeprivationStatusDesSpanish";
            cboDeprivationStatus.DataSource = deprivationStatus;
            cboDeprivationStatus.DataBind();

            DataTable deprivationProcess = LoadDeprivationProcess();
            cboDeprivationProcess.Enabled = true;
            cboDeprivationProcess.DataValueField = "DeprivationProcessCode";
            cboDeprivationProcess.DataTextField = "DeprivationProcessDesSpanish";
            cboDeprivationProcess.DataSource = deprivationProcess;
            cboDeprivationProcess.DataBind();

            DataTable deprivationInstitution = LoadDeprivationInstitution();
            cboDeprivationInstitution.Enabled = true;
            cboDeprivationInstitution.DataValueField = "DeprivationInstitutionCode";
            cboDeprivationInstitution.DataTextField = "DeprivationInstitutionDesSpanish";
            cboDeprivationInstitution.DataSource = deprivationInstitution;
            cboDeprivationInstitution.DataBind();

        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyDeprivationStatus()
        {
            DataTable status = new DataTable();
            status.Columns.Add("DeprivationStatusCode", typeof(string));
            status.Columns.Add("DeprivationStatusDesSpanish", typeof(string));

            return status;
        }


        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyDeprivationProcess()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("DeprivationProcessCode", typeof(string));
            divisions.Columns.Add("DeprivationProcessDesSpanish", typeof(string));

            return divisions;
        }


        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyDeprivationInstitutions()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("DeprivationInstitutionCode", typeof(string));
            divisions.Columns.Add("DeprivationInstitutionDesSpanish", typeof(string));

            return divisions;
        }

        /// <summary>
        /// Load Indicators from database
        /// </summary>        
        private DataTable LoadDeprivationStatus()
        {
            DataTable status = (DataTable)Session[sessionKeyDeprivationStatusResults];

            if (Session[sessionKeyDeprivationStatusResults] == null)
            {
                status = LoadEmptyDeprivationStatus();

                List<DeprivationStatusEntity> registeredCourses = objDeprivationStatusBLL.ListAll();

                registeredCourses.ForEach(x => status.Rows.Add(
                    x.DeprivationStatusCode, x.DeprivationStatusDesSpanish));

                DataRow defaultRow = status.NewRow();
                defaultRow.SetField("DeprivationStatusCode", "-1");
                defaultRow.SetField("DeprivationStatusDesSpanish", string.Empty);
                status.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyDeprivationStatusResults] = status;
            }

            return status;
        }

        /// <summary>
        /// Load Indicators from database
        /// </summary>        
        private DataTable LoadDeprivationProcess()
        {
            DataTable process = (DataTable)Session[sessionKeyDeprivationProcessResults];

            if (Session[sessionKeyDeprivationProcessResults] == null)
            {
                process = LoadEmptyDeprivationProcess();

                List<DeprivationProcessEntity> registeredCourses = objDeprivationProcessBLL.ListAll();

                registeredCourses.ForEach(x => process.Rows.Add(
                    x.DeprivationProcessCode, x.DeprivationProcessDesSpanish));

                DataRow defaultRow = process.NewRow();
                defaultRow.SetField("DeprivationProcessCode", "-1");
                defaultRow.SetField("DeprivationProcessDesSpanish", string.Empty);
                process.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyDeprivationProcessResults] = process;
            }

            return process;
        }

        /// <summary>
        /// Load Indicators from database
        /// </summary>        
        private DataTable LoadDeprivationInstitution()
        {
            DataTable institution = (DataTable)Session[sessionKeyDeprivationInstitutionResults];

            if (Session[sessionKeyDeprivationInstitutionResults] == null)
            {
                institution = LoadEmptyDeprivationInstitutions();

                List<DeprivationInstitutionEntity> registeredCourses = objDeprivationInstitutionBLL.ListAll();

                registeredCourses.ForEach(x => institution.Rows.Add(
                    x.DeprivationInstitutionCode, x.DeprivationInstitutionDesSpanish));

                DataRow defaultRow = institution.NewRow();
                defaultRow.SetField("DeprivationInstitutionCode", "-1");
                defaultRow.SetField("DeprivationInstitutionDesSpanish", string.Empty);
                institution.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyDeprivationInstitutionResults] = institution;
            }

            return institution;
        }

        /// <summary>
        /// Search initiative from database
        /// </summary>
        private void SearchInitiative()
        {
            /*InitiativeEntity initiative = null;
            if (!string.IsNullOrEmpty(hndInitiativeCode.Value))
            {
                initiative = objInitiativesBLL.ListByKey(int.Parse(hndInitiativeCode.Value));
            }

            txtInitiativeName.Text = initiative.InitiativeName;
            cboDivision.SelectedValue = initiative.DivisionCode.ToString();
            LoadCompanies();
            LoadFarms();
            cboCompanies.SelectedValue = initiative.CompanyCode.ToString();
            cboFarm.SelectedValue = initiative.CostFarmId + "|" + initiative.GeographicDivisionCode;
            cboCoordinator.SelectedValue = initiative.CoordinatorCode.ToString();
            dtpStartDate.Text = initiative.StartDate.ToString("MM/dd/yyyy");
            dtpEndDate.Text = initiative.EndDate.ToString("MM/dd/yyyy");
            cboIndicator.SelectedValue = initiative.IndicatorCode.ToString();
            txtBeneficiaries.Text = initiative.Beneficiaries.ToString();
            txtBudget.Text = initiative.Budget.ToString().Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            txtInvestedHours.Text = initiative.InvestedHours.ToString();
            chkBMPIAssociated.Checked = initiative.BMPIAsociated;*/
        }

        private void SearchHouseholdDeprivations()
        {
            PageHelper<HouseholdDeprivationEntity> pageHelper = null;
            if (!string.IsNullOrEmpty(hndInitiativeCode.Value))
            {
                pageHelper = objDeprivationsBLL.ListByEmployee(hndInitiativeCode.Value);

                List<HouseholdDeprivationEntity> lstDeprivations = pageHelper.ResultList;
                foreach (HouseholdDeprivationEntity row in lstDeprivations)
                {
                    switch (row.IndicatorName)
                    {
                        case "1. Acceso a servicios de salud":
                            lblSalud1IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblSalud1Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "2. Cuidado preventivo":
                            lblSalud2IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblSalud2Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "3. Acceso al agua":
                            lblSalud3IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblSalud3Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "4. Saneamiento":
                            lblSalud4IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblSalud4Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "5. Combustible para cocinar":
                            lblSalud5IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblSalud5Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "1. Asistencia a escuela(5-17 años)":
                            lblEducacion1IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblEducacion1Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "2. Sin bachillerato(18-24 años)":
                            lblEducacion2IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblEducacion2Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "3. Bajo desarrollo de capital humano(25-59 años)":
                            lblEducacion3IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblEducacion3Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "1. Hacinamiento":
                            lblVivienda1IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblVivienda1Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "2. Piso + Techo":
                            lblVivienda2IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblVivienda2Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "3. Paredes":
                            lblVivienda3IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblVivienda3Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "4. Sin acceso a Internet":
                            lblVivienda4IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblVivienda4Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "5. Eliminación de basura":
                            lblVivienda5IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblVivienda5Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "1. Empleo":
                            lblEmpleo1IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblEmpleo1Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "2. Seguridad social / Fondo de pensión":
                            lblEmpleo2IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblEmpleo2Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        case "3. Nini y fuera de la fuerza laboral por obligaciones familiares":
                            lblEmpleo3IPMe.Text = row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprived").ToString() : GetLocalResourceObject("lblNotDeprived").ToString();
                            lblEmpleo3Manage.Text = row.DeprivationClosed == "Cerrada" ? GetLocalResourceObject("lblDeprivationClosed").ToString() : row.DeprivatedHousehold == "Carente: Si" ? GetLocalResourceObject("lblDeprivedPending").ToString() : GetLocalResourceObject("lblDeprivationNA").ToString();
                            break;
                        default:
                            break;
                    }
                    
                    
                }
            }
        }

        /// <summary>
        /// Disable all controls
        /// </summary>
        private void DisableControls()
        {
            /*txtInitiativeName.Enabled = false;
            cboDivision.Enabled = false;
            cboCompanies.Enabled = false;
            cboFarm.Enabled = false;
            cboCoordinator.Enabled = false;
            dtpStartDate.Enabled = false;
            dtpEndDate.Enabled = false;
            cboIndicator.Enabled = false;
            txtBeneficiaries.Enabled = false;
            txtBudget.Enabled = false;
            txtInvestedHours.Enabled = false;
            chkBMPIAssociated.Enabled = false;*/
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<IndividualsDeprivations> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<IndividualsDeprivations> pageHelper = objInitiativeBeneficiariesBLL.IndividualsDeprivationsByEmployee(hndInitiativeCode.Value,
                CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                page);

            if (pageHelper.ResultList.Count > 0)
            {
                FillHouseHoldStatus(pageHelper.ResultList[0]);
            }

            Session[sessionKeyInitiativesResults] = pageHelper;
            return pageHelper;
        }

        private void FillHouseHoldStatus(IndividualsDeprivations deprivations)
        {
            txtEmployeeCode.Text = deprivations.EmployeeCode;
            txtEmployeeName.Text = deprivations.EmployeeName;
            txtEmployeeSeniority.Text = deprivations.EmployeeSeniority.ToString();
            txtCompanyName.Text = deprivations.CompanyName;
            txtDivisionName.Text = deprivations.DivisionName;
            txtCostMiniZoneName.Text = deprivations.CostMiniZoneName;
            txtDeprivationScore.Text = deprivations.DeprivationScore.ToString().Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            chkInPoverty.Checked = deprivations.Poverty == 1 ? true : false;


            txtDeprivationName.Text = Session[sessionKeyDeprivationName] != null ? Session[sessionKeyDeprivationName].ToString() : "";

            txtEmployeeCode.Enabled = false;
            txtEmployeeName.Enabled = false;
            txtEmployeeSeniority.Enabled = false;
            txtCompanyName.Enabled = false;
            txtDivisionName.Enabled = false;
            txtCostMiniZoneName.Enabled = false;
            txtDeprivationScore.Enabled = false;
            chkInPoverty.Enabled = false;
            txtDeprivationName.Enabled = false;

        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyInitiativesResults] != null)
            {
                PageHelper<IndividualsDeprivations> pageHelper = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResults];

                grvList.DataSource = pageHelper.ResultList;
                grvList.DataBind();

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            }

            else
            {
                htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }

            hdfSelectedRowIndex.Value = "-1";
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<IndividualsDeprivations> SearchResultsHousehold(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvListHousehold.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvListHousehold.ClientID);

            PageHelper<IndividualsDeprivations> pageHelper = objInitiativeBeneficiariesBLL.HouseholdDeprivationsByEmployee(hndInitiativeCode.Value,
                CommonFunctions.GetSortExpression(Page.ClientID, grvListHousehold.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvListHousehold.ClientID),
                page);

            if (pageHelper.ResultList.Count > 0)
            {
                FillHouseHoldStatus(pageHelper.ResultList[0]);
            }

            Session[sessionKeyInitiativesResultsHousehold] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResultsHousehold()
        {
            if (Session[sessionKeyInitiativesResultsHousehold] != null)
            {
                PageHelper<IndividualsDeprivations> pageHelper = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResultsHousehold];

                grvListHousehold.DataSource = pageHelper.ResultList;
                grvListHousehold.DataBind();

                PagerUtil.SetupPager(blstPagerHousehold, pageHelper.TotalPages, pageHelper.CurrentPage);
                if (PagerUtil.GetActivePage(blstPagerHousehold) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPagerHousehold, pageHelper.TotalPages);
                }

                PagerUtil.SetActivePage(blstPagerHousehold, PagerUtil.GetActivePage(blstPagerHousehold));

                htmlResultsSubtitleHousehold.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            }

            else
            {
                htmlResultsSubtitleHousehold.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }

            hdfSelectedRowIndex.Value = "-1";
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<DeprivationManagementEntity> SearchResultsDeprivationManagement(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvListDeprivationProcedures.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvListDeprivationProcedures.ClientID);

            /*PageHelper<IndividualsDeprivations> pageHelper = objInitiativeBeneficiariesBLL.IndividualsDeprivationsByEmployee(hndInitiativeCode.Value,
                CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                page);*/

            PageHelper<DeprivationManagementEntity> pageHelper = objDeprivationManagementBLL.ListByFilters(int.Parse(hdfDeprivationCode.Value),
                CommonFunctions.GetSortExpression(Page.ClientID, grvListDeprivationProcedures.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvListDeprivationProcedures.ClientID),
                page);

            //FillHouseHoldStatus(pageHelper.ResultList[0]);

            Session[sessionKeyDeprivationManagementResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResultsDeprivationManagement()
        {
            if (Session[sessionKeyDeprivationManagementResults] != null)
            {
                PageHelper<DeprivationManagementEntity> pageHelper = (PageHelper<DeprivationManagementEntity>)Session[sessionKeyDeprivationManagementResults];

                grvListDeprivationProcedures.DataSource = pageHelper.ResultList;
                grvListDeprivationProcedures.DataBind();

                PagerUtil.SetupPager(blstPagerDeprivationProcedure, pageHelper.TotalPages, pageHelper.CurrentPage);
                if (PagerUtil.GetActivePage(blstPagerDeprivationProcedure) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPagerDeprivationProcedure, pageHelper.TotalPages);
                }

                PagerUtil.SetActivePage(blstPagerDeprivationProcedure, PagerUtil.GetActivePage(blstPagerDeprivationProcedure));

                htmlResultsSubtitleDeprivationProcedure.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            }

            else
            {
                htmlResultsSubtitleDeprivationProcedure.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }

            hdfSelectedRowIndex.Value = "-1";
        }

        #endregion
    }
}