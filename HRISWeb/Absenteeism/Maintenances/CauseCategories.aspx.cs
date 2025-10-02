using HRISWeb.Shared;
using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using System;
using System.Web.UI;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity.Attributes;
using DOLE.HRIS.Shared.Entity;
using System.Linq;

namespace HRISWeb.Absenteeism
{
    public partial class CauseCategories : System.Web.UI.Page
    {
        [Dependency]
        public IAbsenteeismCauseCategoriesBll<AbsenteeismCauseCategoryEntity> objCauseCategoriesBll { get; set; }


        //session key for the results
        readonly string sessionKeyCauseCategorysResults = "CauseCategories-CauseCategoriesResults";

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
                    
                    //fire the event
                    btnSearch_ServerClick(sender, e);
                }

                chbSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chbSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));
                
                //activate the pager
                if (Session[sessionKeyCauseCategorysResults] != null)
                {
                    PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = (PageHelper<AbsenteeismCauseCategoryEntity>)Session[sessionKeyCauseCategorysResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
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
        
        /// <summary>
        /// Handles the pre render of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {

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
        
        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {                
                hdfCauseCategoryCodeFilter.Value = txtCauseCategoryCodeFilter.Text;
                hdfCauseCategoryNameFilter.Value = txtCauseCategoryNameFilter.Text;
                                
                PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = SearchResults(1);                
                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

                DisplayResults();
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
               
        /// <summary>
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void btnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string causeCategoryCode =
                    !string.IsNullOrWhiteSpace(hdfActivateDeletedCauseCategoryCode.Value) ?
                        hdfActivateDeletedCauseCategoryCode.Value :
                        null;

                AbsenteeismCauseCategoryEntity causeCategory =
                    new AbsenteeismCauseCategoryEntity(
                        causeCategoryCode,
                        txtCauseCategoryName.Text.Trim(),
                        txtComments.Text.Trim(),                        
                        chbSearchEnabled.Checked,
                        true,
                        UserHelper.GetCurrentFullUserName,
                        DateTime.Now
                        );
                
                PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = (PageHelper<AbsenteeismCauseCategoryEntity>)Session[sessionKeyCauseCategorysResults];

                //activate the deleted item
                if (chbActivateDeleted.Checked)
                {
                    objCauseCategoriesBll.Activate(causeCategory);

                    if (pageHelper != null)
                    {
                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();
                    }

                    hdfSelectedRowIndex.Value = "0";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }
                
                //update and activate the deleted item
                else
                {                    
                    causeCategory.Deleted = false;

                    objCauseCategoriesBll.Edit(causeCategory);
                    
                    if (pageHelper != null)
                    {
                        string position = hdfSelectedRowIndex.Value;

                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();

                        hdfSelectedRowIndex.Value = position;
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                                        
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

        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string causeCategoryCode =
                    !string.IsNullOrWhiteSpace(hdfCauseCategoryCodeEdit.Value) ?
                        hdfCauseCategoryCodeEdit.Value :
                        txtCauseCategoryCode.Text.Trim();

                AbsenteeismCauseCategoryEntity causeCategory = new AbsenteeismCauseCategoryEntity(
                    causeCategoryCode,
                    txtCauseCategoryName.Text.Trim(),
                    txtComments.Text.Trim(),
                    chbSearchEnabled.Checked,
                    false,
                    UserHelper.GetCurrentFullUserName,
                    DateTime.Now);

                PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = (PageHelper<AbsenteeismCauseCategoryEntity>)Session[sessionKeyCauseCategorysResults];

                if (string.IsNullOrWhiteSpace(hdfCauseCategoryCodeEdit.Value))
                {
                    Tuple<bool, AbsenteeismCauseCategoryEntity> addResult = objCauseCategoriesBll.Add(causeCategory);
                    if (addResult.Item1)
                    {
                        if(pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();                            
                        }

                        hdfSelectedRowIndex.Value = "-1";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
                    }

                    else if (!addResult.Item1)
                    {
                        AbsenteeismCauseCategoryEntity previousCauseCategory = addResult.Item2;

                        txtActivateDeletedCauseCategoryCode.Text = previousCauseCategory.CauseCategoryCode;
                        txtActivateDeletedCauseCategoryName.Text = previousCauseCategory.CauseCategoryName;
                        hdfActivateDeletedCauseCategoryCode.Value = previousCauseCategory.CauseCategoryCode;
                        
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackDeleted(); }}, 200);", true);
                    }
                }
                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    objCauseCategoriesBll.Edit(causeCategory);
                    
                    if (pageHelper != null)
                    {                        
                        pageHelper.ResultList.Remove(
                            pageHelper.ResultList.Find(x => x.CauseCategoryCode == causeCategory.CauseCategoryCode)
                        );
                        pageHelper.ResultList.Insert(Convert.ToInt32(hdfSelectedRowIndex.Value), causeCategory);
                        DisplayResults();                     
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
                                        
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
                    string selectedCauseCategoryCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CauseCategoryCode"]);

                    AbsenteeismCauseCategoryEntity causeCategory = objCauseCategoriesBll.ListByKey(selectedCauseCategoryCode);

                    hdfCauseCategoryCodeEdit.Value = causeCategory.CauseCategoryCode;

                    txtCauseCategoryCode.Text = causeCategory.CauseCategoryCode;
                    txtCauseCategoryCode.Enabled = false;
                    txtCauseCategoryName.Text = causeCategory.CauseCategoryName;                    
                    txtComments.Text = causeCategory.Comments;
                    chbSearchEnabled.Checked = causeCategory.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);

                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
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
                    string selectedCauseCategoryCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CauseCategoryCode"]);

                    objCauseCategoriesBll.Delete(
                        new AbsenteeismCauseCategoryEntity(
                            selectedCauseCategoryCode,
                            UserHelper.GetCurrentFullUserName));

                    PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = (PageHelper<AbsenteeismCauseCategoryEntity>)Session[sessionKeyCauseCategorysResults];

                    pageHelper.ResultList.Remove(
                        pageHelper.ResultList.Find(x => x.CauseCategoryCode == selectedCauseCategoryCode)
                        );

                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    DisplayResults();

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
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
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvList.ShowFooter && grvList.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
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
            if (Session[sessionKeyCauseCategorysResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                
                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<AbsenteeismCauseCategoryEntity> SearchResults(int page)
        {
            string causeCategoryCode = string.IsNullOrWhiteSpace(hdfCauseCategoryCodeFilter.Value) ? null : hdfCauseCategoryCodeFilter.Value;
            string causeCategoryName = string.IsNullOrWhiteSpace(hdfCauseCategoryNameFilter.Value) ? null : hdfCauseCategoryNameFilter.Value;
                        
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            
            PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = objCauseCategoriesBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                causeCategoryCode , causeCategoryName,
                sortExpression, sortDirection, page);

            Session[sessionKeyCauseCategorysResults] = pageHelper;
            return pageHelper;
        }


        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyCauseCategorysResults] != null)
            {
                PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = (PageHelper<AbsenteeismCauseCategoryEntity>)Session[sessionKeyCauseCategorysResults];
                                
                grvList.DataSource = pageHelper.ResultList;
                grvList.DataBind();
                
                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

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