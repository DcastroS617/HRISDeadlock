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
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.SocialResponsability.ClosingDeprivations
{
    public partial class BeneficiariesRegistration : System.Web.UI.Page
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
        public IMatrixTargetBll ObjMatrixTargetBll { get; set; }

        //session key for the results
        readonly string sessionKeyInitiativesResults = "InitiativesBeneficiariesAdd-InitiativesResults";
        readonly string sessionKeyInitiativeCodeResults = "TrainingInitiatives-InitiativeCodeResults";
        readonly string sessionKeyDivisionResults = "InitiativesBeneficiariesAdd-DivisionResults";
        readonly string sessionKeyIndicatorsResults = "InitiativesBeneficiariesAdd-IndicatorsResults";
        readonly string sessionKeyCoordinatorsResults = "InitiativesBeneficiariesAdd-CoordinatorsResults";
        readonly string sessionKeyCostZoneList = "StructBy-CostZoneListResults";
        readonly string sessionKeyCostMiniZoneList = "StructBy-CostMiniZoneListResults";
        readonly string sessionKeyCostFarmList = "StructBy-CostFarmListResults";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');
                LoadList();

                cboDivision.Enabled = false;

                cboDivision.SelectedValue = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.ToString();

                if (Session[sessionKeyInitiativeCodeResults] != null && int.TryParse(Session[sessionKeyInitiativeCodeResults] as string, out int tmpInitiativeCode))
                {
                    hndInitiativeCode.Value = Convert.ToString(tmpInitiativeCode);

                    SearchInitiative(sender, e);
                    DisableControls();
                    //SearchResults(1);
                    //DisplayResults();
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
                string gender = rblGender.SelectedValue != "" ? rblGender.SelectedValue : null;
                string familyRelationship = null;
                int? startAge = txtAgeStart.Text == "" ? (int?)null : int.Parse(txtAgeStart.Text);
                int? endAge = txtAgeEnd.Text == "" ? (int?)null : int.Parse(txtAgeEnd.Text);
                int? startSeniority = txtSeniorityStart.Text == "" ? (int?)null : int.Parse(txtSeniorityStart.Text);
                int? endSeniority = txtSeniorityEnd.Text == "" ? (int?)null : int.Parse(txtSeniorityEnd.Text);
                decimal? startPovertyScore = txtPovertyScoreStart.Text == "" ? (int?)null : int.Parse(txtPovertyScoreStart.Text);
                decimal? endPovertyScore = txtPovertyScoreEnd.Text == "" ? (int?)null : int.Parse(txtPovertyScoreEnd.Text);
                string employeeName = txtEmloyeeName.Text == "" ? null : txtEmloyeeName.Text;
                string employeeCode = txtEmloyeeCode.Text == "" ? null : txtEmloyeeCode.Text;

                DbaEntity result = null;
                result = objInitiativeBeneficiariesBLL.InitiativeBeneficiariesSave(
                    int.Parse(Session[sessionKeyInitiativeCodeResults].ToString()),
                    poverty,
                    gender,
                    familyRelationship,
                    startAge,
                    endAge,
                    startSeniority,
                    endSeniority,
                    startPovertyScore,
                    endPovertyScore, 
                    employeeCode, 
                    employeeName,
                    UserHelper.GetCurrentFullUserName
                    );

                if (result.ErrorNumber == 0)
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        string.Format(Convert.ToString(GetLocalResourceObject("msgInitiativeBeneficiariesSaveCompleted")), txtInitiativeName.Text), "ReturnInitiativePage");

                    
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
            if (e.CommandName == "ViewDetails")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                Session[sessionKeyInitiativeCodeResults] = e.CommandArgument;


                Response.Redirect("InitiativeManagement.aspx");

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

        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                /*hdfDivisionValueFilter.Value = GetDivisionFilterSelectedValue();
                hdfCompaniesValueFilter.Value = GetCompanyFilterSelectedValue();
                hdfFarmValueFilter.Value = GetFarmFilterSelectedValue();
                hdfIndicatorValueFilter.Value = GetIndicatorFilterSelectedValue();*/

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

            LoadCompanies();
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

                int selectedDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                List<InitiativeCoordinatorEntity> registeredCourses = ObjInitiativeCoordinatorBll.ListAll(selectedDivision);

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
        private void SearchInitiative(object sender, EventArgs e)
        {
            InitiativeEntity initiative = null;
            //if (!string.IsNullOrEmpty(hndInitiativeCode.Value))
            //{
            //    initiative = objInitiativesBLL.ListByKey(int.Parse(hndInitiativeCode.Value));
            //}

            var resultMultiple = objInitiativesBLL.ListByKeyTuple(int.Parse(hndInitiativeCode.Value));
            initiative = resultMultiple.Item1;

            txtInitiativeName.Text = initiative.InitiativeName;
            cboDivision.SelectedValue = initiative.DivisionCode.ToString();
            //LoadCompanies();
            //LoadFarms();
            //cboCompanies.SelectedValue = initiative.CompanyCode.ToString();
            //cboFarm.SelectedValue = initiative.CostFarmId + "|" + initiative.GeographicDivisionCode;
            LoadCoordinators();
            cboCoordinator.SelectedValue = initiative.CoordinatorCode.ToString();
            dtpStartDate.Text = initiative.StartDate.ToString("MM/dd/yyyy");
            dtpEndDate.Text = initiative.EndDate.ToString("MM/dd/yyyy");
            cboIndicator.SelectedValue = initiative.IndicatorCode.ToString();
            txtBeneficiaries.Text = initiative.Beneficiaries.ToString();
            txtBudget.Text = initiative.Budget.ToString().Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            txtInvestedHours.Text = initiative.InvestedHours.ToString();
            chkBMPIAssociated.Checked = initiative.BMPIAsociated;

            txtInvestedHoursReal.Text = initiative.CostFarmName;
            txtBeneficiariesReal.Text = initiative.CompanyName;

            LoadCostZone();

            CostMiniZoneIdEdit.Items.Clear();
            CostFarmsIdEdit.Items.Clear();

            LoadCostMiniZone();
            LoadCostFarms();

            var costZones = resultMultiple.Item2;
            CostZoneIdEditMultiple.Value = string.Join(",", costZones.Select(r => r.CostZoneId));

            BtnCostZoneIdEdit_ServerClick(sender, e);

            var costMiniZone = resultMultiple.Item3;
            CostMiniZoneIdEditMultiple.Value = string.Join(",", costMiniZone.Select(r => r.CostMiniZoneId));

            BtnCostMiniZoneIdEdit_ServerClick(sender, e);

            var costFarms = resultMultiple.Item4;
            CostFarmsIdEditMultiple.Value = string.Join(",", costFarms.Select(r => r.CostFarmId));

        }

        /// <summary>
        /// Disable all controls
        /// </summary>
        private void DisableControls()
        {
            txtInitiativeName.Enabled = false;
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
            chkBMPIAssociated.Enabled = false;

            CostMiniZoneIdEditMultiple.Disabled = true;
            CostZoneIdEditMultiple.Disabled = true;
            CostFarmsIdEditMultiple.Disabled = true;

            CostMiniZoneIdEdit.Disabled = true;
            CostZoneIdEdit.Disabled = true;
            CostFarmsIdEdit.Disabled = true;
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
            int? poverty = null;
            string gender = rblGender.SelectedValue != "" ? rblGender.SelectedValue : null;
            string familyRelationship = null;
            int?   startAge = txtAgeStart.Text == "" ? (int?)null : int.Parse(txtAgeStart.Text);
            int?   endAge = txtAgeEnd.Text == "" ? (int?)null : int.Parse(txtAgeEnd.Text);
            int? startSeniority = txtSeniorityStart.Text == "" ? (int?)null : int.Parse(txtSeniorityStart.Text);
            int? endSeniority = txtSeniorityEnd.Text == "" ? (int?)null : int.Parse(txtSeniorityEnd.Text);
            decimal? startPovertyScore = txtPovertyScoreStart.Text == "" ? (decimal?)null : decimal.Parse(txtPovertyScoreStart.Text, CultureInfo.InvariantCulture);
            decimal? endPovertyScore = txtPovertyScoreEnd.Text == "" ? (decimal?)null : decimal.Parse(txtPovertyScoreEnd.Text, CultureInfo.InvariantCulture);
            string employeeName = txtEmloyeeName.Text == "" ? null : txtEmloyeeName.Text;
            string employeeCode = txtEmloyeeCode.Text == "" ? null : txtEmloyeeCode.Text;


            PageHelper<IndividualsDeprivations> pageHelper = objInitiativeBeneficiariesBLL.IndividualsDeprivationsByFilters(int.Parse(Session[sessionKeyInitiativeCodeResults].ToString()),
                poverty,
                gender,
                familyRelationship,
                startAge,
                endAge,
                startSeniority,
                endSeniority,
                startPovertyScore,
                endPovertyScore,
                employeeCode,
                employeeName,                
                CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                page);

            //PageHelper<InitiativeEntity> pageHelper = new PageHelper<InitiativeEntity>(objInitiativesBLL.ListAll(), 10, 1, 10);



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

        #endregion
    }
}