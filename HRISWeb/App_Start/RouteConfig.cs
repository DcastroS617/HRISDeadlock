using Microsoft.AspNet.FriendlyUrls;
using System.Web.Routing;

namespace HRISWeb
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings
            {
                AutoRedirectMode = RedirectMode.Off
            };

            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute(
               routeName: "TrainingPurposeList",
               routeUrl: "Training/Maintenances/TrainingPurpose.aspx",
               physicalFile: "~/Training/Maintenances/TrainingPrograms.aspx");

            routes.MapPageRoute(
             routeName: "EmployeeLaborList",
             routeUrl: "Configuration/AssignEmployeeLabor.aspx",
             physicalFile: "~/Configuration/AssingEmployeeLabor.aspx");
        }
    }
}
