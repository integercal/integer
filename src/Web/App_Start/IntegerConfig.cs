using System;
using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(
    typeof(Web.App_Start.IntegerConfig), "PreStart")]

namespace Web.App_Start
{
    public static class IntegerConfig
    {
        public static void PreStart()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}