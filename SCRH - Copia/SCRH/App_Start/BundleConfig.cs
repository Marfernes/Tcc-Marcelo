using System.Web.Optimization;

namespace SCRH
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region CSS & JS Layout
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/layout-interno").Include(
                      "~/Scripts/popper.js",
                      "~/Scripts/bootstrap-material-design.js",
                      "~/Scripts/plugins/perfect-scrollbar.jquery.js",
                      "~/Scripts/material-dashboard.js",
                      "~/Scripts/demo.js"));

            bundles.Add(new StyleBundle("~/Content/layout-interno").Include(
                      "~/Content/material-dashboard.css",
                      "~/Content/material-dashboard-rtl.css",
                      "~/Content/demo.css"));

            bundles.Add(new ScriptBundle("~/bundles/layout-externo").Include(
                      "~/Scripts/manifest.js",
                      "~/Scripts/vendor.js",
                      "~/Scripts/app.js"));

            bundles.Add(new StyleBundle("~/Content/layout-externo").Include(
                      "~/Content/fontawesome.css",
                      "~/Content/layout-externo.css"));
            #endregion

            #region CSS & JS Plugins
            bundles.Add(new ScriptBundle("~/bundles/sweetalert2").Include("~/Scripts/plugins/sweetalert2.js"));
            bundles.Add(new StyleBundle("~/Content/sweetalert2").Include("~/Content/sweetalert2.css"));
            bundles.Add(new ScriptBundle("~/bundles/mask").Include("~/Scripts/jquery.mask.js", "~/Scripts/interno/mask.js", "~/Scripts/jquery.priceformat.js", "~/Scripts/interno/maskmoney.config.js"));
            bundles.Add(new ScriptBundle("~/bundles/lobibox").Include("~/Scripts/lobibox.js"));
            bundles.Add(new StyleBundle("~/Content/lobibox").Include("~/Content/lobibox.css"));
            bundles.Add(new ScriptBundle("~/bundles/loading").Include("~/Scripts/plugins/jquery.loading.js"));
            #endregion

            #region CSS & JS Internos
            bundles.Add(new ScriptBundle("~/bundles/evento-ativo").Include("~/Scripts/interno/evento-ativo.js"));
            bundles.Add(new ScriptBundle("~/bundles/carregar-modal").Include("~/Scripts/interno/carregar-modal.js"));
            bundles.Add(new ScriptBundle("~/bundles/buscar-paginacao").Include("~/Scripts/interno/buscar-paginacao.js"));
            bundles.Add(new ScriptBundle("~/bundles/buscar-disponiveis").Include("~/Scripts/interno/buscar-disponiveis.js"));
            #endregion
        }
    }
}
