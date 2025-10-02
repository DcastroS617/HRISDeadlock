using HRISWeb.Shared;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using System;
using System.Web.UI;
using System.Globalization;
using System.Threading;
using Unity;
using Unity.Web;
using System.Web.UI.WebControls;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity.Attributes;
using DOLE.HRIS.Shared.Entity;
using static DOLE.HRIS.Shared.Entity.HrisEnum;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HRISWeb.SocialResponsability.Maintenances
{
    public partial class HouseholdContributionRanges : System.Web.UI.Page
    {
        [Dependency]
        protected IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity> objHouseholdContributionRangesByDivisionsBll { get; set; }
        [Dependency]
        protected ICurrenciesBll<CurrencyEntity> objCurrenciesBll { get; set; }

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
                    LoadUserDivisions();
                    LoadAvailableOrders();
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
        /// Handles the cboUserDivisionsFilter selected index change event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboUserDivisionsFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboUserDivisionsFilter.SelectedItem != null && !cboUserDivisionsFilter.SelectedValue.Equals("-1"))
            {
                LoadCurency();
                LoadContributionRanges();
            }
            else
            {
                lblCurrency.Text = string.Empty;
                htmlResultsSubtitle.InnerHtml = string.Empty;
                grvList.DataSource = null;
                grvList.DataBind();
            }
        }
        
        /// <summary>
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void btnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenHouseholdContributionRangeCode = !string.IsNullOrWhiteSpace(hdfHouseholdContributionRangeCodeEdit.Value) ?
                    Convert.ToInt16(hdfHouseholdContributionRangeCodeEdit.Value) : (short)-1;

                int divisionCode = Convert.ToInt32(cboDivisionCode.SelectedValue);
                decimal rangeFrom = Convert.ToDecimal(txtRangeFrom.Text);
                decimal rangeTo = Convert.ToDecimal(txtRangeTo.Text);
                byte rangeOrder = Convert.ToByte(cboRangeOrder.SelectedValue);
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                var RangeOff = 0;
                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    RangeOff= objHouseholdContributionRangesByDivisionsBll.Activate(new HouseHoldContributionRangeByDivisionEntity(hiddenHouseholdContributionRangeCode, lastModifiedUser));
                }
                //update and activate the deleted item
                else
                {

                    objHouseholdContributionRangesByDivisionsBll.Edit(new HouseHoldContributionRangeByDivisionEntity(hiddenHouseholdContributionRangeCode
                        , divisionCode
                        , rangeFrom
                        , rangeTo
                        , rangeOrder
                        , searchEnable
                        , deleted
                        , lastModifiedUser));                    
                }
                if (RangeOff==0)
                {
 
                DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                hdfSelectedRowIndex.Value = "-1";
                //
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions] != null)
                {
                    List<HouseHoldContributionRangeByDivisionEntity> householdContributionRangeByDivisionBDList = objHouseholdContributionRangesByDivisionsBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions, householdContributionRangeByDivisionBDList);
                }

                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Informacion,GetLocalResourceObject("msgRangeOffValidate").ToString());
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenHouseholdContributionRangeCode = !string.IsNullOrWhiteSpace(hdfHouseholdContributionRangeCodeEdit.Value) ?
                    Convert.ToInt16(hdfHouseholdContributionRangeCodeEdit.Value) : (short)-1;
                
                int divisionCode = Convert.ToInt32(cboDivisionCode.SelectedValue);
                decimal rangeFrom = Convert.ToDecimal(txtRangeFrom.Text);
                decimal rangeTo = Convert.ToDecimal(txtRangeTo.Text);
                byte rangeOrder = Convert.ToByte(cboRangeOrder.SelectedValue);
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenHouseholdContributionRangeCode.Equals(-1))
                {
                    Tuple<bool, HouseHoldContributionRangeByDivisionEntity> addResult = objHouseholdContributionRangesByDivisionsBll.Add(new HouseHoldContributionRangeByDivisionEntity(divisionCode
                        , rangeFrom
                        , rangeTo
                        , rangeOrder
                        , searchEnable
                        , deleted
                        , lastModifiedUser));

                    hiddenHouseholdContributionRangeCode = addResult.Item2.HouseholdContributionRangeCode;
                    hdfHouseholdContributionRangeCodeEdit.Value = addResult.Item2.HouseholdContributionRangeCode.ToString();

                    if (addResult.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }
                    else if (!addResult.Item1)
                    {
                        hdfHouseholdContributionRangeCodeEdit.Value = "-1";
                        HouseHoldContributionRangeByDivisionEntity previousEntity = addResult.Item2;
                        if (previousEntity.Deleted)
                        {
                            hdfHouseholdContributionRangeCodeEdit.Value = Convert.ToString(hiddenHouseholdContributionRangeCode);
                            cboActivateDeletedDivisionCode.SelectedValue = cboActivateDeletedDivisionCode.Items.FindByValue(previousEntity.DivisionCode.ToString()).Value;
                            txtActivateDeletedRangeFrom.Text = string.Format("{0:N}", previousEntity.RangeFrom);
                            txtActivateDeletedRangeTo.Text = string.Format("{0:N}", previousEntity.RangeTo);
                            cboActivateDeletedRangeOrder.SelectedValue = cboActivateDeletedRangeOrder.Items.FindByValue(previousEntity.RangeOrder.ToString()).Value;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }
                        else
                        {
                            cboDuplicatedDivisionCode.SelectedValue = cboDuplicatedDivisionCode.Items.FindByValue(previousEntity.DivisionCode.ToString()).Value;
                            txtDuplicatedRangeFrom.Text = string.Format("{0:N}", previousEntity.RangeFrom);
                            txtDuplicatedRangeTo.Text = string.Format("{0:N}", previousEntity.RangeTo);
                            cboDuplicatedRangeOrder.SelectedValue = cboDuplicatedRangeOrder.Items.FindByValue(previousEntity.RangeOrder.ToString()).Value;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated(); ", true);
                        }
                    }
                }
                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    objHouseholdContributionRangesByDivisionsBll.Edit(new HouseHoldContributionRangeByDivisionEntity(hiddenHouseholdContributionRangeCode
                        , divisionCode
                        , rangeFrom
                        , rangeTo
                        , rangeOrder
                        , searchEnable
                        , deleted
                        , lastModifiedUser));

                    DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                }
                //
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions] != null)
                {
                    List<HouseHoldContributionRangeByDivisionEntity> householdContributionRangeByDivisionBDList = objHouseholdContributionRangesByDivisionsBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions, householdContributionRangeByDivisionBDList);
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
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    btnAdd.Disabled = true;
                    btnEdit.Disabled = true;
                    btnDelete.Disabled = true;
                    short selectedHouseholdContributionRangeCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HouseHoldContributionRangeByDivisionEntity entity = objHouseholdContributionRangesByDivisionsBll.ListByKey(selectedHouseholdContributionRangeCode);

                    hdfHouseholdContributionRangeCodeEdit.Value = selectedHouseholdContributionRangeCode.ToString();
                    cboDivisionCode.SelectedValue = cboDivisionCode.Items.FindByValue(entity.DivisionCode.ToString()).Value;
                    txtRangeFrom.Text = decimal.ToInt32(entity.RangeFrom).ToString();
                    txtRangeTo.Text = decimal.ToInt32(entity.RangeTo).ToString();
                    cboRangeOrder.SelectedValue = cboRangeOrder.Items.FindByValue(entity.RangeOrder.ToString()).Value;
                    chkSearchEnabled.Checked = entity.SearchEnabled;
                    hdfUiLanguage.Value = GetCurrentCulture().Name;
                    RangeOrderOld.Value = entity.RangeOrder.ToString();

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , $"ReturnFromBtnEditClickPostBack{Guid.NewGuid().ToString()}"
                        , $"setTimeout(function () {{ ReturnFromBtnEditClickPostBack('{GetCurrentCulture().Name}'); }}, 200);"
                        , true);
                }
                else
                {

                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                    btnAdd.Disabled = false;
                    btnEdit.Disabled = false;
                    btnDelete.Disabled = false;
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
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    short selectedHouseholdContributionRangeCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    objHouseholdContributionRangesByDivisionsBll.Delete(new HouseHoldContributionRangeByDivisionEntity(selectedHouseholdContributionRangeCode, UserHelper.GetCurrentFullUserName));
                                        
                    PageHelper<HouseHoldContributionRangeByDivisionEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }
                    DisplayResults(pageHelper);

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);

                    //
                    if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions] != null)
                    {
                        List<HouseHoldContributionRangeByDivisionEntity> householdContributionRangeByDivisionBDList = objHouseholdContributionRangesByDivisionsBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions, householdContributionRangeByDivisionBDList);
                    }
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
        protected void blstPager_Click(object sender, BulletedListEventArgs e)
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
        protected void grvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0)
                || (grvList.ShowHeaderWhenEmpty))
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
        protected void grvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<HouseHoldContributionRangeByDivisionEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Load the user divisions
        /// </summary>
        private void LoadUserDivisions()
        {
            cboUserDivisionsFilter.DataValueField = "DivisionCode";
            cboUserDivisionsFilter.DataTextField = "DivisionName";
            cboUserDivisionsFilter.DataSource = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions).OrderBy(d => d.DivisionName);
            cboUserDivisionsFilter.DataBind();
            cboUserDivisionsFilter.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboDivisionCode.DataValueField = "DivisionCode";
            cboDivisionCode.DataTextField = "DivisionName";
            cboDivisionCode.DataSource = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions).OrderBy(d => d.DivisionName);
            cboDivisionCode.DataBind();
            cboDivisionCode.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboDuplicatedDivisionCode.DataValueField = "DivisionCode";
            cboDuplicatedDivisionCode.DataTextField = "DivisionName";
            cboDuplicatedDivisionCode.DataSource = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions).OrderBy(d => d.DivisionName);
            cboDuplicatedDivisionCode.DataBind();
            cboDuplicatedDivisionCode.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboActivateDeletedDivisionCode.DataValueField = "DivisionCode";
            cboActivateDeletedDivisionCode.DataTextField = "DivisionName";
            cboActivateDeletedDivisionCode.DataSource = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions).OrderBy(d => d.DivisionName);
            cboActivateDeletedDivisionCode.DataBind();
            cboActivateDeletedDivisionCode.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }
        /// <summary>
        /// Load the household contribution ranges by the selected division
        /// </summary>
        private void LoadContributionRanges()
        {
            try
            {
                DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));                
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
        /// Load the available orders for the contribution ranges
        /// </summary>
        private void LoadAvailableOrders()
        {
            cboRangeOrder.Items.AddRange(Enumerable.Range(1, 12).Select(ro => new ListItem(ro.ToString())).ToArray());
            cboRangeOrder.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboDuplicatedRangeOrder.Items.AddRange(Enumerable.Range(1, 12).Select(ro => new ListItem(ro.ToString())).ToArray());
            cboDuplicatedRangeOrder.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboActivateDeletedRangeOrder.Items.AddRange(Enumerable.Range(1, 12).Select(ro => new ListItem(ro.ToString())).ToArray());
            cboActivateDeletedRangeOrder.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }
        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<HouseHoldContributionRangeByDivisionEntity> SearchResults(int page)
        {            
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<HouseHoldContributionRangeByDivisionEntity> pageHelper = objHouseholdContributionRangesByDivisionsBll.ListByFilters(
                Convert.ToInt32(cboUserDivisionsFilter.SelectedValue)
                , sortExpression
                , sortDirection
                , page
                , null);

            return pageHelper;
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults(PageHelper<HouseHoldContributionRangeByDivisionEntity> pageHelper)
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
        /// Load the Currency By Division
        /// </summary>
        private void LoadCurency()
        {
            try
            {
                List<CurrencyEntity> currenciesBDList = new List<CurrencyEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies] != null)
                {
                    currenciesBDList = (List<CurrencyEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies];
                }
                else
                {
                    objCurrenciesBll = objCurrenciesBll ?? Application.GetContainer().Resolve<ICurrenciesBll<CurrencyEntity>>();
                    currenciesBDList = objCurrenciesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                List<CurrencyEntity> currencyByDivision = currenciesBDList.Where(r => r.DivisionCode.Equals(Convert.ToInt32(cboUserDivisionsFilter.SelectedValue))).ToList();

                if (currencyByDivision != null && currencyByDivision.Any())
                {
                    lblCurrency.Text = string.Format(Convert.ToString(GetLocalResourceObject("lblCurrency.Text"))
                        , currencyByDivision.FirstOrDefault().CurrencyCode
                        , currentCulture.Name.Equals(Constants.cCultureEsCR)
                            ? currencyByDivision.FirstOrDefault().CurrencyNameSpanish
                            : currencyByDivision.FirstOrDefault().CurrencyNameEnglish);
                }
                else
                {
                    lblCurrency.Text = string.Format(Convert.ToString(GetLocalResourceObject("lblCurrency.Text"))
                        , string.Empty
                        , string.Empty);
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
        #endregion
    }
}