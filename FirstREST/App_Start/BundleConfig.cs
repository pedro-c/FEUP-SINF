﻿using System.Web;
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


            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                "~/Content/vendors/font-awesome/css/font-awesome.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/nprogress").Include(
                "~/Content/vendors/nprogress/nprogress.css"
                ));

            bundles.Add(new StyleBundle("~/Content/iCheck").Include(
                "~/Content/vendors/iCheck/skins/flat/green.css"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap-progressbar").Include(
                "~/Content/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap-daterangepicker").Include(
                "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.css"
                ));

            bundles.Add(new StyleBundle("~/Content/jqvmap").Include(
                "~/Content/vendors/jqvmap/dist/jqvmap.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                "~/Content/custom.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                "~/Content/vendors/jquery-ui/jquery-ui.min.css",
                "~/Content/vendors/jquery-ui/jquery-ui.structure.css",
                "~/Content/vendors/jquery-ui/jquery-ui.structure.min.css",
                "~/Content/vendors/jquery-ui/jquery-ui.theme.css",
                "~/Content/vendors/jquery-ui/jquery-ui.theme.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
                       "~/Content/vendors/fastclick/lib/fastclick.js"));

            bundles.Add(new ScriptBundle("~/bundles/nprogress").Include(
                       "~/Content/vendors/nprogress/nprogress.js"));

            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
                       "~/Content/vendors/Chart.js/dist/Chart.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/gauge").Include(
                       "~/Content/vendors/gauge.js/dist/gauge.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-progressbar").Include(
                       "~/Content/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/icheck").Include(
                       "~/Content/vendors/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/skycons").Include(
                       "~/Content/vendors/skycons/skycons.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                       "~/Content/vendors/Flot/jquery.flot.js",
                       "~/Content/vendors/Flot/jquery.flot.pie.js",
                       "~/Content/vendors/Flot/jquery.flot.time.js",
                       "~/Content/vendors/Flot/jquery.flot.slack.js",
                       "~/Content/vendors/Flot/jquery.flot.resize.js"
                       ));
            
            bundles.Add(new ScriptBundle("~/bundles/flot-plugins").Include(
                       "~/Content/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                       "~/Content/vendors/flot-spline/js/jquery.flot.spline.min.js",
                       "~/Content/vendors/flot.curvedlines/curvedLines.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                       "~/Content/vendors/datatables.net/js/jquery.dataTables.min.js",
                       "~/Content/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js",
                       "~/Content/vendors/datatables.net-buttons/js/dataTables.buttons.min.js",
                       "~/Content/vendors/datatables.net-fixedHeader/js/dataTables.fixedHeader.min.js",
                       "~/Content/vendors/datatables.net-keytable/js/dataTables.keytable.min.js",
                       "~/Content/vendors/datatables.net-responsive/js/dataTables.responsive.min.js",
                       "~/Content/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.min.js",
                       "~/Content/vendors/datatables.net-scroller/js/dataTables.scroller.min.js"
                       ));
            
            bundles.Add(new ScriptBundle("~/bundles/date").Include(
                       "~/Content/vendors/DateJS/build/date.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqvmap").Include(
                       "~/Content/vendors/jqvmap/dist/jquery.vmap.js",
                       "~/Content/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                       "~/Content/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js"
                       ));
            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                       "~/Content/vendors/moment/min/moment.min.js",
                       "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/custom.js"));

        }

    }
}