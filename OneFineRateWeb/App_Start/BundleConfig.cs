using System.Web;
using System.Web.Optimization;

namespace OneFineRateWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-1.10.2.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/userManagementJs").Include(
                      "~/js/jquery.cookie.js",
                      "~/js/wSelect.js",
                      "~/js/jquery-ui.min.js",
                      "~/js/jquery.flagstrap.js",
                      "~/Scripts/waitingSpinner.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/UserScripts/jquery.bootpage.min.js",
                      "~/Scripts/UserScripts/jquery.smoothScroll.js",
                      "~/js/bootstrap-number-input-user.js",
                      "~/Scripts/UserScripts/common.js"
                      ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                //"~/Scripts/bootstrap-datepicker.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/css/bootstrap.min.css",
                      "~/css/font-awesome.css",
                      "~/css/flags.css",
                      "~/css/datepicker.css",
                      "~/css/style.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/userManagementCss").Include(
                    "~/css/jquery-ui-1.8.1.custom.css",
                    "~/Content/waitingSpinner.css",
                    "~/Content/toastr.min.css",
                    "~/css/jqueryslidemenu.css",
                    "~/css/wSelect.css",
                    "~/css/UserStyle.css",
                    "~/css/usermenu.css"
                    ));

            //js autofill bundle
            bundles.Add(new ScriptBundle("~/bundles/autofill").Include(
                      "~/js/Jquery1.10.3.js",
                      "~/js/GetScallerValueWithAjax.js"
                    ));

            //js ajax ajax post form form bundle
            bundles.Add(new ScriptBundle("~/bundles/ajaxpostform").Include(
                      "~/js/Jquery1.10.3.js",
                      "~/Scripts/jquery.unobtrusive-ajax.js"
                     ));


            bundles.Add(new ScriptBundle("~/bundles/Slider").Include(
                    "~/js/jQRangeSliderMouseTouch.js",
                    "~/js/jQRangeSliderDraggable.js",
                    "~/js/jQRangeSliderHandle.js",
                    "~/js/jQRangeSliderBar.js",
                    "~/js/jQRangeSliderLabel.js",
                    "~/js/jQRangeSlider.js"
                  ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
