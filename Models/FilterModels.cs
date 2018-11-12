using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Smoking.Extensions;

namespace Smoking.Models
{

    public class FilterQueryEntry
    {
        public int id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class CategoryFilter
    {

        public static int SearchCount(int CategoryID, List<FilterQueryEntry> query)
        {
            var all = SearchQuery(query).Select(x => x.ID).Distinct().ToList();

            var children = CatalogBrowser.GetChildrenCategories(CategoryID)/*.JoinToString(";")*/;

/*
            children =
                children.Where(
                    x =>
                        CatalogBrowser.CategoriesList.Any(
                            z => z.Value.StoreProductsToCategories.Any(c => c.CategoryID == x))).ToList();
*/

            return CatalogBrowser.CategoriesList.Where(x => children.Contains(x.Value.ID))
                .Select(x => x.Value)
                .SelectMany(x => x.StoreProductsToCategories)
                .Select(x => x.ProductID)
                .Intersect(all).Distinct().Count();

            /*
                        var pids =
                            CatalogBrowser.CategoriesList.SelectMany(x => x.Value.StoreProductsToCategories)
                                .Where(x => children.Contains(x.CategoryID));
            */
            /*var db = new DB();



            var products = db.getIntListByJoinedString(children.JoinToString(";"), ";")
                .Join(db.StoreProductsToCategories, x => x.ID, y => y.CategoryID,
                    (x, y) => new { y.StoreProduct }).Where(x => !x.StoreProduct.Deleted).Select(x => x.StoreProduct);*/

        }

        public static IQueryable<StoreProduct> SearchQuery(List<FilterQueryEntry> query)
        {

            var db = new DB();
            var products = db.StoreProducts.Where(x => !x.Deleted);

            if (query.Any(x => x.type == "price"))
            {
                var vv = query.First(x => x.type == "price").value.Split<string>(";").Select(x=> x.ToDecimal()).ToList();
                products = products.Where(x => x.SitePrice >= vv[0] && x.SitePrice <= vv[1]);
            }

            products = (from entry in query.Where(x => x.type == "check")
                select entry.value.Split<int>(";").ToList()
                into vv
                where vv.Any()
                select db.StoreCharacterValues.Where(x => vv.Contains(x.ID))).Aggregate(products,
                    (current, chars) =>
                        chars.SelectMany(x => x.StoreCharacterToProducts)
                            .Select(x => x.StoreProduct)
                            .Join(current, x => x.ID, y => y.ID, (x, y) => y));

            foreach (var entry in query.Where(x => x.type == "range"))
            {

                var vv = entry.value.Split<string>(";").ToList();


                if (vv.All(x => x.IsDecimal()))
                {

                    products =
                        products.Where(
                            x =>
                                x.StoreCharacterToProducts.Any(
                                    z =>
                                        z.StoreCharacterValue.CharacterID == entry.id &&
                                        z.StoreCharacterValue.DecimalValue.HasValue &&
                                        z.StoreCharacterValue.DecimalValue >= vv[0].ToDecimal() &&
                                        z.StoreCharacterValue.DecimalValue <= vv[1].ToDecimal()));
                }
                else
                {
                    products =
                        products.Where(
                            x =>
                                x.StoreCharacterToProducts.Any(
                                    z =>
                                        z.StoreCharacterValue.CharacterID == entry.id &&
                                        z.StoreCharacterValue.Value == entry.value));
                }


            }


            return query.Where(x => x.type == "value")
                .Aggregate(products,
                    (current, entry) =>
                        current.Where(
                            x =>
                                x.StoreCharacterToProducts.Any(
                                    z =>
                                        z.StoreCharacterValue.CharacterID == entry.id &&
                                        z.StoreCharacterValue.Value == entry.value)));
          
        }

        public int CategoryID { get; set; }
        public int FilterRootID { get; set; }

        public string BaseURL { get; set; }

        public Filter FilterEntry { get; set; }
        private DB _db;
        public CategoryFilter(int CategoryID)
        {
            this.CategoryID = CategoryID;
            _db = new DB();
            var cat = CatalogBrowser.GetCategory(CategoryID);
            if (cat != null)
            {
                BaseURL = cat.FullUrl;
            }
            FilterEntry = _db.Filters.FirstOrDefault(x => x.CatID == CategoryID);
            FilterRootID = CategoryID;
            Filters = new List<CategoryFilterItem>();

            if (FilterEntry == null)
            {
                FilterEntry = SearchFilterInRoot(CategoryID);
            }
            if (FilterEntry != null)
            {
                Filters =
                    FilterEntry.FilterItems.OrderBy(x => x.OrderNum)
                        .Select(x => new CategoryFilterItem(x, CategoryID))
                        .ToList()
                        .Where(x => !x.Item.CharID.HasValue || x.PossibleValues.Count > 1)
                        .ToList();
            }

        }

        private Filter SearchFilterInRoot(int Cat)
        {
            while (true)
            {
                var me = CatalogBrowser.CategoriesList.FirstOrDefault(x => x.Value.ID == Cat).Value;
                var parent = _db.StoreCategories.FirstOrDefault(x => x.ID == me.ParentID);
                if (parent == null)
                    return null;
                else
                {
                    if (parent.Filters.Any())
                    {
                        FilterRootID = parent.ID;
                        return parent.Filters.First();
                    }
                    else
                    {
                        Cat = parent.ID;
                    }
                }
            }


        }

        public List<CategoryFilterItem> Filters { get; set; }
        public string Query { get; set; }
    }

    public class CategoryFilterItem
    {
        private DB _db;
        public FilterItem Item { get; set; }
        private int CatID { get; set; }


        private List<int> _allChildren;
        public List<int> AllChildren
        {
            get { return _allChildren ?? (_allChildren = CatalogBrowser.GetChildrenCategories(CatID)); }
        }

        public int MinPrice
        {
            get
            {
                try
                {
                    return (int)_db.getIntListByJoinedString(AllChildren.JoinToString(";"), ";")
                        .Join(_db.StoreProductsToCategories, x => x.ID, y => y.CategoryID,
                            (x, y) => new {y.StoreProduct})
                        .Where(x => !x.StoreProduct.Deleted).Where(x => x.StoreProduct.SitePrice.HasValue)
                        .Min(x => x.StoreProduct.SitePrice.Value);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private bool? _isAllDecimal;
        public bool IsAllDecimal
        {
            get
            {
                if (!_isAllDecimal.HasValue)
                {
                    _isAllDecimal = PossibleValues.All(x => x.IsDecimal());
                }
                return _isAllDecimal.Value;
            }
        }


        private List<string> _possibleValues;
        public List<string> PossibleValues
        {
            get
            {
                if (_possibleValues == null)
                {
                    if (!Item.CharID.HasValue)
                    {
                        _possibleValues = new List<string>();
                    }
                    else
                    {
                        _possibleValues = _db.getIntListByJoinedString(AllChildren.JoinToString(";"), ";")
                            .Join(_db.StoreProductsToCategories, x => x.ID, y => y.CategoryID,
                                (x, y) => new { y.StoreProduct }).Where(x => !x.StoreProduct.Deleted)
                            .SelectMany(x => x.StoreProduct.StoreCharacterToProducts)
                            .Where(x => x.StoreCharacterValue.CharacterID == Item.CharID)
                            .Select(x => x.StoreCharacterValue.Value)
                            .Distinct()
                            .OrderBy(x => x).ToList();

                        var not = _possibleValues.Where(x => !x.IsDecimal()).ToList();
                        if (not.Count > 0)
                        {
                            if (((decimal)not.Count * 100 / _possibleValues.Count) < 5)
                            {
                                _possibleValues = _possibleValues.Except(not).ToList();
                            }
                        }

                        if (_possibleValues.All(x => x.IsDecimal()))
                        {
                            _possibleValues = _possibleValues.Select(x => x.ToDecimal()).Distinct()
                                 .OrderBy(x => x)
                                 .Select(x => x.ToString())
                                 .ToList();
                        }
                    }
                }
                return _possibleValues;
            }
        }


        private List<StoreCharacterValue> _possibleItems;
        public List<StoreCharacterValue> PossibleItems
        {
            get
            {
                if (_possibleItems == null)
                {
                    if (!Item.CharID.HasValue)
                    {
                        _possibleItems = new List<StoreCharacterValue>();
                    }
                    else
                    {
                        _possibleItems = _db.getIntListByJoinedString(AllChildren.JoinToString(";"), ";")
                            .Join(_db.StoreProductsToCategories, x => x.ID, y => y.CategoryID,
                                (x, y) => new { y.StoreProduct }).Where(x => !x.StoreProduct.Deleted)
                            .SelectMany(x => x.StoreProduct.StoreCharacterToProducts)
                            .Where(x => x.StoreCharacterValue.CharacterID == Item.CharID)
                            .Select(x => x.StoreCharacterValue)
                            .Distinct()
                            .OrderBy(x => x.Value).ToList();

                        if (_possibleItems.All(x => x.DecimalValue.HasValue))
                        {
                            _possibleItems = _possibleItems.OrderBy(x => x.DecimalValue).ToList();
                        }

                    }
                }
                return _possibleItems;
            }
        }


        public int MaxPrice
        {
            get
            {
                try
                {
                    return (int) _db.getIntListByJoinedString(AllChildren.JoinToString(";"), ";")
                        .Join(_db.StoreProductsToCategories, x => x.ID, y => y.CategoryID,
                            (x, y) => new {y.StoreProduct})
                        .Where(x => !x.StoreProduct.Deleted).Where(x => x.StoreProduct.SitePrice.HasValue)
                        .Max(x => x.StoreProduct.SitePrice.Value);
                }
                catch
                {
                    return int.MaxValue;
                }
            }
        }

        public CategoryFilterItem(FilterItem item, int Cat)
        {
            _db = new DB();
            Item = item;
            CatID = Cat;
        }
    }
}