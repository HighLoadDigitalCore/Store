using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using NPOI.SS.Formula.Functions;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;

namespace Smoking.Models
{
    #region Делегаты
    public delegate string TextFunctionDelegate(object input);
    public delegate bool PageCheckFunctionDelegate(CMSPage input);
    public delegate string SaveFunctionDelegate(object row, DB db, HttpContextBase context = null);
    public delegate string CompleteSaveFunctionDelegate(UniversalEditorPagedData data, object row);
    public delegate object BeforeSaveFunctionDelegate(object row, DB db);
    public delegate void BeforeDeleteFunctionDelegate(object row, DB db);

    #endregion
    #region Классы для древовидных данных
    [Serializable]
    public class TreeProductListItem
    {
        public int ID { get; set; }
        public string Checked { get; set; }
        public string Name { get; set; }
    }

    public class TreeProductDataSource : TreeDataSource
    {
        public string ItemsDataHandler { get; set; }
        public string SaveDataHandler { get; set; }
        public string GroupName { get; set; }

        public int Branch { get; set; }
    }

    public class TreeDataSource
    {
        public string DataLink { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Values { get; set; }
    }

    public class RecicleItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class UniversalTreeDataSource : UniversalDataSource
    {
        public string PartialNodeParent { get; set; }
        public TextFunctionDelegate LinkFunction { get; set; }
        public PageCheckFunctionDelegate PageCheckFunction { get; set; }
        public object CheckedNodes { get; set; }
        public bool AddCountToCats { get; set; }
        public SerializationType Type { get; set; }
        public JsonResult Serialize()
        {
            return Serialize(Type);
        }
        public JsonResult Serialize(SerializationType type)
        {
            var result = new JsonResult();
            if (type != SerializationType.Partial)
            {
                var root = CreateModel(type);
                result.Data = root;
            }
            else
            {
                var root = CreatePartial();
                result.Data = root;
            }
            result.MaxJsonLength = int.MaxValue;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.ContentType = "application/json";
            return result;

        }

        private object CreatePartial()
        {
            if (PartialNodeParent.IsNullOrEmpty())
            {
                var root = new JsTreeModel();
                root.data = AccessHelper.SiteName.ToNiceForm();
                root.attr = new JsTreeAttribute() { id = "x0", href = "#", uid = 0 };
                root.children = new List<JsTreeModel>();

                root.children = GetLevel(null, true, true, true);
                return root;
            }
            else
            {
                var isCat = PartialNodeParent.StartsWith("c") || (PartialNodeParent == "x"+ CatalogBrowser.ParentPage.ID);
                var isPg = PartialNodeParent.StartsWith("x");
                var isPd = PartialNodeParent.StartsWith("p");
                var id = PartialNodeParent.Substring(1).ToInt();

                if (PartialNodeParent == "x" + CatalogBrowser.ParentPage.ID)
                {
                    id = CatalogBrowser.Root.ID;
                }

                return GetLevel(id, isCat, isPg, isCat);
            }
        }

        public JsTreeModel CreateModel()
        {
            return CreateModel(Type);
        }
        public JsTreeModel CreateModel(SerializationType type)
        {
            var root = new JsTreeModel();
            switch (type)
            {
                case SerializationType.Pages:


                    var cc = new List<string>();
                    if (CheckedNodes is List<string>)
                        cc = CheckedNodes as List<string>;

                    root.attr = new JsTreeAttribute
                        {
                            id = "x0",
                            href = "#",
                            uid = 0,
                            @class = cc.Contains("0") ? "jstree-checked" : ""
                        };
                    root.children = new List<JsTreeModel>();


                    root.data = AccessHelper.SiteName.ToNiceForm();

                    FillPagesModel(ref root, null);
                    break;
                case SerializationType.PagesAnsCategories:
                    root.data = AccessHelper.SiteName.ToNiceForm();
                    root.attr = new JsTreeAttribute() { id = "x0", href = "#", uid = 0 };
                    root.children = new List<JsTreeModel>();

                    FillPagesAndCatsModel(ref root, null);
                    break;
                case SerializationType.PagesAnsCategoriesAndProducts:
                    root.data = AccessHelper.SiteName.ToNiceForm();
                    root.attr = new JsTreeAttribute() { id = "x0", href = "#", uid = 0 };
                    root.children = new List<JsTreeModel>();

                    FillPagesAndCatsAndProductsModel(ref root, null);
                    if (AddCountToCats)
                    {

                        RenameWithCounts(ref root);
                    }
                    break;
                case SerializationType.Categories:
                    var rootPage = CatalogBrowser.CategoriesList.FirstOrDefault(x => x.Value.ID == 1).Value;
                    root.data = rootPage.Name;
                    var rels = new List<StoreProductsToCategory>();
                    if (CheckedNodes is List<StoreProductsToCategory>)
                        rels = CheckedNodes as List<StoreProductsToCategory>;

                    var crels = new List<StoreCategory>();
                    if (CheckedNodes is List<StoreCategory>)
                        crels = CheckedNodes as List<StoreCategory>;

                    root.attr = new JsTreeAttribute
                        {
                            id = "x" + rootPage.ID,
                            href = LinkFunction == null ? "#" : LinkFunction(rootPage.ID),
                            uid = rootPage.ID,
                            @class = rels.Any(z => z.CategoryID == rootPage.ID) || crels.Any(z => z.ID == rootPage.ID) ? "jstree-checked" : ""
                        };
                    root.children = new List<JsTreeModel>();
                    FillCategoriesModel(ref root, rootPage.ID);
                    break;
                case SerializationType.FullCatalog:
                    var rootCat = CatalogBrowser.CategoriesList.FirstOrDefault(x => x.Value.ID == 1).Value;
                    root.data = rootCat.Name;
                    var chc = new List<string>();
                    if (CheckedNodes is List<string>)
                        chc = CheckedNodes as List<string>;

                    root.attr = new JsTreeAttribute
                        {
                            id = "x" + rootCat.ID,
                            href = LinkFunction == null ? "#" : LinkFunction(rootCat.ID),
                            uid = rootCat.ID,
                            @class = chc.Contains("x" + rootCat.ID) ? "jstree-checked" : ""
                        };
                    root.children = new List<JsTreeModel>();
                    FillFullCatalogModel(ref root, rootCat.ID);

                    break;
            }
            return root;

        }

        private int RenameWithCounts(ref JsTreeModel root)
        {
            var cnt = 0;

            if (root.children != null && root.children.Any())
            {
                cnt += root.children.Sum(t => RenameWithCounts(ref t));
                /*
                                foreach (JsTreeModel child in root.children)
                                {
                    
                                }
                */
            }
            cnt += root.attr.cnt;

            if (root.attr.id == "x" + CatalogBrowser.ParentPage.ID || root.attr.id.StartsWith("c") && cnt > 0)
            {
                //root.data = string.Format("{0} ({1})", root.data, cnt);
                root.attr.allcnt = cnt;
            }
            if (root.attr.id.StartsWith("x") && root.attr.id != "x" + CatalogBrowser.ParentPage.ID)
            {
                var id = root.attr.uid;
                root.attr.pagecnt = CMSPage.FullPageTable.Where(x => !x.Deleted).Count(x => x.ParentID == id);
            }
            if (root.attr.id.StartsWith("c") || root.attr.id == "x" + CatalogBrowser.ParentPage.ID)
            {
                var id = root.attr.uid;
                root.attr.catcnt = CatalogBrowser.CategoriesList.Where(x => !x.Value.Deleted).Select(x => x.Value).Count(x => x.ParentID == id);
            }
            return cnt;
        }

        private void FillFullCatalogModel(ref JsTreeModel model, int id)
        {
            var chc = new List<string>();
            if (CheckedNodes is List<string>)
                chc = CheckedNodes as List<string>;

            var pages = CatalogBrowser.CategoriesList.Where(x => x.Value.ParentID == id).OrderBy(x => x.Value.OrderNum);
            foreach (var cmsPage in pages)
            {
                var child = new JsTreeModel
                    {
                        data = TextFunction == null ? cmsPage.Value.Name : TextFunction(cmsPage.Value.Name),
                        attr =
                            new JsTreeAttribute
                                {
                                    id = "x" + cmsPage.Value.ID,
                                    url = cmsPage.Value.FullUrl,
                                    href = LinkFunction == null ? "#" : LinkFunction(cmsPage.Value.ID),
                                    uid = cmsPage.Value.ID,
                                    @class = chc.Contains("x" + cmsPage.Value.ID) ? "jstree-checked" : ""
                                },

                    };
                if (model.children == null)
                    model.children = new List<JsTreeModel>();
                model.children.Add(child);

                var prods = cmsPage.Value.StoreProductsToCategories.OrderBy(x => x.OrderNum).Select(x => x.StoreProduct).Where(x => !x.Deleted);
                foreach (var product in prods)
                {
                    var cp = new JsTreeModel
                    {
                        data = TextFunction == null ? product.Name : TextFunction(product.Name),
                        attr =
                            new JsTreeAttribute
                            {
                                id = "p" + product.ID,
                                url = product.FullUrl,
                                href = LinkFunction == null ? "#" : LinkFunction(product.ID),
                                uid = product.ID,
                                @class = "prod-item" + (chc.Contains("p" + product.ID) ? " jstree-checked" : "")
                            },

                    };
                    if (child.children == null)
                        child.children = new List<JsTreeModel>();
                    child.children.Add(cp);

                }

                FillFullCatalogModel(ref child, cmsPage.Value.ID);
            }

        }

        private void FillCategoriesModel(ref JsTreeModel model, int id)
        {
            var rels = new List<StoreProductsToCategory>();
            var crels = new List<StoreCategory>();
            if (CheckedNodes is List<StoreProductsToCategory>)
                rels = CheckedNodes as List<StoreProductsToCategory>;
            if (CheckedNodes is List<StoreCategory>)
                crels = CheckedNodes as List<StoreCategory>;

            var pages = CatalogBrowser.CategoriesList.Where(x => x.Value.ParentID == id).OrderBy(x => x.Value.OrderNum);
            foreach (var cmsPage in pages)
            {
                var child = new JsTreeModel
                {
                    data = TextFunction == null ? cmsPage.Value.Name : TextFunction(cmsPage.Value.Name),
                    attr =
                        new JsTreeAttribute
                        {
                            id = "x" + cmsPage.Value.ID,
                            href = LinkFunction == null ? "#" : LinkFunction(cmsPage.Value.ID),
                            uid = cmsPage.Value.ID,
                            url = cmsPage.Value.FullUrl,
                            @class = rels.Any(z => z.CategoryID == cmsPage.Value.ID) || crels.Any(z => z.ID == cmsPage.Value.ID) ? "jstree-checked" : ""
                        },

                };
                if (model.children == null)
                    model.children = new List<JsTreeModel>();
                model.children.Add(child);
                FillCategoriesModel(ref child, cmsPage.Value.ID);
            }
        }

        private int? _mainPageID;
        private int MainPageID
        {
            get
            {
                if (!_mainPageID.HasValue)
                {
                    _mainPageID =
                        (new DB().CMSPages.FirstOrDefault(x => x.PageType.TypeName == "MainPage") ?? new CMSPage()).ID;
                }
                return _mainPageID.Value;
            }
        }

        private void FillPagesModel(ref JsTreeModel model, int? id)
        {
            var chc = new List<string>();
            if (CheckedNodes is List<string>)
                chc = CheckedNodes as List<string>;

            var pages = CMSPage.FullPageTable.Where(x => id == null ? !x.ParentID.HasValue : x.ParentID == id).Where(x => !x.Deleted).OrderBy(
                x => x.OrderNum);

            if (PageCheckFunction != null)
            {
                pages = pages.Where(x => PageCheckFunction(x)).OrderBy(x => x.OrderNum);
            }

            foreach (var cmsPage in pages)
            {
                var child = new JsTreeModel()
                    {
                        data = TextFunction == null ? cmsPage.PageName : TextFunction(cmsPage.PageName),
                        attr =
                            new JsTreeAttribute()
                                {
                                    id = "x" + cmsPage.ID,
                                    href = LinkFunction == null ? "#" : LinkFunction(cmsPage.ID),
                                    uid = cmsPage.ID,
                                    @class = (cmsPage.ID == MainPageID ? "home-icon " : "") + (chc.Contains(cmsPage.ID.ToString()) ? "jstree-checked " : ""),
                                    url = cmsPage.FullUrl
                                },
                    };
                if (model.children == null)
                    model.children = new List<JsTreeModel>();
                model.children.Add(child);
                FillPagesModel(ref child, cmsPage.ID);
            }
        }

        private void FillPagesAndCatsModel(ref JsTreeModel model, int? id, bool? catPage = null)
        {
            var old = catPage;
            if (!catPage.HasValue)
            {
                catPage = id.HasValue && id > 0 && id == (CatalogBrowser.ParentPage ?? new CMSPage()).ID;
            }
            var rels = new List<string>();
            if (CheckedNodes is List<string>)
                rels = CheckedNodes as List<string>;
            if (catPage.Value)
            {
                if (old == null)
                {
                    id = 1;
                }

                var pages = CatalogBrowser.CategoriesList.Where(x => x.Value.ParentID == id).OrderBy(x => x.Value.OrderNum);
                foreach (var cmsPage in pages)
                {
                    var child = new JsTreeModel
                    {
                        data = TextFunction == null ? cmsPage.Value.Name : TextFunction(cmsPage.Value.Name),
                        attr =
                            new JsTreeAttribute
                            {
                                id = "c" + cmsPage.Value.ID,
                                href = LinkFunction == null ? "#" : LinkFunction(cmsPage.Value.ID),
                                uid = cmsPage.Value.ID,
                                @class = rels.Any(z => z == "c" + cmsPage.Value.ID) ? "jstree-checked" : "",
                                url = cmsPage.Value.FullUrl
                            },

                    };
                    if (model.children == null)
                        model.children = new List<JsTreeModel>();
                    model.children.Add(child);
                    FillPagesAndCatsModel(ref child, cmsPage.Value.ID, true);
                }

            }
            else
            {
                //страницы
                var pages = CMSPage.FullPageTable.Where(x => id == null ? !x.ParentID.HasValue : x.ParentID == id).Where(x => !x.Deleted)
               .OrderBy(
                   x => x.OrderNum);
                if (PageCheckFunction != null)
                {
                    pages = pages.Where(x => PageCheckFunction(x)).OrderBy(x => x.OrderNum);
                }

                foreach (var cmsPage in pages)
                {
                    var child = new JsTreeModel()
                    {
                        data = TextFunction == null ? cmsPage.PageName : TextFunction(cmsPage.PageName),
                        attr =
                            new JsTreeAttribute()
                            {
                                id = "x" + cmsPage.ID,
                                href = LinkFunction == null ? "#" : LinkFunction(cmsPage.ID),
                                uid = cmsPage.ID,
                                @class = (cmsPage.ID == MainPageID ? "home-icon " : "") + (rels.Any(z => z == "x" + cmsPage.ID) ? "jstree-checked" : "") + (CatalogBrowser.ParentPage != null && cmsPage.ID != CatalogBrowser.ParentPage.ID ? "page-item" : ""),
                                url = cmsPage.FullUrl
                            },
                    };
                    if (model.children == null)
                        model.children = new List<JsTreeModel>();
                    model.children.Add(child);

                    FillPagesAndCatsModel(ref child, cmsPage.ID);

                }
            }




        }
        private void FillPagesAndCatsAndProductsModel(ref JsTreeModel model, int? id, bool? catPage = null)
        {
            var old = catPage;
            if (!catPage.HasValue)
            {
                catPage = id.HasValue && id > 0 && id == (CatalogBrowser.ParentPage ?? new CMSPage()).ID;
            }
            var rels = new List<string>();
            if (CheckedNodes is List<string>)
                rels = CheckedNodes as List<string>;
            if (catPage.Value)
            {
                if (old == null)
                {
                    id = 1;
                }

                var pages = CatalogBrowser.CategoriesList.Where(x => x.Value.ParentID == id && !x.Value.Deleted).OrderBy(x => x.Value.OrderNum);

                foreach (var cmsPage in pages)
                {
                    var name = TextFunction == null ? cmsPage.Value.Name : TextFunction(cmsPage.Value.Name);
                    if (AddCountToCats)
                    {

                        /*
                                                name = string.Format("{0} ({1})", name,
                                                    CatalogBrowser.ProductCounts.ContainsKey(cmsPage.Value.ID)
                                                        ? CatalogBrowser.ProductCounts[cmsPage.Value.ID]
                                                        : 0);
                        */
                    }
                    var child = new JsTreeModel
                    {
                        data = name,
                        attr =
                            new JsTreeAttribute
                            {
                                cnt = CatalogBrowser.ProductCounts.ContainsKey(cmsPage.Value.ID)
                                ? CatalogBrowser.ProductCounts[cmsPage.Value.ID]
                                : 0,
                                id = "c" + cmsPage.Value.ID,
                                href = LinkFunction == null ? "#" : LinkFunction(cmsPage.Value.ID),
                                uid = cmsPage.Value.ID,
                                @class = rels.Any(z => z == "c" + cmsPage.Value.ID) ? "jstree-checked" : "",
                                url = cmsPage.Value.FullUrl
                            },

                    };
                    if (model.children == null)
                        model.children = new List<JsTreeModel>();
                    model.children.Add(child);

                    var prods = cmsPage.Value.StoreProductsToCategories.OrderBy(x => x.OrderNum).Select(x => x.StoreProduct).Where(x => !x.Deleted);
                    foreach (var product in prods)
                    {
                        var cp = new JsTreeModel
                        {
                            data = TextFunction == null ? product.ShortName : TextFunction(product.ShortName),
                            attr =
                                new JsTreeAttribute
                                {
                                    id = "p" + product.ID,
                                    href = LinkFunction == null ? "#" : LinkFunction(product.ID),
                                    uid = product.ID,
                                    @class = "prod-item" + (rels.Any(z => z == "c" + product.ID) ? " jstree-checked" : ""),
                                    url = product.FullUrl
                                },

                        };
                        if (child.children == null)
                            child.children = new List<JsTreeModel>();
                        child.children.Add(cp);

                    }
                    FillPagesAndCatsAndProductsModel(ref child, cmsPage.Value.ID, true);
                }
                if (id == 1)
                {
                    var prods =
                        new DB().StoreProducts.Where(x => !x.StoreProductsToCategories.Any() && !x.Deleted)
                            .ToList()
                            .Select(x => new JsTreeModel
                            {
                                data = x.NameOrDef,
                                attr =
                                    new JsTreeAttribute
                                    {
                                        id = "p" + x.ID,
                                        href = LinkFunction == null ? "#" : LinkFunction(x.ID),
                                        uid = x.ID,
                                        @class = "prod-item" + (rels.Any(z => z == "c" + x.ID) ? " jstree-checked" : ""),
                                        url = x.FullUrl
                                    },

                            }).ToList();

                    if (model.children == null)
                        model.children = new List<JsTreeModel>();
                    model.children.AddRange(prods);

                }

            }
            else
            {
                //страницы
                var pages = CMSPage.FullPageTable.Where(x => id == null ? !x.ParentID.HasValue : x.ParentID == id).Where(x => !x.Deleted)
               .OrderBy(
                   x => x.OrderNum);
                if (PageCheckFunction != null)
                {
                    pages = pages.Where(x => PageCheckFunction(x)).OrderBy(x => x.OrderNum);
                }

                foreach (var cmsPage in pages)
                {
                    var name = TextFunction == null ? cmsPage.PageName : TextFunction(cmsPage.PageName);
                    if (AddCountToCats && CatalogBrowser.ParentPage != null && cmsPage.ID == CatalogBrowser.ParentPage.ID)
                    {
                        /*
                                                name = string.Format("{0} ({1})", name,
                                                    CatalogBrowser.ProductCounts.ContainsKey(CatalogBrowser.Root.ID)
                                                        ? CatalogBrowser.ProductCounts[CatalogBrowser.Root.ID]
                                                        : 0);
                        */

                    }
                    var child = new JsTreeModel()
                    {
                        data = name,
                        attr =
                            new JsTreeAttribute()
                            {
                                cnt = CatalogBrowser.ProductCounts.ContainsKey(CatalogBrowser.Root.ID)
                                    ? CatalogBrowser.ProductCounts[CatalogBrowser.Root.ID]
                                    : 0,
                                id = "x" + cmsPage.ID,
                                href = LinkFunction == null ? "#" : LinkFunction(cmsPage.ID),
                                uid = cmsPage.ID,
                                @class =
                                    (cmsPage.ID == MainPageID ? "home-icon " : "") +
                                    (rels.Any(z => z == "x" + cmsPage.ID) ? "jstree-checked" : "") + (CatalogBrowser.ParentPage != null && cmsPage.ID != CatalogBrowser.ParentPage.ID ? "page-item" : ""),
                                url = cmsPage.FullUrl
                            },
                    };
                    if (model.children == null)
                        model.children = new List<JsTreeModel>();
                    model.children.Add(child);

                    FillPagesAndCatsAndProductsModel(ref child, cmsPage.ID);

                }
            }




        }

        private List<JsTreeModel> GetLevel(int? id, bool cat, bool pg, bool pd)
        {
            var cook = HttpContext.Current.Request.Cookies["node_selected_main"];
            var sel = "";
            if (cook != null)
                sel = HttpUtility.UrlDecode(cook.Value).Replace("#", "");

            var result = new List<JsTreeModel>();
            if (cat)
            {
                var cats =
                    CatalogBrowser.CategoriesList.Where(
                        x => (id == null ? !x.Value.ParentID.HasValue : x.Value.ParentID == id) && !x.Value.Deleted)
                        .OrderBy(x => x.Value.OrderNum);

                foreach (var cmsPage in cats)
                {
                    var name = TextFunction == null ? cmsPage.Value.Name : TextFunction(cmsPage.Value.Name);
                    var child = new JsTreeModel
                    {

                        data = name,
                        attr =
                            new JsTreeAttribute
                            {
                                allcnt = GetAllCnt(cmsPage.Value.ID),
                                id = cmsPage.Value.ParentID.HasValue ? ("c" + cmsPage.Value.ID): ("x"+ CatalogBrowser.ParentPage.ID),
                                href = LinkFunction == null ? "#" : LinkFunction(cmsPage.Value.ID),
                                uid = cmsPage.Value.ParentID.HasValue ? cmsPage.Value.ID : CatalogBrowser.ParentPage.ID,
                                @class = "",
                                url = cmsPage.Value.FullUrl,
                                catcnt = CatalogBrowser.CategoriesList.Where(x => !x.Value.Deleted).Select(x => x.Value).Count(x => x.ParentID == cmsPage.Value.ID)
                            }


                    };
                    var children = CatalogBrowser.CategoriesList.Any(x => x.Value.ParentID == cmsPage.Value.ID) || CatalogBrowser.ProductCounts[cmsPage.Value.ID] > 0;
                    if (children)
                        child.state = "closed";
                    if (child.attr.id == sel)
                    {
                        child.state += " selected";
                    }
                    child.state = (child.state ?? "").Trim();
                    result.Add(child);
                }
            }
            if (pg)
            {
                var pages = CMSPage.FullPageTable.Where(x => id == null ? !x.ParentID.HasValue : x.ParentID == id)
                    .Where(x => x.ID != CatalogBrowser.ParentPage.ID)
                    .Where(x => !x.Deleted)
                    .OrderBy(
                        x => x.OrderNum);

                if (PageCheckFunction != null)
                {
                    pages = pages.Where(x => PageCheckFunction(x)).OrderBy(x => x.OrderNum);
                }

                foreach (var cmsPage in pages)
                {
                    var name = TextFunction == null ? cmsPage.PageName : TextFunction(cmsPage.PageName);
                    if (AddCountToCats && CatalogBrowser.ParentPage != null &&
                        cmsPage.ID == CatalogBrowser.ParentPage.ID)
                    {
                        /*
                                                name = string.Format("{0} ({1})", name,
                                                    CatalogBrowser.ProductCounts.ContainsKey(CatalogBrowser.Root.ID)
                                                        ? CatalogBrowser.ProductCounts[CatalogBrowser.Root.ID]
                                                        : 0);
                        */

                    }
                    var child = new JsTreeModel()
                    {
                        data = name,
                        attr =
                            new JsTreeAttribute()
                            {
                                /*
                                                                cnt = CatalogBrowser.ProductCounts.ContainsKey(CatalogBrowser.Root.ID)
                                                                    ? CatalogBrowser.ProductCounts[CatalogBrowser.Root.ID]
                                                                    : 0,
                                */
                                id = "x" + cmsPage.ID,
                                href = LinkFunction == null ? "#" : LinkFunction(cmsPage.ID),
                                uid = cmsPage.ID,
                                @class =
                                    (cmsPage.ID == MainPageID ? "home-icon " : "") +
                                    (CatalogBrowser.ParentPage != null && cmsPage.ID != CatalogBrowser.ParentPage.ID
                                        ? "page-item"
                                        : ""),
                                url = cmsPage.FullUrl,
                                pagecnt = CMSPage.FullPageTable.Where(x => !x.Deleted).Count(x => x.ParentID == cmsPage.ID)

                            },
                            

                    };



                    var children = CMSPage.FullPageTable.Any(x => x.ParentID == cmsPage.ID);
                    if (children)
                    {
                        child.state = "closed";
                    }
                    if (child.attr.id == sel)
                    {
                        child.state += " selected";
                    }
                    child.state = (child.state ?? "").Trim();

                    result.Add(child);
                }
            }
            if (pd)
            {
                var prods =
                    new DB().StoreProductsToCategories.Where(x => x.CategoryID == id)
                        .OrderBy(x => x.OrderNum)
                        .Select(x => x.StoreProduct)
                        .Where(x => !x.Deleted);
                foreach (var product in prods)
                {
                    var cp = new JsTreeModel
                    {
                        data = product.ShortName,
                        attr =
                            new JsTreeAttribute
                            {
                                id = "p" + product.ID,
                                href = LinkFunction == null ? "#" : LinkFunction(product.ID),
                                uid = product.ID,
                                @class = "prod-item",
                                url = product.FullUrl
                            },

                    };
                    if (cp.attr.id == sel)
                    {
                        cp.state += " selected";
                    }
                    cp.state = (cp.state ?? "").Trim();

                    result.Add(cp);

                }
            }
            return result;
        }

        private int GetAllCnt(int id)
        {
            var all = 0;
            all += CatalogBrowser.ProductCounts.ContainsKey(id)
                ? CatalogBrowser.ProductCounts[id]
                : 0;

            AddCnt(ref all, id);
            return all;

        }

        private void AddCnt(ref int counter, int id)
        {
            var children = CatalogBrowser.CategoriesList.Where(x => x.Value.ParentID == id);
            foreach (var child in children)
            {
                counter += CatalogBrowser.ProductCounts.ContainsKey(child.Value.ID)
                    ? CatalogBrowser.ProductCounts[child.Value.ID]
                    : 0;

                AddCnt(ref counter, child.Value.ID);
            }

        }
    }




    #endregion
    #region Универсальный источник данных

    public class UniversalDataSource
    {
        public object Source { get; set; }
        public string KeyField { get; set; }
        public string ValueField { get; set; }
        public object DefValue { get; set; }
        public bool HasEmptyDef { get; set; }

        public TextFunctionDelegate TextFunction { get; set; }

        public object CalculatedDef(string fieldName, UniversalEditorPagedData model)
        {
            if (model.EditedRow != null && model.Settings.Filters != null && (int)model.EditedRow.GetPropertyValue(model.Settings.UIDColumnName) != 0)
            {
                var filter = model.Settings.Filters.FirstOrDefault(x => x.QueryKey == fieldName);
                if (filter != null && filter.ValueFromQuery.ToString().IsFilled())
                {
                    return filter.ValueFromQuery;
                }
            }
            return HasEmptyDef ? null : DefValue;
        }
    }
    #endregion
    #region Превью
    public class UniversalEditorAddViewInfo
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public RouteValueDictionary Routes { get; set; }

        public bool InEditor { get; set; }
    }

    public class PreviewData
    {
        public int UID { get; set; }
        public int Type { get; set; }

        public string Link
        {
            get
            {
                if (Type == 0)
                    return (CMSPage.FullPageTable.FirstOrDefault(x => x.ID == UID) ?? CMSPage.Get("main")).FullUrl;
                if (Type == 1)
                {
                    var entry = CatalogBrowser.CategoriesList.Values.FirstOrDefault(x => x.ID == UID);
                    if (entry == null) return CMSPage.Get("main").FullUrl;
                    return entry.FullUrl;
                }
                if (Type == 2)
                {
                    var entry = new DB().StoreProducts.FirstOrDefault(x => x.ID == UID);
                    if (entry == null) return CMSPage.Get("main").FullUrl;
                    return entry.FullUrl;
                }
                return CMSPage.Get("main").FullUrl;
            }
        }
    }

    #endregion
    #region Табличный список с пейджером
    public class UniversalEditorPagedData
    {



        public PreviewData PreviewData { get; set; }
        public string[] AddQueryParams { get; set; }
        public string AddQueryParamsJoin
        {
            get { return (AddQueryParams ?? new string[] { }).JoinToString("&"); }
            set { AddQueryParams = value.Split<string>("&").ToArray(); }
        }
        public string EditorName { get; set; }
        public string EditorDescription { get; set; }
        public SaveFunctionDelegate SaveRow { get; set; }
        public SaveFunctionDelegate BeforeSaveRow { get; set; }
        public SaveFunctionDelegate AfterSaveRow { get; set; }
        public CompleteSaveFunctionDelegate CompleteSave { get; set; }

        public object PagedData { get; set; }
        public UniversalEditorSettings Settings { get; set; }
        public CurrentEditorType CurrentType { get; set; }
        public object EditedRow { get; set; }
        public bool IsAddingNew { get; set; }
        public string CallerController { get; set; }
        public string CallerAction { get; set; }
        public UniversalEditorAddViewInfo AddView { get; set; }
        public BeforeDeleteFunctionDelegate BeforeDelFunc { get; set; }
        public BeforeDeleteFunctionDelegate DelFunc { get; set; }

        public int Page
        {
            get
            {
                return PagedData != null ? (int)PagedData.GetPropertyValue("PageIndex") : HttpContext.Current.Request.QueryString["Page"].ToInt();
            }
        }

        public string ErrorList
        {
            get
            {
                if (EditedRow == null)
                    return "";

                var errors = new List<string>();
                foreach (var field in Settings.EditedFieldsList)
                {
                    errors.AddRange(field.Modificators.Select(modificator => modificator.CheckField(field, EditedRow.GetPropertyValue(field.FieldName))).Where(e => e.IsFilled()));
                }
                return errors.JoinToString("<br/>");
            }
        }

        public string RedirectURL { get; set; }

        public string[] FilterParams
        {
            get
            {
                if (Settings.Filters == null || !Settings.Filters.Any())
                    return new string[] { };
                return Settings.Filters.Select(x => x.QueryKey).ToArray();
            }
        }

        public bool HasFileUpload
        {
            get { return Settings != null && Settings.EditedFieldsList.Any(x => x.FieldType == UniversalEditorFieldType.DBImageUpload); }
        }

        public IEnumerable<string> FullParamList
        {
            get { return (FilterParams.Concat(AddQueryParams ?? new string[0]).ToArray()).Distinct().AsEnumerable(); }
        }

        public bool IsPartial { get; set; }
        public bool HorizTabs { get; set; }
        public string EditorUID { get; set; }
        public string NewNode { get; set; }
        public bool ShowInPopup { get; set; }


        public RouteValueDictionary GetFullParamsValuesForDelete(object row, bool ondelete = false)
        {
            var dict = new RouteValueDictionary();
            dict.Add("Type", ondelete ? "List" : "Delete");
            dict.Add("UID", row.GetPropertyValue(Settings.UIDColumnName));
            dict.Add("Page", HttpContext.Current.Request.QueryString["Page"].ToInt());
            if (AddQueryParams != null)
            {
                foreach (var param in AddQueryParams)
                {
                    if (!dict.ContainsKey(param))
                    {
                        var value = HttpContext.Current.Request.QueryString[param];
                        if (value.IsFilled())
                            dict.Add(param, value);
                        else
                        {
                            var propValue = row.GetPropertyValue(param);
                            if (propValue != null)
                            {
                                value = propValue.ToString();
                                if (value.IsFilled())
                                    dict.Add(param, value);

                            }

                        }
                    }
                }
            }
            return dict;
        }

        public RouteValueDictionary GetFullParamsValuesForList()
        {
            var dict = new RouteValueDictionary();
            dict.Add("Type", "List");
            dict.Add("Page", HttpContext.Current.Request.QueryString["Page"].ToInt());
            if (AddQueryParams != null)
            {
                foreach (var param in AddQueryParams)
                {
                    if (!dict.ContainsKey(param))
                    {
                        var value = HttpContext.Current.Request.QueryString[param];
                        if (value.IsFilled())
                            dict.Add(param, value);
                    }
                }
            }
            return dict;
        }
        public RouteValueDictionary GetFullParamsValuesForEdit(object UID)
        {
            var dict = new RouteValueDictionary();
            dict.Add("Type", "Edit");
            dict.Add("Page", HttpContext.Current.Request["Page"].ToInt());
            dict.Add("BackOverride", HttpContext.Current.Request["BackOverride"]);
            dict.Add("UID", UID);
            if (AddQueryParams != null)
            {
                foreach (var param in AddQueryParams)
                {
                    if (!dict.ContainsKey(param))
                    {
                        var value = HttpContext.Current.Request.QueryString[param];
                        if (value.IsFilled())
                            dict.Add(param, value);
                    }
                }
            }
            return dict;
        }

        public RouteValueDictionary JoinRoutes(object routes)
        {
            var dict = new RouteValueDictionary(routes);
            var fr = FilterParams.Where(x => HttpContext.Current.Request.QueryString[x].IsFilled());
            foreach (var r in fr)
            {
                dict.Add(r, HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString[r]));
            }
            return dict;

        }

        public string GetImageWrapper(string fieldName, string columnName = "ID", bool forDL = false)
        {
            if (Settings == null || EditedRow == null)
            {
                return "";
            }

            return GetImageWrapper(Settings.TableName, columnName, EditedRow.GetPropertyValue(columnName).ToString(), fieldName, forDL);

        }

        public static string GetImageWrapper(string tableName, string columnName, string columnValue, string fieldName, int width, int height, bool forDL = false)
        {
            return
                string.Format("/Master/ru/UniversalEditor/Image?tableName={0}&uidName={1}&uidValue={2}&fieldName={3}&width={4}&height={5}&forDL={6}",
                              tableName, columnName, columnValue, fieldName, width, height, forDL);

        }
        public static string GetImageWrapper(string tableName, string columnName, string columnValue, string fieldName, bool forDL = false)
        {
            return
                string.Format("/Master/ru/UniversalEditor/Image?tableName={0}&uidName={1}&uidValue={2}&fieldName={3}&forDL={4}",
                              tableName, columnName, columnValue, fieldName, forDL);

        }
        public static string GetDeleteWrapper(string tableName, string columnName, string columnValue, string fieldName)
        {
            return
                string.Format(
                    "/Master/ru/UniversalEditor/ClearImage?tableName={0}&uidName={1}&uidValue={2}&fieldName={3}",
                    tableName, columnName, columnValue, fieldName);
        }

        public bool IsNullImage(string fieldName)
        {
            if (EditedRow == null) return true;
            var data = (Binary)EditedRow.GetPropertyValue(fieldName);
            return (data == null || data.Length == 0);
        }

        public string GetDeleteWrapper(string fieldName, string columnName = "ID")
        {
            if (Settings == null || EditedRow == null)
            {
                return "";
            }
            return GetDeleteWrapper(Settings.TableName, columnName, EditedRow.GetPropertyValue(columnName).ToString(),
                                    fieldName);

        }



        public static object GetImageHeight(Binary binary)
        {
            var ms = new MemoryStream(binary.ToArray());
            using (var bitmap = new Bitmap(ms))
            {
                return bitmap.Height;
            }
        }
        public static object GetImageWidth(Binary binary)
        {
            var ms = new MemoryStream(binary.ToArray());
            using (var bitmap = new Bitmap(ms))
            {
                return bitmap.Width;
            }
        }
    }
    #endregion
    #region Настройки редактора
    public class EditorLink
    {
        public bool IsPartial { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class UniversalEditorSettings
    {
        public List<UniversalListField> ShowedFieldsInList { get; set; }
        public bool HasDeleteColumn { get; set; }
        public string UIDColumnName { get; set; }
        public List<UniversalEditorField> EditedFieldsList { get; set; }
        public List<FilterConfiguration> Filters { get; set; }
        public bool AutoFilter { get; set; }
        public bool CanAddNew { get; set; }
        public string TableName { get; set; }
        public BeforeSaveFunctionDelegate OnUniversalSaving { get; set; }

        public string FilterDescription { get; set; }

        public List<EditorLink> ListLinks { get; set; }
        public bool HasListLinks
        {
            get { return ListLinks != null && ListLinks.Any(); }
        }

        public List<EditorLink> EditLinks { get; set; }



        public static string GetTelerikEditorFrame(string table, string targetcolumn, string searchcolumn, string condition)
        {
            return ConfigurationManager.AppSettings["TelerikFrameDomain"] + "?table=" + table + "&targetcolumn=" +
                   targetcolumn + "&searchcolumn=" + searchcolumn + "&condition=" + condition;
        }

        public string UniversalTableSaver(object obj, UniversalEditorSettings settings, DB db)
        {
            try
            {
                if (obj == null) return "Объект не найден";
                ITable table = null;
                try
                {
                    table = (ITable)db.GetType()
                                       .GetProperty(settings.TableName)
                                       .GetValue(db, null);
                }
                catch
                {
                    try
                    {
                        table = (ITable)db.GetType()
                                           .GetProperty(settings.TableName + "s")
                                           .GetValue(db, null);

                    }
                    catch
                    {

                    }
                }
                if (table == null)
                {
                    return "Таблица не найдена. Ошибка в определении.";
                }

                if ((int)obj.GetPropertyValue(settings.UIDColumnName) == 0)
                {

                    if (settings.OnUniversalSaving != null)
                    {
                        obj = settings.OnUniversalSaving(obj, db);
                    }
                    table.InsertOnSubmit(obj);
                }
                else
                {
                    //try
                    //{
                    //obj.Detach();
                    //table.Attach(obj, true);
                    //db.Refresh(RefreshMode.KeepChanges, obj);
                    //}
                    //catch (Exception e)
                    {
                        object entry =
                            Enumerable.Cast<object>(table)
                                      .FirstOrDefault(
                                          item =>
                                          (int)item.GetPropertyValue(settings.UIDColumnName) ==
                                          (int)obj.GetPropertyValue(settings.UIDColumnName));
                        if (entry != null)
                        {
                            obj.Detach();
                            entry.LoadPossibleProperties(obj, new string[] { settings.UIDColumnName });
                            db.Refresh(RefreshMode.KeepChanges, entry);
                        }


                    }
                    //catch (Exception e)
                    //{

                    /*
                                            obj.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);

                                            var ndb = new DB();
                                            obj.ToString()|
                    */

                    //}
                }
                db.SubmitChanges();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        public static string DefaultTextChecker(object input)
        {
            if (input is bool)
                return (bool)input ? "Да" : "Нет";

            if (input is DateTime)
                return ((DateTime)input).ToString("d MMMM yyyy, HH:mm");

            if (input == null) return "<noname>";

            return input.ToString().IsNullOrEmpty() ? "<noname>" : input.ToString();
        }
        public static string DefaultTextCheckerNoReplace(object input)
        {
            if (input is bool)
                return (bool)input ? "Да" : "Нет";

            if (input is DateTime)
                return ((DateTime)input).ToString("d MMMM yyyy, HH:mm");

            if (input == null) return "";

            return input.ToString().IsNullOrEmpty() ? "" : input.ToString();
        }

        public string GenerateBaseURL(FilterConfiguration filter)
        {

            if (filter.MainFilter)
            {
                var empty = Filters.Select(
                                      x => new KeyValuePair<string, string>(x.QueryKey, ""))
                                  .ToList();
                return HttpContext.Current.Request.GenerateURL(empty);

            }

            var list = Filters.Where(x => !x.SkipInQuery && x.QueryKey != filter.QueryKey)
                              .Select(
                                  x => new KeyValuePair<string, string>(x.QueryKey, (x.ValueFromQuery ?? "").ToString()))
                              .ToList();
            list.Add(new KeyValuePair<string, string>(filter.QueryKey, ""));
            return HttpContext.Current.Request.GenerateURL(list);

        }
    }

    #endregion
    #region Перечисления
    public enum SerializationType
    {
        Pages,
        Categories,
        FullCatalog,
        PagesAnsCategories,
        PagesAnsCategoriesAndProducts,
        Partial
    }
    public enum FilterType
    {
        Text,
        Integer,
        Date,
        Container
    }

    public enum TextComparators
    {
        Contains,
        Equals
    }
    public enum NumericComparators
    {
        Less,
        LessOrEqual,
        Equal,
        GreaterOrEqual,
        Greater
    }
    public enum DateComparators
    {
        [Display(Name = "<")]
        Less,

        [Display(Name = "<=")]
        LessOrEqual,

        [Display(Name = "==")]
        Equal,

        [Display(Name = ">=")]
        GreaterOrEqual,

        [Display(Name = ">")]
        Greater,

        [Display(Name = "Range")]
        InRange
    }


    public enum UniversalEditorFieldType
    {
        DropDown,
        TextBox,
        CheckBox,
        TextArea,
        TextEditor,
        Calendar,
        Label,
        DBImageUpload,
        TreeEditor,
        FileImageUpload,
        FileUpload,
        ProductSelect,
        TagBox,
        RelatedTable,
        Hidden,
        Custom,
        Delimeter,
        CatalogSlider,
        ProductSlider,
        CatalogVideo
    }

    public enum CurrentEditorType
    {
        List,
        Edit,
        Delete
    }
    #endregion
    #region Описание колонок
    public class FilterConfiguration
    {
        public UniversalDataSource FilterSource { get; set; }
        public bool IsDropDown { get; set; }
        public string QueryKey { get; set; }
        public string HeaderText { get; set; }
        public FilterType Type { get; set; }
        public bool SkipInQuery { get; set; }
        public bool MainFilter { get; set; }
        public object ValueFromQuery
        {
            get
            {
                var q = HttpUtility.UrlDecode(HttpContext.Current.Request[QueryKey]);
                return string.IsNullOrEmpty(q) ? FilterSource.DefValue : q;
            }
        }

        public int? MaxHeight { get; set; }

        public int PageTypeID { get; set; }


        protected Expression BuildExpressionDate(MemberExpression property, DateComparators comparator)
        {
            if (ValueFromQuery == null || ValueFromQuery.ToString().IsNullOrEmpty())
            {
                PropertyInfo p = typeof(DateTime?).GetProperty("HasValue");
                Expression nullValue = Expression.Constant(false, p.PropertyType);
                return Expression.Equal(property, nullValue);
            }

            DateTime? dValue = null;
            DateTime temp;
            if (
                DateTime.TryParseExact(ValueFromQuery.ToString(), "yyyy.MM.ddTHH:mm:ss.fff", CultureInfo.CurrentCulture,
                                        DateTimeStyles.None, out temp))
                dValue = temp;

            DateTime? secondValue = null;

            Expression searchExpression1 = null;
            Expression searchExpression2 = null;

            switch (comparator)
            {
                case DateComparators.Less:
                    searchExpression1 = Expression.LessThan(property, Expression.Constant(dValue));
                    break;
                case DateComparators.LessOrEqual:
                    searchExpression1 = Expression.LessThanOrEqual(property, Expression.Constant(dValue));
                    break;
                case DateComparators.Equal:
                    searchExpression1 = Expression.Equal(property, Expression.Constant(dValue));
                    break;
                case DateComparators.GreaterOrEqual:
                case DateComparators.InRange:
                    searchExpression1 = Expression.GreaterThanOrEqual(property, Expression.Constant(dValue));
                    break;
                case DateComparators.Greater:
                    searchExpression1 = Expression.GreaterThan(property, Expression.Constant(dValue));
                    break;
                default:
                    searchExpression1 = null;
                    break;
            }



            if (comparator == DateComparators.InRange && secondValue.HasValue)
            {
                searchExpression2 = Expression.LessThanOrEqual(property, Expression.Constant(secondValue.Value));
            }

            if (searchExpression1 == null && searchExpression2 == null)
            {
                return null;
            }
            else if (searchExpression1 != null && searchExpression2 != null)
            {
                var combinedExpression = Expression.AndAlso(searchExpression1, searchExpression2);
                return combinedExpression;
            }
            else if (searchExpression1 != null)
            {
                return searchExpression1;
            }
            else
            {
                return searchExpression2;
            }
        }
        protected Expression BuildExpressionNumeric(MemberExpression property, NumericComparators comparator)
        {

            if (ValueFromQuery == null || ValueFromQuery.ToString().IsNullOrEmpty())
            {
                return null;
            }

            var iValue = ValueFromQuery.ToTypedValue<int?>();
            if (!iValue.HasValue) return null;
            switch (comparator)
            {
                case NumericComparators.Less:
                    return Expression.LessThan(property, Expression.Constant(iValue.Value));
                case NumericComparators.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(iValue.Value));
                case NumericComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(iValue.Value));
                case NumericComparators.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(iValue.Value));
                case NumericComparators.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(iValue.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }


        protected Expression BuildExpressionText(MemberExpression property, TextComparators comparator)
        {
            if (ValueFromQuery == null)
            {
                return null;
            }

            var searchExpression = Expression.Call(
                property,
                typeof(string).GetMethod(comparator.ToString(), new[] { typeof(string) }),
                Expression.Constant(ValueFromQuery));

            return searchExpression;
        }


        private Expression<Func<T, bool>> CreatePredicateWithNullCheck<T>(Expression searchExpression, ParameterExpression arg, MemberExpression targetProperty)
        {
            string[] parts = QueryKey.Split('.');

            Expression nullCheckExpression = null;
            if (parts.Length > 1)
            {
                MemberExpression property = Expression.Property(arg, parts[0]);
                nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                for (int i = 1; i < parts.Length - 1; i++)
                {
                    property = Expression.Property(property, parts[i]);
                    Expression innerNullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                    nullCheckExpression = Expression.AndAlso(nullCheckExpression, innerNullCheckExpression);
                }
            }

            if (!targetProperty.Type.IsValueType || (targetProperty.Type.IsGenericType && targetProperty.Type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                var innerNullCheckExpression = Expression.NotEqual(targetProperty, Expression.Constant(null));

                if (nullCheckExpression == null)
                {
                    nullCheckExpression = innerNullCheckExpression;
                }
                else
                {
                    nullCheckExpression = Expression.AndAlso(nullCheckExpression, innerNullCheckExpression);
                }
            }

            if (nullCheckExpression == null || ValueFromQuery == null)
            {
                return Expression.Lambda<Func<T, bool>>(searchExpression, arg);
            }
            else
            {
                var combinedExpression = Expression.AndAlso(nullCheckExpression, searchExpression);

                var predicate = Expression.Lambda<Func<T, bool>>(combinedExpression, arg);

                return predicate;
            }
        }



        protected Expression BuildExpression(MemberExpression property)
        {
            if (Type == FilterType.Integer || Type == FilterType.Container)
                return BuildExpressionNumeric(property, NumericComparators.Equal);
            if (Type == FilterType.Text)
                return BuildExpressionText(property, TextComparators.Equals);
            if (Type == FilterType.Date)
                return BuildExpressionDate(property, DateComparators.Equal);
            return null;
        }

        public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            //var arg = Expression.Parameter(typeof(T), "p");
            var arg = Expression.Parameter(typeof(T), "p");
            var property = GetPropertyAccess(arg);

            Expression searchExpression = null;

            if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (ValueFromQuery == null)
                    searchExpression = BuildExpression(Expression.Property(property, "HasValue"));
                else searchExpression = BuildExpression(Expression.Property(property, "Value"));
            }
            else
            {
                searchExpression = BuildExpression(property);
            }

            if (searchExpression == null)
            {
                return query;
            }
            else
            {
                var predicate = CreatePredicateWithNullCheck<T>(searchExpression, arg, property);
                return query.Where(predicate);
            }
        }

        private MemberExpression GetPropertyAccess(ParameterExpression arg)
        {
            string[] parts = QueryKey.Split('.');

            MemberExpression property = Expression.Property(arg, parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                property = Expression.Property(property, parts[i]);
            }

            return property;
        }


    }

    public class UniversalField
    {
        public string FieldName { get; set; }
        public string HeaderText { get; set; }
        public TextFunctionDelegate TextFunction { get; set; }
        public string Template { get; set; }
        public string CheckedText(object input)
        {
            return TextFunction == null ? UniversalEditorSettings.DefaultTextChecker(input) : TextFunction(input);
        }
    }

    public class UniversalListField : UniversalField
    {
        public bool IsLinkToEdit { get; set; }
        public bool IsOrderColumn { get; set; }
        public int? Width { get; set; }
        public ComplexReorder ComplexReorder { get; set; }
        public string ImageField { get; set; }
    }

    public class ComplexReorder
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string TableName { get; set; }
        public string OrderName { get; set; }
        public TextFunctionDelegate TextFunction1 { get; set; }
        public TextFunctionDelegate TextFunction2 { get; set; }
    }

    public class UniversalEditorField : UniversalField
    {
        public UniversalEditorField()
        {
            FieldType = UniversalEditorFieldType.TextBox;
            DataType = typeof(string);
            Modificators = new List<IUniversalFieldModificator>();
        }
        public UniversalDataSource InnerListDataSource { get; set; }
        public TreeDataSource TreeDataSource { get; set; }
        public bool ReadOnly { get; set; }
        public UniversalEditorFieldType FieldType { get; set; }
        public Type DataType { get; set; }
        public List<IUniversalFieldModificator> Modificators { get; set; }
        public bool Hidden { get; set; }
        public string AdditionalData { get; set; }

        public bool AdditionalTypeFlag { get; set; }
        public string GroupName { get; set; }
        public object AdditionalDataObject { get; set; }
    }

    #endregion
    #region Модификаторы свойств

    public class RequiredModificator : IUniversalFieldModificator
    {
        public string CheckField(UniversalEditorField field, object value)
        {
            var errText = "Поле \"" + field.HeaderText + "\" обязательно для заполнения";
            if (field.FieldType == UniversalEditorFieldType.DBImageUpload)
            {
                var rq = HttpContext.Current.Request[field.FieldName + "_Path"];

                if (rq.IsNullOrEmpty() && value == null)
                    return errText;
                return "";
            }
            var obj = value.ToTypedObject(field.DataType);
            if (obj == null || (obj is DateTime && (DateTime)obj == DateTime.MinValue))
                return errText;
            if (obj is string && ((string)obj).IsNullOrEmpty()) return errText;
            return "";
        }
    }

    public class EnglishLettersModificator : IUniversalFieldModificator
    {
        public string CheckField(UniversalEditorField field, object value)
        {
            var errText = "Поле \"" + field.HeaderText + "\" должно содержать только латинские символы";
            var obj = value.ToTypedObject(field.DataType);
            var rx = new Regex("[А-Яа-я]+");
            if (obj is string && rx.IsMatch(((string)obj))) return errText;
            return "";
        }
    }
    public class UniqueModificator : IUniversalFieldModificator
    {
        public int ItemID { get; set; }

        public UniqueModificator(int itemID)
        {
            ItemID = itemID;
        }
        /// <summary>
        /// Проверка уникальности (требуется InnerDataSource)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckField(UniversalEditorField field, object value)
        {
            var errText = "Поле \"" + field.HeaderText + "\" должно быть уникальным";
            var obj = value.ToTypedObject(field.DataType);
            var enumareable = (IEnumerable)field.InnerListDataSource.Source;
            var found = enumareable.Cast<object>()
                                   .FirstOrDefault(
                                       item => (int)item.GetPropertyValue("ID") > 0 && (int)item.GetPropertyValue("ID") != ItemID &&
                                               (item.GetPropertyValue(field.InnerListDataSource.KeyField) ?? "").ToString().ToLower() ==
                                               obj.ToString().ToLower());
            return
                found == null
                    ? ""
                    : errText;
        }
    }

    public class RangeModificator : IUniversalFieldModificator
    {
        public decimal MinVal { get; set; }
        public decimal MaxVal { get; set; }

        public RangeModificator(decimal minVal, decimal maxVal)
        {
            MinVal = minVal;
            MaxVal = maxVal;
        }

        public string CheckField(UniversalEditorField field, object value)
        {
            string errText = "Поле \"" + field.HeaderText + "\" должно быть ";
            if (MinVal != decimal.MinValue)
                errText += "больше " + MinVal.ToUniString();
            if (MinVal != decimal.MinValue && MaxVal != decimal.MaxValue)
                errText += " и ";
            if (MaxVal != decimal.MaxValue)
                errText += "меньше " + MaxVal.ToUniString();
            var obj = value.ToTypedObject(field.DataType);
            if (obj == null)
            {
                if (field.DataType == typeof(decimal))
                {
                    obj = (value ?? "").ToString().Replace(",", ".").ToTypedObject(field.DataType) ??
                          (value ?? "").ToString().Replace(".", ",").ToTypedObject(field.DataType);
                }
            }
            if (obj == null)
                return errText;
            if (obj is decimal)
            {
                if ((decimal)obj <= MinVal || (decimal)obj >= MaxVal)
                    return errText;
            }
            if (obj is int)
            {
                if ((int)obj <= MinVal || (int)obj >= MaxVal)
                    return errText;
            }
            return "";
        }
    }

    public interface IUniversalFieldModificator
    {
        string CheckField(UniversalEditorField field, object value);
    }
    #endregion
}