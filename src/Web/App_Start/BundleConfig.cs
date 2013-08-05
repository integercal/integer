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
              new ScriptBundle("~/scripts/vendor")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/jquery.globalize/globalize.js")
                .Include("~/scripts/knockout-{version}.debug.js")
                .Include("~/scripts/sammy-{version}.js")
                .Include("~/scripts/toastr.js")
                .Include("~/scripts/Q.js")
                .Include("~/scripts/breeze.debug.js")
                .Include("~/scripts/moment.js")
              );

            bundles.Add(
                new ScriptBundle("~/scripts/bootstrap")
                    .Include("~/scripts/bootstrap.js")
            );

            bundles.Add(
                new ScriptBundle("~/scripts/wijmo")
                    .Include("~/Scripts/jquery-ui-{version}.js")
                    .Include("~/scripts/wijmo/jquery.wijmo-open.all.{version}.js")
                    .Include("~/scripts/wijmo/jquery.wijmo-pro.all.{version}.js")
                    .Include("~/scripts/wijmo/interop/knockout.wijmo.{version}.js")
                    .Include("~/scripts/jquery.globalize/cultures/globalize.culture.pt-BR.js")
            );

            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/durandal.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/app.css")
                .Include("~/Content/themes/bootstrap/jquery-ui.css")
                .Include("~/Content/wijmo/jquery.wijmo-pro.all.{version}.min.css")
                .Include("~/Content/zocial/zocial.css")
              );
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