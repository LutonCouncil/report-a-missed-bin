using System.Web;
using System.Web.Optimization;

namespace Waste_Bin_Collections
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/lbc-custom").Include(
                      "~/Scripts/retina.min.js",
                      "~/Scripts/ReView.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-core").Include(
                      "~/Scripts/angular.min.js",
                      "~/Scripts/angular-resource.min.js"
                      ));
            
            bundles.Add(new ScriptBundle("~/bundles/angular-app").Include(
                      "~/Scripts/app/controllers/BinController.js",
                      "~/Scripts/app/app.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/custom.css",
                       "~/Content/project.css"));
        }
    }
}
