using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Smoking.Models;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;

namespace Smoking.Controllers
{
    public class SearchController : Controller
    {

        public ActionResult Admin(string word, int? page)
        {
            var db = new DB();
            ViewBag.Word = word;

            if (word.StartsWith("http://"))
            {
                var slug = word.Split<string>("/").LastOrDefault();
                if (!slug.IsNullOrEmpty())
                {
                    var results = CatalogBrowser.Categories.Where(x => x.Value == slug)
                        .Select(x => db.StoreCategories.FirstOrDefault(z => z.ID == x.Key))
                        .Where(x => x != null)
                        .Select(x => new fSearchResult() {ID = x.ID, PageName = x.Name, Rank = 100, Type = 0}).Concat(
                            CatalogBrowser.Products.Where(x => x.Value == slug)
                                .Select(x => db.StoreProducts.FirstOrDefault(z => z.ID == x.Key))
                                .Where(x => x != null)
                                .Select(x => new fSearchResult()
                                {
                                    ID = x.ID,
                                    PageName = x.Name,
                                    Rank = 100,
                                    Type = 1
                                })).ToList();
                    if (results.Any())
                    {
                        ViewBag.Count = results.Count();
                        return PartialView(new PagedData<fSearchResult>(results.AsQueryable(), page ?? 0, 30));

                    }

                }
            }

            
            var result = db.fSearch(word, 20);
            ViewBag.Count = result.Count();
            return PartialView(new PagedData<fSearchResult>(result, page ?? 0, 30));
        }

        [ClientTemplate("Результаты поиска")]
        public ActionResult Index(string word, int? section, int? page)
        {
            var rd = new RouteValueDictionary();
            rd.Add("word", word ?? "");
            rd.Add("section", section ?? 0);
            var category = new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false};
            var searcher = new Searcher(word ?? "", section ?? 0);

            

            var item = searcher.FullTextSearchQuery;
            switch (category.CatalogFilter.ProductOrder)
            {
                default:
                    if (category.CatalogFilter.ProductOrder.StartsWith("Char_"))
                    {
                        int cid = category.CatalogFilter.ProductOrder.Replace("Char_", "").ToInt();
                        item =
                            item.OrderBy(
                                x =>
                                    (x.StoreCharacterToProducts.FirstOrDefault(
                                        z => z.StoreCharacterValue.CharacterID == cid) ??
                                     new StoreCharacterToProduct()
                                     {
                                         StoreCharacterValue = new StoreCharacterValue() {Value = "ZZZZZZZZZZ"}
                                     })
                                        .StoreCharacterValue.Value.Length).ThenBy(
                                            x =>
                                                (x.StoreCharacterToProducts.FirstOrDefault(
                                                    z => z.StoreCharacterValue.CharacterID == cid) ??
                                                 new StoreCharacterToProduct()
                                                 {
                                                     StoreCharacterValue =
                                                         new StoreCharacterValue() {Value = "ZZZZZZZZZZ"}
                                                 })
                                                    .StoreCharacterValue.Value);
                    }
                    break;
                case "OrderNum":
                    item = item.OrderBy(x => x.StoreProductsToCategories.Any() ? x.StoreProductsToCategories.First().OrderNum : 100000);
                    break;
                case "AlphaBet":
                    item = item.OrderByDescending(x=> x.ViewCount).ThenBy(x => x.Name);
                    break;
                case "AlphaBetDesc":
                    item = item.OrderBy(x=> x.ViewCount).ThenByDescending(x => x.Name);
                    break;
                case "Cheap":
                    item = item.OrderBy(x => x.SitePrice);
                    break;
                case "CheapDesc":
                    item = item.OrderByDescending(x => x.SitePrice);
                    break;
                case "Expensive":
                    item =
                        item.OrderByDescending(x => x.Price);
                    break;
                case "AddDate":
                    item =
                        item.OrderByDescending(x => x.AddDate);
                    break;
                case "VoteOverage":
                    item =
                        item.OrderByDescending(x => x.VoteOverage);
                    break;
            }

            if ((word??"").Length <= 2)
            {
                item = new List<StoreProduct>().AsQueryable();
            }


            var paged = new PagedData<StoreProduct>(item,
                CatalogBrowser.PageNumber,
                category.CatalogFilter.ProductCount, rd);
            category.ProductList = paged;
            return
                PartialView(category);
        }

        public ActionResult FastList(string term, int section)
        {
            var contentResult = new ContentResult() { ContentEncoding = Encoding.UTF8, ContentType = "text/plain" };




            var result = new List<SearchItem>();
            var serializer = new JavaScriptSerializer();


            if (string.IsNullOrEmpty(term) || term.Length < 1)
            {
                contentResult.Content = serializer.Serialize(result);
                return contentResult;
            }
            var searcher = new Searcher(term, section);


            contentResult.Content = serializer.Serialize(searcher.FullTextSearchResult);
            return contentResult;


            var tmp = searcher.ProductList;
            term = searcher.Term;
            var terms = new[] { term, (term + " ").Replace("  ", " ") };
            var rpts = new[] { term.Replace(" ", "_"), (term + " ").Replace(" ", "_").Replace("__", "_") };

            var names = tmp.Select(x => x.Name.ToLower()).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                for (int j = 0; j < terms.Length; j++)
                {
                    names[i] = names[i].Replace(terms[j], rpts[j]);
                }
            }


            var rpl = names
                         .SelectMany(x => x.Split<string>(" ")).Select(x => x.Trim(',', ';'));

            var words =
                rpl
                    .Where(
                        x => rpts.Any(z => x.StartsWith(z, StringComparison.CurrentCultureIgnoreCase)) && x.Length > 3)
                    .Distinct().Select(x => x.Replace("_", " "));


            var re =
                words.Select(x => new SearchItem() { label = x, count = tmp.Count(z => z.Name.ToLower().Contains(x.ToLower())) })
                     .Select(x => new SearchItem() { label = "{0} - около {1} товаров".FormatWith(x.label, x.count), value = x.label });



            contentResult.Content = serializer.Serialize(re);
            return contentResult;



            /*      var names = tmp.Select(x => x.Name).Take(limit).ToArray();
                  var r =
                      GuessGroups(names, 15, 4)
                          .ToList()
                          .Select(
                              x =>
                              new SearchItem()
                                  {
                                      label = string.Format("{0} - (около {1} товаров)", ClearLast(x.Item1), x.Item2.Count()),
                                      count = x.Item2.Count()
                                  }).Where(x=> x.label.Contains(term))
                          .ToArray()
                          .GroupBy(x => x.label)
                          .Select(x => new SearchItem() {count = x.Sum(z => z.count), label = x.First().label});


                  contentResult.Content = serializer.Serialize(r);
                  return contentResult;*/
        }

        private string ClearLast(string inp)
        {
            var arr = inp.Trim().Split<string>(" ").ToArray();
            return arr.Last().Length < 3
                       ? ClearLast(arr.Take(arr.Count() - 1).JoinToString(" "))
                       : arr.JoinToString(" ");
        }


        IEnumerable<Tuple<String, IEnumerable<string>>> GuessGroups(IEnumerable<string> source, int minNameLength = 0, int minGroupSize = 1)
        {
            // TODO: error checking
            return InnerGuessGroups(new Stack<string>(source.OrderByDescending(x => x)), minNameLength, minGroupSize);
        }

        IEnumerable<Tuple<String, IEnumerable<string>>> InnerGuessGroups(Stack<string> source, int minNameLength, int minGroupSize)
        {
            if (source.Any())
            {
                var tuple = ExtractTuple(GetBestGroup(source, minNameLength), source);
                if (tuple.Item2.Count() >= minGroupSize)
                    yield return tuple;
                foreach (var element in GuessGroups(source, minNameLength, minGroupSize))
                    yield return element;
            }
        }

        Tuple<String, IEnumerable<string>> ExtractTuple(string prefix, Stack<string> source)
        {
            return Tuple.Create(prefix, PopWithPrefix(prefix, source).ToList().AsEnumerable());
        }

        IEnumerable<string> PopWithPrefix(string prefix, Stack<string> source)
        {
            while (source.Any() && source.Peek().StartsWith(prefix))
                yield return source.Pop();
        }

        string GetBestGroup(IEnumerable<string> source, int minNameLength)
        {
            var s = new Stack<string>(source);
            var counter = new DictionaryWithDefault<string, int>(0);
            while (s.Any())
            {
                var g = GetCommonPrefix(s);
                if (!string.IsNullOrEmpty(g) && g.Length >= minNameLength)
                    counter[g]++;
                s.Pop();
            }
            return counter.OrderBy(c => c.Value).Last().Key;
        }

        string GetCommonPrefix(IEnumerable<string> coll)
        {
            return (from len in Enumerable.Range(0, coll.Min(s => s.Length)).Reverse()
                    let possibleMatch = coll.First().Substring(0, len)
                    where coll.All(f => f.StartsWith(possibleMatch))
                    select possibleMatch).FirstOrDefault();
        }

        public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
        {
            TValue _default;
            public TValue DefaultValue
            {
                get { return _default; }
                set { _default = value; }
            }
            public DictionaryWithDefault() : base() { }
            public DictionaryWithDefault(TValue defaultValue)
                : base()
            {
                _default = defaultValue;
            }
            public new TValue this[TKey key]
            {
                get { return base.ContainsKey(key) ? base[key] : _default; }
                set { base[key] = value; }
            }
        }


    }
}