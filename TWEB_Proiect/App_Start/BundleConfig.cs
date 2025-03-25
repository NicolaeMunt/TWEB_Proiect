using System.Web;
using System.Web.Optimization;

namespace TWEB_Proiect
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // 1. jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 2. jQuery Validate
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 3. Modernizr
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // 4. Bootstrap JS 
            //    (If bootstrap.js is in Scripts, use "~/Scripts/bootstrap.js" 
            //     If it's in Content/js, use "~/Content/js/bootstrap.js")
            bundles.Add(new ScriptBundle("~/Content/js").Include(
                        "~/Content/js/bootstrap.js"));
            // OR
            // bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //             "~/Content/js/bootstrap.js"));

            // 5. CSS Bundle
            //    (Paths MUST match your actual file locations in Content/css)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/style.css",
                "~/Content/css/bootstrap.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/animate.css",
                "~/Content/css/editor.css",
                "~/Content/css/loginstyle.css",
                "~/Content/css/responsive.css"
            ));

            // Optional: Enable or disable optimizations
            // (Set to false for debugging, true for production)
            BundleTable.EnableOptimizations = false;
        }
    }
}
