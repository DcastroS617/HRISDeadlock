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
    public partial class DeprivationInstitution : System.Web.UI.Page
    {
        [Dependency]
        protected IDeprivationInstitutionBLL<DeprivationInstitutionEntity> objDeprivationInstitutionBll { get; set; }

        //session key for the results
        readonly string sessionKeySocialResponsabilityResults = "SocialResponsability-DeprivationInstitutionResults";

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
                    PageHelper<DeprivationInstitutionEntity> pageHelper = (PageHelper<DeprivationInstitutionEntity>)Session[sessionKeySocialResponsabilityResults];
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
                short hiddenDeprivationInstitutionCode = !string.IsNullOrWhiteSpace(hdfDeprivationInstitutionCodeEdit.Value) ?
                    Convert.ToInt16(hdfDeprivationInstitutionCodeEdit.Value) : (short)-1;

                string DeprivationInstitutionDesSpanish = txtDeprivationInstitutionDesSpanish.Text.Trim();
                string DeprivationInstitutionDesEnglish = txtDeprivationInstitutionDesEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    DeprivationInstitutionEntity dpe = new DeprivationInstitutionEntity();
                    dpe.DeprivationInstitutionDesEnglish = DeprivationInstitutionDesEnglish;
                    dpe.DeprivationInstitutionDesSpanish = DeprivationInstitutionDesSpanish;
                    dpe.DeprivationInstitutionCode = int.Parse(hiddenDeprivationInstitutionCode.ToString());
                    dpe.LastModifiedUser = lastModifiedUser;
                    objDeprivationInstitutionBll.Activate(dpe);
                }

                //update and activate the deleted item
                else
                {
                    DeprivationInstitutionEntity dpe = new DeprivationInstitutionEntity();
                    dpe.DeprivationInstitutionDesEnglish = DeprivationInstitutionDesEnglish;
                    dpe.DeprivationInstitutionDesSpanish = DeprivationInstitutionDesSpanish;
                    dpe.DeprivationInstitutionCode = int.Parse(hiddenDeprivationInstitutionCode.ToString());
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    objDeprivationInstitutionBll.Edit(dpe);
                }

                SearchResults(PagerUtil.GetActivePage(blstPager));
                DisplayResults();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);

                hdfSelectedRowIndex.Value = "-1";

                /*if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationInstitution] != null)
                {
                    List<DeprivationInstitutionEntity> DeprivationInstitutionBDList = objDeprivationInstitutionBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationInstitution, DeprivationInstitutionBDList);
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
                short hiddenDeprivationInstitutionCode = !string.IsNullOrWhiteSpace(hdfDeprivationInstitutionCodeEdit.Value) ?
                    Convert.ToInt16(hdfDeprivationInstitutionCodeEdit.Value) : (short)-1;

                string DeprivationInstitutionDesSpanish = txtDeprivationInstitutionDesSpanish.Text.Trim();
                string DeprivationInstitutionDesEnglish = txtDeprivationInstitutionDesEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenDeprivationInstitutionCode.Equals(-1))
                {
                    DeprivationInstitutionEntity dpe = new DeprivationInstitutionEntity();
                    dpe.DeprivationInstitutionDesEnglish = DeprivationInstitutionDesEnglish;
                    dpe.DeprivationInstitutionDesSpanish = DeprivationInstitutionDesSpanish;
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    Tuple<bool, DeprivationInstitutionEntity> addResult = objDeprivationInstitutionBll.Add(dpe);

                    hiddenDeprivationInstitutionCode = short.Parse(addResult.Item2.DeprivationInstitutionCode.ToString());
                    hdfDeprivationInstitutionCodeEdit.Value = addResult.Item2.DeprivationInstitutionCode.ToString();

                    if (addResult.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        DeprivationInstitutionEntity previousEntity = addResult.Item2;
                        hdfDeprivationInstitutionCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfDeprivationInstitutionCodeEdit.Value = Convert.ToString(hiddenDeprivationInstitutionCode);
                            txtActivateDeletedDeprivationInstitutionDesSpanish.Text = previousEntity.DeprivationInstitutionDesSpanish;
                            txtActivateDeletedDeprivationInstitutionDesEnglish.Text = previousEntity.DeprivationInstitutionDesEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            txtDuplicatedDeprivationInstitutionDesSpanish.Text = previousEntity.DeprivationInstitutionDesSpanish;
                            txtDuplicatedDeprivationInstitutionDesEnglish.Text = previousEntity.DeprivationInstitutionDesEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    DeprivationInstitutionEntity dpe = new DeprivationInstitutionEntity();
                    dpe.DeprivationInstitutionCode = hiddenDeprivationInstitutionCode;
                    dpe.DeprivationInstitutionDesEnglish = DeprivationInstitutionDesEnglish;
                    dpe.DeprivationInstitutionDesSpanish = DeprivationInstitutionDesSpanish;
                    dpe.SearchEnabled = searchEnable;
                    dpe.Deleted = deleted;
                    dpe.LastModifiedUser = lastModifiedUser;
                    var result = objDeprivationInstitutionBll.Edit(dpe);

                    if (result.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else
                    {
                        DeprivationInstitutionEntity previousEntity = result.Item2;
                        hdfDeprivationInstitutionCodeEdit.Value = "-1";

                        if (previousEntity.Deleted)
                        {
                            hdfDeprivationInstitutionCodeEdit.Value = Convert.ToString(previousEntity.DeprivationInstitutionCode);
                            txtActivateDeletedDeprivationInstitutionDesSpanish.Text = previousEntity.DeprivationInstitutionDesSpanish;
                            txtActivateDeletedDeprivationInstitutionDesEnglish.Text = previousEntity.DeprivationInstitutionDesEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedDeprivationInstitutionDesSpanish.Text = previousEntity.DeprivationInstitutionDesSpanish;
                            txtDuplicatedDeprivationInstitutionDesEnglish.Text = previousEntity.DeprivationInstitutionDesEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }
                
                /*if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationInstitution] != null)
                {
                    List<DeprivationInstitutionEntity> DeprivationInstitutionBDList = objDeprivationInstitutionBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationInstitution, DeprivationInstitutionBDList);
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
                    short selectedDeprivationInstitutionCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    DeprivationInstitutionEntity entity = objDeprivationInstitutionBll.ListByKey(selectedDeprivationInstitutionCode);

                    hdfDeprivationInstitutionCodeEdit.Value = selectedDeprivationInstitutionCode.ToString();
                    txtDeprivationInstitutionDesSpanish.Text = entity.DeprivationInstitutionDesSpanish;
                    txtDeprivationInstitutionDesEnglish.Text = entity.DeprivationInstitutionDesEnglish;
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
                    DeprivationInstitutionEntity dpe = new DeprivationInstitutionEntity();
                    
                    short selectedDeprivationInstitutionCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    dpe.DeprivationInstitutionCode = short.Parse(selectedDeprivationInstitutionCode.ToString());
                    dpe.LastModifiedUser = UserHelper.GetCurrentFullUserName;

                    objDeprivationInstitutionBll.Delete(dpe);

                    PageHelper<DeprivationInstitutionEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
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
                    /*if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationInstitution] != null)
                    {
                        List<DeprivationInstitutionEntity> DeprivationInstitutionBDList = objDeprivationInstitutionBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDeprivationInstitution, DeprivationInstitutionBDList);
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

            PageHelper<DeprivationInstitutionEntity> pageHelper = SearchResults(1);
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
        private PageHelper<DeprivationInstitutionEntity> SearchResults(int page)
        {
            string DeprivationInstitutionDesSpanish = string.IsNullOrWhiteSpace(txtDeprivationInstitutionDesSpanishFilter.Text.Trim()) ? null : txtDeprivationInstitutionDesSpanishFilter.Text.Trim();
            string DeprivationInstitutionDesEnglish = string.IsNullOrWhiteSpace(txtDeprivationInstitutionDesEnglishFilter.Text.Trim()) ? null : txtDeprivationInstitutionDesEnglishFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<DeprivationInstitutionEntity> pageHelper = objDeprivationInstitutionBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , DeprivationInstitutionDesSpanish
                , DeprivationInstitutionDesEnglish
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
                PageHelper<DeprivationInstitutionEntity> pageHelper = (PageHelper<DeprivationInstitutionEntity>)Session[sessionKeySocialResponsabilityResults];

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