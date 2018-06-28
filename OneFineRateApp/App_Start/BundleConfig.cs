using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;


namespace OneFineRateApp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {


            //Css Intranet bundle
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/css/bootstrap.min.css",
                                                                 "~/css/font-awesome.css",
                                                                 "~/css/style.css",
                                                                 "~/css/jquery.dataTables.min.css",
                                                                 "~/css/bootstrap-responsive.min.css",
                                                                 "~/css/toastr.css",
                                                                 "~/css/select2.css",
                                                                 "~/css/select2-bootstrap.css",
                                                                 "~/css/jqueryslidemenu.css",
                                                                 "~/css/jquery-ui-1.8.1.custom.css",
                                                                 "~/css/calendar.css"
                                                               ));

            //js Intranet bundle
            bundles.Add(new ScriptBundle("~/bundles/js")
                                                         .Include(
                                                                "~/js/jquery-1.9.1.js",
                                                                "~/js/jqueryslidemenu.js",
                                                                "~/js/bootbox.min.js",
                                                                "~/js/bootstrap.min.js",
                                                                "~/js/main.js",
                                                                "~/js/jquery.dataTables.min.js",
                                                                "~/js/matrix.tables.js",
                                                                "~/js/toastr.min.js",
                                                                "~/js/select2.min.js"
                                                               ));

            //js Intranet bundle
            bundles.Add(new ScriptBundle("~/bundles_Mobile/js")
                                                         .Include(
                                                                "~/js/jquery-1.9.1.js",
                                                                "~/js/bootstrap.min.js",
                                                                "~/js/main.js",
                                                                "~/js/toastr.min.js"
                                                               ));



            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                    "~/Scripts/dropzone/dropzone.min.js"));

            bundles.Add(new StyleBundle("~/bundles/dropzonescss").Include(
                     "~/Scripts/dropzone/css/basic.css",
                     "~/Scripts/dropzone/css/dropzone.css"));

            //Css Home Page bundle
            bundles.Add(new StyleBundle("~/bundles/HomePagecss").Include("~/css/bootstrap.min.css",
                                                                 "~/css/font-awesome.css",
                                                                 "~/css/style.css",
                                                                 "~/css/bootstrap-responsive.min.css",
                                                                 "~/css/toastr.css"
                                                               ));
            //js Home Page bundle
            bundles.Add(new ScriptBundle("~/bundles/HomePagejs").Include(
                                                                "~/js/jquery-1.9.1.js",
                                                                "~/js/bootstrap.min.js",
                                                                "~/js/main.js",
                                                                "~/js/toastr.min.js"
                                                                 ));
            //js Home Page bundle
            bundles.Add(new ScriptBundle("~/bundles/HomePagejs").Include(
                                                                "~/js/jquery-1.9.1.js",
                                                                "~/js/bootstrap.min.js",
                                                                "~/js/main.js",
                                                                "~/js/toastr.min.js"
                                                                 ));

            //js validation bundle
            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                                                                "~/js/jquery.validate.js",
                                                                "~/Scripts/jquery.validate.unobtrusive.min.js"
                                                                 ));

            //js autofill bundle
            bundles.Add(new ScriptBundle("~/bundles/autofill").Include(
                                                                "~/js/Jquery1.10.3.js",
                                                                "~/js/GetScallerValueWithAjax.js"
                                                                 ));

            //js ajax ajax post form form bundle
            bundles.Add(new ScriptBundle("~/bundles/ajaxpostform").Include(
                                                                "~/js/Jquery1.10.3.js",
                                                                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                                                                 ));




            BundleTable.EnableOptimizations = false;
        }
    }
}