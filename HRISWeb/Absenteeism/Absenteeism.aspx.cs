using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.Business.Remote;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using DOLE.HRIS.Shared.Entity.MassiveData;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;
using Employee = DOLE.HRIS.Shared.Entity.ADAM.Employee;

namespace HRISWeb.Absenteeism
{
    public partial class Absenteeism : System.Web.UI.Page
    {
        [Dependency]
        public IAbsenteeismCausesBll<AbsenteeismCauseEntity> ObjCausesBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll ObjGeneralConfigurationsBll { get; set; }

        [Dependency]
        public IAbsenteeismInterestGroupBll<AbsenteeismInterestGroupEntity> ObjInterestGroupBll { get; set; }

        //session key for the results
        readonly string sessionKeyInterestGroupAbsenteeism = "Absenteeism-InterestGroupCodes";
        readonly string sessionKeyAbsenteeismInitialDataResults = "Absenteeism-InitialDataResults";
        readonly string sessionKeyAbsenteeismCausesDataResults = "Absenteeism-CausesDataResults";
        readonly string sessionKeyGroupDataSelected = "Absenteeism-GroupDataSelected";
        readonly string sessionKeyEmployeesLoaded = "Absenteeism-EmployeesLoaded";
        readonly string sessionKeyDocumentsSaved = "Absenteeism-DocumentsSaved";
        readonly string sessionKeyEmployeesFilterLoaded = "Absenteeism-EmployeesFilterLoaded";
        readonly string sessionKeyReleatedCaseSelected = "Absenteeism-AbsenteeismReleatedCaseSelected";
        readonly string sessionKeyFilterAbsenteeismLoaded = "Absenteeism-FilterAbsenteeismLoaded";
        readonly string sessionKeyDivisionCode = "Absenteeism-DivisionCodeUser";
        readonly string sessionKeyEmployeesByFilter = "Absenteeism-AbsenteeismEmployeesByFilter";
        readonly string sessionAbsenteeismCausesCategory = "Absenteeism-CausesCategory";
        readonly string sessionAbsenteeismCausesAdditionalInfo = "Absenteeism-CausesAdditionalInfo";
        readonly string serviceTimeout = "ServiceTimeout";

        //Object to load and share initial data from the service
        AbsenteeismsInitialData initialData;
        List<AbsenteeismCauseEntity> absenteeismCauses;

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
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.AsyncPostBackTimeout = Convert.ToInt32(ConfigurationManager.AppSettings[serviceTimeout].ToString());

            //HACK: DIFERENCIA REGIONAL #1 y #2: RESUELTA! SE ESTANDRIZARON LOS LABELS PARA LAS DIVISIONES!
            var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            //HACK: DIFERENCIA REGIONAL #?
            lblEstablishments.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblEstablishmentsECU") : GetLocalResourceObject("lblEstablishments"));
            lblUnsafeActs.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblUnsafeActsECU") : GetLocalResourceObject("lblUnsafeActs"));
            lblPersonalFactor.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblPersonalFactorECU") : GetLocalResourceObject("lblPersonalFactor"));
            lblUnsafeConditions.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblUnsafeConditionsECU") : GetLocalResourceObject("lblUnsafeConditions"));

            if (Session[sessionKeyDivisionCode] == null)
            {
                Session[sessionKeyDivisionCode] = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            }

            try
            {
                if (!IsPostBack)
                {
                    if (Convert.ToInt32(Session[sessionKeyDivisionCode]) != SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode)
                    {
                        Session[sessionKeyDivisionCode] = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        Session[sessionKeyAbsenteeismInitialDataResults] = null;
                    }

                    Session[sessionKeyAbsenteeismCausesDataResults] = null;
                    Session[sessionKeyGroupDataSelected] = null;
                    Session[sessionKeyEmployeesLoaded] = null;
                    Session[sessionKeyDocumentsSaved] = null;
                    Session[sessionAbsenteeismCausesCategory] = null;
                    Session[sessionAbsenteeismCausesAdditionalInfo] = null;

                    LoadInitialData();

                    cboPayroll.Enabled = true;
                    cboPayroll.DataValueField = "PayrollCode";
                    cboPayroll.DataTextField = "PayrollDescription";
                    cboPayroll.DataSource = GetPayroll();
                    cboPayroll.DataBind();

                    cboInterestGroups.Enabled = true;

                    //HACK: DIFERENCIA REGIONAL #4 y #5        
                    if (DivisionSession == 1//División de Costa Rica
                        || DivisionSession == 11//Doce Costa Rica
                        || DivisionSession == 15//Dole Tropical Products
                        || DivisionSession == 16//Dole Shared Services
                        || DivisionSession == 5//División de Ecuador
                        || DivisionSession == 9//División de Perú
                        || DivisionSession == 4//División de Honduras
                        || DivisionSession == 14) //División de Guatemala

                    {
                        cboGroupType.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("GroupTypeEmployeeDescription")), "Employee")); //trabajador
                        cboGroupType.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("GroupTypeSeatDescription")), "Seat")); //plaza
                    }

                    else
                    {
                        cboGroupType.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("GroupTypeEmployeeDescription")), "Employee")); //trabajador
                        cboGroupType.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("GroupTypePositionDescription")), "Position")); //puesto
                        cboGroupType.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("GroupTypeSeatDescription")), "Seat")); //plaza
                    }

                    cboGroupType.SelectedIndex = 0;

                    cboGroup.Enabled = true;
                    cboGroup.DataValueField = "GroupCode";
                    cboGroup.DataTextField = "GroupDescription";
                    cboGroup.DataSource = GetGroupByType();
                    cboGroup.DataBind();

                    LoadGroupData();

                    cboCaseState.Enabled = true;
                    cboCaseState.DataValueField = "CDUCode";
                    cboCaseState.DataTextField = "CDUDescription";
                    initialData.StateCases.Insert(0, new Cdu() { CDUCode = "-1", CDUDescription = "" });
                    cboCaseState.DataSource = initialData.StateCases.Where(x => x.CDUCode == "OP").ToList<Cdu>();
                    cboCaseState.DataBind();

                    cboCause.Enabled = true;
                    cboCause.DataValueField = "CauseCode";
                    cboCause.DataTextField = "CauseName";
                    cboCause.DataSource = absenteeismCauses;
                    cboCause.DataBind();

                    divAccidentCase.Visible = false;
                    divState.Visible = false;
                    divRelated.Visible = false;
                    divCloseDate.Visible = false;

                    //Time unit
                    cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                    cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                    cboTimeUnit.ClearSelection();

                    grvEmployees.DataSource = LoadEmptyEmployees();
                    grvEmployees.DataBind();

                    cboUnsafeActs.Enabled = true;
                    cboUnsafeActs.DataValueField = "CDUCode";
                    cboUnsafeActs.DataTextField = "CDUCodeDescription";
                    cboUnsafeActs.DataSource = GetCdUsWithCodeAndDescription(initialData.UnsafeActs, true);
                    cboUnsafeActs.DataBind();

                    cboMaterialAgents.Enabled = true;
                    cboMaterialAgents.DataValueField = "CDUCode";
                    cboMaterialAgents.DataTextField = "CDUCodeDescription";
                    cboMaterialAgents.DataSource = GetCdUsWithCodeAndDescription(initialData.MaterialAgents, true);
                    cboMaterialAgents.DataBind();

                    cboUnsafeConditions.Enabled = true;
                    cboUnsafeConditions.DataValueField = "CDUCode";
                    cboUnsafeConditions.DataTextField = "CDUCodeDescription";
                    cboUnsafeConditions.DataSource = GetCdUsWithCodeAndDescription(initialData.UnsafeConditions, true);
                    cboUnsafeConditions.DataBind();

                    cboEstablishments.Enabled = true;
                    cboEstablishments.DataValueField = "CDUCode";
                    cboEstablishments.DataTextField = "CDUCodeDescription";
                    cboEstablishments.DataSource = GetCdUsWithCodeAndDescription(initialData.Establishments, true);
                    cboEstablishments.DataBind();

                    cboDayFactor.Enabled = true;
                    cboDayFactor.DataValueField = "CDUCode";
                    cboDayFactor.DataTextField = "CDUCodeDescription";
                    cboDayFactor.DataSource = GetCdUsWithCodeAndDescription(initialData.DayFactors, true);
                    cboDayFactor.DataBind();

                    cboPersonalFactor.Enabled = true;
                    cboPersonalFactor.DataValueField = "CDUCode";
                    cboPersonalFactor.DataTextField = "CDUCodeDescription";
                    cboPersonalFactor.DataSource = GetCdUsWithCodeAndDescription(initialData.PersonalFactors, true);
                    cboPersonalFactor.DataBind();

                    cboJourneys.Enabled = true;
                    cboJourneys.DataValueField = "CDUCode";
                    cboJourneys.DataTextField = "CDUCodeDescription";
                    cboJourneys.DataSource = GetCdUsWithCodeAndDescription(initialData.Journeys, true);
                    cboJourneys.DataBind();

                    cboLaborMade.Enabled = true;
                    cboLaborMade.DataValueField = "CDUCode";
                    cboLaborMade.DataTextField = "CDUCodeDescription";
                    cboLaborMade.DataSource = GetCdUsWithCodeAndDescription(initialData.LaborMade, true);
                    cboLaborMade.DataBind();

                    cboBodyParts.Enabled = true;
                    cboBodyParts.DataValueField = "CDUCode";
                    cboBodyParts.DataTextField = "CDUCodeDescription";
                    cboBodyParts.DataSource = GetCdUsWithCodeAndDescription(initialData.BodyParts, true);
                    cboBodyParts.DataBind();

                    cboAccidentType.Enabled = true;
                    cboAccidentType.DataValueField = "CDUCode";
                    cboAccidentType.DataTextField = "CDUCodeDescription";
                    cboAccidentType.DataSource = GetCdUsWithCodeAndDescription(initialData.AccidentTypes, true);
                    cboAccidentType.DataBind();

                    cboDiseaseType.Enabled = true;
                    cboDiseaseType.DataValueField = "CDUCode";
                    cboDiseaseType.DataTextField = "CDUCodeDescription";
                    cboDiseaseType.DataSource = GetCdUsWithCodeAndDescription(initialData.DiseaseTypes, true);
                    cboDiseaseType.DataBind();

                    cboDocuments.Enabled = true;
                    cboDocuments.DataValueField = "DocumentCode";
                    cboDocuments.DataTextField = "DocumentDescription";
                    cboDocuments.DataSource = GetDocumentsWithCodeAndDescription(initialData.Documents, true);
                    cboDocuments.DataBind();

                    Session[sessionKeyEmployeesFilterLoaded] = null;

                }

                trvGroupData.Attributes.Add("onclick", "postBackTrvGroupDataByObject();");
                txtNaturalDays.Attributes.Add("readonly", "readonly"); //readonly attr keep value on postback

                SaveEmployeesState();
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

        private DataTable LoadEmptyFilterAbsenteeism()
        {
            DataTable absenteeisms = new DataTable();
            absenteeisms.Columns.Add("EmployeeName", typeof(string));
            absenteeisms.Columns.Add("AbsteeismId", typeof(string));
            absenteeisms.Columns.Add("StartDate", typeof(string));
            absenteeisms.Columns.Add("Description", typeof(string));
            absenteeisms.Columns.Add("descripcion_detallada", typeof(string));
            absenteeisms.Columns.Add("fecha_inicio", typeof(string));
            absenteeisms.Columns.Add("tiempo", typeof(string));
            absenteeisms.Columns.Add("unidad_tiempo", typeof(string));
            absenteeisms.Columns.Add("fecha_regreso", typeof(string));
            absenteeisms.Columns.Add("caso", typeof(string));
            absenteeisms.Columns.Add("causa", typeof(string));
            absenteeisms.Columns.Add("estado_caso", typeof(string));
            absenteeisms.Columns.Add("fecha_cierre", typeof(string));
            absenteeisms.Columns.Add("fecha_final", typeof(string));
            absenteeisms.Columns.Add("fecha_suspende", typeof(string));
            absenteeisms.Columns.Add("GrupoInteres_RSC", typeof(string));
            absenteeisms.Columns.Add("au_relacionado", typeof(string));
            absenteeisms.Columns.Add("actos_inseguros", typeof(string));
            absenteeisms.Columns.Add("agentes_materiales", typeof(string));
            absenteeisms.Columns.Add("condiciones_inseguras", typeof(string));
            absenteeisms.Columns.Add("lugares", typeof(string));
            absenteeisms.Columns.Add("Factor_de_Jornada", typeof(string));
            absenteeisms.Columns.Add("Factores_Personales", typeof(string));
            absenteeisms.Columns.Add("Jornadas", typeof(string));
            absenteeisms.Columns.Add("Labores", typeof(string));
            absenteeisms.Columns.Add("Partes_Cuerpo", typeof(string));
            absenteeisms.Columns.Add("Tipos_Accidente", typeof(string));
            absenteeisms.Columns.Add("Tipos_Enfermedad", typeof(string));
            absenteeisms.Columns.Add("IsSelected", typeof(bool));

            return absenteeisms;
        }

        private DataTable LoadEmptyRelatedCase()
        {
            DataTable absenteeisms = new DataTable();
            absenteeisms.Columns.Add("EmployeeName", typeof(string));
            absenteeisms.Columns.Add("AbsteeismId", typeof(string));
            absenteeisms.Columns.Add("StartDate", typeof(string));
            absenteeisms.Columns.Add("Description", typeof(string));
            absenteeisms.Columns.Add("IsSelected", typeof(bool));

            return absenteeisms;
        }

        public void AddAbsenteeismsFilterLoaded(List<AbsenteeismCase> absenteeisms)
        {
            if (Session[sessionKeyFilterAbsenteeismLoaded] == null)
            {
                Session[sessionKeyFilterAbsenteeismLoaded] = LoadEmptyFilterAbsenteeism();
            }

            DataTable loadedAbsenteeisms = (DataTable)Session[sessionKeyFilterAbsenteeismLoaded];
            int i = 0;

            foreach (AbsenteeismCase absenteeism in absenteeisms)
            {
                DataTable employees = new DataTable();
                employees.Columns.Add("Nombre", typeof(string));

                foreach (KeyValuePair<Company, Employee> ce in absenteeism.Employees)
                {
                    employees.Rows.Add(ce.Value.Name);
                }

                DataRow row = loadedAbsenteeisms.NewRow();
                row.ItemArray = new object[] {
                    employees.Rows[0][0],
                    absenteeism.AbsenteeismId,
                    absenteeism.StartDate,
                    absenteeism.Description,
                    absenteeism.DetailedDescription,
                    absenteeism.StartDate,
                    absenteeism.NaturalDays,
                    absenteeism.TimeUnit,
                    absenteeism.ReturnDate,
                    absenteeism.Case,
                    absenteeism.Cause,
                    absenteeism.State,
                    absenteeism.CloseDate,
                    absenteeism.FinalDate,
                    absenteeism.SuspensionDate,
                    absenteeism.InterestGroup,
                    absenteeism.RelatedAu,
                    absenteeism.UnsafeActs,
                    absenteeism.MaterialAgents,
                    absenteeism.UnsafeConditions,
                    absenteeism.Establishments,
                    absenteeism.DayFactors,
                    absenteeism.PersonalFactors,
                    absenteeism.Journeys,
                    absenteeism.LaborMade,
                    absenteeism.BodyParts,
                    absenteeism.AccidentTypes,
                    absenteeism.DiseaseTypes,
                    false
                };

                loadedAbsenteeisms.Rows.InsertAt(row, i++);
            }
        }

        public void AddAbsenteeismsRelatedCaseLoaded(List<AbsenteeismCase> absenteeisms)
        {
            if (Session[sessionKeyReleatedCaseSelected] == null)
            {
                Session[sessionKeyReleatedCaseSelected] = LoadEmptyRelatedCase();
            }

            DataTable loadedAbsenteeisms = (DataTable)Session[sessionKeyReleatedCaseSelected];
            int i = 0;

            foreach (AbsenteeismCase absenteeism in absenteeisms)
            {
                DataTable employees = new DataTable();
                employees.Columns.Add("Nombre", typeof(string));

                foreach (KeyValuePair<Company, Employee> ce in absenteeism.Employees)
                {
                    employees.Rows.Add(ce.Value.Name);
                }
                DataRow row = loadedAbsenteeisms.NewRow();
                row.ItemArray = new object[] {
                    employees.Rows[0][0],
                    absenteeism.AbsenteeismId,
                    absenteeism.StartDate,
                    absenteeism.Description,
                    false
                };

                loadedAbsenteeisms.Rows.InsertAt(row, i++);
            }
        }

        /// Get the causes with additional information to register
        /// </summary>
        /// <returns>The causes with additional information to register</returns>
        public string GetCausesWithAdditionalInformation()
        {
            try
            {
                LoadInitialData();
                string causeCodeList = "";

                if (Session[sessionAbsenteeismCausesAdditionalInfo] == null)
                {
                    List<AbsenteeismCauseEntity> causesWithAdditionalInformation = absenteeismCauses.Where(a => a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.NeedsAdditionalInformation)).ToList();

                    causeCodeList = string.Join(",", causesWithAdditionalInformation.Select(c => string.Format("'{0}'", c.CauseCode)).ToList<string>());
                    Session[sessionAbsenteeismCausesAdditionalInfo] = causeCodeList;
                }

                else
                {
                    causeCodeList = Session[sessionAbsenteeismCausesAdditionalInfo].ToString();
                }

                return causeCodeList;
            }

            catch
            {
                return string.Empty;
            }
        }

        /// Get the causes with category accident
        /// </summary>
        /// <returns>The causes category accident</returns>
        public string GetCausesWithCategoryAccident()
        {
            try
            {
                //LoadInitialData();
                List<string> Codes = null;

                if (Session[sessionAbsenteeismCausesCategory] == null)
                {
                    //GetParameter AccidenteCauseCode
                    GeneralConfigurationEntity configuration = Session[CauseCategoryAccidentSesion] as GeneralConfigurationEntity;

                    Codes = new List<string>(configuration.GeneralConfigurationValue.Split(','));
                    Session[sessionAbsenteeismCausesCategory] = Codes;
                }

                else
                {
                    Codes = (List<string>)Session[sessionAbsenteeismCausesCategory];
                }

                string causeCodeList = string.Join(",", Codes.Select(c => string.Format("'{0}'", c)).ToList<string>());

                return causeCodeList;
            }

            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Save employees state
        /// </summary>
        private void SaveEmployeesState()
        {
            if (Session[sessionKeyEmployeesLoaded] == null)
            {
                Session[sessionKeyEmployeesLoaded] = LoadEmptyEmployees();
            }

            DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];

            foreach (GridViewRow item in grvEmployees.Rows)
            {
                CheckBox chbEmployee = (CheckBox)item.FindControl("chbEmployee");
                HiddenField hdfUniqueKey = (HiddenField)item.FindControl("hdfUniqueKey");

                if (hdfUniqueKey != null)
                {
                    string participantUniqueKey = hdfUniqueKey.Value;
                    DataRow selectedRow = loadedEmployees.AsEnumerable().FirstOrDefault(r => r.Field<string>("UniqueKey") == participantUniqueKey);
                    if (selectedRow != null)
                    {
                        selectedRow.SetField<bool>("IsSelected", chbEmployee.Checked);
                    }
                }
            }
        }

        /// <summary>
        /// Return the groups by the selected type 
        /// </summary>
        /// <returns>The groups by the selected type </returns>
        private List<Group> GetGroupByType()
        {
            List<Group> groupsSelected;

            //employee
            if (cboGroupType.SelectedItem.Value == "Employee")
            {
                groupsSelected = initialData.GroupsTypeEmployee;
            }

            //Seat
            else if (cboGroupType.SelectedItem.Value == "Seat")
            {
                groupsSelected = initialData.GroupsTypeSeat;
            }

            //Position
            else if (cboGroupType.SelectedItem.Value == "Position")
            {
                groupsSelected = initialData.GroupsTypePosition;
            }

            else
            {
                groupsSelected = new List<Group>();
            }

            return groupsSelected;
        }

        /// <summary>
        /// Return the combination of companies and payroll classes
        /// </summary>
        /// <returns>The combination of companies and payroll classes </returns>
        private DataTable GetPayroll()
        {
            DataTable payroll = LoadEmptyPayroll();

            foreach (Company c in initialData.Companies)
            {
                List<PayrollClass> payrollOfCompany = initialData.PayrollClasses.Where(x => x.CompanyCode == c.CompanyCode).ToList();
                foreach (PayrollClass p in payrollOfCompany)
                {
                    //HACK: DIFERENCIA REGIONAL #6 y #7
                    var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                    if (DivisionSession == 1//División de Costa Rica
                        || DivisionSession == 11//Doce Costa Rica
                        || DivisionSession == 15//Dole Tropical Products
                        || DivisionSession == 16)//Dole Shared Services
                    {
                        payroll.Rows.Add(string.Format("{0}-{1}", c.CompanyCode, p.PayrollClassCode), string.Format("{0}-{1}", c.CompanyCode, p.PayrollClassDescription));
                    }
                    else
                    {
                        payroll.Rows.Add(string.Format("{0}-{1}", c.CompanyCode, p.PayrollClassCode), string.Format("{0}-{1} {2}", c.CompanyCode, p.PayrollClassCode, p.PayrollClassDescription));
                    }

                }
            }

            return payroll;
        }

        /// <summary>
        /// Return the cdu's with code and description
        /// </summary>
        /// <param name="cdus">The cdus</param>
        /// <param name="insertEmptyOption">True if an empty option mut be added. False otherwise.</param>
        /// <returns>The cdu's </returns>
        private DataTable GetCdUsWithCodeAndDescription(List<Cdu> cdus, bool insertEmptyOption)
        {
            DataTable procesedCdUs = LoadEmptyCdUs();

            if (insertEmptyOption)
            {
                procesedCdUs.Rows.Add("-1", string.Empty);
            }

            foreach (Cdu c in cdus)
            {
                procesedCdUs.Rows.Add(c.CDUCode.Trim(), string.Format("{0}", c.CDUDescription));
            }

            return procesedCdUs;
        }

        /// <summary>
        /// Return the documents with code
        /// </summary>
        /// <param name="cdus">The cdus</param>
        /// <param name="insertEmptyOption">True if an empty option mut be added. False otherwise.</param>
        /// <returns>The documents </returns>
        private DataTable GetDocumentsWithCodeAndDescription(List<Document> documents, bool insertEmptyOption)
        {
            DataTable procesedDocuments = LoadEmptyDocuments();

            if (insertEmptyOption)
            {
                procesedDocuments.Rows.Add(string.Empty, string.Empty);
            }

            foreach (Document d in documents)
            {
                procesedDocuments.Rows.Add(d.DocumentCode, d.DocumentCode);
            }

            return procesedDocuments;
        }

        public string CauseCategoryAccidentSesion { get; set; } = "CauseCategoryAccidentTemp";

        /// <summary>
        /// Load employees from database
        /// </summary>        
        private void LoadInitialData()
        {
            if (Session[sessionKeyAbsenteeismInitialDataResults] == null)
            {
                DOLE.HRIS.Services.CR.Business.GroupsBll objGroupsBll = new DOLE.HRIS.Services.CR.Business.GroupsBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                DOLE.HRIS.Services.CR.Business.CompaniesBll objCompaniesBll = new DOLE.HRIS.Services.CR.Business.CompaniesBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                DOLE.HRIS.Services.CR.Business.CdUsBll objCdUsBll = new DOLE.HRIS.Services.CR.Business.CdUsBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                DOLE.HRIS.Services.CR.Business.PayrollClassesBll objPayrollClassesBll = new DOLE.HRIS.Services.CR.Business.PayrollClassesBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                DOLE.HRIS.Services.CR.Business.DocumentsBll objDocumentsBll = new DOLE.HRIS.Services.CR.Business.DocumentsBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                List<Company> companies = objCompaniesBll.ListByUser(HttpContext.Current.User.Identity.Name);
                List<PayrollClass> payrollClasses = objPayrollClassesBll.ListByCompanyUser(null, HttpContext.Current.User.Identity.Name);
                List<Cdu> stateCases = objCdUsBll.ListCaseStates();
                List<Cdu> absenteeismCauses = new List<Cdu>();
                List<Cdu> insterestGroups = objCdUsBll.ListInsterestGroups();
                List<Group> groupsTypeEmployee = objGroupsBll.ListByType(0);
                List<Group> groupsTypeSeat = objGroupsBll.ListByType(1);
                List<Group> groupsTypePosition = objGroupsBll.ListByType(2);
                List<GroupData> groupData = new List<GroupData>();

                List<Cdu> unsafeActs = objCdUsBll.ListUnsafeActs();
                List<Cdu> materialAgents = objCdUsBll.ListMaterialAgents();
                List<Cdu> unsafeConditions = objCdUsBll.ListUnsafeConditions();
                List<Cdu> establishments = objCdUsBll.ListEstablishments();
                List<Cdu> dayFactors = objCdUsBll.ListDayFactors();
                List<Cdu> personalFactors = objCdUsBll.ListPersonalFactors();
                List<Cdu> journeys = objCdUsBll.ListJourneys();
                List<Cdu> laborMade = objCdUsBll.ListLaborMade();
                List<Cdu> bodyParts = objCdUsBll.ListBodyParts();
                List<Cdu> accidentTypes = objCdUsBll.ListAccidentTypes();
                List<Cdu> diseaseTypes = objCdUsBll.ListDiseaseTypes();

                List<Document> documents = objDocumentsBll.List();

                AbsenteeismsInitialData absenteeismsInitialData = new AbsenteeismsInitialData(
                    companies, payrollClasses, stateCases, absenteeismCauses, insterestGroups, groupsTypeEmployee, groupsTypeSeat, groupsTypePosition, groupData,
                    unsafeActs, materialAgents, unsafeConditions, establishments, dayFactors, personalFactors, journeys, laborMade, bodyParts, accidentTypes, diseaseTypes,
                    documents);


                Session[sessionKeyAbsenteeismInitialDataResults] = absenteeismsInitialData;

                var datatable = absenteeismsInitialData.InterestGroups.ToDataTableGet();
                ObjInterestGroupBll.UpdateInterestGroupFromADAM(datatable, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);
            }

            if (Session[sessionKeyAbsenteeismCausesDataResults] == null)
            {

                List<AbsenteeismCauseEntity> causes = ObjCausesBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);
                AbsenteeismCauseEntity emptyCause = new AbsenteeismCauseEntity()
                {
                    CauseCode = "-1",
                    CauseName = ""
                };
                causes.Insert(0, emptyCause);
                Session[sessionKeyAbsenteeismCausesDataResults] = causes;
            }

            if (Session[CauseCategoryAccidentSesion] == null)
            {
                GeneralConfigurationEntity configuration = ObjGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.CauseCategoryAccident);
                Session[CauseCategoryAccidentSesion] = configuration;
            }

            initialData = (AbsenteeismsInitialData)Session[sessionKeyAbsenteeismInitialDataResults];

            absenteeismCauses = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
        }

        /// <summary>
        /// Load empty data structure for employees
        /// </summary>
        /// <returns>Empty data structure for employees</returns>
        private DataTable LoadEmptyEmployees()
        {
            DataTable employees = new DataTable();
            employees.Columns.Add("UniqueKey", typeof(string));
            employees.Columns.Add("IsSelected", typeof(bool));
            employees.Columns.Add("EmployeeCode", typeof(string));
            employees.Columns.Add("EmployeeName", typeof(string));
            employees.Columns.Add("CompanyCode", typeof(string));
            employees.Columns.Add("GroupCode-GroupDataCode", typeof(string));
            employees.Columns.Add("GroupDataDescription", typeof(string));
            employees.Columns.Add("Company-PayrollClass", typeof(string));
            employees.Columns.Add("CostCenter", typeof(string));

            return employees;
        }

        /// <summary>
        /// Load empty data structure for payroll
        /// </summary>
        /// <returns>Empty data structure for payroll</returns>
        private DataTable LoadEmptyPayroll()
        {
            DataTable payroll = new DataTable();
            payroll.Columns.Add("PayrollCode", typeof(string));
            payroll.Columns.Add("PayrollDescription", typeof(string));

            return payroll;
        }

        /// <summary>
        /// Load empty data structure for interest groups
        /// </summary>
        /// <returns>Empty data structure for interest groups</returns>
        private DataTable LoadEmptyCdUs()
        {
            DataTable CdUs = new DataTable();
            CdUs.Columns.Add("CDUCode", typeof(string));
            CdUs.Columns.Add("CDUCodeDescription", typeof(string));

            return CdUs;
        }

        /// <summary>
        /// Load empty data structure for interest groups
        /// </summary>
        /// <returns>Empty data structure for interest groups</returns>
        private DataTable LoadEmptyDocuments()
        {
            DataTable documents = new DataTable();
            documents.Columns.Add("DocumentCode", typeof(string));
            documents.Columns.Add("DocumentDescription", typeof(string));

            return documents;
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
                string control = Request.Form["__EVENTTARGET"];
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
        /// Handles the event selectedIndexChanged on cboGroupType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboPayrollClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadInitialData();
                LoadGroupData();
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
        /// Handles the event selectedIndexChanged on cboGroupType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboPayroll_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadInitialData();
                LoadGroupData();
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
        /// Handles the event selectedIndexChanged on cboGroupType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadInitialData();

                cboGroup.DataSource = GetGroupByType();
                cboGroup.DataBind();

                LoadGroupData();
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
        /// Handles the event selectedIndexChanged on cboGroupType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadInitialData();
                LoadGroupData();
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
        /// Handles the event TreeNodeCheckChanged on trvGroupData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TrvGroupData_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            try
            {
                CommonFunctions.ResetSortDirection(Page.ClientID, grvEmployees.ClientID);

                string groupDataCode = e.Node.Value;
                string groupDataDescription = e.Node.Text;
                string groupDataKey = GetCurrentGroupDataKey(groupDataCode);

                if (e.Node.Checked)
                {
                    AddGroupDataSelected(groupDataKey);
                    string[] companyPayroll = cboPayroll.SelectedValue.Split('-');
                    DOLE.HRIS.Services.CR.Business.EmployeesBll objEmployeesBll = new DOLE.HRIS.Services.CR.Business.EmployeesBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                    List<Employee> employees = objEmployeesBll.ListByUserCompanyPayrollGroup(
                        HttpContext.Current.User.Identity.Name,
                        companyPayroll[0], companyPayroll[1],
                        cboGroup.SelectedValue, groupDataCode);

                    AddEmployeesLoaded(employees, groupDataCode, groupDataDescription);
                }

                else
                {
                    RemoveGroupDataSelected(groupDataKey);
                    RemoveEmployeesLoaded(groupDataCode);
                }

                grvEmployees.DataSource = Session[sessionKeyEmployeesLoaded];
                grvEmployees.DataBind();
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
        /// Handles the event Click on btnPrevToPage1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnPrevToPage1_Click(object sender, EventArgs e)
        {
            try
            {
                pnlMainContent.Visible = true;
                pnlAbsenteeismContent.Visible = false;
                ClearForm();
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
        /// Handles the event Click on btnNextToPage2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnNextToPage2_Click(object sender, EventArgs e)
        {
            try
            {
                LoadInitialData();

                if (Session[sessionKeyEmployeesLoaded] == null)
                {
                    Session[sessionKeyEmployeesLoaded] = LoadEmptyEmployees();
                }

                if (Session[sessionKeyEmployeesByFilter] == null)
                {
                    Session[sessionKeyEmployeesByFilter] = LoadEmptyEmployees();
                }

                DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];
                if (pnlDirectSearch.Visible)
                {
                    searchSelected.Value = "true";
                    loadedEmployees = (DataTable)Session[sessionKeyEmployeesByFilter];
                    if (hdfEmployeeSelectedDirect.Value != "" && loadedEmployees.Rows.Count > 0)
                    {
                        loadedEmployees = loadedEmployees.AsEnumerable().Where(empl => empl.Field<string>("EmployeeCode") == hdfEmployeeSelectedDirect.Value).CopyToDataTable();
                        Session[sessionKeyEmployeesByFilter] = loadedEmployees;
                        grvEmployeesByFilter.DataSource = loadedEmployees;
                        grvEmployeesByFilter.DataBind();
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Error,
                            Convert.ToString(GetLocalResourceObject("msjSelectEmployees")));

                        return;
                    }
                }

                else
                {
                    searchSelected.Value = "false";
                    if (hdfEmployeesSelected.Value != "")
                    {
                        List<string> employeesSelected = new List<string>(hdfEmployeesSelected.Value.Split(','));
                        loadedEmployees = loadedEmployees.AsEnumerable().Where(empl => employeesSelected.Contains(empl.Field<string>("EmployeeCode"))).CopyToDataTable();
                    }

                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Error,
                            Convert.ToString(GetLocalResourceObject("msjSelectEmployees")));

                        return;
                    }
                }

                if (loadedEmployees.Rows.Count > 0)
                {

                    if (loadedEmployees.Rows.Count > 750)
                    {
                        ScriptManager.RegisterStartupScript((Control)sender
                            , sender.GetType()
                            , string.Format("BtnNextToPage2_Click{0}"
                                            , Guid.NewGuid())
                                            , "setTimeout(function () {{ ValidateCheckAll(); }}, 200);"
                                            , true);
                        MensajeriaHelper.MostrarMensaje(Page
                          , TipoMensaje.Error
                          , Convert.ToString(GetLocalResourceObject("msjMaxEmployeesSelected")));
                    }

                    else
                    {
                        if (chkMultiCause.Checked && loadedEmployees.Rows.Count >= 20)
                        {
                            MensajeriaHelper.MostrarMensaje(Page
                              , TipoMensaje.Error
                              , Convert.ToString(GetLocalResourceObject("msjMaxEmployeesMultipleTfl")));
                        }

                        else
                        {
                            lblCount.Text = string.Concat("(", loadedEmployees.Rows.Count.ToString(), ")");

                            pnlMainContent.Visible = false;
                            pnlAbsenteeismContent.Visible = true;

                            if (!hdfEmployeeSelectedDirect.Value.Equals(""))
                            {
                                hdfEmployeesSelected.Value = "";
                                hdfParticipantsSelected.Value = hdfEmployeeSelectedDirect.Value;
                            }
                            if (!hdfEmployeesSelected.Value.Equals(""))
                            {
                                hdfEmployeeSelectedDirect.Value = "";
                                hdfParticipantsSelected.Value = hdfEmployeesSelected.Value;
                            }

                            grvEmployeesSelected.DataSource = loadedEmployees;
                            grvEmployeesSelected.DataBind();

                            //let's check if related case must be an option
                            if (loadedEmployees.Rows.Count == 1)
                            {
                                loadedEmployees.Rows[0][1] = true;
                                grvEmployeesSelected.DataBind();

                                //Hora en la que inicia el dia.
                                int startDayHour = 7;
                                string timemer = (startDayHour > 12) ? "PM" : "AM";
                                string hourText = startDayHour < 10 ? string.Format("0{0}:00", Convert.ToInt32(startDayHour), timemer) : string.Format("{0}:00", Convert.ToInt32(startDayHour));
                                hdfEmployeeDirectSuspendTime.Value = hourText;

                                pnlMultipleTFL.Visible = false;
                                pnlNormalTfl.Visible = true;

                                if (cboCause.SelectedIndex == 0)
                                {
                                    if (initialData.StateCases[0].CDUCode != "-1")
                                    {
                                        initialData.StateCases.Insert(0, new Cdu() { CDUCode = "-1", CDUDescription = "" });
                                    }

                                    cboCaseState.DataSource = initialData.StateCases.Where(x => x.CDUCode == "OP").ToList<Cdu>();
                                    cboCaseState.DataBind();
                                }

                                LoadRelatedCase(loadedEmployees.Rows[0][2].ToString().Replace(" ", ""));
                            }

                            else
                            {
                                hdfEmployeeDirectSuspendTime.Value = "07:00";
                                if (initialData.StateCases[0].CDUCode != "-1")
                                {
                                    initialData.StateCases.Insert(0, new Cdu() { CDUCode = "-1", CDUDescription = "" });
                                }

                                List<Cdu> stateCasesWithoutReOpeningOption = new List<Cdu>(initialData.StateCases);
                                stateCasesWithoutReOpeningOption.RemoveAll(x => x.CDUCode == "RP");
                                stateCasesWithoutReOpeningOption.RemoveAll(x => x.CDUCode == "AP");
                                stateCasesWithoutReOpeningOption.RemoveAll(x => x.CDUCode == "PP");
                                stateCasesWithoutReOpeningOption.RemoveAll(x => x.CDUCode == "AB");
                                stateCasesWithoutReOpeningOption.RemoveAll(x => x.CDUCode == "CL");

                                cboCaseState.DataSource = stateCasesWithoutReOpeningOption;
                                cboCaseState.DataBind();

                                if (Session[sessionKeyReleatedCaseSelected] != null)
                                {
                                    Session[sessionKeyReleatedCaseSelected] = null;
                                }

                                string[] listCauseDelete = GetCausesWithCategoryAccident().Split(',');
                                List<AbsenteeismCauseEntity> causes = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
                                List<AbsenteeismCauseEntity> causesFilter = causes.Where(x => !listCauseDelete.Any(y => y.Contains(x.CauseCode))).ToList<AbsenteeismCauseEntity>();

                                Session["causesFiltered"] = null;
                                Session["causesFiltered"] = causesFilter;

                                //se quitan las causas que están configuradas ya que no son para registros múltiples
                                cboCause.DataSource = causesFilter;
                                cboCause.DataBind();

                                Session[sessionKeyReleatedCaseSelected] = LoadEmptyRelatedCase();

                                rptRelatedCase.DataSource = Session[sessionKeyReleatedCaseSelected];
                                rptRelatedCase.DataBind();

                                //esto es para mostrar el formulario de registros múltiple causa
                                pnlMultipleTFL.Visible = chkMultiCause.Checked;
                                pnlNormalTfl.Visible = !chkMultiCause.Checked;

                                if (chkMultiCause.Checked)
                                {
                                    rptMultipleTfl.DataSource = loadedEmployees;
                                    rptMultipleTfl.DataBind();
                                }
                                else
                                {
                                    rptMultipleTfl.DataSource = null;
                                    rptMultipleTfl.DataBind();
                                }

                            }
                        }
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , Convert.ToString(GetLocalResourceObject("msjSelectEmployees")));
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

            finally
            {
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFrombtnNextToPage2_Click{0}", Guid.NewGuid()), "setTimeout(function () {{ ActivateModalProgress(false); }}, 10);", true);
            }
        }

        public void FillData(DataTable absenteeimSelected)
        {
            LoadInitialData();
            cboCaseState.DataSource = initialData.StateCases;
            cboCaseState.DataBind();
            cboCaseState.SelectedValue = Convert.ToString(absenteeimSelected.Rows[0]["estado_caso"]);
            cboCaseState.Enabled = false;

            List<AbsenteeismCauseEntity> causes = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
            cboCause.SelectedValue = causes.Where(a => a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.CauseCodeAdamMapped == Convert.ToString(absenteeimSelected.Rows[0]["causa"]))).FirstOrDefault().CauseCode;
            cboCause.Enabled = false;

            List<Cdu> stateCases = new List<Cdu>();

            if (GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
            {
                //HACK: DIFERENCIA REGIONAL #8
                var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                if (DivisionSession == 1//División de Costa Rica
                    || DivisionSession == 11//Doce Costa Rica
                    || DivisionSession == 15//Dole Tropical Products
                    || DivisionSession == 16//Dole Shared Services
                    || DivisionSession == 5//División de Ecuador
                    || DivisionSession == 9//División de Perú
                    || DivisionSession == 4//División de Honduras
                    || DivisionSession == 14) //División de Guatemala
                {
                    divAccidentCase.Visible = true;

                }

                divState.Visible = true;
                divRelated.Visible = true;
            }

            else
            {
                divState.Visible = false;
                divRelated.Visible = false;
                divAccidentCase.Visible = false;
                txtDescription.Text = "";
                divCloseDate.Visible = false;
            }

            cboInterestGroups.SelectedValue = Convert.ToString(absenteeimSelected.Rows[0]["GrupoInteres_RSC"]);
            txtDescription.Text = Convert.ToString(absenteeimSelected.Rows[0]["Description"]);
            txtDetailedDescription.Text = Convert.ToString(absenteeimSelected.Rows[0]["descripcion_detallada"]);
            txtRelatedCase.Text = Convert.ToString(absenteeimSelected.Rows[0]["au_relacionado"]);

            var date = Convert.ToString(absenteeimSelected.Rows[0]["StartDate"]).Split('/')[1];
            date += "/" + Convert.ToString(absenteeimSelected.Rows[0]["StartDate"]).Split('/')[0];
            date += "/" + Convert.ToString(absenteeimSelected.Rows[0]["StartDate"]).Split('/')[2];

            dtpIncidentDate.Text = date;
            tpcIncidentDateTime.Text = Convert.ToDateTime(absenteeimSelected.Rows[0]["StartDate"]).ToShortTimeString();

            //Suspended
            var suspendedDate = Convert.ToString(absenteeimSelected.Rows[0]["fecha_suspende"]).Split('/')[1];
            suspendedDate += "/" + Convert.ToString(absenteeimSelected.Rows[0]["fecha_suspende"]).Split('/')[0];
            suspendedDate += "/" + Convert.ToString(absenteeimSelected.Rows[0]["fecha_suspende"]).Split('/')[2];
            dtpSuspendWorkDate.Text = suspendedDate;
            tpcSuspendWorkTime.Text = Convert.ToDateTime(absenteeimSelected.Rows[0]["fecha_suspende"]).ToShortTimeString();

            //EdnDate
            var endDate = Convert.ToString(absenteeimSelected.Rows[0]["fecha_final"]).Split('/')[1];
            endDate += "/" + Convert.ToString(absenteeimSelected.Rows[0]["fecha_final"]).Split('/')[0];
            endDate += "/" + Convert.ToString(absenteeimSelected.Rows[0]["fecha_final"]).Split('/')[2];
            dtpFinalDate.Text = endDate;

            if (Convert.ToString(absenteeimSelected.Rows[0]["unidad_tiempo"]) == "0")
            {
                cboTimeUnit.SetSelectedValueDole("Days");
            }

            else
            {
                cboTimeUnit.SetSelectedValueDole("Hours");
            }

            txtFinalHours.Text = Convert.ToString(absenteeimSelected.Rows[0]["tiempo"]);
            cboUnsafeActs.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["actos_inseguros"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["actos_inseguros"]);
            cboMaterialAgents.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["agentes_materiales"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["agentes_materiales"]);
            cboUnsafeConditions.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["condiciones_inseguras"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["condiciones_inseguras"]);
            cboEstablishments.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["lugares"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["lugares"]);
            cboDayFactor.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Factor_de_Jornada"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Factor_de_Jornada"]);
            cboPersonalFactor.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Factores_Personales"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Factores_Personales"]);
            cboJourneys.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Jornadas"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Jornadas"]);
            cboLaborMade.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Labores"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Labores"]);
            cboBodyParts.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Partes_Cuerpo"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Partes_Cuerpo"]);
            cboAccidentType.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Tipos_Accidente"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Tipos_Accidente"]);
            cboDiseaseType.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeimSelected.Rows[0]["Tipos_Enfermedad"])) ? "-1" : Convert.ToString(absenteeimSelected.Rows[0]["Tipos_Enfermedad"]);
        }

        /// <summary>
        /// Handles the event Click on btnPrevToPage2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnPrevToPage2_ServerClick(object sender, EventArgs e)
        {
            try
            {
                pnlDetailsContent.Visible = false;
                pnlAbsenteeismContent.Visible = true;
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
        /// Handles the event Click on btnNextToPage3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnNextToPage3_ServerClick(object sender, EventArgs e)
        {
            try
            {
                pnlAbsenteeismContent.Visible = false;
                pnlDetailsContent.Visible = true;
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
        /// Handles the event Click on btnNextToPage4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnNextToPage4_ServerClick(object sender, EventArgs e)
        {
            try
            {
                pnlDetailsContent.Visible = false;
                pnlDocumentsContent.Visible = true;
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
        /// Handles the event Click on btnNextToPage4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnPrevToPage3_ServerClick(object sender, EventArgs e)
        {
            try
            {
                pnlDocumentsContent.Visible = false;
                pnlDetailsContent.Visible = true;
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
        /// Handles the event SelectedIndexChanged on cboDocuments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session[sessionKeyDocumentsSaved] == null)
                {
                    Session[sessionKeyDocumentsSaved] = new Dictionary<string, Document>();
                }

                Dictionary<string, Document> documents = (Dictionary<string, Document>)Session[sessionKeyDocumentsSaved];
                string documentCode = cboDocuments.SelectedValue;

                if (documents.ContainsKey(documentCode))
                {
                    txtDocumentDescription.Text = documents[documentCode].DocumentValue;
                }

                else
                {
                    txtDocumentDescription.Text = string.Empty;
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
        /// Handles the event Click on btnSaveDocumentValue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSaveDocumentValue_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (Session[sessionKeyDocumentsSaved] == null)
                {
                    Session[sessionKeyDocumentsSaved] = new Dictionary<string, Document>();
                }

                Dictionary<string, Document> documents = (Dictionary<string, Document>)Session[sessionKeyDocumentsSaved];
                string documentCode = cboDocuments.SelectedValue;
                string documentValue = txtDocumentDescription.Text;
                Document doc = new Document(documentCode, string.Empty, documentValue);

                if (documents.ContainsKey(documentCode))
                {
                    documents[documentCode] = doc;
                }

                else
                {
                    documents.Add(documentCode, doc);
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
        /// Handles the event Click on btnSave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (chkMultiCause.Checked && pnlMultipleTFL.Visible)
                {
                    SaveMultipleTfl();
                }

                else
                {
                    SaveTFLSigleCause();
                }
            }

            catch (Exception ex)
            {

            }

            finally
            {
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFrombtnSave_ServerClick{0}", Guid.NewGuid()), "setTimeout(function () {{ ActivateModalProgress(false); }}, 10);", true);
            }
        }

        /// <summary>
        /// Save records to single Cause
        /// </summary>
        private void SaveTFLSigleCause()
        {
            try
            {
                LoadInitialData();

                if (Session[sessionKeyEmployeesByFilter] == null)
                {
                    Session[sessionKeyEmployeesByFilter] = LoadEmptyEmployees();
                }

                if (Session[sessionKeyEmployeesLoaded] == null)
                {
                    Session[sessionKeyEmployeesLoaded] = LoadEmptyEmployees();
                }

                if (Session[sessionKeyDocumentsSaved] == null)
                {
                    Session[sessionKeyDocumentsSaved] = new Dictionary<string, Document>();
                }

                bool savedWithDetails = !pnlAbsenteeismContent.Visible;

                //Validations
                bool errors = false;
                string errorsMessages = string.Empty;

                DateTime dateTest = DateTime.Now;

                DateTime? incidentDateTest = null;
                DateTime? suspendWorkTest = null;
                DateTime? finalDateTest = null;
                int? naturalDaysTest = null;

                if (string.IsNullOrWhiteSpace(cboInterestGroups.SelectedValue) || cboInterestGroups.SelectedValue == "-1")
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjInterestGroupValidation")));
                }

                if (string.IsNullOrWhiteSpace(cboCause.SelectedValue))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjCauseValidation")));
                }

                if (string.IsNullOrWhiteSpace(cboCaseState.SelectedValue))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjCaseStateValidation")));
                }

                if ((string.Equals(cboCaseState.SelectedValue, "RP", StringComparison.CurrentCultureIgnoreCase) || string.Equals(cboCaseState.SelectedValue, "AP", StringComparison.CurrentCultureIgnoreCase)) && (string.IsNullOrWhiteSpace(txtRelatedCase.Text) || int.TryParse(txtRelatedCase.Text, out int integerTest) == false))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjRelatedCaseValidation")));
                }

                //HACK: DIFERENCIA REGIONAL #9
                var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                if (DivisionSession == 1//División de Costa Rica
                    || DivisionSession == 11//Doce Costa Rica
                    || DivisionSession == 15//Dole Tropical Products
                    || DivisionSession == 16//Dole Shared Services
                    || DivisionSession == 5//División de Ecuador
                    || DivisionSession == 9//División de Perú
                    || DivisionSession == 4//División de Honduras
                    || DivisionSession == 14) //División de Guatemala
                {
                    if (chkCloseDate.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(txtDescription.Text) ||
                        txtDescription.Text.Length > 250)
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDescriptionValidation")));
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(txtDetailedDescription.Text) ||
                    txtDetailedDescription.Text.Length > 2000)
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDetailedDescriptionValidation")));
                }

                if (!GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
                {
                    if (cboTimeUnit.SelectedValue == "Days")
                    {
                        tpcSuspendWorkTime.Text = "07:00";
                    }
                }

                else
                {
                    if (string.IsNullOrWhiteSpace(dtpIncidentDate.Text) || string.IsNullOrWhiteSpace(tpcIncidentDateTime.Text) ||
                    DateTime.TryParseExact(string.Format("{0} {1}", dtpIncidentDate.Text, tpcIncidentDateTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false)
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjIncidentDateValidation")));
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjIncidentTimeValidation")));
                    }

                    else
                    {
                        incidentDateTest = DateTime.ParseExact(string.Format("{0} {1}", dtpIncidentDate.Text, tpcIncidentDateTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                }
                if (string.IsNullOrWhiteSpace(dtpSuspendWorkDate.Text) || string.IsNullOrWhiteSpace(tpcSuspendWorkTime.Text) ||
                DateTime.TryParseExact(string.Format("{0} {1}", dtpSuspendWorkDate.Text, tpcSuspendWorkTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false)
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkValidation")));
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkTimeValidation")));
                }
                else
                {
                    if (DivisionSession != 5 && dateTest.DayOfWeek == DayOfWeek.Sunday)
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkValidation")));
                    }
                    if (!errors)
                    {
                        suspendWorkTest = DateTime.ParseExact(string.Format("{0} {1}", dtpSuspendWorkDate.Text, tpcSuspendWorkTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                }
                if (string.IsNullOrWhiteSpace(dtpFinalDate.Text) ||
                    DateTime.TryParseExact(dtpFinalDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false)
                {
                    //CAUTION: dtpFinalDate value is not been sent to ADAM, it is used just to calculate natural days but is validated in order to make natural days validation completed
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalDateValidation")));
                }

                else
                {
                    finalDateTest = DateTime.ParseExact(string.Format("{0} 23:59", dtpFinalDate.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }

                if (string.IsNullOrWhiteSpace(cboTimeUnit.SelectedValue))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjTimeUnitValidation")));
                }

                if (cboTimeUnit.SelectedValue == "Hours" && string.IsNullOrWhiteSpace(txtNaturalDays.Text))
                {
                    txtNaturalDays.Text = "1";
                }

                if (string.IsNullOrWhiteSpace(txtNaturalDays.Text) || int.TryParse(txtNaturalDays.Text, out integerTest) == false || integerTest <= 0 || integerTest > 10000)
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjNaturalDaysValidation")));
                }

                else
                {
                    naturalDaysTest = int.Parse(txtNaturalDays.Text);
                }

                if (!string.IsNullOrEmpty(txtFinalHours.Text) && (txtFinalHours.Text.Contains(".") || txtFinalHours.Text.Contains(",")))
                {
                    txtFinalHours.Text = txtFinalHours.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }

                if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) &&
                        (string.IsNullOrWhiteSpace(txtFinalHours.Text) ||
                        decimal.TryParse(txtFinalHours.Text, out decimal decimalTest) == false ||
                        !decimalTest.HasThisManyDecimalPlacesOrLess(3) ||
                        decimalTest <= 0 || decimalTest > 8))

                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalHoursValidation")));
                }

                if (incidentDateTest.HasValue && suspendWorkTest.HasValue)
                {
                    if (DateTime.Compare(incidentDateTest.Value, suspendWorkTest.Value) > 0)
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjIncidentStartAndSuspendedDate")));
                    }
                }

                if (suspendWorkTest.HasValue && finalDateTest.HasValue)
                {
                    if (DateTime.Compare(suspendWorkTest.Value, finalDateTest.Value) > 0)
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkAndFinalDate")));
                    }
                }

                if (suspendWorkTest.HasValue)
                {
                    if (DateTime.Compare(suspendWorkTest.Value, DateTime.Now.AddDays(30)) > 0)
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkCannotFutureDistant")));
                    }
                }

                if (GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
                {
                    if (incidentDateTest.HasValue)
                    {
                        if (DateTime.Compare(incidentDateTest.Value, DateTime.Now.Date.AddDays(1)) > 0)
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjAccidentDateCannotFutureAccident")));
                        }
                    }
                }

                AbsenteeismCauseEntity selectedCause = absenteeismCauses.FirstOrDefault(a => string.Equals(a.CauseCode, cboCause.SelectedValue, StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrWhiteSpace(cboCause.SelectedValue) && selectedCause != null)
                {
                    List<AbsenteeismCauseEntity> causesNotConfigured = absenteeismCauses.Where(a => a.CauseCode == cboCause.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours == false && cbd.Days == false)).ToList();
                    if (causesNotConfigured.Count > 0)
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjCauseNotConfigured")));
                    }

                    //HACK: DIFERENCIA REGIONAL #11
                    if (DivisionSession == 5) // ECU
                    {
                        List<string> disabilities = new List<string> { "EAT" };
                        if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) &&
                            disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                        }
                    }

                    //HACK: DIFERENCIA REGIONAL #11
                    if (DivisionSession == 4) // HND
                    {
                        List<string> disabilities = new List<string> { "IN", "INH", "ING", "ACTY" };
                        if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) &&
                            disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                        }
                    }

                    //HACK: DIFERENCIA REGIONAL #11
                    if (DivisionSession == 1) // CRC
                    {
                        List<string> disabilities = new List<string> { "IL", "IN", "IN3", "INCC", "INC2", "IPMT", "TACC" };
                        if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) &&
                            disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                        }

                        List<string> disabilities3Days = new List<string> { "INCC" };
                        if (disabilities3Days.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped) &&
                            naturalDaysTest.HasValue &&
                            (naturalDaysTest <= 0 || naturalDaysTest > 3))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilities3Days")));
                        }

                        List<string> listNextDayAfterMidDay = new List<string> { "IL", "IN", "IN3", "INCC", "INC2", "IPMT", "TACC" };
                        if (suspendWorkTest.HasValue
                             && listNextDayAfterMidDay.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                        {
                            if (suspendWorkTest.Value.Hour >= 12 && (finalDateTest.Value.Date == suspendWorkTest.Value.Date))
                            {
                                errors = true;
                                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalDateCannotSameSuspended")));
                            }
                        }
                    }
                }

                DataTable loadedEmployees = new DataTable();
                if (searchSelected.Value == "true")
                {
                    loadedEmployees = (DataTable)Session[sessionKeyEmployeesByFilter];
                    loadedEmployees = loadedEmployees.AsEnumerable().Where(empl => empl.Field<string>("EmployeeCode") == hdfEmployeeSelectedDirect.Value).CopyToDataTable();
                }

                else
                {
                    loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];
                    List<string> employeesSelected = new List<string>(hdfEmployeesSelected.Value.Split(','));
                    loadedEmployees = loadedEmployees.AsEnumerable().Where(empl => employeesSelected.Contains(empl.Field<string>("EmployeeCode"))).CopyToDataTable();
                }

                if (loadedEmployees.Rows.Count <= 0)
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSelectEmployees")));
                }

                if (!errors)
                {
                    DataTable loadedParticipants = LoadEmptyEmployees();
                    if (hdfParticipantsSelected.Value != "")
                    {
                        List<string> participantselected = new List<string>(hdfParticipantsSelected.Value.Split(','));
                        loadedParticipants = loadedEmployees.AsEnumerable().Where(empl => participantselected.Contains(empl.Field<string>("EmployeeCode"))).CopyToDataTable();

                    }

                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjSelectEmployees")));
                        return;
                    }

                    List<KeyValuePair<Company, Employee>> employeesAbsenteeism = new List<KeyValuePair<Company, Employee>>();
                    loadedParticipants.AsEnumerable().ToList().ForEach(x => employeesAbsenteeism.Add(new KeyValuePair<Company, Employee>(new Company(x.Field<string>("CompanyCode")), new Employee(x.Field<string>("EmployeeCode")))));

                    List<Document> documents =
                        savedWithDetails ?
                            ((Dictionary<string, Document>)Session[sessionKeyDocumentsSaved]).Values.ToList<Document>() :
                            new List<Document>();

                    string selectedUnsafeActs =
                        cboUnsafeActs.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboUnsafeActs.SelectedValue;

                    string selectedMaterialAgents =
                        cboMaterialAgents.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboMaterialAgents.SelectedValue;

                    string selectedUnsafeConditions =
                        cboUnsafeConditions.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboUnsafeConditions.SelectedValue;

                    string selectedEstablishments =
                        cboEstablishments.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboEstablishments.SelectedValue;

                    string selectedDayFactor =
                        cboDayFactor.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboDayFactor.SelectedValue;

                    string selectedPersonalFactor =
                        cboPersonalFactor.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboPersonalFactor.SelectedValue;

                    string selectedJourneys =
                        cboJourneys.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboJourneys.SelectedValue;

                    string selectedLaborMade =
                        cboLaborMade.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboLaborMade.SelectedValue;

                    string selectedBodyParts =
                        cboBodyParts.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboBodyParts.SelectedValue;

                    string selectedAccidentType =
                        cboAccidentType.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboAccidentType.SelectedValue;

                    string selectedDiseaseType =
                        cboDiseaseType.SelectedIndex == 0 || !savedWithDetails ? null :
                            cboDiseaseType.SelectedValue;

                    DateTime suspendWork = DateTime.ParseExact(string.Format("{0} {1}", dtpSuspendWorkDate.Text, tpcSuspendWorkTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime incidentStart = DateTime.MinValue;
                    if (incidentDateTest != null)
                    {
                        incidentStart = DateTime.ParseExact(string.Format("{0} {1}", dtpIncidentDate.Text, tpcIncidentDateTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }

                    AbsenteeismCase absenteeismCase = new AbsenteeismCase(
                        employeesAbsenteeism, 0,
                        txtDescription.Text.Trim().ToUpper(),
                        txtDetailedDescription.Text.Trim().ToUpper(),
                        incidentStart == DateTime.MinValue ? suspendWork : incidentStart, // suspendWork, //DateTime.ParseExact(String.Format("{0} {1}", dtpIncidentDate.Text, tpcIncidentTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        string.IsNullOrWhiteSpace(txtNaturalDays.Text) ? 0 : Convert.ToInt32(txtNaturalDays.Text),
                        cboTimeUnit.SelectedValue == "Days" ? 0 : 1,
                        null,
                        0,
                        absenteeismCauses.First(a => a.CauseCode == cboCause.SelectedValue).AbsenteeismCausesByDivision[0].CauseCodeAdamMapped,
                        cboCaseState.SelectedValue,
                        finalDateTest,
                        0, //Convert.ToInt32(txtBusinessDays.Text),
                        string.IsNullOrWhiteSpace(txtFinalHours.Text) ? 0 : Convert.ToDecimal(txtFinalHours.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)),
                        suspendWork, //DateTime.ParseExact(dtpSuspendWorkDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        cboInterestGroups.SelectedValue,
                        string.IsNullOrWhiteSpace(txtRelatedCase.Text) ? 0 : Convert.ToInt32(txtRelatedCase.Text),
                        1,
                        UserHelper.GetCurrentFullUserName,
                        selectedUnsafeActs, selectedMaterialAgents, selectedUnsafeConditions, selectedEstablishments, selectedDayFactor, selectedPersonalFactor, selectedJourneys, selectedLaborMade, selectedBodyParts, selectedAccidentType, selectedDiseaseType,
                        documents,
                        1,//randomico
                        chkCloseDate.Checked); //Close Absenteeism (Dar de alta)

                    DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                    List<AbsenteeismID> absenteeismID = objAbsenteeismCaseBll.Save(absenteeismCase, Session["culture"] == null ? "es-CR" : Session["culture"].ToString());

                    pnlAbsenteeismContent.Visible = false;
                    pnlDocumentsContent.Visible = false;
                    pnlResults.Visible = true;

                    var cause = absenteeismCauses.First(a => a.CauseCode == cboCause.SelectedValue).CauseName;

                    loadedParticipants.Columns.Add("CauseName", typeof(string));

                    loadedParticipants.Columns.Add("CaseID", typeof(string));

                    loadedParticipants.AsEnumerable().ToList<DataRow>().ForEach(r =>
                    {
                        if (absenteeismID.Exists(a => a.EmployeeID == Convert.ToString(r["EmployeeCode"])))
                        {
                            r["CaseID"] = absenteeismID.First(a => a.EmployeeID == Convert.ToString(r["EmployeeCode"])).AbsenteeismId;
                        }
                    });

                    loadedParticipants.AsEnumerable().ToList<DataRow>().ForEach(r =>
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(r["CaseID"])))
                        {
                            r["CaseID"] = Convert.ToString(GetLocalResourceObject("msjNoCase"));
                        }
                    });

                    loadedParticipants.AsEnumerable().ToList<DataRow>().ForEach(r =>
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(r["CauseName"])))
                        {
                            r["CauseName"] = absenteeismCauses.First(a => a.CauseCode == cboCause.SelectedValue).CauseName;
                        }
                    });

                    rptResults.DataSource = loadedParticipants;
                    rptResults.DataBind();
                }

                else
                {
                    errorsMessages = string.Format("{0}\\n{1}", Convert.ToString(GetLocalResourceObject("verifyErrors")), errorsMessages);
                    errorsMessages = errorsMessages.Replace("\\n", "<br/>");
                    errorsMessages = errorsMessages.Replace("\n", "");
                    errorsMessages = errorsMessages.Replace("\r", "");

                    MensajeriaHelper.MostrarMensaje(btnSave
                      , TipoMensaje.Error
                      , Convert.ToString(errorsMessages));

                    dtpFinalDate.Text = "";
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException
                    || ex is ServiceException)
                {
                    MensajeriaHelper.MostrarMensaje(btnSave
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(btnSave
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }

        }

        /// <summary>
        /// Load the group data tree
        /// </summary>
        public bool LoadGroupData()
        {
            if (cboGroup.SelectedValue != "")
            {
                if (initialData.GroupData.Count == 0)
                {
                    initialData = (AbsenteeismsInitialData)Session[sessionKeyAbsenteeismInitialDataResults];

                    DOLE.HRIS.Services.CR.Business.GroupsBll objGroupsBll = new DOLE.HRIS.Services.CR.Business.GroupsBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                    initialData.GroupData = objGroupsBll.ListGroupData();
                    Session[sessionKeyAbsenteeismInitialDataResults] = initialData;
                }
            }

            if (initialData.GroupData.Count > 0)
            {
                if (!string.IsNullOrEmpty(cboPayroll.SelectedValue)
                && !string.IsNullOrEmpty(cboGroupType.SelectedValue)
                && !string.IsNullOrEmpty(cboGroup.SelectedValue))
                {
                    string[] companyPayroll = cboPayroll.SelectedValue.Split('-');

                    List<GroupData> selectedGroupData = initialData.GroupData.Where(x => x.CompanyCode == companyPayroll[0] && x.PayrollclassCode == companyPayroll[1]
                        && x.GroupCode == cboGroup.SelectedValue).OrderBy(x => x.GroupDataDescription).ToList();

                    trvGroupData.Nodes.Clear();

                    string groupDataKey = string.Format("{0}/{1}/{2}/{3}",
                        companyPayroll[0], companyPayroll[1], //cboCompany.SelectedValue, cboPayrollClass.SelectedValue, 
                        cboGroupType.SelectedValue, cboGroup.SelectedValue);

                    //Add the head node from the group selected
                    TreeNode headnode = new TreeNode
                    {
                        Text = cboGroup.SelectedIndex >= 0 ? cboGroup.Items[cboGroup.SelectedIndex].Text : Convert.ToString(GetLocalResourceObject("NoGroupSelected")),
                        SelectAction = TreeNodeSelectAction.Expand
                    };

                    trvGroupData.Nodes.Add(headnode);

                    if (selectedGroupData.Count > 0)
                    {
                        //Loop for binding Parent Values
                        foreach (GroupData data in selectedGroupData)
                        {
                            TreeNode child = new TreeNode
                            {
                                Text = data.GroupDataDescription,
                                Value = data.GroupDataCode,
                                Checked = IsGroupDataSelected(GetCurrentGroupDataKey(data.GroupDataCode)),
                                SelectAction = TreeNodeSelectAction.Expand
                            };

                            headnode.ChildNodes.Add(child);
                        }

                        trvGroupData.ShowCheckBoxes = TreeNodeTypes.Leaf;
                    }

                    else
                    {
                        TreeNode child = new TreeNode
                        {
                            Text = Convert.ToString(GetLocalResourceObject("NoDataFound")),
                            Value = "null",
                            SelectAction = TreeNodeSelectAction.Expand
                        };

                        headnode.ChildNodes.Add(child);

                        trvGroupData.ShowCheckBoxes = TreeNodeTypes.None;
                    }

                    trvGroupData.DataBind();
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the current group data key by the s given in page
        /// </summary>
        /// <param name="groupDataCode">Group data</param>
        /// <returns>The current group data key by the s given in page</returns>
        public string GetCurrentGroupDataKey(string groupDataCode)
        {
            string[] companyPayroll = cboPayroll.SelectedValue.Split('-');

            return GetGroupDataKey(
                companyPayroll[0], companyPayroll[1],//cboCompany.SelectedValue, cboPayrollClass.SelectedValue, 
                cboGroupType.SelectedValue, cboGroup.SelectedValue, groupDataCode);
        }

        /// <summary>
        /// Get the group data key compose by the company, payroll, grouptype, group and groupdata
        /// </summary>
        /// <param name="companyCode">Company</param>
        /// <param name="payrollCode">Payroll</param>
        /// <param name="groupType">Group type</param>
        /// <param name="groupCode">Group</param>
        /// <param name="groupDataCode">Group data</param>
        /// <returns>Tthe group data key</returns>
        public string GetGroupDataKey(string companyCode, string payrollCode, string groupType, string groupCode, string groupDataCode)
        {
            return string.Format("{0}/{1}/{2}/{3}/{4}", companyCode, payrollCode, groupType, groupCode, groupDataCode);
        }

        /// <summary>
        /// Return true if the group data represented by the groupDataKey
        /// is selected. False otherwise.
        /// </summary>
        /// <param name="groupDataKey">Group data code</param>
        /// <returns>True if the group data represented by the groupDataKey is selected. False otherwise</returns>
        public bool IsGroupDataSelected(string groupDataKey)
        {
            if (Session[sessionKeyGroupDataSelected] == null)
            {
                Session[sessionKeyGroupDataSelected] = new List<string>();
            }

            List<string> groupDataSelected = (List<string>)Session[sessionKeyGroupDataSelected];
            return groupDataSelected.Contains(groupDataKey);
        }

        /// <summary>
        /// Add the group data represented by the groupDataKey as selected.
        /// </summary>
        /// <param name="groupDataKey">Group data code</param>        
        public void AddGroupDataSelected(string groupDataKey)
        {
            if (Session[sessionKeyGroupDataSelected] == null)
            {
                Session[sessionKeyGroupDataSelected] = new List<string>();
            }

            List<string> groupDataSelected = (List<string>)Session[sessionKeyGroupDataSelected];
            if (!groupDataSelected.Contains(groupDataKey))
            {
                groupDataSelected.Add(groupDataKey);
            }
        }

        /// <summary>
        /// Remove the group data represented by the groupDataKey as selected.
        /// </summary>
        /// <param name="groupDataKey">Group data code</param>        
        public void RemoveGroupDataSelected(string groupDataKey)
        {
            if (Session[sessionKeyGroupDataSelected] == null)
            {
                Session[sessionKeyGroupDataSelected] = new List<string>();
            }

            List<string> groupDataSelected = (List<string>)Session[sessionKeyGroupDataSelected];
            if (groupDataSelected.Contains(groupDataKey))
            {
                groupDataSelected.Remove(groupDataKey);
            }
        }

        /// <summary>
        /// Add theemployees represented by the groupDataKey as selected.
        /// </summary>
        /// <param name="groupDataKey">Group data code</param>        
        public void AddEmployeesLoaded(List<Employee> employees, string groupDataCode, string groupDataDescripcion)
        {
            if (Session[sessionKeyEmployeesLoaded] == null)
            {
                Session[sessionKeyEmployeesLoaded] = LoadEmptyEmployees();
            }
            groupDataCode = groupDataCode.Replace("/", "\\");
            string[] companyPayroll = cboPayroll.SelectedValue.Split('-');

            DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];
            int i = 0;

            foreach (Employee employee in employees)
            {
                DataRow row = loadedEmployees.NewRow();
                row.ItemArray = new object[] {
                    string.Format("{0}/{1}/{2}/{3}/{4}",
                        companyPayroll[0], companyPayroll[1],
                        cboGroup.SelectedValue, groupDataCode, employee.ID),
                    false,
                    employee.ID,
                    employee.Name,
                    companyPayroll[0],
                    string.Format("{0}-{1}",
                        cboGroup.SelectedValue, groupDataCode),
                    groupDataDescripcion,
                    cboPayroll.SelectedValue };

                DataRow[] employeesLoad = loadedEmployees.Select("EmployeeCode ='" + employee.ID + "'");
                if (employeesLoad.Length == 0)
                {
                    loadedEmployees.Rows.InsertAt(row, i++);
                }
            }
        }

        /// <summary>
        /// Remove the employees represented by the groupDataKey as selected.
        /// </summary>
        /// <param name="groupDataKey">Group data code</param>        
        public void RemoveEmployeesLoaded(string groupDataCode)
        {
            if (Session[sessionKeyEmployeesLoaded] == null)
            {
                Session[sessionKeyEmployeesLoaded] = LoadEmptyEmployees();
            }

            DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];
            loadedEmployees.AsEnumerable()
                .Where(x =>
                    x.Field<string>("GroupCode-GroupDataCode") == string.Format("{0}-{1}", cboGroup.SelectedValue, groupDataCode)
                    && x.Field<string>("Company-PayrollClass") == cboPayroll.SelectedValue). //String.Format("{0}-{1}", cboCompany.SelectedValue, cboPayrollClass.SelectedValue)).
                ToList().ForEach(x =>
                    loadedEmployees.Rows.Remove(x));
        }

        #endregion


        protected void CboCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsCaseEnable();
            EnableDisableUnitTime();
            LoadInterestGroupByCause();
        }

        private void LoadInterestGroupByCause()
        {
            DataTable procesedCdUs = LoadEmptyCdUs();
            List<Cdu> cdus = new List<Cdu>();

            if (Session[sessionKeyInterestGroupAbsenteeism] == null)
            {
                Session[sessionKeyInterestGroupAbsenteeism] = ObjInterestGroupBll.ListAll();
            }

            int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            string geographicDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

            List<AbsenteeismInterestGroupEntity> listInterestGroups = (List<AbsenteeismInterestGroupEntity>)Session[sessionKeyInterestGroupAbsenteeism];
            listInterestGroups = listInterestGroups.Where(x => x.GeographicDivisionCode == geographicDivision).ToList();

            absenteeismCauses = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
            List<AbsenteeismCauseEntity> listCause = absenteeismCauses.Where(c => c.CauseCode == cboCause.SelectedValue && c.AbsenteeismCausesByDivision != null && c.AbsenteeismCausesByDivision.Exists(cbd => cbd.DivisionCode == divisionCode)).ToList<AbsenteeismCauseEntity>();
            string[] lstInteresGroupCodeByCause = listCause[0].AbsenteeismCausesByDivision[0].InterestGroupCodes.Split(',');

            if (lstInteresGroupCodeByCause[0] != "")
            {
                if (lstInteresGroupCodeByCause.Count<string>() > 1)
                {
                    Cdu cduInterestEmpty = new Cdu("-1", string.Empty);
                    cdus.Add(cduInterestEmpty);
                }

                foreach (string splitData in lstInteresGroupCodeByCause)
                {
                    List<AbsenteeismInterestGroupEntity> listInterestGroupsCustom = listInterestGroups.Where(x => x.Code == splitData).ToList();
                    Cdu cduInterest = new Cdu(listInterestGroupsCustom[0].Code, listInterestGroupsCustom[0].Description);
                    cdus.Add(cduInterest);
                }
            }

            cboInterestGroups.DataValueField = "CDUCode";
            cboInterestGroups.DataTextField = "CDUDescription";
            cboInterestGroups.DataSource = cdus;
            cboInterestGroups.DataBind();
        }

        /// <summary>
        /// Show or not INS Case fields by selected cause
        /// </summary>
        private void InsCaseEnable()
        {
            initialData = (AbsenteeismsInitialData)Session[sessionKeyAbsenteeismInitialDataResults];
            List<Cdu> stateCases = new List<Cdu>();

            if (GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
            {
                stateCases = initialData.StateCases.Where(x => x.CDUCode == "OP" || x.CDUCode == "AP" || x.CDUCode == "RP").ToList<Cdu>();

                //HACK: DIFERENCIA REGIONAL #15
                var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                if (DivisionSession == 1//División de Costa Rica
                    || DivisionSession == 11//Doce Costa Rica
                    || DivisionSession == 15//Dole Tropical Products
                    || DivisionSession == 16//Dole Shared Services
                    || DivisionSession == 5//División de Ecuador
                    || DivisionSession == 9//División de Perú
                    || DivisionSession == 4//División de Honduras
                    || DivisionSession == 14) //División de Guatemala
                {
                    divAccidentCase.Visible = true;
                }

                divCloseDate.Visible = true;
                divState.Visible = true;
                divRelated.Visible = true;
            }

            else
            {
                stateCases = initialData.StateCases.Where(x => x.CDUCode == "OP").ToList<Cdu>();
                divAccidentCase.Visible = false;
                divState.Visible = false;
                divRelated.Visible = false;
                txtDescription.Text = "";
                divCloseDate.Visible = false;
            }

            DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];
            if (searchSelected.Value == "false") //búsqueda avanzada, entonces puede elegir más de un empleado
            {
                List<string> employeesSelected = new List<string>(hdfEmployeesSelected.Value.Split(','));

                if (employeesSelected.Count > 2)
                {
                    stateCases.RemoveAll(x => x.CDUCode == "-1");
                    stateCases.RemoveAll(x => x.CDUCode == "RP");
                    stateCases.RemoveAll(x => x.CDUCode == "AP");
                    stateCases.RemoveAll(x => x.CDUCode == "PP");
                    stateCases.RemoveAll(x => x.CDUCode == "AB");
                    stateCases.RemoveAll(x => x.CDUCode == "CL");
                }
            }

            cboCaseState.Enabled = true;
            cboCaseState.DataValueField = "CDUCode";
            cboCaseState.DataTextField = "CDUDescription";
            cboCaseState.DataSource = stateCases;
            cboCaseState.DataBind();
        }

        /// <summary>
        /// Set values from Time Unit by selected cause
        /// </summary>
        private bool EnableDisableUnitTime()
        {
            absenteeismCauses = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];

            //Time unit
            if (cboTimeUnit.Items.Count > 0)
            {
                cboTimeUnit.Items.Clear();
            }

            List<AbsenteeismCauseEntity> causes2Forms = absenteeismCauses.Where(a => a.CauseCode == cboCause.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours && cbd.Days)).ToList();

            if (causes2Forms.Count > 0)
            {
                //Time unit
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                cboTimeUnit.ClearSelection();
                return true;
            }

            List<AbsenteeismCauseEntity> causesOnlyHours = absenteeismCauses.Where(a => a.CauseCode == cboCause.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours)).ToList();
            if (causesOnlyHours.Count > 0)
            {
                //Time unit
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                cboTimeUnit.ClearSelection();
            }

            else
            {
                //Time unit
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                cboTimeUnit.ClearSelection();
            }

            return true;
        }

        private void LoadRelatedCase(string idEmployee)
        {
            if (Session[sessionKeyReleatedCaseSelected] != null)
            {
                Session[sessionKeyReleatedCaseSelected] = null;
            }

            Session[sessionKeyReleatedCaseSelected] = LoadEmptyRelatedCase();

            DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
            List<AbsenteeismCase> absenteeisms = objAbsenteeismCaseBll.GetRelatedAbsenteeimsByEmployee(idEmployee, HttpContext.Current.User.Identity.Name);

            AddAbsenteeismsRelatedCaseLoaded(absenteeisms);

            rptRelatedCase.DataSource = Session[sessionKeyReleatedCaseSelected];
            rptRelatedCase.DataBind();
        }

        protected void BtnAcceptRelatedCase_ServerClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptRelatedCase.Items)
            {
                CheckBox chbEmployee = (CheckBox)item.FindControl("chbEmployeeRelatedCase");
                HiddenField hdfUniqueKey = (HiddenField)item.FindControl("hdfUniqueKeyRelated");
                if (chbEmployee.Checked)
                {
                    DataTable relatedCases = (DataTable)Session[sessionKeyReleatedCaseSelected];
                    DataTable relatedCase = relatedCases.AsEnumerable().Where(x => x.Field<string>("AbsteeismId") == hdfUniqueKey.Value).CopyToDataTable();
                    if (relatedCase.Rows.Count == 1)
                    {
                        txtDescription.Text = Convert.ToString(relatedCase.Rows[0][3]);
                        txtRelatedCase.Text = Convert.ToString(relatedCase.Rows[0][1]);
                        var incidentDate = Convert.ToDateTime(relatedCase.Rows[0][2]);

                        var date = incidentDate.Month < 10 ? "0" + incidentDate.Month.ToString() : incidentDate.Month.ToString();
                        date += incidentDate.Day < 10 ? "/0" + incidentDate.Day.ToString() : "/" + incidentDate.Day.ToString();
                        date += "/" + incidentDate.Year.ToString();

                        dtpIncidentDate.Text = date;

                        var time = incidentDate.Hour < 10 ? "0" + incidentDate.Hour.ToString() : incidentDate.Hour.ToString();
                        time += ":";
                        time += incidentDate.Minute < 10 ? "0" + incidentDate.Minute.ToString() : incidentDate.Minute.ToString();
                        tpcIncidentDateTime.Text = time;
                    }

                    break;
                }
            }
        }

        protected void BtnReturnToMain_ServerClick(object sender, EventArgs e)
        {
            grvEmployeesByFilter.Visible = false;
            cboPayroll.SelectedIndex = 0;
            cboGroupType.SelectedIndex = 0;
            cboGroup.SelectedIndex = 0;
            cboGroup.ClearSelection();
            Session[sessionKeyGroupDataSelected] = null;
            trvGroupData.ShowCheckBoxes = TreeNodeTypes.None;
            trvGroupData.Nodes.Clear();
            chkMultiCause.Checked = false;

            LoadInitialData();
            LoadGroupData();

            Session[sessionKeyEmployeesLoaded] = null;
            Session[sessionKeyEmployeesByFilter] = null;
            hdfEmployeeSelectedDirect.Value = "";
            hdfEmployeesSelected.Value = "";
            hdfParticipantsSelected.Value = "";
            searchSelected.Value = "";
            divCloseDate.Visible = false;
            divAccidentCase.Visible = false;
            divState.Visible = false;
            divRelated.Visible = false;
            grvEmployees.DataSource = LoadEmptyEmployees();
            grvEmployees.DataBind();
            grvEmployeesByFilter.DataSource = LoadEmptyEmployees();
            grvEmployeesByFilter.DataBind();

            txtEmployeeIdFilter.Text = "";
            txtEmployeeNameFilter.Text = "";

            pnlAbsenteeismContent.Visible = false;
            pnlDetailsContent.Visible = false;
            pnlDocumentsContent.Visible = false;
            pnlResults.Visible = false;
            pnlMainContent.Visible = true;
            pnlDirectSearch.Visible = true;
            pnlAdvancedSearch.Visible = false;

            ClearForm();
        }

        private void ClearForm()
        {
            cboCause.SelectedValue = "-1";
            cboCaseState.SelectedValue = "OP";
            cboInterestGroups.ClearSelection();
            Session[sessionKeyInterestGroupAbsenteeism] = null;
            List<Cdu> listaVacia = new List<Cdu>();
            cboInterestGroups.DataSource = listaVacia;
            cboInterestGroups.DataBind();
            txtDescription.Text = "";
            txtRelatedCase.Text = "";
            txtDetailedDescription.Text = "";
            dtpIncidentDate.Text = "";
            tpcIncidentDateTime.Text = "";
            dtpSuspendWorkDate.Text = "";
            tpcSuspendWorkTime.Text = "";
            dtpFinalDate.Text = "";
            chkCloseDate.Checked = false;
            cboTimeUnit.ClearSelection();
            txtNaturalDays.Text = "";
            cboUnsafeActs.SelectedValue = "-1";
            cboMaterialAgents.SelectedValue = "-1";
            cboUnsafeConditions.SelectedValue = "-1";
            cboEstablishments.SelectedValue = "-1";
            cboDayFactor.SelectedValue = "-1";
            cboPersonalFactor.SelectedValue = "-1";
            cboJourneys.SelectedValue = "-1";
            cboLaborMade.SelectedValue = "-1";
            cboBodyParts.SelectedValue = "-1";
            cboAccidentType.SelectedValue = "-1";
            cboDiseaseType.SelectedValue = "-1";
            cboDocuments.SelectedValue = "";
            txtDocumentDescription.Text = "";
            Session[sessionKeyDocumentsSaved] = null;
            divCloseDate.Visible = false;
            divAccidentCase.Visible = false;
            divState.Visible = false;
            divRelated.Visible = false;
        }

        #region GridMethods
        /// <summary>
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvEmployees_PreRender(object sender, EventArgs e)
        {
            if ((grvEmployees.ShowHeader && grvEmployees.Rows.Count > 0) || (grvEmployees.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvEmployees.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (grvEmployees.ShowFooter && grvEmployees.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvEmployees.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvEmployees_Sorting(object sender, GridViewSortEventArgs e)
        {
            // 5265 - Paolo Aguilar
            if (Session[sessionKeyEmployeesLoaded] != null)
            {
                DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];
                DataView dataview = new DataView(loadedEmployees);

                string sortdirection = CommonFunctions.SwitchGetSortDirection(Page.ClientID, grvEmployees.ClientID, e.SortExpression);
                dataview.Sort = string.Format("{0} {1}", e.SortExpression, sortdirection);
                DataTable dt = dataview.ToTable();

                grvEmployees.DataSource = dt;
                grvEmployees.DataBind();
            }
        }

        #endregion

        protected void BtnAdvancedSearch_ServerClick(object sender, EventArgs e)
        {
            hdfEmployeeSelectedDirect.Value = "";
            hdfEmployeesSelected.Value = "";

            grvEmployeesByFilter.DataSource = LoadEmptyEmployees();
            grvEmployeesByFilter.DataBind();

            pnlDirectSearch.Visible = false;
            pnlAdvancedSearch.Visible = true;

            grvEmployees.DataSource = LoadEmptyEmployees();
            grvEmployees.DataBind();
            Session[sessionKeyAbsenteeismCausesDataResults] = null;
            Session[sessionKeyGroupDataSelected] = null;
            Session[sessionKeyEmployeesLoaded] = null;
            Session[sessionKeyDocumentsSaved] = null;
            Session[sessionAbsenteeismCausesCategory] = null;
            Session[sessionAbsenteeismCausesAdditionalInfo] = null;

            Session[sessionKeyEmployeesFilterLoaded] = null;

            if (cboPayroll.Items.Count > 0)
            {
                cboPayroll.SelectedIndex = 0;
            }

            if (cboGroupType.Items.Count > 0)
            {
                cboGroupType.SelectedIndex = 0;
            }

            if (cboGroup.Items.Count > 0)
            {
                cboGroup.SelectedIndex = 0;
            }

            trvGroupData.Nodes.Clear();
        }

        protected void BtnDirectSearch_ServerClick(object sender, EventArgs e)
        {
            hdfEmployeeSelectedDirect.Value = "";
            hdfEmployeesSelected.Value = "";

            grvEmployees.DataSource = LoadEmptyEmployees();
            grvEmployees.DataBind();

            pnlDirectSearch.Visible = true;
            pnlAdvancedSearch.Visible = false;
        }

        protected void BtnSearchFilter_ServerClick(object sender, EventArgs e)
        {
            grvEmployeesByFilter.Visible = true;
            SearchEmployeesByFilter();
        }

        /// <summary>
        /// Search employees by filter
        /// </summary>
        private void SearchEmployeesByFilter()
        {
            if (string.IsNullOrEmpty(txtEmployeeIdFilter.Text.Trim()) && string.IsNullOrEmpty(txtEmployeeNameFilter.Text))
            {
                MensajeriaHelper.MostrarMensaje(Page
                          , TipoMensaje.Error
                          , Convert.ToString(GetLocalResourceObject("msjSelectFilter")));
            }

            else
            {
                DOLE.HRIS.Services.CR.Business.EmployeesBll objEmployeesBll = new DOLE.HRIS.Services.CR.Business.EmployeesBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                List<Employee> employees = objEmployeesBll.ListAllEmployeesByFilter(HttpContext.Current.User.Identity.Name, txtEmployeeIdFilter.Text.Trim(), txtEmployeeNameFilter.Text.Trim());

                AddEmployeesByFilter(employees);
            }
        }

        public void AddEmployeesByFilter(List<Employee> employees)
        {

            Session[sessionKeyEmployeesByFilter] = LoadEmptyEmployees();

            DataTable loadedEmployeesByFilter = (DataTable)Session[sessionKeyEmployeesByFilter];
            int i = 0;
            var isSelected = employees.Count == 1;

            foreach (Employee employee in employees)
            {
                DataRow row = loadedEmployeesByFilter.NewRow();
                row.ItemArray = new object[] {
                    string.Format("{0}/{1}/{2}/{3}/{4}",
                        employee.Company, employee.Payroll,
                        "", "", employee.ID),
                    isSelected,
                    employee.ID,
                    employee.Name,
                    employee.Company,
                    string.Empty,
                    string.Empty,
                    employee.Company + "-" + employee.Payroll };
                loadedEmployeesByFilter.Rows.InsertAt(row, i++);
                if (isSelected)
                {
                    hdfEmployeeSelectedDirect.Value = employee.ID;
                }
            }

            Session[sessionKeyEmployeesByFilter] = loadedEmployeesByFilter;
            grvEmployeesByFilter.DataSource = loadedEmployeesByFilter;
            grvEmployeesByFilter.DataBind();
        }

        protected void GrvEmployeesByFilter_PreRender(object sender, EventArgs e)
        {
            if ((grvEmployeesByFilter.ShowHeader && grvEmployeesByFilter.Rows.Count > 0) || (grvEmployeesByFilter.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvEmployeesByFilter.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvEmployeesByFilter.ShowFooter && grvEmployeesByFilter.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvEmployeesByFilter.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Enent to hadle post render to grv.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvEmployees_DataBound(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromGridDataBound{0}", Guid.NewGuid()), "ReturnFromGridDataBound();", true);
        }

        #region "Múltiples Causes"

        /// <summary>
        /// Carga de datos en los combos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RptMultipleTfl_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList cboCauseMultipleTfl = (e.Item.FindControl("cboCauseMultipleTfl") as DropDownList);
                List<AbsenteeismCauseEntity> causesFilter = (List<AbsenteeismCauseEntity>)Session["causesFiltered"];

                cboCauseMultipleTfl.DataValueField = "CauseCode";
                cboCauseMultipleTfl.DataTextField = "CauseName";
                cboCauseMultipleTfl.DataSource = causesFilter;
                cboCauseMultipleTfl.DataBind();
            }
        }

        /// <summary>
        /// Establecer eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RptMultipleTfl_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }

        /// <summary>
        /// Carga y filtro de Grupos de Interés según la causa seleccionada
        /// </summary>
        /// <param name="cboCauseMultipleTfl"></param>
        /// <param name="cboInterestGroupsMultipleTfl"></param>
        private void LoadInterestGroupByCauseMultipleTfl(DropDownList cboCauseMultipleTfl, DropDownList cboInterestGroupsMultipleTfl)
        {
            DataTable procesedCdUs = LoadEmptyCdUs();
            List<Cdu> cdus = new List<Cdu>();

            if (Session[sessionKeyInterestGroupAbsenteeism] == null)
            {
                Session[sessionKeyInterestGroupAbsenteeism] = ObjInterestGroupBll.ListAll();
            }

            int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            string geographicDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

            List<AbsenteeismInterestGroupEntity> listInterestGroups = (List<AbsenteeismInterestGroupEntity>)Session[sessionKeyInterestGroupAbsenteeism];
            listInterestGroups = listInterestGroups.Where(x => x.GeographicDivisionCode == geographicDivision).ToList();

            absenteeismCauses = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
            List<AbsenteeismCauseEntity> listCause = absenteeismCauses.Where(c => c.CauseCode == cboCauseMultipleTfl.SelectedValue && c.AbsenteeismCausesByDivision != null && c.AbsenteeismCausesByDivision.Exists(cbd => cbd.DivisionCode == divisionCode)).ToList<AbsenteeismCauseEntity>();
            string[] lstInteresGroupCodeByCause = listCause[0].AbsenteeismCausesByDivision[0].InterestGroupCodes.Split(',');

            if (lstInteresGroupCodeByCause[0] != "")
            {
                if (lstInteresGroupCodeByCause.Count<string>() > 1)
                {
                    Cdu cduInterestEmpty = new Cdu("-1", string.Empty);
                    cdus.Add(cduInterestEmpty);
                }

                foreach (string splitData in lstInteresGroupCodeByCause)
                {
                    List<AbsenteeismInterestGroupEntity> listInterestGroupsCustom = listInterestGroups.Where(x => x.Code == splitData).ToList();
                    Cdu cduInterest = new Cdu(listInterestGroupsCustom[0].Code, listInterestGroupsCustom[0].Description);
                    cdus.Add(cduInterest);
                }
            }

            cboInterestGroupsMultipleTfl.DataValueField = "CDUCode";
            cboInterestGroupsMultipleTfl.DataTextField = "CDUDescription";
            cboInterestGroupsMultipleTfl.DataSource = cdus;
            cboInterestGroupsMultipleTfl.DataBind();
        }

        /// <summary>
        /// Save Massive records 
        /// </summary>
        private void SaveMultipleTfl()
        {
            //LoadInitialData();
            var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            List<AbsenteeismCase> listMultiple = new List<AbsenteeismCase>();
            List<AbsenteeismCase> listAbsenteeismCase = new List<AbsenteeismCase>();
            pnlNewRecord.Visible = true;

            try
            {
                foreach (RepeaterItem item in rptMultipleTfl.Items)
                {
                    #region LimpiezaMensajesValidacion

                    Panel pnlMessage = item.FindControl("pnlMessage") as Panel;
                    Label lblMessage = item.FindControl("lblMessage") as Label;

                    #endregion

                    CheckBox chkIsSelected = item.FindControl("chkIsSelected") as CheckBox;
                    if (chkIsSelected.Checked)
                    {
                        //Validations
                        bool errors = false;
                        string errorsMessages = string.Empty;
                        DateTime? suspendWorkTest = null;
                        DateTime? finalDateTest = null;
                        DateTime dateTest = DateTime.Now;
                        int? naturalDaysTest = null;
                        int integerTest = 0;

                        HiddenField hdnCompanyMultipleTfl = item.FindControl("hdnCompanyMultipleTfl") as HiddenField;
                        HiddenField hdnEmployeeCode = item.FindControl("hdnEmployeeCode") as HiddenField;

                        List<KeyValuePair<Company, Employee>> employee = new List<KeyValuePair<Company, Employee>>
                        {
                            new KeyValuePair<Company, Employee>(new Company(hdnCompanyMultipleTfl.Value), new Employee(hdnEmployeeCode.Value))
                        };

                        TextBox txtDescriptionMultipleTfl = item.FindControl("txtDescriptionMultipleTfl") as TextBox;
                        TextBox tpcSuspendWorkTimeMultipleTfl = item.FindControl("tpcSuspendWorkTimeMultipleTfl") as TextBox;
                        TextBox dtpSuspendWorkDateMultipleTfl = item.FindControl("dtpSuspendWorkDateMultipleTfl") as TextBox;
                        TextBox dtpFinalDateMultipleTfl = item.FindControl("dtpFinalDateMultipleTfl") as TextBox;
                        DropDownList cboInterestGroupsMultipleTfl = item.FindControl("cboInterestGroupsMultipleTfl") as DropDownList;
                        DropDownList cboCauseMultipleTfl = item.FindControl("cboCauseMultipleTfl") as DropDownList;
                        DropDownList cboTimeUnitMultipleTfl = item.FindControl("cboTimeUnitMultipleTfl") as DropDownList;
                        TextBox txtNaturalDaysMultipleTfl = item.FindControl("txtNaturalDaysMultipleTfl") as TextBox;
                        TextBox txtFinalHoursMultipleTfl = item.FindControl("txtFinalHoursMultipleTfl") as TextBox;
                        Label txtFinalHoursMultipleTflValidation = item.FindControl("txtFinalHoursMultipleTflValidation") as Label;

                        #region Validaciones

                        if (string.IsNullOrWhiteSpace(cboCauseMultipleTfl.SelectedValue) || cboCauseMultipleTfl.SelectedValue == "-1")
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjCauseValidation")));
                        }

                        if (string.IsNullOrWhiteSpace(cboInterestGroupsMultipleTfl.SelectedValue) || cboInterestGroupsMultipleTfl.SelectedValue == "-1")
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjInterestGroupValidation")));
                        }

                        if (string.IsNullOrWhiteSpace(txtDescriptionMultipleTfl.Text) ||
                        txtDescriptionMultipleTfl.Text.Length > 2000)
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDetailedDescriptionValidation")));
                        }

                        if (string.IsNullOrWhiteSpace(cboTimeUnitMultipleTfl.SelectedValue))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjTimeUnitValidation")));
                        }

                        if (string.IsNullOrWhiteSpace(dtpSuspendWorkDateMultipleTfl.Text) || string.IsNullOrWhiteSpace(tpcSuspendWorkTimeMultipleTfl.Text) ||
                        DateTime.TryParseExact(string.Format("{0} {1}", dtpSuspendWorkDateMultipleTfl.Text, tpcSuspendWorkTimeMultipleTfl.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false ||
                        DayOfWeek.Sunday.Equals(dateTest.DayOfWeek))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkValidation")));
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkTimeValidation")));
                        }

                        else
                        {
                            suspendWorkTest = DateTime.ParseExact(string.Format("{0} {1}", dtpSuspendWorkDateMultipleTfl.Text, tpcSuspendWorkTimeMultipleTfl.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        }

                        if (string.IsNullOrWhiteSpace(dtpFinalDateMultipleTfl.Text) ||
                            DateTime.TryParseExact(dtpFinalDateMultipleTfl.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false)
                        {
                            //parches
                            if (cboTimeUnitMultipleTfl.SelectedValue.Equals("Hours"))
                            {
                                if (!string.IsNullOrWhiteSpace(dtpSuspendWorkDateMultipleTfl.Text)
                                    && !string.IsNullOrEmpty(txtFinalHoursMultipleTfl.Text))
                                {
                                    dtpFinalDateMultipleTfl.Text = dtpSuspendWorkDateMultipleTfl.Text;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrWhiteSpace(dtpSuspendWorkDateMultipleTfl.Text)
                                    && !string.IsNullOrEmpty(txtNaturalDaysMultipleTfl.Text))
                                {
                                    dtpFinalDateMultipleTfl.Text = Convert.ToDateTime(dtpSuspendWorkDateMultipleTfl.Text).AddDays(Convert.ToInt16(txtNaturalDaysMultipleTfl.Text)).ToShortDateString();
                                }
                            }

                            if (string.IsNullOrWhiteSpace(dtpFinalDateMultipleTfl.Text))
                            {
                                //CAUTION: dtpFinalDate value is not been sent to ADAM, it is used just to calculate natural days but is validated in order to make natural days validation completed
                                errors = true;
                                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalDateValidation")));
                            }
                        }

                        else
                        {
                            finalDateTest = DateTime.ParseExact(string.Format("{0} 23:59", dtpFinalDateMultipleTfl.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        }

                        if (cboTimeUnitMultipleTfl.SelectedValue.Equals("Days"))
                        {
                            if (string.IsNullOrWhiteSpace(txtNaturalDaysMultipleTfl.Text) || int.TryParse(txtNaturalDaysMultipleTfl.Text, out integerTest) == false || integerTest <= 0 || integerTest > 10000)
                            {
                                if (suspendWorkTest.HasValue && finalDateTest.HasValue)
                                {
                                    txtNaturalDaysMultipleTfl.Text = Convert.ToInt32(((TimeSpan)(finalDateTest.Value - suspendWorkTest.Value)).TotalDays).ToString();
                                }

                                if (string.IsNullOrWhiteSpace(txtNaturalDaysMultipleTfl.Text))
                                {
                                    errors = true;
                                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjNaturalDaysValidation")));
                                }
                            }

                            else
                            {
                                naturalDaysTest = int.Parse(txtNaturalDaysMultipleTfl.Text);
                            }
                        }


                        var finalHoursMTFL = txtFinalHoursMultipleTfl.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                        if (string.Equals(cboTimeUnitMultipleTfl.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) &&
                            (string.IsNullOrWhiteSpace(finalHoursMTFL) ||
                            decimal.TryParse(finalHoursMTFL, out decimal decimalTest) == false ||
                            !decimalTest.HasThisManyDecimalPlacesOrLess(3) ||
                            decimalTest <= 0 || decimalTest > 8))

                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalHoursValidation")));
                            txtFinalHoursMultipleTflValidation.Visible = true;
                        }

                        if (suspendWorkTest.HasValue && finalDateTest.HasValue)
                        {
                            if (DateTime.Compare(suspendWorkTest.Value, finalDateTest.Value) > 0)
                            {
                                errors = true;
                                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkAndFinalDate")));
                            }
                        }

                        if (suspendWorkTest.HasValue)
                        {
                            if (DateTime.Compare(suspendWorkTest.Value, DateTime.Now.AddDays(30)) > 0)
                            {
                                errors = true;
                                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkCannotFutureDistant")));
                            }
                        }

                        LoadInitialData();
                        AbsenteeismCauseEntity selectedCause = null;

                        if (!string.IsNullOrWhiteSpace(cboCauseMultipleTfl.SelectedValue) && cboCauseMultipleTfl.SelectedValue != "-1")
                        {
                            selectedCause = new AbsenteeismCauseEntity();
                            selectedCause = absenteeismCauses.FirstOrDefault(a => string.Equals(a.CauseCode, cboCauseMultipleTfl.SelectedValue, StringComparison.CurrentCultureIgnoreCase));
                        }

                        if (!string.IsNullOrWhiteSpace(cboCauseMultipleTfl.SelectedValue) && selectedCause != null)
                        {
                            List<AbsenteeismCauseEntity> causesNotConfigured = absenteeismCauses.Where(a => a.CauseCode == cboCauseMultipleTfl.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours == false && cbd.Days == false)).ToList();
                            if (causesNotConfigured.Count > 0)
                            {
                                errors = true;
                                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjCauseNotConfigured")));
                            }

                            //HACK: DIFERENCIA REGIONAL #11
                            if (DivisionSession == 5) // ECU
                            {
                                List<string> disabilities = new List<string> { "EAT" };
                                if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) && disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                                {
                                    errors = true;
                                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                                }
                            }

                            //HACK: DIFERENCIA REGIONAL #11
                            if (DivisionSession == 4) // HND
                            {
                                List<string> disabilities = new List<string> { "IN", "INH", "ING", "ACTY" };
                                if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) && disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                                {
                                    errors = true;
                                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                                }
                            }

                            //HACK: DIFERENCIA REGIONAL #11
                            if (DivisionSession == 1) // CRC
                            {
                                List<string> disabilities = new List<string> { "IL", "IN", "IN3", "INCC", "INC2", "IPMT", "TACC" };
                                if (string.Equals(cboTimeUnitMultipleTfl.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) && disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                                {
                                    errors = true;
                                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                                }

                                List<string> disabilities3Days = new List<string> { "INCC" };
                                if (disabilities3Days.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped) && naturalDaysTest.HasValue && (naturalDaysTest <= 0 || naturalDaysTest > 3))
                                {
                                    errors = true;
                                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilities3Days")));
                                }

                                List<string> listNextDayAfterMidDay = new List<string> { "IL", "IN", "TACC" };
                                if (suspendWorkTest.HasValue)
                                {
                                    if (suspendWorkTest.Value.Hour >= 12 && (finalDateTest.Value.Date == suspendWorkTest.Value.Date))
                                    {
                                        errors = true;
                                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalDateCannotSameSuspended")));
                                    }
                                }
                            }
                        }

                        #endregion

                        if (!errors)
                        {
                            DateTime suspendWork = DateTime.ParseExact(string.Format("{0} {1}", dtpSuspendWorkDateMultipleTfl.Text, tpcSuspendWorkTimeMultipleTfl.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            List<Document> documents = new List<Document>();
                            AbsenteeismCase absenteeismCase = new AbsenteeismCase(employee
                                , 0 //AbsenteeismId
                                , "" //description
                                , txtDescriptionMultipleTfl.Text.Trim()//detail
                                , suspendWork //startDate
                                , string.IsNullOrWhiteSpace(txtNaturalDaysMultipleTfl.Text) ? 0 : Convert.ToInt32(txtNaturalDaysMultipleTfl.Text)
                                , cboTimeUnitMultipleTfl.SelectedValue == "Days" ? 0 : 1
                                , null //return days
                                , 0 //case
                                , absenteeismCauses.First(a => a.CauseCode == cboCauseMultipleTfl.SelectedValue).AbsenteeismCausesByDivision[0].CauseCodeAdamMapped //cause
                                , "CL" //state always CL for multiple
                                , finalDateTest //close Date
                                , 0 //business Days
                                , string.IsNullOrWhiteSpace(txtFinalHoursMultipleTfl.Text) ? 0 : Convert.ToDecimal(txtFinalHoursMultipleTfl.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                                , suspendWork //suspended
                                , cboInterestGroupsMultipleTfl.SelectedValue
                                , 0 //related case
                                , 1
                                , UserHelper.GetCurrentFullUserName //username
                                , null //selectedUnsafeActs
                                , null //selectedMaterialAgents
                                , null //selectedUnsafeConditions
                                , null //selectedEstablishments
                                , null //selectedDayFactor
                                , null //selectedPersonalFactor
                                , null //selectedJourneys
                                , null //selectedLaborMade
                                , null //selectedBodyParts
                                , null //selectedAccidentType
                                , null //selectedDiseaseType
                                , documents //null
                                , 1 //randomico
                                , false); //Close Absenteeism (Dar de alta)


                            //add to list
                            //listAbsenteeismCase.Add(absenteeismCase);
                            try
                            {
                                //Llama a base de datos por cada registro seleccionado en el repeater
                                DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                                List<AbsenteeismID> absenteeismID = objAbsenteeismCaseBll.Save(absenteeismCase, Session["culture"] == null ? "es-CR" : Session["culture"].ToString());

                                pnlMessage.Visible = true;
                                pnlMessage.CssClass = "alert alert-success";
                                lblMessage.Text = string.Format(Convert.ToString(GetLocalResourceObject("msjRecordInsertSuccess")), absenteeismID[0].AbsenteeismId);
                                chkIsSelected.Checked = false;

                                chkIsSelected.Enabled = false;
                                txtDescriptionMultipleTfl.Enabled = false;
                                tpcSuspendWorkTimeMultipleTfl.Enabled = false;
                                dtpSuspendWorkDateMultipleTfl.Enabled = false;
                                dtpFinalDateMultipleTfl.Enabled = false;
                                cboInterestGroupsMultipleTfl.Enabled = false;
                                cboCauseMultipleTfl.Enabled = false;
                                cboTimeUnitMultipleTfl.Enabled = false;
                                txtNaturalDaysMultipleTfl.Enabled = false;
                                txtFinalHoursMultipleTfl.Enabled = false;
                            }

                            catch (Exception ex)
                            {
                                if (ex is DataAccessException
                                    || ex is BusinessException
                                    || ex is PresentationException
                                    || ex is ServiceException)
                                {
                                    lblMessage.Text = ex.Message;
                                    pnlMessage.Visible = true;
                                    pnlMessage.CssClass = "alert alert-warning";
                                }
                                else
                                {
                                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                                    lblMessage.Text = newEx.Message;
                                    pnlMessage.Visible = true;
                                }
                            }
                        }

                        else
                        {
                            errorsMessages = string.Format("{0}\\n{1}", Convert.ToString(GetLocalResourceObject("verifyErrors")), errorsMessages);
                            errorsMessages = errorsMessages.Replace("\\n", "<br/>");
                            errorsMessages = errorsMessages.Replace("\n", "");
                            errorsMessages = errorsMessages.Replace("\r", "");
                            lblMessage.Text = errorsMessages;
                            pnlMessage.CssClass = "alert alert-warning";
                            pnlMessage.Visible = true;
                        }
                    }

                    else
                    {
                        pnlMessage.Visible = true;

                        if (chkIsSelected.Enabled) //esto siginifica que está deshabilitado porque ya está insertado
                        {
                            pnlMessage.CssClass = "alert alert-info";
                            lblMessage.Text = Convert.ToString(GetLocalResourceObject("msjRecordOmitted"));
                        }

                        else
                        {
                            pnlMessage.CssClass = "alert alert-success";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException
                    || ex is ServiceException)
                {
                    MensajeriaHelper.MostrarMensaje(btnSave
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(btnSave
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #endregion

        protected void CboTimeUnitMultipleTfl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cboTimeUnitMultipleTfl = (sender as DropDownList);
            Panel pnlNaturalDaysDataMultipleTfl = cboTimeUnitMultipleTfl.FindControl("pnlNaturalDaysDataMultipleTfl") as Panel;
            Panel pnlHoursDataMultipleTfl = cboTimeUnitMultipleTfl.FindControl("pnlHoursDataMultipleTfl") as Panel;
            pnlNaturalDaysDataMultipleTfl.Visible = cboTimeUnitMultipleTfl.SelectedValue == "Days";
            pnlHoursDataMultipleTfl.Visible = cboTimeUnitMultipleTfl.SelectedValue != "Days";
            TextBox tpcSuspendWorkTimeMultipleTfl = cboTimeUnitMultipleTfl.FindControl("tpcSuspendWorkTimeMultipleTfl") as TextBox;
            tpcSuspendWorkTimeMultipleTfl.ReadOnly = cboTimeUnitMultipleTfl.SelectedValue == "Days";

            TextBox dtpSuspendWorkDateMultipleTfl = cboTimeUnitMultipleTfl.FindControl("dtpSuspendWorkDateMultipleTfl") as TextBox;
            TextBox dtpFinalDateMultipleTfl = cboTimeUnitMultipleTfl.FindControl("dtpFinalDateMultipleTfl") as TextBox;

            TextBox txtNaturalDaysMultipleTfl = cboTimeUnitMultipleTfl.FindControl("txtNaturalDaysMultipleTfl") as TextBox;

            if ((dtpSuspendWorkDateMultipleTfl.Text != "" && dtpFinalDateMultipleTfl.Text != "") && (dtpSuspendWorkDateMultipleTfl.Text.Equals(dtpFinalDateMultipleTfl.Text)))
            {
                txtNaturalDaysMultipleTfl.Text = "1";
            }

            else
            {
                dtpFinalDateMultipleTfl.Text = "";
                txtNaturalDaysMultipleTfl.Text = "";
            }

            if (cboTimeUnitMultipleTfl.SelectedValue.Equals("Hours"))
            {
                dtpFinalDateMultipleTfl.Text = dtpSuspendWorkDateMultipleTfl.Text;
            }
        }

        protected void CboCauseMultipleTfl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cboCauseMultipleTfl = (sender as DropDownList);

            DropDownList cboInterestGroupsMultipleTfl = cboCauseMultipleTfl.FindControl("cboInterestGroupsMultipleTfl") as DropDownList;
            DropDownList cboTimeUnitMultipleTfl = cboCauseMultipleTfl.FindControl("cboTimeUnitMultipleTfl") as DropDownList;

            LoadInterestGroupByCauseMultipleTfl(cboCauseMultipleTfl, cboInterestGroupsMultipleTfl);

            absenteeismCauses = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];

            //Time unit
            if (cboTimeUnitMultipleTfl.Items.Count > 0)
            {
                cboTimeUnitMultipleTfl.Items.Clear();
            }

            List<AbsenteeismCauseEntity> causes2Forms = absenteeismCauses.Where(a => a.CauseCode == cboCauseMultipleTfl.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours && cbd.Days)).ToList();
            List<AbsenteeismCauseEntity> causesOnlyHours = absenteeismCauses.Where(a => a.CauseCode == cboCauseMultipleTfl.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours)).ToList();
            List<AbsenteeismCauseEntity> causesOnlyDays = absenteeismCauses.Where(a => a.CauseCode == cboCauseMultipleTfl.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Days)).ToList();

            cboTimeUnitMultipleTfl.Items.Add(new ListItem("", ""));

            if (causes2Forms.Count > 0)
            {
                //Time unit
                cboTimeUnitMultipleTfl.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                cboTimeUnitMultipleTfl.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                cboTimeUnitMultipleTfl.ClearSelection();
            }

            else if (causesOnlyHours.Count > 0)
            {
                //Time unit
                cboTimeUnitMultipleTfl.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                cboTimeUnitMultipleTfl.ClearSelection();
            }

            else if (causesOnlyDays.Count > 0)
            {
                //Time unit
                cboTimeUnitMultipleTfl.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                cboTimeUnitMultipleTfl.ClearSelection();
            }
        }

        protected void BtnReturnMultipleTfl_ServerClick(object sender, EventArgs e)
        {
            grvEmployeesByFilter.Visible = false;
            cboPayroll.SelectedIndex = 0;
            cboGroupType.SelectedIndex = 0;
            cboGroup.SelectedIndex = 0;
            cboGroup.ClearSelection();
            Session[sessionKeyGroupDataSelected] = null;
            trvGroupData.ShowCheckBoxes = TreeNodeTypes.None;
            trvGroupData.Nodes.Clear();
            chkMultiCause.Checked = false;
            LoadInitialData();
            LoadGroupData();

            Session[sessionKeyEmployeesLoaded] = null;
            Session[sessionKeyEmployeesByFilter] = null;
            hdfEmployeeSelectedDirect.Value = "";
            hdfEmployeesSelected.Value = "";
            searchSelected.Value = "";
            grvEmployees.DataSource = LoadEmptyEmployees();
            grvEmployees.DataBind();
            grvEmployeesByFilter.DataSource = LoadEmptyEmployees();
            grvEmployeesByFilter.DataBind();
            txtEmployeeIdFilter.Text = "";
            txtEmployeeNameFilter.Text = "";

            pnlAbsenteeismContent.Visible = false;
            pnlDetailsContent.Visible = false;
            pnlDocumentsContent.Visible = false;
            pnlResults.Visible = false;
            pnlAdvancedSearch.Visible = false;
            pnlMultipleTFL.Visible = false;
            pnlNewRecord.Visible = false;
            pnlMainContent.Visible = true;
            pnlDirectSearch.Visible = true;
        }

        /// <summary>
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvEmployeesSelected_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((grvEmployeesSelected.ShowHeader && grvEmployeesSelected.Rows.Count > 0) || (grvEmployeesSelected.ShowHeaderWhenEmpty))
                {
                    //Force GridView to use <thead> instead of <tbody>
                    grvEmployeesSelected.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                if (grvEmployeesSelected.ShowFooter && grvEmployeesSelected.Rows.Count > 0)
                {
                    //Force GridView to use <tfoot> instead of <tbody>
                    grvEmployeesSelected.FooterRow.TableSection = TableRowSection.TableFooter;
                }
            }
        }

        /// <summary>
        /// Enent to hadle post render to grv.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvEmployeesSelected_DataBound(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromGridDataBound{0}", Guid.NewGuid()), "ReturnFromGridDataBound();", true);
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvEmployeesSelected_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyFilterAbsenteeismLoaded] != null)
            {
                DataTable loadedAbsenteeism = (DataTable)Session[sessionKeyFilterAbsenteeismLoaded];
                DataView dataview = new DataView(loadedAbsenteeism);

                string sortdirection = CommonFunctions.SwitchGetSortDirection(Page.ClientID, grvEmployeesSelected.ClientID, e.SortExpression);
                dataview.Sort = string.Format("{0} {1}", e.SortExpression, sortdirection);

                DataTable dt = dataview.ToTable();
                grvEmployeesSelected.DataSource = dt;
                grvEmployeesSelected.DataBind();
            }
        }

        protected void ChkEmployeesSelected_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                foreach (GridViewRow row in grvEmployeesSelected.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chbParticipantSelected") as CheckBox);
                        if (chkRow.Checked)
                        {
                            count++;
                        }
                    }
                }

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCountEmployes{0}", Guid.NewGuid()), "UpdateLblCountEmployes(" + count.ToString() + ");", true);
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

        protected void txtEmployeeNameFilter_TextChanged(object sender, EventArgs e)
        {
            grvEmployees.DataSource = LoadEmptyEmployees();
            grvEmployees.DataBind();
        }
    }
}