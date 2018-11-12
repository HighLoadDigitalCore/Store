using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Smoking.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Smoking.Extensions.Helpers
{

    public class CacheRepository
    {
        public bool ContainsKey(string key)
        {
            return HttpContext.Current.Items[key] != null;
        }

        public object this[string key]
        {
            get { return HttpContext.Current.Items[key]; }
            set
            {
                HttpContext.Current.Items.Add(key, value);
/*
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, null);
*/
            }
        }

        public void Add(string key, object value)
        {
            if (!ContainsKey(key))
            {
                HttpContext.Current.Items.Add(key, value);
                /*HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, null);*/
            }
        }

        public void Remove(string key)
        {
            HttpContext.Current.Items.Remove(key);
            /*HttpContext.Current.Cache.Remove(key);*/
        }

        public void Clear()
        {
            
        }
    }


    public static class AccessHelper
    {
        public static bool IsCatalogPage 
        {
            get { return CatalogBrowser.IsCategoryPage || CatalogBrowser.IsProductPage; }
        }
        public static CatalogBrowser CatalogBrowser
        {
            get { return CatalogBrowser.Init(); }
        }

        public static CacheRepository Repository { get; set; }
        public static void AddToRepository(string key, object value)
        {
            if (Repository.ContainsKey(key))
                Repository[key] = value;
            else Repository.Add(key, value);
        }
        static AccessHelper()
        {
            Repository = new CacheRepository();
        }

        private static List<string> _shortLangList;
        public static List<string> ShortLangList
        {
            get { return _shortLangList ?? (_shortLangList = new DB().Languages.Select(x => x.ShortName).ToList()); }
        }

        public static string CurrentClientLangShortName
        {
            get
            {
                var slashed = HttpContext.Current.Request.RawUrl.Split<string>("/").ToList();
                var intersect = slashed.Intersect(ShortLangList).ToList();
                return intersect.Any() ? intersect.First() : new DB().Languages.First(x => x.ByDef).ShortName;
            }
        }

        public static Language CurrentLang
        {
            get
            {
                bool isMaster = IsMasterPage;
                var slashed = HttpContext.Current.Request.RawUrl.Split<string>("/").ToList();
                if (isMaster)
                {
                    var cook = HttpContext.Current.Request.Cookies["MasterLang"];
                    if (cook != null && Repository.ContainsKey("CurrentLang") &&
                        ((Language)Repository["CurrentLang"]).ShortName != cook.Value)
                    {
                        Repository.Remove("CurrentLang");
                    }
                }
                else if (Repository.ContainsKey("CurrentLang"))
                {
                    var rpl = (Language)Repository["CurrentLang"];
                    if (rpl.ShortName != CurrentClientLangShortName)
                    {
                        Repository.Remove("CurrentLang");
                        CMSPage.FullPageTable = null;
                    }
                }

                if (Repository.ContainsKey("CurrentLang"))
                    return (Language)Repository["CurrentLang"];

                Language lang = null;
                var db = new DB();
                if (slashed.Any())
                {
                    lang =
                        db.Languages.FirstOrDefault(
                            x =>
                            x.Enabled &&
                            (x.ShortName == (isMaster && slashed.Skip(1).Any() ? slashed.Skip(1).First() : slashed.First())));
                }
                if (lang == null)
                {
                    lang = AccessHelper.DefLang;
                }
                if (Repository.ContainsKey("CurrentLang"))
                    Repository["CurrentLang"] = lang;
                else Repository.Add("CurrentLang", lang);
                if (Repository.ContainsKey("CommonInfo"))
                    ((CommonPageInfo)Repository["CommonInfo"]).CurrentLang = lang;
                return lang;
            }
        }

        private static Language _defLang;
        public static Language DefLang
        {
            get { return _defLang ?? (_defLang = new DB().Languages.FirstOrDefault(x => x.ByDef)); }
        }

        public static CommonPageInfo LoadFromQuery(string query)
        {
            var info = CommonPageInfo.InitFromQueryParams(query);
            if (Repository.ContainsKey("CommonInfo"))
                Repository["CommonInfo"] = info;
            else Repository.Add("CommonInfo", info);
            return info;
        }


        public static CommonPageInfo CurrentPageInfo
        {
            get
            {
                if (Repository.ContainsKey("CommonInfo") && Repository["CommonInfo"] is CommonPageInfo)
                    return (CommonPageInfo)Repository["CommonInfo"];
                var info = CommonPageInfo.InitFromQueryParams();
                if (Repository.ContainsKey("CommonInfo"))
                    Repository["CommonInfo"] = info;
                else Repository.Add("CommonInfo", info);
                return info;
            }
            set
            {
                if (value == null)
                    Repository.Remove("CommonInfo");
                else
                {
                    if (Repository.ContainsKey("CommonInfo"))
                        Repository["CommonInfo"] = value;
                    else Repository.Add("CommonInfo", value);
                }
            }
        }

        private static string _siteUrl;
        public static string SiteUrl
        {
            get
            {
                if (_siteUrl.IsNullOrEmpty())
                    try
                    {
                        _siteUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;
                    }
                    catch
                    {
                        _siteUrl = "http://Smoking.ru";
                    }
                return _siteUrl;
            }
        }
        public static string SiteName
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.Url.Host;
                }
                catch
                {
                    return "Smoking.ru";
                }
            }
        }
        public static string getStartUserController(string userName)
        {
            return MasterRoles.Any(role => Roles.IsUserInRole(userName, role)) ? "MasterHome" : "Home";
        }

        public static bool HasAccess(string controller)
        {
            return true;
            /*
                        string[] userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
                        if (userRoles.Contains<string>("GrandAdmin") || userRoles.Contains<string>("Director"))
                        {
                            return true;
                        }
                        DB db = new DB();
                        return (from x in db.eControllers.First<eController>(x => (x.Controller == controller)).xRolesInControllers select x.cRole.RoleName).ToList<string>().Intersect<string>(userRoles).Any<string>();
            */
        }

        public static string CurrentRole
        {
            get
            {
                return Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).FirstOrDefault();
            }
        }

        public static Guid CurrentRoleID
        {
            get
            {
                var db = new DB();
                return
                    db.Roles.First(
                        x =>
                        (x.RoleName == Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).First()))
                      .RoleId;
            }
        }

        public static Guid CurrentUserKey
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    return (Guid)Membership.GetUser().ProviderUserKey;
                return new Guid();
            }
        }

        public static bool IsMasterPage
        {
            get { return HttpContext.Current.Request.RawUrl.Contains("/Master"); }
        }

        public static bool IsMaster
        {
            get { return MasterRoles.Any(Roles.IsUserInRole); }
        }
        private static List<string> _masterRoles;
        public static List<string> MasterRoles
        {
            get { return _masterRoles ?? (_masterRoles = new List<string> { "Administrator" }); }
        }

        public static bool IsAuthClient
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated && Roles.IsUserInRole("Client"); }
        }

        public static bool IsMainPage
        {
            get { return CurrentPageInfo.CurrentPageType == "Catalog"; }
        }

        public static string BodyContentClass
        {
            get
            {
                if (CurrentPageInfo.CurrentPageType == "Lenta")
                    return "content-grey";
                return "";
            }
        }


        public static MapFilterData MapFilter
        {
            get
            {
                return new MapFilterData(HttpContext.Current.Request.Params);
            }
        }

        public static UserProfile CurrentUserProfile
        {
            get
            {
                var db = new DB();
                return (db.UserProfiles.FirstOrDefault(x => x.UserID == CurrentUserKey) ?? new UserProfile());
            }
        }

        public static bool IsAuth
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }

        public static Guid ProfileUID
        {
            get { return HttpContext.Current.Request["uid"].ToGuid() ?? CurrentUserKey; }
        }

        public static Dictionary<string, string> QueryDict
        {
            get {
                var dict = new Dictionary<string, string>();
                var q = HttpContext.Current.Request.RawUrl.Split<string>("?").ToList();
                if (q.Count() > 1)
                {
                    dict =
                        q.ElementAt(1)
                            .Split<string>("&")
                            .Select(x => x.Split<string>("="))
                            .ToDictionary(x => x.ElementAt(0), y => y.Count() > 1 ? y.ElementAt(1) : "");
                }
                var spl = q.ElementAt(0).Split<string>("/").ToList();
                if (spl.Contains("Master"))
                {
                    spl = spl.Skip(4).ToList();
                }
                if (spl.Count%2 == 0)
                {
                    for (int i = 0; i < spl.Count; i+=2)
                    {
                        dict.Add(spl[i].ToLower(), spl[i + 1]);
                        
                    }
                }
                return dict;
            }
        }

        public static string BaseURL
        {
            get
            {
                if (CatalogBrowser.ParentPage.ID == CurrentPageInfo.CurrentPage.ID)
                {
                    var browser = CatalogBrowser.Init();
                    if (browser.IsCategoryPage)
                        return browser.CurrentCategory.ID == 0
                                   ? CatalogBrowser.BaseURL
                                   : browser.CurrentCategory.FullUrl;
                    if (browser.IsProductPage)
                        return browser.CurrentProduct.FullUrl;
                }
                else
                {
                    return CurrentPageInfo.CurrentPage.FullUrl;
                }
                return "/";
            }
        }

        public static MasterPanelData MasterPanel
        {
            get { return new MasterPanelData();  }
        }

        public static string Pluralize(int count, string[] post)
        {
            var rest = count % 10;
            if (rest == 0 || rest >= 5)
                return post[0];
            if (rest == 1)
                return post[1];
            if (rest > 1 && rest < 5)
                return post[2];
            return "";
        }
    }

    public class MasterPanelData
    {
        public string DefaultController = "Home";
        public string DefaultAction = "Index";
        public string DefaultLang = "ru";
        public string BodyCSS
        {
            get
            {
                if (IsSiteMap)
                    return "body-dashboard";
                return "body-inner";
            }
        }
        public bool IsSiteMap
        {
            get { return AccessHelper.CurrentPageInfo.Controller == "Pages"; }
        }

        private int? _chatUnreadCount;
        public int ChatUnreadCount
        {
            get
            {
                if (!_chatUnreadCount.HasValue)
                {
                    _chatUnreadCount = new xChatDB().ChatMessages.Count(x => !x.IsRead);
                    
                }
                return _chatUnreadCount.Value;
            }
        }
    }
}

