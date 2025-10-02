using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.Business.Remote;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using DOLE.HRIS.Shared.Entity.MassiveData;
using HRISWeb.Help;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
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
    public partial class AbsenteeismUpdate : System.Web.UI.Page
    {
        [Dependency]
        public IAbsenteeismCausesBll<AbsenteeismCauseEntity> ObjCausesBll { get; set; }

        [Dependency]
        public IAbsenteeismFileBll<FileEntity> ObjAbsenteeismFileBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll ObjGeneralConfigurationsBll { get; set; }

        [Dependency]
        public IAdminUsersByModulesBll<AdminUserByModuleEntity> ObjAdminUsersByModulesBll { get; set; }

        [Dependency]
        public IAbsenteeismInterestGroupBll<AbsenteeismInterestGroupEntity> ObjInterestGroupBll { get; set; }

        [Dependency]
        public IAbsenteeismAttachedDocumentsBll<AbsenteeismAttachedDocumentEntity> ObjAttachedDocumentsBll { get; set; }

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
        readonly string sessionKeyAbsenteeismAditionalInfoLoaded = "Absenteeism-AditionalInfoAbsenteeismLoaded";
        readonly string sessionKeyAbsenteeismFiles = "Absenteeism-AbsenteeismFiles";
        readonly string sessionKeyDivisionCode = "Absenteeism-DivisionCodeUser";
        readonly string sessionKeyAbsenteeismResults = "Absenteeism-AbsenteeismResults";

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
            var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            //HACK: DIFERENCIA REGIONAL #?
            lblEstablishments.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblEstablishmentsECU") : GetLocalResourceObject("lblEstablishments"));

            // 5435 - Paolo Aguilar
            lblUnsafeActs.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblUnsafeActsECU") : GetLocalResourceObject("lblUnsafeActs"));
            lblUnsafeConditions.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblUnsafeConditionsECU") : GetLocalResourceObject("lblUnsafeConditions"));
            lblPersonalFactor.InnerText = Convert.ToString(DivisionSession == 5 ? GetLocalResourceObject("lblPersonalFactorECU") : GetLocalResourceObject("lblPersonalFactor"));
            
            // 5435 - Paolo Aguilar
            if (Session[sessionKeyDivisionCode] == null)
            {
                Session[sessionKeyDivisionCode] = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            }

            try
            {
                if (!IsPostBack)
                {
                    cboPageSize.Items.Insert(cboPageSize.Items.Count, new ListItem("50", "50"));
                    cboPageSize.Items.Insert(cboPageSize.Items.Count, new ListItem("100", "100"));
                    cboPageSize.Items.Insert(cboPageSize.Items.Count, new ListItem("200", "200"));

                    cboDateType.Items.Insert(cboDateType.Items.Count, new ListItem("Fecha Suspende", "1"));
                    cboDateType.Items.Insert(cboDateType.Items.Count, new ListItem("Fecha Registro / Fecha Cambio", "2"));

                    if (Convert.ToInt32(Session[sessionKeyDivisionCode]) != SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode)
                    {
                        Session[sessionKeyDivisionCode] = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        Session[sessionKeyAbsenteeismInitialDataResults] = null;
                    }

                    Session[sessionKeyAbsenteeismCausesDataResults] = null;
                    Session[sessionKeyGroupDataSelected] = null;
                    Session[sessionKeyEmployeesLoaded] = null;
                    Session[sessionKeyDocumentsSaved] = null;
                    Session["ArchivosTemporales"] = null;

                    LoadInitialData();

                    cboPayroll.Enabled = true;
                    cboPayroll.DataValueField = "PayrollCode";
                    cboPayroll.DataTextField = "PayrollDescription";
                    cboPayroll.DataSource = GetPayroll();
                    cboPayroll.DataBind();

                    cboInterestGroups.Enabled = true;

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
                    divRelated.Visible = false;
                    divCloseDate.Visible = false;

                    //Time unit
                    cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                    cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                    cboTimeUnit.SelectedIndex = 0;

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

                    cboDocumentsAbsenteeism.DataValueField = "DocumentCode";
                    cboDocumentsAbsenteeism.DataTextField = "DocumentDescription";

                    //HACK: DIFERENCIA REGIONAL #34 #35
                    List<Document> listDocuments = new List<Document>();
                    if (DivisionSession == 1 //División de Costa Rica
                        || DivisionSession == 4 //División de Honduras
                        || DivisionSession == 5 //División de Ecuador
                        || DivisionSession == 9 ///División de Perú
                        || DivisionSession == 11 //Doce Costa Rica
                        || DivisionSession == 14 //División de Guatemala
                        || DivisionSession == 15 //Dole Tropical Products
                        || DivisionSession == 16 //Dole Shared Services
                        )
                    {

                        List<Document> documents = ObjAttachedDocumentsBll.ListAttachedDocumentTypeByDivision(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, false, true);
                        cboDocumentsAbsenteeism.DataSource = GetDocumentsWithCodeAndDescription(documents, true);
                        cboDocumentsAbsenteeism.DataBind();
                    }

                    Session[sessionKeyEmployeesFilterLoaded] = null;

                    //HACK: DIFERENCIA REGIONAL #36 #37
                    //Visible text field to INS Case Filter
                    if (DivisionSession == 1//División de Costa Rica
                        || DivisionSession == 11//Doce Costa Rica
                        || DivisionSession == 15//Dole Tropical Products
                        || DivisionSession == 16//Dole Shared Services
                        || DivisionSession == 5//División de Ecuador
                        || DivisionSession == 9//División de Perú
                        || DivisionSession == 4//División de Honduras
                        || DivisionSession == 14) //División de Guatemala
                    {
                        divDescriptionFilter.Visible = true;
                    }

                    else
                    {
                        divDescriptionFilter.Visible = false;
                    }

                    GeneralConfigurationEntity uploadSize = ObjGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.UploadFileSize);
                    lblInformationFile.Text = string.Format(Convert.ToString(GetLocalResourceObject("fileInformation")), uploadSize.GeneralConfigurationValue);

                }else {
                    if (Session[sessionKeyAbsenteeismResults] != null)
                    {
                        PageHelper<AbsenteeismCase> pageHelper = (PageHelper<AbsenteeismCase>)Session[sessionKeyAbsenteeismResults];
                        PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                    }
                }

                txtNaturalDays.Attributes.Add("readonly", "readonly"); //readonly attr keep value on postback
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
        /// Load Employess by company code
        /// </summary>
        /// <param name="compania">0 = All companies.</param>
        private void LoadEmployee(List<string> payrolls, string employeeId, string name, DateTime startDate, DateTime endDate)
        {
            List<Employee> employeesLoaded = new List<Employee>();

            foreach (var payroll in payrolls)
            {
                var payrollParts = payroll.Split('-');

                // realizar foreach en en esta parte y cargar la lista entera antes del datasource
                DOLE.HRIS.Services.CR.Business.EmployeesBll objEmployeesBll = new DOLE.HRIS.Services.CR.Business.EmployeesBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                List<Employee> employees = objEmployeesBll.ListAllActive(HttpContext.Current.User.Identity.Name, payrollParts[0], payrollParts[1], employeeId, name, startDate, endDate);


                employeesLoaded.AddRange(employees);
            }

            if (employeesLoaded != null && employeesLoaded.Count > 1)
            {
                employeesLoaded.Insert(0, new Employee("", "", ""));
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
            absenteeisms.Columns.Add("tiempo", typeof(decimal));
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
            absenteeisms.Columns.Add("compania", typeof(string));
            absenteeisms.Columns.Add("clave_unica", typeof(string));
            absenteeisms.Columns.Add("trabajador", typeof(string));
            absenteeisms.Columns.Add("IsSelected", typeof(bool));
            absenteeisms.Columns.Add("CauseDescription", typeof(string));
            absenteeisms.Columns.Add("StateDescription", typeof(string));
            absenteeisms.Columns.Add("ShowFileButton", typeof(bool));

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

            initialData = (AbsenteeismsInitialData)Session[sessionKeyAbsenteeismInitialDataResults];
            List<AbsenteeismCauseEntity> causes = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];

            var causesWithAdtionalInfo = GetCausesWithAdditionalInformation();
            int i = 0;

            foreach (AbsenteeismCase absenteeism in absenteeisms)
            {
                DataTable employees = new DataTable();
                employees.Columns.Add("Nombre", typeof(string));
                employees.Columns.Add("Compania", typeof(string));
                employees.Columns.Add("Clave_unica", typeof(string));
                employees.Columns.Add("Trabajador", typeof(string));

                foreach (KeyValuePair<Company, Employee> ce in absenteeism.Employees)
                {
                    employees.Rows.Add(ce.Value.Name);
                    employees.Rows.Add(ce.Key.CompanyCode);
                    employees.Rows.Add(ce.Value.UniqueKey);
                    employees.Rows.Add(ce.Value.ID);
                }

                var cause = causes.Where(a => a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.CauseCodeAdamMapped == Convert.ToString(absenteeism.Cause))).FirstOrDefault();
                var causeName = absenteeism.Cause;
                if (cause != null)
                {
                    causeName = cause.CauseName;
                }

                DataRow row = loadedAbsenteeisms.NewRow();
                row.ItemArray = new object[] {
                    employees.Rows[0][0],
                    absenteeism.AbsenteeismId,
                    absenteeism.StartDate,
                    absenteeism.Description,
                    absenteeism.DetailedDescription,
                    absenteeism.StartDate,
                    absenteeism.TimeUnit == 0 ? absenteeism.NaturalDays :absenteeism.Hours,
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
                    employees.Rows[1][0],
                    employees.Rows[2][0],
                    employees.Rows[3][0],
                    false,
                    causeName,
                    initialData.StateCases.Where(x => x.CDUCode == absenteeism.State).FirstOrDefault().CDUDescription,
                    cause != null && causesWithAdtionalInfo.Contains(cause.CauseCode),
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
                List<AbsenteeismCauseEntity> causesWithAdditionalInformation = absenteeismCauses.Where(a => a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.NeedsAdditionalInformation)).ToList();
                string causeCodeList = string.Join(",", causesWithAdditionalInformation.Select(c => string.Format("'{0}'", c.CauseCode)).ToList<string>());

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
                LoadInitialData();

                GeneralConfigurationEntity configuration = ObjGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.CauseCategoryAccident);
                List<string> Codes = new List<string>(configuration.GeneralConfigurationValue.Split(','));
                List<AbsenteeismCauseEntity> causesWithCategoryAccident = absenteeismCauses.Where(a => a.CauseCode != null && Codes.Contains(a.CauseCode)).ToList();

                string causeCodeList = string.Join(",", causesWithCategoryAccident.Select(c => string.Format("'{0}'", c.CauseCode)).ToList<string>());

                return causeCodeList;
            }

            catch (Exception)
            {
                return string.Empty;
            }
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
                    //HACK: DIFERENCIA REGIONAL #38 #39
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
                string documentText = !string.IsNullOrEmpty(d.DocumentName) ? d.DocumentName : d.DocumentCode;
                procesedDocuments.Rows.Add(d.DocumentCode, documentText);
            }

            return procesedDocuments;
        }

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

            initialData = (AbsenteeismsInitialData)Session[sessionKeyAbsenteeismInitialDataResults];
            absenteeismCauses = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
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

                if (string.IsNullOrEmpty(hdfEmployeeSelectedPayroll.Value))
                {
                    cboPayroll.SelectedIndex = -1;
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
        /// Handles the event Click on btnNextToPage2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnNextToPage2_Click(object sender, EventArgs e)
        {
            try
            {
                rptRelatedCase.DataSource = LoadEmptyRelatedCase();
                rptRelatedCase.DataBind();
                bool absenteeismSelected = false;

                if (Session[sessionKeyFilterAbsenteeismLoaded] == null)
                {
                    Session[sessionKeyFilterAbsenteeismLoaded] = LoadEmptyFilterAbsenteeism();
                }

                DataTable loadedEmployeesFilter = (DataTable)Session[sessionKeyFilterAbsenteeismLoaded];

                foreach (GridViewRow item in grvCausas.Rows)
                {
                    CheckBox chbEmployee = (CheckBox)item.FindControl("chbEmployeeFilter");
                    HiddenField hdnAbsteeismId = (HiddenField)item.FindControl("hdnAbsteeismId");
                    HiddenField hdnCompany = (HiddenField)item.FindControl("hdnCompany");

                    if (chbEmployee.Checked)
                    {
                        absenteeismSelected = true;
                        DataTable absenteeimsFilter = (DataTable)Session[sessionKeyFilterAbsenteeismLoaded];
                        DataTable absenteeimSelected = absenteeimsFilter.AsEnumerable().Where(x => x.Field<string>("AbsteeismId") == hdnAbsteeismId.Value && x.Field<string>("compania") == hdnCompany.Value).CopyToDataTable();
                        
                        if (absenteeimSelected.Rows.Count == 1)
                        {
                            FillData(absenteeimSelected);
                        }

                        pnlMainContent.Visible = false;
                        pnlAbsenteeismContent.Visible = true;
                        break;
                    }
                }

                if (!absenteeismSelected)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                          , TipoMensaje.Error
                          , Convert.ToString(GetLocalResourceObject("msjNextNoFilter")));
                }

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFrombtnNextToPage2_Click{0}", Guid.NewGuid()), "setTimeout(function () {{ ActivateModalProgress(false); }}, 10);", true);
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

        public void FillData(DataTable absenteeimSelected)
        {
            var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            cboDocuments.SelectedValue = "";
            txtDocumentDescription.Text = "";
            Session[sessionKeyDocumentsSaved] = null;
            initialData = (AbsenteeismsInitialData)Session[sessionKeyAbsenteeismInitialDataResults];
            cboCaseState.DataSource = initialData.StateCases;
            cboCaseState.DataBind();
            cboCaseState.SelectedValue = Convert.ToString(absenteeimSelected.Rows[0]["estado_caso"]).Trim();
            cboCaseState.Enabled = false;

            List<AbsenteeismCauseEntity> causes = (List<AbsenteeismCauseEntity>)Session[sessionKeyAbsenteeismCausesDataResults];
            string causeCode = "";
            AbsenteeismCauseEntity cause = causes.Where(a => a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.CauseCodeAdamMapped == Convert.ToString(absenteeimSelected.Rows[0]["causa"]).Trim())).FirstOrDefault();
            if (cause != null)
            {
                causeCode = cause.CauseCode;
                cboCause.SelectedValue = causeCode;

                //Load Interest Group By Cause
                LoadInterestGroupByCause();
            }

            else
            {
                MensajeriaHelper.MostrarMensaje(Page
                     , TipoMensaje.Informacion
                     , string.Format(GetLocalResourceObject("msjNoCauseMapped").ToString(), Convert.ToString(absenteeimSelected.Rows[0]["causa"]).Trim()));
            }

            cboCause.Enabled = false;

            chkCloseDate.Checked = false;
            List<Cdu> stateCases = new List<Cdu>();

            if (GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
            {
                //HACK: DIFERENCIA REGIONAL #40
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

                //Si fecha cierre existe entonces el check se marca
                chkCloseDate.Checked = Convert.ToString(absenteeimSelected.Rows[0]["fecha_cierre"]) != "" && !(Convert.ToString(absenteeimSelected.Rows[0]["estado_caso"]).Trim() == "OP");
                divRelated.Visible = true;

                if (Convert.ToString(absenteeimSelected.Rows[0]["estado_caso"]).Trim() == "AP"
                    || Convert.ToString(absenteeimSelected.Rows[0]["estado_caso"]).Trim() == "RP")
                {
                    LoadRelatedCase(Convert.ToString(absenteeimSelected.Rows[0]["trabajador"]).Trim());
                    txtDescription.Enabled = false;
                }

                else
                {
                    txtDescription.Enabled = true;
                }
            }

            else
            {
                divAccidentCase.Visible = false;
                divRelated.Visible = false;
                divCloseDate.Visible = false;
                txtDescription.Text = "";
            }

            hdnClaveUnica.Value = Convert.ToString(absenteeimSelected.Rows[0]["clave_unica"]).Trim();
            hdnTrabajador.Value = Convert.ToString(absenteeimSelected.Rows[0]["trabajador"]);
            hdnAbsenteeismId.Value = Convert.ToString(absenteeimSelected.Rows[0]["AbsteeismId"]);
            lblIdAbsenteeism.Text = string.Concat("- #", hdnAbsenteeismId.Value);
            txtCompany.Text = Convert.ToString(absenteeimSelected.Rows[0]["compania"]);
            txtEmployeeName.Text = Convert.ToString(absenteeimSelected.Rows[0]["EmployeeName"]).Trim();

            txtDescription.Text = Convert.ToString(absenteeimSelected.Rows[0]["Description"]).Trim();

            txtRelatedCase.Text = Convert.ToString(absenteeimSelected.Rows[0]["au_relacionado"]);

            var incidentDate = Convert.ToDateTime(absenteeimSelected.Rows[0]["StartDate"]);
            var date = incidentDate.Month < 10 ? "0" + incidentDate.Month.ToString() : incidentDate.Month.ToString();
            date += incidentDate.Day < 10 ? "/0" + incidentDate.Day.ToString() : "/" + incidentDate.Day.ToString();
            date += "/" + incidentDate.Year.ToString();
            dtpIncidentDate.Text = date;
            
            var time = incidentDate.Hour < 10 ? "0" + incidentDate.Hour.ToString() : incidentDate.Hour.ToString();
            time += ":";
            time += incidentDate.Minute < 10 ? "0" + incidentDate.Minute.ToString() : incidentDate.Minute.ToString();
            tpcIncidentDateTime.Text = time;

            //Suspended
            var suspendedDate = Convert.ToDateTime(absenteeimSelected.Rows[0]["fecha_suspende"]);
            var suspendedWorkDate = suspendedDate.Month < 10 ? "0" + suspendedDate.Month.ToString() : suspendedDate.Month.ToString();
            suspendedWorkDate += suspendedDate.Day < 10 ? "/0" + suspendedDate.Day.ToString() : "/" + suspendedDate.Day.ToString();
            suspendedWorkDate += "/" + suspendedDate.Year.ToString();
            dtpSuspendWorkDate.Text = suspendedWorkDate;

            var SuspendedTime = suspendedDate.Hour < 10 ? "0" + suspendedDate.Hour.ToString() : suspendedDate.Hour.ToString();
            SuspendedTime += ":";
            SuspendedTime += suspendedDate.Minute < 10 ? "0" + suspendedDate.Minute.ToString() : suspendedDate.Minute.ToString();
            tpcSuspendWorkTime.Text = SuspendedTime;

            //EdnDate
            var endDate = Convert.ToDateTime(absenteeimSelected.Rows[0]["fecha_final"]);
            var finalEndDate = endDate.Month < 10 ? "0" + endDate.Month.ToString() : endDate.Month.ToString();
            finalEndDate += endDate.Day < 10 ? "/0" + endDate.Day.ToString() : "/" + endDate.Day.ToString();
            finalEndDate += "/" + endDate.Year.ToString();
            dtpFinalDate.Text = finalEndDate;

            EnableDisableUnitTime();

            if (Convert.ToString(absenteeimSelected.Rows[0]["unidad_tiempo"]) == "0")
            {
                cboTimeUnit.SelectedValue = "Days";
            }

            else
            {
                cboTimeUnit.SelectedValue = "Hours";
            }

            txtFinalHours.Text = Convert.ToString(absenteeimSelected.Rows[0]["tiempo"]).Replace(',', '.');

            FillDataAditionalInfo();
        }

        private void FillDataAditionalInfo()
        {
            Session[sessionKeyAbsenteeismAditionalInfoLoaded] = LoadEmptyFilterAbsenteeism();

            DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
            List<AbsenteeismCase> listAbsenteeism = objAbsenteeismCaseBll.GetAditionalInfoAbsenteeims(Convert.ToInt32(txtCompany.Text), Convert.ToInt32(hdnAbsenteeismId.Value));

            Session[sessionKeyAbsenteeismAditionalInfoLoaded] = listAbsenteeism;

            foreach (AbsenteeismCase absenteeism in (List<AbsenteeismCase>)Session[sessionKeyAbsenteeismAditionalInfoLoaded])
            {
                txtDetailedDescription.Text = absenteeism.DetailedDescription;

                cboInterestGroups.SetSelectedValueDole(string.IsNullOrEmpty(Convert.ToString(absenteeism.InterestGroup)) ? "0000" : Convert.ToString(absenteeism.InterestGroup).Trim());

                cboUnsafeActs.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.UnsafeActs)) ? "-1" : Convert.ToString(absenteeism.UnsafeActs).Trim();
                cboMaterialAgents.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.MaterialAgents)) ? "-1" : Convert.ToString(absenteeism.MaterialAgents).Trim();
                cboUnsafeConditions.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.UnsafeConditions)) ? "-1" : Convert.ToString(absenteeism.UnsafeConditions).Trim();
                cboEstablishments.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.Establishments)) ? "-1" : Convert.ToString(absenteeism.Establishments).Trim();
                cboDayFactor.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.DayFactors)) ? "-1" : Convert.ToString(absenteeism.DayFactors).Trim();
                cboPersonalFactor.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.PersonalFactors)) ? "-1" : Convert.ToString(absenteeism.PersonalFactors).Trim();
                cboJourneys.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.Journeys)) ? "-1" : Convert.ToString(absenteeism.Journeys).Trim();
                cboLaborMade.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.LaborMade)) ? "-1" : Convert.ToString(absenteeism.LaborMade).Trim();
                cboBodyParts.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.BodyParts)) ? "-1" : Convert.ToString(absenteeism.BodyParts).Trim();
                cboAccidentType.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.AccidentTypes)) ? "-1" : Convert.ToString(absenteeism.AccidentTypes).Trim();
                cboDiseaseType.SelectedValue = string.IsNullOrEmpty(Convert.ToString(absenteeism.DiseaseTypes)) ? "-1" : Convert.ToString(absenteeism.DiseaseTypes).Trim();
                
                //Dictionary<string, Document> documents = (Dictionary<string, Document>)Session[sessionKeyDocumentsSaved];
                Dictionary<string, Document> documents = new Dictionary<string, Document>();
                if (Session[sessionKeyDocumentsSaved] != null)
                {
                    documents = (Dictionary<string, Document>)Session[sessionKeyDocumentsSaved];
                }

                foreach (Document document in absenteeism.Documents)
                {
                    if (Session[sessionKeyDocumentsSaved] == null)
                    {
                        Session[sessionKeyDocumentsSaved] = new Dictionary<string, Document>();
                    }

                    string documentCode = document.DocumentCode;
                    string documentValue = document.DocumentValue;
                    Document doc = new Document(documentCode, string.Empty, documentValue);
                    if (documents != null && documents.ContainsKey(documentCode))
                    {
                        documents[documentCode] = doc;
                    }
                    else
                    {
                        documents.Add(documentCode, doc);
                    }

                    cboDocuments.SelectedValue = documentCode;
                    txtDocumentDescription.Text = documentValue;
                }

                Session[sessionKeyDocumentsSaved] = documents;
            }
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
                LoadInitialData();

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

                if (string.IsNullOrWhiteSpace(cboInterestGroups.SelectedValue))
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

                if ((string.Equals(cboCaseState.SelectedValue, "RP", StringComparison.CurrentCultureIgnoreCase) || string.Equals(cboCaseState.SelectedValue, "AP", StringComparison.CurrentCultureIgnoreCase))
                    && (string.IsNullOrWhiteSpace(txtRelatedCase.Text) || int.TryParse(txtRelatedCase.Text, out int integerTest) == false))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjRelatedCaseValidation")));
                }

                //HACK: DIFERENCIA REGIONAL #42
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
                        if (string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text.Length > 250)
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDescriptionValidation")));
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(txtDetailedDescription.Text) || txtDetailedDescription.Text.Length > 2000)
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDetailedDescriptionValidation")));
                }

                if (!GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
                {
                    tpcSuspendWorkTime.Text = "07:00";
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
                    DateTime.TryParseExact(string.Format("{0} {1}", dtpSuspendWorkDate.Text, tpcSuspendWorkTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false || DayOfWeek.Sunday.Equals(dateTest.DayOfWeek))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkValidation")));
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjSuspendWorkTimeValidation")));
                }

                else
                {
                    suspendWorkTest = DateTime.ParseExact(string.Format("{0} {1}", dtpSuspendWorkDate.Text, tpcSuspendWorkTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }

                if (string.IsNullOrWhiteSpace(dtpFinalDate.Text) || DateTime.TryParseExact(dtpFinalDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTest) == false)
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

                if (!string.IsNullOrWhiteSpace(txtNaturalDays.Text) && int.TryParse(txtNaturalDays.Text, out integerTest) != false && integerTest > 0 && integerTest <= 10000)
                {
                    naturalDaysTest = int.Parse(txtNaturalDays.Text);
                }

                if (!string.IsNullOrEmpty(txtFinalHours.Text) && txtFinalHours.Text.Contains(".") && !Session[Constants.cCulture].ToString().Equals("en-US"))
                {
                    txtFinalHours.Text = txtFinalHours.Text.Replace('.', ',');
                }

                if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) && (string.IsNullOrWhiteSpace(txtFinalHours.Text) || decimal.TryParse(txtFinalHours.Text, out decimal decimalTest) == false || !decimalTest.HasThisManyDecimalPlacesOrLess(3) || decimalTest <= 0 || decimalTest > 8))
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalHoursValidation")));
                    
                    txtFinalHours.Text = txtFinalHours.Text.Replace(',', '.');
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
                        if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) && disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                        {
                            errors = true;
                            errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDisabilitiesInDays")));
                        }
                    }

                    //HACK: DIFERENCIA REGIONAL #44
                    if (DivisionSession == 1) // CRC
                    {
                        List<string> disabilities = new List<string> { "IL", "IN", "IN3", "INCC", "INC2", "IPMT", "TACC" };
                        if (string.Equals(cboTimeUnit.SelectedValue, "Hours", StringComparison.CurrentCultureIgnoreCase) && disabilities.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
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

                        List<string> listNextDayAfterMidDay = new List<string> { "IL", "IN", "IN3", "INCC", "INC2", "IPMT", "TACC" };
                        if (suspendWorkTest.HasValue && listNextDayAfterMidDay.Contains(selectedCause.AbsenteeismCausesByDivision[0].CauseCodeAdamMapped))
                        {
                            if (suspendWorkTest.Value.Hour >= 12 && (finalDateTest.Value.Date == suspendWorkTest.Value.Date))
                            {
                                errors = true;
                                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFinalDateCannotSameSuspended")));
                            }
                        }
                    }
                }

                if (!errors)
                {
                    List<KeyValuePair<Company, Employee>> employeesAbsenteeism = new List<KeyValuePair<Company, Employee>>
                    {
                        new KeyValuePair<Company, Employee>(
                            new Company(Convert.ToString(txtCompany.Text)),
                            new Employee(Convert.ToString(txtEmployeeName),
                                        Convert.ToString(hdnClaveUnica.Value),//clave_unica
                                        Convert.ToString(hdnTrabajador.Value)) //trabajador
                            )
                    };

                    List<Document> documents =
                        savedWithDetails ? ((Dictionary<string, Document>)Session[sessionKeyDocumentsSaved]).Values.ToList<Document>() : new List<Document>();

                    string selectedUnsafeActs = cboUnsafeActs.SelectedIndex == 0 || !savedWithDetails ? "" : cboUnsafeActs.SelectedValue;

                    string selectedMaterialAgents = cboMaterialAgents.SelectedIndex == 0 || !savedWithDetails ? "" : cboMaterialAgents.SelectedValue;

                    string selectedUnsafeConditions = cboUnsafeConditions.SelectedIndex == 0 || !savedWithDetails ? "" : cboUnsafeConditions.SelectedValue;

                    string selectedEstablishments = cboEstablishments.SelectedIndex == 0 || !savedWithDetails ? "" : cboEstablishments.SelectedValue;

                    string selectedDayFactor = cboDayFactor.SelectedIndex == 0 || !savedWithDetails ? "" : cboDayFactor.SelectedValue;

                    string selectedPersonalFactor = cboPersonalFactor.SelectedIndex == 0 || !savedWithDetails ? "" : cboPersonalFactor.SelectedValue;

                    string selectedJourneys = cboJourneys.SelectedIndex == 0 || !savedWithDetails ? "" : cboJourneys.SelectedValue;

                    string selectedLaborMade = cboLaborMade.SelectedIndex == 0 || !savedWithDetails ? "" : cboLaborMade.SelectedValue;

                    string selectedBodyParts = cboBodyParts.SelectedIndex == 0 || !savedWithDetails ? "" : cboBodyParts.SelectedValue;

                    string selectedAccidentType = cboAccidentType.SelectedIndex == 0 || !savedWithDetails ? "" : cboAccidentType.SelectedValue;

                    string selectedDiseaseType = cboDiseaseType.SelectedIndex == 0 || !savedWithDetails ? "" : cboDiseaseType.SelectedValue;

                    DateTime suspendWork = DateTime.ParseExact(string.Format("{0} {1}", dtpSuspendWorkDate.Text, tpcSuspendWorkTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime incidentStart = DateTime.MinValue;

                    if (incidentDateTest != null)
                    {
                        incidentStart = DateTime.ParseExact(string.Format("{0} {1}", dtpIncidentDate.Text, tpcIncidentDateTime.Text), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }

                    AbsenteeismCase absenteeismCase = new AbsenteeismCase(
                        employeesAbsenteeism,
                        Convert.ToInt32(hdnAbsenteeismId.Value),
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
                        0, 
                        string.IsNullOrWhiteSpace(txtFinalHours.Text) ? 0 : Convert.ToDecimal(txtFinalHours.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)),
                        suspendWork, 
                        cboInterestGroups.SelectedValue,
                        string.IsNullOrWhiteSpace(txtRelatedCase.Text) ? 0 : Convert.ToInt32(txtRelatedCase.Text),
                        1,
                        UserHelper.GetCurrentFullUserName,
                        selectedUnsafeActs, selectedMaterialAgents, selectedUnsafeConditions, selectedEstablishments, selectedDayFactor, selectedPersonalFactor, selectedJourneys, selectedLaborMade, selectedBodyParts, selectedAccidentType, selectedDiseaseType,
                        documents,
                        0,
                        chkCloseDate.Checked); 

                    DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                    List<AbsenteeismID> absenteeismID = objAbsenteeismCaseBll.Save(absenteeismCase, Session["culture"] == null ? "es-CR" : Session["culture"].ToString());

                    pnlMainContent.Visible = true;
                    pnlAbsenteeismContent.Visible = false;
                    pnlDocumentsContent.Visible = false;
                    pnlResults.Visible = false;
                    DisplayResults(SearchAbsenteeismsPaged(1));
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

            finally
            {
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFrombtnSave_ServerClick", Guid.NewGuid()), "setTimeout(function () {{ ActivateModalProgress(false); }}, 10);", true);
            }
        }

        #endregion

        /// <summary>
        /// Event to enable ins case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsCaseEnable();
            EnableDisableUnitTime();
        }

        /// <summary>
        /// Enable Ins Case field
        /// </summary>
        private void InsCaseEnable()
        {
            List<Cdu> stateCases = new List<Cdu>();
            if (GetCausesWithCategoryAccident().Contains(cboCause.SelectedValue))
            {
                stateCases = initialData.StateCases.Where(x => x.CDUCode == "OP" || x.CDUCode == "AP" || x.CDUCode == "RP").ToList<Cdu>();

                var DivisionSession = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                //HACK: DIFERENCIA REGIONAL #48
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

                divRelated.Visible = true;
            }

            else
            {
                stateCases = initialData.StateCases.Where(x => x.CDUCode == "OP").ToList<Cdu>();
                divAccidentCase.Visible = false;
                divRelated.Visible = false;
                txtDescription.Text = "";
            }

            DataTable loadedEmployees = (DataTable)Session[sessionKeyEmployeesLoaded];

            DataTable selectedEmployees = loadedEmployees.AsEnumerable().Where(x => x.Field<bool>("IsSelected"))
                .GroupBy(x => x.Field<string>("EmployeeCode")).Select(x => x.FirstOrDefault()).CopyToDataTable();

            if (selectedEmployees.Rows.Count > 1)
            {
                stateCases.RemoveAll(x => x.CDUCode == "RP");
                stateCases.RemoveAll(x => x.CDUCode == "AP");
                stateCases.RemoveAll(x => x.CDUCode == "PP");
                stateCases.RemoveAll(x => x.CDUCode == "CL");
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
        private void EnableDisableUnitTime()
        {
            //Time unit
            if (cboTimeUnit.Items.Count > 0)
            {
                cboTimeUnit.Items.Clear();
            }

            List<AbsenteeismCauseEntity> causes2Forms = absenteeismCauses.Where(a => a.CauseCode == cboCause.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours && cbd.Days)).ToList();
            List<AbsenteeismCauseEntity> causesOnlyDays = absenteeismCauses.Where(a => a.CauseCode == cboCause.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Days)).ToList();
            List<AbsenteeismCauseEntity> causesOnlyHours = absenteeismCauses.Where(a => a.CauseCode == cboCause.SelectedValue && a.AbsenteeismCausesByDivision != null && a.AbsenteeismCausesByDivision.Exists(cbd => cbd.Hours)).ToList();

            if (causes2Forms.Count > 0)
            {
                //Time unit
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                cboTimeUnit.SelectedIndex = 0;
            }

            else if (causesOnlyHours.Count > 0)
            {
                //Time unit
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitHoursDescription")), "Hours"));
                cboTimeUnit.SelectedIndex = 0;
            }

            else
            {
                //Time unit
                cboTimeUnit.Items.Add(new ListItem(Convert.ToString(GetLocalResourceObject("TimeUnitDaysDescription")), "Days"));
                cboTimeUnit.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Search Absenteeisms
        /// </summary>
        private PageHelper<AbsenteeismCase> SearchAbsenteeismsPaged(int page = 1)
        {
            Session[sessionKeyAbsenteeismAditionalInfoLoaded] = null;
            Session[sessionKeyFilterAbsenteeismLoaded] = null;
            Session[sessionKeyFilterAbsenteeismLoaded] = LoadEmptyFilterAbsenteeism();

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvCausas.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvCausas.ClientID);
            string errorsMessages = string.Empty;

            var selectedPayrollValue = string.IsNullOrEmpty(hdfEmployeeSelectedPayroll.Value) ? "-1" : hdfEmployeeSelectedPayroll.Value;
            string[] payrolls = selectedPayrollValue.Split(',');

            if (payrolls.Contains("-1"))
            {
                payrolls = payrolls.Where(val => val != "-1").ToArray();
                cboPayroll.SelectedIndex = -1;
            }

            var dateType = cboDateType.SelectedValue; // 1 - Suspende / 2 - Inicia

            DateTime.TryParseExact(dtpStartDateFilter.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtpStartDate);
            DateTime.TryParseExact(dtpEndDateFilter.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtpEndDate);

            DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
            PageHelper<AbsenteeismCase> pageHelper = objAbsenteeismCaseBll.GetAbsenteeimsPaged(
                  string.Join(",", payrolls), txtEmployeeIdFilter.Text, txtEmployeeNameFilter.Text
                , txtIdAbsenteeimFilter.Text.ToString().Trim() == "" ? 0 : Convert.ToInt32(txtIdAbsenteeimFilter.Text.ToString().Trim())
                , txtDescriptionFilter.Text //# de Accidente
                , HttpContext.Current.User.Identity.Name
                , Convert.ToInt32(dateType)
                , dtpStartDate
                , dtpEndDate
                , sortExpression
                , sortDirection
                , page
                , Convert.ToInt32(cboPageSize.SelectedValue));



            pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults(PageHelper<AbsenteeismCase> pageHelper)
        {
            if (pageHelper != null)
            {
                AddAbsenteeismsFilterLoaded(pageHelper.ResultList);

                grvCausas.DataSource = Session[sessionKeyFilterAbsenteeismLoaded];
                grvCausas.DataBind();

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                Session[sessionKeyAbsenteeismResults] = pageHelper;

                if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            }

            else
            {
                htmlResultsSubtitle.InnerHtml = Convert.ToString("");
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

                    PagerUtil.SetActivePage(blstPager, Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value));
                    DisplayResults(SearchAbsenteeismsPaged(page));
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
        /// Search Absenteeisms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearchFilter_ServerClick(object sender, EventArgs e)
        {
            CommonFunctions.ResetSortDirection(Page.ClientID, grvCausas.ClientID);

            DisplayResults(SearchAbsenteeismsPaged(1));

            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnbtnSearchFilter_ServerClick{0}", Guid.NewGuid()), "setTimeout(function () {{ ActivateModalProgress(false);RestoreFilter(); }}, 10);", true);
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

        /// <summary>
        /// Delete case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll objAbsenteeismCaseBll = new DOLE.HRIS.Services.CR.Business.AbsenteeismCaseBll(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                objAbsenteeismCaseBll.DeleteAbsenteeism(Convert.ToInt32(txtCompany.Text),Convert.ToInt32(hdnAbsenteeismId.Value));

                //Delete files from Absenteeism
                ObjAbsenteeismFileBll.DeleteAllByAdamNumberCase(Convert.ToInt32(hdnAbsenteeismId.Value));

                pnlMainContent.Visible = true;
                pnlAbsenteeismContent.Visible = false;
                pnlDocumentsContent.Visible = false;
                pnlResults.Visible = false;

                DisplayResults(SearchAbsenteeismsPaged(1));
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
        /// Search employess
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearchEmployeesFilter_ServerClick(object sender, EventArgs e)
        {
            SearchEmployees(false);
        }

        /// <summary>
        /// Search employees
        /// </summary>
        /// <param name="directMode">Indicates if needs filters</param>
        private void SearchEmployees(bool directMode)
        {
            bool errors = false;
            string errorsMessages = string.Empty;

            if (hdfEmployeeSelectedPayroll.Value != "")
            {
                var selectedPayrollValue = hdfEmployeeSelectedPayroll.Value;
                string[] payrolls = selectedPayrollValue.Split(',');
                if (payrolls.Contains("-1"))
                {
                    payrolls = payrolls.Where(val => val != "-1").ToArray();
                }

                if (directMode)
                {
                    LoadEmployee(payrolls.ToList(), string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue);
                }

                else
                {
                    DateTime.TryParseExact(dtpStartDateFilter.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtpStartDate);
                    DateTime.TryParseExact(dtpEndDateFilter.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtpEndDate);

                    LoadEmployee(payrolls.ToList(), txtEmployeeIdFilter.Text, txtEmployeeNameFilter.Text, dtpStartDate, dtpEndDate);
                }
            }

            else
            {
                errors = true;
                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjPayrollValidation")));
            }

            if (errors)
            {
                errorsMessages = string.Format("{0}\\n{1}", Convert.ToString(GetLocalResourceObject("verifyErrors")), errorsMessages);
                errorsMessages = errorsMessages.Replace("\\n", "<br/>");
                errorsMessages = errorsMessages.Replace("\n", "");
                errorsMessages = errorsMessages.Replace("\r", "");

                MensajeriaHelper.MostrarMensaje(btnAddFile, 
                    TipoMensaje.Error, 
                    Convert.ToString(errorsMessages));
            }
        }

        /// <summary>
        /// Open modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFileButton_ServerClick(object sender, EventArgs e)
        {
            fupAbsenteeismFile.Dispose();
            cboDocumentsAbsenteeism.SelectedIndex = 0;

            rptFiles.DataSource = LoadEmptyFiles();
            rptFiles.DataBind();
            txtModalEmployeeId.Text = "";
            txtModalAdamCase.Text = "";
            txtModalCompany.Text = "";
            txtModalEmployeeName.Text = "";
            txtModalCause.Text = "";
            txtModalInsCase.Text = "";
            Session["ArchivosTemporales"] = null;

            foreach (GridViewRow item in grvCausas.Rows)
            {
                CheckBox chbEmployee = (CheckBox)item.FindControl("chbEmployeeFilter");
                HiddenField hdnAbsteeismId = (HiddenField)item.FindControl("hdnAbsteeismId");
                HiddenField hdnCauseFile = (HiddenField)item.FindControl("hdnCauseFile");

                if (chbEmployee.Checked)
                {
                    DataTable absenteeimsFilter = (DataTable)Session[sessionKeyFilterAbsenteeismLoaded];
                    DataTable absenteeimSelected = absenteeimsFilter.AsEnumerable().Where(x => x.Field<string>("AbsteeismId") == hdnAbsteeismId.Value && x.Field<string>("CauseDescription") == hdnCauseFile.Value).CopyToDataTable();
                    
                    if (absenteeimSelected.Rows.Count == 1)
                    {
                        FillDataModal(absenteeimSelected, hdnCauseFile.Value);
                    }

                    Session[sessionKeyAbsenteeismFiles] = LoadEmptyFiles();

                    List<FileEntity> absenteeismFiles = ObjAbsenteeismFileBll.ListByKey(Convert.ToInt32(hdnAbsteeismId.Value), 
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                    AddAbsenteeismsFilesLoaded(absenteeismFiles);

                    rptFiles.DataSource = Session[sessionKeyAbsenteeismFiles];
                    rptFiles.DataBind();
                    break;
                }
            }
        }

        /// <summary>
        /// Fill form modal
        /// </summary>
        /// <param name="absenteeimSelected"></param>
        /// <param name="causeDescription"></param>
        private void FillDataModal(DataTable absenteeimSelected, string causeDescription)
        {
            //HACK: DIFERENCIA REGIONAL #49 #50
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

                divModalInsCase.Visible = true;
            }

            else
            {
                divModalInsCase.Visible = false;
            }

            txtModalEmployeeId.Text = Convert.ToString(absenteeimSelected.Rows[0]["trabajador"]).Replace(" ", "");
            txtModalAdamCase.Text = Convert.ToString(absenteeimSelected.Rows[0]["AbsteeismId"]);
            txtModalCompany.Text = Convert.ToString(absenteeimSelected.Rows[0]["compania"]);
            txtModalEmployeeName.Text = Convert.ToString(absenteeimSelected.Rows[0]["EmployeeName"]);
            txtModalCause.Text = causeDescription;
            txtModalInsCase.Text = Convert.ToString(absenteeimSelected.Rows[0]["Description"]);
        }

        /// <summary>
        /// Load Empty Files
        /// </summary>
        /// <returns></returns>
        private DataTable LoadEmptyFiles()
        {
            DataTable files = new DataTable();
            files.Columns.Add("IdFile", typeof(int));
            files.Columns.Add("Company", typeof(string));
            files.Columns.Add("AdamNumberCase", typeof(int));
            files.Columns.Add("GeographicDivisionCode", typeof(string));
            files.Columns.Add("DivisionCode", typeof(int));
            files.Columns.Add("DocumentTypeDescription", typeof(string));
            files.Columns.Add("File");
            files.Columns.Add("FileName", typeof(string));
            files.Columns.Add("FileExtension", typeof(string));

            return files;
        }

        /// <summary>
        /// Load Files
        /// </summary>
        /// <param name="absenteeismFiles"></param>
        public void AddAbsenteeismsFilesLoaded(List<FileEntity> absenteeismFiles)
        {
            if (Session[sessionKeyAbsenteeismFiles] == null)
            {
                Session[sessionKeyAbsenteeismFiles] = LoadEmptyFiles();
            }

            DataTable loadedAbsenteeismFiles = (DataTable)Session[sessionKeyAbsenteeismFiles];
            int i = 0;

            foreach (FileEntity absenteeismFile in absenteeismFiles)
            {
                DataRow row = loadedAbsenteeismFiles.NewRow();

                row.ItemArray = new object[] {
                    absenteeismFile.IdFile, 
                    absenteeismFile.Company, 
                    absenteeismFile.AdamNumberCase, 
                    absenteeismFile.GeographicDivisionCode, 
                    absenteeismFile.DivisionCode, 
                    absenteeismFile.DocumentTypeDescription, 
                    absenteeismFile.File, 
                    absenteeismFile.FileName + "." + absenteeismFile.FileExtension, 
                    absenteeismFile.FileExtension
                };

                loadedAbsenteeismFiles.Rows.InsertAt(row, i++);
            }

            Session[sessionKeyAbsenteeismFiles] = loadedAbsenteeismFiles;
        }

        protected void BtnAddFile_ServerClick(object sender, EventArgs e)
        {
            bool errors = false;
            string errorsMessages = string.Empty;

            if (cboDocumentsAbsenteeism.SelectedValue == "")
            {
                errors = true;
                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjDocumentTypeValidation")));
            }

            if (Session["ArchivosTemporales"] != null)
            {
                List<FileEntity> archivos = (List<FileEntity>)Session["ArchivosTemporales"];

                if (archivos[0].File == null)
                {
                    errors = true;
                    errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFileValidation")));
                }

                if (!errors)
                {
                    FileEntity objectFile = archivos[0];
                    objectFile.DocumentTypeDescription = cboDocumentsAbsenteeism.SelectedItem.Text;
                    objectFile.DocumentTypeId = Convert.ToInt32(cboDocumentsAbsenteeism.SelectedValue);
                    GeneralConfigurationEntity uploadSize = ObjGeneralConfigurationsBll.ListByCode(HrisEnum.GeneralConfigurations.UploadFileSize);

                    if ((objectFile.File.Length / 1024f) / 1024 < Convert.ToDouble(uploadSize.GeneralConfigurationValue))
                    {
                        ObjAbsenteeismFileBll.Add(objectFile);
                        Session[sessionKeyAbsenteeismFiles] = LoadEmptyFiles();

                        List<FileEntity> absenteeismFiles = ObjAbsenteeismFileBll.ListByKey(Convert.ToInt32(txtModalAdamCase.Text),
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                        AddAbsenteeismsFilesLoaded(absenteeismFiles);

                        rptFiles.DataSource = Session[sessionKeyAbsenteeismFiles];
                        rptFiles.DataBind();

                        Session["ArchivosTemporales"] = null;
                    }

                    else
                    {
                        errors = true;
                        errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(string.Format(GetLocalResourceObject("msjFileSize").ToString(), uploadSize.GeneralConfigurationValue)));
                    }
                }
            }

            else
            {
                errors = true;
                errorsMessages = string.Format("{0}\\n- {1}", errorsMessages, Convert.ToString(GetLocalResourceObject("msjFileValidation")));
            }

            if (errors)
            {
                errorsMessages = string.Format("{0}\\n{1}", Convert.ToString(GetLocalResourceObject("verifyErrors")), errorsMessages);
                errorsMessages = errorsMessages.Replace("\\n", "<br/>");
                errorsMessages = errorsMessages.Replace("\n", "");
                errorsMessages = errorsMessages.Replace("\r", "");

                MensajeriaHelper.MostrarMensaje(btnAddFile, 
                    TipoMensaje.Error, 
                    Convert.ToString(errorsMessages));
            }
        }

        public string DeleteFile(object idFile)
        {
            return "DeleteFile(this," + idFile + ");";
        }

        public string DownloadFile(object idFile)
        {
            return "DownloadFile(this," + idFile + ");";
        }

        protected void BtnDeleteFile_ServerClick(object sender, EventArgs e)
        {
            var idFileToDelete = Convert.ToInt32(hdnIdFileToDelete.Value);
            FileEntity absenteeismFile = new FileEntity(idFileToDelete);

            ObjAbsenteeismFileBll.Delete(absenteeismFile);

            Session[sessionKeyAbsenteeismFiles] = LoadEmptyFiles();

            List<FileEntity> absenteeismFiles = ObjAbsenteeismFileBll.ListByKey(Convert.ToInt32(txtModalAdamCase.Text), 
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

            AddAbsenteeismsFilesLoaded(absenteeismFiles);

            rptFiles.DataSource = Session[sessionKeyAbsenteeismFiles];
            rptFiles.DataBind();
        }

        protected void CboDocumentsAbsenteeism_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ArchivosTemporales"] = null;
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

        protected void BtnCleanFilters_Click(object sender, EventArgs e)
        {
            txtEmployeeIdFilter.Text = "";
            txtEmployeeNameFilter.Text = "";
            dtpStartDateFilter.Text = "";
            dtpEndDateFilter.Text = "";
            hdfEmployeeSelectedPayroll.Value = "";
            txtIdAbsenteeimFilter.Text = "";
            txtDescriptionFilter.Text = "";
            cboPayroll.ClearSelection();
            grvCausas.DataSource = null;
            grvCausas.DataBind();

            DisplayResults(null);
            Session[sessionKeyFilterAbsenteeismLoaded] = null;

            PagerUtil.SetupPager(blstPager, 0, 0);
        }

        /// <summary>
        /// Load Interest Group
        /// </summary>
        private void LoadInterestGroupByCause()
        {
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
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvCausas_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((grvCausas.ShowHeader && grvCausas.Rows.Count > 0) || (grvCausas.ShowHeaderWhenEmpty))
                {
                    //Force GridView to use <thead> instead of <tbody>
                    grvCausas.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                if (grvCausas.ShowFooter && grvCausas.Rows.Count > 0)
                {
                    //Force GridView to use <tfoot> instead of <tbody>
                    grvCausas.FooterRow.TableSection = TableRowSection.TableFooter;
                }
            }
        }

        /// <summary>
        /// Enent to hadle post render to grv.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvCausas_DataBound(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromGridDataBound{0}", Guid.NewGuid()), "ReturnFromGridDataBound();", true);
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvCausas_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvCausas.ClientID, e.SortExpression);

            PageHelper<AbsenteeismCase> pageHelper = SearchAbsenteeismsPaged(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults(pageHelper);
        }
    }
}