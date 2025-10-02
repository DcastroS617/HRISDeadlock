using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.SocialResponsability.ClosingDeprivations
{
    public partial class DeprivationsList : System.Web.UI.Page
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
        readonly string sessionKeyInitiativesResults = "TrainingInitiatives-InitiativesDeprivationsResults";
        readonly string sessionKeyInitiativesResultsManagement = "TrainingInitiatives-InitiativesDeprivationHouseResults";
        readonly string sessionKeyInitiativeCodeResults = "TrainingInitiatives-EmployeeCodeResults";
        readonly string sessionKeyDivisionResults = "Initiatives-DivisionResults";
        readonly string sessionKeyIndicatorsResults = "Initiatives-IndicatorsResults";
        readonly string sessionKeyCoordinatorsResults = "Initiatives-CoordinatorsResults";
        readonly string sessionKeyDeprivationManagementResults = "Initiatives-DeprivationManagementResults";      
        readonly string sessionKeyDeprivationStatusResults = "Initiatives-DeprivationStatusResults";
        readonly string sessionKeyDeprivationProcessResults = "Initiatives-DeprivationProcessResults";
        readonly string sessionKeyDeprivationInstitutionResults = "Initiatives-DeprivationInstitutionResults";
        readonly string sessionKeyDeprivationOrigin = "TrainingInitiatives-DeprivationOrigin";
        readonly string sessionKeyCostZoneList = "StructBy-CostZoneListResults";
        readonly string sessionKeyCostMiniZoneList = "StructBy-CostMiniZoneListResults";
        readonly string sessionKeyCostFarmList = "StructBy-CostFarmListResults";
        readonly string sessionKeyInitiativeList = "Initiatives-InitiativeList";


        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session[sessionKeyInitiativesResults] = null;
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');

                Session[sessionKeyDeprivationInstitutionResults] = null;
                Session[sessionKeyDeprivationProcessResults] = null;
                Session[sessionKeyDeprivationStatusResults] = null;
                Session[sessionKeyInitiativeList] = null;

                LoadList();
                LoadListProcedure();

                cboDivision.Enabled = false;
                cboDivision.SelectedValue = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.ToString();

                cboDivision_SelectedIndexChanged(sender, e);

                if (Session[sessionKeyInitiativeCodeResults] != null && int.TryParse(Session[sessionKeyInitiativeCodeResults] as string, out int tmpInitiativeCode))
                {
                    hndInitiativeCode.Value = Convert.ToString(tmpInitiativeCode);
                }

                //activate the pager
                if (Session[sessionKeyInitiativesResults] != null)
                {
                    PageHelper<IndividualsDeprivations> pageHelper = (PageHelper<IndividualsDeprivations>)Session[sessionKeyInitiativesResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                    DisplayResults();
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

                var DeprivationResult = pageHelper.ResultList.FirstOrDefault(ind => ind.DeprivationCode == int.Parse(hdfDeprivationCode.Value));

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
                Response.Redirect("Initiatives.aspx");
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
                int.TryParse(Session[sessionKeyInitiativeCodeResults] as string, out int initiativeCode);
                int? poverty = null;
                //string gender = rblGender.SelectedValue != "" ? rblGender.SelectedValue : null;
                string familyRelationship = null;
                int? startAge = txtAgeStart.Text == "" ? (int?)null : int.Parse(txtAgeStart.Text);
                int? endAge = txtAgeEnd.Text == "" ? (int?)null : int.Parse(txtAgeEnd.Text);
                int? startSeniority = txtSeniorityStart.Text == "" ? (int?)null : int.Parse(txtSeniorityStart.Text);
                int? endSeniority = txtSeniorityEnd.Text == "" ? (int?)null : int.Parse(txtSeniorityEnd.Text);
                decimal? startPovertyScore = txtPovertyScoreStart.Text == "" ? (int?)null : int.Parse(txtPovertyScoreStart.Text);
                decimal? endPovertyScore = txtPovertyScoreEnd.Text == "" ? (int?)null : int.Parse(txtPovertyScoreEnd.Text); ;

                DbaEntity result = null;
                result = objInitiativeBeneficiariesBLL.InitiativeBeneficiariesSave(
                    int.Parse(Session[sessionKeyInitiativeCodeResults].ToString()),
                    poverty,
                    "",
                    familyRelationship,
                    startAge,
                    endAge,
                    startSeniority,
                    endSeniority,
                    startPovertyScore,
                    endPovertyScore, null, null, UserHelper.GetCurrentFullUserName);

                if (result.ErrorNumber == 0)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        string.Format(Convert.ToString(GetLocalResourceObject("msgInitiativeBeneficiariesSaveCompleted")), ""), "ReturnInitiativePage");


                }

                else if (result.ErrorNumber == -3)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Advertencia,
                        GetLocalResourceObject("msgorderduplicate").ToString());
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
            if (e.CommandName == "Household")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                //Int64 index = Convert.ToInt32(e.CommandArgument);
                Session[sessionKeyInitiativesResultsManagement] = Session[sessionKeyInitiativesResults];
                Session[sessionKeyInitiativeCodeResults] = e.CommandArgument.ToString().Split('-')[0];//e.CommandArgument;
                Session[sessionKeyDeprivationOrigin] = "2";

                Response.Redirect("DeprivationManagement.aspx");

                // Get the last name of the selected author from the appropriate
                // cell in the GridView control.
                //GridViewRow selectedRow = grvList.Rows[index];


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
                if (cboDivision.SelectedValue != "")
                {
                    LoadCostZone();

                    CostMiniZoneIdEdit.Items.Clear();
                    CostFarmsIdEdit.Items.Clear();

                    LoadCostMiniZone();
                    LoadCostFarms();
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
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                hdfDivisionValueFilter.Value = GetDivisionFilterSelectedValue();
                hdfCompaniesValueFilter.Value = GetCompanyFilterSelectedValue();
                hdfFarmValueFilter.Value = GetFarmFilterSelectedValue();
                hdfIndicatorValueFilter.Value = GetIndicatorFilterSelectedValue();

                SearchResults(1);

                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

                DisplayResults();
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
        /// Handles the grvListHousehold pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvListDeprivationProcedures_PreRender(object sender, EventArgs e)
        {
            if ((grvListDeprivationProcedures.ShowHeader && grvListDeprivationProcedures.Rows.Count > 0) || (grvListDeprivationProcedures.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvListDeprivationProcedures.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvListDeprivationProcedures.ShowFooter && grvListDeprivationProcedures.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvListDeprivationProcedures.FooterRow.TableSection = TableRowSection.TableFooter;
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
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvListDeprivationProcedures.ClientID, e.SortExpression);

                PageHelper<IndividualsDeprivations> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                //DisplayResults();
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

                    //SearchResults(page);
                    //DisplayResults();
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
            /*if (e.CommandName == "ViewDetails")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                Session[sessionKeyInitiativeCodeResults] = e.CommandArgument;


                Response.Redirect("InitiativeManagement.aspx");

                // Get the last name of the selected author from the appropriate
                // cell in the GridView control.
                //GridViewRow selectedRow = grvListHousehold.Rows[index];


            }*/
        }

        /// <summary>
        /// Handles the btnCostZoneIdEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnCostZoneIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<MatrixTargetByCostMiniZonesEntity> costMiniZoneList = Session[sessionKeyCostMiniZoneList] as List<MatrixTargetByCostMiniZonesEntity>;

                var filterCostZones = CostZoneIdEditMultiple.Value.Split(',');
                costMiniZoneList = costMiniZoneList.Where(w => filterCostZones.Contains(w.CostZoneID)).ToList();

                var options = costMiniZoneList.AsEnumerable().Select(fr => new ListItem
                {
                    Value = fr.CostMiniZoneId,
                    Text = fr.CostMiniZoneName
                }).ToArray();

                CostMiniZoneIdEdit.Items.Clear();
                CostMiniZoneIdEdit.Items.AddRange(options);
                CostMiniZoneIdEdit.SelectedIndex = 0;
                CostMiniZoneIdEditMultiple.Value = "";

                CostFarmsIdEdit.Items.Clear();
                CostFarmsIdEditMultiple.Value = "";

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCostZoneIdEdit{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRefreshDropdownList(); },200);  ", true);
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
        protected void BtnCostMiniZoneIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<MatrixTargetByCostFarmsEntity> costFarmList = Session[sessionKeyCostFarmList] as List<MatrixTargetByCostFarmsEntity>;

                var filterCostZones = CostZoneIdEditMultiple.Value.Split(',');
                costFarmList = costFarmList.Where(w => filterCostZones.Contains(w.CostZoneID)).ToList();

                var filterCostMiniZones = CostMiniZoneIdEditMultiple.Value.Split(',');
                costFarmList = costFarmList.Where(w => filterCostMiniZones.Contains(w.CostMiniZoneID)).ToList();

                var options = costFarmList.AsEnumerable().Select(fr => new ListItem
                {
                    Value = fr.CostFarmId,
                    Text = fr.CostFarmName
                }).ToArray();

                CostFarmsIdEdit.Items.Clear();
                CostFarmsIdEdit.Items.AddRange(options);
                CostFarmsIdEditMultiple.Value = "";

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCostMiniZoneIdEdit{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRefreshDropdownList(); },200);  ", true);
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
        /// Returns the selected division id
        /// </summary>
        /// <returns>The selected division id</returns>
        private string GetDivisionFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboDivision.SelectedValue) && !"-1".Equals(cboDivision.SelectedValue))
            {
                selected = cboDivision.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected company id
        /// </summary>
        /// <returns>The selected company id</returns>
        private string GetCompanyFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboCompanies.SelectedValue) && !"-1".Equals(cboCompanies.SelectedValue))
            {
                selected = cboCompanies.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected farm id
        /// </summary>
        /// <returns>The selected farm id</returns>
        private string GetFarmFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboFarm.SelectedValue) && !"-1".Equals(cboFarm.SelectedValue))
            {
                selected = cboFarm.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected indicator id
        /// </summary>
        /// <returns>The selected indicator id</returns>
        private string GetIndicatorFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboIndicator.SelectedValue) && !"-1".Equals(cboIndicator.SelectedValue))
            {
                selected = cboIndicator.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Load Lists from database
        /// </summary>
        private void LoadList()
        {
            DataTable divisions = LoadDivisions();
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

            DataTable initiatives = LoadInitiatives();
            cboInitiative.Enabled = true;
            cboInitiative.DataValueField = "InitiativeCode";
            cboInitiative.DataTextField = "InitiativeName";
            cboInitiative.DataSource = initiatives;
            cboInitiative.DataBind();            

            LoadCompanies();
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
        /// Load empty data structure for coordinators
        /// </summary>
        /// <returns>Empty data structure for coordinators</returns>
        private DataTable LoadEmptyInitiatives()
        {
            DataTable initiatives = new DataTable();
            initiatives.Columns.Add("InitiativeCode", typeof(string));
            initiatives.Columns.Add("InitiativeName", typeof(string));

            return initiatives;
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
        /// Load initiatives from database
        /// </summary>        
        private DataTable LoadInitiatives()
        {
            DataTable coordinators = (DataTable)Session[sessionKeyInitiativeList];

            if (Session[sessionKeyInitiativeList] == null)
            {
                coordinators = LoadEmptyInitiatives();

                List<InitiativeEntity> registeredCourses = objInitiativesBLL.ListAll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                registeredCourses.ForEach(x => coordinators.Rows.Add(
                    x.InitiativeCode, x.InitiativeName));

                DataRow defaultRow = coordinators.NewRow();
                defaultRow.SetField("InitiativeCode", "-1");
                defaultRow.SetField("InitiativeName", string.Empty);
                coordinators.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyInitiativeList] = coordinators;
            }

            return coordinators;
        }

        /// <summary>
        /// Load companies from database
        /// </summary>
        private void LoadCompanies()
        {
            var division = String.IsNullOrEmpty(cboDivision.SelectedValue) ? (int?)null : int.Parse(cboDivision.SelectedValue);
            UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
            var companyList = objSurveyBll.RptCboSurveyExportCompany(division, currentUser.UserCode);

            cboCompanies.DataValueField = "CompanyID";
            cboCompanies.DataTextField = "CompanyName";
            cboCompanies.DataSource = companyList;
            cboCompanies.DataBind();
        }

        /// <summary>
        /// Load farms from database
        /// </summary>
        private void LoadFarms()
        {
            var division = String.IsNullOrEmpty(cboDivision.SelectedValue) ? (int?)null : int.Parse(cboDivision.SelectedValue);
            List<KeyValuePair<DivisionEntity, string>> divisionsLst = ObjDivisionBll.ListAllWithGeographicDivision();

            DataTable farms = LoadEmptyFarms();


            var foundPair = divisionsLst.Find(pair => pair.Key.DivisionCode == division);

            string associatedValue = foundPair.Value;

            var farmList = ObjMatrixTargetBll.CostFarmsListEnableByDivision(associatedValue);

            farmList.ForEach(x => farms.Rows.Add(
                x.CostFarmId, x.CostFarmName));

            DataRow defaultRow = farms.NewRow();

            defaultRow.SetField("CostFarmId", "-1");
            defaultRow.SetField("CostFarmName", string.Empty);
            farms.Rows.InsertAt(defaultRow, 0);

            cboFarm.DataValueField = "CostFarmId";
            cboFarm.DataTextField = "CostFarmName";
            cboFarm.DataSource = farms;
            cboFarm.DataBind();
        }

        /// <summary>
        /// Search initiative from database
        /// </summary>
        private void SearchInitiative()
        {
            InitiativeEntity initiative = null;
            if (!string.IsNullOrEmpty(hndInitiativeCode.Value))
            {
                initiative = objInitiativesBLL.ListByKey(int.Parse(hndInitiativeCode.Value));
            }

            //txtInitiativeName.Text = initiative.InitiativeName;
            cboDivision.SelectedValue = initiative.DivisionCode.ToString();
            LoadCompanies();
            LoadFarms();
            cboCompanies.SelectedValue = initiative.CompanyCode.ToString();
            cboFarm.SelectedValue = initiative.CostFarmId + "|" + initiative.GeographicDivisionCode;
            cboCoordinator.SelectedValue = initiative.CoordinatorCode.ToString();
            //dtpStartDate.Text = initiative.StartDate.ToString("MM/dd/yyyy");
            //dtpEndDate.Text = initiative.EndDate.ToString("MM/dd/yyyy");
            //cboIndicator.SelectedValue = initiative.IndicatorCode.ToString();
            //txtBeneficiaries.Text = initiative.Beneficiaries.ToString();
            //txtBudget.Text = initiative.Budget.ToString().Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            //txtInvestedHours.Text = initiative.InvestedHours.ToString();
            //chkBMPIAssociated.Checked = initiative.BMPIAsociated;
        }

        /// <summary>
        /// Disable all controls
        /// </summary>
        private void DisableControls()
        {
            //txtInitiativeName.Enabled = false;
            cboDivision.Enabled = false;
            cboCompanies.Enabled = false;
            cboFarm.Enabled = false;
            cboCoordinator.Enabled = false;
            //dtpStartDate.Enabled = false;
            //dtpEndDate.Enabled = false;
            //cboIndicator.Enabled = false;
            //txtBeneficiaries.Enabled = false;
            //txtBudget.Enabled = false;
            //txtInvestedHours.Enabled = false;
            //chkBMPIAssociated.Enabled = false;
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


            //int initiativeCode;

            int.TryParse(Session[sessionKeyInitiativeCodeResults] as string, out int initiativeCode);

            string familyRelationship = null;
            int? startAge = txtAgeStart.Text == "" ? (int?)null : int.Parse(txtAgeStart.Text);
            int? endAge = txtAgeEnd.Text == "" ? (int?)null : int.Parse(txtAgeEnd.Text);
            int? startSeniority = txtSeniorityStart.Text == "" ? (int?)null : int.Parse(txtSeniorityStart.Text);
            int? endSeniority = txtSeniorityEnd.Text == "" ? (int?)null : int.Parse(txtSeniorityEnd.Text);
            decimal? startPovertyScore = txtPovertyScoreStart.Text == "" ? (int?)null : int.Parse(txtPovertyScoreStart.Text);
            decimal? endPovertyScore = txtPovertyScoreEnd.Text == "" ? (int?)null : int.Parse(txtPovertyScoreEnd.Text);            

            PageHelper<IndividualsDeprivations> pageHelper = objInitiativeBeneficiariesBLL.DeprivationsByFilters(
                txtEmloyeeCode.Text == "" ? null : txtEmloyeeCode.Text,
                string.IsNullOrWhiteSpace(hdfDivisionValueFilter.Value) ? (int?)null : int.Parse(hdfDivisionValueFilter.Value),
                string.IsNullOrWhiteSpace(hdfCompaniesValueFilter.Value) ? (int?)null : int.Parse(hdfCompaniesValueFilter.Value),
                string.IsNullOrWhiteSpace(hdfFarmValueFilter.Value) ? null : hdfFarmValueFilter.Value.Split('|')[0],
                string.IsNullOrWhiteSpace(hdfIndicatorValueFilter.Value) ? (int?)null : int.Parse(hdfIndicatorValueFilter.Value),
                string.IsNullOrWhiteSpace(hdfCoordinatorValueFilter.Value) ? (int?)null : int.Parse(hdfCoordinatorValueFilter.Value),
                string.IsNullOrWhiteSpace(cboInitiative.SelectedValue) ? (int?)null : int.Parse(cboInitiative.SelectedValue),
                null,
                null,
                familyRelationship,
                startAge,
                endAge,
                startSeniority,
                endSeniority,
                startPovertyScore,
                endPovertyScore,
                CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                page);

            Session[sessionKeyInitiativesResults] = pageHelper;
            return pageHelper;
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

        /// <summary>
        /// Laod cost zones
        /// </summary>
        public void LoadCostZone()
        {
            var dtDivision = cboDivision.SelectedValue.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();

            var costZones = ObjMatrixTargetBll.CostZonesListEnableByDivisions(dtDivision);
            Session[sessionKeyCostZoneList] = costZones;

            var options = costZones.AsEnumerable().Select(fr => new ListItem
            {
                Value = fr.CostZoneId,
                Text = fr.CostZoneName
            }).ToArray();

            CostZoneIdEdit.Items.Clear();
            CostZoneIdEdit.Items.AddRange(options);
        }

        /// <summary>
        /// Load cost mini zones
        /// </summary>
        public void LoadCostMiniZone()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

            var dtDivision = cboDivision.SelectedValue.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();
            Session[sessionKeyCostMiniZoneList] = ObjMatrixTargetBll.CostMiniZonesListEnableByDivisions(geographicDivisionCode, dtDivision);
        }

        /// <summary>
        /// Load cost farms
        /// </summary>
        public void LoadCostFarms()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            var dtDivision = cboDivision.SelectedValue.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();

            Session[sessionKeyCostFarmList] = ObjMatrixTargetBll.CostFarmsListEnableByDivisions(geographicDivisionCode, dtDivision);
        }

        /// <summary>
        /// Gets the selected cost zones
        /// </summary>
        public DataTable GetSelectedCostZones()
        {
            var dtCostZones = CostZoneIdEditMultiple.Value.Split(',').Select(r =>
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
            var dtCostMiniZones = CostMiniZoneIdEditMultiple.Value.Split(',').Select(r =>
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
            var dtCostMiniZones = CostFarmsIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Compare elements of Array with DataTable
        /// </summary>
        public bool CompareArrayToDataTable(ListItem[] listItem, DataTable dataTable)
        {
            var listItems = listItem != null ? listItem.Length : 0;
            return listItems == dataTable.Rows.Count;
        }

        #endregion
    }
}