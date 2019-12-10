using System.Web;
using System.Web.Optimization;

namespace MovieLibrary.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            //jquery ui
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                     "~/Scripts/jquery-ui.min.js",
                     "~/Scripts/jquery.js"));

            //SweetAlert2 js plugin
            bundles.Add(new ScriptBundle("~/bundles/sweetalert2").Include(
                      "~/Scripts/sweetalert/dist/sweetalert2.all.min.js"));

            //Chosen js plugin
            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                      "~/Scripts/chosen/chosen.jquery.min.js"));

            //select2 js plugin
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                      "~/Scripts/select2/select2.min.js"));

            //Datatable js plugin
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                      "~/Scripts/datatable/datatables.min.js"));

            //jsGrid js plugin
            bundles.Add(new ScriptBundle("~/bundles/jsgrid").Include(
                      "~/Scripts/jsGrid/jsgrid.min.js"));

            //my_jsGrid js file
            bundles.Add(new ScriptBundle("~/bundles/movies").Include(
                      "~/Scripts/movielibrary/movies.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Bootstrap4 preffered icons - Iconic : https://getbootstrap.com/docs/4.0/extend/icons/
            bundles.Add(new StyleBundle("~/Content/Icons/css").Include(
                     "~/Content/open-iconic-master/font/css/open-iconic-bootstrap.min.css"));

            //SweetAlert2 css
            bundles.Add(new StyleBundle("~/Content/SweetAlert2/css").Include(
                   "~/Content/sweetalert/dist/sweetalert2.min.css"));

            //Chosen css
            bundles.Add(new StyleBundle("~/Content/Chosen/css").Include(
                   "~/Content/chosen/chosen.min.css"));

            //select2
            bundles.Add(new StyleBundle("~/Content/select2").Include(
             "~/Content/select2/select2.min.css"));

            //Datatable css
            bundles.Add(new StyleBundle("~/Content/Datatable/css").Include(
                   "~/Content/datatable/datatable.min.css"));

            //jsGird css
            bundles.Add(new StyleBundle("~/Content/jsGrid/css").Include(
                   "~/Content/jsGrid/jsgrid.min.css",
                   "~/Content/jsGrid/jsgrid-theme.min.css"));

            //jsGird css
            bundles.Add(new StyleBundle("~/Content/my-grid/css").Include(
                   "~/Content/movielibrary/my-grid.css"));

            //jquery ui
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
             "~/Content/jquery-ui.min.css"));

            //site css
            bundles.Add(new StyleBundle("~/Content/site/css").Include(
                   "~/Content/movielibrary/site.css"));
        }
    }
}
