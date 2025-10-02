using System;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using Unity.Attributes;
using DOLE.HRIS.Application.Business.Interfaces;

namespace HRISWeb.Absenteeism.Report
{
    public partial class ReportAbsenteeism1 : System.Web.UI.Page
    {
        [Dependency]
        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }

        private const string reportServerUrl = "ReportServerUrl";

        protected void Page_Load(object sender, EventArgs e)
        {
            string usuario = HttpContext.Current.User.Identity.Name;
            var division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
            var code = Request.QueryString.Get("code");
            var nombreReporte = GetLocalResourceObject(code).ToString();
                
                var url = Request.QueryString.Get("rpt");
                var embededText = "&rs:embed=true";
                url = url.Replace(embededText, "");
                url += string.Format("&Division={0}&Usuario={1}{2}", division, usuario, embededText);

                tituloReporte.InnerHtml = nombreReporte;
                TituloPantalla.InnerText = nombreReporte;

                var urlString = ObjGeneralParametersBll.ListByFilter(reportServerUrl) + url;

                Iframe1.Attributes["src"] = urlString;

        }

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
    }
}