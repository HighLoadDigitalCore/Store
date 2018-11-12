using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{


    public class HomeController : Controller
    {
        private DB db = new DB();

        [HttpGet]
        /*[MenuItem("Карта сайта", 1, Icon = "home")]*/
        [AuthorizeMaster]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AuthorizeMaster]

        public ContentResult DeleteCategorySlider(int CategoryID, string type)
        {
            if (type == "product")
            {
                var images = db.CMSPageSliders.Where(x => x.ProductID == CategoryID).ToList();
                if (images.Any())
                {
                    db.CMSPageSliders.DeleteAllOnSubmit(images);
                    db.SubmitChanges();
                }

            }
            else
            {
                var images = db.CMSPageSliders.Where(x => x.CategoryID == CategoryID).ToList();
                if (images.Any())
                {
                    db.CMSPageSliders.DeleteAllOnSubmit(images);
                    db.SubmitChanges();
                }
            }
            return new ContentResult();
        }

        [HttpGet]
        [AuthorizeMaster]
        public ContentResult DeleteCategoryVideo(int CategoryID, string type)
        {
            var images = db.CMSPageVideos.Where(x => x.CategoryID == CategoryID).ToList();
            if (images.Any())
            {
                db.CMSPageVideos.DeleteAllOnSubmit(images);
                db.SubmitChanges();
            }

            return new ContentResult();
        }


        [HttpGet]
        [AuthorizeMaster]
        public JsonResult GetText(int CMSPageID, int ViewID)
        {
            var text = db.CMSPageTextDatas.FirstOrDefault(x => x.ViewID == ViewID && x.CMSPageID == CMSPageID);
            return new JsonResult()
            {
                ContentEncoding = Encoding.UTF8,
                Data = text == null ? new { id = 0, value = "" } : new { id = text.ID, value = text.Text },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        [ValidateInput(false)]
        [AuthorizeMaster]
        public ContentResult SetText(int CMSPageID, int ViewID, string text)
        {
            var t = db.CMSPageTextDatas.FirstOrDefault(x => x.ViewID == ViewID && x.CMSPageID == CMSPageID);
            if (t != null)
            {
                t.Text = text;
                db.SubmitChanges();
            }
            else
            {
                t = new CMSPageTextData()
                {
                    ViewID = ViewID,
                    CMSPageID = CMSPageID,
                    LangID = 1,
                    Text = text,
                    Visible = true,
                    OrderNum = db.CMSPageTextDatas.Count() + 1
                };
                db.CMSPageTextDatas.InsertOnSubmit(t);
                db.SubmitChanges();
            }
            return new ContentResult() { Content = "" };
        }

        /*
                [HttpGet]
                [MenuItem("Корзина", -200, ParentID = 1)]
        */
        [AuthorizeMaster]
        public ActionResult Recicle()
        {
            var recicle = db.CMSPages.Where(x => x.Deleted).ToList().Select(x => x.LoadLangValues()).Select(x => new RecicleItem() { Name = x.PageName, Code = "x" + x.ID }).ToList();
            recicle.AddRange(db.StoreCategories.Where(x => x.Deleted)
                .Select(x => new RecicleItem() { Name = x.Name, Code = "c" + x.ID }));
            recicle.AddRange(db.StoreProducts.Where(x => x.Deleted).Select(x => new RecicleItem() { Name = x.Name, Code = "p" + x.ID }));
            return View(recicle);
        }

        [AuthorizeMaster]
        [HttpPost]
        public ActionResult Recicle(FormCollection collection)
        {
            if (!collection["Restore"].IsNullOrEmpty())
            {
                foreach (string key in collection.Keys)
                {
                    if (key.StartsWith("p"))
                    {
                        var p = db.StoreProducts.FirstOrDefault(x => x.ID == key.Substring(1).ToInt());
                        if (p != null)
                        {
                            p.Deleted = false;
                            db.SubmitChanges();
                            CatalogBrowser.Init().ClearAllCaches();
                        }
                    }
                    if (key.StartsWith("c"))
                    {
                        var c = db.StoreCategories.FirstOrDefault(x => x.ID == key.Substring(1).ToInt());
                        if (c != null)
                        {
                            RestoreCats(c);
                            db.SubmitChanges();
                            CatalogBrowser.Init().ClearAllCaches();
                        }
                    }
                    if (key.StartsWith("x"))
                    {
                        var page = db.CMSPages.FirstOrDefault(x => x.ID == key.Substring(1).ToInt());
                        if (page != null)
                        {
                            RestorePages(page);
                            db.SubmitChanges();
                            CMSPage.ClearAllCache();
                        }
                    }
                }
            }
            else if (!collection["Delete"].IsNullOrEmpty())
            {
                foreach (string key in collection.Keys)
                {
                    if (key.StartsWith("p"))
                    {
                        var p = db.StoreProducts.FirstOrDefault(x => x.ID == key.Substring(1).ToInt());
                        if (p != null)
                        {
                            DeleteProductData(p);
                            db.StoreProducts.DeleteOnSubmit(p);
                            db.SubmitChanges();
                            CatalogBrowser.Init().ClearAllCaches();
                        }
                    }
                    if (key.StartsWith("c"))
                    {
                        var c = db.StoreCategories.FirstOrDefault(x => x.ID == key.Substring(1).ToInt());
                        if (c != null)
                        {
                            DeleteCategory(c);
                            CatalogBrowser.Init().ClearAllCaches();
                        }
                    }
                    if (key.StartsWith("x"))
                    {
                        var page = db.CMSPages.FirstOrDefault(x => x.ID == key.Substring(1).ToInt());
                        if (page != null)
                        {
                            DeletePage(page);
                            CMSPage.ClearAllCache();
                        }
                    }
                }
            }
            else if (!collection["Clear"].IsNullOrEmpty())
            {
                var recicle = db.CMSPages.Where(x => x.Deleted).ToList().Select(x => x.LoadLangValues()).Select(x => new RecicleItem() { Name = x.PageName, Code = "x" + x.ID }).ToList();
                recicle.AddRange(db.StoreCategories.Where(x => x.Deleted)
                    .Select(x => new RecicleItem() { Name = x.Name, Code = "c" + x.ID }));
                recicle.AddRange(db.StoreProducts.Where(x => x.Deleted).Select(x => new RecicleItem() { Name = x.Name, Code = "p" + x.ID }));
                foreach (var item in recicle)
                {
                    if (item.Code.StartsWith("p"))
                    {
                        var p = db.StoreProducts.FirstOrDefault(x => x.ID == item.Code.Substring(1).ToInt());
                        if (p != null)
                        {
                            DeleteProductData(p);
                            db.StoreProducts.DeleteOnSubmit(p);
                            db.SubmitChanges();
                            CatalogBrowser.Init().ClearAllCaches();
                        }
                    }
                    if (item.Code.StartsWith("c"))
                    {
                        var c = db.StoreCategories.FirstOrDefault(x => x.ID == item.Code.Substring(1).ToInt());
                        if (c != null)
                        {
                            DeleteCategory(c);
                            CatalogBrowser.Init().ClearAllCaches();
                        }
                    }
                    if (item.Code.StartsWith("x"))
                    {
                        var page = db.CMSPages.FirstOrDefault(x => x.ID == item.Code.Substring(1).ToInt());
                        if (page != null)
                        {
                            DeletePage(page);
                            CMSPage.ClearAllCache();
                        }
                    }
                }
            }


            return Recicle();
        }

        private void DeleteCategory(object row)
        {
            var tr = db.StoreCategories.FirstOrDefault(x => x.ID == ((StoreCategory)row).ID);
            if (tr == null)
                return;
            if (tr.Children.Any())
            {
                var arr = tr.Children.ToArray();
                foreach (var child in arr)
                {
                    DeleteCategory(child);
                }
            }
            db.Refresh(RefreshMode.KeepChanges, tr);
            db.StoreCategories.DeleteOnSubmit(tr);
            db.SubmitChanges();


        }
        private void DeletePage(object row)
        {
            var tr = db.CMSPages.FirstOrDefault(x => x.ID == ((CMSPage)row).ID);
            if (tr == null)
                return;
            if (tr.Children.Any())
            {
                var arr = tr.Children.ToArray();
                foreach (var child in arr)
                {
                    DeletePage(child);
                }
            }
            db.Refresh(RefreshMode.KeepChanges, tr);
            db.CMSPages.DeleteOnSubmit(tr);
            db.SubmitChanges();


        }

        private void DeleteProductData(object row)
        {
            var p = (StoreProduct)row;
            var rels = db.StoreProductRelations.Where(x => x.BaseProductID == p.ID || x.RelatedProductID == p.ID);
            if (rels.Any())
            {
                db.StoreProductRelations.DeleteAllOnSubmit(rels);
                db.SubmitChanges();
            }

            var catrels = db.StoreCategoryRelations.Where(x => x.RelatedProductID == p.ID);
            if (catrels.Any())
            {
                db.StoreCategoryRelations.DeleteAllOnSubmit(catrels);
                db.SubmitChanges();

            }

        }

        public ContentResult CreateModuls()
        {
            var veiws = db.CMSPageCellViews.ToList();
            var pages = db.CMSPages.ToList();
            var processed = new List<int>();
            foreach (var cmsPage in pages)
            {
                if (processed.Contains(cmsPage.PageType.ID))
                    continue;

                var typedPages = pages.Where(x => x.PageType.ID == cmsPage.PageType.ID).ToList();
                var views = veiws.Where(x => x.CMSPageCell.PageType.ID == cmsPage.PageType.ID).ToList();

                var cnt = 0;
                foreach (var page in typedPages)
                {
                    foreach (var view in views)
                    {
                        var texts = new List<CMSPageTextData>();
                        if (view.Controller == "TextPage")
                        {
                            texts = db.CMSPageTextDatas.Where(x => x.CMSPageID == page.ID && x.ViewID == view.ID).ToList();
                        }

                        if (cnt == 0)
                        {
                            view.PageID = page.ID;
                            db.SubmitChanges();
                        }
                        else
                        {
                            var v = new CMSPageCellView()
                            {
                                Action = view.Action,
                                Controller = view.Controller,
                                CellID = view.CellID,
                                Path = view.Path,
                                Description = view.Description,
                                OrderNum = view.OrderNum,
                                PageID = page.ID
                            };
                            db.CMSPageCellViews.InsertOnSubmit(v);


                            if (texts.Any())
                            {
                                foreach (var text in texts)
                                {
                                    text.CMSPageCellView = v;
                                }

                            }

                            db.SubmitChanges();
                        }

                    }

                    cnt++;




                }

                processed.Add(cmsPage.PageType.ID);

            }

            return new ContentResult() { Content = "1" };
        }

        private void RestorePages(CMSPage page)
        {
            page.Deleted = false;
            if (page.Children.Any())
            {
                foreach (var cmsPage in page.Children)
                {
                    RestorePages(cmsPage);
                }
            }
        }

        private void RestoreCats(StoreCategory storeCategory)
        {
            storeCategory.Deleted = false;

            if (storeCategory.Children.Any())
            {
                foreach (var child in storeCategory.Children)
                {
                    RestoreCats(child);
                }
            }
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult SaveModulOrder(int cell, string list, int pageID)
        {
            var ids = list.Split<int>().ToList();
            var modulsOrdered = ids.Select((x, index) => new { ID = x, Num = index }).ToList();
            var moduls = db.CMSPageCellViews.Where(x => ids.Contains(x.ID));
            foreach (var modul in moduls)
            {
                var order = modulsOrdered.FirstOrDefault(x => x.ID == modul.ID);
                if (order != null)
                {
                    modul.OrderNum = order.Num + 1;
                }
                modul.CellID = cell;
            }
            db.SubmitChanges();

            return RedirectToAction("UniversalModuls", "Pages", new { HideSettings = 1, pageID = pageID });

        }
        [HttpPost]
        [AuthorizeMaster]
        public ActionResult DeleteModul(int id, int pageID)
        {
            var modul = db.CMSPageCellViews.FirstOrDefault(x => x.ID == id && x.PageID == pageID);
            if (modul != null)
            {
                db.CMSPageCellViews.DeleteOnSubmit(modul);
                db.SubmitChanges();
            }


            return RedirectToAction("UniversalModuls", "Pages", new { HideSettings = 1, pageID = pageID });

        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult SaveModulOrderNew(int cell, string list, int pageID)
        {
            var ids = list.Split<string>().ToList();
            var modulsOrdered = ids.Select((x, index) => new { ID = x, Num = index }).ToList();

            foreach (var order in modulsOrdered)
            {
                if (order.ID.IsInt())
                {
                    var modul = db.CMSPageCellViews.FirstOrDefault(x => x.ID == order.ID.ToInt());
                    if (modul != null)
                    {
                        modul.OrderNum = order.Num + 1;
                        modul.CellID = cell;
                    }
                }
                else
                {
                    var tmpl =
                        db.CMSPageAllowedClientModuls.FirstOrDefault(x => x.ID == order.ID.Replace("M", "").ToInt());

                    if (tmpl != null)
                    {
                        var modul = new CMSPageCellView()
                        {
                            Action = tmpl.Action,
                            Controller = tmpl.Controller,
                            Path = string.Format("~/Views/{0}/{1}.cshtml", tmpl.Controller, tmpl.Action),
                            CellID = cell,
                            OrderNum = order.Num,
                            PageID = pageID

                        };
                        db.CMSPageCellViews.InsertOnSubmit(modul);
                    }
                }
            }
            db.SubmitChanges();

            return RedirectToAction("UniversalModuls", "Pages", new { HideSettings = 1, pageID = pageID });

        }

        private static JsonResult _getTreeDataMenu
        {
            get { return System.Web.HttpContext.Current.Cache["PagesAnsCategoriesAndProducts"] as JsonResult; }
            set
            {
                System.Web.HttpContext.Current.Cache.Add("PagesAnsCategoriesAndProducts", value, null,
                    DateTime.Now.AddHours(2), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
        }
        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeData(string SelectedSection, bool? prods, bool isReload = false)
        {
            prods = prods ?? false;
            var list = new List<string>();
            if (SelectedSection.IsFilled())
            {
                list.Add(SelectedSection);

            }
            var treeData = new UniversalTreeDataSource() { CheckedNodes = list, AddCountToCats = prods.Value };
            if (isReload)
            {
                _getTreeDataMenu = treeData.Serialize(SerializationType.PagesAnsCategoriesAndProducts);
            }
            return _getTreeDataMenu ??
                   (_getTreeDataMenu = treeData.Serialize(SerializationType.PagesAnsCategoriesAndProducts));
        }


        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeDataPartial(string nodeId)
        {
            var treeData = new UniversalTreeDataSource() { PartialNodeParent = nodeId };
            return treeData.Serialize(SerializationType.Partial);
        }



        private JsonResult _getTreeDataForPage;

        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeDataForPage(string SelectedSection)
        {
            var list = new List<string>();
            if (SelectedSection.IsFilled())
            {
                list.Add(SelectedSection);

            }
            var treeData = new UniversalTreeDataSource() { CheckedNodes = list, AddCountToCats = false };

            return _getTreeDataForPage ??
                   (_getTreeDataForPage = treeData.Serialize(SerializationType.Pages));
        }

        [HttpGet]
        [AuthorizeMaster]

        public ActionResult Categories(int ParentID)
        {

            return PartialView(db.StoreCategories.FirstOrDefault(x => x.ID == ParentID));
        }
        public ActionResult ChangeOrder(string Type, int CategoryID)
        {
            var db = new DB();
            var prod = db.StoreCategories.FirstOrDefault(x => x.ID == CategoryID);
            if (prod != null)
            {
                if (Type == "up")
                {
                    var prev =
                        db.StoreCategories.Where(x => x.ParentID == prod.ParentID && !x.Deleted)
                            .OrderBy(x => x.OrderNum).ToList()
                            .LastOrDefault(x => x.OrderNum < prod.OrderNum);
                    if (prev != null)
                    {
                        var prevOrder = prev.OrderNum;
                        prev.OrderNum = prod.OrderNum;
                        prod.OrderNum = prevOrder;
                    }
                }
                if (Type == "down")
                {
                    var next =
                        db.StoreCategories.Where(x => x.ParentID == prod.ParentID && !x.Deleted)
                            .OrderBy(x => x.OrderNum)
                            .FirstOrDefault(x => x.OrderNum > prod.OrderNum);
                    if (next != null)
                    {
                        var nextOrder = next.OrderNum;
                        next.OrderNum = prod.OrderNum;
                        prod.OrderNum = nextOrder;
                    }
                }
                db.SubmitChanges();
            }
            return new ContentResult();
        }
    }
}
