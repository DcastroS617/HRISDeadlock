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
    public partial class UserCodeList : System.Web.UI.Page
    {
        [Dependency]
        public IUserCodeBLL<UserCodeEntity> userCodeBLL { get; set; }
        [Dependency]
        public ICodeRoleApproverBLL<CodeRoleApproverEntity> codeRoleApproverBLL { get; set; }
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
                    BindCodeRoleApprover(cboRoleApproverID);
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
        /// Bind Code Role Approver DropDown
        /// </summary>
        /// <param name="dropDownList">pass dropdown name</param>
        private void BindCodeRoleApprover(DropDownList dropDownList)
        {
            List<CodeRoleApproverEntity> codeRoleApproverEntities = new List<CodeRoleApproverEntity>();
            try
            {
                codeRoleApproverEntities.Add(new CodeRoleApproverEntity() { CodeRoleApproverID = 0, RoleApprover = "" });
                codeRoleApproverEntities.AddRange(codeRoleApproverBLL.GetCodeRoleApproverListForDropdown());
                dropDownList.DataSource = codeRoleApproverEntities;
                dropDownList.DataValueField = "CodeRoleApproverID";
                dropDownList.DataTextField = "RoleApprover";
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
        /// Handles the btnCleanFilters click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnCleanFilters_Click(object sender, EventArgs e)
        {
            txtUserCode.Text = "";
            cboRoleApproverID.SelectedIndex = 0;
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
                BindCodeRoleApprover(cboCodeRoleApproverID);
                txtUserCodeID.Text = "0";
                cboCodeRoleApproverID.SelectedIndex = 0;
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
            UserCodeEntity userCodeEntity = new UserCodeEntity();
            try
            {
                
                userCodeEntity.UserCodeID= string.IsNullOrEmpty(txtUserCodeID.Text) ? 0 : Convert.ToInt32(txtUserCodeID.Text);
                userCodeEntity.CodeRoleApproverID = string.IsNullOrEmpty(cboCodeRoleApproverID.SelectedValue) ? 0 : Convert.ToInt32(cboCodeRoleApproverID.SelectedValue);
                bool isSuccess = false;

                if (userCodeEntity.UserCodeID.Equals(0))
                {
                    userCodeEntity.CreatedBy = UserHelper.GetCurrentFullUserName;
                    isSuccess = userCodeBLL.AddUserCode(userCodeEntity);
                }
                else
                {
                    userCodeEntity.LastModifiedUser = UserHelper.GetCurrentFullUserName;
                    isSuccess = userCodeBLL.UpdateUserCode(userCodeEntity);
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
                    int userCodeID = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayUserCodeInformation(userCodeID);

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
                var result = userCodeBLL.DeleteUserCode(code);

                if (result)
                {
                    PageHelper<UserCodeEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

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
        /// Displays on modal the user code information
        /// </summary>
        /// <param name="userCodeID">The user code id to search</param>
        private void DisplayUserCodeInformation(int userCodeID)
        {
            try
            {
                UserCodeEntity userCodeEntity = userCodeBLL.UserCodeByUserCodeID(userCodeID);
                if (userCodeEntity != null)
                {
                    BindCodeRoleApprover(cboCodeRoleApproverID);
                    txtUserCodeID.Text = userCodeEntity.UserCodeID.ToString();
                    cboCodeRoleApproverID.SelectedValue = userCodeEntity.CodeRoleApproverID.ToString();
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
        private PageHelper<UserCodeEntity> SearchResults(int page)
        {
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            //
            if (userCodeBLL == null)
            {
                userCodeBLL = Application.GetContainer().Resolve<IUserCodeBLL<UserCodeEntity>>();
            }
            PageHelper<UserCodeEntity> pageHelper = userCodeBLL.GetUserCodeList(
                string.IsNullOrEmpty(txtUserCode.Text) ? 0 : Convert.ToInt32(txtUserCode.Text)
                , string.IsNullOrEmpty(cboRoleApproverID.SelectedValue) ? 0 : Convert.ToInt32(cboRoleApproverID.SelectedValue)
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
        private void DisplayResults(PageHelper<UserCodeEntity> pageHelper)
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