using System.Web;
using System.Web.Optimization;

namespace FFR
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/baguetteBox.min.js",
                        "~/Scripts/contact-form-script.js",
                        "~/Scripts/custom.js",
                        "~/Scripts/images-loded.min.js",
                        "~/Scripts/isotope.min.js",
                        "~/Scripts/jquery.mapify.js",
                        "~/Scripts/jquery.superslides.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/animate.css",
                      "~/Content/baguetteBox.min.css",
                      "~/Content/classic.css",
                      "~/Content/classic.date.css",
                      "~/Content/classic.time.css",
                      "~/Content/custom.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/responsive.css",
                      "~/Content/style.css",
                      "~/Content/superslides.css"
                      ));
        }
    }
}
