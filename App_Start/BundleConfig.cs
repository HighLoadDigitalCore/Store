using System;
using System.Web;
using System.Web.Optimization;
using JavaScriptEngineSwitcher.Core;


namespace Smoking
{

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.UseCdn = true;


            var styles = new Bundle("~/css");
            styles.Include(

                "~/content/theme_main/css/width.css",
                "~/content/theme_main/css/index.css",
                "~/content/theme_main/css/jquery.slider.min.css",
                "~/content/theme_main/css/login.css",
                "~/content/theme_main/css/jquery.qtip.css",
                "~/content/theme_main/css/jquery.formstyler.css",
                "~/content/theme_main/css/not.valid.browesrs.css",

                "~/content/theme_main/css/jquery.mCustomScrollbar_min.css",
                "~/content/theme_main/css/new_head_and_catalog.css",
                "~/content/theme_main/css/nivo-slider.css",
                "~/content/theme_main/truncate/truncate.css",
                "~/content/theme_main/css/checkBo.css"


                );

            bundles.Add(styles);


            var js = new Bundle("~/js");
            js.Include("~/content/theme_main/js/jquery-1.8.2.min.js",
                "~/content/theme_main/js/jquery-ui-1.10.4.custom.min.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/swfobject.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/swfobject.js",
                "~/content/theme_main/js/valid.browesrs.js",
                "~/content/theme_main/js/String.js",
                "~/Scripts/jquery.json-2.3.min.js",
                "~/content/theme_main/js/jshashtable-2.1_src.js",
                "~/content/theme_main/js/jquery.numberformatter-1.2.3.js",
                "~/content/theme_main/js/tmpl.js",
                "~/content/theme_main/js/jquery.dependClass-0.1.js",
                "~/content/theme_main/js/draggable-0.1.js",
                "~/content/theme_main/js/jquery.slider.js",
                "~/content/theme_main/js/jquery.loupe_min.js",
                "~/content/theme_main/js/jquery.formstyler.js",
                "~/content/theme_main/js/jquery.qtip.js",
                "~/Scripts/jquery.ui.dialog.js",
                "~/content/theme_main/js/jquery.mCustomScrollbar.concat_min.js",
                "~/content/theme_main/js/script.js",
                "~/content/theme_main/js/jquery.nivo.slider.js",
                "~/content/theme_main/js/nivo-slider-load.js",
                "~/content/theme_main/truncate/truncate.js",
                "~/content/theme_main/js/checkBo.js"

                );
            bundles.Add(js);

        }
    }

}