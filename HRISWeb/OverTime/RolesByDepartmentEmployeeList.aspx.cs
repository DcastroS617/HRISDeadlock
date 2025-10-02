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
    public partial class RolesByDepartmentEmployeeList : System.Web.UI.Page
    {
        [Dependency]
        public IUsersBll<UserEntity> usersBll { get; set; }
        [Dependency]
        public IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL { get; set; }
        [Dependency]
        public IEmployeesBll<EmployeeEntity> employeesBll { get; set; }
        [Dependency]
        public IOverTimeRecordsBLL<OverTimeRecordsEntity> overTimeRecordsBLL { get; set; }

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
                    UserEntity userEntity = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                    OverTimeEmployeeView overTimeEmployeeView = overTimeRecordsBLL.GetOverTimeEmployee(userEntity.ActiveDirectoryUserAccount);
                    if (overTimeEmployeeView != null && !string.IsNullOrEmpty(overTimeEmployeeView.EmployeeCode))
                    {
                        int superVisorcode = Convert.ToInt32(ConfigurationManager.AppSettings["SupervisorCode"]);
                        var superVisors = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(new RolesByDepartmentEmployeeEntity { RoleApproversCode = superVisorcode, DepartmentCode = overTimeEmployeeView.DepartmentCode, EmployeeCode = overTimeEmployeeView.EmployeeCode });
                        if (superVisors != null && superVisors.Count() > 0)
                        {
                            SearchResults(1);
                            BindDepartments(cboDepartmentId);
                            BindDepartments(cboDepartmentCode);
                            BindRoleApprovers(cboRoleApproversCode);
                        }
                        else
                        {
                            //Se debe informar que no es admin y redirigirlo al index main
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjAdminValidation")), "RedirectHome");
                        }
                    }
                    else
                    {
                        //Se debe informar que no es admin y redirigirlo al index main
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjAdminValidation")), "RedirectHome");
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
        /// Bind Departments DropDown
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
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
            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }
        /// <summary>
        /// Bind Role Approvers DropDown
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
        private void BindRoleApprovers(DropDownList dropDownList)
        {
            List<RoleApproversEntity> roleApproversEntities = new List<RoleApproversEntity>();

            try
            {
                roleApproversEntities.Add(new RoleApproversEntity() { RoleApproversCode = 0, RoleApproversDescription = "" });
                roleApproversEntities.AddRange(rolesByDepartmentEmployeeBLL.GetRoleApproversListForDropdown());
                dropDownList.DataSource = roleApproversEntities;
                dropDownList.DataValueField = "RoleApproversCode";
                dropDownList.DataTextField = "RoleApproversDescription";
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
        /// <summary>
        /// Bind Employee Dropdown
        /// </summary>
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
        /// <summary>
        /// Handles the cboDepartmentCode Selected Index Changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboDepartmentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDownList = (DropDownList)sender;
            BindEmployeeDropdown(dropDownList.SelectedValue);
        }
        /// <summary>
        /// Handles the btnCleanFilters click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnCleanFilters_Click(object sender, EventArgs e)
        {
            txtEmployeeId.Text = "";
            cboDepartmentId.SelectedIndex = 0;
            SearchResults(PagerUtil.GetActivePage(blstPager));
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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                BindEmployeeDropdown(null);
                cboDepartmentCode.Enabled = true;
                cboEmployeeCode.Enabled = true;
                txtRolesByDepartmentEmployeeID.Text = "0";
                cboDepartmentCode.SelectedIndex = 0;
                cboEmployeeCode.SelectedIndex = 0;
                cboRoleApproversCode.SelectedIndex = 0;
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            RolesByDepartmentEmployeeEntity rolesByDepartmentEmployeeEntity = new RolesByDepartmentEmployeeEntity();
            try
            {
                
                rolesByDepartmentEmployeeEntity.RolesByDepartmentEmployeeID = string.IsNullOrEmpty(txtRolesByDepartmentEmployeeID.Text) ? 0 : Convert.ToInt32(txtRolesByDepartmentEmployeeID.Text);
                rolesByDepartmentEmployeeEntity.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                rolesByDepartmentEmployeeEntity.DepartmentCode = string.IsNullOrEmpty(cboDepartmentCode.SelectedValue) ? "" : cboDepartmentCode.SelectedValue;
                rolesByDepartmentEmployeeEntity.EmployeeCode = string.IsNullOrEmpty(cboEmployeeCode.SelectedValue) ? "" : cboEmployeeCode.SelectedValue;
                rolesByDepartmentEmployeeEntity.RoleApproversCode = string.IsNullOrEmpty(cboRoleApproversCode.SelectedValue) ? 0 : Convert.ToInt32(cboRoleApproversCode.Text);
                bool isSuccess = false;

                if (rolesByDepartmentEmployeeEntity.RolesByDepartmentEmployeeID.Equals(0))
                {
                    rolesByDepartmentEmployeeEntity.CreatedBy = UserHelper.GetCurrentFullUserName;
                    isSuccess = rolesByDepartmentEmployeeBLL.AddRolesByDepartmentEmployee(rolesByDepartmentEmployeeEntity);
                }
                else
                {
                    rolesByDepartmentEmployeeEntity.LastModifiedUser = UserHelper.GetCurrentFullUserName;
                    isSuccess = rolesByDepartmentEmployeeBLL.UpdateRolesByDepartmentEmployee(rolesByDepartmentEmployeeEntity);
                }
                if (isSuccess)
                {
                    SearchResults(PagerUtil.GetActivePage(blstPager));
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#MaintenanceDialog').modal('hide')", true);
                    return;
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msgjAddUpdateFailed")));
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
                    int rolesByDepartmentEmployeeID = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayRolesByDepartmentEmployeeInformation(rolesByDepartmentEmployeeID);

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
        /// Handles the btnDelete click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var SelectedRow = grvList.DataKeys[int.Parse(hdfSelectedRowIndex.Value)];
                var code = Convert.ToInt32(SelectedRow.Value.ToString());
                var result = rolesByDepartmentEmployeeBLL.DeleteRolesByDepartmentEmployee(code);

                if (result)
                {
                    PageHelper<RolesByDepartmentEmployeeEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

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
        /// <summary>
        /// Displays on modal the roles by department employee information
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeID">The roles by department employee id to search</param>
        private void DisplayRolesByDepartmentEmployeeInformation(int rolesByDepartmentEmployeeID)
        {
            try
            {
                RolesByDepartmentEmployeeEntity rolesByDepartmentEmployeeEntity = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeById(rolesByDepartmentEmployeeID);
                if (rolesByDepartmentEmployeeEntity != null)
                {
                    BindDepartments(cboDepartmentCode);
                    BindEmployeeDropdown(rolesByDepartmentEmployeeEntity.DepartmentCode);
                    BindRoleApprovers(cboRoleApproversCode);
                    cboDepartmentCode.Enabled=false;
                    cboEmployeeCode.Enabled = false;
                    txtRolesByDepartmentEmployeeID.Text = rolesByDepartmentEmployeeEntity.RolesByDepartmentEmployeeID.ToString();
                    cboDepartmentCode.SelectedValue = rolesByDepartmentEmployeeEntity.DepartmentCode;
                    cboEmployeeCode.SelectedValue = rolesByDepartmentEmployeeEntity.EmployeeCode;
                    cboRoleApproversCode.SelectedValue = rolesByDepartmentEmployeeEntity.RoleApproversCode.ToString();
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
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<RolesByDepartmentEmployeeEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

            //
            if (rolesByDepartmentEmployeeBLL == null)
            {
                rolesByDepartmentEmployeeBLL = Application.GetContainer().Resolve<IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity>>();
            }
            PageHelper<RolesByDepartmentEmployeeEntity> pageHelper = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(
                  SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , string.IsNullOrEmpty(cboDepartmentId.SelectedValue) ? "" : cboDepartmentId.SelectedValue
                , txtEmployeeId.Text
                , sortExpression
                , sortDirection
                , page
                , pageSizeValue); 
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);

            return pageHelper;
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        /// <param name="pageHelper">The page helper that contains result information</param>
        private void DisplayResults(PageHelper<RolesByDepartmentEmployeeEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
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
    }
}