using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.SocialResponsability.Maintenances
{
    public partial class InitiativeCoordinator : System.Web.UI.Page
    {
        [Dependency]
        protected IInitiativeCoordinatorsBLL<InitiativeCoordinatorEntity> objInitiativeCoordinatorBll { get; set; }

        [Dependency]
        protected IUsersBll<UserEntity> objUsersBll { get; set; }

        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionBll { get; set; }

        //session key for the results
        readonly string sessionKeySocialResponsabilityResults = "SocialResponsability-InitiativeCoordinatorResults";
        readonly string sessionKeyDivisionResults = "Coordinators-DivisionResults";
        readonly string sessionKeyDivisionSearchResults = "Coordinators-DivisionSearchResults";
        readonly string sessionKeyUsersResults = "Coordinators-UserResults";

        public string CultureGlobal { get; set; }

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
                if (!IsPostBack)
                {
                    LoadList();
                    BtnSearch_ServerClick(sender, e);
                    LoadUsers();
                }

                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    CultureGlobal = ci.Name;
                }

                //activate the pager
                if (Session[sessionKeySocialResponsabilityResults] != null)
                {
                    PageHelper<InitiativeCoordinatorEntity> pageHelper = (PageHelper<InitiativeCoordinatorEntity>)Session[sessionKeySocialResponsabilityResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
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
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
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
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
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
                short hiddenInitiativeCoordinatorCode = !string.IsNullOrWhiteSpace(hdfInitiativeCoordinatorCodeEdit.Value) ?
                    Convert.ToInt16(hdfInitiativeCoordinatorCodeEdit.Value) : (short)-1;

                string coordinatorName = cboUsers.SelectedItem.Text.Trim().Split('-')[0].Trim();
                string userName = cboUsers.SelectedValue.Trim();

                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    InitiativeCoordinatorEntity dpe = new InitiativeCoordinatorEntity();
                    dpe.CoordinatorName = coordinatorName;
                    dpe.UserName = userName;
                    dpe.CoordinatorCode = int.Parse(hiddenInitiativeCoordinatorCode.ToString());
                    dpe.LastModifiedUser = lastModifiedUser;
                    objInitiativeCoordinatorBll.Activate(dpe);
                }

                //update and activate the deleted item
                else
                {
                    InitiativeCoordinatorEntity dpe = new InitiativeCoordinatorEntity();
                    dpe.CoordinatorName = coordinatorName;
                    dpe.UserName = userName;
                    dpe.CoordinatorCode = int.Parse(hiddenInitiativeCoordinatorCode.ToString());
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    objInitiativeCoordinatorBll.Activate(dpe);
                }

                SearchResults(PagerUtil.GetActivePage(blstPager));
                DisplayResults();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);

                hdfSelectedRowIndex.Value = "-1";
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenInitiativeCoordinatorCode = !string.IsNullOrWhiteSpace(hdfInitiativeCoordinatorCodeEdit.Value) ?
                    Convert.ToInt16(hdfInitiativeCoordinatorCodeEdit.Value) : (short)-1;

                string coordinatorName = cboUsers.SelectedItem.Text.Trim().Split('-')[0].Trim();
                string userName = cboUsers.SelectedValue.Trim();
                bool searchEnable = true;//chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenInitiativeCoordinatorCode.Equals(-1))
                {
                    InitiativeCoordinatorEntity dpe = new InitiativeCoordinatorEntity();
                    dpe.CoordinatorName = coordinatorName.Split('-')[0].Trim();
                    dpe.UserName = userName;
                    dpe.DivisionCode = int.Parse(cboDivision.SelectedValue);
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    Tuple<bool, InitiativeCoordinatorEntity> addResult = objInitiativeCoordinatorBll.Add(dpe);

                    hiddenInitiativeCoordinatorCode = short.Parse(addResult.Item2.CoordinatorCode.ToString());
                    hdfInitiativeCoordinatorCodeEdit.Value = addResult.Item2.CoordinatorCode.ToString();

                    if (addResult.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        InitiativeCoordinatorEntity previousEntity = addResult.Item2;
                        hdfInitiativeCoordinatorCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfInitiativeCoordinatorCodeEdit.Value = Convert.ToString(hiddenInitiativeCoordinatorCode);

                            cboDeletedDivision.SelectedValue = previousEntity.DivisionCode.ToString();
                            cboDeletedUsers.SelectedValue = previousEntity.UserName;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            cboDuplicatedDivision.SelectedValue = previousEntity.DivisionCode.ToString();
                            cboDuplicatedUsers.SelectedValue = previousEntity.UserName;

                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
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
                    short selectedInitiativeCoordinatorCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    int division = int.Parse(cboDivisionFilter.SelectedValue);

                    InitiativeCoordinatorEntity entity = objInitiativeCoordinatorBll.ListByKey(selectedInitiativeCoordinatorCode, division);

                    hdfInitiativeCoordinatorCodeEdit.Value = selectedInitiativeCoordinatorCode.ToString();
                    cboDivision.SelectedValue = entity.DivisionCode.ToString();

                    Session[sessionKeyUsersResults] = null;

                    cboDivision_SelectedIndexChanged(sender, e);

                    LoadUsers();

                    cboUsers.SelectedValue = entity.UserName;                  

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
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
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    InitiativeCoordinatorEntity dpe = new InitiativeCoordinatorEntity();
                    
                    short selectedInitiativeCoordinatorCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    dpe.CoordinatorCode = short.Parse(selectedInitiativeCoordinatorCode.ToString());
                    dpe.LastModifiedUser = UserHelper.GetCurrentFullUserName;

                    objInitiativeCoordinatorBll.Delete(dpe);

                    PageHelper<InitiativeCoordinatorEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }

                    DisplayResults();

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);

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

            PageHelper<InitiativeCoordinatorEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults();
        }

        /// <summary>
        /// Handles the grvList data bound event
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session[Constants.cCulture] != null)
            {
                if (e.Row.Cells.Count <= 1)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Handles the event selectedIndexChanged on cboGroupType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboDivision.SelectedValue != "")
                {
                    //LoadUsers();
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

        #region Private methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<InitiativeCoordinatorEntity> SearchResults(int page)
        {
            string InitiativeCoordinatorDesSpanish = string.IsNullOrWhiteSpace(txtInitiativeCoordinatorDesSpanishFilter.Text.Trim()) ? null : txtInitiativeCoordinatorDesSpanishFilter.Text.Trim();
            int division = int.Parse(cboDivisionFilter.SelectedValue);
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<InitiativeCoordinatorEntity> pageHelper = objInitiativeCoordinatorBll.ListByFilters(
                division
                , InitiativeCoordinatorDesSpanish
                , null
                , sortExpression
                , sortDirection
                , page
                , null);

            Session[sessionKeySocialResponsabilityResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeySocialResponsabilityResults] != null)
            {
                PageHelper<InitiativeCoordinatorEntity> pageHelper = (PageHelper<InitiativeCoordinatorEntity>)Session[sessionKeySocialResponsabilityResults];

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
        /// Load Lists from database
        /// </summary>
        private void LoadList()
        {   DataTable divisionsSearch = LoadDivisionsSearch();  
            
            cboDivisionFilter.Enabled = true;
            cboDivisionFilter.DataValueField = "DivisionCode";
            cboDivisionFilter.DataTextField = "DivisionName";
            cboDivisionFilter.DataSource = divisionsSearch;
            cboDivisionFilter.DataBind();

            DataTable divisions = LoadDivisions();

            cboDivision.Enabled = true;
            cboDivision.DataValueField = "DivisionCode";
            cboDivision.DataTextField = "DivisionName";
            cboDivision.DataSource = divisions;
            cboDivision.DataBind();           

            cboDeletedDivision.Enabled = true;
            cboDeletedDivision.DataValueField = "DivisionCode";
            cboDeletedDivision.DataTextField = "DivisionName";
            cboDeletedDivision.DataSource = divisions;
            cboDeletedDivision.DataBind();

            cboDuplicatedDivision.Enabled = true;
            cboDuplicatedDivision.DataValueField = "DivisionCode";
            cboDuplicatedDivision.DataTextField = "DivisionName";
            cboDuplicatedDivision.DataSource = divisions;
            cboDuplicatedDivision.DataBind();
        }

        /// <summary>
        /// Load Divisions from database
        /// </summary>        
        private DataTable LoadDivisions()
        {
            DataTable divisions = (DataTable)Session[sessionKeyDivisionResults];

            if (Session[sessionKeyDivisionResults] == null)
            {
                divisions = LoadEmptyDivisions();

                List<DivisionEntity> indicatorsList = ObjDivisionBll.ListAll();

                indicatorsList.ForEach(x => divisions.Rows.Add(
                    x.DivisionCode, x.DivisionName));

                DataRow defaultRow = divisions.NewRow();

                defaultRow.SetField("DivisionCode", "-1");
                defaultRow.SetField("DivisionName", string.Empty);
                divisions.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyDivisionResults] = divisions;
            }

            return divisions;
        }

        /// <summary>
        /// Load Divisions from database
        /// </summary>        
        private DataTable LoadDivisionsSearch()
        {
            DataTable divisions = (DataTable)Session[sessionKeyDivisionSearchResults];

            if (Session[sessionKeyDivisionSearchResults] == null)
            {
                divisions = LoadEmptyDivisions();

                List<DivisionEntity> indicatorsList = ObjDivisionBll.ListAll();

                indicatorsList.ForEach(x => divisions.Rows.Add(
                    x.DivisionCode, x.DivisionName));

                Session[sessionKeyDivisionSearchResults] = divisions;
            }

            return divisions;
        }


        /// <summary>
        /// Load Divisions from database
        /// </summary>        
        private void LoadUsers()
        {
            DataTable users = (DataTable)Session[sessionKeyUsersResults];

            if (Session[sessionKeyUsersResults] == null)
            {
                users = LoadEmptyUsers();

                int division = int.Parse(cboDivision.SelectedValue);

                List<UserEntity> indicatorsList = objUsersBll.ListActive();

                indicatorsList.ForEach(x => users.Rows.Add(
                    x.ActiveDirectoryUserAccount, x.UserName));

                DataRow defaultRow = users.NewRow();

                defaultRow.SetField("ActiveDirectoryUserAccount", "-1");
                defaultRow.SetField("UserName", string.Empty);
                users.Rows.InsertAt(defaultRow, 0);


                Session[sessionKeyUsersResults] = users;
            }

            cboUsers.DataValueField = "ActiveDirectoryUserAccount";
            cboUsers.DataTextField = "UserName";
            cboUsers.DataSource = users;
            cboUsers.DataBind();

            cboDeletedUsers.DataValueField = "ActiveDirectoryUserAccount";
            cboDeletedUsers.DataTextField = "UserName";
            cboDeletedUsers.DataSource = users;
            cboDeletedUsers.DataBind();

            cboDuplicatedUsers.DataValueField = "ActiveDirectoryUserAccount";
            cboDuplicatedUsers.DataTextField = "UserName";
            cboDuplicatedUsers.DataSource = users;
            cboDuplicatedUsers.DataBind();
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyDivisions()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("DivisionCode", typeof(string));
            divisions.Columns.Add("DivisionName", typeof(string));

            return divisions;
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyUsers()
        {
            DataTable divisions = new DataTable();
            divisions.Columns.Add("ActiveDirectoryUserAccount", typeof(string));
            divisions.Columns.Add("UserName", typeof(string));

            return divisions;
        }

        #endregion
    }
}