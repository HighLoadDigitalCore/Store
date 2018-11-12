using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class CommonBlocksController : Controller
    {
        [ClientTemplate("Шапка сайта")]
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult ColorPicker(string ID)
        {
            return PartialView(ID);
        }

        /*[MenuItem("Меню", 2, Icon = "book_alt")]*/
        public ActionResult IndexList()
        {
            return View();
        }

        [ClientTemplate("Верхнее меню сайта")]
        public ActionResult UpperMenu()
        {
            return PartialView(db.CMSPageMenuCustoms.Where(x => x.UID == "upper" && x.Visible).OrderBy(x => x.OrderNum));
        }

        [ClientTemplate("Меню спецпредложений")]
        public ActionResult SpecMenu()
        {
            return PartialView(db.CMSPageMenuCustoms.Where(x => x.UID == "spec" && x.Visible).OrderBy(x => x.OrderNum));
        }

        [ClientTemplate("Заголовок H1")]
        public ActionResult H1Block()
        {
            return PartialView();
        }

        [ClientTemplate("Логотип")]
        public ActionResult Logo()
        {
            return PartialView();
        }

        [ClientTemplate("Меню подразделов в подвале")]
        public ActionResult FooterMenu()
        {
            return PartialView(db.CMSPageMenuCustoms.Where(x => x.Visible && x.UID == "bottom"));
        }
        [ClientTemplate("Заголовок H2")]
        public ActionResult H2Block()
        {
            return PartialView();
        }

        [ClientTemplate("Обратный след")]
        public ActionResult BreadCrumbs()
        {
            var crumbs = new BreadCrumbModel() { ShowMain = true };
            return PartialView(crumbs);
        }

        [ClientTemplate("Меню для текстовых страниц")]
        public ActionResult PageMenu()
        {
            return PartialView();
        }
        [ClientTemplate("Меню для каталога")]
        public ActionResult CatalogMenu()
        {
            return PartialView(new CatalogMenu());
        }




        [ClientTemplate("Меню для каталога Ver.2")]
        /*[OutputCache(Duration = 86400)]*/
        public ActionResult CatalogMenuV2()
        {
            return PartialView(/*new CatalogMenu()*/);
        }
        [ClientTemplate("Меню для каталога Ver.3")]
        public ActionResult CatalogMenuV3()
        {
            return PartialView();
        }
        [ClientTemplate("Меню для каталога Ver.4")]
        public ActionResult CatalogMenuV4()
        {
            return PartialView(new CatalogMenu());
        }

        public ActionResult CatalogMenuChildren(string url)
        {
            return PartialView(new CatalogMenu(url));
        }

        public ActionResult Counters()
        {
            return PartialView(db.SiteCounters.ToList());
        }


        [HttpGet]
        public PartialViewResult CommentsLenta(int ProductID)
        {
            ViewBag.ProductID = ProductID;
            return
                PartialView(
                    new DB().StoreProductComments.Where(
                        z => z.ProductID == ProductID).Select(x => x.Comment)
                            .OrderBy(x => x.Date));

        }

        [HttpPost]
        [Authorize]
        public ActionResult CommentRating(int CommentID, int Useful)
        {
            var exist = db.CommentsRatings.FirstOrDefault(
                x =>
                x.CommentID == CommentID && x.UserID == HttpContext.GetCurrentUserUID());
            if (exist != null)
            {
                if (exist.Useful != (Useful == 1))
                {
                    exist.Useful = !exist.Useful;
                }
            }
            else
            {
                db.CommentsRatings.InsertOnSubmit(new CommentsRating()
                    {
                        CommentID = CommentID,
                        UserID = HttpContext.GetCurrentUserUID(),
                        Useful = Useful == 1
                    });
            }
            db.SubmitChanges();
            var good = db.CommentsRatings.Count(x => x.CommentID == CommentID && x.Useful);
            var bad = db.CommentsRatings.Count(x => x.CommentID == CommentID && !x.Useful);
            return new ContentResult() { Content = string.Format("{0};{1}", good, bad) };
        }

        [HttpPost]
        [Authorize]
        public PartialViewResult CommentsLenta(int ProductID, string Message)
        {

            if (Message.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Необходимо заполнить текст сообщения");
                return CommentsLenta(ProductID);
            }

            var comment = new Comment()
                {
                    UserID = HttpContext.GetCurrentUserUID(),
                    CommentText = Message,
                    Date = DateTime.Now
                };

            var msg = new StoreProductComment()
                {
                    ProductID = ProductID,
                    Comment = comment

                };
            var db = new DB();
            db.Comments.InsertOnSubmit(comment);
            db.StoreProductComments.InsertOnSubmit(msg);
            db.SubmitChanges();
            return CommentsLenta(ProductID);


        }





        [ClientTemplate("Слайдер")]
        public ActionResult Slider(int ViewID)
        {
            

            var browser = CatalogBrowser.Init();
            if (browser.IsProductPage)
            {
                return PartialView();
            }
            if (browser.IsCategoryPage && /*!browser.CurrentCategory.ShowSlider*/ browser.CurrentCategory.ID != CatalogBrowser.Root.ID)
            {
                return PartialView();
            }

            var db = new DB();
            var info = AccessHelper.CurrentPageInfo;
            var data = db.CMSPageSliders.Where(
                x =>
                x.CMSPageID == info.CurrentPage.ID && x.LangID == info.CurrentLang.ID &&
                x.ViewID == ViewID).Where(x => x.Visible).OrderBy(x => x.OrderNum);

            return
                PartialView(data.ToList());
        }




        private DB db = new DB();


        [HttpGet]
        [ClientTemplate("Строка поиска")]
        public ActionResult Search()
        {
            return PartialView(new CommonSearch());
        }


        [HttpPost]
        public ActionResult Search(CommonSearch search, FormCollection collection)
        {
            if (search.SearchQuery.IsNullOrEmpty())
                return PartialView(search);
            Response.Redirect(search.ToString());
            return PartialView(search);
        }




    }
}