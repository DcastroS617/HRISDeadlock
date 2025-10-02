using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Configuration
{
    public partial class AssingEmployeeLabor : System.Web.UI.Page
    {
        [Dependency]
        public IEmployeeByLaborBll ObjIEmployeeByLaborBll { get; set; }

        [Dependency]
        public ILaborBll ObjILaborBll { get; set; }

        public List<EmployeeByLaborEntity> AssingEmployeeLaborResults
        {
            get
            {
                if (Session["AssingEmployeeLabor-AssingEmployeeLaborResults"] != null)
                {
                    return Session["AssingEmployeeLabor-AssingEmployeeLaborResults"] as List<EmployeeByLaborEntity>;
                }

                return new List<EmployeeByLaborEntity>();
            }

            set { Session["AssingEmployeeLabor-AssingEmployeeLaborResults"] = value; }
        }

        public EmployeeByLaborEntity EmployeeByLaborResults
        {
            get
            {
                if (Session["AssingEmployeeLabor-EmployeeByLaborResults"] != null)
                {
                    return Session["AssingEmployeeLabor-EmployeeByLaborResults"] as EmployeeByLaborEntity;
                }

                return new EmployeeByLaborEntity();
            }

            set { Session["AssingEmployeeLabor-EmployeeByLaborResults"] = value; }
        }

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
                    AssingEmployeeLaborResults = new List<EmployeeByLaborEntity>();
                    var DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                    var GeografiaGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                    LaborIdAsignar.Items.Add(new ListItem(GetLocalResourceObject("lblaEliminarCbo").ToString(), "-1"));
                    LaborIdAsignar.Items.AddRange(ObjILaborBll.LaborList(DivisionCodeGlobal));

                    CompaniesFilter.Items.Add(new ListItem("", ""));
                    CompaniesFilter.Items.AddRange(ObjIEmployeeByLaborBll.CompaniesListEnableByDivision(DivisionCodeGlobal));

                    PayrollClassFilter.Items.Add(new ListItem("", ""));
                    PayrollClassFilter.Items.AddRange(ObjIEmployeeByLaborBll.NominalClassListEnableByDivision(GeografiaGlobal, null));

                    CostCenterFilter.Items.AddRange(ObjIEmployeeByLaborBll.CostCenterListEnableByDivision(GeografiaGlobal, null, null));

                    PositionFilter.Items.AddRange(ObjIEmployeeByLaborBll.PositionsListEnabled(null, null));
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
                var DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                var GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                EmployeeByLaborResults = new EmployeeByLaborEntity
                {
                    DivisionCode = DivisionCodeGlobal,
                    GeographicDivisionCode = GeographicDivisionCode,
                    EmployeeCode = string.IsNullOrEmpty(EmployeeCodeFilter.Value) ? null : EmployeeCodeFilter.Value,
                    EmployeeName = string.IsNullOrEmpty(EmployeeNameFilter.Value) ? null : EmployeeNameFilter.Value,
                    CompanyCode = string.IsNullOrEmpty(CompaniesFilter.SelectedValue) ? null : CompaniesFilter.SelectedValue,
                    PayrollClassCode = string.IsNullOrEmpty(PayrollClassFilter.SelectedValue) ? null : PayrollClassFilter.SelectedValue,
                    CostsCenterCode = string.IsNullOrEmpty(CostCenterFilter.Value) ? null : CostCenterFilter.Value,
                    PositionCode = string.IsNullOrEmpty(PositionFilter.Value) ? null : PositionFilter.Value,
                    RecruitmentDATE = string.IsNullOrEmpty(RecruitmentDate.Text) ? null : ToDateEN(RecruitmentDate.Text),
                };

                AssingEmployeeLaborResults = ObjIEmployeeByLaborBll.EmployeeByLaborWithFilter(EmployeeByLaborResults, null, null);
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
        /// Handles the btnSearchRefresh click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearchRefresh_ServerClick(object sender, EventArgs e)
        {
            try
            {
                AssingEmployeeLaborResults = ObjIEmployeeByLaborBll.EmployeeByLaborWithFilter(EmployeeByLaborResults, null, null);
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

        #region Companies Payroll

        /// <summary>
        /// Handles the ddlCompaniesPayroll onchange event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void DdlOnchange_CompaniesPayroll(object sender, EventArgs e)
        {
            try
            {
                var GeografiaGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                CostCenterFilter.Items.Clear();
                int? Companies = string.IsNullOrEmpty(CompaniesFilter.SelectedValue) ? (int?)null : int.Parse(CompaniesFilter.SelectedValue);

                PayrollClassFilter.Items.Clear();
                PayrollClassFilter.Items.Add(new ListItem("", ""));
                PayrollClassFilter.Items.AddRange(ObjIEmployeeByLaborBll.NominalClassListEnableByDivision(GeografiaGlobal, Companies));

                string Payroll = string.IsNullOrEmpty(PayrollClassFilter.SelectedValue) ? null : PayrollClassFilter.SelectedValue;
                CostCenterFilter.Items.Add(new ListItem("", ""));
                CostCenterFilter.Items.AddRange(ObjIEmployeeByLaborBll.CostCenterListEnableByDivision(GeografiaGlobal, Companies, Payroll));

                PositionFilter.Items.Clear();
                PositionFilter.Items.Add(new ListItem("", ""));
                PositionFilter.Items.AddRange(ObjIEmployeeByLaborBll.PositionsListEnabled(Companies, Payroll));
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
        /// Handles the ddlCompaniesPayroll onchange event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void DdlOnchange_CompaniesPayroll2(object sender, EventArgs e)
        {
            try
            {
                var GeografiaGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                CostCenterFilter.Items.Clear();
                int? Companies = string.IsNullOrEmpty(CompaniesFilter.SelectedValue) ? (int?)null : int.Parse(CompaniesFilter.SelectedValue);

                string Payroll = string.IsNullOrEmpty(PayrollClassFilter.SelectedValue) ? null : PayrollClassFilter.SelectedValue;
                CostCenterFilter.Items.Add(new ListItem("", ""));
                CostCenterFilter.Items.AddRange(ObjIEmployeeByLaborBll.CostCenterListEnableByDivision(GeografiaGlobal, Companies, Payroll));

                PositionFilter.Items.Clear();
                PositionFilter.Items.Add(new ListItem("", ""));
                PositionFilter.Items.AddRange(ObjIEmployeeByLaborBll.PositionsListEnabled(Companies, Payroll));
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

        #endregion

        #region Private methods

        /// <summary>
        /// Return one string with format date specific
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public DateTime? ToDateEN(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                DateTimeFormatInfo dateTimeFormatInfo = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;

                string[] formats = { "MM/dd/yyyy", "MM/d/yyyy", "M/dd/yyyy", "M/d/yyyy" };

                DateTime.TryParseExact(dateString, formats, dateTimeFormatInfo, DateTimeStyles.None, out DateTime Date);
                return Date;
            }

            else
            {
                return null;
            }
        }

        #endregion

        #region WebMethod

        [WebMethod(EnableSession = true)]
        public static DbaEntity AsignarLabor(EmployeeByLaborEntity obj, string[] Employees)
        {
            try
            {
                obj.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                obj.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                var page = new AssingEmployeeLabor
                {
                    ObjIEmployeeByLaborBll = new EmployeeByLaborBll(new EmployeeByLaborDal())
                };

                var result = page.ObjIEmployeeByLaborBll.EmployeeByLaborAdd(obj,
                    Employees.Select(r => new TypeTableMultipleIdDto() { Code = r }).ToList().ToDataTableGet());

                if (result.ErrorNumber == 0)
                {
                    result.ErrorMessage = " " + Employees.Length;
                }

                return result;
            }

            catch (Exception ex)
            {
                return new DbaEntity { ErrorNumber = ex.HResult, ErrorMessage = "Hubo un problema al asignar el labor" };
            }
        }

        [WebMethod(EnableSession = true)]
        public static List<EmployeeByLaborEntity> ExportarLabor(EmployeeByLaborEntity obj)
        {
            try
            {
                obj.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                obj.GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                var page = new AssingEmployeeLabor
                {
                    ObjIEmployeeByLaborBll = new EmployeeByLaborBll(new EmployeeByLaborDal())
                };

                return page.ObjIEmployeeByLaborBll.EmployeeByLaborWithFilter(obj, null, null);
            }

            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}