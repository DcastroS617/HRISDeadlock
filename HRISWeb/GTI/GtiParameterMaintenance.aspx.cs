using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.GTI
{
    public partial class GtiParameterMaintenance : System.Web.UI.Page
    {
        [Dependency]
        public IGtiPeriodParameterDivisionCurrencyBLL ObjGtiPeriodParameterDivisionCurrencyBLL { get; set; }
        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionsBll { get; set; }

        readonly string sessionKeyGtiParameterResults = "GtiParameterResults";
        //readonly string sessionKeyGtiPeriodDivisionResults = "GtiPeriodDivisionResults";

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
                    LoadCurrencies(cboCurrencyFilter);
                    LoadGeographicDivisionsByDivisions(cboDivisionFilter, false);
                    LoadNominalClass(cboNominalClassFilter);

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
        private void LoadCurrencies(DropDownList dropdown)
        {
            var nominalClass = ObjGtiPeriodParameterDivisionCurrencyBLL.ListCurrenciesActive();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in nominalClass)
            {
                dropdown.Items.Add(new ListItem(item.CurrencyNameSpanish, item.CurrencyCode.ToString()));
            }
        }

        /// <summary>
        /// Method to load de year of the period
        /// </summary>
        private void LoadNominalClass(DropDownList dropdown)
        {
            var nominalClass = ObjGtiPeriodParameterDivisionCurrencyBLL.ListNominalClassEnable();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in nominalClass)
            {
                dropdown.Items.Add(new ListItem(item.NominalClassName, item.NominalClassId.ToString()));
            }
        }

        /// <summary>
        /// Method to load de year of the period
        /// </summary>
        private void LoadNominalClassByDivision(DropDownList dropdown, string geographicDivisionCode)
        {
            var nominalClass = ObjGtiPeriodParameterDivisionCurrencyBLL.ListNominalClassEnableByDivision(geographicDivisionCode);
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));
            foreach (var item in nominalClass)
            {
                dropdown.Items.Add(new ListItem(item.NominalClassName, item.NominalClassId.ToString()));
            }
        }

        /// <summary>
        /// Method to load de year of the period
        /// </summary>
        private void LoadGeographicDivisionsByDivisions(DropDownList dropdown, bool isEdit)
        {
            var listGeographicDivisionsByDivisions = ObjGtiPeriodParameterDivisionCurrencyBLL.ListGeographicDivisionsByDivisions();
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem(string.Empty, ""));

            var geographicCodes = new Dictionary<string, string>();

            foreach (var item in listGeographicDivisionsByDivisions)
            {
                ListItem listItem = new ListItem(item.DivisionName.ToString(), item.DivisionCode.ToString());

                dropdown.Items.Add(listItem);

                geographicCodes[item.DivisionCode.ToString()] = item.GeographicDivisionCode.ToString();
                //dropdown.Items.Add(new ListItem(item.DivisionName.ToString(), item.DivisionCode.ToString()));
            }
            if(isEdit)
                cboNominalClassModal.Enabled = true;

            Session["GeographicCodes"] = geographicCodes;
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<PeriodParameterDivisionCurrencyEntity> SearchResults(int page)
        {
            string input = cboNominalClassFilter.SelectedValue;
            int index = input.IndexOf('|');
            string nominalClass = input;
            var geographicDivisionCode = "";


            if (index != -1)
            {
                nominalClass = input.Substring(0, index);
            }
            if (Session["GeographicCodes"] != null)
            {
                var geographicCodes = (Dictionary<string, string>)Session["GeographicCodes"];
                string selectedValue = cboDivisionFilter.SelectedValue;
                if (geographicCodes.ContainsKey(cboDivisionFilter.SelectedValue))
                {
                    geographicDivisionCode = geographicCodes[selectedValue];
                }
            }
            var geoDivisionId = String.IsNullOrEmpty(geographicDivisionCode) ? "0" : geographicDivisionCode;
            //var nominalClass = cboNominalClassFilter.SelectedValue == "" ? "0" : cboNominalClassFilter.SelectedValue;
            var currency = cboCurrencyFilter.SelectedValue == "" ? "0" : cboCurrencyFilter.SelectedValue;

            PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity = new PeriodParameterDivisionCurrencyEntity()
            {
                PeriodParameterDivisionCurrencyName = txtPeriodPeriodParameterDivisionName.Text
                ,
                GeographicDivisionID = geoDivisionId
                ,
                NominalClassId = nominalClass
                ,
                CurrencyCode = currency
            };
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<PeriodParameterDivisionCurrencyEntity> pageHelper = ObjGtiPeriodParameterDivisionCurrencyBLL.ListGtiPeriodParameterDivisionCurrencyByFilters(periodParameterDivisionCurrencyEntity, sortExpression, sortDirection, page);


            //var listStatus = GetAllKeysAndNames<GtiPeriodStatus>();

            //foreach (var entity in pageHelper.ResultList)
            //{
            //    entity.PeriodStateDescrition = listStatus.Where(v => v.Key == entity.PeriodState).FirstOrDefault().Value;
            //}

            Session[sessionKeyGtiParameterResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyGtiParameterResults] != null)
            {
                PageHelper<PeriodParameterDivisionCurrencyEntity> pageHelper = (PageHelper<PeriodParameterDivisionCurrencyEntity>)Session[sessionKeyGtiParameterResults];

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
            //CultureInfo currentCulture = GetCurrentCulture();

            //Label lblCurrencyDescription = item.FindControl("lblCurrencyDescription") as Label;
            //Label lblDivisionCode = item.FindControl("lblDivisionCode") as Label;

            //var divisionCode = Convert.ToInt32(lblDivisionCode.Text);
            //string currencyDescription = "";

            //List<DivisionByActiveEmployeesEntity> listDivision = (List<DivisionByActiveEmployeesEntity>)Session[sessionKeyGtiPeriodDivisionResults];

            //if (currentCulture.Name.Equals(Constants.cCultureEsCR))
            //{
            //    currencyDescription = listDivision.Where(x => x.DivisionCode == divisionCode).FirstOrDefault().CurrencyNameSpanish;
            //}
            //else
            //{
            //    currencyDescription = listDivision.Where(x => x.DivisionCode == divisionCode).FirstOrDefault().CurrencyNameEnglish;
            //}

            //lblCurrencyDescription.Text = currencyDescription;

        }

        /// <summary>
        /// Display entity
        /// </summary>
        /// <param name="gtiPeriodIdSelected">Gti Period Id Selected</param>
        private void DisplayEntity(object sender, int gtiPeriodIdSelected)
        {
            var result = ObjGtiPeriodParameterDivisionCurrencyBLL.ListByKey(gtiPeriodIdSelected);

            //txtName.Text = result.PeriodCampaignDescription;

        }

        /// <summary>
        ///Clear Modal Form
        /// </summary>
        private void ClearModalForm()
        {
            // Limpiar los controles del formulario           
            cboDivisionModal.SelectedIndex = -1;
            cboNominalClassModal.SelectedIndex = -1;
            cboCurrencyModal.SelectedIndex = -1;
            chbSearchEnabled.Checked = false;

            // Desactivar el combo de clases nominales inicialmente
            cboNominalClassModal.Enabled = false;
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
                string input = cboNominalClassModal.SelectedValue;
                int index = input.IndexOf('|');
                string nominalClass = input;
                var geographicDivisionCode = "";
                
                int selectedIndex = !string.IsNullOrWhiteSpace(hdfSelectedRowIndex.Value) ? Convert.ToInt32(hdfSelectedRowIndex.Value) : 0;
                
                int parameterId;
                
                if (selectedIndex != -1)
                {
                    GridViewRow selectedRow = grvList.Rows[selectedIndex];
                    parameterId = (int)grvList.DataKeys[selectedIndex].Value == 0 ? 0 : (int)grvList.DataKeys[selectedIndex].Value;
                }
                else
                {
                    parameterId = 0;
                }
                
                

                if (index != -1)
                {
                    nominalClass = input.Substring(0, index);
                }

                if (Session["GeographicCodes"] != null)
                {
                    var geographicCodes = (Dictionary<string, string>)Session["GeographicCodes"];
                    string selectedValue = cboDivisionModal.SelectedValue;
                    if (geographicCodes.ContainsKey(cboDivisionModal.SelectedValue))
                    {
                        geographicDivisionCode = geographicCodes[selectedValue];
                    }
                }

                var entity = new PeriodParameterDivisionCurrencyEntity()
                {
                    PeriodParameterDivisionCurrencyId = parameterId
                   ,
                    PeriodParameterDivisionCurrencyName = string.Format("{0}-{1}-{2}-{3}",geographicDivisionCode,cboDivisionModal.SelectedValue,nominalClass,cboCurrencyModal.SelectedValue)
                   ,
                    DivisionCode = Int32.Parse(cboDivisionModal.SelectedValue)
                   ,
                    GeographicDivisionID = geographicDivisionCode
                   ,
                    NominalClassId = nominalClass
                   ,
                    CurrencyCode = cboCurrencyModal.SelectedValue.ToString()
                   ,
                    CurrencyNameSpanish = "Moneda"
                   ,
                    CurrencyNameEnglish = "Currency"
                   ,
                    SearchEnabled = chbSearchEnabled.Checked
                   ,
                    Deleted = false                  
                   ,
                    LastModifiedUser = UserHelper.GetCurrentFullUserName
                };


                PeriodParameterDivisionCurrencyEntity result = null;

                result = ObjGtiPeriodParameterDivisionCurrencyBLL.AddOrUpdate(entity);

                if (result.ErrorNumber == 0)
                {
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
                    txtDuplicatedPeriodId.Text = result.PeriodParameterDivisionCurrencyId.ToString();
                    txtDuplicatedPeriodDescription.Text = result.PeriodParameterDivisionCurrencyName;

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
                    txtDuplicatedPeriodId.Text = result.PeriodParameterDivisionCurrencyId.ToString();
                    txtDuplicatedPeriodDescription.Text = result.PeriodParameterDivisionCurrencyName;

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

            LoadCurrencies(cboCurrencyModal);
            LoadGeographicDivisionsByDivisions(cboDivisionModal, false);
            cboNominalClassModal.Enabled = false;

            //SearchResults(1);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowAddModal", "$('#MaintenanceDialog').modal('show');", true);
        }

        /// <summary>
        /// Handles the cboDivisionModal SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender">Refers to the DropDownList control that triggered the event.</param>
        /// <param name="e">Contains the event data.</param>
        protected void cboDivisionModal_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown = (DropDownList)sender;
            string selectedValue = dropdown.SelectedValue;
            string geographicDivisionCode = "";

            if (Session["GeographicCodes"] != null)
            {
                var geographicCodes = (Dictionary<string, string>)Session["GeographicCodes"];
                if (geographicCodes.ContainsKey(selectedValue))
                {
                    geographicDivisionCode = geographicCodes[selectedValue];
                }                
            }
            LoadNominalClassByDivision(cboNominalClassModal, geographicDivisionCode);

            cboNominalClassModal.Enabled = true;
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
                        string periodParameterDivisionCurrencyId = grvList.DataKeys[selectedIndex].Value.ToString();

                        // Cargar los detalles del periodo seleccionado para edición
                        PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity = ObjGtiPeriodParameterDivisionCurrencyBLL.ListByKey(Convert.ToInt32(periodParameterDivisionCurrencyId));

                        if (periodParameterDivisionCurrencyEntity != null)
                        {
                            //txtName.Text = periodParameterDivisionCurrencyEntity.PeriodParameterDivisionCurrencyName;
                            LoadCurrencies(cboCurrencyModal);
                            LoadGeographicDivisionsByDivisions(cboDivisionModal, true);                            
                            
                            string geographicDivisionCode = "";

                            cboDivisionModal.SelectedValue = periodParameterDivisionCurrencyEntity.DivisionCode.ToString();
                            cboCurrencyModal.SelectedValue = periodParameterDivisionCurrencyEntity.CurrencyCode;
                            string selectedValue = cboDivisionModal.SelectedValue;
                            if (Session["GeographicCodes"] != null)
                            {
                                var geographicCodes = (Dictionary<string, string>)Session["GeographicCodes"];
                                if (geographicCodes.ContainsKey(selectedValue))
                                {
                                    geographicDivisionCode = geographicCodes[selectedValue];
                                }
                            }
                            LoadNominalClassByDivision(cboNominalClassModal, geographicDivisionCode);
                            cboNominalClassModal.SelectedValue = periodParameterDivisionCurrencyEntity.NominalClassId.ToString().Trim() +"|"+ geographicDivisionCode;
                            chbSearchEnabled.Checked = periodParameterDivisionCurrencyEntity.SearchEnabled;
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
                    int periodParameterDivisionCurrencyId = Convert.ToInt32(grvList.DataKeys[selectedIndex].Value.ToString());

                    var entity = new PeriodParameterDivisionCurrencyEntity()
                    {
                        PeriodParameterDivisionCurrencyId = periodParameterDivisionCurrencyId,
                        Deleted = true,
                        LastModifiedUser = UserHelper.GetCurrentFullUserName

                    };

                    PeriodParameterDivisionCurrencyEntity result = null;

                    result = ObjGtiPeriodParameterDivisionCurrencyBLL.AddOrUpdate(entity);

                    PageHelper<PeriodParameterDivisionCurrencyEntity> pageHelper = (PageHelper<PeriodParameterDivisionCurrencyEntity>)Session[sessionKeyGtiParameterResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.PeriodParameterDivisionCurrencyId == selectedIndex));
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

        protected void BtnEnable_ServerClick(object sender, EventArgs e)
        {

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
        /// Handles the retrieval of nominal classes based on the division ID provided.
        /// </summary>
        /// <param name="divisionId">The ID of the selected division used to determine which nominal classes to load.</param>
        /// <returns>A list of ListItem representing the nominal classes associated with the division.</returns>
        [WebMethod]
        public static List<ListItem> GetNominalClasses(string divisionId)
        {
            string geographicDivisionCode = "";
            IGtiPeriodParameterDivisionCurrencyBLL objGtiPeriodParameterDivisionCurrencyBLL = null;

            // Obtén el contenedor de dependencias
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["UnityContainer"];
            if (container != null)
            {
                // Resuelve la dependencia manualmente
                objGtiPeriodParameterDivisionCurrencyBLL = container.Resolve<IGtiPeriodParameterDivisionCurrencyBLL>();
            }

            if (objGtiPeriodParameterDivisionCurrencyBLL == null)
            {
                throw new InvalidOperationException("Dependency resolution failed.");
            }

            // Verifica si el diccionario "GeographicCodes" existe en la sesión
            if (HttpContext.Current.Session["GeographicCodes"] != null)
            {
                var geographicCodes = (Dictionary<string, string>)HttpContext.Current.Session["GeographicCodes"];
                if (geographicCodes.ContainsKey(divisionId))
                {
                    geographicDivisionCode = geographicCodes[divisionId];
                }
            }

            // Usa el método estático para obtener las clases nominales
            return LoadNominalClassByDivision(geographicDivisionCode, objGtiPeriodParameterDivisionCurrencyBLL);
        }

        /// <summary>
        /// Loads nominal classes for a specific geographic division.
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code to filter the nominal classes.</param>
        /// <returns>A list of ListItem objects representing the available nominal classes for the given division.</returns>
        private static List<ListItem> LoadNominalClassByDivision(string geographicDivisionCode, IGtiPeriodParameterDivisionCurrencyBLL objGtiPeriodParameterDivisionCurrencyBLL)
        {
            // Obtiene las clases nominales habilitadas para la división
            var nominalClass = objGtiPeriodParameterDivisionCurrencyBLL.ListNominalClassEnableByDivision(geographicDivisionCode);

            // Crea una lista de ListItem
            List<ListItem> nominalClassItems = new List<ListItem>();

            // Añade un elemento vacío al principio
            nominalClassItems.Add(new ListItem(string.Empty, ""));

            // Itera sobre los elementos obtenidos y añade cada clase nominal a la lista
            foreach (var item in nominalClass)
            {
                nominalClassItems.Add(new ListItem(item.NominalClassName, item.NominalClassId.ToString()));
            }

            // Retorna la lista de clases nominales
            return nominalClassItems;
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>      
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<PeriodParameterDivisionCurrencyEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults();
        }
    }
}