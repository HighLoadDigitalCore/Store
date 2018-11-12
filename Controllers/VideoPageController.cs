using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{

    public class VideoPageController : Controller
    {
        private DB db = new DB();

        [ClientTemplate("Видео-ролик")]
        public ActionResult Index(int ViewID)
        {
            var info = AccessHelper.CurrentPageInfo;
            var data = db.CMSPageVideos.Where(
                x =>
                x.CMSPageID == info.CurrentPage.ID && x.LangID == info.CurrentLang.ID &&
                x.ViewID == ViewID).Where(x => x.Visible).OrderBy(x => x.OrderNum);
            return
                PartialView(data);
        }
    }
}
