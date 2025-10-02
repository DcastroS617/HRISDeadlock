using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Linq;
using System.Web.UI;
using Unity.Attributes;

namespace HRISWeb.Training.Report
{
    public partial class ReportTraining : System.Web.UI.Page
    {
        [Dependency]
        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }

        private const string reportServerUrl = "ReportServerUrl";

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
            var codigoReporte = Request.QueryString.Get("code");
            var informacionAdicional = Request.QueryString.Get("add");
            var urlReporte = Request.QueryString.Get("rpt");

            if (informacionAdicional.Equals("1"))
            {
                var nombreArchivo = from f in Directory.GetFiles(Server.MapPath("~/Training/Report/AddInformation"))
                                    where Path.GetFileName(f).Contains(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode) && Path.GetExtension(f) != ".htm"
                                    select Path.GetFileNameWithoutExtension(f);

                if (nombreArchivo.Any())
                {
                    lnkbtnAddInformation.Enabled = true;
                    lnkbtnAddInformation.Visible = true;
                    lnkbtnAddInformation.OnClientClick = $"OpenHelpDialog('{AddInformation(codigoReporte)}');";
                }
            }

            var parametrosAdicionales = string.Empty;

            if (codigoReporte == "RCMCI" || codigoReporte == "RTPM")
            {
                parametrosAdicionales += $"&UserCode={SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation).UserCode}";
            }

            parametrosAdicionales += $"&GeographicDivisionCode={SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode}&Language={Session[Constants.cCulture]}&rs:embed=true";

            tituloReporte.InnerHtml = GetLocalResourceObject(codigoReporte).ToString();
            TituloPantalla.InnerText = GetLocalResourceObject(codigoReporte).ToString();

            var urlString = ObjGeneralParametersBll.ListByFilter(reportServerUrl) + HttpUtility.UrlEncode(urlReporte) + parametrosAdicionales;

            Iframe1.Attributes["src"] = urlString;
        }

        /// <summary>
        /// Method for add information the reports
        /// </summary>
        /// <param name="codigoReporte">Código reporte</param>
        /// <returns></returns>
        private string AddInformation(string codigoReporte)
        {
            string pageName = Request.Url.Segments[Request.Url.Segments.Length - 1];
            pageName = pageName.Replace("ReportTraining", "AddInformation");

            string reemplazo = $"{codigoReporte}.{SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode}.";
            pageName = pageName.Replace(".", reemplazo);

            return $"AddInformation/{pageName}.{Thread.CurrentThread.CurrentCulture}.htm";
        }
    }
}