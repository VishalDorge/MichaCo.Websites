using System.Web;
using System.Web.Optimization;

namespace mconrad.azurewebsites.net
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Prettify/prettify.js",
                      "~/Scripts/site.js",
                      "~/Scripts/toc.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Prettify/prettify.css",
                      "~/Content/Prettify/Themes/cSharp.css"));

            bundles.Add(new StyleBundle("~/Content/fontAwesome").Include(
                      "~/Content/font-awesome.css"));
        }
    }
}
