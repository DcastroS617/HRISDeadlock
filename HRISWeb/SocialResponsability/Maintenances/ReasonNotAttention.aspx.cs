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
    public partial class ReasonNotAttention : System.Web.UI.Page
    {
        [Dependency]
        protected IReasonNotAttentionBLL<ReasonNotAttentionEntity> objReasonNotAttentionBll { get; set; }

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
                short hiddenReasonNotAttentionCode = !string.IsNullOrWhiteSpace(hdfReasonNotAttentionCodeEdit.Value) ?
                    Convert.ToInt16(hdfReasonNotAttentionCodeEdit.Value) : (short)-1;

                string ReasonNotAttentionNameSpanish = txtReasonNotAttentionNameSpanish.Text.Trim();
                string ReasonNotAttentionNameEnglish = txtReasonNotAttentionNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    objReasonNotAttentionBll.Activate(new ReasonNotAttentionEntity(hiddenReasonNotAttentionCode, lastModifiedUser));
                }
                //update and activate the deleted item
                else
                {
                    objReasonNotAttentionBll.Edit(new ReasonNotAttentionEntity(hiddenReasonNotAttentionCode
                        , ReasonNotAttentionNameSpanish
                        , ReasonNotAttentionNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser));
                }
                DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                hdfSelectedRowIndex.Value = "-1";

                //
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention] != null)
                {
                    List<ReasonNotAttentionEntity> ReasonNotAttentionsBDList = objReasonNotAttentionBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention, ReasonNotAttentionsBDList);
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
                short hiddenReasonNotAttentionCode = !string.IsNullOrWhiteSpace(hdfReasonNotAttentionCodeEdit.Value) ?
                    Convert.ToInt16(hdfReasonNotAttentionCodeEdit.Value) : (short)-1;

                string ReasonNotAttentionNameSpanish = txtReasonNotAttentionNameSpanish.Text.Trim();
                string ReasonNotAttentionNameEnglish = txtReasonNotAttentionNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenReasonNotAttentionCode.Equals(-1))
                {
                    Tuple<bool, ReasonNotAttentionEntity> addResult = objReasonNotAttentionBll.Add(new ReasonNotAttentionEntity(ReasonNotAttentionNameSpanish
                        , ReasonNotAttentionNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser));

                    hiddenReasonNotAttentionCode = addResult.Item2.ReasonNotAttentionCode;
                    hdfReasonNotAttentionCodeEdit.Value = addResult.Item2.ReasonNotAttentionCode.ToString();

                    if (addResult.Item1)
                    {
                        DisplayResults(SearchResults(PagerUtil.GetActivePage(blstPager)));

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }
                    else if (!addResult.Item1)
                    {
                        ReasonNotAttentionEntity previousEntity = addResult.Item2;
                        hdfReasonNotAttentionCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfReasonNotAttentionCodeEdit.Value = Convert.ToString(hiddenReasonNotAttentionCode);
                            txtActivateDeletedReasonNotAttentionNameSpanish.Text = previousEntity.ReasonNotAttentionDescriptionSpanish;
                            txtActivateDeletedReasonNotAttentionNameEnglish.Text = previousEntity.ReasonNotAttentionDescriptionEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }
                        else
                        {
                            txtDuplicatedReasonNotAttentionNameSpanish.Text = previousEntity.ReasonNotAttentionDescriptionSpanish;
                            txtDuplicatedReasonNotAttentionNameEnglish.Text = previousEntity.ReasonNotAttentionDescriptionEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }
                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = objReasonNotAttentionBll.Edit(new ReasonNotAttentionEntity(hiddenReasonNotAttentionCode
                          , ReasonNotAttentionNameSpanish
                          , ReasonNotAttentionNameEnglish
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
                        ReasonNotAttentionEntity previousEntity = result.Item2;
                        hdfReasonNotAttentionCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfReasonNotAttentionCodeEdit.Value = Convert.ToString(previousEntity.ReasonNotAttentionCode);
                            txtActivateDeletedReasonNotAttentionNameSpanish.Text = previousEntity.ReasonNotAttentionDescriptionSpanish;
                            txtActivateDeletedReasonNotAttentionNameEnglish.Text = previousEntity.ReasonNotAttentionDescriptionEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }
                        else
                        {
                            txtDuplicatedReasonNotAttentionNameSpanish.Text = previousEntity.ReasonNotAttentionDescriptionSpanish;
                            txtDuplicatedReasonNotAttentionNameEnglish.Text = previousEntity.ReasonNotAttentionDescriptionEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }


                }
                //
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention] != null)
                {
                    List<ReasonNotAttentionEntity> ReasonNotAttentionsBDList = objReasonNotAttentionBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention, ReasonNotAttentionsBDList);
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
                    short selectedReasonNotAttentionCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    ReasonNotAttentionEntity entity = objReasonNotAttentionBll.ListByKey(selectedReasonNotAttentionCode);

                    hdfReasonNotAttentionCodeEdit.Value = selectedReasonNotAttentionCode.ToString();
                    txtReasonNotAttentionNameSpanish.Text = entity.ReasonNotAttentionDescriptionSpanish;
                    txtReasonNotAttentionNameEnglish.Text = entity.ReasonNotAttentionDescriptionEnglish;
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
                    short selectedReasonNotAttentionCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    objReasonNotAttentionBll.Delete(new ReasonNotAttentionEntity(selectedReasonNotAttentionCode, UserHelper.GetCurrentFullUserName));

                    PageHelper<ReasonNotAttentionEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
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
                    if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention] != null)
                    {
                        List<ReasonNotAttentionEntity> ReasonNotAttentionsBDList = objReasonNotAttentionBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogReasonNotAttention, ReasonNotAttentionsBDList);
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

            PageHelper<ReasonNotAttentionEntity> pageHelper = SearchResults(1);
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
        private PageHelper<ReasonNotAttentionEntity> SearchResults(int page)
        {
            string ReasonNotAttentionNameSpanish = string.IsNullOrWhiteSpace(txtReasonNotAttentionNameSpanishFilter.Text.Trim()) ? null : txtReasonNotAttentionNameSpanishFilter.Text.Trim();
            string ReasonNotAttentionNameEnglish = string.IsNullOrWhiteSpace(txtReasonNotAttentionNameEnglishFilter.Text.Trim()) ? null : txtReasonNotAttentionNameEnglishFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<ReasonNotAttentionEntity> pageHelper = objReasonNotAttentionBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , ReasonNotAttentionNameSpanish
                , ReasonNotAttentionNameEnglish
                , sortExpression
                , sortDirection
                , page
                , null);

            return pageHelper;
        }
        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults(PageHelper<ReasonNotAttentionEntity> pageHelper)
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