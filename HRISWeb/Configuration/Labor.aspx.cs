using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.Business.Remote;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Services.CR.Business;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Configuration
{
    public partial class Labor : Page
    {
        [Dependency]
        public ILaborBll ObjILaborBll { get; set; }

        //session key for the results
        readonly string sessionKeyLaborConfigurationResults = "LaborConfiguration-LaborConfigurationResults";

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
                    LaborRegionalBll objLaborRegional = new LaborRegionalBll(1);
                    var listaLaborRegional = objLaborRegional.ListarLaborRegional();

                    if (listaLaborRegional != null)
                    {
                        ObjILaborBll.LaborRegionalInsert(listaLaborRegional.ToDataTableGet());
                    }

                    BtnSearch_ServerClick(sender, e);

                    LaborRegionalCodeEdit.Items.Clear();
                    LaborRegionalCodeEdit.Items.AddRange(ObjILaborBll.LaborRegionalList(null, 
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode));
                }

                if (Session[sessionKeyLaborConfigurationResults] != null)
                {
                    PageHelper<LaborEntity> pageHelper = (PageHelper<LaborEntity>)Session[sessionKeyLaborConfigurationResults];
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
        /// Handles the btnAddClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddClick_ServerClick(object sender, EventArgs e)
        {
            try
            {
                LaborRegionalCodeEdit.Items.Clear();
                LaborRegionalCodeEdit.Items.AddRange(ObjILaborBll.LaborRegionalList(null, 
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode));

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnRequestBtnAddOpen{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnAddOpen(); },200);  ", true);
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
                var seleectid = LaborIdEdit.Value == "-1" || string.IsNullOrEmpty(LaborIdEdit.Value) ? (int?)null : int.Parse(LaborIdEdit.Value);

                var entity = new LaborEntity
                {
                    LaborId = seleectid,
                    LaborCode = LaborCodeEdit.Value?.Trim(),
                    LaborName = LaborNameEdit.Value?.Trim(),
                    LaborRegionalCode = LaborRegionalCodeEdit.Value,
                    Orders = int.Parse(OrdersEdit.Value?.Trim()),
                    DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SearchEnabled = SearchEnabledEdit.Checked,
                };

                DbaEntity result = null;
                if (seleectid.HasValue)
                {
                    result = ObjILaborBll.LaborEdit(entity);
                }
                else
                {
                    result = ObjILaborBll.LaborAdd(entity);
                }

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);", true);
                }

                else if (result.ErrorNumber == -3)
                {
                    DisplayResults();
                    var editresult = seleectid.HasValue ? 1 : 0;

                    MensajeriaHelper.MostrarMensaje(Page, 
                        TipoMensaje.Advertencia, 
                        GetLocalResourceObject("msgorderduplicate").ToString());

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnPostBackAcceptClickSave2(" + editresult + "); },200);", true);
                }

                else if (result.ErrorNumber != -1)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                else
                {
                    DisplayResults();

                    var duplicado = ObjILaborBll.LaborById(new LaborEntity
                    {
                        LaborCode = entity.LaborCode
                    });

                    txtDuplicatedLaborCode.Text = duplicado.LaborCode;
                    txtDuplicatedLaborName.Text = duplicado.LaborName;
                    var typetext = GetLocalResourceObject("CodeLbl").ToString();

                    divDuplicatedDialogText.InnerHtml =
                        string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(),
                        typetext);

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBackDuplicated(); },200); ", true);
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnPostBackAcceptClickSave2(); },200);", true);
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
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["LaborId"]);

                    var result = ObjILaborBll.LaborById(new LaborEntity
                    {
                        LaborId = selectedid
                    });

                    LaborIdEdit.Value = result.LaborId.ToString();
                    LaborCodeEdit.Value = result.LaborCode.ToString();
                    LaborNameEdit.Value = result.LaborName;
                    SearchEnabledEdit.Checked = result.SearchEnabled;
                    OrdersEdit.Value = result.Orders?.ToString();

                    LaborRegionalCodeEdit.Items.Clear();
                    LaborRegionalCodeEdit.Items.AddRange(ObjILaborBll.LaborRegionalList(result.LaborRegionalCode, 
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode));

                    LaborRegionalCodeEdit.Value = result.LaborRegionalCode;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEdit_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(); },200);  ", true);

                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
                }

                else
                {
                    LaborIdEdit.Value = "";
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
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["LaborId"]);
                    var result = ObjILaborBll.LaborDelete(
                        new LaborEntity
                        {
                            LaborId = selectedid,
                            Deleted = true,
                        });

                    if (result.ErrorNumber == 0)
                    {
                        RefreshTable(true);
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);
                    }

                    else if (result.ErrorNumber == -2)
                    {
                        MensajeriaHelper.MostrarMensaje(Page, 
                            TipoMensaje.Advertencia, 
                            GetLocalResourceObject("msj005.Text").ToString());
                    }

                    else
                    {
                        Exception exception = new Exception(result.ErrorMessage);
                        throw exception;
                    }

                    hdfSelectedRowIndex.Value = "-1";
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
            if (Session[sessionKeyLaborConfigurationResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<LaborEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<LaborEntity> SearchResults(int page)
        {
            var Filter = new LaborEntity
            {
                LaborCode = string.IsNullOrEmpty(LaborCodeFilter.Value) ? null : LaborCodeFilter.Value,
                LaborName = string.IsNullOrEmpty(LaborNameFilter.Value) ? null : LaborNameFilter.Value,
                LaborRegionalCode = string.IsNullOrEmpty(LaborRegionalFilter.Value) ? null : LaborRegionalFilter.Value
            };

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<LaborEntity> pageHelper = ObjILaborBll.LaborListByFilter(
                Filter, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                sortExpression, sortDirection, page);

            Session[sessionKeyLaborConfigurationResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyLaborConfigurationResults] != null)
            {
                PageHelper<LaborEntity> pageHelper = (PageHelper<LaborEntity>)Session[sessionKeyLaborConfigurationResults];

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

        private void RefreshTable(bool IsDelete = false)
        {
            if (IsDelete)
            {
                var rows = grvList.Rows.Count - 1;
                if (rows < 1)
                {
                    SearchResults(0);
                    DisplayResults();
                }

                else
                {
                    SearchResults(PagerUtil.GetActivePage(blstPager));
                    DisplayResults();
                }
            }

            else
            {
                SearchResults(PagerUtil.GetActivePage(blstPager));
                DisplayResults();
            }
        }

        #endregion

    }
}