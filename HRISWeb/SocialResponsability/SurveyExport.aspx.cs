using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using Unity.Attributes;
using static System.String;

namespace HRISWeb.SocialResponsability
{
    public partial class SurveyExport : System.Web.UI.Page
    {
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveyBll { get; set; }

        [Dependency]
        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }

        private const string reportServerUrl = "ReportServerUrl";
        private const string surveyExport = "SurveyExport";

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

        public string UrlReport { get; set; }

        public string UserCodeSession { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string serverUrl = ObjGeneralParametersBll.ListByFilter(reportServerUrl);
            string report = ObjGeneralParametersBll.ListByFilter(surveyExport);
            UrlReport = $"{serverUrl}{report}";

            UserCodeSession = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation).UserCode.ToString();
            
            if (!IsPostBack)
            {
                LoadDivisions();
                LoadCompany();
            }
        }

        protected void cboDivisionCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCompany();
        }

        private void LoadDivisions()
        {
            try
            {
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                var divisionList = objSurveyBll.RptCboSurveyExportDivision(currentUser.UserCode);

                cboDivisionCode.DataValueField = "DCParam";
                cboDivisionCode.DataTextField = "DivisionName";
                cboDivisionCode.DataSource = divisionList;
                cboDivisionCode.DataBind();
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

        private void LoadCompany()
        {
            try
            {
                var division = IsNullOrEmpty(cboDivisionCode.SelectedValue) ? (int?)null : int.Parse(cboDivisionCode.SelectedValue);
                UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                var companyList = objSurveyBll.RptCboSurveyExportCompany(division, currentUser.UserCode);

                cboCompany.DataValueField = "CompanyID";
                cboCompany.DataTextField = "CompanyName";
                cboCompany.DataSource = companyList;
                cboCompany.DataBind();
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
    }
}