using HRISWeb.Shared;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using System;
using System.Web.UI;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity.Attributes;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace HRISWeb.SocialResponsability.Maintenances
{
    public partial class ReasonNotMedical : System.Web.UI.Page
    {
        [Dependency]
        protected IReasonNotMedicalBLL<ReasonNotMedicalEntity> objReasonNotMedicalBll { get; set; }

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
                    btnSearch_ServerClick(sender, e);
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
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnSearch_ServerClick(object sender, EventArgs e)
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
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void btnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenReasonNotMedicalCode = !string.IsNullOrWhiteSpace(hdfReasonNotMedicalCodeEdit.Value) ?
                    Convert.ToInt16(hdfReasonNotMedicalCodeEdit.Value) : (short)-1;

                string ReasonNotMedicalNameSpanish = txtReasonNotMedicalNameSpanish.Text.Trim();
                string ReasonNotMedicalNameEnglish = txtReasonNotMedicalNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    objReasonNotMedicalBll.Activate(new ReasonNotMedicalEntity(hiddenReasonNotMedicalCode, lastModifiedUser));
                }
                //update and activate the deleted item
                else
                {
                    objReasonNotMedicalBll.Edit(new ReasonNotMedicalEntity(hiddenReasonNotMedicalCode
                        , ReasonNotMedicalNameSpanish
                        , ReasonNotMedicalNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser));
                }
                DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                hdfSelectedRowIndex.Value = "-1";

                //
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical] != null)
                {
                    List<ReasonNotMedicalEntity> ReasonNotMedicalsBDList = objReasonNotMedicalBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical, ReasonNotMedicalsBDList);
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
                short hiddenReasonNotMedicalCode = !string.IsNullOrWhiteSpace(hdfReasonNotMedicalCodeEdit.Value) ?
                    Convert.ToInt16(hdfReasonNotMedicalCodeEdit.Value) : (short)-1;

                string ReasonNotMedicalNameSpanish = txtReasonNotMedicalNameSpanish.Text.Trim();
                string ReasonNotMedicalNameEnglish = txtReasonNotMedicalNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenReasonNotMedicalCode.Equals(-1))
                {
                    Tuple<bool, ReasonNotMedicalEntity> addResult = objReasonNotMedicalBll.Add(new ReasonNotMedicalEntity(ReasonNotMedicalNameSpanish
                        , ReasonNotMedicalNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser));

                    hiddenReasonNotMedicalCode = addResult.Item2.ReasonNotMedicalCode;
                    hdfReasonNotMedicalCodeEdit.Value = addResult.Item2.ReasonNotMedicalCode.ToString();

                    if (addResult.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }
                    else if (!addResult.Item1)
                    {
                        ReasonNotMedicalEntity previousEntity = addResult.Item2;
                        hdfReasonNotMedicalCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfReasonNotMedicalCodeEdit.Value = Convert.ToString(hiddenReasonNotMedicalCode);
                            txtActivateDeletedReasonNotMedicalNameSpanish.Text = previousEntity.ReasonNotMedicalDescriptionSpanish;
                            txtActivateDeletedReasonNotMedicalNameEnglish.Text = previousEntity.ReasonNotMedicalDescriptionEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }
                        else
                        {
                            txtDuplicatedReasonNotMedicalNameSpanish.Text = previousEntity.ReasonNotMedicalDescriptionSpanish;
                            txtDuplicatedReasonNotMedicalNameEnglish.Text = previousEntity.ReasonNotMedicalDescriptionEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }
                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = objReasonNotMedicalBll.Edit(new ReasonNotMedicalEntity(hiddenReasonNotMedicalCode
                          , ReasonNotMedicalNameSpanish
                          , ReasonNotMedicalNameEnglish
                          , searchEnable
                          , deleted
                          , lastModifiedUser));
                    if (result.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);

                    }
                    else
                    {
                        ReasonNotMedicalEntity previousEntity = result.Item2;
                        hdfReasonNotMedicalCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfReasonNotMedicalCodeEdit.Value = Convert.ToString(previousEntity.ReasonNotMedicalCode);
                            txtActivateDeletedReasonNotMedicalNameSpanish.Text = previousEntity.ReasonNotMedicalDescriptionSpanish;
                            txtActivateDeletedReasonNotMedicalNameEnglish.Text = previousEntity.ReasonNotMedicalDescriptionEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }
                        else
                        {
                            txtDuplicatedReasonNotMedicalNameSpanish.Text = previousEntity.ReasonNotMedicalDescriptionSpanish;
                            txtDuplicatedReasonNotMedicalNameEnglish.Text = previousEntity.ReasonNotMedicalDescriptionEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }


                }
                //
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical] != null)
                {
                    List<ReasonNotMedicalEntity> ReasonNotMedicalsBDList = objReasonNotMedicalBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical, ReasonNotMedicalsBDList);
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
                    short selectedReasonNotMedicalCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    ReasonNotMedicalEntity entity = objReasonNotMedicalBll.ListByKey(selectedReasonNotMedicalCode);

                    hdfReasonNotMedicalCodeEdit.Value = selectedReasonNotMedicalCode.ToString();
                    txtReasonNotMedicalNameSpanish.Text = entity.ReasonNotMedicalDescriptionSpanish;
                    txtReasonNotMedicalNameEnglish.Text = entity.ReasonNotMedicalDescriptionEnglish;
                    chkSearchEnabled.Checked = entity.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                }
                else
                {
                    btnAdd.Disabled = false;
                    btnEdit.Disabled = false;
                    btnDelete.Disabled = false;
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
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    short selectedReasonNotMedicalCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    objReasonNotMedicalBll.Delete(new ReasonNotMedicalEntity(selectedReasonNotMedicalCode, UserHelper.GetCurrentFullUserName));

                    PageHelper<ReasonNotMedicalEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
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
                    if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical] != null)
                    {
                        List<ReasonNotMedicalEntity> ReasonNotMedicalsBDList = objReasonNotMedicalBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotMedical, ReasonNotMedicalsBDList);
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

            PageHelper<ReasonNotMedicalEntity> pageHelper = SearchResults(1);
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
        private PageHelper<ReasonNotMedicalEntity> SearchResults(int page)
        {
            string ReasonNotMedicalNameSpanish = string.IsNullOrWhiteSpace(txtReasonNotMedicalNameSpanishFilter.Text.Trim()) ? null : txtReasonNotMedicalNameSpanishFilter.Text.Trim();
            string ReasonNotMedicalNameEnglish = string.IsNullOrWhiteSpace(txtReasonNotMedicalNameEnglishFilter.Text.Trim()) ? null : txtReasonNotMedicalNameEnglishFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<ReasonNotMedicalEntity> pageHelper = objReasonNotMedicalBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , ReasonNotMedicalNameSpanish
                , ReasonNotMedicalNameEnglish
                , sortExpression
                , sortDirection
                , page
                , null);

            return pageHelper;
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults(PageHelper<ReasonNotMedicalEntity> pageHelper)
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
        #endregion
    }
}