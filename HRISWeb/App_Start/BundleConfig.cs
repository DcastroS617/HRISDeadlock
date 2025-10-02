using System.Web.Optimization;
using System.Web.UI;

namespace HRISWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/HRISPlugins").Include(
                "~/Scripts/jquery-3.3.1.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/DateTimePickerScript.js",
                "~/Scripts/Utilities.js",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/bootstrap-confirmation.min.js",
                "~/Scripts/bootstrap-treeview.js",
                "~/Scripts/bootstrap-toggle.min.js",
                "~/Scripts/mensajeria-util.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/additional-methods.min.js",
                "~/Scripts/bootstrap-select.min.js",
                "~/Scripts/i18n/defaults-es_ES.min.js",
                "~/Scripts/inputmask/min/inputmask/inputmask.min.js",
                "~/Scripts/inputmask/min/inputmask/jquery.inputmask.min.js",
                "~/Scripts/inputmask/min/inputmask/inputmask.extensions.min.js",
                "~/Scripts/inputmask/min/inputmask/inputmask.numeric.extensions.min.js",
                "~/Scripts/inputmask/min/inputmask/inputmask.date.extensions.min.js",
                "~/Scripts/fileinput.min.js",
                "~/Content/bootstrap-fileinput/themes/explorer/theme.min.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;

            ScriptManager.ScriptResourceMapping.AddDefinition("respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });
        }
    }
}