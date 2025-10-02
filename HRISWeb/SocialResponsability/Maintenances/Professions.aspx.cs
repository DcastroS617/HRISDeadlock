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
    public partial class Professions : System.Web.UI.Page
    {
        [Dependency]
        protected IProfessionsBll<ProfessionEntity> objProfessionsBll { get; set; }

        //session key for the results
        readonly string sessionKeySocialResponsabilityResults = "SocialResponsability-ProfessionsResults";

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
                    PageHelper<ProfessionEntity> pageHelper = (PageHelper<ProfessionEntity>)Session[sessionKeySocialResponsabilityResults];
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
                short hiddenProfessionCode = !string.IsNullOrWhiteSpace(hdfProfessionCodeEdit.Value) ?
                    Convert.ToInt16(hdfProfessionCodeEdit.Value) : (short)-1;

                string professionNameSpanish = txtProfessionNameSpanish.Text.Trim();
                string professionNameEnglish = txtProfessionNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    objProfessionsBll.Activate(new ProfessionEntity(hiddenProfessionCode, lastModifiedUser));
                }

                //update and activate the deleted item
                else
                {
                    objProfessionsBll.Edit(new ProfessionEntity(hiddenProfessionCode
                        , professionNameSpanish
                        , professionNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser));
                }

                SearchResults(PagerUtil.GetActivePage(blstPager));
                DisplayResults();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);

                hdfSelectedRowIndex.Value = "-1";

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions] != null)
                {
                    List<ProfessionEntity> professionsBDList = objProfessionsBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions, professionsBDList);
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
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short hiddenProfessionCode = !string.IsNullOrWhiteSpace(hdfProfessionCodeEdit.Value) ?
                    Convert.ToInt16(hdfProfessionCodeEdit.Value) : (short)-1;

                string professionNameSpanish = txtProfessionNameSpanish.Text.Trim();
                string professionNameEnglish = txtProfessionNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (hiddenProfessionCode.Equals(-1))
                {
                    Tuple<bool, ProfessionEntity> addResult = objProfessionsBll.Add(new ProfessionEntity(professionNameSpanish
                        , professionNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser));

                    hiddenProfessionCode = addResult.Item2.ProfessionCode;
                    hdfProfessionCodeEdit.Value = addResult.Item2.ProfessionCode.ToString();

                    if (addResult.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        ProfessionEntity previousEntity = addResult.Item2;
                        hdfProfessionCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfProfessionCodeEdit.Value = Convert.ToString(hiddenProfessionCode);
                            txtActivateDeletedProfessionNameSpanish.Text = previousEntity.ProfessionNameSpanish;
                            txtActivateDeletedProfessionNameEnglish.Text = previousEntity.ProfessionNameEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            txtDuplicatedProfessionNameSpanish.Text = previousEntity.ProfessionNameSpanish;
                            txtDuplicatedProfessionNameEnglish.Text = previousEntity.ProfessionNameEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = objProfessionsBll.Edit(new ProfessionEntity(hiddenProfessionCode
                          , professionNameSpanish
                          , professionNameEnglish
                          , searchEnable
                          , deleted
                          , lastModifiedUser));

                    if (result.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else
                    {
                        ProfessionEntity previousEntity = result.Item2;
                        hdfProfessionCodeEdit.Value = "-1";

                        if (previousEntity.Deleted)
                        {
                            hdfProfessionCodeEdit.Value = Convert.ToString(previousEntity.ProfessionCode);
                            txtActivateDeletedProfessionNameSpanish.Text = previousEntity.ProfessionNameSpanish;
                            txtActivateDeletedProfessionNameEnglish.Text = previousEntity.ProfessionNameEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedProfessionNameSpanish.Text = previousEntity.ProfessionNameSpanish;
                            txtDuplicatedProfessionNameEnglish.Text = previousEntity.ProfessionNameEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }
                
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions] != null)
                {
                    List<ProfessionEntity> professionsBDList = objProfessionsBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions, professionsBDList);
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
                    short selectedProfessionCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    ProfessionEntity entity = objProfessionsBll.ListByKey(selectedProfessionCode);

                    hdfProfessionCodeEdit.Value = selectedProfessionCode.ToString();
                    txtProfessionNameSpanish.Text = entity.ProfessionNameSpanish;
                    txtProfessionNameEnglish.Text = entity.ProfessionNameEnglish;
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
                    short selectedProfessionCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    objProfessionsBll.Delete(new ProfessionEntity(selectedProfessionCode, UserHelper.GetCurrentFullUserName));

                    PageHelper<ProfessionEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
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
                    if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions] != null)
                    {
                        List<ProfessionEntity> professionsBDList = objProfessionsBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions, professionsBDList);
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

            PageHelper<ProfessionEntity> pageHelper = SearchResults(1);
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
        private PageHelper<ProfessionEntity> SearchResults(int page)
        {
            string professionNameSpanish = string.IsNullOrWhiteSpace(txtProfessionNameSpanishFilter.Text.Trim()) ? null : txtProfessionNameSpanishFilter.Text.Trim();
            string professionNameEnglish = string.IsNullOrWhiteSpace(txtProfessionNameEnglishFilter.Text.Trim()) ? null : txtProfessionNameEnglishFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<ProfessionEntity> pageHelper = objProfessionsBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , professionNameSpanish
                , professionNameEnglish
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
                PageHelper<ProfessionEntity> pageHelper = (PageHelper<ProfessionEntity>)Session[sessionKeySocialResponsabilityResults];

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