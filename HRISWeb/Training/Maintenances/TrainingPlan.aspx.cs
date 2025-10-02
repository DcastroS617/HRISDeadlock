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
    public partial class TrainingPlan : Page
    {
        [Dependency]
        public IMasterProgramBll ObjIMasterProgramBll { get; set; }

        [Dependency]
        public ICycleTrainingBll ObjCycleTrainingBll { get; set; }

        [Dependency]
        public ITrainingPlanProgramsBll<TrainingPlanProgramEntity> ObjTrainingPlanProgramsBll { get; set; }

        public int ViewRelatedSummary
        {
            get
            {
                if (Session["TrainingPlan_ViewRelatedSummary"] != null)
                {
                    return Session["TrainingPlan_ViewRelatedSummary"].ToInt32();
                }

                return 1;
            }

            set { Session["TrainingPlan_ViewRelatedSummary"] = value; }
        }

        public List<EmployeeEntity> GridRelatedEmployees
        {
            get
            {
                if (Session["TrainingPlan_GridRelatedEmployees"] != null)
                {
                    return Session["TrainingPlan_GridRelatedEmployees"] as List<EmployeeEntity>;
                }

                return new List<EmployeeEntity>();
            }

            set { Session["TrainingPlan_GridRelatedEmployees"] = value; }
        }

        public List<PositionEntity> GridRelatedPosition
        {
            get
            {
                if (Session["TrainingPlan_GridRelatedPosition"] != null)
                {
                    return Session["TrainingPlan_GridRelatedPosition"] as List<PositionEntity>;
                }

                return new List<PositionEntity>();

            }

            set { Session["TrainingPlan_GridRelatedPosition"] = value; }
        }

        public List<LaborEntity> GridRelatedLabor
        {
            get
            {
                if (Session["TrainingPlan_GridRelatedLabor"] != null)
                {
                    return Session["TrainingPlan_GridRelatedLabor"] as List<LaborEntity>;
                }

                return new List<LaborEntity>();
            }

            set { Session["TrainingPlan_GridRelatedLabor"] = value; }
        }

        readonly string sessionKeyTrainingPlanResults = "Trainers-TrainingPlanResults";

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
                    int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                    string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                    //fire the event
                    BtnSearch_ServerClick(sender, e);

                    MatrixTargetNameFilter.Items.Clear();
                    MatrixTargetNameFilter.Items.Add(new ListItem() { Value = "", Text = "" });
                    MatrixTargetNameFilter.Items.AddRange(ObjIMasterProgramBll.MatrixTargetList(new MasterProgramEntity { DivisionCode = divisionCode }));

                    MatrixTargetIdEdit.Items.AddRange(ObjIMasterProgramBll.MatrixTargetList(new MasterProgramEntity { DivisionCode = divisionCode }));

                    TrainingPlanProgramEdit.Items.Clear();
                    TrainingPlanProgramEdit.Items.Add(new ListItem() { Value = "-1", Text = GetLocalResourceObject("msjSelectProgram").ToString() });
                    TrainingPlanProgramEdit.Items.AddRange(ObjTrainingPlanProgramsBll.TrainingPlanProgramsList(divisionCode, geographicDivisionCode));

                    CycleTrainingIdEdit.Items.Clear();
                    CycleTrainingIdEdit.Items.Add(new ListItem() { Value = "", Text = GetLocalResourceObject("msjSelectCycleTraining").ToString() });
                    CycleTrainingIdEdit.Items.AddRange(ObjCycleTrainingBll.CycleTrainingListByCatalog(new CycleTrainingEntity() { GeographicDivisionCode = geographicDivisionCode }));

                    RelatedByEdit.Items.Add(new ListItem() { Value = "1", Text = GetLocalResourceObject("realateBy1").ToString() });
                    RelatedByEdit.Items.Add(new ListItem() { Value = "2", Text = GetLocalResourceObject("realateBy2").ToString() });
                }

                //activate the pager
                if (Session[sessionKeyTrainingPlanResults] != null)
                {
                    PageHelper<MasterProgramEntity> pageHelper = (PageHelper<MasterProgramEntity>)Session[sessionKeyTrainingPlanResults];
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
                var seleectId = hdfMasterProgramIdEdit.Value == "-1" || string.IsNullOrEmpty(hdfMasterProgramIdEdit.Value) ? (long?)null : long.Parse(hdfMasterProgramIdEdit.Value);

                int? cycleTrainingId = null;
                if (!string.IsNullOrEmpty(CycleTrainingIdEdit.Value))
                {
                    var cycleTrainingIdArray = CycleTrainingIdEdit.Value.Split('|');

                    cycleTrainingId = int.Parse(cycleTrainingIdArray[0]);
                }

                var entity = new MasterProgramEntity
                {
                    MasterProgramId = seleectId,
                    MasterProgramIdExisted = !string.IsNullOrEmpty(hdfMasterProgramIdEditExisted.Value) ? long.Parse(hdfMasterProgramIdEditExisted.Value) : (long?)null,
                    MasterProgramCode = MasterProgramCodeEdit.Value.Trim(),
                    MasterProgramName = MasterProgramNameEdit.Value.Trim(),
                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SearchEnabled = SearchEnabledEdit.Checked,
                    MatrixTargetId = int.Parse(MatrixTargetIdEdit.Value),
                    TrainingPlanProgramCode = TrainingPlanProgramEdit.Value,
                    IsExpiration = IsExpirationEdit.Checked,
                    ApplyRuleRecruitmentDate = ApplyRuleRecruitmentDateEdit.Checked,
                    FromDate = !ApplyRuleRecruitmentDateEdit.Checked ? null : ToDateEN(FromDateEdit.Value),
                    ToDate = !ApplyRuleRecruitmentDateEdit.Checked ? null : ToDateEN(ToDateEdit.Value),
                    CycleTrainingId = !IsExpirationEdit.Checked ? null : cycleTrainingId
                };

                DbaEntity result = null;
                var messageDuplicated = string.Empty;

                if (seleectId.HasValue)
                {
                    result = ObjIMasterProgramBll.MasterProgramEdit(entity);
                }

                else
                {
                    result = ObjIMasterProgramBll.MasterProgramIsExists(entity);
                    if (result.ErrorMessage.Equals("MasterProgramCode"))
                    {
                        messageDuplicated = GetLocalResourceObject("CodeLbl").ToString();
                    }

                    if (result.ErrorMessage.Equals("MasterProgramName"))
                    {
                        messageDuplicated = GetLocalResourceObject("NameLbl").ToString();
                    }

                    result = ObjIMasterProgramBll.MasterProgramAdd(entity);
                }

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    var extendsJs = entity.IsExpiration ? " $('.IsExpirationEdit').show();" : "$('.IsExpirationEdit').hide();";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnPostBackAcceptClickSave{0}", Guid.NewGuid()), extendsJs + "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);", true);
                }

                else if (result.ErrorNumber != -1)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                else
                {
                    RefreshTable();

                    txtDuplicatedMasterProgramCode.Text = entity.MasterProgramCode;
                    txtDuplicatedMasterProgramName.Text = entity.MasterProgramName;
                    var typetext = (!string.IsNullOrEmpty(messageDuplicated)) ? messageDuplicated : GetLocalResourceObject("CodeLbl").ToString();

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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                string geographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                MasterProgramListEdit.Items.Clear();
                MasterProgramListEdit.Items.Add(new ListItem() { Value = "-1", Text = GetLocalResourceObject("msjSelectProgram").ToString() });
                MasterProgramListEdit.Items.AddRange(ObjIMasterProgramBll.MasterProgramList(new MasterProgramEntity { DivisionCode = divisionCode, GeographicDivisionCode = geographicDivisionCode }));

                MasterProgramListEdit.SelectedValue = "-1";

                TrainingPlanProgramEdit.SelectedIndex = 0;
                CycleTrainingIdEdit.Value = "";
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
                    long masterProgramIdSelected = Convert.ToInt64(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MasterProgramId"]);
                    DisplayEntity(sender, masterProgramIdSelected);

                    hdfMasterProgramIdEdit.Value = masterProgramIdSelected.ToString();

                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnRequestBtnEditOpen{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnEditOpen(''); }, 200);  ", true);
                }

                else
                {
                    hdfMasterProgramIdEdit.Value = "";

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

        #region Master Programs

        /// <summary>
        /// Handles the MasterProgramListEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void MasterProgramListEdit_ServerChange(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(MasterProgramListEdit.SelectedValue) && !MasterProgramListEdit.SelectedValue.Equals("-1"))
                {
                    long masterProgramIdSelected = long.Parse(MasterProgramListEdit.SelectedValue);

                    DisplayEntity(sender, masterProgramIdSelected);

                    hdfMasterProgramIdEditExisted.Value = masterProgramIdSelected.ToString();
                }
                else
                {
                    MasterProgramCodeEdit.Value = string.Empty;
                    MasterProgramNameEdit.Value = string.Empty;
                    MatrixTargetIdEdit.Value = string.Empty;
                    TrainingPlanProgramEdit.Value = "-1";
                    IsExpirationEdit.Checked = false;
                    ApplyRuleRecruitmentDateEdit.Checked = false;
                    FromDateEdit.Value = string.Empty;
                    ToDateEdit.Value = string.Empty;
                    CycleTrainingIdEdit.Value = "";
                    SearchEnabledEdit.Checked = false;
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

        #endregion

        #region Relate

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRelate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Informacion,
                        GetLocalResourceObject("msjRelationshipValidation").ToString());

                    long selectedid = Convert.ToInt64(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MasterProgramId"]);
                    var entity = new MasterProgramEntity() { MasterProgramId = selectedid };

                    var result = ObjIMasterProgramBll.MasterProgramRelationshipById(entity);

                    MasterProgramIdRelate.Value = entity.MasterProgramId.ToString();

                    PositionsIdEdit.Items.Clear();
                    PositionsIdEdit.Items.AddRange(ObjIMasterProgramBll.MasterProgramByPositionsByPlacesOccupation(entity));

                    LaborsIdEdit.Items.Clear();
                    LaborsIdEdit.Items.AddRange(ObjIMasterProgramBll.MasterProgramByLaborById(entity));

                    EmployeesIdEdit.Items.Clear();
                    EmployeesIdEdit.Items.AddRange(ObjIMasterProgramBll.MasterProgramByEmployeesByPlacesOccupation(entity));

                    var relatedbyresult = result.Item1.Relatedby ?? 1;
                    RelatedByEdit.Value = relatedbyresult.ToString();

                    PositionsIdEditMultiple.Value = string.Join(",", result.Item4);
                    LaborsIdEditMultiple.Value = string.Join(",", result.Item3);
                    EmployeesIdEditMultiple.Value = string.Join(",", result.Item2);

                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnRequestBtnRelateOpen{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnRelateOpen(''); },200);  ", true);
                }

                else
                {
                    hdfMasterProgramIdEdit.Value = "";

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

        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAcceptRelate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var seleectid = MasterProgramIdRelate.Value == "-1" || string.IsNullOrEmpty(MasterProgramIdRelate.Value) ? (long?)null : long.Parse(MasterProgramIdRelate.Value);

                var entity = new MasterProgramEntity
                {
                    MasterProgramId = seleectid,
                    Relatedby = int.Parse(RelatedByEdit.Value),
                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                };

                var positions = new List<TypeTableMultipleIdDto>();
                var Labors = new List<TypeTableMultipleIdDto>();
                var Employee = new List<TypeTableMultipleIdDto>();

                if (entity.Relatedby == 1)
                {
                    positions = string.IsNullOrEmpty(PositionsIdEditMultiple.Value) ?
                       new List<TypeTableMultipleIdDto>() :
                       PositionsIdEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto { Code = r }).ToList();

                    Labors = string.IsNullOrEmpty(LaborsIdEditMultiple.Value) ?
                       new List<TypeTableMultipleIdDto>() :
                       LaborsIdEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto { Id = int.Parse(r) }).ToList();
                }

                else
                {
                    Employee = EmployeesIdEditMultiple.Value.Split(',').Select(r => new TypeTableMultipleIdDto { Code = r }).ToList();
                }

                var result = ObjIMasterProgramBll.MasterProgramRelationship(entity, Employee.ToDataTableGet(), Labors.ToDataTableGet(), positions.ToDataTableGet());

                PageHelper<MasterProgramEntity> pageHelper = (PageHelper<MasterProgramEntity>)Session[sessionKeyTrainingPlanResults];
                PagerUtil.SetActivePage(blstPager, pageHelper.CurrentPage);

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnPostBackAcceptRelateClickSave{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnPostBackAcceptRelateClickSave(); },200);", true);
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

        #endregion

        #region Relate Summary

        /// <summary>
        /// Handles the btnRelateSummary click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRelateSummary_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    long selectedid = Convert.ToInt64(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MasterProgramId"]);
                    var entity = new MasterProgramEntity() { MasterProgramId = selectedid };

                    var result = ObjIMasterProgramBll.MasterProgramRelatedSummary(entity);

                    ViewRelatedSummary = result.Item1;

                    GridRelatedEmployees = result.Item2;
                    GridRelatedPosition = result.Item3;
                    GridRelatedLabor = result.Item4;

                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnRelateSummary_ServerClick{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnRequestBtnRelateSummaryOpen(); },200);  ", true);
                }

                else
                {
                    hdfMasterProgramIdEdit.Value = "";

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
                    long selectedMasterProgramId = Convert.ToInt64(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["MasterProgramId"]);

                    var result = ObjIMasterProgramBll.MasterProgramDelete(
                        new MasterProgramEntity
                        {
                            MasterProgramId = selectedMasterProgramId,
                        });

                    if (result.ErrorNumber == 0)
                    {
                        PageHelper<MasterProgramEntity> pageHelper = (PageHelper<MasterProgramEntity>)Session[sessionKeyTrainingPlanResults];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.MasterProgramId == selectedMasterProgramId));
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
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
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
            if (Session[sessionKeyTrainingPlanResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<MasterProgramEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert string to date
        /// </summary>
        /// <param name="dateString">DateString</param>
        /// <returns></returns>
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
        /// Display entity
        /// </summary>
        /// <param name="masterProgramIdSelected">Masterprogram Id Selected</param>
        public void DisplayEntity(object sender, long masterProgramIdSelected)
        {
            var result = ObjIMasterProgramBll.MasterProgramById(new MasterProgramEntity
            {
                MasterProgramId = masterProgramIdSelected
            });

            MasterProgramCodeEdit.Value = result.MasterProgramCode;
            MasterProgramNameEdit.Value = result.MasterProgramName;
            MatrixTargetIdEdit.Value = result.MatrixTargetId.ToString();
            TrainingPlanProgramEdit.Value = result.TrainingPlanProgramCode ?? "-1";
            IsExpirationEdit.Checked = result.IsExpiration;

            CycleTrainingIdEdit.Value = "";
            if (!result.CycleTrainingId.Equals(null))
            {
                var item = CycleTrainingIdEdit.Items.Cast<ListItem>().ToList().Where(w => w.Value.Split('|')[0] == result.CycleTrainingId.Value.ToString()).FirstOrDefault();
                CycleTrainingIdEdit.Value = item.Value;
            }

            ApplyRuleRecruitmentDateEdit.Checked = result.ApplyRuleRecruitmentDate;
            FromDateEdit.Value = result.FromDate?.ToString("MM/dd/yyyy");
            ToDateEdit.Value = result.ToDate?.ToString("MM/dd/yyyy");

            SearchEnabledEdit.Checked = result.SearchEnabled;
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<MasterProgramEntity> SearchResults(int page)
        {
            var filter = new MasterProgramEntity
            {
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                MasterProgramCode = string.IsNullOrEmpty(MasterProgramCodeFilter.Value) ? null : MasterProgramCodeFilter.Value,
                MasterProgramName = string.IsNullOrEmpty(MasterProgramNameFilter.Value) ? null : MasterProgramNameFilter.Value,
            };

            if (int.TryParse(MatrixTargetNameFilter.Value, out int matrixTargetId))
            {
                filter.MatrixTargetId = matrixTargetId;
            }

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<MasterProgramEntity> pageHelper = ObjIMasterProgramBll.MasterProgramByFilter(
                filter,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                sortExpression, sortDirection, page);

            Session[sessionKeyTrainingPlanResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyTrainingPlanResults] != null)
            {
                PageHelper<MasterProgramEntity> pageHelper = (PageHelper<MasterProgramEntity>)Session[sessionKeyTrainingPlanResults];

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
        /// 
        /// </summary>
        private void RefreshTable()
        {
            SearchResults(PagerUtil.GetActivePage(blstPager));
            DisplayResults();
        }

        #endregion

    }
}