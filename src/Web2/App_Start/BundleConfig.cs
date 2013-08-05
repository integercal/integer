using System;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/app.css")
                .Include("~/Content/themes/bootstrap/jquery-ui.css")
                .Include("~/Content/wijmo/jquery.wijmo-pro.all.{version}.min.css")
                .Include("~/Content/zocial/zocial.css")
                .Include("~/Content/datetimepicker/datetimepicker.css")
                .Include("~/Content/gmail-scrollbars.css")
                .Include("~/Content/colorPicker.css")
              );

            bundles.Add(
              new Bundle("~/scripts/lib")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/jquery.globalize/globalize.js")
                
                .Include("~/scripts/toastr.js")
                
                .Include("~/scripts/angular.js")
                .Include("~/scripts/i18n/angular-locale_pt-br.js")
                .Include("~/scripts/angular-resource.js")
                .Include("~/scripts/angular-cookies.js")
                .Include("~/scripts/angular-ui.js")

                .Include("~/scripts/i18next-{version}.min.js")
                .Include("~/scripts/ng-i18next/ng-i18next.js")

                .Include("~/scripts/bootstrap.js")

                .Include("~/scripts/moment.js")
                .Include("~/scripts/i18n.moment/pt-br.js")

                .Include("~/scripts/datetimepicker/bootstrap-datetimepicker.js")
                .Include("~/scripts/datetimepicker/locale/bootstrap-datetimepicker.pt-BR.js")

                .Include("~/scripts/jquery.colorPicker.js")

                .Include("~/scripts/angular-dragdrop.js")
              );

            bundles.Add(
              new Bundle("~/scripts/_wijmo")
                .Include("~/Scripts/jquery-ui-{version}.js")
                .Include("~/scripts/wijmo/jquery.wijmo-open.all.{version}.js")
                .Include("~/scripts/wijmo/jquery.wijmo-pro.all.{version}.js")
                .Include("~/scripts/wijmo/interop/angular.wijmo.{version}.js")
                .Include("~/scripts/jquery.globalize/cultures/globalize.culture.pt-BR.js")
              );

            BundleTable.EnableOptimizations = true;
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
            {
                throw new ArgumentNullException("ignoreList");
            }

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");

            //ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}