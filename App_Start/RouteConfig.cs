using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Smoking.Extensions.Helpers;

namespace Smoking
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            routes.IgnoreRoute("{*ckfinder}", new { ckfinder = @"(.*)ckfinder(.*)" });
            routes.IgnoreRoute("{*alljs}", new { alljs = @".*\.js(/.*)?" });
            routes.IgnoreRoute("{*allcss}", new { allcss = @".*\.css(/.*)?" });


            routes.MapRoute(
                name: "SiteMap",
                url: "sitemap.xml",
                defaults: new {controller = "SiteMapEdit", action = "SiteMap"}
                );           
            
            routes.MapRoute(
                name: "Master",
                url: "Master/{lang}/{controller}/{action}/{id}",
                defaults: new { controller = AccessHelper.MasterPanel.DefaultController, action = AccessHelper.MasterPanel.DefaultAction, lang = AccessHelper.MasterPanel.DefaultLang, id = UrlParameter.Optional }
                );

/*
            routes.MapRoute(
                name: "MasterPreview",
                url: "MasterPreview/{controller}/{action}/{id}",
                defaults: new { controller = AccessHelper.MasterPanel.DefaultController, action = AccessHelper.MasterPanel.DefaultAction, lang = AccessHelper.MasterPanel.DefaultLang, id = UrlParameter.Optional }
                );
*/

/*
            routes.MapRoute(
                name: "UploadAvatar",
                url: "Master/Service/UploadAvatar",
                defaults: new { controller = "CommonBlocks", action = "AvatarUpload"}
                );
*/


            routes.MapRoute(
                name: "MasterListPaged",
                url: "Master/{lang}/{controller}/{action}/Page/{page}",
                defaults: new { controller = AccessHelper.MasterPanel.DefaultController, action = AccessHelper.MasterPanel.DefaultAction, lang = AccessHelper.MasterPanel.DefaultLang, page = UrlParameter.Optional });


            routes.MapRoute(
                name: "Default",
                url: "{url1}/{url2}/{url3}/{url4}/{url5}/{url6}/{url7}/{url8}/{url9}/{url10}/{url11}/{url12}/{url13}/{url14}/{url15}",
                defaults: new
                    {
                        controller = "Selector",
                        action = "Index",
                        url1 = UrlParameter.Optional,
                        url2 = UrlParameter.Optional,
                        url3 = UrlParameter.Optional,
                        url4 = UrlParameter.Optional,
                        url5 = UrlParameter.Optional,
                        url6 = UrlParameter.Optional,
                        url7 = UrlParameter.Optional,
                        url8 = UrlParameter.Optional,
                        url9 = UrlParameter.Optional,
                        url10 = UrlParameter.Optional,
                        url11 = UrlParameter.Optional,
                        url12 = UrlParameter.Optional,
                        url13 = UrlParameter.Optional,
                        url14 = UrlParameter.Optional,
                        url15 = UrlParameter.Optional,

                    }
                );

        }
    }
}