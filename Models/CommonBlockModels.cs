using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.Security;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;

namespace Smoking.Models
{

    public class SocialAuthResult
    {
        public string Message { get; set; }
        public bool HasResult { get; set; }
        public string PageURL { get; set; }
        public string Popup { get; set; }
        public bool NeedHide { get; set; }

        private static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
        }


        public static SocialAuthResult CheckAuth()
        {
            var from = HttpContext.Current.Request["from"];
            if (from.IsNullOrEmpty())
                return new SocialAuthResult();

            var target = String.Format("http://ulogin.ru/token.php?token={0}&host={1}", HttpContext.Current.Request["token"],
                                       HttpContext.Current.Request.Url.Host);

            var wc = new WebClient();
            byte[] data = null;
            try
            {
                data = wc.DownloadData(target);
            }
            catch (Exception exxxx)
            {
                return new SocialAuthResult()
                    {
                        HasResult = true,
                        Message = "Ошибка при установлении соединения с сервером авторизации",
                        PageURL = "",
                        Popup = "Login"
                    };
            }
            var js = Encoding.UTF8.GetString(data);
            js = DecodeEncodedNonAsciiCharacters(js);
            var serializer = new JavaScriptSerializer();
            var jsData = serializer.Deserialize<UserDataFromNetwork>(js);

            if (string.IsNullOrEmpty(jsData.email))
            {
                return new SocialAuthResult()
                    {
                        HasResult = true,
                        Message = "Для регистрации через соцсеть, в соцсети должен быть указан email",
                        PageURL = "",
                        Popup = "Login"
                    };
            }


            try
            {

                MembershipUser user = null;
                var exist = Membership.GetUserNameByEmail(jsData.email);
                if (!string.IsNullOrEmpty(exist))
                {
                    user = Membership.GetUser(exist);
                }

                //нет такого
                if (user == null)
                {
                    var pass = new Random(DateTime.Now.Millisecond).GeneratePassword(6);
                    //SiteExceptionLog.WriteToLog("Creating user = "+jsData.email);
                    user = Membership.CreateUser(jsData.email, pass, jsData.email);
                    Roles.AddUserToRole(user.UserName, "Client");

                    var profile = new UserProfile()
                    {
                        UserID = (Guid)user.ProviderUserKey,
                        FromIP = HttpContext.Current.Request.GetRequestIP().ToIPInt(),
                        RegDate = DateTime.Now,
                        Email = jsData.email,
                    };

                    profile.Name = jsData.first_name ?? "";
                    profile.Surname = jsData.last_name ?? "";
                    profile.Nick = jsData.nickname;
                    byte[] avatar;
                    try
                    {
                        avatar = wc.DownloadData(jsData.photo_big.IsNullOrEmpty() ? jsData.photo : jsData.photo_big);
                    }
                    catch
                    {
                        avatar = null;
                    }
                    profile.Avatar = avatar;

                    var db = new DB();
                    db.UserProfiles.InsertOnSubmit(profile);
                    db.SubmitChanges();


                    MailingList.Get("RegisterLetter")
                               .WithReplacement(
                                   new MailReplacement("{PASSWORD}", pass)
                        ).To(jsData.email).Send();

                    FormsAuthentication.SetAuthCookie(jsData.email, true);
                }
                //есть чувак
                else
                {
                    //мыло подтверждено и совпало, логин совпал
                    if ((/*jsData.verified_email == 1 && */jsData.email.ToLower() == user.Email.ToLower()))
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, true);
                    }
                    //редирект на страницу с формой, где выводим сообщение
                    else
                    {
                        return new SocialAuthResult()
                            {
                                HasResult = true,
                                Message = (jsData.nickname == user.UserName
                                                  ? "Пользователь с таким логином уже зарегистрирован. Пожалуйста, укажите другой логин."
                                                  : "Пользователь с таким Email уже зарегистрирован. Пожалуйста укажите другой Email"),
                                PageURL = "",
                                Popup = "Login"//?
                            };
                    }
                }

            }
            catch (Exception ex)
            {
                return new SocialAuthResult()
                    {
                        HasResult = true,
                        Message = ex.Message,
                        PageURL = "",
                        Popup = "Login"
                    };

            }
            return new SocialAuthResult()
                {
                    HasResult = true,
                    Message = "",
                    PageURL = CMSPage.Get(from).FullUrl
                };

        }
    }


    public delegate object ParentFunc(int id, DB db);
    public delegate List<object> ChildrenFunc(int parentId, DB db);
    public delegate string LinkFunc(object baseObject);
    public delegate object ConverterFunc(object baseObject);

    public class CatalogMenu
    {
        public SiteMapItem CatalogMap { get; set; }
        public CatalogBrowser CatalogBrowser { get; set; }

        private int? _openLevel;
        public int OpenLevel
        {
            get
            {
                if (!_openLevel.HasValue)
                {
                    int maxLevel = 0;
                    FindMaxLevel(CatalogMap.WrapWithEnumerable(), 0, ref maxLevel);
                    _openLevel = maxLevel;
                }
                return _openLevel.Value;
            }
        }

        private void FindMaxLevel(IEnumerable<SiteMapItem> list, int level, ref int maxLevel)
        {
            foreach (var item in list)
            {
                if (item.IsActive && level > maxLevel)
                    maxLevel = level;
                FindMaxLevel(item.GetChildren<SiteMapItem>(), level + 1, ref maxLevel);
            }
        }


        public CatalogMenu(string url = "")
        {
            CatalogMap = SiteMapItem.GetCatalogRoot(null);
            CatalogBrowser = CatalogBrowser.Init(url);
        }

        private IEnumerable<SiteMapItem> _firstLevel;
        public IEnumerable<SiteMapItem> FirstLevel
        {
            get { return _firstLevel ?? (_firstLevel = CatalogMap.GetChildren<SiteMapItem>()); }
        }

        private bool? _menuHidden;
        public bool MenuHidden
        {
            get
            {
                if (!_menuHidden.HasValue)
                {
                    if (HttpContext.Current.Request.Cookies["MenuHidden"] != null &&
                        HttpContext.Current.Request.Cookies["MenuHidden"].Value.IsFilled())
                    {
                        _menuHidden = HttpContext.Current.Request.Cookies["MenuHidden"].Value == "1";
                    }
                    else
                        _menuHidden = false;
                }
                return _menuHidden.Value;
            }
        }
    }


    public class SiteMapItem
    {

        private ParentFunc _parentDelegate;
        private ChildrenFunc _childrenDelegate;
        private ConverterFunc _converterDelegate;
        private DB _db;

        public static SiteMapItem GetCatalogRoot(int? root)
        {
/*
            if (HttpContext.Current.Cache["CatalogRoot"] != null)
                return HttpContext.Current.Cache["CatalogRoot"] as SiteMapItem;
*/
            

            var db = new DB();
            var dlo = new DataLoadOptions();
            dlo.LoadWith<StoreCategory>(x => x.StoreProductsToCategories);
            dlo.LoadWith<StoreProduct>(x => x.StoreProductsToCategories);
            //dlo.LoadWith<StoreCategory>(x=> x.StoreCategories);
            db.LoadOptions = dlo;

            var obj = db.StoreCategories.FirstOrDefault(x => x.ParentID == root);
            if (!root.HasValue)
                obj = db.StoreCategories.FirstOrDefault(x => x.ParentID == null);
            if (obj == null) return null;
            var cr =
                obj.WrapWithEnumerable()
                   .Select(
                       x =>
                       new SiteMapItem(
                           (id, dbx) => CatalogBrowser.CategoriesList[CatalogBrowser.Categories[id]],
                           (id, dbx) => CatalogBrowser.CategoriesList.Values.Where(v => v.ParentID == id).OrderBy(z => z.OrderNum).Select(c => (object)c).ToList(),
                           baseObject =>
                           {
                               var cat = (StoreCategory)baseObject;
                               return new SiteMapItem()
                                   {
                                       BaseObject = cat,
                                       ParentID = cat.ParentID,
                                       ID = cat.ID,
                                       Name = cat.Name,
                                       Link = cat.FullUrl,
                                       OrderNum = cat.OrderNum,
                                       IsActive = CatalogBrowser.Init().ParentIds.Contains(cat.ID),
                                       ShowInMenu = cat.ShowInMenu,
                                       Description = cat.MenuUpperText,
                                       Title = cat.Description.ClearHTML(),
                                       Image = cat.MenuImageUrl
                                       
                                       /*(cat.StoreProductsToCategories.OrderBy(c => c.OrderNum).LastOrDefault() ??
                                        new StoreProductsToCategory() { OrderNum = 99999 }).OrderNum*/
                                   };
                           }, db)
                           {
                               Name = x.Name,
                               ID = x.ID,
                               ParentID = x.ParentID,
                               BaseObject = x,
                               IsActive = CatalogBrowser.Init().ParentIds.Contains(x.ID),
                               ShowInMenu = x.ShowInMenu,
                               Link = x.FullUrl,
                               OrderNum = x.OrderNum,
                               Description = x.MenuUpperText,
                               Title = x.Description.ClearHTML(),
                               Image = x.MenuImageUrl
                           }).First();

/*
            HttpContext.Current.Cache.Add("CatalogRoot", cr, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration,
                CacheItemPriority.High, null);
*/

            return cr;
        }


        public SiteMapItem()
        {

        }
        public SiteMapItem(SiteMapItem parent)
        {
            _childrenDelegate = parent._childrenDelegate;
            _parentDelegate = parent._parentDelegate;
            _converterDelegate = parent._converterDelegate;
            _db = parent._db;

        }
        public SiteMapItem(ParentFunc parentFunc, ChildrenFunc childrenFunc, ConverterFunc converterFunc, DB db)
        {
            _childrenDelegate = childrenFunc;
            _parentDelegate = parentFunc;
            _converterDelegate = converterFunc;
            _db = db;
        }
        public object BaseObject { get; set; }
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int OrderNum { get; set; }
        public bool IsActive { get; set; }
        public bool ShowInMenu { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public T GetParent<T>()
        {
            if (_parentDelegate == null)
                return default(T);

            if (typeof(T) == typeof(SiteMapItem))
            {
                var con = _converterDelegate(_parentDelegate(ID, _db)) as SiteMapItem;
                if (con != null)
                {
                    con._converterDelegate = _converterDelegate;
                    con._childrenDelegate = _childrenDelegate;
                    con._parentDelegate = _parentDelegate;
                    con._db = _db;
                }
                return (T)(object)con;
            }

            return (T)_parentDelegate(ID, _db);
        }
        public List<T> GetChildren<T>()
        {
            if (_childrenDelegate == null)
                return new List<T>();

            if (typeof(T) == typeof(SiteMapItem))
            {
                var cons = _childrenDelegate(ID, _db).Select(x => (SiteMapItem)_converterDelegate(x)).ToList();
                foreach (var con in cons)
                {
                    con._converterDelegate = _converterDelegate;
                    con._childrenDelegate = _childrenDelegate;
                    con._parentDelegate = _parentDelegate;
                    con._db = _db;

                }
                return cons.Select(x => (T)(object)x).ToList();
            }
            return _childrenDelegate(ID, _db).Select(x => (T)x).ToList();
        }

    }

    public class BreadCrumbModel
    {
        private CatalogBrowser _catalogBrowser;
        public CatalogBrowser CatalogBrowser
        {
            get { return _catalogBrowser ?? (_catalogBrowser = CatalogBrowser.Init()); }
        }
        public string Delimiter { get; set; }
        public string MainPageTemplate { get; set; }
        public bool ShowMain { get; set; }


        public string Html
        {
            get
            {

                CMSPage page = null;
                var info = AccessHelper.CurrentPageInfo;
                page = info.CurrentPage;//.ID == CatalogBrowser.ParentPage.ID ? info.CurrentPage.Parent : info.CurrentPage;

                if (page == null || (!ShowMain && info.CurrentPageType == "MainPage")) return "";
                var path = new List<string>();

                while (true)
                {

                    path.Add(string.Format("<a href='{0}'>{1}</a>", page.FullUrl, page.PageName));
                    if (!page.ParentID.HasValue)
                        break;
                    page = CMSPage.Get(page.ParentID.Value);
                    if (page == null || page.Type == 1) break;
                }

                path.Reverse();

                var catPath = new List<string>();

                if (info.CurrentPage.ID == CatalogBrowser.ParentPage.ID)
                {
                    if (!CatalogBrowser.NotFound)
                    {
                        var category = CatalogBrowser.CurrentCategory;
                        while (category != null)
                        {
                            if (category.ID > 1)
                            {
                                if (category.ID != CatalogBrowser.CurrentCategory.ID)
                                {
                                    catPath.Add(string.Format("<a href='{0}'>{1}</a>", category.FullUrl,
                                                              category.Name));
                                }
                                else
                                {
                                    if (!CatalogBrowser.IsProductPage)
                                    {
                                        catPath.Add(string.Format("<span class=\"act\">{0}</span>", category.Name));
                                    }
                                    else
                                    {
                                        catPath.Add(string.Format("<a href='{0}'>{1}</a>", category.FullUrl,
                                                                  category.Name));
                                        
                                    }

                                }
                            }
                            category = category.Parent;
                        }

                        catPath.Reverse();
                        if (CatalogBrowser.IsProductPage)
                        {
                            catPath.Add(string.Format("<span class=\"act\">{0}</span>", CatalogBrowser.CurrentProduct.Name));
                        }
                        path.AddRange(catPath);

                    }
                }

                return path.JoinToString(Delimiter ?? "<span class=\"rarr\">{0}</span>".FormatWith(SiteSetting.Get<string>("BreadCrumbDelimeter")));
            }
        }

        private List<BreadCrumbItem> _items;
        public List<BreadCrumbItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<BreadCrumbItem>();
                    CMSPage page = null;
                    var info = AccessHelper.CurrentPageInfo;
                    page = info.CurrentPage;
                        //.ID == CatalogBrowser.ParentPage.ID ? info.CurrentPage.Parent : info.CurrentPage;

                    if (page == null || (/*!ShowMain &&*/ info.CurrentPageType == "MainPage")) return _items;
                    /*var path = new List<string>();*/

                    while (true)
                    {
                        var i = new BreadCrumbItem() {Url = page.FullUrl, Name = page.PageName};
                        _items.Add(i);
                        var children = CMSPage.FullPageTable.Where(x => x.ParentID == page.ID && !x.Deleted).ToList();
                        if (children.Any())
                        {
                            i.Children = new List<BreadCrumbItem>();
                            foreach (var sp in children)
                            {
                                i.Children.Add(new BreadCrumbItem(){Url = sp.FullUrl, Name = sp.PageName});
                            }
                        }

                        if (!page.ParentID.HasValue)
                            break;
                        page = CMSPage.Get(page.ParentID.Value);
                        if (page == null || page.Type == 1) break;
                    }

                    _items.Reverse();

                    var catPath = new List<BreadCrumbItem>();

                    if (info.CurrentPage.ID == CatalogBrowser.ParentPage.ID)
                    {
                        if (!CatalogBrowser.NotFound)
                        {
                            var category = CatalogBrowser.CurrentCategory;
                            while (category != null)
                            {
                                if (category.ID > 1)
                                {
                                    if (category.ID != CatalogBrowser.CurrentCategory.ID)
                                    {
                                        var i = new BreadCrumbItem() {Name = category.Name, Url = category.FullUrl};
                                        catPath.Add(i); 
                                        if (category.Children != null && category.Children.Any())
                                        {
                                            i.Children = new List<BreadCrumbItem>();
                                            foreach (var child in category.Children.Where(x=> !x.Deleted /*&& x.ShowInMenu*/ && x.ShowInBreadcrumb))
                                            {
                                                i.Children.Add(new BreadCrumbItem(){Name = child.Name, Url = child.FullUrl});
                                            }
                                        }
                                        else if (category.StoreProductsToCategories.Any())
                                        {
                                            i.Children = new List<BreadCrumbItem>();
                                            foreach (var child in category.StoreProductsToCategories.Where(x=> !x.StoreProduct.Deleted))
                                            {
                                                i.Children.Add(new BreadCrumbItem() { Name = child.StoreProduct.ShortName, Url = child.StoreProduct.FullUrl });
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (!CatalogBrowser.IsProductPage)
                                        {
                                            var i = new BreadCrumbItem() { Name = category.Name, Url = category.FullUrl, IsSpan = true};
                                            catPath.Add(i);
                                            if (category.Children != null && category.Children.Any())
                                            {
                                                i.Children = new List<BreadCrumbItem>();
                                                foreach (var child in category.Children.Where(x=> !x.Deleted && /*x.ShowInMenu && */x.ShowInBreadcrumb))
                                                {
                                                    i.Children.Add(new BreadCrumbItem() { Name = child.Name, Url = child.FullUrl });
                                                }
                                            }
                                            else if (category.StoreProductsToCategories.Any())
                                            {
                                                i.Children = new List<BreadCrumbItem>();
                                                foreach (var child in category.StoreProductsToCategories.Where(x=> !x.StoreProduct.Deleted))
                                                {
                                                    i.Children.Add(new BreadCrumbItem() { Name = child.StoreProduct.ShortName, Url = child.StoreProduct.FullUrl });
                                                }
                                            }

                                            /*catPath.Add(new BreadCrumbItem(){Name = category.Name, Url = category.FullUrl, IsSpan = true});*/
                                        }
                                        else
                                        {
                                            var i = new BreadCrumbItem() { Name = category.Name, Url = category.FullUrl };
                                            catPath.Add(i);
                                            if (category.Children != null && category.Children.Any())
                                            {
                                                i.Children = new List<BreadCrumbItem>();
                                                foreach (var child in category.Children.Where(x=> !x.Deleted/* && x.ShowInMenu*/ && x.ShowInBreadcrumb))
                                                {
                                                    i.Children.Add(new BreadCrumbItem() { Name = child.Name, Url = child.FullUrl });
                                                }
                                            }

                                            else if (category.StoreProductsToCategories.Any())
                                            {
                                                i.Children = new List<BreadCrumbItem>();
                                                foreach (var child in category.StoreProductsToCategories.Where(x=> !x.StoreProduct.Deleted))
                                                {
                                                    i.Children.Add(new BreadCrumbItem() { Name = child.StoreProduct.ShortName, Url = child.StoreProduct.FullUrl });
                                                }
                                            }



                                        }

                                    }
                                }
                                category = category.Parent;
                            }

                            catPath.Reverse();
                            if (CatalogBrowser.IsProductPage)
                            {




                                var p = new BreadCrumbItem()
                                {
                                    Name = CatalogBrowser.CurrentProduct.ShortName,
                                    Url = CatalogBrowser.CurrentProduct.FullUrl,
                                    IsSpan = true
                                };
                                catPath.Add(p);

/*
                                var db = new DB();

                                var prods =
                                    db.StoreProductsToCategories.Where(x => !x.StoreProduct.Deleted && x.StoreProduct.Name!=null && x.StoreProduct.Name.Length>0)
                                        .OrderBy(x => x.OrderNum);

                                if (prods.Any())
                                {
                                    p.Children = new List<BreadCrumbItem>();
                                    foreach (var child in prods)
                                    {
                                        p.Children.Add(new BreadCrumbItem() { Name = child.StoreProduct.Name, Url = child.StoreProduct.FullUrl });
                                    }
                                }
*/


                            }
                            _items.AddRange(catPath);

                        }
                    }
                }
                return _items;
            }
        }
    }

    public class BreadCrumbItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsSpan { get; set; }
        public List<BreadCrumbItem> Children { get; set; }
    }

    public class HeaderViewModel
    {
        protected DB db = new DB();
        public List<CMSPage> MainMenu { get; private set; }
        public string HeaderPhones { get; private set; }
        public string FooterPhones { get; private set; }
        public HeaderViewModel()
        {

            var main = CMSPage.GetByType("MainPage").ToList();
            if (main.Any())
            {
                MainMenu = main.Concat(CMSPage.FullPageTable.Where(x => x.ViewMenu && x.Visible && !x.Deleted && x.ParentID == main.First().ID)
                                              .OrderBy(
                                                  x => x.OrderNum).ToList()).ToList();
            }
            HeaderPhones = SiteSetting.Get<string>("HeaderPhone");
            FooterPhones = SiteSetting.Get<string>("FooterAdress");
        }



    }



    public class CommonSearch
    {
        public CommonSearch()
            : this("search_field_text")
        {
            
        }

        public bool NeedShowSection { get; set; }
        public string SearchQuery { get; set; }
        public bool OnlyInCurrentSection { get; set; }
        public int? SectionID { get; set; }
        public string CSS { get; set; }

        public CommonSearch(string css = "")
        {
            if (css.IsNullOrEmpty())
                CSS = "search_field_text";
            else CSS = css;
            var browser = CatalogBrowser.Init();
            NeedShowSection = (browser.IsCategoryPage || browser.IsProductPage || browser.IsMainPage) && browser.CurrentCategory.ID > 1;
            var decode = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["word"]);
            if (decode != null)
                SearchQuery = decode.Trim();

            SectionID = NeedShowSection ? browser.CurrentCategory.ID : 0;
            OnlyInCurrentSection = SectionID.HasValue;
        }

        public override string ToString()
        {
            var info = AccessHelper.CurrentPageInfo;
            string targetPage = "/catalog";
            if (info.CurrentPageType == "Catalog")
                targetPage = info.CurrentPage.FullUrl;
            return targetPage +
                   "?section={0}&search={1}".FormatWith(OnlyInCurrentSection ? info.CurrentPage.ID : 0, Microsoft.JScript.GlobalObject.escape(SearchQuery));
        }
    }


  
}