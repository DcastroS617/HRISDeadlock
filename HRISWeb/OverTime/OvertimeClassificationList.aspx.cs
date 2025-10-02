using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;
namespace HRISWeb.Overtime
{
    public partial class OvertimeClassificationList : System.Web.UI.Page
    {
        [Dependency]
        public IOvertimeTypesBLL<OvertimeTypesEntity> overtimeTypesBLL { get; set; }
        [Dependency]
        public IOvertimeClassificationBLL<OvertimeClassificationEntity> overtimeClassificationBLL { get; set; }
        [Dependency]
        public IWorkingTimeTypesBLL<WorkingTimeTypesEntity> workingTimeTypesBLL { get; set; }
        [Dependency]
        public IDayTypesBLL<DayTypesEntity> dayTypesBLL { get; set; }
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
                    BindOvertimeType(cboTypeCode);
                    BindWorkingTimeTypes(droWorkingTimeTypeCode);
                    BindDayType(cboDayType);
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
        /// Handles the btnCleanFilters click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnCleanFilters_Click(object sender, EventArgs e)
        {
            txtClassificationCode.Text = "";
            txtClassificationName.Text = "";
            cboTypeCode.SelectedIndex = -1;
            cboDayType.SelectedIndex = -1;
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
                BindOvertimeType(droOvertimeTypeCode);
                BindWorkingTimeTypes(droWorkingTimeTypeCode);
                BindDayType(ddlDayType);
                txtOvertimeClassificationCode.Text = "0";
                txtOvertimeClassificationName.Text = "";
                droOvertimeTypeCode.SelectedIndex = 0;
                droWorkingTimeTypeCode.SelectedIndex = 0;
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
            OvertimeClassificationEntity overtimeClassificationEntity = new OvertimeClassificationEntity();
            try
            {
                
                overtimeClassificationEntity.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                overtimeClassificationEntity.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                overtimeClassificationEntity.OvertimeClassificationCode = string.IsNullOrEmpty(txtOvertimeClassificationCode.Text) ? 0 : Convert.ToInt32(txtOvertimeClassificationCode.Text);
                overtimeClassificationEntity.OvertimeClassificationName = txtOvertimeClassificationName.Text;
                overtimeClassificationEntity.OvertimeTypeCode = Convert.ToInt32(droOvertimeTypeCode.SelectedValue);
                overtimeClassificationEntity.WorkingTimeTypeCode = Convert.ToInt32(droWorkingTimeTypeCode.SelectedValue);
                overtimeClassificationEntity.DayTypeCode = Convert.ToInt32(ddlDayType.SelectedValue);
                overtimeClassificationEntity.IsExtra = chkIsExtra.Checked;
                bool isSuccess = false;

                if (overtimeClassificationEntity.OvertimeClassificationCode.Equals(0))
                {
                    overtimeClassificationEntity.CreatedBy = UserHelper.GetCurrentFullUserName;
                    isSuccess = overtimeClassificationBLL.AddOvertimeClassification(
                          SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , overtimeClassificationEntity);
                }
                else
                {
                    overtimeClassificationEntity.LastModifiedUser = UserHelper.GetCurrentFullUserName;
                    isSuccess = overtimeClassificationBLL.UpdateOvertimeClassification(overtimeClassificationEntity);
                }

                if (isSuccess)
                {
                    SearchResults(PagerUtil.GetActivePage(blstPager));
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#MaintenanceDialog').modal('hide')", true);
                    return;
                }
                else {
                    //MAnejo del error
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjSaveRecordError")));
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
                    int overtimeClassificationCode = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayOvertimeClassificationInformation(overtimeClassificationCode);

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
                var result = overtimeClassificationBLL.DeleteOvertimeClassification(code);

                if (result)
                {
                    PageHelper<OvertimeClassificationEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

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
        /// Displays on modal the overtime classification information
        /// </summary>
        /// <param name="overtimeClassificationCode">The overtime classification code to search</param>
        private void DisplayOvertimeClassificationInformation(int overtimeClassificationCode)
        {
            try
            {
                OvertimeClassificationEntity overtimeClassificationEntity = overtimeClassificationBLL.GetOvertimeClassificationByCode(overtimeClassificationCode);
                if (overtimeClassificationEntity != null)
                {
                    BindOvertimeType(droOvertimeTypeCode);
                    BindWorkingTimeTypes(droWorkingTimeTypeCode);
                    BindDayType(ddlDayType);
                    txtOvertimeClassificationCode.Text = overtimeClassificationEntity.OvertimeClassificationCode.ToString();
                    txtOvertimeClassificationName.Text = overtimeClassificationEntity.OvertimeClassificationName;
                    droOvertimeTypeCode.SelectedValue = overtimeClassificationEntity.OvertimeTypeCode.ToString();
                    droWorkingTimeTypeCode.SelectedValue = overtimeClassificationEntity.WorkingTimeTypeCode.ToString();
                    ddlDayType.SelectedValue = overtimeClassificationEntity.DayTypeCode.ToString();
                    chkIsExtra.Checked = overtimeClassificationEntity.IsExtra;
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
        private PageHelper<OvertimeClassificationEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            OvertimeClassificationEntity overtimeClassificationEntity = new OvertimeClassificationEntity();

            overtimeClassificationEntity.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            overtimeClassificationEntity.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            overtimeClassificationEntity.OvertimeClassificationCode = string.IsNullOrEmpty(txtClassificationCode.Text) ? 0 : Convert.ToInt32(txtClassificationCode.Text);
            overtimeClassificationEntity.OvertimeClassificationName = txtClassificationName.Text;
            overtimeClassificationEntity.OvertimeTypeCode = cboTypeCode.SelectedIndex;
            overtimeClassificationEntity.WorkingTimeTypeCode = Convert.ToInt32(cboTypeCode.SelectedIndex);
            overtimeClassificationEntity.DayTypeCode = Convert.ToInt32(cboDayType.SelectedIndex);
            overtimeClassificationEntity.IsExtra = chkIsExtra.Checked;
            //
            if (overtimeClassificationBLL == null)
            {
                overtimeClassificationBLL = Application.GetContainer().Resolve<IOvertimeClassificationBLL<OvertimeClassificationEntity>>();
            }
            PageHelper<OvertimeClassificationEntity> pageHelper = overtimeClassificationBLL.GetOvertimeClassificationList(
                  cboDayType.SelectedIndex
                , sortExpression
                , sortDirection
                , page
                , pageSizeValue
                , overtimeClassificationEntity);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);

            return pageHelper;
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        /// <param name="pageHelper">The page helper that contains result information</param>
        private void DisplayResults(PageHelper<OvertimeClassificationEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            // htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
        }
        /// <summary>
        /// Bind Overtime Type
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
        private void BindOvertimeType(DropDownList dropDownList)
        {
            List<OvertimeTypesEntity> overtimeClassifications = new List<OvertimeTypesEntity>();

            try
            {
                overtimeClassifications.Add(new OvertimeTypesEntity() { OvertimeTypeCode = -1, OvertimeTypeName = "" });
                overtimeClassifications.AddRange(overtimeTypesBLL.GetOvertimeTypesListForDropdown());
                dropDownList.DataSource = overtimeClassifications;
                dropDownList.DataValueField = "OvertimeTypeCode";
                dropDownList.DataTextField = "OvertimeTypeName";
                dropDownList.DataBind();
                dropDownList.SelectedIndex = -1;
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
        /// Bind Overtime Type
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
        private void BindDayType(DropDownList dropDownList)
        {
            List<DayTypesEntity> dayTypesList = new List<DayTypesEntity>();

            try
            {
                dayTypesList.Add(new DayTypesEntity() { DayTypeCode = -1, DayTypesName = "" });
                dayTypesList.AddRange(dayTypesBLL.GetDayTypesListForDropdown(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode));
                dropDownList.DataSource = dayTypesList;
                dropDownList.DataValueField = "DayTypeCode";
                dropDownList.DataTextField = "DayTypesName";
                dropDownList.DataBind();
                dropDownList.SelectedIndex = -1;
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
        /// Bind Working Time Types
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
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