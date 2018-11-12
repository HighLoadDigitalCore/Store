using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class AnimeController : Controller
    {


        [ClientTemplate("Блок анимации")]
        public ActionResult AnimeView()
        {

            AnimeBlock block = null;
            if (CatalogBrowser.Init().IsCategoryPage)
            {
                block = db.AnimeBlocks.FirstOrDefault(x => x.CatID == CatalogBrowser.Init(Request.RawUrl).CurrentCategory.ID);
            }
            else
            {
                block = db.AnimeBlocks.FirstOrDefault(x => x.PageID == AccessHelper.CurrentPageInfo.ID);
            }
            return PartialView(block);
        }

        private DB db = new DB();

        [MenuItem("Анимационные блоки", ID = 51, ParentID = 4)]
        public ActionResult Index()
        {
            return View(db.AnimeBlocks.ToList());
        }


        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeData(string SelectedSection)
        {
            var list = new List<string>();
            if (SelectedSection.IsFilled())
            {
                list.Add(SelectedSection);

            }
            var treeData = new UniversalTreeDataSource() { CheckedNodes = list };
            return treeData.Serialize(SerializationType.PagesAnsCategories);

        }

        [AuthorizeMaster]
        public ActionResult DeletePoint(int id)
        {
            var item = db.AnimeBlockItems.FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                db.AnimeBlockItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            return new ContentResult(){Content = "1"};
        }

        [AuthorizeMaster]
        public ActionResult SavePoint(int id, string Link, string Text)
        {
            var item = db.AnimeBlockItems.FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                item.Link = Link;
                item.Text = Text;
                db.SubmitChanges();
            }
            return new ContentResult(){Content = "1"};
        }

        [AuthorizeMaster]
        public ActionResult ReadPoint(int id)
        {
            var item = db.AnimeBlockItems.FirstOrDefault(x => x.ID == id);
            var result = item == null ? new {Text = "", Link = ""} : (object)new {item.Text, item.Link};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeMaster]
        public ActionResult SavePos(int id, int x, int y, int aid)
        {
            AnimeBlockItem item = null;
            if (id > 0)
            {
                item = db.AnimeBlockItems.FirstOrDefault(z => z.ID == id);
                if (item != null)
                {
                    item.XPos = x;
                    item.YPos = y;
                }
            }
            else
            {
                item = new AnimeBlockItem()
                {
                    AnimeBlockID = aid,
                    XPos = x,
                    YPos = y,
                    Text = "",
                    Link = ""
                };
                db.AnimeBlockItems.InsertOnSubmit(item);
            }

            db.SubmitChanges();

            if (item != null)
            {
                id = item.ID;
            }
            return new ContentResult(){Content = id.ToString()};
        }

        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Edit(int? ID)
        {
            var filter = db.AnimeBlocks.FirstOrDefault(x => x.ID == ID) ?? new Models.AnimeBlock() { Visible = true, Width = 700, Height = 300};
            if (filter.Wheel.IsNullOrEmpty())
            {
                filter.Wheel = "/content/theme_main/img/gear.png";
            }
            return View(filter);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Edit(Models.AnimeBlock filter, HttpPostedFileBase BackgroundFile, HttpPostedFileBase WheelFile)
        {
            if (!filter.HasSection)
            {
                ViewBag.Message = "Необходимо выбрать раздел";
                return View(filter);
            }

            if (filter.Wheel.IsNullOrEmpty())
            {
                filter.Wheel = "/content/theme_main/img/gear.png";
            }

            var dbf = new AnimeBlock();
            if (filter.ID == 0)
            {

                db.AnimeBlocks.InsertOnSubmit(filter);
            }
            else
            {
                dbf = db.AnimeBlocks.First(x => x.ID == filter.ID);
                dbf.SelectedSection = filter.SelectedSection;
                dbf.Visible = filter.Visible;
                dbf.Width = filter.Width;
                dbf.Height = filter.Height;

            }
            if ((BackgroundFile == null || BackgroundFile.ContentLength == 0) &&
                (filter.ID == 0 || dbf.Background.IsNullOrEmpty()))
            {
                ViewBag.Message = "Необходимо выбрать фоновое изображение";
                return View(filter);
            }

            
            var fileName = BackgroundFile.SaveAsRelative("/content/Anime/", Guid.NewGuid().ToString());
            if (fileName.IsFilled())
            {
                dbf.Background = fileName;
            }
            var fileNameWheel = WheelFile.SaveAsRelative("/content/Anime/", Guid.NewGuid().ToString());
            if (fileNameWheel.IsFilled())
            {
                dbf.Wheel = fileNameWheel;
            }
            else
            {
                dbf.Wheel = "/content/theme_main/img/gear.png";
            }

            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeMaster]
        public ActionResult Delete(int ID)
        {
            var filter = db.AnimeBlocks.FirstOrDefault(x => x.ID == ID);
            if (filter != null)
            {
                db.AnimeBlocks.DeleteOnSubmit(filter);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        [AuthorizeMaster]
        public ActionResult Items(int FilterID)
        {
            ViewBag.FilterID = FilterID;
            ViewBag.Anime = db.AnimeBlocks.FirstOrDefault(x => x.ID == FilterID);
            if (ViewBag.Anime == null)
            {
                return RedirectToAction("Index");
            }
            var filter = db.AnimeBlockItems.Where(x => x.AnimeBlockID == FilterID).ToList();
            
            return View(filter);

        }

/*
        [AuthorizeMaster]
        public ActionResult Items(int FilterID)
        {
            ViewBag.FilterID = FilterID;
            var filter = db.AnimeBlockItems.Where(x => x.AnimeBlockID == FilterID).ToList();
            return View(filter);

        }

        [AuthorizeMaster]
        [HttpGet]
        public ActionResult ItemEdit(int FilterID, int? ID)
        {
            FilterItem item = null;
            if (!ID.HasValue)
            {
                item
                    = new FilterItem()
                    {
                        FilterID = FilterID
                    };
            }
            else
            {
                item = db.FilterItems.FirstOrDefault(x => x.ID == ID) ?? new FilterItem() { FilterID = FilterID };
            }
            return View(item);
        }

        [AuthorizeMaster]
        [HttpPost]
        public ActionResult ItemEdit(int FilterID, int? ID, FilterItem item)
        {

            string err = "";
            if (item.Name.IsNullOrEmpty())
            {
                err = "Необходимо указать название фильтра";
            }
            if (err.IsFilled())
            {
                ModelState.AddModelError("", err);
                return View(item);
            }

            if (item.ID == 0)
            {
                db.FilterItems.InsertOnSubmit(item);
            }
            else
            {
                var dbi = db.FilterItems.FirstOrDefault(x => x.ID == ID);
                if (dbi != null)
                {
                    dbi.LoadPossibleProperties(item);
                }
            }

            db.SubmitChanges();

            return RedirectToAction("Items", new {FilterID = FilterID});
        }

        public ActionResult ItemDelete(int ID, int FilterID)
        {
            var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                db.FilterItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            return RedirectToAction("Items",new {FilterID});
        }
*/

    }

    
}
