using System.Web;
using System.Web.Optimization;

namespace ELSO.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.easing.min.js",
                        "~/Scripts/jquery.datetimepicker.full.js",
                         "~/Scripts/jquery-ui-1.10.0.js",
                        "~/Scripts/grid.locale-en.js",
                         "~/Scripts/moment.js",
                         "~/Scripts/moment-with-locales.js",
                         "~/Scripts/jquery.timepicker.js",
                         "~/Scripts/jquery.maskedinput.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            // bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
                ));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                 "~/Content/bootstrap.css"
                ));

           // bootstrap table
           bundles.Add(new ScriptBundle("~/bundles/bootstrap-table").Include(
                "~/Scripts/Plugins/bootstrap-table/bootstrap-table.js",
                "~/Scripts/Plugins/bootstrap-table/bootstrap-table-filter-control.js",
                "~/Scripts/Plugins/bootstrap-table/bootstrap-table-export.js"
                ));
            bundles.Add(new StyleBundle("~/Content/bootstrap-table").Include(
                 "~/Content/bootstrap-table.css"
                ));

            // Knockoutjs
            bundles.Add(new ScriptBundle("~/bundles/knockoutjs").Include(
                 "~/Scripts/knockout-{version}.js",
                 "~/Scripts/knockout.mapping-latest.js"
                  ));

            // ELSO Template
            bundles.Add(new ScriptBundle("~/bundles/elso").Include(
                    "~/Scripts/knockout-{version}.js",
                    "~/Scripts/scrollreveal.js",
                    "~/Scripts/jquery.magnific-popup.js",
                    "~/Scripts/elso.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(                   
                    "~/Content/site.css",
                    "~/Content/jquery.datetimepicker.css",
                    "~/Content/font-awesome.css",
                     "~/Content/jquery-ui.css",
                     "~/Content/ui.jqgrid.css",
                     "~/Content/jquery.timepicker.css"
                   //"~/Content/accesskey.css",
                   //"~/Content/reset.css",
                   //"~/Content/error_handling.css"
                   ));
            //error handling
            //bundles.Add(new ScriptBundle("~/bundles/errorHandling").Include(
            //    "~/Scripts/jquery.min.js",
            //    "~/Scripts/jquery.tools.min.js",
            //    "~/Scripts/error_handling.js",
            //    "~/Scripts/accesskey.js"
            //    ));

            // jQuerydatatable
            bundles.Add(new ScriptBundle("~/bundles/jQuerydatatable").Include(
                    //"~/Scripts/Jquerydatatable/buttons.flash.min.js",
                    "~/Scripts/Plugins/jQuerydatatable/jquery.dataTables.js",
                    "~/Scripts/Plugins/jQuerydatatable/buttons.html5.min.js",
                    "~/Scripts/Plugins/jQuerydatatable/buttons.print.min.js",
                    "~/Scripts/Plugins/jQuerydatatable/dataTables.buttons.min.js",
                    "~/Scripts/Plugins/jQuerydatatable/dataTables.select.min.js",
                    "~/Scripts/Plugins/jQuerydatatable/dataTables.responsive.min.js",
                     "~/Scripts/Plugins/jQuerydatatable/jszip.min",
                     "~/Scripts/Plugins/jQuerydatatable/pdfmake.min.js",
                     "~/Scripts/Plugins/jQuerydatatable/vfs_fonts.js"));

            bundles.Add(new StyleBundle("~/Content/jQuerydatatable").Include(
                  "~/Content/buttons.dataTables.min.css",
                 "~/Content/jquery.dataTables.min.css",
                 "~/Content/select.dataTables.min.css",
                 "~/Content/responsive.dataTables.min.css"
                ));
        }
    }
}
