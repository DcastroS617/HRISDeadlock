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
    public partial class DeprivationProcess : System.Web.UI.Page
    {
        [Dependency]
        protected IDeprivationProcessBLL<DeprivationProcessEntity> objDeprivationProcessBll { get; set; }

        //session key for the results
        readonly string sessionKeySocialResponsabilityResults = "SocialResponsability-DeprivationProcessResults";

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
                    BtnSearch_ServerClick(sender, e);
                }

                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    CultureGlobal = ci.Name;
                }

                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeySocialResponsabilityResults] != null)
                {
                    PageHelper<DeprivationProcessEntity> pageHelper = (PageHelper<DeprivationProcessEntity>)Session[sessionKeySocialResponsabilityResults];
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
                short hiddenDeprivationProcessCode = !string.IsNullOrWhiteSpace(hdfDeprivationProcessCodeEdit.Value) ?
                    Convert.ToInt16(hdfDeprivationProcessCodeEdit.Value) : (short)-1;

                string DeprivationProcessDesSpanish = txtDeprivationProcessDesSpanish.Text.Trim();
                string DeprivationProcessDesEnglish = txtDeprivationProcessDesEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    DeprivationProcessEntity dpe = new DeprivationProcessEntity();
                    dpe.DeprivationProcessDesEnglish = DeprivationProcessDesEnglish;
                    dpe.DeprivationProcessDesSpanish = DeprivationProcessDesSpanish;
                    dpe.DeprivationProcessCode = int.Parse(hiddenDeprivationProcessCode.ToString());
                    dpe.LastModifiedUser = lastModifiedUser;
                    objDeprivationProcessBll.Activate(dpe);
                }

                //update and activate the deleted item
                else
                {
                    DeprivationProcessEntity dpe = new DeprivationProcessEntity();
                    dpe.DeprivationProcessDesEnglish = DeprivationProcessDesEnglish;
                    dpe.DeprivationProcessDesSpanish = DeprivationProcessDesSpanish;
                    dpe.DeprivationProcessCode = int.Parse(hiddenDeprivationProcessCode.ToString());
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    objDeprivationProcessBll.Edit(dpe);
                }

                SearchResults(PagerUtil.GetActivePage(blstPager));
                DisplayResults();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);

                hdfSelectedRowIndex.Value = "-1";

                /*if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationProcess] != null)
                {
                    List<DeprivationProcessEntity> DeprivationProcessBDList = objDeprivationProcessBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationProcess, DeprivationProcessBDList);
                }*/
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
                short hiddenDeprivationProcessCode = !string.IsNullOrWhiteSpace(hdfDeprivationProcessCodeEdit.Value) ?
                    Convert.ToInt16(hdfDeprivationProcessCodeEdit.Value) : (short)-1;

                string DeprivationProcessDesSpanish = txtDeprivationProcessDesSpanish.Text.Trim();
                string DeprivationProcessDesEnglish = txtDeprivationProcessDesEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenDeprivationProcessCode.Equals(-1))
                {
                    DeprivationProcessEntity dpe = new DeprivationProcessEntity();
                    dpe.DeprivationProcessDesEnglish = DeprivationProcessDesEnglish;
                    dpe.DeprivationProcessDesSpanish = DeprivationProcessDesSpanish;
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    Tuple<bool, DeprivationProcessEntity> addResult = objDeprivationProcessBll.Add(dpe);

                    hiddenDeprivationProcessCode = short.Parse(addResult.Item2.DeprivationProcessCode.ToString());
                    hdfDeprivationProcessCodeEdit.Value = addResult.Item2.DeprivationProcessCode.ToString();

                    if (addResult.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        DeprivationProcessEntity previousEntity = addResult.Item2;
                        hdfDeprivationProcessCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfDeprivationProcessCodeEdit.Value = Convert.ToString(hiddenDeprivationProcessCode);
                            txtActivateDeletedDeprivationProcessDesSpanish.Text = previousEntity.DeprivationProcessDesSpanish;
                            txtActivateDeletedDeprivationProcessDesEnglish.Text = previousEntity.DeprivationProcessDesEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            txtDuplicatedDeprivationProcessDesSpanish.Text = previousEntity.DeprivationProcessDesSpanish;
                            txtDuplicatedDeprivationProcessDesEnglish.Text = previousEntity.DeprivationProcessDesEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    DeprivationProcessEntity dpe = new DeprivationProcessEntity();
                    dpe.DeprivationProcessCode = hiddenDeprivationProcessCode;
                    dpe.DeprivationProcessDesEnglish = DeprivationProcessDesEnglish;
                    dpe.DeprivationProcessDesSpanish = DeprivationProcessDesSpanish;
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    var result = objDeprivationProcessBll.Edit(dpe);

                    if (result.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else
                    {
                        DeprivationProcessEntity previousEntity = result.Item2;
                        hdfDeprivationProcessCodeEdit.Value = "-1";

                        if (previousEntity.Deleted)
                        {
                            hdfDeprivationProcessCodeEdit.Value = Convert.ToString(previousEntity.DeprivationProcessCode);
                            txtActivateDeletedDeprivationProcessDesSpanish.Text = previousEntity.DeprivationProcessDesSpanish;
                            txtActivateDeletedDeprivationProcessDesEnglish.Text = previousEntity.DeprivationProcessDesEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedDeprivationProcessDesSpanish.Text = previousEntity.DeprivationProcessDesSpanish;
                            txtDuplicatedDeprivationProcessDesEnglish.Text = previousEntity.DeprivationProcessDesEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }
                
                /*if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationProcess] != null)
                {
                    List<DeprivationProcessEntity> DeprivationProcessBDList = objDeprivationProcessBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationProcess, DeprivationProcessBDList);
                }*/
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
                    short selectedDeprivationProcessCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    DeprivationProcessEntity entity = objDeprivationProcessBll.ListByKey(selectedDeprivationProcessCode);

                    hdfDeprivationProcessCodeEdit.Value = selectedDeprivationProcessCode.ToString();
                    txtDeprivationProcessDesSpanish.Text = entity.DeprivationProcessDesSpanish;
                    txtDeprivationProcessDesEnglish.Text = entity.DeprivationProcessDesEnglish;
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
                    DeprivationProcessEntity dpe = new DeprivationProcessEntity();
                    
                    short selectedDeprivationProcessCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    dpe.DeprivationProcessCode = short.Parse(selectedDeprivationProcessCode.ToString());
                    dpe.LastModifiedUser = UserHelper.GetCurrentFullUserName;

                    objDeprivationProcessBll.Delete(dpe);

                    PageHelper<DeprivationProcessEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }

                    DisplayResults();

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);

                    //
                    /*if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationProcess] != null)
                    {
                        List<DeprivationProcessEntity> DeprivationProcessBDList = objDeprivationProcessBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationProcess, DeprivationProcessBDList);
                    }*/
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

            PageHelper<DeprivationProcessEntity> pageHelper = SearchResults(1);
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
        private PageHelper<DeprivationProcessEntity> SearchResults(int page)
        {
            string DeprivationProcessDesSpanish = string.IsNullOrWhiteSpace(txtDeprivationProcessDesSpanishFilter.Text.Trim()) ? null : txtDeprivationProcessDesSpanishFilter.Text.Trim();
            string DeprivationProcessDesEnglish = string.IsNullOrWhiteSpace(txtDeprivationProcessDesEnglishFilter.Text.Trim()) ? null : txtDeprivationProcessDesEnglishFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<DeprivationProcessEntity> pageHelper = objDeprivationProcessBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , DeprivationProcessDesSpanish
                , DeprivationProcessDesEnglish
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
                PageHelper<DeprivationProcessEntity> pageHelper = (PageHelper<DeprivationProcessEntity>)Session[sessionKeySocialResponsabilityResults];

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