using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.SocialResponsability.Maintenances
{
    public partial class Nationalities : System.Web.UI.Page
    {
        [Dependency]
        protected IPoliticalDivisionsBll<PoliticalDivisionEntity> objPoliticalDivisionsBll { get; set; }

        public string Gridresult { get; set; } = "GridResultIEmployeeTaskBll";

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
                if (Request.Form["__EVENTTARGET"] != null)
                {
                    if (Request.Form["__EVENTTARGET"].Equals("ctl00$cntBody$btnLoadProvince"))
                    {
                        int polititalId = Convert.ToInt32(grvProvince.DataKeys[Convert.ToInt32(hdfPoliticalDivisionID.Value)]["PoliticalDivisionID"]);

                        foreach (GridViewRow item in grvProvince.Rows)
                        {
                            if ((int)grvProvince.DataKeys[item.DataItemIndex].Value == polititalId)
                            {
                                grvProvince.SelectedIndex = item.DataItemIndex;
                                break;
                            }
                        }
                    }
                }

                if (!IsPostBack)
                {
                    btnSearch_ServerClick(sender, e);
                }

                if (Session[Gridresult] != null)
                {
                    PageHelper<NationalityEntity> pageHelper = (PageHelper<NationalityEntity>)Session[Gridresult];
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
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int hiddenPoliticalID = !string.IsNullOrWhiteSpace(hdfPoliticalDivisionID.Value) ?
                    Convert.ToInt32(hdfPoliticalDivisionID.Value) : (int)-1;

                var entity = new PoliticalDivisionEntity()
                {
                    PoliticalDivisionID = hiddenPoliticalID,
                    PoliticalDivisionName = txtProvince.Text.Trim(),
                    PoliticalDivisionCode = Convert.ToInt32(txtOrderList.Text),
                    CountryID = grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["Alpha3Code"].ToString(),
                    SearchEnabled = chkSearchEnabled.Checked,
                    LastModifiedUser = UserHelper.GetCurrentFullUserName
                };


                PoliticalDivisionEntity result = null;

                if (hiddenPoliticalID.Equals(-1))
                {
                    result = objPoliticalDivisionsBll.Add(entity);
                }
                else {
                    result = objPoliticalDivisionsBll.Edit(entity);
                }

                if (result.ErrorNumber == 0)
                {
                    RefreshProvinceTable(sender,e);

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , string.Format("ReturnFromBtnAcceptClickPostBack{0}"
                        , Guid.NewGuid())
                        , "setTimeout(function () {  ReturnFromBtnAcceptClickPostBack(); },200);"
                        , true);

                }else if (result.ErrorNumber != -1 && result.ErrorNumber != 2 && result.ErrorNumber != -3)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }else if (result.ErrorNumber == -1)
                {
                    txtDuplicatedProvinceId.Text = result.PoliticalDivisionID.ToString();
                    txtDuplicatedProvinceDescription.Text = result.PoliticalDivisionName;

                    var typetext = result.ErrorNumber == -1 ?
                        GetLocalResourceObject("PoliticalDivisionID.HeaderText").ToString() : GetLocalResourceObject("PoliticalDivisionName.HeaderText").ToString();

                    divDuplicatedDialogText.InnerHtml = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}"
                        , Guid.NewGuid())
                        , "setTimeout(function () {  ReturnFromBtnAcceptClickPostBackDuplicated(); },200); "
                        , true);
                }
                else {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("PoliticalOrderlistDuplicated").ToString());
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
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                PageHelper<NationalityEntity> pageHelper = SearchResults(1);
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
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddProvince_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedid = grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["Alpha3Code"].ToString();

                    PageHelper < NationalityEntity > pageHelper = (PageHelper<NationalityEntity>)Session[Gridresult];

                    var selectedName = pageHelper.ResultList.Find(x => x.Alpha3Code == selectedid );
                    txtNationality.Text = selectedName.NationalityName;
                    LoadProvince(selectedid);

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , string.Format("ReturnBtnAddProvince_ServerClick{0}"
                        , Guid.NewGuid())
                        , "setTimeout(function () {{ ReturnFromBtnAddProvinceClickPostBack(); }}, 200);"
                        , true);
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
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnLoadProvince_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int polititalId = Convert.ToInt32(grvProvince.DataKeys[Convert.ToInt32(hdfPoliticalDivisionID.Value)]["PoliticalDivisionID"]);
                    var province = objPoliticalDivisionsBll.ListByPoliticalDivision(polititalId);

                    txtProvince.Text = province.PoliticalDivisionName;
                    txtOrderList.Text = province.PoliticalDivisionCode.ToString();
                    chkSearchEnabled.Checked = province.SearchEnabled;
                    hdfPoliticalDivisionID.Value = province.PoliticalDivisionID.ToString();                    
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
        /// Load the LoadProvince
        /// </summary>
        private void LoadProvince(string Alpha3Code)
        {
            try
            {
                var listaProvincias = objPoliticalDivisionsBll.ListByCountryByParentPoliticalDivision(new PoliticalDivisionEntity() {CountryID=Alpha3Code });

                txtProvince.Text = "";
                txtOrderList.Text = "";
                chkSearchEnabled.Checked = false;
                hdfPoliticalDivisionID.Value = "-1";

            grvProvince.DataSource = listaProvincias;
                grvProvince.DataBind();
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
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvProvince_PreRender(object sender, EventArgs e)
        {
            if ((grvProvince.ShowHeader && grvProvince.Rows.Count > 0) || (grvProvince.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvProvince.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvProvince.ShowFooter && grvProvince.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvProvince.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void grvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[Gridresult] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<NationalityEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<NationalityEntity> SearchResults(int page)
        {
            var Filter = new NationalityEntity();

            Filter.NationalityName = string.IsNullOrEmpty(txtDescriptionFilter.Text) ? null : txtDescriptionFilter.Text;
            Filter.Alpha3Code = string.IsNullOrEmpty(txtAplhaCode.Text) ? null : txtAplhaCode.Text;

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<NationalityEntity> pageHelper = objPoliticalDivisionsBll.ListNationalities(
                Filter, sortExpression, sortDirection, page);

            Session[Gridresult] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[Gridresult] != null)
            {
                PageHelper<NationalityEntity> pageHelper = (PageHelper<NationalityEntity>)Session[Gridresult];

                grvList.DataSource = pageHelper.ResultList;
                grvList.DataBind();

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation"))
                    , pageHelper.ResultList.Count
                    , pageHelper.TotalResults
                    , pageHelper.TotalPages);
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

        private void RefreshProvinceTable(object sender, EventArgs e)
        {
            BtnAddProvince_ServerClick(sender,e);
        }

        #endregion
    }
}