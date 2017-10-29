using System.Web;
using System.Web.Optimization;

namespace FirstREST
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                       "~/Content/vendors/jquery/dist/jquery.min.js",
                       "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js",
                       "~/Content/vendors/fastclick/lib/fastclick.js",
                       "~/Content/vendors/nprogress/nprogress.js",
                       "~/Content/vendors/Chart.js/dist/Chart.min.js",
                       "~/Content/vendors/gauge.js/dist/gauge.min.js",
                       "~/Content/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
                       "~/Content/vendors/iCheck/icheck.min.js",
                       "~/Content/vendors/skycons/skycons.js",
                       "~/Content/vendors/Flot/jquery.flot.js",
                       "~/Content/vendors/Flot/jquery.flot.pie.js",
                       "~/Content/vendors/Flot/jquery.flot.time.js",
                       "~/Content/vendors/Flot/jquery.flot.stack.js",
                       "~/Content/vendors/Flot/jquery.flot.resize.js",
                       "~/Content/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                       "~/Content/vendors/flot-spline/js/jquery.flot.spline.min.js",
                       "~/Content/vendors/flot.curvedlines/curvedLines.js",
                       "~/Content/vendors/DateJS/build/date.js",
                       "~/Content/vendors/jqvmap/dist/jquery.vmap.js",
                       "~/Content/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                       "~/Content/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                       "~/Content/vendors/moment/min/moment.min.js",
                       "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.js",
                       "~/Scripts/custom.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard").Include(
                "~/Content/vendors/bootstrap/dist/css/bootstrap.css",
                "~/Content/vendors/font-awesome/css/font-awesome.css",
                "~/Content/vendors/nprogress/nprogress.css",
                "~/Content/vendors/iCheck/skins/flat/green.css",
                "~/Content/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.css",
                "~/Content/vendors/jqvmap/dist/jqvmap.css",
                "~/Content/vendors/vendors/bootstrap-daterangepicker/daterangepicker.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery-jvectormap-2.0.3.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}