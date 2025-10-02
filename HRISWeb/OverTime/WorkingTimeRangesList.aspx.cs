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
    public partial class WorkingTimeRangeList : System.Web.UI.Page
    {
        [Dependency]
        public IUsersBll<UserEntity> usersBll { get; set; }
        [Dependency]
        public IWorkingTimeRangesBLL<WorkingTimeRangesEntity> workingTimeRangesBLL { get; set; }
        [Dependency]
        public IWorkingTimeTypesBLL<WorkingTimeTypesEntity> workingTimeTypesBLL { get; set; }
        
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
                    SearchResults(1);
                    BindWorkingTimeTypes(cboTimeTypeCode);
                    BindHoursDropDown(cboStartTime);
                    BindHoursDropDown(cboEndTime);
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
        private void BindHoursDropDown(DropDownList dropDownList)
        {
            List<DropdownItems> dropdownItems = new List<DropdownItems>();
            try
            {
                dropdownItems.Add(new DropdownItems() { Text = "" , Value = "" });
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
        protected void btnCleanFilters_Click(object sender, EventArgs e)
        {
            txtTimeRangeCode.Text = "";
            cboTimeTypeCode.SelectedIndex = 0;
            cboStartTime.SelectedIndex = 0;
            cboEndTime.SelectedIndex = 0;
            SearchResults(PagerUtil.GetActivePage(blstPager));
        }
        protected void btnSearchFilter_ServerClick(object sender, EventArgs e)
        {
            SearchResults(1);
        }
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                BindWorkingTimeTypes(DropWorkingTimeTypeCode);
                BindHoursDropDown(DropWorkingStartTime);
                BindHoursDropDown(DropWorkingEndTime);
                txtWorkingTimeRangeCode.Text = "0";
                DropWorkingTimeTypeCode.SelectedIndex=0;
                DropWorkingStartTime.SelectedIndex=0;
                DropWorkingEndTime.SelectedIndex=0;
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
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            WorkingTimeRangesEntity workingTimeRangesEntity = new WorkingTimeRangesEntity();
            try
            {
                workingTimeRangesEntity.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                workingTimeRangesEntity.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                workingTimeRangesEntity.WorkingTimeRangeCode = string.IsNullOrEmpty(txtWorkingTimeRangeCode.Text) ? 0 : Convert.ToInt32(txtWorkingTimeRangeCode.Text);
                workingTimeRangesEntity.WorkingTimeTypeCode = string.IsNullOrEmpty(DropWorkingTimeTypeCode.SelectedValue) ? 0 : Convert.ToInt32(DropWorkingTimeTypeCode.SelectedValue);
                workingTimeRangesEntity.WorkingStartTime = DropWorkingStartTime.SelectedValue;
                workingTimeRangesEntity.WorkingEndTime = DropWorkingEndTime.SelectedValue;
                bool isSuccess = false;

                if (DateTime.ParseExact(workingTimeRangesEntity.WorkingStartTime, "HH:mm", CultureInfo.InvariantCulture) >=
                    DateTime.ParseExact(workingTimeRangesEntity.WorkingEndTime, "HH:mm", CultureInfo.InvariantCulture))
                { 
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjRangeHourValidation")));
                    return;
                }
                if (workingTimeRangesEntity.WorkingTimeRangeCode.Equals(0))
                {
                    workingTimeRangesEntity.CreatedBy = UserHelper.GetCurrentFullUserName;
                    isSuccess = workingTimeRangesBLL.AddWorkingTimeRanges(
                          SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , workingTimeRangesEntity);
                }
                else
                {
                    workingTimeRangesEntity.LastModifiedUser = UserHelper.GetCurrentFullUserName;
                    isSuccess = workingTimeRangesBLL.UpdateWorkingTimeRanges(workingTimeRangesEntity);
                }

                if (isSuccess)
                {
                    SearchResults(PagerUtil.GetActivePage(blstPager));
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion, Convert.ToString(GetLocalResourceObject("msjSaveRecordSuccess")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#MaintenanceDialog').modal('hide')", true);
                    return;
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
                    int workingTimeRangeCode = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayWorkingTimeRangeInformation(workingTimeRangeCode);

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
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {

                var SelectedRow = grvList.DataKeys[int.Parse(hdfSelectedRowIndex.Value)];
                var code = Convert.ToInt32(SelectedRow.Value.ToString());
                var result = workingTimeRangesBLL.DeleteWorkingTimeRanges(code);

                if (result)
                {
                    PageHelper<WorkingTimeRangesEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

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
        private void DisplayWorkingTimeRangeInformation(int workingTimeRangeCode)
        {
            try
            {
                WorkingTimeRangesEntity workingTimeRangesEntity = workingTimeRangesBLL.GetWorkingTimeRangesByWorkingTimeRangeCode(workingTimeRangeCode);
                if (workingTimeRangesEntity != null)
                {
                    BindWorkingTimeTypes(DropWorkingTimeTypeCode);
                    BindHoursDropDown(DropWorkingStartTime);
                    BindHoursDropDown(DropWorkingEndTime);
                    txtWorkingTimeRangeCode.Text = workingTimeRangesEntity.WorkingTimeRangeCode.ToString();
                    DropWorkingTimeTypeCode.SelectedValue = workingTimeRangesEntity.WorkingTimeTypeCode.ToString();
                    DropWorkingStartTime.SelectedValue = workingTimeRangesEntity.WorkingStartTime.ToString();
                    DropWorkingEndTime.SelectedValue = workingTimeRangesEntity.WorkingEndTime.ToString();
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
        private PageHelper<WorkingTimeRangesEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            hdfSelectedRowIndex.Value = "-1";
            //
            if (workingTimeRangesBLL == null)
            {
                workingTimeRangesBLL = Application.GetContainer().Resolve<IWorkingTimeRangesBLL<WorkingTimeRangesEntity>>();
            }
            PageHelper<WorkingTimeRangesEntity> pageHelper = workingTimeRangesBLL.GetWorkingTimeRangesList(
                  SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , string.IsNullOrEmpty(txtTimeRangeCode.Text) ? 0 : Convert.ToInt32(txtTimeRangeCode.Text)
                , string.IsNullOrEmpty(cboTimeTypeCode.SelectedValue) ? 0 : Convert.ToInt32(cboTimeTypeCode.SelectedValue)
                , string.IsNullOrEmpty(cboStartTime.SelectedValue) ? "0" : cboStartTime.SelectedValue
                , string.IsNullOrEmpty(cboEndTime.SelectedValue) ? "0" : cboEndTime.SelectedValue
                , sortExpression
                , sortDirection
                , page
                , pageSizeValue);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);

            return pageHelper;
        }
        private void DisplayResults(PageHelper<WorkingTimeRangesEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
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
    }
}