using ClosedXML.Excel;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;

namespace HRISWeb.Overtime
{
    public partial class OverTimePayrollAnalystApproval : System.Web.UI.Page
    {
        [Dependency]
        public IOverTimeRecordsBLL<OverTimeRecordsEntity> overTimeRecordsBLL { get; set; }
        [Dependency]
        public IOverTimeStatusBLL<OverTimeStatusEntity> overTimeStatusBLL { get; set; }
        [Dependency]
        public IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL { get; set; }


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
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnExportExcel);
                if (!IsPostBack)
                {
                    SearchResults(1);
                    BindOvertimeStatusDropdown("cboOvertimeStatu");
                    BindDateTypeDropdown();
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
        /// Handles the btnExportExcel click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeCode = string.Empty;
                UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                OverTimeEmployeeView overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);
                if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                {
                    int superVisorcode = Convert.ToInt32(ConfigurationManager.AppSettings["SupervisorCode"]);
                    var superVisors = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(new RolesByDepartmentEmployeeEntity { RoleApproversCode = superVisorcode, DepartmentCode = overTimeEmployeeView.DepartmentCode });
                    if (superVisors != null && superVisors.Count() > 0)
                    {
                        employeeCode = $"'{string.Join("','", superVisors.Select(x => x.EmployeeCode).ToList())}'";
                        divEmloyeeCode.Visible = true;
                    }
                    else
                    {
                        employeeCode = $"'{overTimeEmployeeView.EmployeeCode}'";
                        divEmloyeeCode.Visible = false;
                    }
                    int dateType = string.IsNullOrEmpty(cboDateType.SelectedValue) ? 0 : cboDateType.SelectedIndex;
                    DateTime dtpStartDate;
                    DateTime.TryParseExact(dtpOvertimeDateFrom.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtpStartDate);

                    DateTime dtpEndDate;
                    DateTime.TryParseExact(dtpOvertimeDateto.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtpEndDate);


                    List<OverTimeRecordsEntity> overTimeRecordsEntities = overTimeRecordsBLL.GetOverTimePayrollAnalystApprovalList(
                          SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , txtEmloyeeCode.Text.Trim()
                        , dateType
                        , dtpStartDate
                        , dtpEndDate
                        , Convert.ToInt32(string.IsNullOrEmpty(txtStartHoursFrom.Text) ? "0" : txtStartHoursFrom.Text.Trim())
                        , Convert.ToInt32(string.IsNullOrEmpty(txtStartHoursTo.Text) ? "0" : txtStartHoursTo.Text.Trim())
                        , Convert.ToInt32(string.IsNullOrEmpty(txtEndHoursFrom.Text) ? "0" : txtEndHoursFrom.Text.Trim())
                        , Convert.ToInt32(string.IsNullOrEmpty(txtEndHoursTo.Text) ? "0" : txtEndHoursTo.Text.Trim())
                        , Convert.ToString(cboOvertimeStatu.SelectedValue) == String.Empty ? 0 : cboOvertimeStatu.SelectedValue.ToInt32()
                        , Convert.ToInt32(string.IsNullOrEmpty(txtovetimeNumber.Text) ? "0" : txtovetimeNumber.Text.Trim())
                        , overTimeEmployeeView.DepartmentCode);

                    if (overTimeRecordsEntities != null && overTimeRecordsEntities.Count() > 0)
                    {
                        var jsonExport = overTimeRecordsEntities.AsEnumerable().Select(c => new {
                            OvertimeNumber = c.OverTimeNumber,
                            OvertimeDate = c.OvertimeDateFormatted,
                            Company = c.Company,
                            Employee = c.Employee,
                            StarHour = c.StartHour,
                            EndHour = c.EndHour,
                            TotalHours = c.TotalOvertimeHours,
                            Justification = c.JustificationForExtraTime,
                            OvertimeClass = c.OvertimeClassificationName,
                            CreatedDate = c.OvertimeCreatedDateFormatted,
                            Status = c.OverTimeStatus,
                        });

                        var jsonserializado = new JavaScriptSerializer().Serialize(jsonExport);

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnExportClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnExportClickPostBack(" + jsonserializado + "); }}, 500);", true);
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgOverTimeApprovalNotFound")));
                    }
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
        /// Bind Overtime Status Dropdown
        /// </summary>
        /// <param name="searchcbo">pass dropdown name for cboOvertimeStatu and cboOverTimeStatusCode</param>
        private void BindOvertimeStatusDropdown(string searchcbo = "")
        {
            try
            {
                List<OverTimeStatusEntity> overTimeStatusEntities = overTimeStatusBLL.GetOverTimeStatusList();

                if (searchcbo == "cboOvertimeStatu")
                {
                    overTimeStatusEntities.Insert(0, new OverTimeStatusEntity { OverTimeStatusCode = 0, OverTimeStatusName = "" });
                    cboOvertimeStatu.DataSource = overTimeStatusEntities;
                    cboOvertimeStatu.DataValueField = "OverTimeStatusCode";
                    cboOvertimeStatu.DataTextField = "OverTimeStatusName";
                    cboOvertimeStatu.DataBind();
                    cboOvertimeStatu.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                }
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }
        /// <summary>
        /// Bind Date Type Dropdown
        /// </summary>
        private void BindDateTypeDropdown()
        {
            try
            {
                List<OvertimeDateTypes> overtimeDateTypes = new List<OvertimeDateTypes>();
                string overtimeDateNme = GetLocalResourceObject("OvertimeDateName").ToString();
                string overtimeCreatedDateNme = GetLocalResourceObject("OvertimeCreatedDateName").ToString();

                overtimeDateTypes.Add(new OvertimeDateTypes { OvertimeDateType = "OvertimeDate", OvertimeDateName = overtimeDateNme });
                overtimeDateTypes.Add(new OvertimeDateTypes { OvertimeDateType = "OvertimeCreatedDate", OvertimeDateName = overtimeCreatedDateNme });
                cboDateType.DataSource = overtimeDateTypes;
                cboDateType.DataValueField = "OvertimeDateType";
                cboDateType.DataTextField = "OvertimeDateName";
                cboDateType.DataBind();
                cboDateType.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                }
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }
        /// <summary>
        /// Handles the btnCleanFilters click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnCleanFilters_Click(object sender, EventArgs e)
        {
            txtEmloyeeCode.Text = "";
            dtpOvertimeDateFrom.Text = "";
            dtpOvertimeDateto.Text = "";
            cboOvertimeStatu.SelectedIndex = 0;
            txtStartHoursFrom.Text = "";
            txtStartHoursTo.Text = "";
            txtovetimeNumber.Text = "";
            txtEndHoursFrom.Text = "";
            txtEndHoursTo.Text = "";
            SearchResults(1);
        }
        /// <summary>
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void blstPager_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    PagerUtil.SetActivePage(blstPager, Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value));
                    SearchResults(PagerUtil.GetActivePage(blstPager));
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
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader == true && grvList.Rows.Count > 0)
            || (grvList.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvList.ShowFooter == true && grvList.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);
            SearchResults(PagerUtil.GetActivePage(blstPager));
        }
        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                PageHelper<OverTimeRecordsEntity> pageHelper = SearchResults(1);
                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);
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
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }
        /// <summary>
        /// Handles the btnSearchFilter click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnSearchFilter_ServerClick(object sender, EventArgs e)
        {
            SearchResults(1);
        }
        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<OverTimeRecordsEntity> SearchResults(int page)
        {
            string userName = String.Empty;
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            //
            if (IsNullOrEmpty(dtpOvertimeDateFrom.Text))
            {
                dtpOvertimeDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                dtpOvertimeDateto.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }
            int dateType = string.IsNullOrEmpty(cboDateType.SelectedValue) ? 0 : cboDateType.SelectedIndex;
            DateTime dtpStartDate;
            DateTime.TryParseExact(dtpOvertimeDateFrom.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtpStartDate);

            DateTime dtpEndDate;
            DateTime.TryParseExact(dtpOvertimeDateto.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtpEndDate);
            string employeeCode = string.Empty;
            UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
            OverTimeEmployeeView overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);
            if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
            {
                int superVisorcode = Convert.ToInt32(ConfigurationManager.AppSettings["SupervisorCode"]);
                var superVisors = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(new RolesByDepartmentEmployeeEntity { RoleApproversCode = superVisorcode, DepartmentCode = overTimeEmployeeView.DepartmentCode});
                if (superVisors != null && superVisors.Count() > 0)
                {
                    employeeCode = $"'{string.Join("','", superVisors.Select(x => x.EmployeeCode).ToList())}'";
                    divEmloyeeCode.Visible = true;
                }
                else
                {
                    employeeCode = $"'{overTimeEmployeeView.EmployeeCode}'";
                    divEmloyeeCode.Visible = false;
                }
                int statusOvertime = cboOvertimeStatu.SelectedValue.Length > 0 ? Convert.ToInt32(cboOvertimeStatu.SelectedValue) : 0 ;
                PageHelper <OverTimeRecordsEntity> pageHelper = overTimeRecordsBLL.GetOverTimeRecordPayrollList(
                  SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , txtEmloyeeCode.Text.Trim()
                , dateType
                , dtpStartDate
                , dtpEndDate
                , Convert.ToInt32(string.IsNullOrEmpty(txtStartHoursFrom.Text) ? "0" : txtStartHoursFrom.Text.Trim())
                , Convert.ToInt32(string.IsNullOrEmpty(txtStartHoursTo.Text) ? "0" : txtStartHoursTo.Text.Trim())
                , Convert.ToInt32(string.IsNullOrEmpty(txtEndHoursFrom.Text) ? "0" : txtEndHoursFrom.Text.Trim())
                , Convert.ToInt32(string.IsNullOrEmpty(txtEndHoursTo.Text) ? "0" : txtEndHoursTo.Text.Trim())
                , Convert.ToString(cboOvertimeStatu.SelectedValue) == String.Empty ? 0 : cboOvertimeStatu.SelectedValue.ToInt32()
                , Convert.ToInt32(string.IsNullOrEmpty(txtovetimeNumber.Text) ? "0" : txtovetimeNumber.Text.Trim())
                , sortExpression
                , sortDirection
                , page
                , pageSizeValue
                , overTimeEmployeeView.DepartmentCode);

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults(pageHelper);

                return pageHelper;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        /// <param name="pageHelper">The page helper that contains result information</param>
        private void DisplayResults(PageHelper<OverTimeRecordsEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            // htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
        }
    }
}