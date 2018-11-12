using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;

namespace Smoking.Models
{

    public class CommonPageInfo
    {

        public static CommonPageInfo InitFromQueryParams(string query)
        {
            var parts = query.Split<string>("?").ToList();
            return InitFromQueryParams(parts[0].Split<string>("/").ToList(),
                                       parts.Count == 2
                                           ? parts[1].Split<string>("?", "&")
                                                     .Select(x => x.Split<string>("=").ToArray())
                                                     .Where(x => x.Length == 2)
                                                     .Select(x => new KeyValuePair<string, string>(x[0], x[1])).ToList()
                                           : new List<KeyValuePair<string, string>>());
        }
        public static CommonPageInfo InitFromQueryParams()
        {
            var url = HttpContext.Current.Request.RawUrl;
            if (url.Contains("?"))
                url = url.Substring(0, url.IndexOf("?"));
            return InitFromQueryParams(url.Split<string>("/").ToList());
        }
        public static CommonPageInfo InitFromQueryParams(List<string> slashedParams)
        {
            return InitFromQueryParams(slashedParams,
                                       HttpContext.Current.Request.QueryString.ToString()
                                                  .Split<string>("?", "&")
                                                  .Select(x => x.Split<string>("=").ToArray()).Where(x => x.Length == 2)
                                                  .Select(x => new KeyValuePair<string, string>(x[0], x[1])).ToList());

        }
        public static CommonPageInfo InitFromQueryParams(List<string> allSlashedParams, List<KeyValuePair<string, string>> queryParams)
        {

            var db = new DB();
            var url = "";

            foreach (var allSlashedParam in allSlashedParams)
            {

            }


            var slashedParams =
                allSlashedParams.Where(x => AccessHelper.CurrentLang.AvailableList.All(z => z.ShortName != x)).ToList();

            //var request = HttpContext.Current.Request;
            url = slashedParams.All(x => x.IsNullOrEmpty())
                      ? db.CMSPages.First(x => x.PageType.TypeName == "Catalog").URL
                      : slashedParams.Last(x => !x.IsNullOrEmpty());



            var routes = new RouteValueDictionary();
            CommonPageInfo info;
            var main = CatalogBrowser.ParentPage;
            if (main == null)
                main = db.CMSPages.FirstOrDefault(x => x.PageType.TypeName == "MainPage");
            if (main == null)
                main = db.CMSPages.First();

            if (slashedParams.Any() && slashedParams[0] == "Master")
            {
                var offset = AccessHelper.CurrentLang.AvailableList.Any(z => slashedParams.Contains(z.ShortName)) ? 0 : 1;
                routes = ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.Values;
                if (routes.ContainsKey("url"))
                    return InitFromQueryParams(routes["url"].ToString());

                if (slashedParams.Count == 1)
                {
                    slashedParams.Add(AccessHelper.MasterPanel.DefaultController);
                    slashedParams.Add(AccessHelper.MasterPanel.DefaultAction);
                }

                if (slashedParams.Count < 3)
                    slashedParams.Add("Index");




                info = new CommonPageInfo()
                    {
                        Controller = slashedParams[2 - offset],
                        Action = slashedParams[3 - offset],
                        CurrentPage = main.LoadLangValues(),
                        CurrentLang = AccessHelper.CurrentLang,
                        Layout = "_Master",
                        Routes = ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.Values
                    };
                return info;
            }

            var pathPairs =
                slashedParams.Where(x => !x.IsNullOrEmpty()).Select((x, index) => new { Key = "url" + (index + 1), Value = x }).ToList();



            var cntBefore = pathPairs.Count;
            pathPairs = pathPairs.Where(x => !CatalogBrowser.CategoriesList.ContainsKey(x.Value) || x.Value == "catalog").ToList();
            pathPairs = pathPairs.Where(x => !CatalogBrowser.Products.ContainsValue(x.Value) || x.Value == "catalog").ToList();
            if (pathPairs.Count != cntBefore && pathPairs.Count > 0)
                url = pathPairs.Last().Value;

            foreach (var pair in pathPairs)
            {
                routes.Add(pair.Key, pair.Value);
            }
            foreach (var pair in queryParams)
            {
                routes.Add(pair.Key, pair.Value);
            }
            var cmsPage = db.CMSPages.FirstOrDefault(x => x.URL.ToLower() == url.ToLower());
            var browser = CatalogBrowser.Init();

            if (cmsPage == null && CatalogBrowser.CategoriesList.ContainsKey(url.ToLower()))
            {
                cmsPage = db.CMSPages.FirstOrDefault(x=> x.ID == main.ID);
            }

/*
            var trunks = url.Split<string>("-").ToList();
            if (trunks.Count() > 1 && CatalogBrowser.Products.ContainsKey(trunks.Last().ToInt()))
            {
                cmsPage = db.CMSPages.FirstOrDefault(x => x.ID == main.ID);
            }
            else
            {
*/

                if (cmsPage == null && CatalogBrowser.Products.ContainsValue(url.ToLower()))
                {
                    cmsPage = db.CMSPages.FirstOrDefault(x => x.ID == main.ID);
                }

/*
            }
*/

            if (cmsPage == null || (slashedParams.Any() && slashedParams[0] == "404") || (CatalogBrowser.ParentPage != null && CatalogBrowser.ParentPage.ID == cmsPage.ID && browser.IsNotFound(cmsPage)))
            {
                info = new CommonPageInfo
                    {
                        URL = "404",
                        Action = "NotFound",
                        Controller = "TextPage",
                        CurrentPage = main.LoadLangValues(),
                        CurrentLang = AccessHelper.CurrentLang,
                        Routes = ((MvcHandler)HttpContext.Current.Handler).RequestContext.RouteData.Values,
                        StatusCode = 404,
                        Title = SiteSetting.Get<string>("CommonTitle") + "Страница не найдена"
                    };
            }
            else
            {
                info = new CommonPageInfo()
                {
                    ID = cmsPage.ID,
                    URL = url,
                    CurrentPage = cmsPage.LoadLangValues(),
                    Routes = routes,
                    CurrentLang = AccessHelper.CurrentLang,
                    StatusCode = 200
                };
                info.CurrentPage.Title = SiteSetting.Get<string>("CommonTitle") +
                                         (cmsPage.Title.IsNullOrEmpty() ? cmsPage.PageName : cmsPage.Title);
                if (browser.IsCategory(cmsPage))
                {
                    if (browser.CurrentCategory.ID == 1)
                    {
                        info.CurrentPage.Title = SiteSetting.Get<string>("CommonTitle") +
                                                 cmsPage.Title;
                        info.CurrentPage.Keywords = cmsPage.Keywords;
                        info.CurrentPage.Description = cmsPage.Description;
                    }
                    else
                    {
                        info.CurrentPage.Title = SiteSetting.Get<string>("CommonTitle") +
                                                 browser.CurrentCategory.PageTitle;
                        info.CurrentPage.Keywords = browser.CurrentCategory.PageKeywords;
                        info.CurrentPage.Description = browser.CurrentCategory.PageDescription;
                    }
                }
                if (browser.IsProductPage)
                {
                    info.CurrentPage.Title = SiteSetting.Get<string>("CommonTitle") + browser.CurrentProduct.PageTitle;
                    info.CurrentPage.Keywords = browser.CurrentProduct.PageKeywords;
                    info.CurrentPage.Description = browser.CurrentProduct.PageDescription;
                }
            }

            info.Layout = "MainPage";
            if (cmsPage != null)
            {
                var currentRole = AccessHelper.CurrentRole;
                var rel = db.CMSPageRoleRels.FirstOrDefault(
                    x =>
                    x.PageID == cmsPage.ID &&
                    (!HttpContext.Current.User.Identity.IsAuthenticated || currentRole.IsNullOrEmpty()
                         ? !x.RoleID.HasValue
                         : x.Role.RoleName == currentRole));

                if (rel == null)
                {
                    info.Controller = "TextPage";
                    info.Action = "AccessDenied";
                    info.Layout = "MainPage";
                    info.StatusCode = 403;
                    info.Title = SiteSetting.Get<string>("CommonTitle") + "Доступ к этой странице ограничен";
                }

                
                info.Layout = cmsPage.PageType.CMSPageTemplate.Layout;
            }
            return info;
        }
        public bool HasModulsForCell(string cellName)
        {
            try
            {
                return CurrentPage.PageType.CMSPageCells.First(x => x.ColumnName == cellName).CMSPageCellViews.Any();
            }
            catch
            {
                return false;
            }
        }
        public int ID { get; set; }
        public string URL { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public CMSPage CurrentPage { get; set; }
        public RouteValueDictionary Routes { get; set; }
        public Language CurrentLang { get; set; }
        public string Layout { get; set; }
        public int StatusCode { get; set; }
        private string _keywords;
        public string Keywords
        {
            get
            {
                if (_keywords.IsNullOrEmpty())
                {
                    _keywords = CurrentPage == null ? "" : CurrentPage.Keywords;
                }
                return _keywords;
            }
            set { _keywords = value; }
        }

        public string FullPageName
        {
            get
            {
                if (CurrentPage == null)
                    return "Страница не найдена";
                return CurrentPage.FullName.IsFilled() ? CurrentPage.FullName : CurrentPage.PageName;
            }
        }
        public string FullPageNameH2
        {
            get
            {
                if (CurrentPage == null)
                    return "Страница не найдена";
                return CurrentPage.FullNameH2.IsFilled() ? CurrentPage.FullNameH2 : CurrentPage.PageName;
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                if (_description.IsNullOrEmpty())
                {
                    _description = CurrentPage == null ? "" : CurrentPage.Description;
                }
                return _description;
            }
            set { _description = value; }
        }

        private string _title;
        public string Title
        {
            get
            {
                if (_title.IsNullOrEmpty())
                {
                    _title = CurrentPage == null ? "Страница не найдена" : CurrentPage.Title;
                }
                _title = SiteSetting.Get<string>("CommonTitle").Trim() + " " + _title;
                return _title.Trim();
            }
            set { _title = value; }
        }


        public string CurrentPageType
        {
            get
            {
                if (CurrentPage == null)
                    return "TextPage";
                return new DB().PageTypes.First(x => x.ID == CurrentPage.Type).TypeName;
            }

        }

        public bool IsProfilePage
        {
            get { return CurrentPageType == "ShopCart" || CurrentPageType == "Cabinet"; }
        }
    }

}