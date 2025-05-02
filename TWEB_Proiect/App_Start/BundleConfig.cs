using System.Web;
using System.Web.Optimization;

namespace TWEB_Proiect
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

            /* 3 ── Modernizr (only in DEV; remove in production) --------------- */
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                        .Include("~/Scripts/modernizr-*"));

            /* 4 ── Bootstrap **and Popper**  ----------------------------------- */
            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
            .Include("~/Content/js/jquery-3.1.1.min.js",
                     "~/Content/js/bootstrap.min.js",
                     "~/Content/js/editor.js"));


            /* 5 ── Styles ------------------------------------------------------- */
            bundles.Add(new StyleBundle("~/Content/css")
                        .Include("~/Content/css/bootstrap.css",
                                 "~/Content/css/font-awesome.min.css",
                                 "~/Content/css/animate.css",
                                 "~/Content/css/editor.css",
                                 "~/Content/css/loginstyle.css",
                                 "~/Content/css/responsive.css",
                                 "~/Content/css/style.css"));   // keep site-specific last

            /* 6 ── turn optimisation OFF while debugging ----------------------- */
            BundleTable.EnableOptimizations = false;
        }

    }
}
