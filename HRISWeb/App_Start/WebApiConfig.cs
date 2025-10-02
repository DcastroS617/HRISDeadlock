using System.Web.Http;
using System.Web.Http.Cors;

namespace HRISWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes se va agregar por el motivo de problemas croos orign de consulta para la syncronizacion
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true };
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
               name: "ApiEchoSurvey",
                routeTemplate: "api/Surveys/Echo/{echo}",
                defaults: new
                {
                    controller = "Surveys",
                    action = "Echo",
                }
           );

            config.Routes.MapHttpRoute(
                name: "SurveysApi",
                routeTemplate: "api/Surveys/Save",
                defaults: new
                {
                    controller = "Surveys",
                    action = "Save"
                }
           );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}