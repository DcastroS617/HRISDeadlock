using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;

namespace HRISWeb.Security
{
    public partial class AdminUsersByModules : System.Web.UI.Page
    {
        [Dependency]
        public IAdminUsersByModulesBll<AdminUserByModuleEntity> ObjAdminUsersByModulesBll { get; set; }
        
        [Dependency]
        public IUsersBll<UserEntity> ObjUsersBll { get; set; }
        
        [Dependency]
        public IDivisionsByUsersBll<DivisionByUserEntity> ObjDivisionsByUsersBll { get; set; }
        
        [Dependency]
        public IModulesBll<ModuleEntity> ObjModulesBll { get; set; }

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
                if (ObjAdminUsersByModulesBll == null)
                {
                    ObjAdminUsersByModulesBll = Application.GetContainer().Resolve<IAdminUsersByModulesBll<AdminUserByModuleEntity>>();
                }

                if(ObjUsersBll == null)
                {
                    ObjUsersBll = Application.GetContainer().Resolve<IUsersBll<UserEntity>>();
                }

                if(ObjDivisionsByUsersBll == null)
                {
                    ObjDivisionsByUsersBll = Application.GetContainer().Resolve<IDivisionsByUsersBll<DivisionByUserEntity>>();
                }

                if (!IsPostBack)
                {
                    LoadActiveUsers();
                }
                
                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));
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
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                PageHelper<AdminUserByModuleEntity> pageHelper = SearchResults(1);

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

                DisplayResults(pageHelper);
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
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void BtnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {                
                short userCode = Convert.ToInt16(cboUsersFilter.SelectedValue.Trim());
                int divisionCode = Convert.ToInt32(cboDivision.SelectedValue.Trim());
                byte moduleCode = Convert.ToByte(cboModules.SelectedValue.Trim());
                bool searchEnable = chkSearchEnabled.Checked;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                
                if (chkActivateDeleted.Checked)
                {
                    ObjAdminUsersByModulesBll.Activate(userCode, divisionCode, moduleCode, lastModifiedUser);                                        
                    hdfSelectedRowIndex.Value = "0";                    
                }

                else
                {                    
                    ObjAdminUsersByModulesBll.Edit(userCode, 
                        divisionCode, 
                        moduleCode, 
                        searchEnable, 
                        false, 
                        lastModifiedUser);                    
                    
                    hdfSelectedRowIndex.Value = "-1";
                }

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnAcceptActivateDeletedClickPostBack{Guid.NewGuid()}", "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
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
                DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));
            }
        }

        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string hiddenDivisionModuleCode = !string.IsNullOrWhiteSpace(hdfDivisionModuleCodeEdit.Value) ? Convert.ToString(hdfDivisionModuleCodeEdit.Value) : "-1";

                short userCode = Convert.ToInt16(cboUsersFilter.SelectedValue.Trim());
                int divisionCode = Convert.ToInt32(cboDivision.SelectedValue.Trim());
                byte moduleCode = Convert.ToByte(cboModules.SelectedValue.Trim());
                bool searchEnable = chkSearchEnabled.Checked;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                
                if (hiddenDivisionModuleCode.Equals("-1"))
                {
                    Tuple<bool, AdminUserByModuleEntity> addResult = ObjAdminUsersByModulesBll.Add(userCode, 
                        divisionCode, 
                        moduleCode, 
                        searchEnable, 
                        lastModifiedUser);

                    if (addResult.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnAcceptClickPostBack{Guid.NewGuid()}", "ReturnFromBtnAcceptClickPostBack();", true);
                    }

                    else if (!addResult.Item1)
                    {
                        AdminUserByModuleEntity previousAdminUserByModule = addResult.Item2;
                        if (previousAdminUserByModule.Deleted)
                        {
                            ListItem liDivision = cboActivateDeletedDivision.Items.FindByValue(Convert.ToString(previousAdminUserByModule.DivisionCode));
                            if (liDivision == null)
                            {
                                liDivision = new ListItem(previousAdminUserByModule.DivisionName, Convert.ToString(previousAdminUserByModule.DivisionCode));
                                cboActivateDeletedDivision.Items.Add(liDivision);
                            }

                            cboActivateDeletedDivision.SelectedIndex = -1;
                            liDivision.Selected = true;

                            ListItem liModule = cboActivateDeletedModule.Items.FindByValue(Convert.ToString(previousAdminUserByModule.ModuleCode));
                            if (liModule == null)
                            {
                                liModule = new ListItem(previousAdminUserByModule.ModuleName, Convert.ToString(previousAdminUserByModule.ModuleCode));
                                cboActivateDeletedModule.Items.Add(liModule);
                            }

                            cboActivateDeletedModule.SelectedIndex = -1;
                            liModule.Selected = true;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnAcceptClickPostBackDeleted{Guid.NewGuid()}", "ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            cboDuplicatedDivision.SelectedValue = previousAdminUserByModule.DivisionCode.ToString();
                            cboDuplicatedModule.SelectedValue = previousAdminUserByModule.ModuleCode.ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnAcceptClickPostBackDuplicated{Guid.NewGuid()}", " ReturnFromBtnAcceptClickPostBackDuplicated();", true);                            
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    ObjAdminUsersByModulesBll.Edit(userCode, 
                        divisionCode, 
                        moduleCode, 
                        searchEnable, 
                        false, lastModifiedUser);

                    DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnAcceptClickPostBack{Guid.NewGuid()}", " ReturnFromBtnAcceptClickPostBack(); ", true);
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
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    btnAdd.Disabled = true;
                    btnEdit.Disabled = true;
                    btnDelete.Disabled = true;

                    short selectedUserCode = Convert.ToInt16(cboUsersFilter.SelectedValue);
                    int selectedDivisionCode = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    byte selectedModuleCode = Convert.ToByte(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Values[1]);

                    AdminUserByModuleEntity user = ObjAdminUsersByModulesBll.ListByUserDivisionModule(selectedUserCode, selectedDivisionCode, selectedModuleCode);
                    
                    hdfDivisionModuleCodeEdit.Value = Convert.ToString(user.DivisionCode);
                    ListItem liDivision = cboDivision.Items.FindByValue(Convert.ToString(user.DivisionCode));

                    if(liDivision == null)
                    {
                        liDivision = new ListItem(user.DivisionName, Convert.ToString(user.DivisionCode));
                        cboDivision.Items.Add(liDivision);
                    }

                    cboDivision.SelectedIndex = -1;
                    liDivision.Selected = true;

                    ListItem liModule = cboModules.Items.FindByValue(Convert.ToString(user.ModuleCode));
                    if(liModule == null)
                    {
                        liModule = new ListItem(user.ModuleName, Convert.ToString(user.ModuleCode));
                        cboModules.Items.Add(liModule);
                    }

                    cboModules.SelectedIndex = -1;
                    liModule.Selected = true;

                    chkSearchEnabled.Checked = user.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnEditClickPostBack{Guid.NewGuid()}", "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }                
            }

            catch (Exception ex)
            {
                btnAdd.Disabled = false;
                btnEdit.Disabled = false;
                btnDelete.Disabled = false;
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
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    short selectedUserCode = Convert.ToInt16(cboUsersFilter.SelectedValue);
                    int selectedDivisionCode = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    byte selectedModuleCode = Convert.ToByte(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Values[1]);

                    ObjAdminUsersByModulesBll.Delete(selectedUserCode, 
                        selectedDivisionCode, 
                        selectedModuleCode, 
                        UserHelper.GetCurrentFullUserName);
                    
                    PageHelper<AdminUserByModuleEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));                    
                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }

                    DisplayResults(pageHelper);
                    
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ReturnFromBtnDeleteClickPostBack{Guid.NewGuid()}", "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
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
                    PagerUtil.SetActivePage(blstPager, Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value));
                    DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));
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
        protected void GrvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0) || (grvList.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvList.ShowFooter && grvList.Rows.Count > 0)
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
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<AdminUserByModuleEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);
        }

        /// <summary>
        /// Handles the cboUsersFilter Selected Index Changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void CboUsersFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadUserDivisions();
                LoadModules();
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
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), $"ProcessUserFilterSelectedIndexResponse{Guid.NewGuid()}", "ProcessUserFilterSelectedIndexResponse();", true);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<AdminUserByModuleEntity> SearchResults(int page)
        {
            short userCode = Convert.ToInt16(cboUsersFilter.SelectedValue.Trim());
            string divisionCodeStr = cboUserDivisionsFilter.SelectedValue.Trim();
            string moduleCodeStr = cboModulesFilter.SelectedValue.Trim();

            int? divisionCode = null;
            if (!string.IsNullOrWhiteSpace(divisionCodeStr) && !divisionCodeStr.Equals("0"))
            {
                divisionCode = Convert.ToInt32(cboUserDivisionsFilter.SelectedValue.Trim());
            }

            byte? moduleCode = null;
            if (!string.IsNullOrWhiteSpace(moduleCodeStr) && !moduleCodeStr.Equals("0"))
            {
                moduleCode = Convert.ToByte(cboModulesFilter.SelectedValue.Trim());
            }

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<AdminUserByModuleEntity> pageHelper = ObjAdminUsersByModulesBll.ListByFilters(
                userCode, 
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                divisionCode, 
                moduleCode, 
                sortExpression, 
                sortDirection, 
                page);

            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults(PageHelper<AdminUserByModuleEntity> pageHelper)
        {
            if (pageHelper != null)
            {
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
        /// Load the active users
        /// </summary>
        private void LoadActiveUsers()
        {
            cboUsersFilter.DataTextField = "UserName";
            cboUsersFilter.DataValueField = "UserCode";
            cboUsersFilter.DataSource = ObjUsersBll.ListActive();
            cboUsersFilter.DataBind();
            cboUsersFilter.Items.Insert(0, new ListItem(string.Empty, "0"));
        }

        /// <summary>
        /// Load the user divisions
        /// </summary>
        private void LoadUserDivisions()
        {
            if(cboUsersFilter.SelectedItem != null && !cboUsersFilter.SelectedValue.Equals("0"))
            {
                List<DivisionByUserEntity> divisions = ObjDivisionsByUsersBll.ListByUser(Convert.ToInt16(cboUsersFilter.SelectedValue));

                cboUserDivisionsFilter.Enabled = true;
                cboUserDivisionsFilter.DataValueField = "DivisionCode";
                cboUserDivisionsFilter.DataTextField = "DivisionName";
                cboUserDivisionsFilter.DataSource = divisions;
                cboUserDivisionsFilter.DataBind();
                cboUserDivisionsFilter.Items.Insert(0, new ListItem(string.Empty, "0"));

                cboDivision.Enabled = true;
                cboDivision.DataValueField = "DivisionCode";
                cboDivision.DataTextField = "DivisionName";
                cboDivision.DataSource = divisions;
                cboDivision.DataBind();
                cboDivision.Items.Insert(0, new ListItem(string.Empty, "0"));

                cboDuplicatedDivision.Enabled = false;                
                cboDuplicatedDivision.DataValueField = "DivisionCode";
                cboDuplicatedDivision.DataTextField = "DivisionName";
                cboDuplicatedDivision.DataSource = divisions;
                cboDuplicatedDivision.DataBind();

                cboActivateDeletedDivision.Enabled = false;
                cboActivateDeletedDivision.DataValueField = "DivisionCode";
                cboActivateDeletedDivision.DataTextField = "DivisionName";
                cboActivateDeletedDivision.DataSource = divisions;
                cboActivateDeletedDivision.DataBind();
            }

            else
            {
                cboUserDivisionsFilter.SelectedIndex = -1;
                cboUserDivisionsFilter.Enabled = false;

                cboDivision.SelectedIndex = -1;
                cboDivision.Enabled = false;

                cboDuplicatedDivision.SelectedIndex = -1;
                cboActivateDeletedDivision.SelectedIndex = -1;

                grvList.DataSource = null;
                grvList.DataBind();

                hdfSelectedRowIndex.Value = "-1";
                htmlResultsSubtitle.InnerHtml = string.Empty;
                PagerUtil.SetupPager(blstPager, 0, 0);
            }
        }

        /// <summary>
        /// Load the active modules
        /// </summary>
        private void LoadModules()
        {
            if (cboUsersFilter.SelectedItem != null && !cboUsersFilter.SelectedValue.Equals("0"))
            {
                List<ModuleEntity> modules = ObjModulesBll.ListActive();

                cboModulesFilter.Enabled = true;
                cboModulesFilter.DataValueField = "ModuleCode";
                cboModulesFilter.DataTextField = "ModuleName";
                cboModulesFilter.DataSource = modules;
                cboModulesFilter.DataBind();
                cboModulesFilter.Items.Insert(0, new ListItem(string.Empty, "0"));

                cboModules.Enabled = true;
                cboModules.DataValueField = "ModuleCode";
                cboModules.DataTextField = "ModuleName";
                cboModules.DataSource = modules;
                cboModules.DataBind();
                cboModules.Items.Insert(0, new ListItem(string.Empty, "0"));

                cboDuplicatedModule.Enabled = false;
                cboDuplicatedModule.DataValueField = "ModuleCode";
                cboDuplicatedModule.DataTextField = "ModuleName";
                cboDuplicatedModule.DataSource = modules;
                cboDuplicatedModule.DataBind();

                cboActivateDeletedModule.Enabled = false;
                cboActivateDeletedModule.DataValueField = "ModuleCode";
                cboActivateDeletedModule.DataTextField = "ModuleName";
                cboActivateDeletedModule.DataSource = modules;
                cboActivateDeletedModule.DataBind();
            }

            else
            {
                cboModulesFilter.SelectedIndex = -1;
                cboModulesFilter.Enabled = false;

                cboModules.SelectedIndex = -1;
                cboModules.Enabled = false;

                cboDuplicatedModule.SelectedIndex = -1;
                cboActivateDeletedModule.SelectedIndex = -1;
            }
        }

        #endregion        
    }
}