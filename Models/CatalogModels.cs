using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.WebPages.Razor.Configuration;
using NPOI.SS.Formula.Functions;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;

namespace Smoking.Models
{

    public partial class FilterItem
    {
        private static List<SelectListItem> _chars;
        public static List<SelectListItem> Chars
        {
            get
            {
                if (_chars == null)
                {
                    var db = new DB();
                    _chars = //db.StoreCharacters.Select(x => new SelectListItem() {Text = x.Name, Value = x.ID.ToString() }).ToList();
                        db.StoreImporters.Where(x => x.ColumnName == "Character")
                            .OrderBy(x => x.Priority)
                            .Select(x => new SelectListItem() { Text = x.Header, Value = x.ID.ToString() }).ToList();


                }
                return _chars;
            }
        }

       
    }

    public partial class Filter
    {
        public bool HasSection
        {
            get { return PageID.HasValue || CatID.HasValue; }
        }

        public string SelectedSection
        {
            get
            {
                if (PageID.HasValue)
                    return "x" + PageID;
                if (CatID.HasValue)
                    return "c" + CatID;
                return "";
            }
            set
            {
                if (value.StartsWith("x") && value != "x0")
                {
                    CatID = null;
                    PageID = value.Substring(1).ToInt();
                }
                else if (value.StartsWith("c"))
                {
                    CatID = value.Substring(1).ToInt();
                    PageID = null;
                }
                else
                {
                    CatID = null;
                    PageID = null;
                }
            }
        }

        public string SectionName
        {
            get
            {
                if (StoreCategory != null)
                    return StoreCategory.Name;
                if (CMSPage != null)
                {
                    CMSPage.LoadLangValues();
                    return CMSPage.PageName;
                }
                return "---";
            }
        }
    }
    public partial class AnimeBlock
    {
        public bool HasSection
        {
            get { return PageID.HasValue || CatID.HasValue; }
        }

        public string SelectedSection
        {
            get
            {
                if (PageID.HasValue)
                    return "x" + PageID;
                if (CatID.HasValue)
                    return "c" + CatID;
                return "";
            }
            set
            {
                if (value.StartsWith("x") && value != "x0")
                {
                    CatID = null;
                    PageID = value.Substring(1).ToInt();
                }
                else if (value.StartsWith("c"))
                {
                    CatID = value.Substring(1).ToInt();
                    PageID = null;
                }
                else
                {
                    CatID = null;
                    PageID = null;
                }
            }
        }

        public string SectionName
        {
            get
            {
                if (StoreCategory != null)
                    return StoreCategory.Name;
                if (CMSPage != null)
                {
                    CMSPage.LoadLangValues();
                    return CMSPage.PageName;
                }
                return "---";
            }
        }
    }

    public class Comparator
    {
        public bool InCompare { get; set; }

        public string Text
        {
            get { return InCompare ? "Убрать из сравнения" : "Добавить в сравнение"; }
        }

        public string CSS
        {
            get { return InCompare ? "active" : ""; }
        }
        public int ID { get; set; }
        public Comparator(int id)
        {
            ID = id;
            var cook = HttpContext.Current.Request.Cookies.Get("ForCompare");
            if (cook != null)
            {
                var ids = HttpUtility.UrlDecode(cook.Value).Split<int>(";");
                InCompare = ids.Contains(id);

            }
        }
    }

    public class RadiatorFilter
    {
        public RadiatorFilter(bool skipAny = false, bool skipTextConvert = false)
        {
            var db = new DB();

            Depth = new List<KeyValuePair<string, string>>();
            Height = new List<KeyValuePair<string, string>>();
            Width = new List<KeyValuePair<string, string>>();

            if (!skipAny)
            {
                Depth.Add(new KeyValuePair<string, string>("", "любая"));
            }
            var depth = db.StoreCharacters.FirstOrDefault(x => x.Name == "Тип");
            if (depth != null)
            {
                Depth.AddRange(
                    depth.StoreCharacterValues.Select(x => new KeyValuePair<string, string>(x.Value, skipTextConvert ? x.Value : ConvertToTypeText(x.Value))));
                DepthID = depth.ID;
            }
            if (!skipAny)
            {
                Width.Add(new KeyValuePair<string, string>("", "любая"));
            }
            var width = db.StoreCharacters.FirstOrDefault(x => x.Name == "Ширина");
            if (width != null)
            {
                Width.AddRange(
                    width.StoreCharacterValues.Select(x => new KeyValuePair<string, string>(x.Value, skipTextConvert ? x.Value : x.Value + " мм")));
                WidthID = width.ID;
            }
            if (!skipAny)
            {
                Height.Add(new KeyValuePair<string, string>("", "любая"));
            }
            var height = db.StoreCharacters.FirstOrDefault(x => x.Name == "Высота");
            if (height != null)
            {
                Height.AddRange(
                    height.StoreCharacterValues.Select(x => new KeyValuePair<string, string>(x.Value, skipTextConvert ? x.Value : x.Value + " мм")));
                HeightID = height.ID;
            }

            Filters = new CatalogCharacterFilters(new List<string>() { "Тип", "Ширина", "Высота", "Мощность", "Подключение" });
            foreach (var filter in Filters.Filters)
            {
                if (filter.FilteredValues == null)
                    filter.FilteredValues = new List<string>();

                if (filter.Values == null)
                    filter.Values = new List<string>();
            }

        }

        public CatalogCharacterFilter Get(string name)
        {
            return Filters.Filters.FirstOrDefault(x => x.Header == name);
        }

        public CatalogCharacterFilters Filters { get; set; }

        private string ConvertToTypeText(string value)
        {
            switch (value)
            {
                case "10":
                    return "46 мм (тип 10)";
                case "11":
                    return "59 мм (тип 11)";
                case "12":
                    return "64 мм (тип 12)";
                case "22":
                    return "100 мм (тип 22)";
                case "33":
                    return "155 мм (тип 33)";
            }
            return "любая";
        }

        public int DepthID { get; set; }
        public int WidthID { get; set; }
        public int HeightID { get; set; }
        public decimal MinPower { get; set; }
        public decimal MaxPower { get; set; }

        public List<KeyValuePair<string, string>> Depth { get; set; }
        public List<KeyValuePair<string, string>> Width { get; set; }
        public List<KeyValuePair<string, string>> Height { get; set; }
    }


    public class CatalogItem
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }

        public decimal? Price { get; set; }
        public int? ID { get; set; }
    }

    public partial class StoreCharacterValue
    {
        public string FilterLink
        {
            get { return "#"; }
        }
    }
    public partial class StoreCategory
    {
        private int? _stripeIndex;

        public int StripeIndex
        {
            get
            {
                if (!_stripeIndex.HasValue)
                {
                    _stripeIndex = ID;
                    /*   _stripeIndex = 0;
                    var ordered =
                        CatalogBrowser.CategoriesList.Where(x => x.Value != null)
                            .OrderBy(x => x.Value.ID).ToList();


                    if (ordered.All(x => x.Value.ID != ID))
                        _stripeIndex = -1;
                    else
                        _stripeIndex = ordered.IndexOf(ordered.First(x => x.Value.ID == ID));*/
                }
                return _stripeIndex.Value;
            }
            set { _stripeIndex = value; }
        }

        public string DescriptionShadow
        {
            get { return Description; }
            set { Description = value; }
        }
        public int ExportID { get; set; }
        public int ExportParentID { get; set; }

        public string SlugOrId
        {
            get { return Slug.IsNullOrEmpty() ? ID.ToString() : Slug; }
        }

        public string FullUrl
        {
            get
            {
                var list = new List<string>();
                list.Add(ID > 1 ? CatalogBrowser.BaseURL : "");
                list.AddRange(Segments);
                return
                    list.Where(x => x.IsFilled()).Select(x => HttpContext.Current.Server.UrlPathEncode(x)).JoinToURL();
            }
        }


        private SelectList _productOrderSelectList;
        public SelectList ProductOrderSelectList
        {
            get
            {
                if (_productOrderSelectList == null)
                {
                    var db = new DB();
                    var forList =
                        db.StoreImporters.Where(z => z.ShowInList)
                            .OrderBy(x => x.Priority)
                            .Join(db.StoreCharacters, x => x.Header.ToLower(), y => y.Name.ToLower(),
                                (x, y) => new KeyValuePair<string, string>("Char_" + y.ID, y.Name))
                            .ToList();

                    var list = new List<KeyValuePair<string, string>>();
                    if (forList.Any())
                    {
                        list.AddRange(forList);
                    }
                    forList.AddRange(new[]
                    {
                        new KeyValuePair<string, string>("OrderNum", "Стандартно"),
                        //new KeyValuePair<string, string>( "VoteOverage", "по популярности"),
                        new KeyValuePair<string, string>("AlphaBet", "По алфавиту"),
                        new KeyValuePair<string, string>("Cheap", "Сначала дешевые"),
                        new KeyValuePair<string, string>("Expensive", "Сначала дорогие")
                    });

                    /*
                                        string name = "";
                                        if (CatalogFilter.ProductOrder.StartsWith("Char_"))
                                        {
                                            var c =
                                                db.StoreCharacters.FirstOrDefault(
                                                    x => x.ID == CatalogFilter.ProductOrder.Replace("Char_", "").ToInt());
                                            if (c != null)
                                            {
                                                name = c.Name;
                                            }
                                        }
                                        else
                                        {
                                            if (forList.Any(x => x.Key == CatalogFilter.ProductOrder))
                                            {
                                                name = forList.FirstOrDefault(x => x.Key == CatalogFilter.ProductOrder).Key;
                                            }
                                        }
                    */
                    _productOrderSelectList = new SelectList(forList, "Key", "Value", CatalogFilter.ProductOrder);

                }
                return _productOrderSelectList;
                return _productOrderSelectList ?? (_productOrderSelectList = new SelectList(new[]
                    {
                        
                        new {Value = "OrderNum", Text = "стандартно"},
                        new {Value = "VoteOverage", Text = "по популярности"},
                        new {Value = "AlphaBet", Text = "по алфавиту"},
                        new {Value = "Cheap", Text = "сначала дешевые"},
                        new {Value = "Expensive", Text = "сначала дорогие"}
                    }, "Value", "Text", Enum.GetName(typeof(CatalogListOrder), CatalogFilter.ProductOrder)));
            }
        }


        private SelectList _productPageCountSelectList;
        public SelectList ProducPageCountSelectList
        {
            get
            {
                return _productPageCountSelectList ?? (_productPageCountSelectList = new SelectList(new[]
                    {
                        new {Value = "15", Text = "15"},
                        new {Value = "30", Text = "30"},
                        new {Value = "60", Text = "60"},
                        new {Value = "90", Text = "90"}

                    }, "Value", "Text", CatalogFilter.ProductCount));
            }
        }


        private CatalogFilter _catalogFilter;
        public CatalogFilter CatalogFilter
        {
            get { return _catalogFilter ?? (_catalogFilter = new CatalogFilter()); }
        }

        private PagedData<StoreProduct> _productList;
        public PagedData<StoreProduct> ProductList
        {
            get
            {
                if (AccessHelper.IsMasterPage)
                {
                    _productList = new PagedData<StoreProduct>(
                        new DB().StoreProductsToCategories.Where(
                            x => x.CategoryID == ID && x.StoreProduct.IsActive && !x.StoreProduct.Deleted)
                            .OrderBy(x => x.OrderNum)
                            .Select(x => x.StoreProduct),
                        CatalogBrowser.PageNumber, CatalogFilter.ProductCount);
                    return _productList;

                }

                var npn = 0;
                if (AccessHelper.QueryDict.ContainsKey("page"))
                    npn = AccessHelper.QueryDict["page"].ToNullInt() ?? 0;
                if (!AccessHelper.QueryDict.ContainsKey("word") && !AccessHelper.QueryDict.ContainsKey("filter"))
                {
                    _productList = null;
                }

                if (_productList == null)
                {
                    var productList =
                        new DB().StoreProductsToCategories.Where(x => x.CategoryID == ID && x.StoreProduct.IsActive && !x.StoreProduct.Deleted);


                    IQueryable<StoreProduct> orderedList = null;
                    switch (CatalogFilter.ProductOrder)
                    {
                        default:
                            if (CatalogFilter.ProductOrder.StartsWith("Char_"))
                            {
                                int cid = CatalogFilter.ProductOrder.Replace("Char_", "").ToInt();
                                orderedList =
                                    productList.OrderBy(
                                        x =>
                                            (x.StoreProduct.StoreCharacterToProducts.FirstOrDefault(
                                                z => z.StoreCharacterValue.CharacterID == cid) ??
                                             new StoreCharacterToProduct()
                                             {
                                                 StoreCharacterValue = new StoreCharacterValue() { Value = "ZZZZZZZZZZ" }
                                             })
                                                .StoreCharacterValue.Value.Length).ThenBy(x =>
                                                    (x.StoreProduct.StoreCharacterToProducts.FirstOrDefault(
                                                        z => z.StoreCharacterValue.CharacterID == cid) ??
                                                     new StoreCharacterToProduct()
                                                     {
                                                         StoreCharacterValue =
                                                             new StoreCharacterValue() { Value = "ZZZZZZZZZZZ" }
                                                     })
                                                        .StoreCharacterValue.Value).Select(x => x.StoreProduct);
                            }
                            else
                            {
                                orderedList = productList.OrderBy(x => x.StoreProduct.Price).Select(x => x.StoreProduct);
                            }
                            break;

                        case "OrderNum":
                            orderedList = productList.OrderBy(x => x.OrderNum).Select(x => x.StoreProduct);
                            break;
                        case "AlphaBet":
                            orderedList = productList.OrderByDescending(x=> x.StoreProduct.ViewCount).ThenBy(x => x.StoreProduct.Name).Select(x => x.StoreProduct);
                            break;
                        case "AlphaBetDesc":
                            orderedList = productList.OrderBy(x=> x.StoreProduct.ViewCount).ThenByDescending(x => x.StoreProduct.Name).Select(x => x.StoreProduct);
                            break;
                        case "Cheap":
                            orderedList = productList.OrderBy(x => x.StoreProduct.SitePrice).Select(x => x.StoreProduct);
                            break;
                        case "CheapDesc":
                            orderedList = productList.OrderByDescending(x => x.StoreProduct.SitePrice).Select(x => x.StoreProduct);
                            break;
                        case "Expensive":
                            orderedList =
                                productList.OrderByDescending(x => x.StoreProduct.Price).Select(x => x.StoreProduct);
                            break;
                        case "AddDate":
                            orderedList =
                                productList.OrderByDescending(x => x.StoreProduct.AddDate).Select(x => x.StoreProduct);
                            break;
                        case "VoteOverage":
                            orderedList =
                                productList.OrderByDescending(x => x.StoreProduct.VoteOverage)
                                           .Select(x => x.StoreProduct);

                            break;
                    }
                    if (orderedList == null)
                    {
                        orderedList = new List<StoreProduct>().AsQueryable();
                    }

                    _productList = new PagedData<StoreProduct>(
                        orderedList,
                        CatalogBrowser.PageNumber, CatalogFilter.ProductCount);

                }
                return _productList;
            }
            set { _productList = value; }

        }

        public string ImageUrl
        {
            get
            {
                if (CategoryImage == null || CategoryImage.Length == 0)
                {
                    return "/content/noimage.jpg";
                }

                return "/content/category/" + Slug + "-w250-ci.jpg";

                return UniversalEditorPagedData.GetImageWrapper("StoreCategories", "ID", ID.ToString(), "CategoryImage", 250, 250);
            }
        }
        public string MenuImageUrl
        {
            get
            {
                if (Image == null || Image.Length == 0)
                {
                    return "/content/noimage.jpg";
                }
                return "/content/category/" + Slug + "-w40-mi.jpg";
                return UniversalEditorPagedData.GetImageWrapper("StoreCategories", "ID", ID.ToString(), "Image", 40, 40);
            }
        }

        public string BaseMenuImageUrl
        {
            get
            {
                if (Image == null || Image.Length == 0)
                {
                    return "/content/noimage.jpg";
                }
                return "/content/category/" + Slug + "-w40-ci.jpg";
                return UniversalEditorPagedData.GetImageWrapper("StoreCategories", "ID", ID.ToString(), "CategoryImage", 40, 40);
            }
        }
  public string SearchImageUrl
        {
            get
            {
                if (Image == null || Image.Length == 0)
                {
                    return "/content/noimage.jpg";
                }
                return "/content/category/" + Slug + "-w75-ci.jpg";
                return UniversalEditorPagedData.GetImageWrapper("StoreCategories", "ID", ID.ToString(), "CategoryImage", 75, 75);
            }
        }

        public string[] Segments
        {
            get
            {
                var list = new List<string>();
                var cat = this;
                while (cat != null)
                {
                    cat = CatalogBrowser.CategoriesList.Values.FirstOrDefault(x => x.ID == cat.ParentID && x.ID > 1);
                    if (cat != null)
                    {
                        list.Add(cat.SlugOrId);
                    }
                }
                list.Reverse();
                list.Add(SlugOrId);
                return list.ToArray();
            }
        }

        public string GetExportValue(StoreImporter setting)
        {
            switch (setting.ColumnName)
            {
                case "ID":
                    return ID.ToString();
                case "ParentID":
                    return ParentID.ToString();
                case "CategoryOrder":
                    return OrderNum.ToString();
                case "ProductOrder":
                    return "";
                case "URL":
                    return FullUrl;
                case "PageTitle":
                    return PageTitle;
                case "PageKeywords":
                    return PageKeywords;
                case "PageDescription":
                    return PageDescription;
                case "H1":
                    return PageHeader;
                case "H2":
                    return PageSubHeader;
                case "H3":
                    return PageHeaderH3;
                case "TextUnderH2":
                    return Description;
                case "RelatedCategories":
                    return "";
                case "TextUnderH3":
                    return PageTextH3Lower;
                case "RelatedProductsSame":
                    return
                        StoreCategoryRelations.Where(x => x.StoreProduct != null && x.GroupName == "related")
                            .ToList()
                            .Select(x => x.StoreProduct.SlugOrId)
                            .JoinToString(";");
                case "RelatedProductsBuy":
                    return
                        StoreCategoryRelations.Where(x => x.StoreProduct != null && x.GroupName == "recomend")
                            .ToList()
                            .Select(x => x.StoreProduct.SlugOrId)
                            .JoinToString(";");
                case "RelatedProductsSimilar":
                    return
                        StoreCategoryRelations.Where(x => x.StoreProduct != null && x.GroupName == "similar")
                            .ToList()
                            .Select(x => x.StoreProduct.SlugOrId)
                            .JoinToString(";");
                default:
                    return "";
            }
        }

        public List<StoreFile> GetFileList()
        {
            return
                new DB().StoreFiles.Where(x => x.CategoryID == ID)
                    .OrderBy(x => x.OrderNum)
                    .ToList()
                    .Where(x => x.Link.IsFilled())
                    .ToList();
        }
    }

    public enum CatalogListOrder
    {
        OrderNum,
        VoteOverage,
        AlphaBet,
        AlphaBetDesc,
        Cheap,
        CheapDesc,
        Expensive,
        AddDate
    }


    public class CatalogCharacterFilter
    {
        public List<string> Values { get; set; }
        public List<string> FilteredValues { get; set; }
        public bool IsSlider { get; set; }
        public string Header { get; set; }
        public int CharacterID { get; set; }
        public int OrderNum { get; set; }
        public bool Visible { get; set; }
        public string Help { get; set; }
        public string Range
        {
            get
            {
                if (FilteredValues == null)
                    FilteredValues = new List<string>();

                if (IsSlider)
                {
                    string r = "";
                    if (Values.ElementAt(0).ToDecimal() != FilteredValues.ElementAt(0).ToDecimal())
                        r = FilteredValues.ElementAt(0);
                    //if (Values.ElementAt(1).ToDecimal() != FilteredValues.ElementAt(1).ToDecimal())
                    {
                        if (r.IsFilled())
                            r += "&mdash;";
                        r += FilteredValues.ElementAt(1);
                        r += "&nbsp;";
                    }
                    return r;
                }
                else
                {
                    return "";
                }
            }
        }
        public string GetValue(int i)
        {
            if (FilteredValues != null && FilteredValues.Any())
                return FilteredValues.ElementAt(i);
            return Values.ElementAt(i);
        }

        public bool IsFilled
        {
            get
            {
                if (FilteredValues == null)
                    FilteredValues = new List<string>();

                if (IsSlider)
                {
                    var fn = Values.Select(x => x.ToDecimal()).Min() !=
                             FilteredValues.ElementAt(0).ToDecimal();
                    var sn = FilteredValues.Count > 1 &&
                             Values.Select(x => x.ToDecimal()).Max() !=
                             FilteredValues.ElementAt(1).ToDecimal();
                    return FilteredValues.Any() && (fn || sn);
                }
                else
                {
                    return FilteredValues.Any();
                }
            }
        }


    }
    [Serializable]
    public class CatalogCharacterFilterResult
    {
        public int Count { get; set; }
        [ScriptIgnore]
        public IQueryable<StoreProduct> Products { get; set; }
        public string Link { get; set; }
        public int IsInCatalog { get; set; }
    }

    public class CatalogCharacterFilters
    {
        public string BaseURL
        {
            get
            {
                var u = HttpContext.Current.Request.RawUrl;
                if (u.IndexOf("filter=") >= 0)
                {
                    u = u.Substring(0, u.IndexOf("filter=") - 1);
                }
                if (u.EndsWith("?"))
                    u = u.Substring(0, u.Length - 1);
                return u;
            }
        }

        public List<CatalogCharacterFilter> Filters { get; set; }

        public void Deserealize(string url)
        {
            if (url.IndexOf("filter=") < 0) return;
            var sub = url.Substring(url.IndexOf("filter=") + 7);
            if (sub.IndexOf("&") >= 0)
            {
                sub = sub.Substring(0, sub.IndexOf("&"));
            }
            var filters = sub.Split<string>(";");
            foreach (var filter in filters)
            {
                var els = filter.Split<string>("=").ToList();

                if (els.Count() == 2)
                {
                    var f = Filters.FirstOrDefault(x => x.CharacterID == els.ElementAt(0).ToInt());
                    if (f != null)
                    {
                        var items = els.ElementAt(1).Replace("[", "").Replace("]", "").Split<string>(",");
                        f.FilteredValues = items.Select(x => HttpUtility.UrlDecode(x)).ToList();
                        if (f.FilteredValues.Count < 2)
                        {
                            f.IsSlider = false;
                        }
                    }
                }
            }
        }

        public string Serialize(int pageID, ref int isInCatalog)
        {
            var value = new List<string>();
            foreach (var filter in Filters)
            {
                if (filter.IsSlider && (filter.FilteredValues ?? new List<string>()).Count > 1)
                {
                    if (filter.FilteredValues != null && filter.FilteredValues.Any() && (filter.FilteredValues.ElementAt(0) != filter.Values.ElementAt(0) || (filter.FilteredValues.Count > 1 &&
                        filter.FilteredValues.ElementAt(1) != filter.Values.ElementAt(1))))
                    {
                        value.Add(filter.CharacterID + "=[" + filter.FilteredValues.ElementAt(0) + "," +
                                  filter.FilteredValues.ElementAt(1) + "]");
                    }
                }
                else
                {
                    if (filter.FilteredValues != null && filter.FilteredValues.Any())
                    {
                        value.Add(filter.CharacterID + "=[" + filter.FilteredValues.JoinToString(",") + "]");
                    }
                }
            }
            var p = CMSPage.Get(pageID);
            if (p.Type != CMSPage.GetTypes("Catalog").First())
            {
                isInCatalog = 0;
                var baseURL = CatalogBrowser.CategoriesList.First(x => x.Value.ID > 1).Value.FullUrl;
                return baseURL + "?filter=" +
                       value.JoinToString(";");
            }
            else
            {
                isInCatalog = 1;
                return "filter=" + value.JoinToString(";");
            }
        }

        public string ExcludeParam(object fv)
        {
            var value = new List<string>();
            if (fv is CatalogCharacterFilter)
            {
                foreach (var filter in Filters)
                {
                    if (filter.IsSlider && filter.CharacterID != (fv as CatalogCharacterFilter).CharacterID)
                    {
                        if (filter.FilteredValues != null && filter.FilteredValues.Any() && (filter.FilteredValues.ElementAt(0) != filter.Values.ElementAt(0) || (filter.FilteredValues.Count > 1 &&
                            filter.FilteredValues.ElementAt(1) != filter.Values.ElementAt(1))))
                        {
                            value.Add(filter.CharacterID + "=[" + filter.FilteredValues.ElementAt(0) + "," +
                                      filter.FilteredValues.ElementAt(1) + "]");
                        }
                    }
                    else if (filter.CharacterID != (fv as CatalogCharacterFilter).CharacterID)
                    {
                        if (filter.FilteredValues != null && filter.FilteredValues.Any())
                        {
                            value.Add(filter.CharacterID + "=[" + filter.FilteredValues.JoinToString(",") + "]");
                        }
                    }
                }
            }
            else
            {
                foreach (var filter in Filters)
                {
                    if (filter.IsSlider)
                    {
                        if (filter.FilteredValues != null && filter.FilteredValues.Any() && ((filter.FilteredValues.ElementAt(0) != filter.Values.ElementAt(0) ||
                            (filter.FilteredValues.Count > 1 && filter.FilteredValues.ElementAt(1) != filter.Values.ElementAt(1)))))
                        {
                            value.Add(filter.CharacterID + "=[" + filter.FilteredValues.ElementAt(0) + "," +
                                      filter.FilteredValues.ElementAt(1) + "]");
                        }
                    }
                    else
                    {
                        if (filter.FilteredValues != null && filter.FilteredValues.Any())
                        {
                            value.Add(filter.CharacterID + "=[" +
                                      filter.FilteredValues.Where(x => x != fv.ToString()).JoinToString(",") + "]");
                        }
                    }
                }

            }

            return BaseURL + "?filter=" + value.JoinToString(";");
        }

        public CatalogCharacterFilterResult Calculate(int pageID)
        {

            foreach (var filter in Filters)
            {
                if (filter.Values == null)
                    filter.Values = new List<string>();
                if (filter.FilteredValues == null)
                    filter.FilteredValues = new List<string>();
                if (filter.FilteredValues.Count < 2)
                    filter.IsSlider = false;
            }


            var db = new DB();
            var enumFilters = Filters.Where(x => !x.IsSlider && x.CharacterID > 0);
            var afterEnum = (from enumFilter in enumFilters
                             where enumFilter.FilteredValues.Any()
                             select
                                 db.StoreCharacterValues.Where(
                                     x =>
                                     enumFilter.FilteredValues.Contains(x.Value) &&
                                     x.StoreCharacter.ID == enumFilter.CharacterID)
                                   .SelectMany(
                                       x => x.StoreCharacterToProducts.Select(z => z.StoreProduct)))
                .Aggregate<IQueryable<StoreProduct>, IQueryable<StoreProduct>>(null,
                                                                               (current, cv) =>
                                                                               current != null
                                                                                   ? current.Intersect(cv)
                                                                                   : cv);
            var rangeFilters =
                Filters.Where(
                    x =>
                    x.IsSlider && x.CharacterID > 0 &&
                    ((x.Values.Any() && x.FilteredValues.Any() && x.Values.ElementAt(0).ToDecimal() !=
                     x.FilteredValues.ElementAt(0).ToDecimal()) || (
                     x.Values.Count > 1 && x.FilteredValues.Count > 1 &&
                     x.Values.ElementAt(1).ToDecimal() != x.FilteredValues.ElementAt(1).ToDecimal())));

            var afterRange =
                rangeFilters.Select(
                    rangeFilter =>
                    db.StoreCharacterValues.Where(
                        z =>
                        db.ToDecimal(z.Value) >= rangeFilter.FilteredValues.ElementAt(0).ToDecimal() &&
                        db.ToDecimal(z.Value) <= rangeFilter.FilteredValues.ElementAt(1).ToDecimal() &&
                        z.StoreCharacter.ID == rangeFilter.CharacterID)
                      .SelectMany(x => x.StoreCharacterToProducts.Select(z => z.StoreProduct)))
                            .Aggregate<IQueryable<StoreProduct>, IQueryable<StoreProduct>>(null,
                                                                                           (current, cv) =>
                                                                                           current != null
                                                                                               ? current.Intersect(cv)
                                                                                               : cv);
            var price = Filters.FirstOrDefault(x => x.CharacterID == 0);
            IQueryable<StoreProduct> afterPrice = null;
            if (price != null && price.FilteredValues != null && price.FilteredValues.Any() &&
                (price.FilteredValues.ElementAt(0).ToDecimal() != price.Values.ElementAt(0).ToDecimal() ||
                 (price.FilteredValues.Count > 1 &&
                  price.FilteredValues.ElementAt(1).ToDecimal() != price.Values.ElementAt(1).ToDecimal())))
            {
                afterPrice =
                    db.StoreProducts.Where(
                        z =>
                            !z.Deleted &&
                        z.StoreCharacterToProducts.Any(
                            c =>
                            (c.StoreProduct.Price *
                             (100 - c.StoreProduct.Discount - EventCalendar.CurrentDiscountReal) / 100) >
                            price.Values.ElementAt(0).ToDecimal() &&
                            (c.StoreProduct.Price *
                             (100 - c.StoreProduct.Discount - EventCalendar.CurrentDiscountReal) / 100) <
                            price.Values.ElementAt(1).ToDecimal()));
            }

            IQueryable<StoreProduct> prods = null;
            if (afterEnum != null)
            {
                prods = afterEnum;
                if (afterPrice != null)
                    prods = prods.Intersect(afterPrice);
                if (afterRange != null)
                    prods = prods.Intersect(afterRange);
            }
            else if (afterPrice != null)
            {
                prods = afterPrice;
                if (afterRange != null)
                    prods = prods.Intersect(afterRange);
            }
            else if (afterRange != null)
                prods = afterRange;
            else prods = db.StoreProducts.Where(x => !x.Deleted);

            var count = prods
                .Distinct()
                .Count();

            int isInCatalog = 1;
            var link = Serialize(pageID, ref isInCatalog);
            return new CatalogCharacterFilterResult() { Count = count, Products = prods, Link = link, IsInCatalog = isInCatalog };
        }

        public CatalogCharacterFilters(bool fromQS)
        {
            string url = HttpContext.Current.Request.RawUrl;
            if (url.IndexOf("filter=") < 0) return;
            var sub = url.Substring(url.IndexOf("filter=") + 7);
            if (sub.IndexOf("&") >= 0)
            {
                sub = sub.Substring(0, sub.IndexOf("&"));
            }
            var filters = sub.Split<string>(";");
            Filters = new List<CatalogCharacterFilter>();
            DB db = new DB();
            foreach (var filter in filters)
            {
                var els = filter.Split<string>("=").ToList();
                if (els.Count() == 2)
                {
                    var items = els.ElementAt(1).Replace("[", "").Replace("]", "").Split<string>(",");
                    var filtered = items.Select(x => HttpUtility.UrlDecode(x)).ToList();
                    var charID = els.ElementAt(0).ToInt();
                    var fc = db.StoreCharacters.First(x => x.ID == charID);
                    var values = fc.StoreCharacterValues;
                    Filters.Add(new CatalogCharacterFilter()
                    {
                        IsSlider = values.Count > 20,
                        Header = fc.Name,
                        CharacterID = charID,
                        Values = values.Select(x => x.Value).Distinct().ToList(),
                        FilteredValues = filtered
                    });

                }
            }
        }



        public CatalogCharacterFilters()
        {
            var db = new DB();
            var browser = CatalogBrowser.Init();
            Filter filterEntry = null;
            if (browser.CurrentCategory.ID > 1)
            {
                filterEntry = db.Filters.FirstOrDefault(x => x.CatID == browser.CurrentCategory.ID && x.Visible);
                //filterEntry = db.Filters.FirstOrDefault(x => x.PageID == AccessHelper.CurrentPageInfo.ID && x.Visible)
            }
            else
            {
                filterEntry = db.Filters.FirstOrDefault(x => x.PageID == AccessHelper.CurrentPageInfo.ID && x.Visible);
            }



            Filters = new List<CatalogCharacterFilter>();

            if (filterEntry == null)
                return;

            var chars = filterEntry.FilterItems.Select(x => x.CharID).ToList();

            var settings = db.StoreImporters.Where(x => chars.Contains(x.ID)).ToList();
            /*    var minPrice =
                    db.StoreProducts.Where(x => x.Price > 0)
                      .FirstOrDefault(x => x.Price == db.StoreProducts.Min(z => z.Price));
                var maxPrice =
                    db.StoreProducts.Where(x => x.Price > 0)
                      .FirstOrDefault(x => x.Price == db.StoreProducts.Max(z => z.Price));

                var priceFilter = db.StoreImporters.FirstOrDefault(x => x.ColumnName == "Price");


                if (priceFilter != null && priceFilter.ShowInFilter && minPrice != null && maxPrice != null && minPrice.Price != maxPrice.Price)
                {
                    Filters.Add(new CatalogCharacterFilter()
                        {
                            IsSlider = true,
                            Header = "Цена, руб.",
                            CharacterID = 0,
                            Values =
                                new List<string>()
                                    {
                                        minPrice.TradingPriceDecimal.ToString("f0"),
                                        maxPrice.TradingPriceDecimal.ToString("f0")
                                    }
                        });
                }
    */

            foreach (var setting in settings)
            {
                var filterItem = filterEntry.FilterItems.First(z => z.CharID == setting.ID);
                var isSlider = filterItem.Type == 1;
                var values = db.StoreCharacterValues.Where(x => x.StoreCharacter.Name == setting.Header && x.StoreCharacterToProducts.Any()).ToList();

                if (values.Select(x => x.Value.Trim()).Distinct().Count() > 1)
                {
                    if (isSlider)
                    {
                        var decs = values.Select(x => x.Value.ToDecimal()).ToList();
                        if (decs.Distinct().Count() > 1)
                        {
                            Filters.Add(new CatalogCharacterFilter()
                                {
                                    IsSlider = true,
                                    CharacterID = values.First().CharacterID,
                                    Values = new List<string> { decs.Min().ToString("f1"), decs.Max().ToString("f1") },
                                    Header = filterItem.Name,
                                    OrderNum = filterItem.OrderNum,
                                    Visible = filterItem.Visible ?? false,
                                    Help = filterItem.Help
                                });
                        }
                    }
                    else
                    {
                        Filters.Add(new CatalogCharacterFilter()
                        {
                            CharacterID = values.First().CharacterID,
                            Header = filterItem.Name,
                            IsSlider = false,
                            Values = values.Select(x => x.Value).ToList(),
                            OrderNum = filterItem.OrderNum,
                            Visible = filterItem.Visible ?? false,
                            Help = filterItem.Help

                        });
                    }
                }

            }
            Filters = Filters.OrderBy(x => x.OrderNum).ToList();
            Deserealize(HttpContext.Current.Request.RawUrl);
            foreach (var filter in Filters)
            {
                if (filter.Values == null)
                    filter.Values = new List<string>();
                if (filter.FilteredValues == null)
                    filter.FilteredValues = new List<string>();
            }
        }
        public CatalogCharacterFilters(string objID)
        {
            var db = new DB();
            Filter filterEntry = null;
            if (objID.StartsWith("c"))
            {
                filterEntry = db.Filters.FirstOrDefault(x => x.PageID == CatalogBrowser.ParentPage.ID && x.Visible);
                //filterEntry = db.Filters.FirstOrDefault(x => x.CatID == objID.Substring(1).ToInt() && x.Visible);
            }
            else
            {
                filterEntry = db.Filters.FirstOrDefault(x => x.PageID == objID.Substring(1).ToInt() && x.Visible);
            }


            Filters = new List<CatalogCharacterFilter>();

            if (filterEntry == null)
                return;

            var chars = filterEntry.FilterItems.Select(x => x.CharID).ToList();

            var settings = db.StoreImporters.Where(x => chars.Contains(x.ID)).ToList();
            foreach (var setting in settings)
            {
                var filterItem = filterEntry.FilterItems.First(z => z.CharID == setting.ID);
                var isSlider = filterItem.Type == 1;
                var values = db.StoreCharacterValues.Where(x => x.StoreCharacter.Name == setting.Header && x.StoreCharacterToProducts.Any()).ToList();

                if (values.Select(x => x.Value.Trim()).Distinct().Count() > 1)
                {
                    if (isSlider)
                    {
                        var decs = values.Select(x => x.Value.ToDecimal()).ToList();
                        if (decs.Distinct().Count() > 1)
                        {
                            Filters.Add(new CatalogCharacterFilter()
                                {
                                    IsSlider = true,
                                    CharacterID = values.First().CharacterID,
                                    Values = new List<string> { decs.Min().ToString("f1"), decs.Max().ToString("f1") },
                                    Header = filterItem.Name,
                                    OrderNum = filterItem.OrderNum,
                                    Visible = filterItem.Visible ?? false,
                                    Help = filterItem.Help
                                });
                        }
                    }
                    else
                    {
                        Filters.Add(new CatalogCharacterFilter()
                        {
                            CharacterID = values.First().CharacterID,
                            Header = filterItem.Name,
                            IsSlider = false,
                            Values = values.Select(x => x.Value).ToList(),
                            OrderNum = filterItem.OrderNum,

                            Help = filterItem.Help

                        });
                    }
                }

            }
            Filters = Filters.OrderBy(x => x.OrderNum).ToList();
            Deserealize(HttpContext.Current.Request.RawUrl);
            foreach (var filter in Filters)
            {
                if (filter.Values == null)
                    filter.Values = new List<string>();
                if (filter.FilteredValues == null)
                    filter.FilteredValues = new List<string>();
            }
        }


        public CatalogCharacterFilters(List<string> chars)
        {
            Filters = new List<CatalogCharacterFilter>();

            var db = new DB();
            var settings = db.StoreImporters.Where(x => x.ColumnName == "Character" && chars.Contains(x.Header)).ToList();
            if (chars.Contains("Цена"))
            {
                var minPrice =
                    db.StoreProducts.Where(x => x.Price > 0 && !x.Deleted)
                        .FirstOrDefault(x => x.Price == db.StoreProducts.Min(z => z.Price));
                var maxPrice =
                    db.StoreProducts.Where(x => x.Price > 0 && !x.Deleted)
                        .FirstOrDefault(x => x.Price == db.StoreProducts.Max(z => z.Price));

                var priceFilter = db.StoreImporters.FirstOrDefault(x => x.ColumnName == "Price");


                if (priceFilter != null && priceFilter.ShowInFilter && minPrice != null && maxPrice != null &&
                    minPrice.Price != maxPrice.Price)
                {
                    Filters.Add(new CatalogCharacterFilter()
                    {
                        IsSlider = true,
                        Header = "Цена, руб.",
                        CharacterID = 0,
                        Visible = true,
                        Values =
                            new List<string>()
                            {
                                minPrice.TradingPriceDecimal.ToString("f0"),
                                maxPrice.TradingPriceDecimal.ToString("f0")
                            }
                    });
                }
            }


            foreach (var setting in settings)
            {
                var values = db.StoreCharacterValues.Where(x => x.StoreCharacter.Name == setting.Header && x.StoreCharacterToProducts.Any()).ToList();
                if (values.Select(x => x.Value.Trim()).Distinct().Count() > 1)
                {
                    if (values.All(x => x.Value.IsDecimal()) && values.Count > 20)
                    {
                        var decs = values.Select(x => x.Value.ToDecimal()).ToList();
                        if (decs.Distinct().Count() > 1)
                        {
                            Filters.Add(new CatalogCharacterFilter()
                                {
                                    IsSlider = true,
                                    CharacterID = values.First().CharacterID,
                                    Values = new List<string> { decs.Min().ToString("f1"), decs.Max().ToString("f1") },
                                    Header = setting.Header,
                                    Visible = true
                            });
                        }
                    }
                    else
                    {
                        Filters.Add(new CatalogCharacterFilter()
                            {
                                CharacterID = values.First().CharacterID,
                                Header = setting.Header,
                                IsSlider = false,
                                Values = values.Select(x => x.Value).ToList(),
                                Visible = true,
                        });
                    }
                }

            }
            Filters = Filters.OrderBy(x => !x.IsSlider).ThenBy(x => x.CharacterID).ToList();
            Deserealize(HttpContext.Current.Request.RawUrl);
        }

        public StoreCategory CreateCategory(int pageID)
        {
            var rd = new RouteValueDictionary();
            rd.Add("filter", HttpContext.Current.Request["filter"]);

            var category = new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false};
            var result = Calculate(pageID);

            IOrderedQueryable<StoreProduct> ordered = null;
            switch (category.CatalogFilter.ProductOrder)
            {
                default:
                    if (category.CatalogFilter.ProductOrder.StartsWith("Char_"))
                    {
                        int cid = category.CatalogFilter.ProductOrder.Replace("Char_", "").ToInt();
                        ordered =
                            result.Products.OrderBy(
                                x =>
                                    (x.StoreCharacterToProducts.FirstOrDefault(
                                        z => z.StoreCharacterValue.CharacterID == cid) ??
                                     new StoreCharacterToProduct()
                                     {
                                         StoreCharacterValue = new StoreCharacterValue() { Value = "ZZZZZZZZZZ" }
                                     })
                                        .StoreCharacterValue.Value.Length).ThenBy(
                                            x =>
                                                (x.StoreCharacterToProducts.FirstOrDefault(
                                                    z => z.StoreCharacterValue.CharacterID == cid) ??
                                                 new StoreCharacterToProduct()
                                                 {
                                                     StoreCharacterValue =
                                                         new StoreCharacterValue() { Value = "ZZZZZZZZZZ" }
                                                 })
                                                    .StoreCharacterValue.Value);
                    }
                    else
                    {
                        ordered = result.Products.OrderBy(x => x.StoreProductsToCategories.Any() ? x.StoreProductsToCategories.First().OrderNum : 100000);
                    }
                    break;
                case "OrderNum":
                    ordered = result.Products.OrderBy(x => x.StoreProductsToCategories.Any() ? x.StoreProductsToCategories.First().OrderNum : 100000);
                    break;
                case "AlphaBet":
                    ordered = result.Products.OrderByDescending(x=> x.ViewCount).ThenBy(x => x.Name);
                    break;
                case "AlphaBetDesc":
                    ordered = result.Products.OrderBy(x=> x.ViewCount).ThenByDescending(x => x.Name);
                    break;
                case "Cheap":
                    ordered = result.Products.OrderBy(x => x.SitePrice);
                    break;
                case "CheapDesc":
                    ordered = result.Products.OrderByDescending(x => x.SitePrice);
                    break;
                case "Expensive":
                    ordered =
                        result.Products.OrderByDescending(x => x.Price);
                    break;
                case "AddDate":
                    ordered =
                        result.Products.OrderByDescending(x => x.AddDate);
                    break;
                case "VoteOverage":
                    ordered =
                        result.Products.OrderByDescending(x => x.VoteOverage);
                    break;
            }

            if (ordered == null)
            {
                ordered =
                    result.Products.OrderBy(
                        x => x.StoreProductsToCategories.Any() ? x.StoreProductsToCategories.First().OrderNum : 100000);
            }

            category.ProductList = new PagedData<StoreProduct>(ordered,
                                                               CatalogBrowser.PageNumber,
                                                               category.CatalogFilter.ProductCount, rd);
            return category;

        }
    }

    public class CatalogFilter
    {

        public CatalogCharacterFilter CharacterFilter { get; set; }

        public string ProductOrder
        {
            get
            {

                var cook = HttpContext.Current.Request.Cookies["ProductOrder"];
                if (cook != null && cook.Value.IsFilled())
                {
                    /*
                                        CatalogListOrder order;
                                        if (Enum.TryParse(cook.Value, out order))
                                            return order;
                    */
                    return cook.Value;

                }
                return "";
                /*return CatalogListOrder.OrderNum;*/
            }
        }
        public int ProductCount
        {
            get
            {
                if (AccessHelper.IsMasterPage)
                    return 50;

                var cook = HttpContext.Current.Request.Cookies["ProductCount"];
                if (cook != null && cook.Value.IsFilled())
                {
                    int number;
                    if (int.TryParse(cook.Value, out number))
                        return number;

                }

                return 15;
            }
        }
        public int ProductList
        {
            get
            {
                var cook = HttpContext.Current.Request.Cookies["ProductList"];
                if (cook != null && cook.Value.IsFilled())
                {
                    int number;
                    if (int.TryParse(cook.Value, out number))
                        return number;

                }
                return 2;
            }
        }

        public string ProductView
        {
            get
            {
                switch (ProductList)
                {
                    case 0:
                    default:
                        return "~/Views/ClientCatalog/CatalogListProdictRow.cshtml";
                    case 1:
                        return "~/Views/ClientCatalog/CatalogListProdictTile.cshtml";
                    case 2:
                        return "~/Views/ClientCatalog/CatalogListProdictLine.cshtml";
                }
            }
        }

        public string ProductViewShort
        {
            get
            {
                switch (ProductList)
                {
                    case 0:
                    default:
                        return "CatalogListProdictRow";
                    case 1:
                        return "CatalogListProdictTile";
                    case 2:
                        return "CatalogListProdictLine";
                }
            }
        }
    }

    public class PriceModule
    {
        private StoreProduct _storeProduct;
        public PriceModule(StoreProduct product)
        {
            _storeProduct = product;
        }

        
        public decimal? BasePrice
        {
            get
            {
                if (_storeProduct.PriceBaseRUR.HasValue || _storeProduct.PriceBaseEUR.HasValue)
                {
                    var currency = new Currency();
                    return _storeProduct.PriceBaseRUR ?? _storeProduct.PriceBaseEUR*currency.Rate;
                }
                return null;
            }
        }

        public decimal BuyingRate
        {
            get
            {
                if (!_storeProduct.BuyingRate.HasValue)
                    return 1;

                return _storeProduct.BuyingRate.Value;
            }
        }

        public decimal? BuyingPrice
        {
            get
            {
                if (BasePrice.HasValue)
                    return BasePrice.Value*BuyingRate;
                return null;
            }
        }

        public decimal ProfitRate
        {
            get { return _storeProduct.ProfitRate ?? (decimal)1; }
        }
        public decimal DiscountRate
        {
            get
            {
                if (_storeProduct.DiscountRate == 0)
                    return (decimal) 1;
                return _storeProduct.DiscountRate ?? (decimal)1;
            }
        }

        public decimal SitePrice
        {
            get
            {
                if (BuyingPrice.HasValue)
                    return BuyingPrice.Value * ProfitRate * DiscountRate;
                return _storeProduct.Price;
            }
        }
        public decimal SitePriceWithoutDiscount
        {
            get
            {
                if (BuyingPrice.HasValue)
                    return BuyingPrice.Value * ProfitRate;
                return _storeProduct.Price;
            }
        }

        public decimal ProfitSum
        {
            get
            {
                if (!BuyingPrice.HasValue)
                    return (decimal)0;

                return SitePrice - BuyingPrice.Value;
            }
        }


        public decimal DiscountShedulerRate
        {
            get { return EventCalendar.CurrentDiscountReal; }
        }


        public decimal ShopCartPrice
        {
            get
            {
                var result = SitePrice;
                if (DiscountShedulerRate != 0)
                {
                    var discount = result*DiscountShedulerRate;
                    result -= discount;
                }
                return result;
            }
        }
    }

    public partial class StoreProduct
    {
        private PriceModule _priceModule;
        public PriceModule PriceModule
        {
            get { return _priceModule ?? (_priceModule = new PriceModule(this)); }
        }

        public string DescriptionShadow
        {
            get { return Description; }
            set { Description = value; }
        }
        public IEnumerable<StoreCharacterValue> CharactersForList
        {
            get
            {
                return
                    StoreCharacterToProducts.Where(
                        x =>
                            CatalogBrowser.CharsIdsForList.Select(z => z.Key)
                                .Contains(x.StoreCharacterValue.CharacterID))
                        .Select(x => x.StoreCharacterValue)
                        .OrderBy(x => CatalogBrowser.CharsIdsForList.First(z => z.Key == x.CharacterID).Value);
            }
        }
        public IEnumerable<StoreCharacterValue> CharactersForFullList
        {
            get
            {
                return
                    StoreCharacterToProducts.Where(
                        x =>
                            CatalogBrowser.CharsIdsForFullList.Select(z => z.Key)
                                .Contains(x.StoreCharacterValue.CharacterID))
                        .Select(x => x.StoreCharacterValue)
                        .OrderBy(x => CatalogBrowser.CharsIdsForFullList.First(z => z.Key == x.CharacterID).Value).Take(8);
            }
        }

        public string PageH2OrName
        {
            get
            {
                if (PageH2.IsFilled())
                    return PageH2;
                return Name;
            }
        }

        public string JoinedCats { get; set; }
        public string JoinedChars { get; set; }

        private string _tagList;
        public string TagList
        {
            get
            {
                return _tagList ??
                       (_tagList = StoreProductTagRels.Select(x => x.StoreProductTag.Tag).ToList().JoinToString(","));
            }
            set { _tagList = value; }
        }

        private int? _isUserFavorite;
        public int IsUserFavorite
        {
            get
            {
                if (!_isUserFavorite.HasValue)
                {
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                        _isUserFavorite = 0;
                    else
                        _isUserFavorite = StoreProductFavorites.Any(x => x.UserID == HttpContext.Current.GetCurrentUserUID()) ? 1 : 0;
                }
                return _isUserFavorite.Value;
            }
        }

        public int AverageRounded
        {
            get { return (int)Math.Round(VoteOverage, 0); }
        }

        public string SlugOrId
        {
            get { return Slug.IsNullOrEmpty() ? ID.ToString() : Slug; }
        }

        public string FullUrl
        {
            get
            {

                var cnt = 1;//CatalogBrowser.Products.Count(x => x.Value == SlugOrId);
                    
                    
                    /*new DB().StoreProducts.Count(x => x.Slug == SlugOrId);*/
                    
                    
                var segments = new List<string>
                {
                    CatalogBrowser.BaseURL.Trim('/'),
                    SlugOrId + (cnt > 1 ? ("-" + ID) : "")
                };
                return
                    segments.Where(x => !x.IsNullOrEmpty())
                        .Distinct()
                        .Select(x => HttpContext.Current.Server.UrlPathEncode(x))
                        .JoinToURL();
/*
                var segments = new List<string>();
                segments.Add(CatalogBrowser.BaseURL.Trim('/'));
                segments.AddRange(StoreProductsToCategories.Any()
                                ? StoreProductsToCategories.OrderBy(x => x.CategoryID).Last().StoreCategory.Segments
                                : new String[] { CatalogBrowser.RootSlug });
                segments.Add(SlugOrId);
                return segments.Where(x=> !x.IsNullOrEmpty()).Distinct().Select(x => HttpContext.Current.Server.UrlPathEncode(x)).JoinToURL();
*/
            }
        }

        public string DefaultThumbURL
        {
            get
            {
                var thumb =
                    (StoreImages.FirstOrDefault(x => x.Enabled) ??
                     new StoreImage() { UrlPath = "/content/noimage.jpg" }).UrlPath;
                if (thumb.IsNullOrEmpty())
                    thumb = "/content/noimage.jpg";
                thumb = thumb.Trim('~');

                return thumb.Replace(".", "-w180" + (thumb.Contains("noimage") ? "-nrt" : "") + ".");


                var rq = HttpContext.Current.Request.RequestContext;
                var helper = new UrlHelper(rq);
                var routeValues = new RouteValueDictionary
                {
                    {"maxWidth", 180},
                    {"maxHeight", 180},
                    {"filePath", thumb},
                    {"padding", 0},
                    {"skipLogo", false}
                };
                if (thumb.Contains("noimage"))
                {
                    routeValues.Add("skipRotate", true);
                }
                return UrlHelper.GenerateUrl("Master", "Resize", "Image", routeValues, helper.RouteCollection, rq, true);
            }
        }

        public string DefaultImage
        {
            get
            {

                var thumb =
                    (StoreImages.FirstOrDefault(x => x.Enabled) ??
                     new StoreImage() { UrlPath = "/content/noimage.jpg" }).UrlPath;
                if (thumb.IsNullOrEmpty())
                    thumb = "/content/noimage.jpg";
                thumb = thumb.Trim('~');

                return thumb.Replace(".", "-w250" + (thumb.Contains("noimage") ? "-nrt" : "") + ".");

                var rq = HttpContext.Current.Request.RequestContext;
                var helper = new UrlHelper(rq);
                var routeValues = new RouteValueDictionary
                    {
                        {"maxWidth", 250},
                        {"maxHeight", 250},
                        {"filePath", thumb},
                        {"padding", 0},
                        {"skipLogo", false}
                    };


                if (thumb.Contains("noimage"))
                {
                    routeValues.Add("skipRotate", true);
                }


                return UrlHelper.GenerateUrl("Master", "Resize", "Image", routeValues, helper.RouteCollection, rq, true);
            }
        }

        public string DefaultImageLink
        {
            get
            {
                var thumb =
                  (StoreImages.FirstOrDefault(x => x.Enabled) ??
                   new StoreImage() { UrlPath = "/content/noimage.jpg" });

                if (!string.IsNullOrEmpty(thumb.Youtube))
                    return thumb.Youtube.Replace("watch?v=", "v/");

                if (thumb.UrlPath.IsNullOrEmpty())
                    thumb.UrlPath = "/content/noimage.jpg";
                thumb.UrlPath = thumb.UrlPath.Trim('~');



                return thumb.UrlPath.Replace(".", "-w250" + (thumb.UrlPath.Contains("noimage") ? "-nrt" : "") + ".");


                var rq = HttpContext.Current.Request.RequestContext;
                var helper = new UrlHelper(rq);
                var routeValues = new RouteValueDictionary
                    {
                        {"maxWidth", 250},
                        {"maxHeight", 250},
                        {"filePath", thumb.UrlPath},
                        {"padding", 0},
                        {"skipLogo", false}
                    };


                if (thumb.UrlPath.Contains("noimage"))
                {
                    routeValues.Add("skipRotate", true);
                }


                return UrlHelper.GenerateUrl("Master", "Resize", "Image", routeValues, helper.RouteCollection, rq, true);

                return thumb.UrlPath;
            }
        }
     public string DefaultFancyType
        {
            get
            {
                var thumb =
                  (StoreImages.FirstOrDefault(x => x.Enabled) ??
                   new StoreImage() { UrlPath = "/content/noimage.jpg" });

                if (!string.IsNullOrEmpty(thumb.Youtube))
                    return "iframe";

                return "image";

            }
        }

        public string GetThumbURL(int maxWidth, int maxHeight, string vert = "", bool skiplogo = false)
        {
            var thumb =
                (StoreImages.OrderBy(x=> x.OrderNum).FirstOrDefault(x => x.Enabled) ??
                 new StoreImage() { UrlPath = "/content/noimage.jpg" }).UrlPath;
            if (thumb.IsNullOrEmpty())
                thumb = "/content/noimage.jpg";
            thumb = thumb.Trim('~');


            return thumb.Replace(".",
                "-w" + maxWidth + (skiplogo || thumb.Contains("noimage") ? "-nlg" : "") +
                (thumb.Contains("noimage") ? "-nrt" : "") + ".");


            var rq = HttpContext.Current.Request.RequestContext;
            var helper = new UrlHelper(rq);
            var routeValues = new RouteValueDictionary
                {
                    {"skiplogo", skiplogo},
                    {"filePath", thumb},
                    {"padding", 0},
                    {"maxWidth", maxWidth},
                    {"maxHeight", maxHeight},
                    {"vertalign", vert.IsFilled() ? vert : "center"}
                };
            if (thumb.Contains("noimage"))
            {
                routeValues.Add("skipRotate", true);
            }

            return UrlHelper.GenerateUrl("Master", "Resize", "Image", routeValues, helper.RouteCollection, rq, true);
        }
     public string GetImgURL(StoreImage img, int maxWidth, int maxHeight, string vert = "", bool skiplogo = false)
        {
            var thumb =
                (img ??
                 new StoreImage() { UrlPath = "/content/noimage.jpg" }).UrlPath;
            if (thumb.IsNullOrEmpty())
                thumb = "/content/noimage.jpg";
            thumb = thumb.Trim('~');



            return thumb.Replace(".",
                "-w" + maxWidth + (skiplogo || thumb.Contains("noimage") ? "-nlg" : "") +
                (thumb.Contains("noimage") ? "-nrt" : "") + ".");

            var rq = HttpContext.Current.Request.RequestContext;
            var helper = new UrlHelper(rq);
            var routeValues = new RouteValueDictionary
                {
                    {"skiplogo", skiplogo},
                    {"filePath", thumb},
                    {"padding", 0},
                    {"maxWidth", maxWidth},
                    {"maxHeight", maxHeight},
                    {"vertalign", vert.IsFilled() ? vert : "center"}
                };
            if (thumb.Contains("noimage"))
            {
                routeValues.Add("skipRotate", true);
            }

            return UrlHelper.GenerateUrl("Master", "Resize", "Image", routeValues, helper.RouteCollection, rq, true);
        }

        public string TradingPrice
        {
            get { return (Price * (100 - DiscountTotal - EventCalendar.CurrentDiscountReal) / 100).ToNiceDigit(); }
        }
        public decimal TradingPriceDecimal
        {
            get { return (Price * (100 - DiscountTotal - EventCalendar.CurrentDiscountReal) / 100); }
        }
        public decimal TradingPriceForCartDecimal
        {
            get { return (TradingPriceDecimal * (100 - CommonDiscountForCart) / 100); }
        }
        public string TradingPriceForCart
        {
            get { return (TradingPriceDecimal * (100 - CommonDiscountForCart) / 100).ToNiceDigit(); }
        }
        public string OriginalPrice
        {
            get { return (Price).ToNiceDigit(); }
        }

        public decimal ProductDiscount
        {
            get { return Math.Abs((TradingPriceDecimal * (100 - CommonDiscountForCart) / 100) - (Price * (100 - DiscountTotal - EventCalendar.CurrentDiscountReal) / 100)); }
        }

        public decimal DiscountTotal
        {
            get { return Discount; }
        }


        public static decimal CommonDiscountForCart
        {
            get
            {
                return (EventCalendar.CurrentDiscountReal < 0 ? EventCalendar.CurrentDiscountForTrade : 0) +
                       SiteSetting.Get<int>("OrderDiscount");
            }
        }

        public bool NeedShowArticle
        {
            get { return StoreProductsToCategories.All(x => x.StoreCategory.ShowArticles) && Article.IsFilled(); }
        }

        public string NameOrDef
        {
            get
            {
                if (Name.IsFilled())
                    return Name;
                if (PageH1.IsFilled())
                    return PageH1;
                if (PageH2.IsFilled())
                    return PageH2;
                if (PageTitle.IsFilled())
                    return PageTitle;
                return PageH2OrName;
            }
        }

        public string DefaultAlt
        {
            get
            {
                var i = StoreImages.FirstOrDefault(x => x.Enabled);
                if (i != null)
                {
                    return i.Alt;
                }
                return "";
            }
        }

        public string GetExportValue(StoreImporter setting)
        {
            switch (setting.ColumnName)
            {
                case "PriceBaseEUR":
                    return PriceBaseEUR.ToString();
                case "PriceBaseRUR":
                    return PriceBaseRUR.ToString();
                case "BuyingRate":
                    return (BuyingRate ??1).ToString();
                case "ProfitRate":
                    return (ProfitRate??1).ToString();
                case "DiscountRate":
                    return (DiscountRate??1).ToString();
                case "RecType":
                    return "товар";
                case "FolderNum":
                    return "";
                case "ProductFolders":
                    return "";
                case "ID":
                    return "";
                case "ParentID":
                    return "";
                case "CategoryOrder":
                    return "";
                case "ProductOrder":
                    return "";
                case "URL":
                    return FullUrl;
                case "PageTitle":
                    return PageTitle;
                case "PageKeywords":
                    return PageKeywords;
                case "PageDescription":
                    return PageDescription;
                case "H1":
                    return PageH1;
                case "H2":
                    return PageH2;
                case "H3":
                    return PageH3;
                case "TextUnderH2":
                    return Description;
                case "TextUnderH3":
                    return DescrptionLower;
                case "Artikul":
                case "Article":
                    return Article;
                case "Weight":
                    return Weight.ToString();
                case "Volume":
                    return Volume.ToString();
                case "Discount":
                    return Discount.ToString();
                case "Visible":
                    return IsActive ? "Да" : "Нет";
                case "Price":
                    return Price.ToString();
                case "SitePrice":
                    return SitePrice.ToString();
                case "Name":
                    return Name;
                case "ShortName":
                    return ShortName;
                case "Character":
                    var charVal =
                        StoreCharacterToProducts.FirstOrDefault(
                            x => x.StoreCharacterValue.StoreCharacter.Name.ToLower() == setting.Header.ToLower().Trim());
                    if (charVal != null)
                        return charVal.StoreCharacterValue.Value;
                    else return "";
                case "SearchTags":
                    return StoreProductTagRels.Select(x => x.StoreProductTag.Tag).ToList().JoinToString("; ");
                case "RelatedProductsSame":
                    return
                        StoreProductRelations.Where(x => x.BaseProductReverse != null && x.GroupName == "related")
                            .ToList()
                            .Select(x => x.BaseProductReverse.SlugOrId)
                            .JoinToString(";");

                case "SimilarArt":
                    return
                        StoreProductRelations.Where(x => x.BaseProductReverse != null && x.GroupName == "similar")
                            .ToList()
                            .Select(x => x.BaseProductReverse.Article)
                            .JoinToString(";");


                case "RelatedProductsBuy":
                    return
                        StoreProductRelations.Where(x => x.BaseProductReverse != null && x.GroupName == "recomend")
                            .ToList()
                            .Select(x => x.BaseProductReverse.SlugOrId)
                            .JoinToString(";");
                case "RecommendArt":
                    return
                        StoreProductRelations.Where(x => x.BaseProductReverse != null && x.GroupName == "recomend")
                            .ToList()
                            .Select(x => x.BaseProductReverse.Article)
                            .JoinToString(";");

                case "RelatedProductsSimilar":
                    return
                        StoreProductRelations.Where(x => x.BaseProductReverse != null && x.GroupName == "similar")
                            .ToList()
                            .Select(x => x.BaseProductReverse.SlugOrId)
                            .JoinToString(";");
                case "RelatedArt":
                    return
                        StoreProductRelations.Where(x => x.BaseProductReverse != null && x.GroupName == "related")
                            .ToList()
                            .Select(x => x.BaseProductReverse.Article)
                            .JoinToString(";");

                case "RelatedCategories":
                    return RelatedCategories;
                default:
                    return "";
            }
        }

        public List<StoreFile> GetFileList()
        {
            var db = new DB();
            var cats = db.StoreProductsToCategories.Where(x => x.ProductID == ID).ToList();
            var files = cats.SelectMany(x => x.StoreCategory.StoreFiles).OrderBy(x => x.OrderNum).ToList();
            files.AddRange(db.StoreFiles.Where(x => x.ProductID == ID).OrderBy(x => x.OrderNum).ToList());
            return files.Where(x=> x.Link.IsFilled()).ToList();
        }
    }   


    public class CatalogBrowser
    {
        private string _url;
        protected CatalogBrowser(string url)
        {
            _url = url;
        }

        public static StoreCategory Root
        {
            get { return CategoriesList.First(x => x.Value.ID == 1).Value; }
        }

        public static CatalogBrowser Init(string url = "")
        {
            if (HttpContext.Current.Items.Contains("CatalogBrawser") &&
                HttpContext.Current.Items["CatalogBrawser"] is CatalogBrowser)
                return HttpContext.Current.Items["CatalogBrawser"] as CatalogBrowser;
            else
            {
                var br = new CatalogBrowser(url);
                if (HttpContext.Current.Items.Contains("CatalogBrawser"))
                    HttpContext.Current.Items.Remove("CatalogBrawser");

                HttpContext.Current.Items.Add("CatalogBrawser", br);
                return br;
            }
        }

        private DB db = new DB();
        private static string _baseURL;
        public static string BaseURL
        {
            get
            {
                if (_baseURL.IsNullOrEmpty())
                {
/*
                    var p = ParentPage;
                    if (p != null)
                        _baseURL = p.FullUrl;
                    else
*/
                        _baseURL = "";
                }
                return _baseURL;
            }
        }

        public static int LastPageNum { get; set; }
        public static int PageNumber
        {
            get
            {
                var lastNum = 0;
                var dict = AccessHelper.QueryDict;
                if (dict.ContainsKey("page"))
                    lastNum = dict["page"].ToNullInt() ?? 0;
                else lastNum = 0;
                LastPageNum = lastNum;
                return lastNum;
            }
        }
        public static int TakeCount
        {
            get { return 36; }
        }
        public static int SkipCount
        {
            get { return PageNumber * TakeCount; }
        }
        private static string _rootSlug;
        public static string RootSlug
        {
            get
            {
                if (_rootSlug.IsNullOrEmpty())
                {
                    _rootSlug =
                        (new DB().StoreCategories.FirstOrDefault(x => x.ID == 1) ?? new StoreCategory() { ID = 0, LastMod = DateTime.Now, ShowBigIcons = false})
                            .SlugOrId;
                }
                return _rootSlug;
            }
        }

        private static CMSPage _parentPage;
        public static CMSPage ParentPage
        {
            get
            {
                if (_parentPage == null)
                {
                    var cell = new DB().CMSPageCellViews.FirstOrDefault(x => x.Action == "CatalogBrowser");
                    _parentPage = cell == null ? null : CMSPage.GetByType(cell.CMSPageCell.PageType.ID).FirstOrDefault();
                }
                return _parentPage;
            }
        }

        private StoreProduct _currentProduct;
        public StoreProduct CurrentProduct
        {
            get
            {
                if (_currentProduct == null)
                {
                    var segs = Segments;
                    if (IsProductPage)
                    {
                        var slug = segs.Last();
                        if (TrunkedLink)
                        {
                            var arr = slug.Split<string>("-").ToList();

                            _currentProduct =
                                db.StoreProducts.FirstOrDefault(
                                    c => c.ID == Products.First(x => x.Key == arr.Last().ToInt()).Key);
                        }
                        else
                        {
                            _currentProduct =
                                db.StoreProducts.FirstOrDefault(c => c.ID == Products.First(x => x.Value == slug).Key);
                        }
                    }
                }
                return _currentProduct;

            }
        }
        protected string[] Segments
        {
            get
            {
                string url = _url.IsFilled() ? _url : HttpContext.Current.Request.Url.AbsoluteUri;
                var urls = url.Split(new[] { '?' })[0].Replace(".aspx", "")
                                      .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(HttpUtility.UrlDecode).ToList();

                return urls.ToArray();
            }
        }

        private StoreCategory _currentCategory;
        public StoreCategory CurrentCategory
        {
            get
            {
                if (_currentCategory == null)
                {

                    var rootExist = CategoriesList.Any(x => x.Value.ID == 1);
                    if (!rootExist)
                    {
                        ClearAllCaches();
                    }
                    rootExist = CategoriesList.Any(x => x.Value.ID == 1);
                    var segs = Segments;

                    if (!rootExist || !segs.Any())
                    {
                        return new StoreCategory() { ID = 0, LastMod = DateTime.Now, ShowBigIcons = false};
                    }



                    if (IsCategoryPage && !IsProductPage)
                        _currentCategory = CategoriesList.ContainsKey(segs.Last())
                            ? CategoriesList[segs.Last()]
                            : /*new StoreCategory() { ID = 0 };*/
                            CategoriesList.First(x => x.Value.ID == 1).Value;
                    else if (IsProductPage)
                    {
                        _currentCategory = CurrentProduct.StoreProductsToCategories.Any() ?
                            CurrentProduct.StoreProductsToCategories.First().StoreCategory :
                            new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false };

/*
                            CategoriesList.ContainsKey(segs.ElementAt(segs.Count() - 2))
                                ? CategoriesList[segs.ElementAt(segs.Count() - 2)]
                                : new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false};
*/
                    }
                    else
                    {
                        _currentCategory = new StoreCategory() { ID = 0, LastMod = DateTime.Now, ShowBigIcons = false};
                    }
                }
                return _currentCategory;
            }
        }

        public bool IsCategory(CMSPage page)
        {
            if (!_isCategoryPage.HasValue)
            {
                var segs = Segments;

                if (Products.ContainsValue(segs.Last()))
                {
                    _isCategoryPage = false;
                }
                else
                {
                    if (CategoriesList.ContainsKey(segs.Last()))
                        _isCategoryPage = true;
                    else
                    {
                        var catParentPage = ParentPage;
                        if (catParentPage == null)
                        {
                            _isCategoryPage = false;
                        }
                        else
                        {
                            _isCategoryPage = catParentPage.ID == (page ?? AccessHelper.CurrentPageInfo.CurrentPage).ID;
                        }
                    }
                }
            }
            return _isCategoryPage.Value;
        }

        private bool? _isCategoryPage;
        public bool IsCategoryPage
        {
            get { return IsCategory(null); }
        }

        public bool IsFiltered
        {
            get
            {
                return HttpContext.Current.Request.QueryString["query"].IsFilled();
            }
        }

        public bool IsFilterPage
        {
            get { return HttpContext.Current.Request.QueryString["filter"].IsFilled(); }
        }

        private List<int> _parentIds;
        public List<int> ParentIds
        {
            get
            {
                if (_parentIds == null)
                {
                    _parentIds = new List<int>();
                    //if (IsCategoryPage)
                    {
                        _parentIds.Add(CurrentCategory.ID);
                        var c = CurrentCategory;
                        int counter = 0;
                        while (c.ParentID.HasValue && counter < 10)
                        {

                            _parentIds.Add(c.ParentID.Value);
                            c = CategoriesList.First(x => x.Value.ID == c.ParentID.Value).Value;
                            counter++;
                        }
                    }
                }

                return _parentIds;
            }
        }

        public bool TrunkedLink { get; set; }


        private bool? _isProductPage;
        public bool IsProductPage
        {
            get
            {
                if (!_isProductPage.HasValue)
                {
                    var segs = Segments;

                    _isProductPage = Products.ContainsValue(segs.Last());
                    if (!_isProductPage.Value)
                    {
                        var trunks = segs.Last().Split<string>("-").ToList();
                        if (trunks.Count > 1 && Products.ContainsKey(trunks.Last().ToInt()))
                        {
                            _isProductPage = true;
                            TrunkedLink = true;
                        }
                    }
                }
                return _isProductPage.Value;
            }
        }

        private static int? _cacheDuration;
        private static int CACHE_DURATION
        {
            get
            {
                if (!_cacheDuration.HasValue)
                {
                    _cacheDuration = ConfigurationManager.AppSettings["CacheDuration"].ToNullInt() ?? 5;
                }
                return _cacheDuration.Value;
            }
        }
        public static Dictionary<string, StoreCategory> CategoriesList
        {
            get
            {
                var o = HttpContext.Current.Cache.Get("CategoriesList");
                if (o == null || !(o is Dictionary<string, StoreCategory>))
                {

                    var db = new DB();
                    var dlo = new DataLoadOptions();
                    dlo.LoadWith<StoreCategory>(x => x.StoreProductsToCategories);
                    dlo.LoadWith<StoreProduct>(x => x.StoreProductsToCategories);
                    db.LoadOptions = dlo;
                    var list = db.StoreCategories.Where(x => !x.Deleted).ToList();
                    var d = new Dictionary<string, StoreCategory>();
                    foreach (var category in list.Where(category => !d.ContainsKey(category.SlugOrId)))
                    {
                        d.Add(category.SlugOrId.Clear(new[] { " ", "\r", "\n", "\t" }), category);
                    }
                    o = d;
                    HttpContext.Current.Cache.Add("CategoriesList", o, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, CACHE_DURATION, 0)
                                                  , CacheItemPriority.Normal, null);

                    return o as Dictionary<string, StoreCategory>;
                }
                return o as Dictionary<string, StoreCategory>;
            }

        }

        public static Dictionary<int, string> Categories
        {
            get
            {
                var o = HttpContext.Current.Cache.Get("Categories");
                if (o == null || !(o is Dictionary<int, string>))
                {
                    o = CategoriesList.ToDictionary(x => x.Value.ID, y => y.Value.SlugOrId);
                    HttpContext.Current.Cache.Add("Categories", o, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, CACHE_DURATION, 0), CacheItemPriority.Normal, null);

                    return o as Dictionary<int, string>;
                }
                return o as Dictionary<int, string>;
            }
        }
        public static Dictionary<int, string> Products
        {
            get
            {
                var o = HttpContext.Current.Cache.Get("Products");
                if (o == null || !(o is Dictionary<int, string>))
                {
                    var db = new DB();
                    o = db.StoreProducts.Where(x => !x.Deleted).ToList()
                                .ToDictionary(x => x.ID, y => y.SlugOrId.Clear(new[] { " ", "\r", "\n", "\t" }, ""));

                    HttpContext.Current.Cache.Add("Products", o, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, CACHE_DURATION, 0), CacheItemPriority.Normal, null);

                    return o as Dictionary<int, string>;
                }
                return o as Dictionary<int, string>;
            }
        }
        public static Dictionary<int, int> ProductCounts
        {
            get
            {
                var o = HttpContext.Current.Cache.Get("ProductCounts");
                if (o == null || !(o is Dictionary<int, int>))
                {
                    var db = new DB();
                    o = db.StoreCategories.Where(x => !x.Deleted).ToList()
                                .ToDictionary(x => x.ID, y => y.StoreProductsToCategories.Count(x => !x.StoreProduct.Deleted));

                    HttpContext.Current.Cache.Add("ProductCounts", o, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, CACHE_DURATION, 0), CacheItemPriority.Normal, null);

                    return o as Dictionary<int, int>;
                }
                return o as Dictionary<int, int>;
            }
        }

        public bool NotFound
        {
            get
            {
                return !IsCategoryPage && !IsProductPage;
            }
        }

        private static List<KeyValuePair<int, int>> _charsIdsForList;
        public static List<KeyValuePair<int, int>> CharsIdsForList
        {
            get
            {
                if (_charsIdsForList == null)
                {
                    var db = new DB();
                    var forList = db.StoreImporters.Where(z => z.ShowInList).OrderBy(x => x.Priority);
                    _charsIdsForList =
                        db.StoreCharacters.Where(
                            x => forList.Any(z => z.Header == x.Name))
                            .ToList()
                            .Select(
                                x => new KeyValuePair<int, int>(x.ID, forList.First(z => z.Header == x.Name).Priority))
                            .ToList();
                }

                return _charsIdsForList;
            }
        }
        private static List<KeyValuePair<int, int>> _charsIdsForFullList;
        public static List<KeyValuePair<int, int>> CharsIdsForFullList
        {
            get
            {
                if (_charsIdsForFullList == null)
                {
                    var db = new DB();
                    var forList = db.StoreImporters.OrderBy(x => x.Priority);
                    _charsIdsForFullList =
                        db.StoreCharacters.Where(
                            x => forList.Any(z => z.Header == x.Name))
                            .ToList()
                            .Select(
                                x => new KeyValuePair<int, int>(x.ID, forList.First(z => z.Header == x.Name).Priority))
                            .ToList();
                }

                return _charsIdsForFullList;
            }
        }

        public bool IsNotFound(CMSPage page)
        {
            var segs = Segments;

            if (Products.ContainsValue(segs.Last()))
            {
                _isCategoryPage = false;
            }
            else
            {
                if (CategoriesList.ContainsKey(segs.Last()))
                    _isCategoryPage = true;
                else
                {
                    var catParentPage = ParentPage;
                    if (catParentPage == null)
                    {
                        _isCategoryPage = false;
                    }
                    else
                    {
                        _isCategoryPage = catParentPage.ID == page.ID;
                    }
                }
            }

            return ((bool)(!_isCategoryPage)) && !IsProductPage;
        }


        public void ClearAllCaches()
        {
            ClearDirectoriesCache();
            ClearProductsCache();
        }

        public void ClearProductsCache()
        {
            HttpContext.Current.Cache.Remove("Products");
            HttpContext.Current.Cache.Remove("ProductCounts");
        }
        public void ClearDirectoriesCache()
        {

            HttpContext.Current.Cache.Remove("CatalogMenuSprite");
            HttpContext.Current.Cache.Remove("CategoriesList");
            HttpContext.Current.Cache.Remove("Categories");
        }

        public static StoreCategory GetCategory(int sectionID)
        {
            return
                CategoriesList.First(x => x.Value.ID == sectionID).Value;
        }

        public static List<int> GetChildrenCategories(int section)
        {
            var list = new List<int>();
            var cat = CategoriesList.Values.FirstOrDefault(x => x.ID == section);
            PopulateChildren(cat, ref list);
            return list;
        }

        private static void PopulateChildren(StoreCategory cat, ref List<int> list)
        {
            if (cat != null)
            {
                if (!list.Contains(cat.ID))
                {
                    list.Add(cat.ID);
                }
                var children = CategoriesList.Values.Where(x => x.ParentID == cat.ID);
                foreach (var child in children)
                {
                    PopulateChildren(child, ref list);
                }

            }
        }


        private static Dictionary<int, int> _countTable;
        public static Dictionary<int, int> CountTable
        {
            get
            {
                if (_countTable == null)
                {
                    var tbl = new DB().StoreProductsToCategories.GroupBy(x => x.StoreCategory)
                        .Select(x => new { CatID = x.Key, Counter = x.Count() })
                        .ToList();

                    _countTable = tbl.ToDictionary(x => x.CatID.ID, y => y.Counter);


                }
                return _countTable;
            }
        }

        public bool IsMainPage
        {
            get { return AccessHelper.IsMainPage; }
        }

        public int GetCount(int id)
        {
            if (CountTable.ContainsKey(id))
                return CountTable[id];
            return 0;
        }

        public List<CMSPageSlider> GetSlider(int id, bool isProduct = false)
        {
            if(isProduct)
                return db.CMSPageSliders.Where(x => x.ProductID == id).ToList();
            return db.CMSPageSliders.Where(x=> x.CategoryID == id).ToList();
        }

        public CategoryFilter GetFilter(int id, HttpRequest request)
        {
            var filter = new CategoryFilter(id);
            filter.Query = request["query"];
            return filter;
        }

        public string IncreaseViewCount(int id)
        {
            try
            {
                var p = db.StoreProducts.FirstOrDefault(x => x.ID == id);
                if (p != null)
                {
                    p.ViewCount++;
                    db.SubmitChanges();
                }
            }
            catch
            {
                
            }
            return "";
        }

        public PagedData<StoreProduct> GetFilterResult(int CategoryID, HttpRequest request)
        {
            IQueryable<StoreProduct> result;
            var category = CategoriesList.FirstOrDefault(x => x.Value.ID == CategoryID).Value;
            try
            {
                var query = new JavaScriptSerializer().Deserialize<List<FilterQueryEntry>>(request["query"]);

                var list = CategoryFilter.SearchQuery(query).Distinct();
                var ids = list.Select(x => x.ID).Distinct().ToList();

                var children = GetChildrenCategories(CategoryID)/*.JoinToString(";")*/;

                var uniq = CategoriesList.Where(x => children.Contains(x.Value.ID))
                    .Select(x => x.Value)
                    .SelectMany(x => x.StoreProductsToCategories)
                    .Select(x => x.ProductID)
                    .Intersect(ids).Distinct();

                result = uniq.Join(list, x => x, y => y.ID, (x, y) => y).AsQueryable();
                switch (category.CatalogFilter.ProductOrder)
                {
                    default:
                      
                        break;

                    case "AlphaBet":
                        result =
                            result.OrderByDescending(x => x.ViewCount)
                                .ThenBy(x => x.Name);
                        break;
                    case "AlphaBetDesc":
                        result =
                            result.OrderBy(x => x.ViewCount)
                                .ThenByDescending(x => x.Name);
                        break;
                    case "Cheap":
                        result = result.OrderBy(x => x.SitePrice);
                        break;
                    case "CheapDesc":
                        result =
                            result.OrderByDescending(x => x.SitePrice);
                        break;
                }
            }
            catch
            {
                result = new List<StoreProduct>().AsQueryable();
            }

            return new PagedData<StoreProduct>(result, PageNumber, category.CatalogFilter.ProductCount, new RouteValueDictionary() { { "query", HttpUtility.UrlEncode(request["query"]) } });
        }

        public List<CMSPageVideo> GetVideo(int CategoryID)
        {
            return db.CMSPageVideos.Where(x => x.CategoryID == CategoryID).ToList();
        }
    }
}