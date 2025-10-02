using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.Business.Remote;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace HRISWeb.Vacations
{
    public partial class VacationRequest : System.Web.UI.Page
    {
        [Dependency]
        public IVacationBLL ObjVacationBll { get; set; }

        //session key for the results
        readonly string sessionKeyVacationResultBySeatSuperior = "Vacation-ResultBySeatSuperior";
        readonly string sessionKeyEmployeesResults = "Vacation-EmployeesResults";
        readonly string sessionKeyHistoryEmployeesResults = "Vacation-HistoryEmployeesResults";
        readonly string sessionKeyDetailVacationsResults = "Vacation-DetailVacationResults";
        readonly string sessionKeySeatSuperiorResults = "Vacation-SeatSeperiorResults";
        readonly string sessionKeysupervisorListResults = "Vacation-SupervisorList";
        readonly string sessionKeyDaysRequest = "Vacation-DaysRequest";
        private static bool isValidate = false;
        private static bool errorInValidate = false; 

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
                ScriptManager.RegisterStartupScript(this, GetType(), "ReturnFromPostBack", "ReturnFromBtnValidatePostBack();", true);
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
                string eventTarget = Request["__EVENTTARGET"];

                /*When Chance event chbTypeRequest Clear the data for request */
                if (!string.IsNullOrEmpty(eventTarget) && eventTarget.Contains("chbTypeRequest"))
                {

                    Session[sessionKeyDaysRequest] = null;
                    dtpSelectDay.Text = string.Empty;
                    dtpEndDate.Text = string.Empty;
                    dtpStartDate.Text = string.Empty;
                    DisplayResultsDaysRequest();


                }

                if (!IsPostBack)
                {


                    //var userActive = "CORP\\svalerin";
                    var userActive = UserHelper.GetCurrentFullUserName;
                    Session[sessionKeyEmployeesResults] = null;
                    Session[sessionKeyVacationResultBySeatSuperior] = null;
                    Session[sessionKeyDetailVacationsResults] = null;
                    Session[sessionKeySeatSuperiorResults] = null;

                    LoadSeatSuperior(userActive);
                    LoadCenterCost();

                    SearchResults(1);
                    BtnSearch_ServerClick(sender, e);

                    //activate the pager
                    if (Session[sessionKeyEmployeesResults] != null)
                    {
                        PageHelper<VacacitonsBalanceEntity> pageHelper = (PageHelper<VacacitonsBalanceEntity>)Session[sessionKeyEmployeesResults];
                        PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                    }
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
        /// Handles the grvHistory pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvHistory_PreRender(object sender, EventArgs e)
        {
            if ((grvHistory.ShowHeader && grvHistory.Rows.Count > 0) || (grvHistory.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvHistory.ShowFooter && grvHistory.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvHistory.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvDetail pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvDetail_PreRender(object sender, EventArgs e)
        {
            if ((grvDetail.ShowHeader && grvDetail.Rows.Count > 0) || (grvDetail.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvDetail.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvDetail.ShowFooter && grvDetail.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvDetail.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyEmployeesResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<VacacitonsBalanceEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        /// <summary>
        /// Handles the grvHistory sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyHistoryEmployeesResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<VacationHistoryEntity> pageHelper = SearchResultsHistory(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResultsHistory();
            }
        }

        /// <summary>
        /// Handles the grvDetail sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyDetailVacationsResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                //PageHelper<VacationDetailEntity> pageHelper = SearchResultsHistory(1);
                //PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                //DisplayResultsHistory();
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
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                hdftxtNameEmployeeSearchFilter.Value = txtNameEmployeeSearch.Text;
                hdftxtEmployeeCode.Value = txtEmployeeCode.Text;
                hdfCenterCostValueFilter.Value = GetCenterCostFilterSelectedValue();

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
        /// Handles the server-side logic when the "Accept" button is clicked, including creating, editing, 
        /// or reactivating trainer records in the system.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "Accept" button.</param>
        /// <param name="e">The event arguments associated with the button click event.</param>
        protected  void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex != null)
                {
                    bool selected = chbTypeRequest.Checked;
                    var userActive = UserHelper.GetCurrentFullUserName;
                    var division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                    int selectedRowIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                    VacacitonsBalanceEntity vacationSelect = GetSelectedVacationBalance(selectedRowIndex);
                    ValidateRequest();
                    if (!errorInValidate)
                    {
                        if (Session[sessionKeyDaysRequest] != null)
                        {
                            List<VacationRequestDay> listSendVacation = (List<VacationRequestDay>)Session[sessionKeyDaysRequest];
                            var idVacacion =  ObjVacationBll.VacationRequestAdd(vacationSelect.SuperiorSeat, vacationSelect.EmployeeCode, division.GeographicDivisionCode, division.DivisionCode, selected, 8, userActive, listSendVacation);
                             WorkFlowRouter.SendRequest(vacationSelect.EmployeeCode, division.GeographicDivisionCode, division.DivisionCode,idVacacion);
                        }
                    }
                    else
                    {
                        /*Open Modal */
                        var dataKey = grvList.DataKeys[selectedRowIndex];
                        string EmployeeCode = Convert.ToString(dataKey["EmployeeCode"]);
                        string name = Convert.ToString(dataKey["EmployeeName"]);
                        string deteStart = dtpStartDate.Text;
                        string deteend = dtpEndDate.Text;

                        txtCodeEmployee.Text = EmployeeCode;
                        txtNameEmployee.Text = name;
                        chbTypeRequest.Checked = selected;
                        dtpEndDate.Text = deteend;
                        dtpStartDate.Text = deteStart;

                        ScriptManager.RegisterStartupScript((System.Web.UI.Control)sender, sender.GetType(), string.Format("ReturnFromBtnRequestPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnRequestPostBack(); },200);", true);
                    }

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
        /// Handles the BtnRequest click event 
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRequest_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedRowIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                    var dataKey = grvList.DataKeys[selectedRowIndex];
                    string EmployeeCode = Convert.ToString(dataKey["EmployeeCode"]);
                    string name = Convert.ToString(dataKey["EmployeeName"]);
                    DisplayResultsDaysRequest();

                    txtCodeEmployee.Text = EmployeeCode;
                    txtNameEmployee.Text = name;
                    ScriptManager.RegisterStartupScript((System.Web.UI.Control)sender, sender.GetType(), string.Format("ReturnFromBtnRequestPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnRequestPostBack(); }}, 200);", true);
                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the btnHistory click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnHistory_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex != null)
                {
                    LoadHistoryEmployee();
                    DisplayResultsHistory();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#VacationHistoryDialog').modal('show');", true);
                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the BtnBalanceDetails click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnBalanceDetails_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex != null)
                {
                    int selectedRowIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                    int division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                    Session[sessionKeyDetailVacationsResults] = null;

                    VacacitonsBalanceEntity vacationSelect = GetSelectedVacationBalance(selectedRowIndex);
                    LoadDetail(division, vacationSelect.EmployeeCode.Trim(), vacationSelect.CompanyCode.Trim());
                    DisplayResultsDetail();

                    ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#VacationDetail').modal('show');", true);
                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the BtnAddDay click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddDay_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var dayRequest = dtpSelectDay.Text;
                if (!string.IsNullOrEmpty(dayRequest))
                {
                    List<VacationRequestDay> daysRequest = new List<VacationRequestDay>();


                    if (Session[sessionKeyDaysRequest] != null)
                    {
                        daysRequest = (List<VacationRequestDay>)(Session[sessionKeyDaysRequest]);
                    }
                    if (hdfSelectedRowIndex.Value != "-1")
                    {
                        var division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                        int selectedRowIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                        VacacitonsBalanceEntity vacationSelect = GetSelectedVacationBalance(selectedRowIndex);
                        daysRequest.Add(new VacationRequestDay
                        {
                            GeographicDivisionCode = division.GeographicDivisionCode,
                            CodeEmployee = vacationSelect.EmployeeCode,
                            RequestDay = DateTime.ParseExact(dayRequest, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        });
                        daysRequest = daysRequest.OrderBy(x => x.RequestDay).ToList();
                        Session[sessionKeyDaysRequest] = daysRequest;
                        dtpSelectDay.Text = string.Empty;
                        DisplayResultsDaysRequest();
                    }

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
        /// Handles the BtnRemoveDay click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRemoveDay_ServerClick(object sender, EventArgs e)
        {
            try
            {


                HtmlButton htmlButton = (HtmlButton)sender;
                string positionDate = ((TextBox)htmlButton.Parent.FindControl("hdfVacationDayCode")).Text;
                DateTime dateSelectect = DateTime.Parse(positionDate);

                if (Session[sessionKeyDaysRequest] != null)
                {
                    List<VacationRequestDay> listRemove = (List<VacationRequestDay>)Session[sessionKeyDaysRequest];
                    VacationRequestDay dataRemove = listRemove.Where(x => x.RequestDay == dateSelectect).FirstOrDefault();
                    listRemove.Remove(dataRemove);

                    if (listRemove.Count > 0)
                    {
                        listRemove = listRemove.OrderBy(x => x.RequestDay).ToList();
                        Session[sessionKeyDaysRequest] = listRemove;
                    }
                    else
                    {
                        Session[sessionKeyDaysRequest] = null;
                    }
                    DisplayResultsDaysRequest();
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
        #endregion

        #region Employees
        /// <summary>
        /// Returns the selected training center id
        /// </summary>
        /// <returns>The selected training center id</returns>
        private string GetCenterCostFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboCenterCostFilter.SelectedValue))
            {
                selected = cboCenterCostFilter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Displays the results of the employee vacation balance query in a GridView.
        /// Updates the pager and other UI elements to reflect the current data.
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyEmployeesResults] != null)
            {

                PageHelper<VacacitonsBalanceEntity> pageHelper = (PageHelper<VacacitonsBalanceEntity>)Session[sessionKeyEmployeesResults];
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
        /// Displays the results of the days vacation in a GridView.
        /// Updates the pager and other UI elements to reflect the current data.
        /// </summary>
        private void DisplayResultsDaysRequest()
        {
            List<VacationRequestDay> daysRequeste = new List<VacationRequestDay>();
            if (Session[sessionKeyDaysRequest] != null)
            {
                daysRequeste = (List<VacationRequestDay>)Session[sessionKeyDaysRequest];

            }

            rptVacationDays.DataSource = daysRequeste;
            rptVacationDays.DataBind();
            uppVacationDay.Update();
        }

        /// <summary>
        /// Searches and retrieves a paginated list of employee vacation balances based on the current division, 
        /// superior information, sorting, and filtering criteria.
        /// </summary>
        /// <param name="page">The current page number to retrieve.</param>
        /// <returns>
        /// A <see cref="PageHelper{VacacitonsBalanceEntity}"/> object containing the paginated results 
        /// of employee vacation balances. Returns <c>null</c> if no superior information is available in the session.
        /// </returns>
        private PageHelper<VacacitonsBalanceEntity> SearchResults(int page)
        {
            if (Session[sessionKeySeatSuperiorResults] != null)
            {
                
                UserForVacationEntity superiorList = (UserForVacationEntity)Session[sessionKeySeatSuperiorResults];
                int division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
                string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

                DOLE.HRIS.Backend.CR.Business.VacationBLL objVacation = new DOLE.HRIS.Backend.CR.Business.VacationBLL(division);
                PageHelper<VacacitonsBalanceEntity> employeeResults = objVacation.ListBalanceVacationByEmployee(
                      superiorList.Seat
                    , superiorList.CompanyID
                    , sortExpression
                    , sortDirection
                    , page
                    , 10
                    , hdftxtNameEmployeeSearchFilter.Value
                    , hdftxtEmployeeCode.Value
                    , hdfCenterCostValueFilter.Value);

                employeeResults = ObjVacationBll.ListByFilter(employeeResults, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                employeeResults.TotalPages = (int)Math.Ceiling((double)employeeResults.TotalResults / employeeResults.PageSize);

                Session[sessionKeyEmployeesResults] = employeeResults;


                return employeeResults;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Loads the superior seat information for the specified user and stores it in the session.
        /// </summary>
        /// <param name="user">The username for which the superior seat information will be retrieved.</param>
        private void LoadSeatSuperior(string user)
        {
            if (Session[sessionKeySeatSuperiorResults] == null)
            {
                UserForVacationEntity superior = new UserForVacationEntity();
                superior = ObjVacationBll.SuperiorSeat(user);
                if (superior != null)
                {
                    Session[sessionKeySeatSuperiorResults] = superior;
                }
                else
                {
                    int division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                    List<UserForVacationEntity> listSuperior = new List<UserForVacationEntity>();
                    DOLE.HRIS.Backend.CR.Business.VacationBLL objVacation = new DOLE.HRIS.Backend.CR.Business.VacationBLL(division);
                    listSuperior = objVacation.ListSuperiorByUser(user);

                    if (listSuperior != null)
                    {
                        Session[sessionKeysupervisorListResults] = listSuperior;
                        LoadListSupervisor();
                    }
                }

            }
        }

        /// <summary>
        /// Loads and populates the list of supervisors from the session into a drop-down list (cboSeatSuperior).
        /// This method retrieves a list of superior users stored in the session, adds a default empty entry, 
        /// and binds the list to the drop-down list with relevant fields for display and selection.
        /// </summary>
        /// <remarks>
        /// The method first checks if the supervisor list exists in the session. If it does, it adds a default 
        /// empty entry and populates the drop-down list (cboSeatSuperior) with the results. The drop-down 
        /// list is then enabled and the relevant JavaScript is triggered to display the seat superior selection.
        /// </remarks>

        private void LoadListSupervisor()
        {
            if (Session[sessionKeysupervisorListResults] != null)
            {
                List<UserForVacationEntity> listSuperior = new List<UserForVacationEntity>();
                listSuperior.Add(new UserForVacationEntity
                {
                    EmployeeCode = "",
                    Seat = 0,
                    EmployeeName = "",
                    CostCenterID = "",
                    CompanyID = ""
                });

                var listSearch = (List<UserForVacationEntity>)Session[sessionKeysupervisorListResults];

                listSuperior.AddRange(listSearch);
                cboSeatSuperior.Enabled = true;
                cboSeatSuperior.DataValueField = "Seat";
                cboSeatSuperior.DataTextField = "EmployeeName";
                cboSeatSuperior.DataSource = listSuperior;
                cboSeatSuperior.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowSeatSuperior();", true);
            }

        }

        /// <summary>
        /// Loads vacation center costs into the 'cboCenterCostFilter' filter control.
        /// It retrieves the list of superior results from the session, queries the vacation center costs for the division and superior's seat,
        /// and adds a default (empty) option to the beginning of the list. Then, it sets up and binds the data to the combo box for display.
        /// </summary>
        private void LoadCenterCost()
        {
            if (Session[sessionKeySeatSuperiorResults] != null)
            {
                UserForVacationEntity superiorList = (UserForVacationEntity)Session[sessionKeySeatSuperiorResults];
                int division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                DOLE.HRIS.Backend.CR.Business.VacationBLL objVacation = new DOLE.HRIS.Backend.CR.Business.VacationBLL(division);
                List<VacationCenterCostEntity> centerCost = objVacation.ListCenterCost(superiorList.Seat, superiorList.CompanyID);

                // Crear un nuevo objeto de tipo VacationCenterCostEntity para la opción predeterminada
                VacationCenterCostEntity defaultCenterCost = new VacationCenterCostEntity
                {
                    CostCenterID = "", // Este valor vacío será el valor predeterminado
                    CostCenterName = "" // Este será el texto que se mostrará en el dropdown
                };

                // Insertar el objeto predeterminado en la primera posición de la lista
                centerCost.Insert(0, defaultCenterCost);

                cboCenterCostFilter.Enabled = true;
                cboCenterCostFilter.DataValueField = "CostCenterID";
                cboCenterCostFilter.DataTextField = "CostCenterID";
                cboCenterCostFilter.DataSource = centerCost;

                cboCenterCostFilter.DataBind();
            }
        }

        /// <summary>
        /// Validates the vacation request based on the selected type (range or single day).
        /// When the request is for a range of days, it checks if both start and end dates are provided and ensures that the end date is not earlier than the start date.
        /// If the request is for a single day, it ensures that the date selection is valid and present in the session.
        /// The method also validates the request against business logic and displays appropriate error messages based on the result.
        /// </summary>
        private void ValidateRequest() 
        {
            bool selected = chbTypeRequest.Checked;
            errorInValidate = false;
            var division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
            int selectedRowIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
            VacacitonsBalanceEntity vacationSelect = GetSelectedVacationBalance(selectedRowIndex);

            /*Validation when is range*/
            if (selected)
            {
                if (!string.IsNullOrEmpty(dtpStartDate.Text) && !string.IsNullOrEmpty(dtpEndDate.Text))
                {
                    DateTime startDate = DateTime.ParseExact(dtpStartDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime endtDate = DateTime.ParseExact(dtpEndDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (endtDate < startDate)
                    {
                        MensajeriaHelper.MostrarMensaje(Page
                          , TipoMensaje.Error
                          , Convert.ToString(GetLocalResourceObject("msgInvalidRangeDate")));
                        errorInValidate = true;
                    }

                }

                if (string.IsNullOrEmpty(dtpStartDate.Text) || string.IsNullOrEmpty(dtpEndDate.Text))
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , Convert.ToString(GetLocalResourceObject("msgInvalidRangeDate")));
                    errorInValidate = true;

                }


                if (!errorInValidate) 
                {
                    List<VacationRequestDay> rangeVacationDays = new List<VacationRequestDay>
                        {
                            new VacationRequestDay
                            {
                                GeographicDivisionCode = division.GeographicDivisionCode,
                                CodeEmployee = vacationSelect.EmployeeCode,
                                RequestDay = DateTime.ParseExact(dtpStartDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                            },
                             new VacationRequestDay
                            {
                                GeographicDivisionCode = division.GeographicDivisionCode,
                                CodeEmployee = vacationSelect.EmployeeCode,
                                RequestDay = DateTime.ParseExact(dtpEndDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                            }
                        };

                    Session[sessionKeyDaysRequest] = rangeVacationDays;
                }

            }

            /*Validate when is not range*/
            else
            {
                if (Session[sessionKeyDaysRequest] == null)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                          , TipoMensaje.Error
                          , Convert.ToString(GetLocalResourceObject("msgInvalidDatesInsert")));
                    errorInValidate = true;
                }
            }

            if (!errorInValidate) 
            {

                List<VacationRequestDay> listSendVacation = new List<VacationRequestDay>();
                listSendVacation = (List<VacationRequestDay>)Session[sessionKeyDaysRequest];
                int codeErreor = ObjVacationBll.ValidateRequest(vacationSelect.EmployeeCode, division.GeographicDivisionCode, division.DivisionCode, selected, listSendVacation);
                /*Here validate Adam*/


                if (codeErreor != null && codeErreor != 0)
                {


                    switch (codeErreor)
                    {
                        case 1:
                            MensajeriaHelper.MostrarMensaje(Page
                             , TipoMensaje.Error
                             , Convert.ToString(GetLocalResourceObject("msgInvalidPreviewRequest")));
                            break;

                        case 2:
                            MensajeriaHelper.MostrarMensaje(Page
                           , TipoMensaje.Error
                           , Convert.ToString(GetLocalResourceObject("msgInvalidDatesRequest")));
                            break;
                    }
                    errorInValidate = true;
                }
            }


        }
        #endregion

        #region History
        /// <summary>
        /// Loads the superior seat information for the specified user and stores it in the session.
        /// </summary>
        /// <param name="user">The username for which the superior seat information will be retrieved.</param>
        private DataTable LoadEmptyHistory()
        {
            DataTable employees = new DataTable();
            employees.Columns.Add("RequestDate", typeof(string)); // Cambiado a EmployeeCode
            employees.Columns.Add("Period", typeof(string)); // Cambiado a Name
            employees.Columns.Add("Days", typeof(int));
            employees.Columns.Add("Status", typeof(string));

            return employees;
        }

        /// <summary>
        /// Carga la historia de vacaciones del empleado.
        /// </summary>
        private void LoadHistoryEmployee()
        {
            // Check if the session already contains history data
            if (Session[sessionKeyHistoryEmployeesResults] == null)
            {
                // Load an empty DataTable for history
                DataTable history = LoadEmptyHistory();

                // Presumably load dummy vacation requests for demonstration
                List<VacationHistoryEntity> vacationsEmployee = LoadHistoryDummie();

                // Optional: Populate the DataTable with dummy data
                foreach (var vacation in vacationsEmployee)
                {
                    DataRow row = history.NewRow();
                    row["RequestDate"] = vacation.RequestDate.ToString("dd/MM/yyyy"); // Assuming VacacitonsBalanceEntity has RequestDate property
                    row["Period"] = vacation.Period; // Assuming it has a Period property
                    row["Days"] = vacation.Days; // Assuming it has Days property
                    row["Status"] = vacation.Status; // Assuming it has Status property
                    history.Rows.Add(row);
                }

                // Store the populated history in the session
                Session[sessionKeyHistoryEmployeesResults] = history;
            }
        }

        /// <summary>
        /// Retrieves paginated vacation history results for a specific page, sorted by the configured sort expression and direction.
        /// </summary>
        /// <param name="page">The current page number to retrieve for vacation history results.</param>
        /// <returns>
        /// A <see cref="PageHelper{VacationHistoryEntity}"/> object containing the vacation history results for the specified page, 
        /// along with the page size and pagination details.
        /// </returns>
        private PageHelper<VacationHistoryEntity> SearchResultsHistory(int page)
        {

            //string trainerCode = string.IsNullOrWhiteSpace(hdfTrainerCodeFilter.Value) ? null : hdfTrainerCodeFilter.Value;
            //string trainerName = string.IsNullOrWhiteSpace(hdfTrainerNameFilter.Value) ? null : hdfTrainerNameFilter.Value;
            //string trainerType = string.IsNullOrWhiteSpace(hdfTrainerTypeValueFilter.Value) ? null : hdfTrainerTypeValueFilter.Value;

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);




            return new PageHelper<VacationHistoryEntity>(LoadHistoryDummie(), 20, 2, page, 10);
        }

        /// <summary>
        /// Displays the results of the employee vacation history in the GridView, if the results exist in the session.
        /// If no results are found in the session, the GridView is cleared.
        /// </summary>
        private void DisplayResultsHistory()
        {
            if (Session[sessionKeyHistoryEmployeesResults] != null)
            {
                DataTable employeesHistory = (DataTable)Session[sessionKeyHistoryEmployeesResults];
                grvHistory.DataSource = employeesHistory;
                grvHistory.DataBind();
            }
            else
            {
                grvHistory.DataSource = null;
                grvHistory.DataBind();
            }
        }
        #endregion

        #region Detail vacations
        /// <summary>
        /// Retrieves the vacation balance entity for a specific employee at the given index from the session results.
        /// </summary>
        /// <param name="index">The index of the employee's vacation balance to retrieve from the result list.</param>
        /// <returns>
        /// A <see cref="VacacitonsBalanceEntity"/> representing the selected employee's vacation balance, 
        /// or a new instance if the session data is not available.
        /// </returns>
        private VacacitonsBalanceEntity GetSelectedVacationBalance(int index)
        {
            VacacitonsBalanceEntity result = new VacacitonsBalanceEntity();
            if (Session[sessionKeyEmployeesResults] != null)
            {
                PageHelper<VacacitonsBalanceEntity> pageHelper = (PageHelper<VacacitonsBalanceEntity>)Session[sessionKeyEmployeesResults];
                result = pageHelper.ResultList[index];
            }
            return result;
        }

        /// <summary>
        /// Loads the vacation details for a specific employee, division, and company code, and stores the results in the session.
        /// If the vacation details are already available in the session, no new request is made.
        /// </summary>
        /// <param name="division">The division code for which the vacation details are being requested.</param>
        /// <param name="employeeCode">The employee code to fetch the vacation details for.</param>
        /// <param name="companyCode">The company code to fetch the vacation details for.</param>
        private void LoadDetail(int division, string employeeCode, string companyCode)
        {
            if (Session[sessionKeyDetailVacationsResults] == null)
            {
                DOLE.HRIS.Backend.CR.Business.VacationBLL objVacation = new DOLE.HRIS.Backend.CR.Business.VacationBLL(division);
                List<VacationDetailEntity> listDeatailVacation = objVacation.ListDetailVacationByEmployee(employeeCode, companyCode);
                PageHelper<VacationDetailEntity> page = new PageHelper<VacationDetailEntity>
                {
                    ResultList = listDeatailVacation,
                };
                Session[sessionKeyDetailVacationsResults] = page;
            }

        }

        /// <summary>
        /// Displays the vacation details stored in the session by binding them to a GridView control.
        /// If the session contains vacation detail results, the GridView is populated with the data.
        /// </summary>
        private void DisplayResultsDetail()
        {
            if (Session[sessionKeyDetailVacationsResults] != null)
            {

                PageHelper<VacationDetailEntity> pageHelper = (PageHelper<VacationDetailEntity>)Session[sessionKeyDetailVacationsResults];
                grvDetail.DataSource = pageHelper.ResultList;
                grvDetail.DataBind();

                //    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                //    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                //    {
                //        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                //    }

                //    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                //    htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
                //}
                //else
                //{
                //    htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
                //}

                //hdfSelectedRowIndex.Value = "-1";
            }
        }
        #endregion

        #region Dummies
        private List<VacationHistoryEntity> LoadHistoryDummie()
        {
            var vacationHistories = new List<VacationHistoryEntity>
                {
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 01, 10), Period = "2023-01", Days = 5, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 02, 12), Period = "2023-02", Days = 3, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 03, 15), Period = "2023-03", Days = 7, Status = "Pendiente" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 04, 20), Period = "2023-04", Days = 2, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 05, 25), Period = "2023-05", Days = 4, Status = "Rechazado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 06, 30), Period = "2023-06", Days = 5, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 07, 05), Period = "2023-07", Days = 6, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 08, 15), Period = "2023-08", Days = 2, Status = "Pendiente" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 09, 10), Period = "2023-09", Days = 3, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 09, 20), Period = "2023-09", Days = 4, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 10, 01), Period = "2023-10", Days = 5, Status = "Rechazado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 10, 15), Period = "2023-10", Days = 2, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 11, 05), Period = "2023-11", Days = 3, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 11, 20), Period = "2023-11", Days = 6, Status = "Pendiente" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 12, 01), Period = "2023-12", Days = 4, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2023, 12, 15), Period = "2023-12", Days = 5, Status = "Rechazado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2024, 01, 10), Period = "2024-01", Days = 3, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2024, 01, 25), Period = "2024-01", Days = 7, Status = "Aprobado" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2024, 02, 14), Period = "2024-02", Days = 2, Status = "Pendiente" },
                    new VacationHistoryEntity { CodeEmployee = "14445555", RequestDate = new DateTime(2024, 03, 02), Period = "2024-03", Days = 4, Status = "Aprobado" },
                };
            return vacationHistories;

        }
        #endregion
    }
}
