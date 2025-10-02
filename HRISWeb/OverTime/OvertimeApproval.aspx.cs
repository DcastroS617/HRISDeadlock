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
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;

namespace HRISWeb.Overtime
{
    public partial class OvertimeApproval : System.Web.UI.Page
    {
        [Dependency]
        public IUsersBll<UserEntity> usersBll { get; set; }
        [Dependency]
        public IOverTimeRecordsBLL<OverTimeRecordsEntity> overTimeRecordsBLL  { get; set; }
        [Dependency]
        public IDaysDetailBLL<DaysDetailEntity> daysDetailBLL { get; set; }
        [Dependency]
        public IOverTimeStatusBLL<OverTimeStatusEntity> overtimeStatusBLL { get; set; }

        [Dependency]
        public IEmployeesBll<EmployeeEntity> employeesBll  { get; set; }

        [Dependency]
        public IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL { get; set; }
        [Dependency]
        public IOvertimeCompaniesBLL<OvertimeCompaniesEntity> objOvertimeCompanies { get; set; }

        [Dependency]
        public IOvertimeClassificationBLL<OvertimeClassificationEntity> objOvertimeClassification { get; set; }

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
                if (!IsPostBack)
                {
                    SearchResults(1);
                    BindOvertimeStatusDropdown("cboOvertimeStatu");
                    BindOvertimeStatusDropdown("cboApprovalType");
                    BindDateTypeDropdown();
                    BindValidatorsDropdown();
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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtOverTimeNumber.Text = "0";
                txtJustificationForExtraTime.Text = string.Empty;
                chkIsExtraHour.Checked = false;
                dtpOverTimeDate.Text = string.Empty;
                txtApprovalRemark.Text = String.Empty;
                BindEmployeeDropdown();
                BindHoursDropDown(cboStartHour);
                BindHoursDropDown(cboEndHour);
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
        /// <summary>
        /// Bind Employee Dropdown
        /// </summary>
        private void BindEmployeeDropdown()
        {
            try
            {
                string geoDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                var entityEmployee = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);
                List<EmployeeEntity> employees = employeesBll.ListByDivisionByDepartment(divisionCode, geoDivisionCode, entityEmployee.DepartmentCode);
                employees.Insert(0, new EmployeeEntity { EmployeeCode = "", EmployeeName = "" });
                cboEmployeeCode.DataSource = employees;
                cboEmployeeCode.DataValueField = "EmployeeCode";
                cboEmployeeCode.DataTextField = "EmployeeName";
                cboEmployeeCode.DataBind();
                cboEmployeeCode.SelectedIndex = 0;
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
        /// <summary>
        /// Bind Overtime Status Dropdown
        /// </summary>
        /// <param name="searchcbo">pass dropdown name for cboOvertimeStatu and cboOverTimeStatusCode</param>
        private void BindOvertimeStatusDropdown(string searchcbo = "")
        {
            List<OverTimeStatusEntity> overTimeStatuses = new List<OverTimeStatusEntity>();
            try
            {
                overTimeStatuses.Add(new OverTimeStatusEntity() { OverTimeStatusCode = 0, OverTimeStatusName = "" });
                overTimeStatuses.AddRange(overtimeStatusBLL.GetOverTimeStatusList());

                if (searchcbo == "cboOvertimeStatu")
                {
                    cboOvertimeStatu.DataSource = overTimeStatuses;
                    cboOvertimeStatu.DataValueField = "OverTimeStatusCode";
                    cboOvertimeStatu.DataTextField = "OverTimeStatusName";
                    cboOvertimeStatu.DataBind();
                    cboOvertimeStatu.SelectedIndex = 0;
                }
                else if (searchcbo == "cboApprovalType")
                {
                    cboApprovalType.DataSource = overTimeStatuses.Where(x => x.OverTimeStatusName != "Pending");
                    cboApprovalType.DataValueField = "OverTimeStatusCode";
                    cboApprovalType.DataTextField = "OverTimeStatusName";
                    cboApprovalType.DataBind();
                    cboApprovalType.SelectedIndex = 0;
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
        /// <summary>
        /// Bind DateType Dropdown
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
        /// <summary>
        /// Bind Validators Dropdown
        /// </summary>
        private void BindValidatorsDropdown()
        {
            List<EmployeeEntity> employees = new List<EmployeeEntity>();
            try
            {
                UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                OverTimeEmployeeView overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);

                /*DESCOMENTAR*/
                if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                {
                    int validatorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ValidatorCode"]);
                    List<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeEntity = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(new RolesByDepartmentEmployeeEntity 
                    { 
                        GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , RoleApproversCode = validatorCode
                        , DepartmentCode = overTimeEmployeeView.DepartmentCode 
                    });
                    if (rolesByDepartmentEmployeeEntity != null && rolesByDepartmentEmployeeEntity.Count() > 0)
                    {
                        string geoDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                        int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        employees = employeesBll.ListByDivision(divisionCode, geoDivisionCode);
                        employees = employees.Where(x => rolesByDepartmentEmployeeEntity.Any(y => y.EmployeeCode == x.EmployeeCode)).ToList();
                        btnAdd.Visible = true;
                        btnValidators.Visible = true;
                        cboValidators.Enabled = true;
                    }
                    else
                    {
                        cboValidators.Enabled = false;
                        btnAdd.Visible = false;
                        btnValidators.Visible = false;

                        //Se debe informar que no es admin y redirigirlo al index main
                        // Response.Redirect("Overtime/RolesByDepartmentEmployeList.aspx");
                        PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), new Exception());
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                    }
                }

                employees.Insert(0, new EmployeeEntity() { EmployeeCode = "", EmployeeName = "" });

                cboValidators.DataSource = employees;
                cboValidators.DataValueField = "EmployeeCode";
                cboValidators.DataTextField = "EmployeeName";
                cboValidators.DataBind();
                cboValidators.SelectedIndex = 0;
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
                    BindOvertimeStatusDropdown();
                    int selectedOverTimeNumber = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayUserInformation(selectedOverTimeNumber);

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
        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                int overtimeNumber = string.IsNullOrEmpty(txtOverTimeNumber.Text) ? 0 : Convert.ToInt32(txtOverTimeNumber.Text);
                string startHour = cboStartHour.SelectedValue;
                string endHour = cboEndHour.SelectedValue;
                if (!string.IsNullOrEmpty(endHour) && !string.IsNullOrEmpty(startHour) && ((Convert.ToDateTime(endHour).TimeOfDay) >= (Convert.ToDateTime(startHour).TimeOfDay)))
                {
                    DateTime overtimeDateOnly = DateTime.ParseExact(String.Format("{0} 00:00", dtpOverTimeDate.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    if (overtimeDateOnly.DayOfWeek.ToString() != "Saturday" && overtimeDateOnly.DayOfWeek.ToString() != "Sunday")
                    {
                        if (daysDetailBLL.GetDaysDetailByDate(new DaysDetailEntity { GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, CodeDateBase = Convert.ToDateTime(overtimeDateOnly) }).Count <= 0 )
                        {
                            string geoDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                            int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                            int overtimeStatusCode = Convert.ToInt32(ConfigurationManager.AppSettings["ApprovedOvertimeStatusCode"]);
                            string justification = txtJustificationForExtraTime.Text;

                            bool isSuccess = false;
                            bool isExtraHour = false;
                            var overtimeRecordsList = overTimeRecordsBLL.GetOvertimeRecordsByFilters(new OverTimeRecordsEntity
                            {
                                GeographicDivisionCode = geoDivisionCode,
                                DivisionCode = divisionCode,
                                EmployeeCode = cboEmployeeCode.SelectedValue,
                                OvertimeDate = overtimeDateOnly.Date
                            });
                            var employeeEntity = employeesBll.ListByEmployeeCodeGeographicDivision(cboEmployeeCode.SelectedValue,geoDivisionCode);

                            if (overtimeRecordsList.Count > 0)
                            {
                                var records = new List<OverTimeRecordsEntity>();
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
                                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjDuplicateRecord")));
                                    return;
                                }
                                else
                                {
                                    isExtraHour = true;
                                    chkIsExtraHour.Checked = true;
                                    if (overtimeNumber.Equals(0) && string.IsNullOrEmpty(justification))
                                    {
                                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjJustificationRequired")));
                                        return;
                                    }
                                }
                            }
                            OverTimeRecordsEntity overtimeRecords = new OverTimeRecordsEntity
                            {
                                OverTimeNumber = overtimeNumber,
                                DivisionCode = divisionCode,
                                EmployeeCode = cboEmployeeCode.SelectedValue,
                                StartHour = startHour,
                                EndHour = endHour,
                                GeographicDivisionCode = geoDivisionCode,
                                JustificationForExtraTime = justification,
                                LastModifiedDate = DateTime.Now,
                                LastModifiedUser = lastModifiedUser,
                                CreatedBy = lastModifiedUser,
                                OverTimeStatusCode = overtimeStatusCode,
                                OvertimeCreatedDate = DateTime.Now,
                                CompanieCode= employeeEntity.CompanyID,
                                DepartmentCode= employeeEntity.DepartmentCode,
                                OvertimeDate = Convert.ToDateTime(overtimeDateOnly.Date.ToString("yyyy-MM-dd")),
                                IsExtraHour = isExtraHour,
                                ApprovalRemark = txtApprovalRemark.Text
                            };

                            if (overtimeNumber.Equals(0))
                            {
                                isSuccess = overTimeRecordsBLL.AddOverTimeRecord(overtimeRecords);
                            }
                            else
                            {
                                isSuccess = overTimeRecordsBLL.UpdateOverTimeRecord(overtimeRecords);
                            }

                            if (isSuccess)
                            {
                                SearchResults(PagerUtil.GetActivePage(blstPager));

                                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#MaintenanceDialog').modal('hide')", true);
                                return;

                            }
                        }
                        else
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgHoliday")));
                        }
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgRestDay")));
                    }
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgHourValidation")));
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
        /// <summary>
        /// Handles the btnSave click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            try
            {
                btnSave.Disabled = true;
                bool isSuccess = false;
                int overtimeStatusCode = cboApprovalType.SelectedValue == null ? 0 : Convert.ToInt32(cboApprovalType.SelectedValue);
                int rejectedOvertimeStatusCode = Convert.ToInt32(ConfigurationManager.AppSettings["RejectedOvertimeStatusCode"]);

                if (cboApprovalType.SelectedIndex > 0)
                {
                    if (rejectedOvertimeStatusCode == overtimeStatusCode && string.IsNullOrEmpty(txtRemark.Text))
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgApprovalRemarkReired")));
                    }
                    else
                    {
                        int pendingByValidatorsOvertimeStatusCode = Convert.ToInt32(ConfigurationManager.AppSettings["PendingByValidatorsOvertimeStatusCode"]);
                        if (pendingByValidatorsOvertimeStatusCode == overtimeStatusCode && cboValidators.SelectedIndex == 0)

                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgValidtorRequired")));
                            return;
                        }
                        string geoDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                        int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        var users = usersBll.ListByDivisionCode(divisionCode);

                        bool sendEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["ValidatorEmail"]);

                        foreach (GridViewRow gvrow in grvList.Rows)
                        {
                            var checkbox = gvrow.FindControl("chkrow") as CheckBox;
                            if (checkbox.Checked)
                            {
                                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                                int overtimeNumber = string.IsNullOrEmpty(checkbox.Text) ? 0 : Convert.ToInt32(checkbox.Text);
                                
                                string approvalRemark = txtRemark.Text;

                                OverTimeRecordsEntity overtimeRecords = new OverTimeRecordsEntity
                                {
                                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                    DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                                    OverTimeNumber = overtimeNumber,
                                    ApprovalRemark = approvalRemark,
                                    LastModifiedDate = DateTime.Now,
                                    LastModifiedUser = lastModifiedUser,
                                    CreatedBy = lastModifiedUser,
                                    OverTimeStatusCode = overtimeStatusCode,
                                    AssignTo = cboValidators.Text
                                };

                                if (pendingByValidatorsOvertimeStatusCode == overtimeStatusCode)
                                {
                                    overtimeRecords.AssignToUser = cboValidators.SelectedValue;
                                }

                                isSuccess = overTimeRecordsBLL.UpdateOverTimeRecordStatus(overtimeRecords);
                                if (isSuccess)
                                {
                                    overTimeRecordsBLL.AddOverTimeApprovalLog(overtimeRecords);
                                    if (sendEmail && !string.IsNullOrEmpty(overtimeRecords.AssignToUser))
                                    {
                                        //var employees = employeesBll.ListByDivision(divisionCode, geoDivisionCode);
                                        //if (employees.Any(x => x.EmployeeCode == overtimeRecords.AssignToUser))
                                        //{
                                        //    string email = employees.FirstOrDefault(x => x.EmployeeCode == overtimeRecords.AssignToUser).Email;
                                        //    string body = $"You have new approval request from Supervisor for Overtime Number: {overtimeRecords.OverTimeNumber}.";
                                        //    overTimeRecordsBLL.SendEmail("Approval Request By Supervisor", body, new List<string> { email });
                                        //}
                                    }
                                }
                            }
                        }
                        if (isSuccess)
                        {
                            SearchResults(PagerUtil.GetActivePage(blstPager));
                            cboApprovalType.SelectedIndex = 0;
                            txtRemark.Text = "";
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                             return;
                        }
                        else
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgSelectRecord")));
                        }
                    }

                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msgApprovalType")));
                }
            }
            catch (Exception ex)
            {
                btnSave.Disabled = false;
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
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFrombtnSaveClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFrombtnSaveClickPostBackError(); }}, 200);", true);
            }
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
                btnSave.Disabled = false;
            }
        }
        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<OverTimeRecordsEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            if (overTimeRecordsBLL == null)
            {
                overTimeRecordsBLL = Application.GetContainer().Resolve<IOverTimeRecordsBLL<OverTimeRecordsEntity>>();
            }
            if (IsNullOrEmpty(dtpOvertimeDateFrom.Text))
            {
                dtpOvertimeDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                dtpOvertimeDateto.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }
            string assignTo = string.Empty;

            int dateType = string.IsNullOrEmpty(cboDateType.SelectedValue) ? 1 : cboDateType.SelectedIndex;
            DateTime dtpStartDate;
            DateTime.TryParseExact(dtpOvertimeDateFrom.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtpStartDate);

            DateTime dtpEndDate;
            DateTime.TryParseExact(dtpOvertimeDateto.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtpEndDate);

            string employeeCode = string.Empty;
            UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
            OverTimeEmployeeView overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);
            if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
            {
                var superVisors = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(new RolesByDepartmentEmployeeEntity { DepartmentCode = overTimeEmployeeView.DepartmentCode });
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
                PageHelper<OverTimeRecordsEntity> pageHelper = overTimeRecordsBLL.GetOverTimeRecordList(
                 SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
               , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
               , employeeCode
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
               ,assignTo, overTimeEmployeeView.DepartmentCode);
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
        /// <summary>
        /// Displays on modal the user information
        /// </summary>
        /// <param name="userCode">The user code to search</param>
        private void DisplayUserInformation(int overTimeNumber)
        {
            try
            {
                var overTimeRecords = overTimeRecordsBLL.GetOverTimeRecordByOverTimeNumber(overTimeNumber);
                if (overTimeRecords != null)
                {
                    txtOverTimeNumber.Text = overTimeNumber.ToString();
                    cboStartHour.SelectedValue = overTimeRecords.StartHour.ToString();
                    cboEndHour.Text = overTimeRecords.EndHour.ToString();
                    txtJustificationForExtraTime.Text = overTimeRecords.JustificationForExtraTime.ToString();
                    dtpOverTimeDate.Text = overTimeRecords.OvertimeDate.ToString("MM/dd/yyyy HH:mm");
                    ddlDivisionCode.Value = overTimeRecords.DivisionCode.ToString();
                    cboEmployeeCode.SelectedValue = overTimeRecords.EmployeeCode.ToString();
                    ddlGeographicDivisionCode.Value = overTimeRecords.GeographicDivisionCode.ToString();
                    cboOverTimeStatusCode.SelectedValue = overTimeRecords.OverTimeStatusCode.ToString();
                    chkIsExtraHour.Checked = overTimeRecords.IsExtraHour;
                    txtApprovalRemark.Text = string.Empty;
                    //ddlOverTimeStatusCode.Items.FindByValue(overTimeRecords.OverTimeStatusCode.ToString()).Selected = true;

                    var source = cboOverTimeStatusCode.Items.FindByValue(overTimeRecords.OverTimeStatusCode.ToString());

                    dtpOverTimeDate.Enabled = false;
                    txtJustificationForExtraTime.Enabled = false;
                    cboOverTimeStatusCode.Enabled = true;
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
        /// <summary>
        /// Bind Hours DropDown
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
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

    }
}