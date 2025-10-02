using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;

namespace HRISWeb.Overtime
{
    public partial class OvertimeRecords : System.Web.UI.Page
    {
        [Dependency]
        public IOverTimeRecordsBLL<OverTimeRecordsEntity> overTimeRecordsBLL { get; set; }
        [Dependency]
        public IDaysDetailBLL<DaysDetailEntity> daysDetailBLL { get; set; }
        [Dependency]
        public IOverTimeStatusBLL<OverTimeStatusEntity> overTimeStatusBLL { get; set; }
        [Dependency]
        public IEmployeesBll<EmployeeEntity> employeesBll { get; set; }
        [Dependency]
        public IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL { get; set; }
        [Dependency]
        public IWorkingTimeRangesBLL<WorkingTimeRangesEntity> workingTimeRangesBLL { get; set; }
        [Dependency]
        public IWorkingTimeTypesBLL<WorkingTimeTypesEntity> workingTimeTypesBLL { get; set; }
        [Dependency]
        public IOvertimeClassificationBLL<OvertimeClassificationEntity> overtimeClassificationBLL { get; set; }

        // <summary>
        // Sets the culture information
        // </summary>
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

        // <summary>
        // Handles the load of the page
        // </summary>
        // <param name = "sender" > Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SearchResults(1);
                    BindOvertimeStatusDropdown("cboOvertimeStatu");
                    BindDateTypeDropdown();
                    GetEmployeeInformation();
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

        // <summary>
        // Handles the grvList pre render event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
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

        // <summary>
        // Handles the grvList sorting event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void grvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);
            SearchResults(PagerUtil.GetActivePage(blstPager));
        }

        // <summary>
        // Handles the blstPager click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
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

        // <summary>
        // Handles the btnSearch click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
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

        // <summary>
        // Handles the btnSearchFilter click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void btnSearchFilter_ServerClick(object sender, EventArgs e)
        {
            SearchResults(1);
        }

        // <summary>
        // Handles the btnCleanFilters click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
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

        // <summary>
        // Handles the btnAdd click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtOverTimeNumber.Text = "0";
                BindHoursDropDown(cboStartHour);
                BindHoursDropDown(cboEndHour);
                txtJustificationForExtraTime.Text = string.Empty;
                chkIsExtraHour.Checked = false;
                dtpOverTimeDate.Text = string.Empty;
                txtApprovalRemark.Text = String.Empty;
                txtApprovalRemark.Enabled = false;
                BindOvertimeStatusDropdown();
                cboOverTimeStatusCode.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        // <summary>
        // Bind Hours DropDown
        // </summary>
        // <param name = "dropDownList" > pass dropdown name</param>
        private void BindHoursDropDown(DropDownList dropDownList)
        {
            List<DropdownItems> dropdownItems = new List<DropdownItems>();
            try
            {
                dropdownItems.Add(new DropdownItems() { Text = "", Value = "" });
                for (int i = 0; i <= 23; i++)
                {
                    string strtimes = i.ToString();
                    if (i.ToString().Length == 1)
                    {
                        strtimes = "0" + strtimes;
                    }
                    dropdownItems.Add(new DropdownItems() { Text = strtimes + ":00", Value = strtimes + ":00" });
                    dropdownItems.Add(new DropdownItems() { Text = strtimes + ":30", Value = strtimes + ":30" });

                }
                dropDownList.DataSource = dropdownItems;
                dropDownList.DataValueField = "Value";
                dropDownList.DataTextField = "Text";
                dropDownList.DataBind();
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        // <summary>
        // Bind Overtime Status Dropdown
        // </summary>
        // <param name = "searchcbo" > pass dropdown name for cboOvertimeStatu and cboOverTimeStatusCode</param>
        private void BindOvertimeStatusDropdown(string searchcbo = "")
        {
            try
            {
                List<OverTimeStatusEntity> overTimeStatuses = overTimeStatusBLL.GetOverTimeStatusList();

                if (searchcbo == "cboOvertimeStatu")
                {
                    overTimeStatuses.Insert(0, new OverTimeStatusEntity { OverTimeStatusCode = 0, OverTimeStatusName = "" });
                    cboOvertimeStatu.DataSource = overTimeStatuses;
                    cboOvertimeStatu.DataValueField = "OverTimeStatusCode";
                    cboOvertimeStatu.DataTextField = "OverTimeStatusName";
                    cboOvertimeStatu.DataBind();
                    cboOvertimeStatu.SelectedIndex = 0;
                }
                else
                {
                    cboOverTimeStatusCode.DataSource = overTimeStatuses;
                    cboOverTimeStatusCode.DataValueField = "OverTimeStatusCode";
                    cboOverTimeStatusCode.DataTextField = "OverTimeStatusName";
                    cboOverTimeStatusCode.DataBind();
                    cboOverTimeStatusCode.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        // <summary>
        // Bind Date Type Dropdown
        // </summary>
        private void BindDateTypeDropdown()
        {
            try
            {
                List<OvertimeDateTypes> overtimeDateTypes = new List<OvertimeDateTypes>();
                string overtimeDateNme = GetLocalResourceObject("OvertimeDateName").ToString();
                string overtimeCreatedDateNme = GetLocalResourceObject("OvertimeCreatedDateName").ToString();

                overtimeDateTypes.Add(new OvertimeDateTypes() { OvertimeDateType = "-1", OvertimeDateName = "" });
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
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        // <summary>
        // Handles the btnEdit click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    BindOvertimeStatusDropdown();
                    int selectedOverTimeNumber = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayOverTimeInformation(selectedOverTimeNumber);

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                    hdfSelectedRowIndex.Value = "-1";
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
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
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        private WorkingTimeHoursByDepartmentEmployeesEntity GetworkingTimeHoursByDepartmentEmployees(string ActiveDirectoryUserAccount)
        {
            WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployeesEntity = null;
            OverTimeEmployeeView overTimeEmployeeView = new OverTimeEmployeeView();
            int CompanieCode = 0;
            try
            {
                overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(ActiveDirectoryUserAccount);

                if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                {
                    if (!string.IsNullOrEmpty(overTimeEmployeeView.DepartmentCode))
                    {
                        DateTime overtimeDateOnly = DateTime.ParseExact(String.Format("{0} 00:00", dtpOverTimeDate.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        CompanieCode = overTimeEmployeeView.CompanyCode;
                        DateTime strDate = Convert.ToDateTime($"{overtimeDateOnly.Day}/{overtimeDateOnly.Month}/{overtimeDateOnly.Year}");
                        overTimeEmployeeView.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                        overTimeEmployeeView.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        workingTimeHoursByDepartmentEmployeesEntity = overTimeRecordsBLL.GetWorkingTimeHoursByDepartmentEmployees(overTimeEmployeeView, strDate.ToString("MM/dd/yyyy"));

                        if (workingTimeHoursByDepartmentEmployeesEntity != null && workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeHoursByDepartmentEmployeesID > 0)
                        {
                            return workingTimeHoursByDepartmentEmployeesEntity;
                        }
                        else workingTimeHoursByDepartmentEmployeesEntity = null;

                    }
                }

                return workingTimeHoursByDepartmentEmployeesEntity;
            }
            catch 
            {
                return null;
            }
        }

        // <summary>
        // Handles the btnAccept click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                OverTimeEmployeeView overTimeEmployeeView = new OverTimeEmployeeView();
                WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployeesEntity = new WorkingTimeHoursByDepartmentEmployeesEntity(); ;
                bool isHoliday = false;
                bool isRestDay = false;

                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                int overtimeNumber = string.IsNullOrEmpty(txtOverTimeNumber.Text) ? 0 : Convert.ToInt32(txtOverTimeNumber.Text);
                int CompanieCode = 0;
                string startHour = cboStartHour.SelectedValue;
                string endHour = cboEndHour.SelectedValue;
                var totalExtraHours = Convert.ToDateTime(endHour) - Convert.ToDateTime(startHour);
                UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHours = GetworkingTimeHoursByDepartmentEmployees(userEntity.ActiveDirectoryUserAccount);
                if (workingTimeHours == null)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkingTimeHoursEmpty")));
                    return;
                }

                if (!(Convert.ToDateTime(startHour).TimeOfDay < Convert.ToDateTime(workingTimeHours.StartHour).TimeOfDay) &&
                    !(Convert.ToDateTime(endHour).TimeOfDay > Convert.ToDateTime(workingTimeHours.EndHour).TimeOfDay))
                {
                    DateTime overtimeDateOnly = DateTime.ParseExact(String.Format("{0} 00:00", dtpOverTimeDate.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    string geoDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                    bool status = false;
                    overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);

                    if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                    {
                        if (!string.IsNullOrEmpty(overTimeEmployeeView.DepartmentCode))
                        {
                            CompanieCode = overTimeEmployeeView.CompanyCode;
                            DateTime strDate = Convert.ToDateTime($"{overtimeDateOnly.Day}/{overtimeDateOnly.Month}/{overtimeDateOnly.Year}");
                            overTimeEmployeeView.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                            overTimeEmployeeView.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                            workingTimeHoursByDepartmentEmployeesEntity = overTimeRecordsBLL.GetWorkingTimeHoursByDepartmentEmployees(overTimeEmployeeView, strDate.ToString("MM/dd/yyyy"));


                            if (workingTimeHoursByDepartmentEmployeesEntity != null && workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeHoursByDepartmentEmployeesID > 0)
                            {
                                if ((String.IsNullOrEmpty(workingTimeHoursByDepartmentEmployeesEntity.RestDay) ? "" : workingTimeHoursByDepartmentEmployeesEntity.RestDay).Contains(((int)overtimeDateOnly.Date.DayOfWeek).ToString()))
                                {
                                    isRestDay = true;
                                    status = true;
                                }
                                else if (daysDetailBLL.GetDaysDetailByDate(new DaysDetailEntity { GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, CodeDateBase = strDate }).Count > 0)
                                {
                                    status = true;
                                    isHoliday = true;
                                }
                                else
                                {//Se evalua si las horas registradas corresponden dentro de la jornada de trabajo.
                                    TimeSpan overtimeStartHour = Convert.ToDateTime(startHour).TimeOfDay;
                                    TimeSpan overtimeEndHour = Convert.ToDateTime(endHour).TimeOfDay;
                                    TimeSpan workingStartHour = Convert.ToDateTime(workingTimeHoursByDepartmentEmployeesEntity.StartHour).TimeOfDay;
                                    TimeSpan workingEndHour = Convert.ToDateTime(workingTimeHoursByDepartmentEmployeesEntity.EndHour).TimeOfDay;
                                    if (((overtimeStartHour <= workingStartHour) && (overtimeStartHour <= workingEndHour) && (overtimeEndHour <= workingStartHour) && (overtimeEndHour <= workingEndHour)) ||
                                        ((overtimeStartHour >= workingStartHour) && (overtimeStartHour >= workingEndHour) && (overtimeEndHour >= workingStartHour) && (overtimeEndHour >= workingEndHour)))
                                    {
                                        status = true;
                                    }
                                    else
                                    {
                                        status = false;
                                    }
                                }
                            }
                            else
                            {
                                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkingTimeHours")));
                                status = false;
                            }

                        }
                        else
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgEmployeeDepartmentNotFound")));
                            status = false;
                        }
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgEmployeeNotFound")));
                        status = false;
                    }
                    if (status)
                    {
                        int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        int overtimeStatusCode = cboOverTimeStatusCode.SelectedValue == null ? 0 : Convert.ToInt32(cboOverTimeStatusCode.SelectedValue);
                        string justification = txtJustificationForExtraTime.Text;

                        bool isSuccess = false;
                        bool isExtraHour = false;

                        int superVisorcode = Convert.ToInt32(ConfigurationManager.AppSettings["SupervisorCode"]);
                        var employees = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(new RolesByDepartmentEmployeeEntity { GeographicDivisionCode = geoDivisionCode, DepartmentCode = overTimeEmployeeView.DepartmentCode });
                        string supervisorEmail = string.Empty;
                        if (employees.Count > 0)
                        {
                            var supervisors = employees.Where(x => x.RoleApproversCode == superVisorcode).ToList();
                            if (supervisors.Count > 0)
                            {
                                var supervisorEmployees = employeesBll.ListByEmployeeCodeGeographicDivision(supervisors.FirstOrDefault().EmployeeCode, geoDivisionCode);
                                supervisorEmail = supervisorEmployees != null ? supervisorEmployees.Email : "";
                            }
                        }
                        if (string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjUserNotMapped")));
                            return;
                        }
                        var overtimeRecordsList = overTimeRecordsBLL.GetOvertimeRecordsByFilters(new OverTimeRecordsEntity
                        {
                            GeographicDivisionCode = geoDivisionCode,
                            DivisionCode = divisionCode,
                            EmployeeCode = overTimeEmployeeView.EmployeeCode                                                                                                                            ,
                            OvertimeDate = overtimeDateOnly.Date
                        });

                        if (overtimeRecordsList.Count > 0)
                        {
                            if (overtimeNumber.Equals(0))
                            {
                                overtimeRecordsList = overtimeRecordsList.Where(X => (Convert.ToDateTime(startHour).TimeOfDay >= Convert.ToDateTime(X.StartHour).TimeOfDay && Convert.ToDateTime(startHour).TimeOfDay <= Convert.ToDateTime(X.EndHour).TimeOfDay) || (Convert.ToDateTime(endHour).TimeOfDay >= Convert.ToDateTime(X.StartHour).TimeOfDay && Convert.ToDateTime(endHour).TimeOfDay <= Convert.ToDateTime(X.EndHour).TimeOfDay)).ToList();
                            }
                            else
                            {
                                overtimeRecordsList = overtimeRecordsList.Where(X => overtimeNumber != X.OverTimeNumber && ((Convert.ToDateTime(startHour).TimeOfDay >= Convert.ToDateTime(X.StartHour).TimeOfDay && Convert.ToDateTime(startHour).TimeOfDay <= Convert.ToDateTime(X.EndHour).TimeOfDay) || (Convert.ToDateTime(endHour).TimeOfDay >= Convert.ToDateTime(X.StartHour).TimeOfDay && Convert.ToDateTime(endHour).TimeOfDay <= Convert.ToDateTime(X.EndHour).TimeOfDay))).ToList();
                            }

                            if (overtimeRecordsList.Count > 0)
                            {
                                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjDuplicateRecord")));
                                return;
                            }
                            else
                            {
                                isExtraHour = true;
                                chkIsExtraHour.Checked = true;
                                if (overtimeNumber.Equals(0) && string.IsNullOrEmpty(justification))
                                {

                                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjJustificationRequired")));
                                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
                                    return;
                                }
                            }
                        }
                        //Obtener el tipo de jornada a pagar y el tipo de nomina a
                        var overtimeWorkingRange = workingTimeRangesBLL.GetWorkingTimeRangesByHours(new WorkingTimeRangesEntity
                        {
                            WorkingStartTime = startHour,
                            WorkingEndTime = endHour
                        });
                        int overtimeClassiFicationCode = 0;
                        var listOvertimeClassication = overtimeClassificationBLL.GetOvertimeClassificationsList(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                        if (isHoliday)
                        {
                            //Asigno el feriado como primer nivel
                            overtimeClassiFicationCode = listOvertimeClassication.Where(o => o.WorkingTimeTypeCode == overtimeWorkingRange.WorkingTimeTypeCode
                            && o.DayTypeCode == 1 && o.IsExtra == true).FirstOrDefault().OvertimeClassificationCode;
                        }
                        else if (isRestDay)
                        {
                            //Trabajo horas extras en su dia de descanso
                            overtimeClassiFicationCode = listOvertimeClassication.Where(o => o.WorkingTimeTypeCode == overtimeWorkingRange.WorkingTimeTypeCode
                            && o.DayTypeCode == 3 && o.IsExtra == true).FirstOrDefault().OvertimeClassificationCode;
                        }
                        else
                        {
                           // Es tiempo extraordinario
                           overtimeClassiFicationCode = listOvertimeClassication.Where(o => o.WorkingTimeTypeCode == overtimeWorkingRange.WorkingTimeTypeCode
                            && o.DayTypeCode == 2 && o.IsExtra == true).FirstOrDefault().OvertimeClassificationCode;
                        }

                        OverTimeRecordsEntity overTimeRecordsEntity = new OverTimeRecordsEntity
                        {
                            OverTimeNumber = overtimeNumber,
                            DivisionCode = divisionCode,
                            EmployeeCode = overTimeEmployeeView.EmployeeCode,
                            StartHour = startHour,
                            EndHour = endHour,
                            GeographicDivisionCode = geoDivisionCode,
                            JustificationForExtraTime = justification,
                            LastModifiedDate = DateTime.Now,
                            LastModifiedUser = lastModifiedUser,
                            CreatedBy = lastModifiedUser,
                            OverTimeStatusCode = overtimeStatusCode,
                            OvertimeCreatedDate = DateTime.Now,
                            OvertimeDate = Convert.ToDateTime(overtimeDateOnly.Date.ToString("yyyy-MM-dd")),
                            IsExtraHour = isExtraHour,
                            CompanieCode = CompanieCode,
                            ApprovalRemark = "", //Se debe de validar este campo
                            DepartmentCode = overTimeEmployeeView.DepartmentCode,
                            WorkingTimeRangeCode = overtimeWorkingRange.WorkingTimeTypeCode,
                            OvertimeClassificationCode = overtimeClassiFicationCode
                        };

                        if (overtimeNumber.Equals(0))
                        {
                            isSuccess = overTimeRecordsBLL.AddOverTimeRecord(overTimeRecordsEntity);
                        }
                        else
                        {
                            isSuccess = overTimeRecordsBLL.UpdateOverTimeRecord(overTimeRecordsEntity);
                        }

                        if (isSuccess)
                        {
                            if (isExtraHour)
                            {
                                try
                                {
                                    bool sendEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["SupervisorEmail"]);
                                    if (!string.IsNullOrEmpty(supervisorEmail) && sendEmail)
                                    {
                                        overTimeRecordsBLL.SendEmailToSupervisor(overTimeRecordsEntity, UserHelper.GetCurrentFullUserName, new List<string> { supervisorEmail }, new List<string>());
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            SearchResults(PagerUtil.GetActivePage(blstPager));

                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackSucces(); }}, 200);", true);
                            return;

                        }
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkingHours")));
                    }
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkingHours")));
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
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
            }

            finally
            {

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));


            }
        }

        // <summary>
        // Handles the btnDelete click event
        // </summary>
        // <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        // <param name="e">Contains the event data</param>
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var SelectedRow = grvList.DataKeys[int.Parse(hdfSelectedRowIndex.Value)];
                var code = Convert.ToInt32(SelectedRow.Value.ToString());
                var records = overTimeRecordsBLL.GetOverTimeRecordByOverTimeNumber(code);
                int overtimeStatusCode = Convert.ToInt32(ConfigurationManager.AppSettings["ApprovedOvertimeStatusCode"]);
                if (records.OverTimeStatusCode == overtimeStatusCode)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjApprovedRecordDelete")));
                    return;
                }
                var result = overTimeRecordsBLL.DeleteOverTimeRecord(code);


                if (result)
                {
                    PageHelper<OverTimeRecordsEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }

                    DisplayResults(pageHelper);

                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(),
                        String.Format("ReturnDeleteSucess{0}", Guid.NewGuid()),
                        "setTimeout(function () {{ ReturnDeleteSucess(); }}, 200);", true);
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgjDeleteFailed")));

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
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        // <summary>
        // Get the employee information 
        // </summary>
        private void GetEmployeeInformation()
        {
            try
            {
                List<WorkingTimeTypesEntity> workingTimeTypesEntities = new List<WorkingTimeTypesEntity>();

                workingTimeTypesEntities.Add(new WorkingTimeTypesEntity() { WorkingTimeTypeCode = 0, WorkingTimeTypeName = "" });
                workingTimeTypesEntities.AddRange(workingTimeTypesBLL.GetWorkingTimeTypesListForDropdown());

                var listJson = Json.Encode(workingTimeTypesEntities);
                string scriptJourneys = string.Format("localStorage.WorkingType= '{0}';", listJson);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "WorkingType", scriptJourneys, true);

                OverTimeEmployeeView overTimeEmployeeView = new OverTimeEmployeeView();
                WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployeesEntity = new WorkingTimeHoursByDepartmentEmployeesEntity();
                UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);

                if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                {
                    if (!string.IsNullOrEmpty(overTimeEmployeeView.DepartmentCode))
                    {
                        overTimeEmployeeView.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                        overTimeEmployeeView.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        workingTimeHoursByDepartmentEmployeesEntity = overTimeRecordsBLL.GetWorkingTimeHoursByDepartmentEmployees(overTimeEmployeeView, DateTime.Now.ToString("MM/dd/yyyy"));
                        if (workingTimeHoursByDepartmentEmployeesEntity != null)
                        {
                            var entityJson = Json.Encode(workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeTypeCode);
                            string script = string.Format("localStorage.Employee= '{0}';", entityJson);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "Employee", script, true);
                        }
                        else {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkingTimeHoursEmpty")));
                            btnAdd.Disabled = true;
                        }                                          
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, newEx.Message);
                }
            }
        }

        // <summary>
        // Search for the results 
        // </summary>
        // <param name = "page" > Results page</param>
        // <returns>Results</returns>
        private PageHelper<OverTimeRecordsEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            //
            if (overTimeRecordsBLL == null)
            {
                overTimeRecordsBLL = Application.GetContainer().Resolve<IOverTimeRecordsBLL<OverTimeRecordsEntity>>();
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

                employeeCode = $"{overTimeEmployeeView.EmployeeCode}";
                divEmloyeeCode.Visible = false;
                PageHelper<OverTimeRecordsEntity> pageHelper = overTimeRecordsBLL.GetOverTimeRecordList(
                  SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , txtEmloyeeCode.Text.Trim()
                , employeeCode
                , dateType
                , dtpStartDate
                , dtpEndDate
                , Convert.ToInt32(string.IsNullOrEmpty(txtStartHoursFrom.Text) ? null : txtStartHoursFrom.Text.Trim())
                , Convert.ToInt32(string.IsNullOrEmpty(txtStartHoursTo.Text) ? null : txtStartHoursTo.Text.Trim())
                , Convert.ToInt32(string.IsNullOrEmpty(txtEndHoursFrom.Text) ? null : txtEndHoursFrom.Text.Trim())
                , Convert.ToInt32(string.IsNullOrEmpty(txtEndHoursTo.Text) ? null : txtEndHoursTo.Text.Trim())
                , Convert.ToInt32(Convert.ToString(cboOvertimeStatu.SelectedValue) == String.Empty ? null : cboOvertimeStatu.SelectedValue.ToString())
                , Convert.ToInt32(string.IsNullOrEmpty(txtovetimeNumber.Text) ? null : txtovetimeNumber.Text.Trim())
                , sortExpression
                , sortDirection
                , page
                , pageSizeValue
                , "", "");

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults(pageHelper);

                return pageHelper;
            }
            else
            {
                return null;
            }

        }

        // <summary>
        // Set the configuration for displaying the results
        // </summary>
        // <param name = "pageHelper" > The page helper that contains result information</param>
        private void DisplayResults(PageHelper<OverTimeRecordsEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
        }

        // <summary>
        // Displays on modal the user information
        // </summary>
        // <param name = "userCode" > The user code to search</param>
        private void DisplayOverTimeInformation(int overTimeNumber)
        {
            try
            {
                var overTimeRecords = overTimeRecordsBLL.GetOverTimeRecordByOverTimeNumber(overTimeNumber);
                if (overTimeRecords != null)
                {
                    BindHoursDropDown(cboStartHour);
                    BindHoursDropDown(cboEndHour);
                    txtOverTimeNumber.Text = overTimeNumber.ToString();
                    cboStartHour.SelectedValue = overTimeRecords.StartHour.ToString();
                    cboEndHour.SelectedValue = overTimeRecords.EndHour.ToString();
                    txtJustificationForExtraTime.Text = overTimeRecords.JustificationForExtraTime.ToString();
                    dtpOverTimeDate.Text = overTimeRecords.OvertimeDate.ToString("MM/dd/yyyy HH:mm");
                    ddlDivisionCode.Value = overTimeRecords.DivisionCode.ToString();
                    ddlEmployeeCode.Value = overTimeRecords.EmployeeCode.ToString();
                    ddlGeographicDivisionCode.Value = overTimeRecords.GeographicDivisionCode.ToString();
                    cboOverTimeStatusCode.SelectedValue = overTimeRecords.OverTimeStatusCode.ToString();
                    chkIsExtraHour.Checked = overTimeRecords.IsExtraHour;
                    txtApprovalRemark.Text = overTimeRecords.ApprovalRemark;
                    txtApprovalRemark.Enabled = false;
                    txtApprovalRemark.Visible = true;
                    var source = cboOverTimeStatusCode.Items.FindByValue(overTimeRecords.OverTimeStatusCode.ToString());

                    if (source.Text.ToLower() != "Pending".ToLower() && source.Text.ToLower() != "Pendiente".ToLower() && source.Text.ToLower() != "Rejected".ToLower() && source.Text.ToLower() != "Rechazada".ToLower())
                    {
                        cboStartHour.Enabled = false;
                        cboEndHour.Enabled = false;
                        dtpOverTimeDate.Enabled = false;
                        txtJustificationForExtraTime.Enabled = false;

                    }
                    else
                    {
                        cboStartHour.Enabled = true;
                        cboEndHour.Enabled = true;
                        dtpOverTimeDate.Enabled = true;
                        txtJustificationForExtraTime.Enabled = true;

                    }
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msjRecordNotFound")));
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
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }

        }

    }
}