using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;

namespace Smoking.Models
{
    public partial class fSearchResultItem
    {
        private fSearchResult _searchResult;
        public fSearchResultItem(fSearchResult searchResult)
        {
            _searchResult = searchResult;
        }

        public string Link
        {
            get
            {
                var db = new DB();
                switch (_searchResult.Type)
                {
                    case 0:
                        var cat = db.StoreCategories.FirstOrDefault(x => x.ID == _searchResult.ID);
                        if (cat != null)
                        {
                            return cat.FullUrl;
                        }
                        return "";
                    case 1:
                        var prod = db.StoreProducts.FirstOrDefault(x => x.ID == _searchResult.ID);
                        if (prod != null)
                        {
                            return prod.FullUrl;
                        }
                        return "";
                    case 2:
                        var page = db.CMSPages.FirstOrDefault(x => x.ID == _searchResult.ID);
                        if (page != null)
                        {
                            return page.FullUrl;
                        }
                        return "";
                    default:
                        return "";
                }
            }
        }  
        public string Description
        {
            get
            {
                var db = new DB();
                switch (_searchResult.Type)
                {
                    case 0:
                        var cat = db.StoreCategories.FirstOrDefault(x => x.ID == _searchResult.ID);
                        if (cat != null)
                        {
                            var d = cat.Description;
                            if (d.IsNullOrEmpty())
                                d = cat.PageKeywords;
                            return d;
                        }
                        return "";
                    case 1:
                        var prod = db.StoreProducts.FirstOrDefault(x => x.ID == _searchResult.ID);
                        if (prod != null)
                        {
                            var d = prod.Description;
                            if (d.IsNullOrEmpty())
                                d = prod.PageKeywords;
                            return d;
                        }
                        return "";
                    case 2:
                        var page = db.CMSPages.FirstOrDefault(x => x.ID == _searchResult.ID);
                        if (page != null)
                        {
                            var d = page.Description;
                            if (d.IsNullOrEmpty())
                                d = page.Keywords;
                            return d;
                        }
                        return "";
                    default:
                        return "";
                }
            }
        }

        public string Arg
        {
            get
            {
                switch (_searchResult.Type)
                {
                    case 0:
                        return "c" + _searchResult.ID;
                    case 1:
                        return "p" + _searchResult.ID;
                    case 2:
                        return "x" + _searchResult.ID;
                    default:
                        return "r0";
                }
            }
        }
    }
    public class Searcher
    {
        public static int SEARCH_LIMIT = 200;
        private string _word;
        private int _section;
        private bool _needDescr;
        private DB db;
        public Searcher(string word, int section, bool needDescr = false)
        {
            _word = word.ToLower();
            _needDescr = needDescr;
            _section = section;
            db = new DB();
        }

        public List<SearchItem> FullTextSearchResult
        {
            get
            {
                if (string.IsNullOrEmpty(_word) || _word.Length <= 2)
                {
                    return new List<SearchItem>();
                }

                var result =
                    db.fSearch(_word, 8).ToList().Where(x=> (x.Type ?? 0) != 0)
                        .Select(
                            x =>
                                x.Type == 0
                                    ? CreateCategory(x)
                                    : CreateProd(x)).DistinctBy(x=> x.url).ToList();
                return result;
            }
        }
        public IQueryable<StoreProduct> FullTextSearchQuery
        {
            get
            {
                return
                    db.fSearch(_word, 8)
                        .Where(x => (x.Type ?? 0) != 0)
                        .Join(db.StoreProducts, x => x.ID, y => y.ID, (x, y) => y).Distinct();
            }
        }

        private SearchItem CreateProd(fSearchResult x)
        {
            var prod = db.StoreProducts.First(z => z.ID == x.ID);
            return new SearchItem()
            {
                label = x.PageName,
                type = x.Type ?? 0,
                text = x.PageName,
                value = x.PageName,
                url = prod.FullUrl,
                img = prod.GetThumbURL(75, 75, skiplogo: true),
                article = prod.Article,
                price = ((int)(prod.SitePrice ?? 0)).ToString("### ### ###")+" руб."
            };
        }

        private SearchItem CreateCategory(fSearchResult x)
        {
            var cat = CatalogBrowser.CategoriesList.Values.First(z => z.ID == x.ID);
            return new SearchItem()
            {
                label = x.PageName,
                text = x.PageName,
                type = x.Type ?? 0,
                value = x.PageName,
                url = cat.FullUrl,
                img = cat.SearchImageUrl,
                article = "",
                price = ""
            };
        }


        public IEnumerable<FastSearchTempItem> ProductList
        {
            get
            {

                var tmp = ProductListQuery;
                if (_needDescr)
                    return tmp
                              .ToList()
                              .Select(
                                  x =>
                                  new FastSearchTempItem()
                                      {
                                          ID = x.ID,
                                          Name = x.Name,
                                          Description = x.Description.ClearHTML().TruncateToSymbol(200, " "),
                                          URL = x.FullUrl
                                      }).ToArray();


                return tmp
                    .Select(x => new { x.ID, x.Name })
                    .ToList()
                    .Select(x => new FastSearchTempItem() { ID = x.ID, Name = x.Name }).ToArray();
            }
        }

        public IQueryable<StoreProduct> ProductListQuery
        {
            get
            {
                var tmp =
                   db.StoreProducts.Where(x=> !x.Deleted).AsQueryable();
                if (_section > 1)
                {
                    var ps = CatalogBrowser.GetChildrenCategories(_section);
                    tmp = tmp.Where(x => x.StoreProductsToCategories.Any(z => ps.Contains(z.CategoryID)));
                }

                var byWord = tmp.Where(
                    x =>
                        SqlMethods.Like(x.Name.ToLower(), _word + "%") ||
                        SqlMethods.Like(x.Name.ToLower(), "% " + _word + "% ") ||
                        SqlMethods.Like(x.Name.ToLower(), "% " + _word + " %") ||
                        SqlMethods.Like(x.Name.ToLower(), "% " + _word));



                var byTag =
                    tmp.Where(
                        x =>
                        x.StoreProductTagRels.Any(
                            z => SqlMethods.Like(z.StoreProductTag.Tag.ToLower(), "%" + _word + "%")));

                var result = byWord.Concat(byTag).Distinct().Take(SEARCH_LIMIT);


                if (!result.Any() && _word.IsEnglish())
                {
                    _word = _word.ToRussian();
                    result =
                        tmp.Where(
                            x =>
                            SqlMethods.Like(x.Name.ToLower(), _word + "% ") ||
                            SqlMethods.Like(x.Name.ToLower(), "% " + _word + "% ") ||
                            SqlMethods.Like(x.Name.ToLower(), "% " + _word + " %") ||
                            SqlMethods.Like(x.Name.ToLower(), "% " + _word))
                           .Concat(tmp.Where(
                               x =>
                               x.StoreProductTagRels.Any(
                                   z => SqlMethods.Like(z.StoreProductTag.Tag.ToLower(), "%" + _word + "%"))))
                           .Distinct()
                           .Take(SEARCH_LIMIT);

                }
                return result;
            }
        }

        public string Term
        {
            get { return _word; }
        }
    }

    public class FastSearchTempItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public string URL { get; set; }
    }

    [Serializable]
    public class SearchItem
    {
        public int type { get; set; }
        public string url { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public string text { get; set; }
        public int count { get; set; }
        public string img { get; set; }
        public string article { get; set; }
        public string price { get; set; }
    }
}