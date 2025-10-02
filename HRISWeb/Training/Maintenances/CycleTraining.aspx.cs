using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Training.Maintenances
{
    public partial class CycleTraining : Page
    {
        [Dependency]
        public ICycleTrainingBll ObjCycleTrainingBll { get; set; }

        readonly string sessionKeyCycleTrainingResults = "Trainers-CycleTrainingResults";

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
                    BtnSearch_ServerClick(sender, e);
                }

                //activate the pager
                if (Session[sessionKeyCycleTrainingResults] != null)
                {
                    PageHelper<CycleTrainingEntity> pageHelper = (PageHelper<CycleTrainingEntity>)Session[sessionKeyCycleTrainingResults];
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var CycleTrainingId = hdfCycleTrainingIdEdit.Value == "-1" || string.IsNullOrEmpty(hdfCycleTrainingIdEdit.Value) ? (int?)null : int.Parse(hdfCycleTrainingIdEdit.Value);

                var entity = new CycleTrainingEntity
                {
                    CycleTrainingId = CycleTrainingId,
                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    CycleTrainingCode = CycleTrainingCodeEdit.Text.Trim(),
                    CycleTrainingName = CycleTrainingNameEdit.Text.Trim(),
                    CycleTrainingStartDate = string.IsNullOrEmpty(FromDateEdit.Value) ? null : ToDateEN(FromDateEdit.Value),
                    CycleTrainingEndDate = string.IsNullOrEmpty(FromDateEdit.Value) ? null : ToDateEN(ToDateEdit.Value),
                    SearchEnabled = SearchEnabledEdit.Checked,
                    Deleted = false,
                    LastModifiedUser = UserHelper.GetCurrentFullUserName
                };

                CycleTrainingEntity result = null;
                if (CycleTrainingId.HasValue)
                {
                    //Editar
                    result = ObjCycleTrainingBll.CycleTrainingEdit(entity);
                }
                else
                {
                    //Insertar
                    result = ObjCycleTrainingBll.CycleTrainingAdd(entity);
                }

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBack(); },200);", true);
                }

                else if (result.ErrorNumber != -1 && result.ErrorNumber != -2)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                else
                {
                    if (result.Deleted)
                    {
                        hdfActivateDeletedCycleTrainingId.Value = result.CycleTrainingId?.ToString();
                        txtActivateDeletedCycleTrainingCode.Text = result.CycleTrainingCode;
                        txtActivateDeletedCycleTrainingName.Text = result.CycleTrainingName;

                        var typetext = result.ErrorNumber == -1 ?
                           GetLocalResourceObject("CycleTrainingCodeHeaderText").ToString() : GetLocalResourceObject("CycleTrainingNameHeaderText").ToString();

                        divActivateDeletedDialog.InnerHtml = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "setTimeout(function () {ReturnFromBtnAcceptClickPostBackDeleted();},200); ", true);
                    }

                    else
                    {
                        txtDuplicatedCycleTrainingCode.Text = result.CycleTrainingCode;
                        txtDuplicatedTrainerCode.Text = result.CycleTrainingName;

                        var typetext = result.ErrorNumber == -1 ?
                            GetLocalResourceObject("CycleTrainingCodeHeaderText").ToString() : GetLocalResourceObject("CycleTrainingNameHeaderText").ToString();

                        divDuplicatedDialogText.InnerHtml = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBackDuplicated(); },200); ", true);
                    }
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
        protected void BtnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CycleTrainingId"]);

                    var result = ObjCycleTrainingBll.CycleTrainingByKey(new CycleTrainingEntity
                    {
                        CycleTrainingId = selectedid
                    });

                    hdfCycleTrainingIdEdit.Value = result.CycleTrainingId?.ToString();
                    CycleTrainingCodeEdit.Text = result.CycleTrainingCode.Trim();
                    CycleTrainingNameEdit.Text = result.CycleTrainingName.Trim();
                    FromDateEdit.Value = result.CycleTrainingStartDate?.ToString("MM/dd/yyyy");
                    ToDateEdit.Value = result.CycleTrainingEndDate?.ToString("MM/dd/yyyy");
                    SearchEnabledEdit.Checked = result.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnFromBtnEditClickPostBack('" + ToDateEdit.Value + "'); },200);  ", true);
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
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void BtnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var CycleTrainingId = hdfActivateDeletedCycleTrainingId.Value == "-1" || string.IsNullOrEmpty(hdfActivateDeletedCycleTrainingId.Value) ? (int?)null : int.Parse(hdfActivateDeletedCycleTrainingId.Value);

                if (CycleTrainingId.HasValue)
                {
                    var updateEntity = new CycleTrainingEntity();

                    if (chbActivateDeleted.Checked)
                    {
                        updateEntity = new CycleTrainingEntity
                        {
                            CycleTrainingId = CycleTrainingId,
                            GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            SearchEnabled = true,
                            Deleted = false,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        };
                    }

                    else
                    {
                        updateEntity = new CycleTrainingEntity
                        {
                            CycleTrainingId = CycleTrainingId,
                            GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            CycleTrainingCode = CycleTrainingCodeEdit.Text.Trim(),
                            CycleTrainingName = CycleTrainingNameEdit.Text.Trim(),
                            SearchEnabled = SearchEnabledEdit.Checked,
                            Deleted = false,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        };
                    }

                    var result = ObjCycleTrainingBll.CycleTrainingEdit(updateEntity);
                    if (result.ErrorNumber == 0)
                    {
                        hdfSelectedRowIndex.Value = "-1";
                        hdfActivateDeletedCycleTrainingId.Value = "-1";
                        RefreshTable();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptActivateDeletedClickPostBack(); },200);  ", true);
                    }

                    else if (result.ErrorNumber != -1 && result.ErrorNumber != -2)
                    {
                        Exception exception = new Exception(result.ErrorMessage);
                        throw exception;
                    }

                    else
                    {
                        if (result.Deleted)
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")));
                        }

                        else
                        {
                            var typetext = result.ErrorNumber == -1 ? GetLocalResourceObject("CycleTrainingCodeHeaderText").ToString() : GetLocalResourceObject("CycleTrainingNameHeaderText").ToString();

                            var msgValidate = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(msgValidate));
                        }
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msj000.Text")));
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
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedCycleTrainingId = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CycleTrainingId"]);

                    ObjCycleTrainingBll.CycleTrainingEdit(
                        new CycleTrainingEntity
                        {
                            GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            CycleTrainingId = selectedCycleTrainingId,
                            SearchEnabled = false,
                            Deleted = true,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                    });

                    PageHelper<CycleTrainingEntity> pageHelper = (PageHelper<CycleTrainingEntity>)Session[sessionKeyCycleTrainingResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.CycleTrainingId == selectedCycleTrainingId));
                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page , TipoMensaje.Error , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page , TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
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
        protected void GrvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0) || (grvList.ShowHeaderWhenEmpty))
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
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyCycleTrainingResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<CycleTrainingEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Methods

        public DateTime? ToDateEN(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;

                string[] formats = { "MM/dd/yyyy", "MM/d/yyyy", "M/dd/yyyy", "M/d/yyyy" };

                DateTime.TryParseExact(dateString, formats, dtfi, DateTimeStyles.None, out DateTime Date);
                return Date;
            }

            else
            {
                return null;
            }
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<CycleTrainingEntity> SearchResults(int page)
        {
            var Filter = new CycleTrainingEntity
            {
                CycleTrainingCode = string.IsNullOrWhiteSpace(CycleTrainingCodeFilter.Text) ? null : CycleTrainingCodeFilter.Text,
                CycleTrainingName = string.IsNullOrWhiteSpace(CycleTrainingNameFilter.Text) ? null : CycleTrainingNameFilter.Text, 
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
            };

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<CycleTrainingEntity> pageHelper = ObjCycleTrainingBll.CycleTrainingListByFilter(
                Filter,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                sortExpression, sortDirection, page);

            Session[sessionKeyCycleTrainingResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyCycleTrainingResults] != null)
            {
                PageHelper<CycleTrainingEntity> pageHelper = (PageHelper<CycleTrainingEntity>)Session[sessionKeyCycleTrainingResults];

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

        private void RefreshTable()
        {
            SearchResults(PagerUtil.GetActivePage(blstPager));
            DisplayResults();
        }

        #endregion
    }
}