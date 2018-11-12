using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class ClientCatalogController : Controller
    {

        

        public ActionResult FastSearch(string query, int CategoryID)
        {
            var s = new JavaScriptSerializer();
            var list = s.Deserialize<List<FilterQueryEntry>>(query);

            var products = CategoryFilter.SearchCount(CategoryID, list);

            var url = Models.CatalogBrowser.CategoriesList.FirstOrDefault(x => x.Value.ID == CategoryID).Value.FullUrl;
            url += "?query=" + query;
            return new JsonResult() { Data = new { count = products, link = url } };
        }

        private DB db = new DB();

        [MenuItem("Каталог", ID = 4, Icon = "new_window")]  
        public ActionResult IndexList()
        {
            return View();
        }

        [MenuItem("Связи товаров", ID = 49, ParentID = 4, Icon = "connectproduct")]
        public ActionResult ProductRelationEditor()
        {
            return View();
        }
        [MenuItem("Связи категорий", ID = 50, ParentID = 4, Icon = "connectcategory")]
        public ActionResult CategoryRelationEditor()
        {
            return View();
        }

        [ClientTemplate("Сравнение товаров")]
        public ActionResult ComparedProducts()
        {
            var cook = Request.Cookies.Get("ForCompare");
            if (cook != null)
            {
                var ids = HttpUtility.UrlDecode(cook.Value).Split<int>(";").ToList();
                var prods = db.StoreProducts.Where(x => ids.Contains(x.ID)).Where(x=> !x.Deleted);
                return PartialView(prods.ToList());
            }
            return PartialView();

        }

        [ClientTemplate("Список товаров для сравнения")]
        public ActionResult ComparedProductsList()
        {
            var cook = Request.Cookies.Get("ForCompare");
            if (cook != null)
            {
                var ids = HttpUtility.UrlDecode(cook.Value).Split<int>(";").ToList();
                var prods = db.StoreProducts.Where(x => ids.Contains(x.ID)).Where(x=> !x.Deleted);
                return PartialView(prods.ToList());
            }
            return PartialView();
        }

        [ClientTemplate("Последние просмотренные продукты")]
        public ActionResult CatalogVisited()
        {
            var visited = new List<string>();
            if (Request.Cookies.AllKeys.Contains("ReviewPath"))
            {
                var cook = Request.Cookies["ReviewPath"];
                if (cook != null)
                    visited = HttpUtility.UrlDecode(cook.Value).Split<string>(";").ToList();
            }
            var cItems = new List<CatalogItem>();
            foreach (var v in visited)
            {
                if (v.StartsWith("p"))
                {
                    var p = db.StoreProducts.FirstOrDefault(x => x.ID == v.Substring(1).ToInt()&& !x.Deleted);
                    if (p != null)
                    {
                        cItems.Add(new CatalogItem() {Image = p.DefaultThumbURL, Link = p.FullUrl, Name = p.Name, Price = p.PriceModule.ShopCartPrice, ID = p.ID});
                    }
                }
                if (v.StartsWith("c"))
                {
                    var c = db.StoreCategories.FirstOrDefault(x => x.ID == v.Substring(1).ToInt() && !x.Deleted);
                    if (c != null)
                    {
                        cItems.Add(new CatalogItem() {Image = c.ImageUrl, Link = c.FullUrl, Name = c.Name});
                    }
                }
            }
            return PartialView(cItems);

        }

        public ActionResult CatalogRelatedProducts(int ProductID)
        {
            var data = db.StoreProductRelations.Where(x => x.BaseProductID == ProductID && x.BaseProductReverse != null).AsEnumerable()
              .GroupBy(x => x.GroupName);
            return PartialView(data);
        }

        public ActionResult CatalogRelatedProductsCategories(int CategoryID)
        {
            var data = db.StoreCategoryRelations.Where(x => x.BaseCategoryID == CategoryID && x.StoreProduct != null).AsEnumerable()
              .GroupBy(x => x.GroupName);
            return PartialView(data);
        }

        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeData(int? product, string selector)
        {
            var list = new List<string>();
            if (product.HasValue)
            {
                var prods =
                    db.StoreProductRelations.Where(
                        x => x.BaseProductID == product && x.BaseProductReverse != null && x.GroupName == selector)
                      .Select(x => "p" + x.RelatedProductID);
                list.AddRange(prods);

            }
            var treeData = new UniversalTreeDataSource() { CheckedNodes = list};
            return treeData.Serialize(SerializationType.FullCatalog);

        }  
        
        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeDataCategory(int? category, string selector)
        {
            var list = new List<string>();
            if (category.HasValue)
            {
                var prods =
                    db.StoreCategoryRelations.Where(
                        x => x.BaseCategoryID == category && x.RelatedProductID.HasValue && x.GroupName == selector)
                      .Select(x => "p" + x.RelatedProductID);
                list.AddRange(prods);

            }
            var treeData = new UniversalTreeDataSource() { CheckedNodes = list};
            return treeData.Serialize(category.HasValue ? SerializationType.FullCatalog : SerializationType.Categories);

        }  
   
        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getRelData(int product, string selector)
        {
            var data = db.StoreProductRelations.Where(x => x.BaseProductID == product && x.GroupName == selector)
              .Where(x => x.BaseProductReverse != null)
              .Select(x => new JSRelatedProduct() {ID = "p"+ x.BaseProductReverse.ID, Name = x.BaseProductReverse.Name}).ToArray();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getRelDataCategory(int category, string selector)
        {
            var data = db.StoreCategoryRelations.Where(x => x.BaseCategoryID == category && x.GroupName == selector)
              .Where(x => x.RelatedProductID.HasValue)
              .Select(x => new JSRelatedProduct() {ID = "p"+ x.RelatedProductID, Name = x.StoreProduct.Name}).ToArray();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult saveRelations(int baseID, string choosed, string group)
        {
            var ids = choosed.Split<string>(";").Select(x => x.Substring(1).ToInt()).ToList();
            var rels = db.StoreProductRelations.Where(x => x.BaseProductID == baseID && x.GroupName == group);
            var forDel = rels.Where(x => !ids.Contains(x.RelatedProductID??0) || x.BaseProductReverse == null);
            var forAdd = ids.Where(x => !rels.Any(z => z.RelatedProductID == x)).ToList();
            if (forDel.Any())
            {
                db.StoreProductRelations.DeleteAllOnSubmit(forDel);
                db.SubmitChanges();
            }
            if (forAdd.Any())
            {
                db.StoreProductRelations.InsertAllOnSubmit(
                    forAdd.Select(
                        x => new StoreProductRelation() {BaseProductID = baseID, GroupName = group, RelatedProductID = x}));
                db.SubmitChanges();
            }
            return new ContentResult();
        } 
        
        [HttpPost]
        [AuthorizeMaster]
        public ActionResult saveRelationsCategory(int baseID, string choosed, string group)
        {
            var ids = choosed.Split<string>(";").Select(x => x.Substring(1).ToInt()).ToList();
            var rels = db.StoreCategoryRelations.Where(x => x.BaseCategoryID == baseID && x.GroupName == group);
            var forDel = rels.Where(x => !ids.Contains(x.RelatedProductID??0) || x.StoreProduct == null);
            var forAdd = ids.Where(x => !rels.Any(z => z.RelatedProductID == x)).ToList();
            if (forDel.Any())
            {
                db.StoreCategoryRelations.DeleteAllOnSubmit(forDel);
                db.SubmitChanges();
            }
            if (forAdd.Any())
            {
                db.StoreCategoryRelations.InsertAllOnSubmit(
                    forAdd.Select(
                        x => new StoreCategoryRelation() { BaseCategoryID = baseID, GroupName = group, RelatedProductID = x }));
                db.SubmitChanges();
            }
            return new ContentResult();
        }
        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getNewRelData(string exist, string added, string removed)
        {
            var addedCats =
                added.Split<string>(";")
                     .Where(x => x.StartsWith("x"))
                     .Select(x => x.Substring(1).ToInt())
                     .SelectMany(Models.CatalogBrowser.GetChildrenCategories)
                     .Distinct().ToList();

            var prodsIds =
                exist.Split<string>(";")
                     .Select(x => x.Substring(1).ToInt())
                     .Concat(added.Split<string>(";").Where(x => x.StartsWith("p")).Select(x => x.Substring(1).ToInt()))
                     .Distinct().ToList();

            var prods =
                db.StoreProductsToCategories.Where(x => addedCats.Contains(x.CategoryID))
                  .Select(x => x.StoreProduct);

            if (prods.Any())
            {
                prods = prods.Concat(db.StoreProducts.Where(x => prodsIds.Contains(x.ID) && !x.Deleted)).Distinct();
            }
            else
            {
                prods = db.StoreProducts.Where(x => prodsIds.Contains(x.ID) && !x.Deleted).Distinct();
            }
                  
                  
                  

            if (removed != ";")
            {
                if (removed.StartsWith("p"))
                    prods = prods.Where(x => x.ID != removed.Substring(1).ToInt());
                else if (removed.StartsWith("x"))
                {
                    var removedCats =
                        Models.CatalogBrowser.GetChildrenCategories(removed.Substring(1).ToInt())
                              .Distinct().ToList();
                    prods = prods.Where(x => !x.StoreProductsToCategories.Any(z => removedCats.Contains(z.CategoryID)));

                }
            }

            return Json(prods.OrderBy(x => x.Name).Select(x => new JSRelatedProduct() { ID = "p" + x.ID, Name = x.Name }), JsonRequestBehavior.AllowGet);

        }



        [ClientTemplate("Горячие скидки")] 
        public ActionResult CatalogPromoHot()
        {
            ViewBag.ShopCart = new ShopCart().InitCart();
            return PartialView(db.StoreProductBlocks.Where(x => x.GroupName == "hot"));
        }

        [ClientTemplate("Хиты сезона")] 
        public ActionResult CatalogPromoTrend()
        {
            ViewBag.ShopCart = new ShopCart().InitCart();
            return PartialView(db.StoreProductBlocks.Where(x => x.GroupName == "trend"));
        }

        /*[ClientTemplate("Фильтр товаров")] */
        public ActionResult CatalogFilter()
        {
            return PartialView(new CatalogCharacterFilters());
        }

        [ClientTemplate("Фильтр по радиаторам")] 
        public ActionResult RadiatorFilter()
        {
            return PartialView(new RadiatorFilter());
        }
        [ClientTemplate("Фильтр по радиаторам с анимацией")] 
        public ActionResult RadiatorFilterV2()
        {
            return PartialView( new RadiatorFilter());
        }        
        [ClientTemplate("Фильтр по радиаторам (над списком товаров)")] 
        public ActionResult RadiatorFilterV3()
        {
            return PartialView( new RadiatorFilter(true, true));
        }

        [HttpPost]
        public ActionResult ToFavoriteSimple(int ProductID)
        {
            bool added = false;
            var exist =
                db.StoreProductFavorites.FirstOrDefault(
                    x => x.ProductID == ProductID && x.UserID == HttpContext.GetCurrentUserUID());
            try
            {
                if (exist == null)
                {
                    db.StoreProductFavorites.InsertOnSubmit(new StoreProductFavorite()
                    {
                        ProductID = ProductID,
                        UserID = HttpContext.GetCurrentUserUID()
                    });
                    added = true;
                }
                else
                {
                    db.StoreProductFavorites.DeleteOnSubmit(exist);
                    added = false;
                }
            }
            catch
            {

            }
            db.SubmitChanges();
            ViewBag.ShopCart = new ShopCart().InitCart();
            return new ContentResult() { Content = added.ToIntString() };

        }

        public ActionResult FilterCount(string args, int pageID, string names = "", string objID = "")
        {
            var pairs =
                args.Split<string>("^^").Select(x => x.Split<string>("::"))
                    .Select(x => new { Key = x.ElementAt(0).ToInt(), Values = x.Skip(1).ToList() }).ToList();



            var model = names.IsNullOrEmpty()
                ? new CatalogCharacterFilters(objID)
                : new CatalogCharacterFilters(names.Split<string>(";").ToList());

            foreach (var filter in model.Filters)
            {
                var pair = pairs.FirstOrDefault(x => x.Key == filter.CharacterID);
                if (pair != null)
                {
                    if (pair.Values.Count == 1 && pair.Values.ElementAt(0).Contains(";"))
                    {
                        filter.FilteredValues = pair.Values.ElementAt(0).Split<string>(";").ToList();
                    }
                    else filter.FilteredValues = pair.Values;
                }
            }

            return Json(model.Calculate(pageID));
        }

        public ActionResult ToFavorite(int ProductID, bool? InPopup)
        {
            ToFavoriteSimple(ProductID);
            var filter = new CatalogFilter();

            if (!InPopup.HasValue)
                return PartialView(filter.ProductViewShort,
                                   db.StoreProducts.FirstOrDefault(x => x.ID == ProductID));
            return PartialView("PopupDescription", db.StoreProducts.FirstOrDefault(x => x.ID == ProductID));

        }
        [HttpGet]
        public ActionResult PopupDescription(int ProductID)
        {
            ViewBag.ShopCart = new ShopCart().InitCart();
            return PartialView(db.StoreProducts.FirstOrDefault(x => x.ID == ProductID));
        }
        [HttpGet]
        public ActionResult CatalogListProdictTile(int ProductID)
        {
            ViewBag.ShopCart = new ShopCart().InitCart();
            return PartialView(db.StoreProducts.FirstOrDefault(x => x.ID == ProductID));
        }
        [HttpGet]
        public ActionResult CatalogListProdictLine(int ProductID)
        {
            ViewBag.ShopCart = new ShopCart().InitCart();
            return PartialView(db.StoreProducts.FirstOrDefault(x => x.ID == ProductID));
        }       
        
        [HttpGet]
        public ActionResult CatalogListProdictRow(int ProductID)
        {
            ViewBag.ShopCart = new ShopCart().InitCart();
            return PartialView(db.StoreProducts.FirstOrDefault(x => x.ID == ProductID));
        }

        public ActionResult CatalogChildren()
        {
            return PartialView(Models.CatalogBrowser.Init().CurrentCategory.Children.DistinctBy(x=> x.ID));
        }


        public ActionResult FancyVideo(int? ID)
        {
            var img = db.StoreImages.FirstOrDefault(x => x.ID == (ID ?? 0));

            return View(img);
        }

        public ActionResult CatalogSection()
        {
            return PartialView(Models.CatalogBrowser.Init().CurrentCategory);
        }

        [AuthorizeMaster]
        public ActionResult Photo3DGallery(int CategoryID, int ProductID)
        {
            var photo = db.StorePhoto3Ds.FirstOrDefault(x => x.ProductID == ProductID);
            if (photo == null)
            {
                photo = new StorePhoto3D() { ProductID = ProductID, URL = Generate3dphotoUrl() };
                db.StorePhoto3Ds.InsertOnSubmit(photo);
                db.SubmitChanges();
            }
            return PartialView(photo);
        }

        public String Generate3dphotoUrl()
        {
            string path = DateTime.Now.ToFileTimeUtc().ToString();
            path = path.Replace(".", "");
            return path;
        }

        [ClientTemplate("Меню разделов каталога в подвале сайта")]
        public ActionResult FooterMenu()
        {
            return PartialView(SiteMapItem.GetCatalogRoot(null));
        }

        [ClientTemplate("Каталог товаров")]
        public ActionResult CatalogBrowser()
        {
            var browser = Models.CatalogBrowser.Init();

            if (browser.IsProductPage && Request.RawUrl.Count(x => x == '/') > 3)
            {
                System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
                System.Web.HttpContext.Current.Response.AddHeader("Location", browser.CurrentProduct.FullUrl);

            }

            return PartialView(browser);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveMark(int mark, int book)
        {
            if (mark < 1 || mark > 5)
                return new ContentResult() { Content = "-1" };
            var av = db.StoreProducts.FirstOrDefault(x => x.ID == book);
            av.VoteSum += mark;
            av.VoteCount++;
            av.VoteOverage = (decimal)av.VoteSum / (decimal)av.VoteCount;
            db.SubmitChanges();
            return PartialView("~/Views/ClientCatalog/ProductRating.cshtml", av);
            return new ContentResult() { Content = av.AverageRounded.ToString() };
        }

    }
}
