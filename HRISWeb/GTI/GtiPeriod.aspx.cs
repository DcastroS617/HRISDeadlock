using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class GtiPeriod : System.Web.UI.Page
    {
        [Dependency]
        public IGtiPeriodBLL ObjGtiPeriodBll { get; set; }
        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionsBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll ObjGeneralConfigurationsBll { get; set; }

        [Dependency]
        public IGtiReportStepOneBLL ObjGtiReportStepOneBll { get; set; }

        [Dependency]
        public IGtiPeriodParameterDivisionCurrencyBLL ObjGtiPeriodParameterDivisionCurrencyBll { get; set; }

        [Dependency]
        public IGtiPeriodConfigurationBLL ObjGtiPeriodConfigurationBll { get; set; }

        [Dependency]
        public IGtiReportBLL ObjGtiReportBll { get; set; }

        readonly string sessionKeyGtiPeriodResults = "GtiPeriodResults";
        readonly string sessionKeyGtiPeriodDivisionResults = "GtiPeriodDivisionResults";
        readonly string sessionPeriodCampaignId = "GtiPeriodCampaignId";
        private readonly string sessionKeyDynamicControlsLoaded = "DynamicControlsLoaded";
        private readonly string sessionKeyPeriodConfiguration = "PeriodConfiguration";


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
        /// Method to load de status of the period
        /// </summary>
        private IDictionary<int, string> LoadPeriodStatus(DropDownList dropdown)
        {
            var listStatus = GetAllKeysAndNames<GtiPeriodStatus>();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in listStatus)
            {
                dropdown.Items.Add(new ListItem(item.Value, item.Key.ToString()));
            }


            // Habilita el DropDownList para edición
            dropdown.Enabled = true;


            return listStatus;
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

        private void LoadEmployees()
        {
            var listEmployees = ObjGtiReportStepOneBll.ListEmployees();
            //TableControl.GenerarTablaHtml(listEmployees, gtiDiv);
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



        protected void BtnStepOneDownload_ServerClick(object sender, EventArgs e)
        {
            var listEmployees = ObjGtiReportStepOneBll.ListEmployees();
            //ExportToExcel();
        }

        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        ///         
        protected void BtnManagers_ServerClick(object sender, EventArgs e)
        {

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

                int selectedIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                int periodCampaignId;

                if (selectedIndex != -1)
                {
                    GridViewRow selectedRow = grvList.Rows[selectedIndex];
                    periodCampaignId = (int)grvList.DataKeys[selectedIndex].Value == 0 ? 0 : (int)grvList.DataKeys[selectedIndex].Value;
                }
                else
                {
                    periodCampaignId = 0;
                }

                var entity = new PeriodCampaignEntity()
                {
                    PeriodCampaignId = periodCampaignId
                    ,
                    PeriodCampaignDescription = txtName.Text
                    ,
                    PeriodState = cboPeriodState.SelectedIndex
                    ,
                    QuarterID = cboQuarterId.SelectedIndex
                    ,
                    QuarterYear = Int32.Parse(cboQuarterYear.SelectedValue)
                    ,
                    InitialDate = DateTime.ParseExact(dtpStartDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                    ,
                    FinalDate = DateTime.ParseExact(dtpFinDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                    ,
                    PeriodMaxDateApprove = DateTime.ParseExact(ToDateEdit.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                    ,
                    LastModifiedUser = UserHelper.GetCurrentFullUserName
                };

                PeriodCampaignEntity result = null;

                result = ObjGtiPeriodBll.AddOrUpdate(entity);

                if (result.ErrorNumber == 0)
                {
                    // RefreshProvinceTable(sender, e);


                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , string.Format("ReturnPostBackAcceptClickSave{0}"
                        , Guid.NewGuid())
                        , "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);"
                        , true);
                    return;
                }
                else if (result.ErrorNumber != -1 && result.ErrorNumber != 2 && result.ErrorNumber != -3 && result.ErrorNumber != -2)
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
                else if (result.ErrorNumber == -2)
                {
                    txtDuplicatedPeriodId.Text = result.PeriodCampaignId.ToString();
                    txtDuplicatedPeriodDescription.Text = result.PeriodCampaignDescription;

                    var typetext = result.ErrorNumber == -2 ?
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
        /// Maneja el evento btnSaveDraft_ServerClick.
        /// Este método recolecta todos los datos de los controles en cada tab,
        /// construye el objeto PeriodConfigurationEntity y llama a ConfigurationAddorUpdate para guardar la configuración.
        /// </summary>
        /// <param name="sender">Objeto que invoca el evento.</param>
        /// <param name="e">Datos del evento.</param>
        protected void btnSaveDraft_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var periodCampaignId = Session[sessionPeriodCampaignId];
                PeriodConfigurationEntity configuration = Session[sessionKeyPeriodConfiguration] as PeriodConfigurationEntity;
                if (configuration == null)
                {

                    configuration = new PeriodConfigurationEntity
                    {
                        PeriodCampaignId = (int)periodCampaignId,
                        ConfigurationState = (int)GtiConfigurationState.Draft,
                        ConfigurationReports = new List<PeriodConfigurationReportEntity>()
                    };
                }


                // Recolectar datos de cada tab
                CollectRepeaterData(configuration, rptGtiParameters, ReportType.GTI, isGtiTab: true);
                CollectRepeaterData(configuration, rptHmtParameters, ReportType.HMT, isGtiTab: false);
                CollectRepeaterData(configuration, rptHleParameters, ReportType.HLE, isGtiTab: false);
                CollectRepeaterData(configuration, rptDuiParameters, ReportType.DUI, isGtiTab: false);
                CollectRepeaterData(configuration, rptQkeParameters, ReportType.QKE, isGtiTab: false);
                CollectRepeaterData(configuration, rptSctParameters, ReportType.SCT, isGtiTab: false);


                // Llamar al método ConfigurationAddorUpdate para guardar la configuración
                PeriodConfigurationEntity result = ObjGtiPeriodConfigurationBll.ConfigurationAddorUpdate(configuration);

                if (result != null && result.ErrorNumber == 0)
                {
                    // Mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript((Control)sender
                        , sender.GetType()
                        , string.Format("ReturnPostBackAcceptClickSave{0}"
                        , Guid.NewGuid())
                        , "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);"
                        , true);
                    return;
                }
                else
                {
                    // Mostrar mensaje de error
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "Error al guardar borrador");

                }
            }
            catch (Exception ex)
            {

                //"Ocurrió un error al guardar el borrador.")

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
                    //GridViewRow selectedRow = grvList.Rows[selectedIndex];
                    //int periodCampaignId = Convert.ToInt32(grvList.DataKeys[selectedIndex].Value.ToString());

                    //var entity = new PeriodCampaignEntity()
                    //{
                    //    PeriodCampaignId = periodCampaignId,
                    //    Deleted = true,
                    //    LastModifiedUser = UserHelper.GetCurrentFullUserName

                    //};                    

                    //PeriodCampaignEntity result = null;

                    //result = ObjGtiPeriodBll.AddOrUpdate(entity);

                    //PageHelper<PeriodCampaignEntity> pageHelper = (PageHelper<PeriodCampaignEntity>)Session[sessionKeyGtiPeriodResults];

                    //pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.PeriodCampaignId == selectedIndex));
                    //pageHelper.TotalResults--;

                    //if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    //{
                    //    SearchResults(pageHelper.TotalPages - 1);
                    //    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                    //}

                    //pageHelper.UpdateTotalPages();
                    //RefreshTable();

                    //hdfSelectedRowIndex.Value = "-1";
                    //ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);

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
        /// Handles the btnDelete click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnReview_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    //GridViewRow selectedRow = grvList.Rows[selectedIndex];
                    //int periodCampaignId = Convert.ToInt32(grvList.DataKeys[selectedIndex].Value.ToString());

                    //var entity = new PeriodCampaignEntity()
                    //{
                    //    PeriodCampaignId = periodCampaignId,
                    //    Deleted = true,
                    //    LastModifiedUser = UserHelper.GetCurrentFullUserName

                    //};

                    //PeriodCampaignEntity result = null;

                    //result = ObjGtiPeriodBll.AddOrUpdate(entity);

                    //PageHelper<PeriodCampaignEntity> pageHelper = (PageHelper<PeriodCampaignEntity>)Session[sessionKeyGtiPeriodResults];

                    //pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.PeriodCampaignId == selectedIndex));
                    //pageHelper.TotalResults--;

                    //if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    //{
                    //    SearchResults(pageHelper.TotalPages - 1);
                    //    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                    //}

                    //pageHelper.UpdateTotalPages();
                    //RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnReviewClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnReviewClickPostBack(); },200);", true);

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
        /// Handles the btnDelete click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSummary_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    //GridViewRow selectedRow = grvList.Rows[selectedIndex];
                    //int periodCampaignId = Convert.ToInt32(grvList.DataKeys[selectedIndex].Value.ToString());

                    //var entity = new PeriodCampaignEntity()
                    //{
                    //    PeriodCampaignId = periodCampaignId,
                    //    Deleted = true,
                    //    LastModifiedUser = UserHelper.GetCurrentFullUserName

                    //};

                    //PeriodCampaignEntity result = null;

                    //result = ObjGtiPeriodBll.AddOrUpdate(entity);

                    //PageHelper<PeriodCampaignEntity> pageHelper = (PageHelper<PeriodCampaignEntity>)Session[sessionKeyGtiPeriodResults];

                    //pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.PeriodCampaignId == selectedIndex));
                    //pageHelper.TotalResults--;

                    //if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    //{
                    //    SearchResults(pageHelper.TotalPages - 1);
                    //    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                    //}

                    //pageHelper.UpdateTotalPages();
                    //RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnReviewClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnReviewClickPostBack(); },200);", true);

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
        /// Handles the BtnGtiReports click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        //protected void BtnGtiReports_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (hdfSelectedRowIndex.Value != "-1")
        //        {
        //            int selectedIndex = Convert.ToInt32(hdfSelectedRowIndex.Value);

        //            if (selectedIndex >= 0 && selectedIndex < grvList.Rows.Count)
        //            {
        //                GridViewRow selectedRow = grvList.Rows[selectedIndex];
        //                string periodCampaignId = grvList.DataKeys[selectedIndex].Value.ToString();

        //                // Cargar los detalles del periodo seleccionado para edición
        //                PeriodCampaignEntity periodCampaign = ObjGtiPeriodBll.ListByKey(Convert.ToInt32(periodCampaignId));

        //                if (periodCampaign != null)
        //                {
        //                    txtPeriodNameReport.Text = periodCampaign.PeriodCampaignDescription;
        //                    txtPeriodStateReport.Text = periodCampaign.PeriodState.ToString();
        //                    txtQuarterYearReport.Text = periodCampaign.QuarterYear.ToString();
        //                    txtQuarterIdReport.Text = periodCampaign.QuarterID.ToString();
        //                    LoadEmployees();
        //                    // Mostrar el modal ReportDialog
        //                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnReportsClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnReportsClickPostBack(); }}, 200);", true);
        //                }
        //                else
        //                {
        //                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "El periodo seleccionado no se encontró.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
        //    }
        //}

        /// <summary>
        /// Handles the BtnGtiReports click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnGtiConfig_ServerClick(object sender, EventArgs e)
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
                        Session[sessionPeriodCampaignId] = periodCampaignId;

                        // Cargar los detalles del período seleccionado
                        PeriodCampaignEntity periodCampaign = ObjGtiPeriodBll.ListByKey(Convert.ToInt32(periodCampaignId));

                        if (periodCampaign != null)
                        {
                            // Validar que el período esté en estado "Nuevo"
                            if (periodCampaign.PeriodState == (int)GtiPeriodStatus.New)
                            {
                                // Verificar si existe una configuración en borrador para este período
                                PeriodConfigurationEntity configuration = ObjGtiPeriodConfigurationBll.GetConfigurationByPeriodAndState(periodCampaign.PeriodCampaignId, (int)GtiConfigurationState.Draft);

                                if (configuration == null)
                                {
                                    configuration = new PeriodConfigurationEntity
                                    {
                                        PeriodCampaignId = periodCampaign.PeriodCampaignId,
                                        ConfigurationState = (int)GtiConfigurationState.Draft,
                                        ConfigurationReports = new List<PeriodConfigurationReportEntity>()
                                    };
                                }

                                Session[sessionKeyPeriodConfiguration] = configuration;


                                // Cargar los detalles del período en la interfaz
                                var listStatus = GetAllKeysAndNames<GtiPeriodStatus>();
                                var periodStateDescription = listStatus
                                    .Where(v => v.Key == periodCampaign.PeriodState)
                                    .Select(v => v.Value)
                                    .FirstOrDefault();

                                txtPeriodNameConfig.Text = periodCampaign.PeriodCampaignDescription;
                                txtPeriodStateConfig.Text = periodStateDescription ?? Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
                                txtQuarterYearConfig.Text = periodCampaign.QuarterYear.ToString();
                                txtQuarterIdConfig.Text = periodCampaign.QuarterPeriodName.ToString();

                                // Cargar los parámetros, pasando la configuración si existe
                                LoadTabParameters(configuration);

                                Session[sessionKeyDynamicControlsLoaded] = true;

                                // Almacenar la configuración actual para recrear los controles
                                Session[sessionKeyPeriodConfiguration] = configuration;

                                // Mostrar el modal ReportDialog
                                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnConfigClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () { ReturnFromBtnConfigClickPostBack(); }, 200);", true);
                            }
                            else
                            {
                                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "El período seleccionado no está en estado Nuevo y no puede ser configurado.");
                            }
                        }
                        else
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "El período seleccionado no se encontró.");
                        }
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                    }
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }
            catch (Exception ex)
            {
                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
            }
        }

        /// <summary>
        /// Handles the btnReview click event to process and save the period configuration.
        /// This method retrieves or creates a period configuration, collects data from UI controls,
        /// validates the configuration, updates its state, and saves it using business logic.
        /// </summary>
        /// <param name="sender">The button control that triggered the event.</param>
        /// <param name="e">Contains the event data.</param>
        protected void btnRunReview_Click(object sender, EventArgs e)
        {
            try
            {
                var periodCampaignId = Session[sessionPeriodCampaignId];

                // Obtener o crear la configuración del período
                PeriodConfigurationEntity configuration = Session[sessionKeyPeriodConfiguration] as PeriodConfigurationEntity;
                if (configuration == null)
                {
                    configuration = new PeriodConfigurationEntity
                    {
                        PeriodCampaignId = (int)periodCampaignId,
                        ConfigurationState = (int)GtiConfigurationState.Draft,
                        ConfigurationReports = new List<PeriodConfigurationReportEntity>()
                    };
                }

                // Recolectar datos de cada tab
                CollectRepeaterData(configuration, rptGtiParameters, ReportType.GTI, isGtiTab: true);
                CollectRepeaterData(configuration, rptHmtParameters, ReportType.HMT, isGtiTab: false);
                CollectRepeaterData(configuration, rptHleParameters, ReportType.HLE, isGtiTab: false);
                CollectRepeaterData(configuration, rptDuiParameters, ReportType.DUI, isGtiTab: false);
                CollectRepeaterData(configuration, rptQkeParameters, ReportType.QKE, isGtiTab: false);
                CollectRepeaterData(configuration, rptSctParameters, ReportType.SCT, isGtiTab: false);

                // Validar los datos recolectados
                if (!ValidateConfiguration(configuration))
                {
                    // Si la validación falla, detener el proceso
                    return;
                }

                // Establecer el estado de la configuración
                configuration.ConfigurationState = (int)1;

                // Llamar al método ConfigurationAddorUpdate para guardar o actualizar la configuración
                PeriodConfigurationEntity result = ObjGtiPeriodConfigurationBll.ConfigurationAddorUpdate(configuration);

                if (result != null && result.ErrorNumber == 0)
                {
                    // Actualizar el estado del período a "En Progreso"
                    //UpdatePeriodStatus((int)periodCampaignId, GtiConfigurationReportState.InProgress);

                    // Mostrar mensaje de éxito
                    //MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Exito, "La configuración ha sido guardada y el período actualizado a 'En Progreso'.");
                }
                else
                {
                    // Mostrar mensaje de error
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "Error al guardar la configuración.");
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
                            var listStatus = GetAllKeysAndNames<GtiPeriodStatus>();
                            var periodStateDescription = listStatus
                                .Where(v => v.Key == periodCampaign.PeriodState)
                                .Select(v => v.Value)
                                .FirstOrDefault();

                            var status = LoadPeriodStatus(cboPeriodState);
                            var selectedItem = listStatus.FirstOrDefault(x => x.Value.Equals(periodStateDescription, StringComparison.InvariantCultureIgnoreCase));
                            LoadYear(cboQuarterYear);
                            LoadQuarterdPeriod(cboQuarterId);

                            txtName.Text = periodCampaign.PeriodCampaignDescription;
                            cboPeriodState.SelectedValue = selectedItem.Key.ToString();
                            if (selectedItem.Key != 1 && selectedItem.Key != 2)
                                cboPeriodState.Enabled = false;

                            cboQuarterYear.SelectedValue = periodCampaign.QuarterYear.ToString();
                            cboQuarterId.SelectedValue = periodCampaign.QuarterID.ToString();
                            CalculateDatesPeriod(Convert.ToInt32(cboQuarterYear.SelectedValue), Convert.ToInt32(cboQuarterId.SelectedValue));
                            ToDateEdit.Value = periodCampaign.PeriodMaxDateApprove.ToString("MM/dd/yyyy");

                            cboQuarterId.Enabled = false;
                            cboQuarterYear.Enabled = false;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnRequestBtnEditOpen{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(''); }, 200);  ", true);
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
        /// Validates the configuration for each report by ensuring that required fields are completed.
        /// Checks if the process responsible is assigned for each parameter, and if the report is GTI,
        /// verifies that the exchange rate is provided and positive.
        /// </summary>
        /// <param name="configuration">The PeriodConfigurationEntity object containing report configurations to validate.</param>
        /// <returns>True if the configuration is valid; otherwise, false.</returns>
        private bool ValidateConfiguration(PeriodConfigurationEntity configuration)
        {
            bool isValid = true;

            foreach (var report in configuration.ConfigurationReports)
            {
                foreach (var parameter in report.ConfigurationParameters)
                {
                    // Validar que el aprobador esté asignado
                    if (string.IsNullOrEmpty(parameter.ProcessResponsible))
                    {
                        isValid = false;
                    }

                    // Si es GTI, validar que el tipo de cambio esté asignado
                    if (report.ReportId == (int)ReportType.GTI)
                    {
                        if (!parameter.ExchangeRate.HasValue || parameter.ExchangeRate <= 0)
                        {
                            isValid = false;
                        }
                    }
                }
            }

            if (!isValid)
            {
                // Mostrar mensaje de error genérico
                MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, "Faltan campos obligatorios por completar. Por favor, revise los campos resaltados.");
            }

            return isValid;
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
        /// Handles the btnAdd_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

            cboGtiPeriodExisted.Items.Clear();
            cboGtiPeriodExisted.Items.Add(new ListItem() { Value = "-1", Text = GetLocalResourceObject("msjSelectPeriod").ToString() });
            cboGtiPeriodExisted.Items.AddRange(ObjGtiPeriodBll.MasterParameterList());
            cboQuarterId.Enabled = true;
            cboQuarterYear.Enabled = true;
            LoadPeriodStatus(cboPeriodState, true);
            LoadYear(cboQuarterYear);
            LoadQuarterdPeriod(cboQuarterId);
            ClearModalForm();



        }

        /// <summary>
        /// Handles the cboQuarterId_SelectedIndexChanged click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboQuarterId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboQuarterId.SelectedValue != "" && cboQuarterId.SelectedValue != "-1" &&
                cboQuarterYear.SelectedValue != "" && cboQuarterYear.SelectedValue != "-1"
                )
            {
                CalculateDatesPeriod(Convert.ToInt32(cboQuarterYear.SelectedValue), Convert.ToInt32(cboQuarterId.SelectedValue));

            }
            else
            {
                ClearModalForm();
            }
        }

        /// <summary>
        /// Handles the cboQuarterYear_SelectedIndexChanged click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboQuarterYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cboQuarterYear.SelectedValue) && !cboQuarterYear.SelectedValue.Equals("-1"))
                {
                    int quarterYearSelected = Int32.Parse(cboQuarterYear.SelectedValue);

                    DisplayEntity(sender, quarterYearSelected);

                    hdfQuarterYearSelectedExisted.Value = quarterYearSelected.ToString();
                }
                else
                {
                    txtName.Text = string.Empty;
                    cboPeriodState.SelectedIndex = -1;
                    cboQuarterYear.SelectedIndex = -1;
                    dtpStartDate.Text = string.Empty;
                    dtpFinDate.Text = string.Empty;

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
        private void CalculateDatesPeriod(int year, int quarterPeriod)
        {
            var listQuarter = ObjGtiPeriodBll.ListQuarterPeriodActive();

            QuarterPeriodEntity entityFiltered = listQuarter.Where(t => t.QuarterPeriodId == quarterPeriod).FirstOrDefault();

            var tomela = entityFiltered.QuarterPeriodStarDateFormatted;
            var dayini = entityFiltered.QuarterPeriodStarDate.Day;
            var mesini = entityFiltered.QuarterPeriodStarDate.Month;
            var dateInicio = new DateTime(year, mesini, dayini);
            var dateFin = dateInicio.AddMonths(3).AddDays(-1);

            dtpStartDate.Text = dateInicio.ToString("MM/dd/yyyy");
            dtpFinDate.Text = dateFin.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// Recolecta los datos de un tab específico y los agrega al PeriodConfigurationEntity.
        /// </summary>
        /// <param name="periodConfig">Objeto PeriodConfigurationEntity para almacenar los datos.</param>
        /// <param name="container">Contenedor de los controles del tab (e.g., gtiParametersContainer).</param>
        /// <param name="reportName">Nombre del reporte correspondiente al tab (e.g., "GTI").</param>
        /// <param name="isGtiTab">Indica si el tab es GTI, que tiene un campo extra de tipo de cambio.</param>
        private void CollectTabData(PeriodConfigurationEntity periodConfig, Control container, string reportName, bool isGtiTab = false)
        {
            // Obtener el ReportId basado en el nombre del reporte
            ReportType reportType = (ReportType)Enum.Parse(typeof(ReportType), reportName);
            int reportId = (int)reportType;

            // Crear una entidad de reporte para agregarla a la configuración
            PeriodConfigurationReportEntity reportEntity = new PeriodConfigurationReportEntity
            {
                ReportId = reportId,
                IsEnabled = false, // Se establecerá en true si al menos un parámetro está habilitado
                ConfigurationParameters = new List<PeriodConfigurationParameterEntity>()
            };

            // Iterar sobre los controles en el contenedor
            foreach (Control control in container.Controls)
            {
                if (control is System.Web.UI.WebControls.Table tbl)
                {
                    // Iterar sobre las filas de la tabla
                    foreach (System.Web.UI.WebControls.TableRow row in tbl.Rows)
                    {
                        // Saltar la fila de encabezado
                        if (row is System.Web.UI.WebControls.TableHeaderRow)
                            continue;

                        // Variables para almacenar los valores de los controles
                        CheckBox chkEnableConfig = null;
                        CheckBox chkPreApproval = null;
                        TextBox txtResponsible = null;
                        TextBox txtExchangeRate = null; // Solo para el tab GTI

                        // Iterar sobre las celdas de la fila
                        foreach (System.Web.UI.WebControls.TableCell cell in row.Cells)
                        {
                            foreach (Control cellControl in cell.Controls)
                            {
                                // Identificar los controles por su ID o tipo
                                if (cellControl is CheckBox chk)
                                {
                                    if (chk.ID.StartsWith("chkEnable"))
                                        chkEnableConfig = chk;
                                    else if (chk.ID.StartsWith("chkPreApproval"))
                                        chkPreApproval = chk;
                                }
                                else if (cellControl is TextBox txt)
                                {
                                    if (txt.ID.StartsWith("txtResponsible"))
                                        txtResponsible = txt;
                                    else if (isGtiTab && txt.ID.StartsWith("txtExchangeRate"))
                                        txtExchangeRate = txt;
                                }
                            }
                        }

                        // Si el parámetro está habilitado, crear una entidad de parámetro
                        if (chkEnableConfig != null && chkEnableConfig.Checked)
                        {
                            reportEntity.IsEnabled = true; // Marcar el reporte como habilitado

                            // Extraer el PeriodParameterDivisionCurrencyId del ID del control
                            int periodParameterDivisionCurrencyId = ExtractIdFromControlID(chkEnableConfig.ID);

                            // Crear la entidad de parámetro
                            PeriodConfigurationParameterEntity parameterEntity = new PeriodConfigurationParameterEntity
                            {
                                PeriodParameterDivisionCurrencyId = periodParameterDivisionCurrencyId,
                                RequiresPreApproval = chkPreApproval?.Checked ?? false,
                                ProcessResponsible = txtResponsible?.Text,
                                ExchangeRate = isGtiTab && txtExchangeRate != null && decimal.TryParse(txtExchangeRate.Text, out decimal exchangeRate)
                                    ? (decimal?)exchangeRate
                                    : null
                            };

                            // Agregar el parámetro a la lista de parámetros del reporte
                            reportEntity.ConfigurationParameters.Add(parameterEntity);
                        }
                    }
                }
            }

            // Si el reporte está habilitado, agregarlo a la lista de reportes de la configuración
            if (reportEntity.IsEnabled)
            {
                periodConfig.ConfigurationReports.Add(reportEntity);
            }
        }

        /// <summary>
        /// Extrae el ID numérico de un control basado en su ID (e.g., "chkEnable123" -> 123).
        /// </summary>
        /// <param name="controlID">ID del control.</param>
        /// <returns>ID numérico extraído.</returns>
        private int ExtractIdFromControlID(string controlID)
        {
            // Usar expresiones regulares para extraer el número del ID del control
            string idNumber = System.Text.RegularExpressions.Regex.Match(controlID, @"\d+").Value;
            return int.TryParse(idNumber, out int id) ? id : 0;
        }

        protected void cboGtiPeriodExisted_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cboGtiPeriodExisted.SelectedValue) && !cboGtiPeriodExisted.SelectedValue.Equals("-1"))
                {
                    int masterPeriodIdSelected = int.Parse(cboGtiPeriodExisted.SelectedValue);

                    DisplayEntity(sender, masterPeriodIdSelected);

                    hdfGtiPeriodIdExisted.Value = masterPeriodIdSelected.ToString();
                }
                else
                {

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
        /// Display entity
        /// </summary>
        /// <param name="gtiPeriodIdSelected">Gti Period Id Selected</param>
        private void DisplayEntity(object sender, int gtiPeriodIdSelected)
        {
            var result = ObjGtiPeriodBll.ListByKey(gtiPeriodIdSelected);

            txtName.Text = result.PeriodCampaignDescription;
            cboPeriodState.Items.FindByValue(result.PeriodState.ToString()).Selected = true;
            cboQuarterYear.Items.FindByValue(result.PeriodState.ToString()).Selected = true;
            dtpStartDate.Text = result.InitialDate.ToString("MM/dd/yyyy");
            dtpFinDate.Text = result.FinalDate.ToString("MM/dd/yyyy");


        }

        /// <summary>
        ///Clear Modal Form
        /// </summary>
        private void ClearModalForm()
        {
            hdfSelectedRowIndex.Value = "-1";
            dtpStartDate.Text = "";
            dtpFinDate.Text = "";
        }

        private void RefreshTable()
        {
            SearchResults(PagerUtil.GetActivePage(blstPager));
            DisplayResults();
        }

        //private void LoadTabParameters(PeriodConfigurationEntity configuration)
        //{
        //    // Obtener los parámetros comunes
        //    var commonParams = ObjGtiPeriodParameterDivisionCurrencyBll.ListNameGeographicDivisionsByDivisions();

        //    // Generar parámetros para cada tab, pasando la configuración correspondiente
        //    AddParameterCheckboxes(commonParams, gtiParametersContainer, configuration, "GTI", isGtiTab: true);
        //    AddParameterCheckboxes(commonParams, hmtParametersContainer, configuration, "HMT");
        //    AddParameterCheckboxes(commonParams, hleParametersContainer, configuration, "HLE");
        //    AddParameterCheckboxes(commonParams, duiParametersContainer, configuration, "DUI");
        //    AddParameterCheckboxes(commonParams, qkeParametersContainer, configuration, "QKE");
        //    AddParameterCheckboxes(commonParams, sctParametersContainer, configuration, "SCT");
        //}

        /// <summary>
        /// Loads parameter data for each report type tab in the configuration, binding each Repeater control
        /// to a data source containing the respective parameters.
        /// </summary>
        /// <param name="configuration">The PeriodConfigurationEntity object containing report configurations to load parameters from.</param>
        private void LoadTabParameters(PeriodConfigurationEntity configuration)
        {
            // GTI
            var gtiParameters = GetParametersForReport(configuration, ReportType.GTI, isGtiTab: true);
            rptGtiParameters.DataSource = gtiParameters;
            rptGtiParameters.DataBind();

            // HMT
            var hmtParameters = GetParametersForReport(configuration, ReportType.HMT, isGtiTab: false);
            rptHmtParameters.DataSource = hmtParameters;
            rptHmtParameters.DataBind();

            // HLE
            var hleParameters = GetParametersForReport(configuration, ReportType.HLE, isGtiTab: false);
            rptHleParameters.DataSource = hleParameters;
            rptHleParameters.DataBind();

            // DUI
            var duiParameters = GetParametersForReport(configuration, ReportType.DUI, isGtiTab: false);
            rptDuiParameters.DataSource = duiParameters;
            rptDuiParameters.DataBind();

            // QKE
            var qkeParameters = GetParametersForReport(configuration, ReportType.QKE, isGtiTab: false);
            rptQkeParameters.DataSource = qkeParameters;
            rptQkeParameters.DataBind();

            // SCT
            var sctParameters = GetParametersForReport(configuration, ReportType.SCT, isGtiTab: false);
            rptSctParameters.DataSource = sctParameters;
            rptSctParameters.DataBind();
        }


        /// <summary>
        /// Dynamically generates a table with parameter checkboxes, pre-approval options, and input fields for configuring process responsibilities.
        /// This method uses the provided parameters and configuration settings to populate the table, with optional exchange rate fields if GTI tab is active.
        /// </summary>
        /// <param name="parameters">List of PeriodParameterDivisionCurrencyEntity objects representing the available parameters.</param>
        /// <param name="container">The container control where the generated table will be added.</param>
        /// <param name="configuration">The PeriodConfigurationEntity object containing report configurations, if available.</param>
        /// <param name="reportName">The name of the report to retrieve the specific configuration for.</param>
        /// <param name="isGtiTab">Specifies whether the GTI tab is active, which includes exchange rate fields in the table.</param>
        private void AddParameterCheckboxes(List<PeriodParameterDivisionCurrencyEntity> parameters, Control container, PeriodConfigurationEntity configuration, string reportName, bool isGtiTab = false)
        {
            // Obtener el ReportId basado en el nombre del reporte
            ReportType reportType = (ReportType)Enum.Parse(typeof(ReportType), reportName);
            int reportId = (int)reportType;

            // Obtener la configuración del reporte si existe
            PeriodConfigurationReportEntity configurationReport = null;
            List<PeriodConfigurationParameterEntity> configurationParameters = null;

            if (configuration != null)
            {
                configurationReport = configuration.ConfigurationReports.FirstOrDefault(r => r.ReportId == reportId);
                if (configurationReport != null)
                {
                    configurationParameters = configurationReport.ConfigurationParameters;
                }
            }

            // Crear la tabla
            System.Web.UI.WebControls.Table tbl = new System.Web.UI.WebControls.Table
            {
                CssClass = "table table-bordered"
            };

            // Crear la fila de encabezados
            TableHeaderRow headerRow = new TableHeaderRow();
            headerRow.Cells.Add(new TableHeaderCell { Text = "Habilitar configuración" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Pre-aprobación" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Responsable de proceso" });

            if (isGtiTab)
            {
                headerRow.Cells.Add(new TableHeaderCell { Text = "Tipo de cambio" });
            }

            // Agregar la fila de encabezados a la tabla
            tbl.Rows.Add(headerRow);

            // Iterar sobre los parámetros para crear las filas de la tabla
            foreach (var parameter in parameters)
            {
                // Obtener el ConfigurationParameterEntity correspondiente si existe
                PeriodConfigurationParameterEntity configParam = null;
                if (configurationParameters != null)
                {
                    configParam = configurationParameters.FirstOrDefault(p => p.PeriodParameterDivisionCurrencyId == parameter.PeriodParameterDivisionCurrencyId);
                }

                // Crear una nueva fila
                System.Web.UI.WebControls.TableRow row = new System.Web.UI.WebControls.TableRow();

                // Columna "Habilitar configuración" con un checkbox y texto del parámetro
                System.Web.UI.WebControls.TableCell enableConfigCell = new System.Web.UI.WebControls.TableCell();
                CheckBox chkEnableConfig = new CheckBox
                {
                    ID = "chkEnable" + parameter.PeriodParameterDivisionCurrencyId,
                    CssClass = "form-check-input",
                    Checked = configParam != null // Si hay configuración, marcar el checkbox
                };
                enableConfigCell.Controls.Add(chkEnableConfig);
                // Agregar el texto del parámetro junto al checkbox
                enableConfigCell.Controls.Add(new Literal { Text = " " + parameter.PeriodParameterDivisionCurrencyName });
                row.Cells.Add(enableConfigCell);

                // Columna "Pre-aprobación" con checkbox
                System.Web.UI.WebControls.TableCell preApprovalCell = new System.Web.UI.WebControls.TableCell();
                CheckBox chkPreApproval = new CheckBox
                {
                    ID = "chkPreApproval" + parameter.PeriodParameterDivisionCurrencyId,
                    CssClass = "form-check-input",
                    Checked = configParam != null ? configParam.RequiresPreApproval : false
                };
                preApprovalCell.Controls.Add(chkPreApproval);
                preApprovalCell.Controls.Add(new Literal { Text = " Requiere preaprobación" });
                row.Cells.Add(preApprovalCell);

                // Columna "Responsable de proceso" con TextBox
                System.Web.UI.WebControls.TableCell responsibleCell = new System.Web.UI.WebControls.TableCell();
                TextBox txtResponsible = new TextBox
                {
                    ID = "txtResponsible" + parameter.PeriodParameterDivisionCurrencyId,
                    CssClass = "form-control",
                    Text = configParam != null ? configParam.ProcessResponsible : string.Empty
                };
                responsibleCell.Controls.Add(txtResponsible);
                row.Cells.Add(responsibleCell);

                if (isGtiTab)
                {
                    // Columna "Tipo de cambio" con TextBox
                    System.Web.UI.WebControls.TableCell exchangeRateCell = new System.Web.UI.WebControls.TableCell();
                    TextBox txtExchangeRate = new TextBox
                    {
                        ID = "txtExchangeRate" + parameter.PeriodParameterDivisionCurrencyId,
                        CssClass = "form-control",
                        Text = configParam != null && configParam.ExchangeRate.HasValue ? configParam.ExchangeRate.Value.ToString() : string.Empty
                    };
                    txtExchangeRate.Attributes.Add("inputmode", "numeric");
                    txtExchangeRate.Attributes.Add("pattern", "[0-9]*");
                    exchangeRateCell.Controls.Add(txtExchangeRate);
                    row.Cells.Add(exchangeRateCell);
                }

                // Agregar la fila a la tabla
                tbl.Rows.Add(row);
            }

            // Agregar la tabla al contenedor proporcionado
            container.Controls.Add(tbl);
        }

        /// <summary>
        /// Retrieves a list of parameters for a specific report type based on the configuration settings.
        /// This method constructs view entities for each parameter by merging common parameters with report-specific configurations.
        /// </summary>
        /// <param name="configuration">The PeriodConfigurationEntity object containing report configurations.</param>
        /// <param name="reportType">The report type to retrieve parameters for, represented by the ReportType enum.</param>
        /// <param name="isGtiTab">Indicates if the GTI tab is active, in which case additional exchange rate data is included.</param>
        /// <returns>A list of PeriodParameterViewEntity objects representing the parameters and their settings for the specified report.</returns>
        private List<PeriodParameterViewEntity> GetParametersForReport(PeriodConfigurationEntity configuration, ReportType reportType, bool isGtiTab)
        {
            int reportId = (int)reportType;

            // Obtener los parámetros comunes
            var commonParams = ObjGtiPeriodParameterDivisionCurrencyBll.ListNameGeographicDivisionsByDivisions();

            // Obtener la configuración del reporte si existe
            var configurationReport = configuration.ConfigurationReports.FirstOrDefault(r => r.ReportId == reportId);

            var parametersViewEntity = commonParams.Select(p => new PeriodParameterViewEntity
            {
                PeriodParameterDivisionCurrencyId = p.PeriodParameterDivisionCurrencyId,
                ParameterName = p.PeriodParameterDivisionCurrencyName,
                IsEnabled = configurationReport?.ConfigurationParameters.Any(cp => cp.PeriodParameterDivisionCurrencyId == p.PeriodParameterDivisionCurrencyId) ?? false,
                RequiresPreApproval = configurationReport?.ConfigurationParameters.FirstOrDefault(cp => cp.PeriodParameterDivisionCurrencyId == p.PeriodParameterDivisionCurrencyId)?.RequiresPreApproval ?? false,
                ProcessResponsible = configurationReport?.ConfigurationParameters.FirstOrDefault(cp => cp.PeriodParameterDivisionCurrencyId == p.PeriodParameterDivisionCurrencyId)?.ProcessResponsible ?? string.Empty,
                ExchangeRate = isGtiTab ? configurationReport?.ConfigurationParameters.FirstOrDefault(cp => cp.PeriodParameterDivisionCurrencyId == p.PeriodParameterDivisionCurrencyId)?.ExchangeRate : null
            }).ToList();

            return parametersViewEntity;
        }

        /// <summary>
        /// Collects data from a Repeater control and populates the configuration parameters for a specific report type.
        /// This method processes each item in the Repeater, checks if the configuration is enabled for each parameter,
        /// and adds the parameters with their properties to the specified report configuration.
        /// </summary>
        /// <param name="configuration">The PeriodConfigurationEntity object where report configuration data is stored.</param>
        /// <param name="repeater">The Repeater control containing parameter configuration data.</param>
        /// <param name="reportType">The report type to configure, represented by the ReportType enum.</param>
        /// <param name="isGtiTab">Indicates whether the current tab is the GTI tab, which includes additional fields.</param>

        private void CollectRepeaterData(PeriodConfigurationEntity configuration, Repeater repeater, ReportType reportType, bool isGtiTab)
        {
            int reportId = (int)reportType;

            // Crear o obtener el reporte correspondiente en la configuración
            PeriodConfigurationReportEntity reportEntity = configuration.ConfigurationReports.FirstOrDefault(r => r.ReportId == reportId);
            if (reportEntity == null)
            {
                reportEntity = new PeriodConfigurationReportEntity
                {
                    ReportId = reportId,
                    IsEnabled = false,
                    ConfigurationParameters = new List<PeriodConfigurationParameterEntity>()
                };
                configuration.ConfigurationReports.Add(reportEntity);
            }
            else
            {
                // Restablecer IsEnabled y limpiar los parámetros existentes
                reportEntity.IsEnabled = false;
                reportEntity.ConfigurationParameters.Clear();
            }

            foreach (RepeaterItem item in repeater.Items)
            {
                // Encontrar los controles dentro del ItemTemplate
                HiddenField hdfParameterId = item.FindControl("hdfParameterId") as HiddenField;
                CheckBox chkEnableConfig = item.FindControl("chkEnableConfig") as CheckBox;
                CheckBox chkPreApproval = item.FindControl("chkPreApproval") as CheckBox;
                TextBox txtResponsible = item.FindControl("txtResponsible") as TextBox;
                TextBox txtExchangeRate = isGtiTab ? item.FindControl("txtExchangeRate") as TextBox : null;

                if (hdfParameterId != null && int.TryParse(hdfParameterId.Value, out int parameterId))
                {
                    if (chkEnableConfig != null && chkEnableConfig.Checked)
                    {
                        reportEntity.IsEnabled = true;

                        PeriodConfigurationParameterEntity parameterEntity = new PeriodConfigurationParameterEntity
                        {
                            PeriodParameterDivisionCurrencyId = parameterId,
                            RequiresPreApproval = chkPreApproval != null && chkPreApproval.Checked,
                            ProcessResponsible = txtResponsible != null ? txtResponsible.Text : string.Empty,
                            ExchangeRate = null
                        };

                        if (isGtiTab && txtExchangeRate != null && decimal.TryParse(txtExchangeRate.Text, out decimal exchangeRate))
                        {
                            parameterEntity.ExchangeRate = exchangeRate;
                        }

                        reportEntity.ConfigurationParameters.Add(parameterEntity);
                    }
                }
            }
        }
    }
}
