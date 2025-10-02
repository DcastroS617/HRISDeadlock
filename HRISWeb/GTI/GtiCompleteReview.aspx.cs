using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.GTI
{
    public partial class GtiCompleteReview : System.Web.UI.Page
    {
        [Dependency]
        public IGtiPeriodBLL ObjGtiPeriodBll { get; set; }
        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionsBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll ObjGeneralConfigurationsBll { get; set; }

        [Dependency]
        public IGtiReportStepOneBLL ObjGtiReportStepOneBll { get; set; }

        readonly string sessionKeyGtiPeriodResults = "GtiPeriodResults";
        readonly string sessionKeyGtiPeriodDivisionResults = "GtiPeriodDivisionResults";

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
                    LoadPeriodStatus(cboPeriodStateFilter, false);
                    LoadQuarterYear(cboQuarterYearFilter);
                    LoadQuarterdPeriod(cboQuarterIDFilter);

                    //fire the event
                    BtnSearch_ServerClick(sender, e);
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
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            if (Session[Constants.cCulture] != null)
            {
                return new CultureInfo(Convert.ToString(Session[Constants.cCulture]));
            }
            return new CultureInfo(Constants.cCultureEsCR);
        }

        /// <summary>
        /// Method to load de status of the period
        /// </summary>
        private void LoadPeriodStatus(DropDownList dropdown, bool isNewRecord)
        {

            var listStatus = GetAllKeysAndNames<GtiPeriodStatus>();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in listStatus)
            {
                dropdown.Items.Add(new ListItem(item.Value, item.Key.ToString()));
            }

            if (isNewRecord)
            {
                // Selecciona el valor "New" y deshabilita el DropDownList
                dropdown.SelectedValue = ((int)GtiPeriodStatus.New).ToString();
                dropdown.Enabled = false;
            }
            else
            {
                // Habilita el DropDownList para edición
                dropdown.Enabled = true;
            }
        }

        /// <summary>
        /// Method to load de year of the period
        /// </summary>
        private void LoadYear(DropDownList dropdown)
        {
            var listYear = new List<int>();
            listYear.AddRange(Enumerable.Range((DateTime.Now.Year) - 25, 50).Reverse());
            dropdown.DataSource = listYear;
            dropdown.DataBind();
            dropdown.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
        }

        /// <summary>
        /// Method to load de year of the period
        /// </summary>
        private void LoadQuarterdPeriod(DropDownList dropdown)
        {
            var listQuarter = ObjGtiPeriodBll.ListQuarterPeriodActive();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in listQuarter)
            {
                dropdown.Items.Add(new ListItem(item.QuarterPeriodName, item.QuarterPeriodId.ToString()));
            }
        }

        /// <summary>
        /// Method to load de year of the period
        /// </summary>
        private void LoadQuarterYear(DropDownList dropdown)
        {
            var listQuarter = ObjGtiPeriodBll.ListQuarterYearActive();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in listQuarter)
            {
                dropdown.Items.Add(new ListItem(item.QuarterYear.ToString(), item.QuarterYearId.ToString()));
            }
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<PeriodCampaignEntity> SearchResults(int page)
        {
            var quarter = cboQuarterIDFilter.SelectedValue == "" ? "0" : cboQuarterIDFilter.SelectedValue;
            var year = cboQuarterYearFilter.SelectedValue == "" ? (DateTime.Now.Year).ToString() : cboQuarterYearFilter.SelectedValue;
            var period = cboPeriodStateFilter.SelectedValue == "" ? "0" : cboPeriodStateFilter.SelectedValue;

            PeriodCampaignEntity periodCampaignEntity = new PeriodCampaignEntity()
            {
                PeriodCampaignDescription = txtPeriodCampaignDescription.Text
                ,
                QuarterID = Convert.ToInt32(quarter)
                ,
                QuarterYear = Convert.ToInt32(year)
                ,
                PeriodState = Convert.ToInt32(period)
            };
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<PeriodCampaignEntity> pageHelper = ObjGtiPeriodBll.ListGtiPeriodByFilters(periodCampaignEntity, sortExpression, sortDirection, page);


            var listStatus = GetAllKeysAndNames<GtiPeriodStatus>();

            foreach (var entity in pageHelper.ResultList)
            {
                entity.PeriodStateDescrition = listStatus.Where(v => v.Key == entity.PeriodState).FirstOrDefault().Value;
            }

            Session[sessionKeyGtiPeriodResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyGtiPeriodResults] != null)
            {
                PageHelper<PeriodCampaignEntity> pageHelper = (PageHelper<PeriodCampaignEntity>)Session[sessionKeyGtiPeriodResults];

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

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayPeriodParameterResults()
        {
            if (Session[sessionKeyGtiPeriodResults] != null)
            {
                PageHelper<PeriodCampaignEntity> pageHelper = (PageHelper<PeriodCampaignEntity>)Session[sessionKeyGtiPeriodResults];

                grvPeriodParameters.DataSource = pageHelper.ResultList;
                grvPeriodParameters.DataBind();

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



        /// <summary>
        /// Load the survey answers for the current page
        /// </summary>
        /// <param name="item">The repeater item for the familiar</param>
        private void LoadGtiPeriodByDivision(RepeaterItem item)
        {
            CultureInfo currentCulture = GetCurrentCulture();

            Label lblCurrencyDescription = item.FindControl("lblCurrencyDescription") as Label;
            Label lblDivisionCode = item.FindControl("lblDivisionCode") as Label;

            var divisionCode = Convert.ToInt32(lblDivisionCode.Text);
            string currencyDescription = "";

            List<DivisionByActiveEmployeesEntity> listDivision = (List<DivisionByActiveEmployeesEntity>)Session[sessionKeyGtiPeriodDivisionResults];

            if (currentCulture.Name.Equals(Constants.cCultureEsCR))
            {
                currencyDescription = listDivision.Where(x => x.DivisionCode == divisionCode).FirstOrDefault().CurrencyNameSpanish;
            }
            else
            {
                currencyDescription = listDivision.Where(x => x.DivisionCode == divisionCode).FirstOrDefault().CurrencyNameEnglish;
            }

            lblCurrencyDescription.Text = currencyDescription;

        }

        /// <summary>
        /// Display entity
        /// </summary>
        /// <param name="gtiPeriodIdSelected">Gti Period Id Selected</param>
        private void DisplayEntity(object sender, int gtiPeriodIdSelected)
        {
            var result = ObjGtiPeriodBll.ListByKey(gtiPeriodIdSelected);

            txtName.Text = result.PeriodCampaignDescription;

        }

        /// <summary>
        ///Clear Modal Form
        /// </summary>
        private void ClearModalForm()
        {

        }

        private void RefreshTable()
        {
            SearchResults(PagerUtil.GetActivePage(blstPager));
            DisplayResults();
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        ///         
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //int hiddenPoliticalID = !string.IsNullOrWhiteSpace(hdfPoliticalDivisionID.Value) ?
                //    Convert.ToInt32(hdfPoliticalDivisionID.Value) : (int)-1;




                PeriodCampaignEntity result = null;

                //result = ObjGtiPeriodBll.AddOrUpdate(entity);

                if (result.ErrorNumber == 0)
                {
                    // RefreshProvinceTable(sender, e);

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , string.Format("ReturnPostBackAcceptClickSave{0}"
                        , Guid.NewGuid())
                        , "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);"
                        , true);
                    return;
                }
                else if (result.ErrorNumber != -1 && result.ErrorNumber != 2 && result.ErrorNumber != -3)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                if (result.ErrorNumber == -1)
                {
                    txtDuplicatedPeriodId.Text = result.PeriodCampaignId.ToString();
                    txtDuplicatedPeriodDescription.Text = result.PeriodCampaignDescription;

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
                else
                {
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
        /// Handles the btnAdd_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {

            ClearModalForm();

            SearchResults(1);

            CommonFunctions.ResetSortDirection(Page.ClientID, grvPeriodParameters.ClientID);

            DisplayPeriodParameterResults();
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
                    int selectedIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                    if (selectedIndex >= 0 && selectedIndex < grvList.Rows.Count)
                    {
                        GridViewRow selectedRow = grvList.Rows[selectedIndex];
                        string periodCampaignId = grvList.DataKeys[selectedIndex].Value.ToString();

                        // Cargar los detalles del periodo seleccionado para edición
                        PeriodCampaignEntity periodCampaign = ObjGtiPeriodBll.ListByKey(Convert.ToInt32(periodCampaignId));

                        if (periodCampaign != null)
                        {
                            txtName.Text = periodCampaign.PeriodCampaignDescription;



                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowEditModal", "$('#MaintenanceDialog').modal('show');", true);
                        }
                        else
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "El periodo seleccionado no se encontró.");
                        }
                    }
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "Por favor seleccione un periodo para editar.");
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
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
                int selectedIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    GridViewRow selectedRow = grvList.Rows[selectedIndex];
                    int periodCampaignId = Convert.ToInt32(grvList.DataKeys[selectedIndex].Value.ToString());

                    var entity = new PeriodCampaignEntity()
                    {
                        PeriodCampaignId = periodCampaignId,
                        Deleted = true,
                        LastModifiedUser = UserHelper.GetCurrentFullUserName

                    };

                    //string selectedTrainingCenterCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingCenterCode"]);

                    PeriodCampaignEntity result = null;

                    result = ObjGtiPeriodBll.AddOrUpdate(entity);

                    PageHelper<PeriodCampaignEntity> pageHelper = (PageHelper<PeriodCampaignEntity>)Session[sessionKeyGtiPeriodResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.PeriodCampaignId == selectedIndex));
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
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
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
        /// Handles the item data bound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void rptGtiDivision_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LoadGtiPeriodByDivision(e.Item);
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
        /// Handles the grvList data bound event
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (Session[Constants.cCulture] != null)
            //{
            //    if (e.Row.Cells.Count <= 1)
            //    {
            //        return;
            //    }

            //    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
            //    if (ci.Name.Equals("en-US"))
            //    {
            //        e.Row.Cells[1].Visible = true;
            //        e.Row.Cells[2].Visible = false;
            //    }

            //    else if (ci.Name.Equals("es-CR"))
            //    {
            //        e.Row.Cells[1].Visible = false;
            //        e.Row.Cells[2].Visible = true;
            //    }
            //}
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>      
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<PeriodCampaignEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults();
        }

        /// <summary>
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvParameterList_PreRender(object sender, EventArgs e)
        {
            if ((grvPeriodParameters.ShowHeader && grvPeriodParameters.Rows.Count > 0) || (grvPeriodParameters.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvPeriodParameters.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvPeriodParameters.ShowFooter && grvList.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvPeriodParameters.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void GrvParameterList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (Session[Constants.cCulture] != null)
            //{
            //    if (e.Row.Cells.Count <= 1)
            //    {
            //        return;
            //    }

            //    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
            //    if (ci.Name.Equals("en-US"))
            //    {
            //        e.Row.Cells[1].Visible = true;
            //        e.Row.Cells[2].Visible = false;
            //    }

            //    else if (ci.Name.Equals("es-CR"))
            //    {
            //        e.Row.Cells[1].Visible = false;
            //        e.Row.Cells[2].Visible = true;
            //    }
            //}
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>      
        protected void GrvParameterList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<PeriodCampaignEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayPeriodParameterResults();
        }
    }
}