using System;
using HRISWeb.Shared;
using System.Globalization;
using System.Threading;
using System.Configuration;
using DOLE.HRIS.Shared.Entity;
using Unity.Attributes;
using DOLE.HRIS.Application.Business.Interfaces;

namespace HRISWeb.SocialResponsability.Report
{
    public partial class ReportGeneralSurvey : System.Web.UI.Page
    {
        private const string reportServerUrl = "ReportServerUrl";
        private const string generalSurvey = "GeneralSurvey";

        [Dependency]
        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
            int division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            string serverUrl = ObjGeneralParametersBll.ListByFilter(reportServerUrl);
            string report = ObjGeneralParametersBll.ListByFilter(generalSurvey);

            string urlReport = $"{serverUrl}{report}";

            Iframe1.Attributes["src"] = $"{urlReport}&UserCode=150&DivisionDefault={division}&rs:embed=true";
        }
    }
}