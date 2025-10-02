using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Unity.Web;
using Unity;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using DOLE.HRIS.Exceptions;
using static System.String;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity.Attributes;
using System.Configuration;
using System.Web.Helpers;

namespace HRISWeb.Overtime
{
    public partial class WorkingTimeHoursByDepartmentEmployeesList : System.Web.UI.Page
    {
        [Dependency]
        public IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL { get; set; }
        [Dependency]
        public IWorkingTimeHoursByDepartmentEmployeesBLL<WorkingTimeHoursByDepartmentEmployeesEntity> workingTimeHoursByDepartmentEmployeesBLL { get; set; }
        [Dependency]
        public IWorkingTimeTypesBLL<WorkingTimeTypesEntity> workingTimeTypesBLL { get; set; }
        [Dependency]
        public IDaysWeekBLL<DaysWeekEntity> daysWeekBll { get; set; }
        [Dependency]
        public IEmployeesBll<EmployeeEntity> employeesBll { get; set; }

        public List<DaysWeekEntity> listDaysWeek { get; set; }

        //session key for the results
        readonly string sessionKeyListDayResults = "Overtime-ListDaysResults";

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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDaysWeek();
                    SearchResults(1);
                    BindDepartments(cboDepartmentId);
                    BindHoursDropDown(cboStartHour);
                    BindHoursDropDown(cboEndHour);
                    BindHoursDropDown(cboSecondStartHour);
                    BindHoursDropDown(cboSecondEndHour);
                    BindWorkingTimeTypes(cboWorkingTimeTypeCode);

                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        private void BindDepartments(DropDownList dropDownList)
        {
            List<Departments> departments = new List<Departments>();

            try
            {
                departments.Add(new Departments() { DepartmentCode = "", Department = "" });
                departments = rolesByDepartmentEmployeeBLL.GetDepartmentListForDropdown().OrderBy(x => x.Department).ToList();
                departments.Insert(0, new Departments { DepartmentCode = "", Department = "" });
                dropDownList.DataSource = departments;
                dropDownList.DataValueField = "DepartmentCode";
                dropDownList.DataTextField = "Department";
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
        }
        private void BindWorkingTimeTypes(DropDownList dropDownList)
        {
            List<WorkingTimeTypesEntity> workingTimeTypesEntities = new List<WorkingTimeTypesEntity>();

            try
            {
                workingTimeTypesEntities.Add(new WorkingTimeTypesEntity() { WorkingTimeTypeCode = 0, WorkingTimeTypeName = "" });
                workingTimeTypesEntities.AddRange(workingTimeTypesBLL.GetWorkingTimeTypesListForDropdown());
                dropDownList.DataSource = workingTimeTypesEntities;
                dropDownList.DataValueField = "WorkingTimeTypeCode";
                dropDownList.DataTextField = "WorkingTimeTypeName";
                dropDownList.DataBind();

                var listJson = Json.Encode(workingTimeTypesEntities);
                string script = string.Format("localStorage.WorkingType= '{0}';", listJson);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "WorkingType", script, true);


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
        private void BindEmployeeDropdown(string departmentCode)
        {
            try
            {
                List<EmployeeEntity> employees = new List<EmployeeEntity>();
                if (!string.IsNullOrEmpty(departmentCode))
                {
                    string geoDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                    int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                    employees = employeesBll.ListByDivisionByDepartment(divisionCode, geoDivisionCode, departmentCode);
                }
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
        }
        private void BindDaysWeek()
        {
            try
            {
                listDaysWeek = daysWeekBll.ListEnabled();
                Session[sessionKeyListDayResults] = listDaysWeek;
                CultureInfo currentCulture = GetCurrentCulture();

                cboRestDay.Items.AddRange(listDaysWeek.OrderBy(f => f.OrderList)
                    .Select(d => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? d.DaysWeekDescriptionSpanish : d.DaysWeekDescriptionEnglish
                        , d.DaysWeekCode.ToString())).ToArray());
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
        protected void btnCleanFilters_Click(object sender, EventArgs e)
        {
            txtEmployeeId.Text = "";
            cboDepartmentId.SelectedIndex = 0;
            SearchResults(PagerUtil.GetActivePage(blstPager));
        }
        protected void btnSearchFilter_ServerClick(object sender, EventArgs e)
        {
            SearchResults(1);
            hdfSelectedRowIndex.Value = "-1";
        }
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                cboDepartmentCode.Enabled = true;
                cboEmployeeCode.Enabled = true;
                BindDepartments(cboDepartmentCode);
                BindEmployeeDropdown(null);
                BindWorkingTimeTypes(cboWorkingTimeTypeCode);
                txtWorkingTimeHoursByDepartmentEmployeesID.Text = "0";
                cboDepartmentCode.SelectedIndex = 0;
                cboEmployeeCode.SelectedIndex = 0;
                dtpStartDate.Text = "";
                dtpEndDate.Text = "";
                cboStartHour.SelectedIndex = 0;
                cboEndHour.SelectedIndex = 0;
                cboSecondStartHour.SelectedIndex = 0;
                cboSecondEndHour.SelectedIndex = 0;
                cboWorkingTimeTypeCode.SelectedIndex = 0;
                hdnAllDays.Value = string.Empty;
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
                ScriptManager.RegisterStartupScript(this
                   , this.GetType()
                   , "ReturnAddServerClicks" + Guid.NewGuid().ToString()
                   , "setTimeout(function () { RestoreSelectedRestDay(); }, 200);", true);
            }
        }
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployeesEntity = new WorkingTimeHoursByDepartmentEmployeesEntity();
            try
            {
                
                bool isAddUpdate = false;
                workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeHoursByDepartmentEmployeesID = string.IsNullOrEmpty(txtWorkingTimeHoursByDepartmentEmployeesID.Text) ? 0 : Convert.ToInt32(txtWorkingTimeHoursByDepartmentEmployeesID.Text);
                workingTimeHoursByDepartmentEmployeesEntity.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                workingTimeHoursByDepartmentEmployeesEntity.DepartmentCode = string.IsNullOrEmpty(cboDepartmentCode.SelectedValue) ? "" : cboDepartmentCode.SelectedValue;
                workingTimeHoursByDepartmentEmployeesEntity.EmployeeCode = string.IsNullOrEmpty(cboEmployeeCode.SelectedValue) ? "" : cboEmployeeCode.SelectedValue;
                workingTimeHoursByDepartmentEmployeesEntity.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                workingTimeHoursByDepartmentEmployeesEntity.StartDate = string.IsNullOrEmpty(dtpStartDate.Text) ? (DateTime?)null : DateTime.ParseExact(String.Format("{0} 00:00", dtpStartDate.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                workingTimeHoursByDepartmentEmployeesEntity.EndDate = string.IsNullOrEmpty(dtpEndDate.Text) ? (DateTime?)null : DateTime.ParseExact(String.Format("{0} 00:00", dtpEndDate.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                workingTimeHoursByDepartmentEmployeesEntity.StartHour = cboStartHour.SelectedValue;
                workingTimeHoursByDepartmentEmployeesEntity.EndHour = cboEndHour.SelectedValue;
                workingTimeHoursByDepartmentEmployeesEntity.SecondStartHour = cboSecondStartHour.SelectedValue;
                workingTimeHoursByDepartmentEmployeesEntity.SecondEndHour = cboSecondEndHour.SelectedValue;
                workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeTypeCode = string.IsNullOrEmpty(cboWorkingTimeTypeCode.SelectedValue) ? 0 : Convert.ToInt32(cboWorkingTimeTypeCode.SelectedValue);
                workingTimeHoursByDepartmentEmployeesEntity.RestDay = hdnAllDays.Value;

                if (workingTimeHoursByDepartmentEmployeesEntity.StartDate > workingTimeHoursByDepartmentEmployeesEntity.EndDate)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjStartDateValidation")));
                    return; 
                }

                if (Convert.ToDateTime(cboStartHour.SelectedValue).TimeOfDay >= Convert.ToDateTime(cboEndHour.SelectedValue).TimeOfDay)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjStartEndFirstHourValidation")));
                    return;
                }

                if (!string.IsNullOrEmpty(cboSecondStartHour.SelectedValue) && !string.IsNullOrEmpty(cboSecondEndHour.SelectedValue) && Convert.ToDateTime(cboSecondStartHour.SelectedValue).TimeOfDay >= Convert.ToDateTime(cboSecondEndHour.SelectedValue).TimeOfDay)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjStartEndSecondHourValidation")));
                    return;
                }

                if (!string.IsNullOrEmpty(cboStartHour.SelectedValue) && !string.IsNullOrEmpty(cboEndHour.SelectedValue) && !string.IsNullOrEmpty(cboSecondStartHour.SelectedValue) && !string.IsNullOrEmpty(cboSecondEndHour.SelectedValue))
                {
                    WorkingTimeTypesEntity workingTimeTypesEntity = workingTimeTypesBLL.GetWorkingTimeTypesByWorkingTimeTypeCode(workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeTypeCode);
                    if (workingTimeTypesEntity != null && workingTimeTypesEntity.TotalWorkingTime > 0)
                    {
                        TimeSpan startHour = !string.IsNullOrEmpty(cboStartHour.SelectedValue) ? Convert.ToDateTime(cboStartHour.SelectedValue).TimeOfDay : TimeSpan.FromSeconds(0);
                        TimeSpan endHour = !string.IsNullOrEmpty(cboEndHour.SelectedValue) ? Convert.ToDateTime(cboEndHour.SelectedValue).TimeOfDay : TimeSpan.FromSeconds(0);
                        TimeSpan secondStartHour = !string.IsNullOrEmpty(cboSecondStartHour.SelectedValue) ? Convert.ToDateTime(cboSecondStartHour.SelectedValue).TimeOfDay : TimeSpan.FromSeconds(0);
                        TimeSpan secondEndtHour = !string.IsNullOrEmpty(cboSecondEndHour.SelectedValue) ? Convert.ToDateTime(cboSecondEndHour.SelectedValue).TimeOfDay : TimeSpan.FromSeconds(0);
                        TimeSpan totalWokingHours = (endHour - startHour) + (secondEndtHour - secondStartHour);
                        TimeSpan workingHours = TimeSpan.FromHours(workingTimeTypesEntity.TotalWorkingTime);
                        if (totalWokingHours <= workingHours)
                        {
                            if (startHour < endHour && startHour < secondStartHour && startHour < secondEndtHour && endHour < secondStartHour && endHour < secondEndtHour && secondStartHour < secondEndtHour)
                            {
                                workingTimeHoursByDepartmentEmployeesEntity.SecondStartHour = cboSecondStartHour.SelectedValue;
                                workingTimeHoursByDepartmentEmployeesEntity.SecondEndHour = cboSecondEndHour.SelectedValue;
                                isAddUpdate = true;
                            }
                            else
                            {
                                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkHours")));
                            }
                        }
                        else
                        {
                            string message = Convert.ToString(GetLocalResourceObject("msgWorkHoursgreter"));
                            message = message.Replace("{workingHours}", totalWokingHours.ToString()).Replace("{totalWokingHours}", workingHours.ToString());
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, message);
                        }
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgWorkHoursnotfound")));
                    }
                }
                else
                {
                    workingTimeHoursByDepartmentEmployeesEntity.SecondStartHour = null;
                    workingTimeHoursByDepartmentEmployeesEntity.SecondEndHour = null;
                    isAddUpdate = true;
                }

                bool isSuccess = false;

                if (isAddUpdate)
                {
                    if (workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeHoursByDepartmentEmployeesID.Equals(0))
                    {
                        workingTimeHoursByDepartmentEmployeesEntity.CreatedBy = UserHelper.GetCurrentFullUserName;
                        isSuccess = workingTimeHoursByDepartmentEmployeesBLL.AddWorkingTimeHoursByDepartmentEmployees(workingTimeHoursByDepartmentEmployeesEntity);                        
                    }
                    else
                    {
                        workingTimeHoursByDepartmentEmployeesEntity.LastModifiedUser = UserHelper.GetCurrentFullUserName;
                        isSuccess = workingTimeHoursByDepartmentEmployeesBLL.UpdateWorkingTimeHoursByDepartmentEmployees(workingTimeHoursByDepartmentEmployeesEntity);
                    }

                    if (isSuccess)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#MaintenanceDialog').modal('hide')", true);
                        hdnAllDays.Value = string.Empty;
                        return;
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgjAddUpdateFailed")));
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
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
            }
            finally
            {

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                
            }
        }
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int workingTimeHoursByDepartmentEmployeesID = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayWorkingTimeHoursByDepartmentEmployeesInformation(workingTimeHoursByDepartmentEmployeesID);

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
                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnEditServerClicks" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { RestoreSelectedRestDay(); CalcularHoras(); }, 200);", true);
            }
        }
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {

                var SelectedRow = grvList.DataKeys[int.Parse(hdfSelectedRowIndex.Value)];
                var code = Convert.ToInt32(SelectedRow.Value.ToString());
                var result = workingTimeHoursByDepartmentEmployeesBLL.DeleteWorkingTimeHoursByDepartmentEmployees(code);

                if (result)
                {
                    PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

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
        private void DisplayWorkingTimeHoursByDepartmentEmployeesInformation(int workingTimeHoursByDepartmentEmployeesID)
        {
            try
            {
                WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployeesEntity = workingTimeHoursByDepartmentEmployeesBLL.GetWorkingTimeHoursByDepartmentEmployeesById(workingTimeHoursByDepartmentEmployeesID);
                if (workingTimeHoursByDepartmentEmployeesEntity != null)
                {
                    BindDepartments(cboDepartmentCode);
                    BindEmployeeDropdown(workingTimeHoursByDepartmentEmployeesEntity.DepartmentCode);
                    BindWorkingTimeTypes(cboWorkingTimeTypeCode);
                    cboDepartmentCode.Enabled = false;
                    cboEmployeeCode.Enabled = false;
                    txtWorkingTimeHoursByDepartmentEmployeesID.Text = workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeHoursByDepartmentEmployeesID.ToString();
                    cboDepartmentCode.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.DepartmentCode;
                    if (cboEmployeeCode.Items.Count > 1)
                    {
                        cboEmployeeCode.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.EmployeeCode;
                    }
                    dtpStartDate.Text = workingTimeHoursByDepartmentEmployeesEntity.StartDate != (DateTime?)null ? workingTimeHoursByDepartmentEmployeesEntity.StartDate.Value.ToString("MM/dd/yyyy HH:mm") : string.Empty;
                    dtpEndDate.Text = workingTimeHoursByDepartmentEmployeesEntity.EndDate != (DateTime?)null ? workingTimeHoursByDepartmentEmployeesEntity.EndDate.Value.ToString("MM/dd/yyyy HH:mm") : string.Empty;
                    cboStartHour.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.StartHour;
                    cboEndHour.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.EndHour;
                    cboSecondStartHour.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.SecondStartHour;
                    cboSecondEndHour.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.SecondEndHour;
                    cboWorkingTimeTypeCode.SelectedValue = workingTimeHoursByDepartmentEmployeesEntity.WorkingTimeTypeCode.ToString();
                    hdnAllDays.Value = workingTimeHoursByDepartmentEmployeesEntity.RestDay;
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
        /// This is commen method for all days checkbox checked or unchecked
        /// </summary>
        /// <param name="isChecked">true or false</param>
        /// <summary>
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            if (Session[Constants.cCulture] != null)
            {
                return new CultureInfo(Convert.ToString(Session[Constants.cCulture]));
            }
            return new CultureInfo(Constants.cCultureEsCR);
        }
        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            listDaysWeek = (List<DaysWeekEntity>)Session[sessionKeyListDayResults];
            //
            if (workingTimeHoursByDepartmentEmployeesBLL == null)
            {
                workingTimeHoursByDepartmentEmployeesBLL = Application.GetContainer().Resolve<IWorkingTimeHoursByDepartmentEmployeesBLL<WorkingTimeHoursByDepartmentEmployeesEntity>>();
            }
            PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> pageHelper = workingTimeHoursByDepartmentEmployeesBLL.GetWorkingTimeHoursByDepartmentEmployeesList(
                  SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , string.IsNullOrEmpty(cboDepartmentId.SelectedValue) ? "" : cboDepartmentId.SelectedValue
                , txtEmployeeId.Text
                , sortExpression
                , sortDirection
                , page
                , pageSizeValue); ;
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
            CultureInfo currentCulture = GetCurrentCulture();
            foreach (var item in pageHelper.ResultList)
            {
                if (!String.IsNullOrEmpty(item.RestDay))
                {
                    var days = item.RestDay.Split(',');
                    if (days.Count() > 0)
                    {
                        item.RestDayDescription = string.Empty;
                        foreach (var day in days)
                        {
                            item.RestDayDescription += listDaysWeek.Where(p => p.DaysWeekCode == Convert.ToInt32(day))
                                .Select(t => currentCulture.Name.Equals(Constants.cCultureEsCR)
                            ? t.DaysWeekDescriptionSpanish : t.DaysWeekDescriptionEnglish).FirstOrDefault() + ",";
                        }
                        item.RestDayDescription = item.RestDayDescription.Substring(0, item.RestDayDescription.Count() - 1);
                    }
                }
            }
            DisplayResults(pageHelper);

            return pageHelper;
        }
        private void DisplayResults(PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            // htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
        }
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
        protected void grvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);
            SearchResults(PagerUtil.GetActivePage(blstPager));
        }
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
        /// Handles the cboDepartmentCode Selected Index Changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboDepartmentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDownList = (DropDownList)sender;
            BindEmployeeDropdown(dropDownList.SelectedValue);
            ScriptManager.RegisterStartupScript(this
                   , this.GetType()
                   , "cboDepartmentCode_SelectedIndexChanged" + Guid.NewGuid().ToString()
                   , "setTimeout(function () { RestoreSelectedRestDay(); }, 200);", true);

        }

        /// <summary>
        /// Handles the chkAllday Checked Changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void chkAllday_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                hdnAllDays.Value += $",{checkBox.Text}";
            }
            else
            {
                hdnAllDays.Value = hdnAllDays.Value.Replace($",{checkBox.Text}", string.Empty);
                if (hdnAllDays.Value.Length == $",{checkBox.Text}".Length)
                {
                    hdnAllDays.Value = string.Empty;
                }
            }
        }
    }
}