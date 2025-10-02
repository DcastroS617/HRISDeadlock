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
using Unity.Attributes;

namespace HRISWeb.SocialResponsability.Maintenances
{
    public partial class Transports : System.Web.UI.Page
    {
        [Dependency]
        protected ITransportsBll<TransportEntity> objTransportsBll { get; set; }
        
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
                    LoadTransportTypes();
                    BtnSearch_ServerClick(sender, e);
                }

                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    CultureGlobal = ci.Name;
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenTransportCode = !string.IsNullOrWhiteSpace(hdfTransportCodeEdit.Value) ?
                    Convert.ToInt16(hdfTransportCodeEdit.Value) : (short)-1;

                string transportDescriptionSpanish = txtTransportDescriptionSpanish.Text.Trim();
                string transportDescriptionEnglish = txtTransportDescriptionEnglish.Text.Trim();
                byte transportTypeCode = Convert.ToByte(cboTransportType.SelectedValue);
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenTransportCode.Equals(-1))
                {
                    Tuple<bool, TransportEntity> addResult = objTransportsBll.Add(new TransportEntity(transportDescriptionSpanish
                        , transportDescriptionEnglish
                        , transportTypeCode
                        , searchEnable
                        , deleted
                        , lastModifiedUser));

                    hiddenTransportCode = addResult.Item2.TransportCode;
                    hdfTransportCodeEdit.Value = addResult.Item2.TransportCode.ToString();

                    if (addResult.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack();", true);
                    }
                    else if (!addResult.Item1)
                    {
                        TransportEntity previousEntity = addResult.Item2;
                        hdfTransportCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfTransportCodeEdit.Value = previousEntity.TransportCode.ToString();
                            txtActivateDeletedTransportDescriptionSpanish.Text = previousEntity.TransportDescriptionSpanish;
                            txtActivateDeletedTransportDescriptionEnglish.Text = previousEntity.TransportDescriptionEnglish;
                            cboActivateDeletedTransportType.SelectedValue = cboActivateDeletedTransportType.Items.FindByValue(previousEntity.TransportTypeCode.ToString()).Value;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }
                        else
                        {
                            hdfTransportCodeEdit.Value = previousEntity.TransportCode.ToString();
                            txtDuplicatedTransportDescriptionSpanish.Text = previousEntity.TransportDescriptionSpanish;
                            txtDuplicatedTransportDescriptionEnglish.Text = previousEntity.TransportDescriptionEnglish;
                            cboDuplicatedTransportType.SelectedValue = cboDuplicatedTransportType.Items.FindByValue(previousEntity.TransportTypeCode.ToString()).Value;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = objTransportsBll.Edit(new TransportEntity(hiddenTransportCode
                          , transportDescriptionSpanish
                          , transportDescriptionEnglish
                          , transportTypeCode
                          , searchEnable
                          , deleted
                          , lastModifiedUser));

                    if (result.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBack();", true);
                    }

                    else
                    {
                        TransportEntity previousEntity = result.Item2;
                        hdfTransportCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfTransportCodeEdit.Value = previousEntity.TransportCode.ToString();
                            txtActivateDeletedTransportDescriptionSpanish.Text = previousEntity.TransportDescriptionSpanish;
                            txtActivateDeletedTransportDescriptionEnglish.Text = previousEntity.TransportDescriptionEnglish;
                            cboActivateDeletedTransportType.SelectedValue = cboActivateDeletedTransportType.Items.FindByValue(previousEntity.TransportTypeCode.ToString()).Value;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }
                    }
                }

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports] != null)
                {
                    List<TransportEntity> TransportsBDList = objTransportsBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports, TransportsBDList);
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
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void BtnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenTransportCode = !string.IsNullOrWhiteSpace(hdfTransportCodeEdit.Value) ?
                    Convert.ToInt16(hdfTransportCodeEdit.Value) : (short)-1;

                string transportDescriptionSpanish = txtTransportDescriptionSpanish.Text.Trim();
                string transportDescriptionEnglish = txtTransportDescriptionEnglish.Text.Trim();
                byte transportTypeCode = Convert.ToByte(cboTransportType.SelectedValue);
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    objTransportsBll.Activate(new TransportEntity(hiddenTransportCode, lastModifiedUser));
                }

                //update and activate the deleted item
                else
                {
                    objTransportsBll.Edit(new TransportEntity(hiddenTransportCode
                        , transportDescriptionSpanish
                        , transportDescriptionEnglish
                        , transportTypeCode
                        , searchEnable
                        , deleted
                        , lastModifiedUser));
                }

                DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);

                hdfSelectedRowIndex.Value = "-1";

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports] != null)
                {
                    List<TransportEntity> TransportsBDList = objTransportsBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports, TransportsBDList);
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
                    short selectedTransportCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    TransportEntity entity = objTransportsBll.ListByKey(selectedTransportCode);

                    hdfTransportCodeEdit.Value = selectedTransportCode.ToString();
                    txtTransportDescriptionSpanish.Text = entity.TransportDescriptionSpanish;
                    txtTransportDescriptionEnglish.Text = entity.TransportDescriptionEnglish;
                    cboTransportType.SelectedValue = cboTransportType.Items.FindByValue(entity.TransportTypeCode.ToString()).Value;
                    chkSearchEnabled.Checked = entity.SearchEnabled;

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
                    short selectedTransportCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    objTransportsBll.Delete(new TransportEntity(selectedTransportCode, UserHelper.GetCurrentFullUserName));

                    PageHelper<TransportEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
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
                    if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports] != null)
                    {
                        List<TransportEntity> TransportsBDList = objTransportsBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports, TransportsBDList);
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
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);
                DisplayResults(SearchResults(1));
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
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<TransportEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);
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

                CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                if (ci.Name.Equals("en-US"))
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                }
                else if (ci.Name.Equals("es-CR"))
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
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
        private PageHelper<TransportEntity> SearchResults(int page)
        {
            string transportDescriptionSpanish = string.IsNullOrWhiteSpace(txtTransportDescriptionSpanishFilter.Text.Trim()) ? null : txtTransportDescriptionSpanishFilter.Text.Trim();
            string transportDescriptionEnglish = string.IsNullOrWhiteSpace(txtTransportDescriptionEnglishFilter.Text.Trim()) ? null : txtTransportDescriptionEnglishFilter.Text.Trim();
            byte? transportTypeCode = cboTransportTypeFilter.SelectedItem != null && cboTransportTypeFilter.SelectedValue != "-1" ? Convert.ToByte(cboTransportTypeFilter.SelectedValue) : (byte?)null;
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<TransportEntity> pageHelper = objTransportsBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , transportDescriptionSpanish
                , transportDescriptionEnglish
                , transportTypeCode
                , sortExpression
                , sortDirection
                , page
                , null);

            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults(PageHelper<TransportEntity> pageHelper)
        {
            if (pageHelper != null)
            {
                pageHelper.ResultList.ForEach(t => t.TransportTypeDescription = t.TransportTypeCode.Equals(Convert.ToByte(HrisEnum.TransportType.TransportToGoWork))
                    ? Convert.ToString(GetLocalResourceObject("TransportTypeToGoWork"))
                    : (t.TransportTypeCode.Equals(Convert.ToByte(HrisEnum.TransportType.OwnTransport))
                        ? Convert.ToString(GetLocalResourceObject("TransportTypeOwn"))
                        : Convert.ToString(GetLocalResourceObject("TransportTypeBoth"))));

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
        /// Load the controls for transport types
        /// </summary>
        private void LoadTransportTypes()
        {
            List<TransportTypeEntity> transportTypes = new List<TransportTypeEntity>(4);
            transportTypes.Add(new TransportTypeEntity(-1, string.Empty));
            transportTypes.Add(new TransportTypeEntity(Convert.ToInt16(HrisEnum.TransportType.TransportToGoWork), Convert.ToString(GetLocalResourceObject("TransportTypeToGoWork"))));
            transportTypes.Add(new TransportTypeEntity(Convert.ToInt16(HrisEnum.TransportType.OwnTransport), Convert.ToString(GetLocalResourceObject("TransportTypeOwn"))));
            transportTypes.Add(new TransportTypeEntity(Convert.ToInt16(HrisEnum.TransportType.Both), Convert.ToString(GetLocalResourceObject("TransportTypeBoth"))));

            cboTransportTypeFilter.DataValueField = "TransportTypeCode";
            cboTransportTypeFilter.DataTextField = "TransportTypeDescription";
            cboTransportTypeFilter.DataSource = transportTypes;
            cboTransportTypeFilter.DataBind();

            cboTransportType.DataValueField = "TransportTypeCode";
            cboTransportType.DataTextField = "TransportTypeDescription";
            cboTransportType.DataSource = transportTypes;
            cboTransportType.DataBind();

            cboDuplicatedTransportType.DataValueField = "TransportTypeCode";
            cboDuplicatedTransportType.DataTextField = "TransportTypeDescription";
            cboDuplicatedTransportType.DataSource = transportTypes;
            cboDuplicatedTransportType.DataBind();

            cboActivateDeletedTransportType.DataValueField = "TransportTypeCode";
            cboActivateDeletedTransportType.DataTextField = "TransportTypeDescription";
            cboActivateDeletedTransportType.DataSource = transportTypes;
            cboActivateDeletedTransportType.DataBind();
        }

        #endregion
    }
}