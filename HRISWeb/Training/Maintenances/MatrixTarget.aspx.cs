using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Training.Maintenances
{
    public partial class MatrixTarget : Page
    {

        [Dependency]
        public IMatrixTargetBll ObjMatrixTargetBll { get; set; }

        [Dependency]
        public IDivisionsByUsersBll<DivisionByUserEntity> ObjIDivisionsByUsersBll { get; set; }

        //session key for the results
        readonly string sessionKeyMatrixTargetResults = "MatrixTarget-MatrixTargetResults";
        readonly string sessionKeyDivisionsList = "StructBy-DivisionsListResults";
        readonly string sessionKeyCompaniesList = "StructBy-CompaniesListResults";
        readonly string sessionKeyCostZoneList = "StructBy-CostZoneListResults";
        readonly string sessionKeyCostMiniZoneList = "StructBy-CostMiniZoneListResults";
        readonly string sessionKeyCostFarmList = "StructBy-CostFarmListResults";
        readonly string sessionKeyNominalClassList = "StructBy-NominalClassListResults";

        #region Properties

        /// <summary>
        /// Get or set divisions.
        /// </summary>
        public List<MatrixTargetByDivisionsEntity> Divisions
        {
            get { return Session[sessionKeyDivisionsList] as List<MatrixTargetByDivisionsEntity>; }
            set { Session[sessionKeyDivisionsList] = value; }
        }

        /// <summary>
        /// Get or set divisions multiple.
        /// </summary>
        public string DivisionsMultiple => string.Join(",", Divisions.Select(r => r.DivisionCode));

        public int DivisionCodeGlobal { get; set; }

        #endregion

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
                DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                if (!IsPostBack)
                {
                    var UserCodeSession = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation).UserCode;

                    //fire the event
                    BtnSearch_ServerClick(sender, e);

                    DivisionCodeEdit.Items.AddRange(ObjIDivisionsByUsersBll.ListByUser(UserCodeSession)
                        .Select(r => new ListItem() { Text = r.DivisionName, Value = r.DivisionCode.ToString() }
                    ).ToArray());

                    Divisions = new List<MatrixTargetByDivisionsEntity>() { new MatrixTargetByDivisionsEntity() { DivisionCode = DivisionCodeGlobal } };

                    StructByFilter.Items.Clear();
                    StructByFilter.Items.Add(new ListItem() { Value = "", Text = "" });
                    StructByFilter.Items.Add(new ListItem() { Value = "1", Text = GetLocalResourceObject("StructByFarm").ToString() });
                    StructByFilter.Items.Add(new ListItem() { Value = "2", Text = GetLocalResourceObject("StructByNominalClass").ToString() });

                    StructByEdit.Items.Add(new ListItem() { Value = "1", Text = GetLocalResourceObject("StructByFarm").ToString() });
                    StructByEdit.Items.Add(new ListItem() { Value = "2", Text = GetLocalResourceObject("StructByNominalClass").ToString() });
                }

                if (Session[sessionKeyMatrixTargetResults] != null)
                {
                    PageHelper<MatrixTargetEntity> pageHelper = (PageHelper<MatrixTargetEntity>)Session[sessionKeyMatrixTargetResults];
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
                var seleectId = hdfMatrixTargetIdEdit.Value == "-1" || string.IsNullOrEmpty(hdfMatrixTargetIdEdit.Value) ? (int?)null : int.Parse(hdfMatrixTargetIdEdit.Value);

                var divisionCodes = GetSelectedDivisions();

                #region Struct Farm

                var costZoneIdList = Session[sessionKeyCostZoneList] as ListItem[];
                var costZoneIdMultiple = GetSelectedCostZones();

                var costMiniZoneIdList = Session[sessionKeyCostMiniZoneList] as ListItem[];
                var costMiniZoneIdMultiple = GetSelectedCostMiniZones();

                var costFarmsIdList = Session[sessionKeyCostFarmList] as ListItem[];
                var costFarmsIdMultiple = GetSelectedCostFarms();

                #endregion

                #region Struct Nominal Class

                var nominalClasssId = Session[sessionKeyNominalClassList] as ListItem[];
                var nominalClassIdMultiple = GetSelectedNominalClass();

                var companyIdList = Session[sessionKeyCompaniesList] as ListItem[];
                var companyIdMultiple = GetSelectedCompanies();

                #endregion

                var entity = new MatrixTargetEntity
                {
                    MatrixTargetId = seleectId,
                    MatrixTargetCode = MatrixTargetCodeEdit.Value,
                    MatrixTargetName = MatrixTargetNameEdit.Value,
                    StructBy = int.Parse(StructByEdit.Value),
                    IsRegional = divisionCodes.Rows.Count > 1,

                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,

                    CostZonesMarkAll = CompareArrayToDataTable(costZoneIdList, costZoneIdMultiple),
                    CostMiniZonesMarkAll = CompareArrayToDataTable(costMiniZoneIdList, costMiniZoneIdMultiple),
                    CostFarmsMarkAll = CompareArrayToDataTable(costFarmsIdList, costFarmsIdMultiple),

                    CompaniesMarkAll = CompareArrayToDataTable(companyIdList, companyIdMultiple),
                    NominalClassMarkAll = CompareArrayToDataTable(nominalClasssId, nominalClassIdMultiple),

                    SearchEnabled = SearchEnabledEdit.Checked,
                };

                DbaEntity result = null;
                if (seleectId.HasValue)
                {
                    result = ObjMatrixTargetBll.MatrixTargetEdit(entity, divisionCodes, costZoneIdMultiple, costMiniZoneIdMultiple, costFarmsIdMultiple, companyIdMultiple, nominalClassIdMultiple);
                }

                else
                {
                    result = ObjMatrixTargetBll.MatrixTargetAdd(entity, divisionCodes, costZoneIdMultiple, costMiniZoneIdMultiple, costFarmsIdMultiple, companyIdMultiple, nominalClassIdMultiple);
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

                    var duplicado = ObjMatrixTargetBll.MatrixTargetById(new MatrixTargetEntity
                    {
                        MatrixTargetCode = entity.MatrixTargetCode
                    });

                    txtDuplicatedMatrixtargetCode.Text = duplicado.Item1.MatrixTargetCode;
                    txtDuplicatedMatrixtargetName.Text = duplicado.Item1.MatrixTargetName;
                    var typetext = GetLocalResourceObject("CodeLbl").ToString();

                    divDuplicatedDialogText.InnerHtml = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClearSelected();

                BtnStructByEdit_ServerClick(sender, e);
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
                ClearSelected();

                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MatrixTargetId"]);
                    UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                    var ValidacionRegional = ObjMatrixTargetBll.MatrixTargetRegionalPermit(new MatrixTargetEntity { MatrixTargetId = selectedid }, currentUser.UserCode);

                    var resultMultiple = ObjMatrixTargetBll.MatrixTargetById(new MatrixTargetEntity
                    {
                        MatrixTargetId = selectedid
                    });

                    var result = resultMultiple.Item1;
                    hdfMatrixTargetIdEdit.Value = result.MatrixTargetId.ToString();
                    MatrixTargetCodeEdit.Value = result.MatrixTargetCode.ToString();
                    MatrixTargetNameEdit.Value = result.MatrixTargetName;
                    SearchEnabledEdit.Checked = result.SearchEnabled;

                    var structByResult = result.StructBy ?? 1;
                    StructByEdit.SelectedIndex = structByResult == 1 ? 0 : 1;

                    var divisions = resultMultiple.Item2;
                    DivisionCodeEditMultiple.Value = string.Join(",", divisions.Select(r => r.DivisionCode));

                    if (result.StructBy.Equals(1))
                    {
                        LoadCostZone();
                        LoadCostMiniZone();
                        LoadCostFarms();

                        var costZones = resultMultiple.Item4;
                        CostZoneIdEditMultiple.Value = string.Join(",", costZones.Select(r => r.CostZoneId));

                        BtnCostZoneIdEdit_ServerClick(sender, e);

                        var costMiniZone = resultMultiple.Item5;
                        CostMiniZoneIdEditMultiple.Value = string.Join(",", costMiniZone.Select(r => r.CostMiniZoneId));

                        BtnCostMiniZoneIdEdit_ServerClick(sender, e);

                        var costFarms = resultMultiple.Item6;
                        CostFarmsIdEditMultiple.Value = string.Join(",", costFarms.Select(r => r.CostFarmId));
                    }

                    if (result.StructBy.Equals(2))
                    {
                        LoadCompanies();
                        LoadNominalClass();

                        var companies = resultMultiple.Item3;
                        CompanyIdEditMultiple.Value = string.Join(",", companies.Select(r => r.CompanyName));

                        BtnCompanyIdEdit_ServerClick(sender, e);

                        var nominalClass = resultMultiple.Item7;
                        NominalClassIdEditMultiple.Value = string.Join(",", nominalClass.Select(r => r.NominalClassId));
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEdit_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(" + ValidacionRegional.ErrorNumber + "); },200);  ", true);
                }

                else
                {
                    hdfMatrixTargetIdEdit.Value = "";

                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        GetLocalResourceObject("msgInvalidSelection").ToString());
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

        #region EventsSelected

        /// <summary>
        /// Handles the btnDivisionCodeEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDivisionCodeEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                LoadCompanies();

                NominalClassIdEdit.Items.Clear();

                LoadCostZone();

                CostMiniZoneIdEdit.Items.Clear();
                CostFarmsIdEdit.Items.Clear();

                ClearSelected();
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
        /// Handles the BtnStructByEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnStructByEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                LoadCompanies();

                LoadNominalClass();
                NominalClassIdEdit.Items.Clear();

                LoadCostZone();

                LoadCostMiniZone();
                CostMiniZoneIdEdit.Items.Clear();

                LoadCostFarms();
                CostFarmsIdEdit.Items.Clear();

                ClearSelected();
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
        /// Handles the btnCostZoneIdEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnCostZoneIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<MatrixTargetByCostMiniZonesEntity> costMiniZoneList = Session[sessionKeyCostMiniZoneList] as List<MatrixTargetByCostMiniZonesEntity>;

                var filterCostZones = CostZoneIdEditMultiple.Value.Split(',');
                costMiniZoneList = costMiniZoneList.Where(w => filterCostZones.Contains(w.CostZoneID)).ToList();

                var options = costMiniZoneList.AsEnumerable().Select(fr => new ListItem
                {
                    Value = fr.CostMiniZoneId,
                    Text = fr.CostMiniZoneName
                }).ToArray();

                CostMiniZoneIdEdit.Items.Clear();
                CostMiniZoneIdEdit.Items.AddRange(options);
                CostMiniZoneIdEdit.SelectedIndex = 0;
                CostMiniZoneIdEditMultiple.Value = "";

                CostFarmsIdEdit.Items.Clear();
                CostFarmsIdEditMultiple.Value = "";

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCostZoneIdEdit{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRefreshDropdownList(); },200);  ", true);
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
        protected void BtnCostMiniZoneIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<MatrixTargetByCostFarmsEntity> costFarmList = Session[sessionKeyCostFarmList] as List<MatrixTargetByCostFarmsEntity>;

                var filterCostZones = CostZoneIdEditMultiple.Value.Split(',');
                costFarmList = costFarmList.Where(w => filterCostZones.Contains(w.CostZoneID)).ToList();

                var filterCostMiniZones = CostMiniZoneIdEditMultiple.Value.Split(',');
                costFarmList = costFarmList.Where(w => filterCostMiniZones.Contains(w.CostMiniZoneID)).ToList();

                var options = costFarmList.AsEnumerable().Select(fr => new ListItem
                {
                    Value = fr.CostFarmId,
                    Text = fr.CostFarmName
                }).ToArray();

                CostFarmsIdEdit.Items.Clear();
                CostFarmsIdEdit.Items.AddRange(options);
                CostFarmsIdEditMultiple.Value = "";

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCostMiniZoneIdEdit{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRefreshDropdownList(); },200);  ", true);
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
        /// Handles the ddlOnchange click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnCompanyIdEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<MatrixTargetByNominalClassEntity> costNominalClassList = Session[sessionKeyNominalClassList] as List<MatrixTargetByNominalClassEntity>;

                var filterCompanies = CompanyIdEditMultiple.Value.Split(',');
                costNominalClassList = costNominalClassList.Where(w => filterCompanies.Contains(w.CompanyCode.ToString())).ToList();

                var options = costNominalClassList.AsEnumerable().Select(fr => new ListItem
                {
                    Value = fr.NominalClassId,
                    Text = fr.NominalClassName
                }).ToArray();

                NominalClassIdEdit.Items.Clear();
                NominalClassIdEdit.Items.AddRange(options);
                NominalClassIdEdit.SelectedIndex = 0;
                NominalClassIdEditMultiple.Value = "";

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCompanyIdEdit{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRefreshDropdownList(); },200);  ", true);
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

        #endregion

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
                    int selectedMatrixTargetId = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MatrixTargetId"]);

                    var result = ObjMatrixTargetBll.MatrixTargetDeactivate(
                        new MatrixTargetEntity
                        {
                            MatrixTargetId = selectedMatrixTargetId,
                            Deleted = true,
                        });

                    if (result.ErrorNumber == 0)
                    {
                        PageHelper<MatrixTargetEntity> pageHelper = (PageHelper<MatrixTargetEntity>)Session[sessionKeyMatrixTargetResults];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.MatrixTargetId == selectedMatrixTargetId));
                        pageHelper.TotalResults--;

                        if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                        {
                            SearchResults(pageHelper.TotalPages - 1);
                        }

                        pageHelper.UpdateTotalPages();
                        RefreshTable();
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
            if (Session[sessionKeyMatrixTargetResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<MatrixTargetEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        #region AdvancedSearch

        /// <summary>
        /// Laod cost zones
        /// </summary>
        public void LoadCostZone()
        {
            var dtDivision = DivisionCodeEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();

            var costZones = ObjMatrixTargetBll.CostZonesListEnableByDivisions(dtDivision);
            Session[sessionKeyCostZoneList] = costZones;

            var options = costZones.AsEnumerable().Select(fr => new ListItem
            {
                Value = fr.CostZoneId,
                Text = fr.CostZoneName
            }).ToArray();

            CostZoneIdEdit.Items.Clear();
            CostZoneIdEdit.Items.AddRange(options);

            LoadCostMiniZone();
        }

        /// <summary>
        /// Load cost mini zones
        /// </summary>
        public void LoadCostMiniZone()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            var dtDivision = DivisionCodeEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();
            Session[sessionKeyCostMiniZoneList] = ObjMatrixTargetBll.CostMiniZonesListEnableByDivisions(geographicDivisionCode, dtDivision);

            LoadCostFarms();
        }

        /// <summary>
        /// Load cost farms
        /// </summary>
        public void LoadCostFarms()
        {
            string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            var dtDivision = DivisionCodeEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();
            Session[sessionKeyCostFarmList] = ObjMatrixTargetBll.CostFarmsListEnableByDivisions(geographicDivisionCode, dtDivision);
        }

        /// <summary>
        /// Load companies
        /// </summary>
        public void LoadCompanies()
        {
            var dtDivision = DivisionCodeEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();

            var companies = ObjMatrixTargetBll.CompaniesListEnableByDivision(dtDivision);
            Session[sessionKeyCompaniesList] = companies;

            var options = companies.AsEnumerable().Select(fr => new ListItem
            {
                Value = fr.CompanyID,
                Text = fr.CompanyName
            }).ToArray();

            CompanyIdEdit.Items.Clear();
            CompanyIdEdit.Items.AddRange(options);

            LoadNominalClass();
        }

        /// <summary>
        /// Load nominal classes
        /// </summary>
        public void LoadNominalClass()
        {
            var divisions = DivisionCodeEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();
            Session[sessionKeyNominalClassList] = ObjMatrixTargetBll.NominalClassListEnabledByCompanies(divisions);
        }

        /// <summary>
        /// Gets the selected divisions
        /// </summary>
        public DataTable GetSelectedDivisions()
        {
            var divisions = DivisionCodeEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto
            {
                Id = int.Parse(r)
            }).ToList().ToDataTableGet();

            return divisions;
        }

        /// <summary>
        /// Gets the selected cost zones
        /// </summary>
        public DataTable GetSelectedCostZones()
        {
            var dtCostZones = CostZoneIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostZones;
        }

        /// <summary>
        /// Gets the selected cost mini zones
        /// </summary>
        public DataTable GetSelectedCostMiniZones()
        {
            var dtCostMiniZones = CostMiniZoneIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Gets the selected cost farms
        /// </summary>
        public DataTable GetSelectedCostFarms()
        {
            var dtCostMiniZones = CostFarmsIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Gets the selected companies
        /// </summary>
        public DataTable GetSelectedCompanies()
        {
            var dtCompanies = CompanyIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                if (string.IsNullOrEmpty(values.ElementAtOrDefault(0)))
                {
                    return new TypeTableMultipleIdDto();
                }
                return new TypeTableMultipleIdDto { Id = int.Parse(values.ElementAtOrDefault(0)), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCompanies;
        }

        /// <summary>
        /// Gets the selected nominal class
        /// </summary>
        public DataTable GetSelectedNominalClass()
        {
            var dtCostMiniZones = NominalClassIdEditMultiple.Value.Split(',').Select(r =>
            {
                var values = r.Split('|').ToList();
                return new TypeTableMultipleIdDto { Code = values.ElementAtOrDefault(0), KeyValue1 = values.ElementAtOrDefault(1) };
            }).ToList().ToDataTableGet();

            return dtCostMiniZones;
        }

        /// <summary>
        /// Clear all options selected
        /// </summary>
        public void ClearSelected()
        {
            CostZoneIdEditMultiple.Value = "";
            CostMiniZoneIdEditMultiple.Value = "";
            CostFarmsIdEditMultiple.Value = "";

            CompanyIdEditMultiple.Value = "";
            NominalClassIdEditMultiple.Value = "";
        }

        #endregion

        /// <summary>
        /// Compare elements of Array with DataTable
        /// </summary>
        public bool CompareArrayToDataTable(ListItem[] listItem, DataTable dataTable)
        {
            var listItems = listItem != null ? listItem.Length : 0;
            return listItems == dataTable.Rows.Count;
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<MatrixTargetEntity> SearchResults(int page)
        {
            var filter = new MatrixTargetEntity
            {
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                MatrixTargetCode = string.IsNullOrEmpty(MatrixTargetCodeFilter.Value) ? null : MatrixTargetCodeFilter.Value,
                MatrixTargetName = string.IsNullOrEmpty(MatrixTargetNameFilter.Value) ? null : MatrixTargetNameFilter.Value,
                StructBy = null,
            };

            if (!string.IsNullOrEmpty(StructByFilter.Value))
            {
                filter.StructBy = int.Parse(StructByFilter.Value);
            }

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            int DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            PageHelper<MatrixTargetEntity> pageHelper = ObjMatrixTargetBll.MatrixTargetByFilter(
                filter, GetLocalResourceObject("Lang").ToString(), DivisionCode,
                sortExpression, sortDirection, page);

            Session[sessionKeyMatrixTargetResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyMatrixTargetResults] != null)
            {
                PageHelper<MatrixTargetEntity> pageHelper = (PageHelper<MatrixTargetEntity>)Session[sessionKeyMatrixTargetResults];

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