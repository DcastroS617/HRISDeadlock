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
    public partial class Initiatives : System.Web.UI.Page
    {
        [Dependency]
        protected IGeneralConfigurationsBll ObjGeneralConfigurationsBll { get; set; }

        [Dependency]
        public IAdminUsersByModulesBll<AdminUserByModuleEntity> ObjAdminUsersByModulesBll { get; set; }

        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionBll { get; set; }

        [Dependency]
        public IIndicatorsBll<IndicatorEntity> ObjIndicatorBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveyBll { get; set; }

        [Dependency]
        protected IInitiativesBLL<InitiativeEntity> objInitiativesBLL { get; set; }

        [Dependency]
        public IMatrixTargetBll ObjMatrixTargetBll { get; set; }

        //session key for the results
        readonly string sessionKeyInitiativesResults = "TrainingInitiatives-InitiativesResults";
        readonly string sessionKeyInitiativeCodeResults = "TrainingInitiatives-InitiativeCodeResults";
        //readonly string sessionKeyInitiativesResults = "TrainingInitiatives-InitiativesResults";
        readonly string sessionKeyCoursesResults = "TrainingInitiatives-CoursesResults";
        readonly string sessionKeyDivisionResults = "Initiatives-DivisionResults";
        readonly string sessionKeyIndicatorsResults = "Initiatives-IndicatorsResults";
        readonly string sessionKeyTypeInitiativeResults = "TrainingInitiatives-InitiativeNumberResults";
        readonly string sessionKeyStatusInitiativeResults = "TrainingInitiatives-InitiativeStatusResults";
        readonly string sessionKeyStatusInitiativeDelete = "TrainingInitiatives-InitiativeStatusDelete";
        readonly string sessionKeyCostZoneList = "StructBy-CostZoneListResults";
        readonly string sessionKeyCostMiniZoneList = "StructBy-CostMiniZoneListResults";
        readonly string sessionKeyCostFarmList = "StructBy-CostFarmListResults";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session[sessionKeyDivisionResults] = null;
                    Session[sessionKeyCoursesResults] = null;
                    
                    //Session[sessionKeyTrainingCentersResults] = null;
                    Session[sessionKeyTypeInitiativeResults] = Request.QueryString.Get("type");

                    

                    LoadFilters();
                    
                    cboDivision.Enabled = false;
                    cboDivision.SelectedValue = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.ToString();

                    cboDivision_SelectedIndexChanged(sender, e);

                    UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                    string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');
                    hdfCurrentUser.Value = userAccount[0] + "$" + userAccount[1];

                    //fire the event
                    BtnSearch_ServerClick(sender, e);

                    if (Session[sessionKeyStatusInitiativeDelete] != null)
                    {
                        if (Session[sessionKeyStatusInitiativeDelete].Equals("true"))
                        {
                            Session[sessionKeyStatusInitiativeDelete] = "false";
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
                        }
                    }
                }

                //activate the pager
                /*if (Session[sessionKeyInitiativesResults] != null)
                {
                    PageHelper<InitiativeEntity> pageHelper = (PageHelper<InitiativeEntity>)Session[sessionKeyInitiativesResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                }*/
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
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0) || (grvList.ShowHeaderWhenEmpty))
            {
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvList.ShowFooter && grvList.Rows.Count > 0)
            {
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

                PageHelper<InitiativeEntity> pageHelper = SearchResults(1);
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

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    Session[sessionKeyInitiativeCodeResults] = grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["InitiativeCode"].ToString();
                    

                    Response.Redirect("InitiativeRegister.aspx");
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                        , TipoMensaje.Error
                        , GetLocalResourceObject("msgInvalidSelection").ToString());
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
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int InitiativeCode = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["InitiativeCode"]);

                    var result = objInitiativesBLL.InitiativesDeactivate(new InitiativeEntity
                    {
                        InitiativeCode = InitiativeCode
                    });

                    if (result.ErrorNumber == 0)
                    {
                        SearchResults(1);
                        DisplayResults();
                        MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        string.Format(Convert.ToString(GetLocalResourceObject("TitleActivateDeletedDialog")), ""), "");
                    }

                    else if (result.ErrorNumber == -1)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("MsjDesactiveUnique").ToString());
                    }

                    else
                    {
                        throw new Exception(result.ErrorMessage);
                    }

                    hdfSelectedRowIndex.Value = "-1";

                    //ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                     , TipoMensaje.Error
                     , GetLocalResourceObject("msgInvalidSelection").ToString());
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
                Session[sessionKeyInitiativesResults] = string.Empty;
                Session[sessionKeyStatusInitiativeResults] = "New";

                Session[sessionKeyStatusInitiativeDelete] = string.Empty;
                Session[sessionKeyStatusInitiativeDelete] = "false";
                System.Web.HttpContext.Current.Session["initiative"] = string.Empty;

                Session[sessionKeyInitiativeCodeResults] = null;

                Response.Redirect("InitiativeRegister.aspx");
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
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<InitiativeEntity> SearchResults(int page)
        {
            Session[sessionKeyInitiativesResults] = null;
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<InitiativeEntity> pageHelper = objInitiativesBLL.ListByFilters(
                string.IsNullOrWhiteSpace(hdfDivisionValueFilter.Value) ? (int?)null : int.Parse(hdfDivisionValueFilter.Value),
                string.IsNullOrWhiteSpace(hdfCompaniesValueFilter.Value) ? (int?)null : int.Parse(hdfCompaniesValueFilter.Value),
                string.IsNullOrWhiteSpace(hdfFarmValueFilter.Value) ? null : hdfFarmValueFilter.Value.Split('|')[0],
                string.IsNullOrWhiteSpace(hdfIndicatorValueFilter.Value) ? (int?)null : int.Parse(hdfIndicatorValueFilter.Value), 
                null,
                CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                page);

            //PageHelper<InitiativeEntity> pageHelper = new PageHelper<InitiativeEntity>(objInitiativesBLL.ListAll(), 10, 1, 10);



            Session[sessionKeyInitiativesResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Load Initiatives from database
        /// </summary>
        private void LoadFilters()
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

            LoadCompanies();

            /*DataTable courses = LoadCoursesUsedByInitiatives();
            cboCourseFilter.Enabled = true;
            cboCourseFilter.DataValueField = "CourseCode";
            cboCourseFilter.DataTextField = "CourseName";
            cboCourseFilter.DataSource = courses;
            cboCourseFilter.DataBind();

            DataTable trainingCenters = LoadTrainingCentersUsedByInitiatives();
            cboTrainingCenterFilter.Enabled = true;
            cboTrainingCenterFilter.DataValueField = "TrainingCenterCode";
            cboTrainingCenterFilter.DataTextField = "TrainingCenterDescription";
            cboTrainingCenterFilter.DataSource = trainingCenters;
            cboTrainingCenterFilter.DataBind();*/
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
            //courses.Columns.Add("CourseDuration", typeof(decimal));

            return divisions;
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyFarms()
        {
            DataTable farms = new DataTable();
            farms.Columns.Add("CostFarmId", typeof(string));
            farms.Columns.Add("CostFarmName", typeof(string));
            //courses.Columns.Add("CourseDuration", typeof(decimal));

            return farms;
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyIndicators()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("IndicatorCode", typeof(string));
            divisions.Columns.Add("IndicatorName", typeof(string));
            //courses.Columns.Add("CourseDuration", typeof(decimal));

            return divisions;
        }

        /// <summary>
        /// Load Trainers from database
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
        /// Load Trainers from database
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
        /// Load companies from database
        /// </summary>
        private void LoadFarms()
        {
            var division = String.IsNullOrEmpty(cboDivision.SelectedValue) ? (int?)null : int.Parse(cboDivision.SelectedValue);
            if (division != -1)
            {
                List<KeyValuePair<DivisionEntity, string>>
                    divisionsLst = ObjDivisionBll.ListAllWithGeographicDivision();

                DataTable farms = LoadEmptyFarms();
                var foundPair = divisionsLst.Find(pair => pair.Key.DivisionCode == division);
                string associatedValue = foundPair.Value;

                var farmList =
                    ObjMatrixTargetBll
                        .CostFarmsListEnableByDivision(
                            associatedValue); //objSurveyBll.RptCboSurveyExportCompany(division, currentUser.UserCode);

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
            else
            {
                cboFarm.Items.Clear();
            }
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyInitiativesResults] != null)
            {
                PageHelper<InitiativeEntity> pageHelper = (PageHelper<InitiativeEntity>)Session[sessionKeyInitiativesResults];

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
    }
}