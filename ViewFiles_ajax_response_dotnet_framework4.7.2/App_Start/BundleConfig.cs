using System.Web;
using System.Web.Optimization;

namespace ViewFiles_ajax_response_dotnet_framework4._7._2
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(

                        "~/Scripts/jquery-3.4.1.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/select2-4.1.0-rc.0/js/select2.min.js"

                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       "~/select2-4.1.0-rc.0/css/select2.min.css",
                      "~/Content/site.css"));
        }
    }
}
