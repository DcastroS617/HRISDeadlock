using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Training.Maintenances
{
    public partial class CarrerMap : Page
    {
        [Dependency]
        public IMapCatalogPositionsBll ObjMapCatalogPositionBll { get; set; }

        [Dependency]
        public IPositionsBll<PositionEntity> ObjIPositionsBll { get; set; }

        //session key for the results
        readonly string sessionKeyCarrerMap = "CarrerMap-CarrerMapResults";

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

                    var Geografia = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                    CategoryFTEIdEdit.Items.AddRange(ObjMapCatalogPositionBll.CategoryFTEListEnabled());

                    var positionsList = ObjIPositionsBll.ListEnabled().OrderBy(r => r.PositionName).Select(r => new ListItem(r.PositionName, r.PositionCode)).ToArray();
                    PositionCodeEdit.Items.AddRange(positionsList);
                    PositionCodeFilter.Items.Add(new ListItem("", ""));
                    PositionCodeFilter.Items.AddRange(positionsList);

                    CompanyIDEdit.Items.AddRange(ObjMapCatalogPositionBll.CompaniesListEnable(Geografia));
                    NominalClassIdEdit.Items.AddRange(ObjMapCatalogPositionBll.NominalClassListEnabled(Geografia));
                    PaymentRateCodeEdit.Items.AddRange(ObjMapCatalogPositionBll.PaymentRatesListByGeographicDivision(Geografia));
                }

                if (Session[sessionKeyCarrerMap] != null)
                {
                    PageHelper<MapCatalogPositionsEntity> pageHelper = (PageHelper<MapCatalogPositionsEntity>)Session[sessionKeyCarrerMap];
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
                var MapCatalogPositionsId = hdfMapCatalogPositionsIdEdit.Value == "-1" || string.IsNullOrEmpty(hdfMapCatalogPositionsIdEdit.Value)
                    ? (int?)null : int.Parse(hdfMapCatalogPositionsIdEdit.Value);

                var entity = new MapCatalogPositionsEntity
                {
                    MapCatalogPositionsId = MapCatalogPositionsId,
                    CategoryFTEId = int.Parse(CategoryFTEIdEdit.Value),
                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    PositionCode = PositionCodeEdit.Value,
                    PaymentRateCode = null,
                    Deleted = false,
                };

                if (entity.CategoryFTEId == 2)
                {
                    entity.PaymentRateCode = short.Parse(PaymentRateCodeEdit.Value);
                }

                var DTCompanyIDEditMultiple = CompanyIDEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto { Id = int.Parse(r) }).ToList().ToDataTableGet();
                var DTNominalClassIdEdit = NominalClassIdEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto { Code = r }).ToList().ToDataTableGet();

                DbaEntity result = null;
                if (MapCatalogPositionsId.HasValue)
                {
                    result = ObjMapCatalogPositionBll.MapCatalogPositionsEdit(entity, DTCompanyIDEditMultiple, DTNominalClassIdEdit);
                }

                else
                {
                    result = ObjMapCatalogPositionBll.MapCatalogPositionsAdd(entity, DTCompanyIDEditMultiple, DTNominalClassIdEdit);
                }

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);", true);
                }

                else if (result.ErrorNumber != -1)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                else
                {
                    DisplayResults();

                    var PosicionName = ObjIPositionsBll.ListEnabled().FirstOrDefault(R => R.PositionCode == entity.PositionCode);

                    txtDuplicatedPositionName.Text = PosicionName?.PositionName;
                    var typetext = GetLocalResourceObject("lblPositionCodeEdit").ToString();

                    divDuplicatedDialogText.InnerHtml =
                            string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBackDuplicated(); },200); ", true);
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
                btnAdd.Disabled = true;
                btnDelete.Disabled = true;
                btnEdit.Disabled = true;
                CompanyIDEditMultiple.Value = "";
                NominalClassIdEditMultiple.Value = "";

                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MapCatalogPositionsId"]);

                    var ResultMulti = ObjMapCatalogPositionBll.MapCatalogPositionsById(new MapCatalogPositionsEntity
                    {
                        MapCatalogPositionsId = selectedid
                    });

                    var result = ResultMulti.Item1;
                    var Companies = ResultMulti.Item2;
                    var NominalClass = ResultMulti.Item3;
                    var Payrrate = ResultMulti.Item4;

                    hdfMapCatalogPositionsIdEdit.Value = result.MapCatalogPositionsId.ToString();
                    CategoryFTEIdEdit.Value = result.CategoryFTEId.ToString();
                    PositionCodeEdit.Value = result.PositionCode;
                    CompanyIDEditMultiple.Value = string.Join(",", Companies.Select(r => r.CompanyID.ToString()));
                    NominalClassIdEditMultiple.Value = string.Join(",", NominalClass.Select(r => r.NominalClassId));

                    PaymentRateCodeEdit.Value = Payrrate?.PaymentRateCode?.ToString() ?? "";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEdit_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(); },200);  ", true);
                }

                else
                {
                    hdfMapCatalogPositionsIdEdit.Value = "";
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
                    int selectedMapCatalogPositionsId = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MapCatalogPositionsId"]);

                    var result = ObjMapCatalogPositionBll.MapCatalogPositionsDesactiveteOrActive(
                        new MapCatalogPositionsEntity
                        {
                            MapCatalogPositionsId = selectedMapCatalogPositionsId,
                            Deleted = true,
                        });

                    if (result.ErrorNumber == 0)
                    {
                        PageHelper<MapCatalogPositionsEntity> pageHelper = (PageHelper<MapCatalogPositionsEntity>)Session[sessionKeyCarrerMap];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.MapCatalogPositionsId == selectedMapCatalogPositionsId));
                        pageHelper.TotalResults--;

                        if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                        {
                            SearchResults(pageHelper.TotalPages - 1);
                            PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                        }

                        pageHelper.UpdateTotalPages();
                        RefreshTable();
                    }

                    else
                    {
                        Exception exception = new Exception(result.ErrorMessage);
                        throw exception;
                    }

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);
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
            if (Session[sessionKeyCarrerMap] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<MapCatalogPositionsEntity> pageHelper = SearchResults(1);
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
        private PageHelper<MapCatalogPositionsEntity> SearchResults(int page)
        {
            Session[sessionKeyCarrerMap] = null;

            var Filter = new MapCatalogPositionsEntity
            {
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                PositionCode = string.IsNullOrEmpty(PositionCodeFilter.Value) ? null : PositionCodeFilter.Value
            };

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            int DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            PageHelper<MapCatalogPositionsEntity> pageHelper = ObjMapCatalogPositionBll.MapCatalogPositionsListByFilter(
                Filter, DivisionCode, sortExpression, sortDirection, page);

            Session[sessionKeyCarrerMap] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyCarrerMap] != null)
            {
                PageHelper<MapCatalogPositionsEntity> pageHelper = (PageHelper<MapCatalogPositionsEntity>)Session[sessionKeyCarrerMap];

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