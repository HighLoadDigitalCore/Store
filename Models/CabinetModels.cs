using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Smoking.Extensions;

namespace Smoking.Models
{

    public class ProfileEdit
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }

    public class CabinetMenu
    {
        public static List<string> CSS = new List<string>()
            {
                "private_office_nav-profile",
                "private_office_nav-messages",
                "private_office_nav-orders",
                "private_office_nav-fav_list"
            };

        public static List<string> Names = new List<string>()
            {
                "Профиль",
                "Адреса",
                "Покупки",
                "Избранное"
            };

        public static List<string> URL = new List<string>()
            {
                "profile",
                "adresses",
                "orders",
                "favorite"
            };

        public CabinetMenu(string url = "")
        {
            if (url.IsNullOrEmpty())
                url = HttpContext.Current.Request.RawUrl;

            if (url.IndexOf("?view=") >= 0)
            {
                var last = url.Substring(url.IndexOf("?view=") + 6);
                if (last.Contains("&"))
                    last = last.Substring(0, last.IndexOf("&"));
                CurrentMenuIndex = URL.IndexOf(last);
            }
            else
            {
                CurrentMenuIndex = 0;
            }

        }

        public static string GetPageURL(string url)
        {
            var u = BaseURL;
            if (URL.IndexOf(url) >= 0)
            {
                u += "?view=" + url;
            }
            return u;
        }

        public int CurrentMenuIndex { get; set; }

        private static string _baseURL;
        public static string BaseURL
        {
            get
            {
                if (_baseURL.IsNullOrEmpty())
                {
                    _baseURL = CMSPage.GetPageLinkByType("Cabinet");
                }
                return _baseURL;
            }
        }

        public static StoreCategory GetFavoriteCategory()
        {
            var db = new DB();
            var item =
                db.StoreProductFavorites.Where(x => x.UserID == HttpContext.Current.GetCurrentUserUID())
                  .Select(x => x.StoreProduct)
                  .Where(x => x.IsActive);

            var category = new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false};

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
                    else
                    {
                        item = item.OrderBy(x => x.StoreProductsToCategories.Any() ? x.StoreProductsToCategories.First().OrderNum : 100000);
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


            var rd = new RouteValueDictionary();
            category.ProductList = new PagedData<StoreProduct>(item, CatalogBrowser.PageNumber, category.CatalogFilter.ProductCount, rd);

            return category;

        }
    }
}