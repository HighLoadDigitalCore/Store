using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using NPOI.HSSF.UserModel;
using Smoking.Extensions;
using Smoking.Models;
namespace Smoking.Models
{

    public class JFieldEntry
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public partial class ShopCartItem
    {
        public decimal Sum { get { return Count * PriceForOffer; } }

        public decimal PriceForOffer
        {
            get { return StoreProduct.PriceModule.ShopCartPrice; }
        }
    }

    public class OrderStep
    {
        public string CSS { get; set; }
        public int Arg { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

    }

    public class OrderSteps : List<OrderStep>
    {

        public static List<string> Menus = new List<string>
            {
                "РЕДАКТИРОВАНИЕ КОРЗИНЫ",
                "ВЫБОР АДРЕСА ДОСТАВКИ",
                "ПОДТВЕРЖДЕНИЕ ЗАКАЗА",
                "ВЫБОР СПОСОБА ОПЛАТЫ",
                "БЛАГОДАРИМ ЗА ПОКУПКУ!"
            };
        public static List<string> Actions = new List<string>
            {
                "CartEditor",
                "CartAdress",
                "CartConfirm",
                "CartPayment",
                "CartFinal"
            };
        public OrderSteps(int step)
            : this()
        {
            CurrentStep = step;
        }
        public OrderSteps()
        {

            AddRange(
                Menus.Select(
                    (x, i) =>
                    new OrderStep
                    {
                        Name = x,
                        Arg = i,
                        Url =
                            CMSPage.GetPageLinkByType("ShopCart") + "?step=" + Actions[i].Replace("Cart", "").ToLower()
                    }));
        }

        private int? _currentStep;
        public int CurrentStep
        {
            get
            {
                if (!_currentStep.HasValue)
                {
                    var current = HttpContext.Current.Request["step"].IsNullOrEmpty()
                                      ? 0
                                      : Actions.IndexOf("Cart" + HttpContext.Current.Request["step"].ToNiceForm());
                    if (current < 0)
                        current = 0;
                    _currentStep = current;
                }
                return _currentStep.Value;
            }
            set { _currentStep = value; }
        }

        public string NextStepUrl
        {
            get
            {
                return CMSPage.GetPageLinkByType("ShopCart") + "?step=" +
                       Actions[CurrentStep + 1].Replace("Cart", "").ToLower();
            }
        }


        public string CurrentStepUrl
        {
            get
            {
                return CMSPage.GetPageLinkByType("ShopCart") + "?step=" +
                       Actions[CurrentStep].Replace("Cart", "").ToLower();
            }
        }



        public string CurrentStepAction
        {
            get { return Actions[CurrentStep]; }
        }

        public string PrevStepUrl
        {
            get
            {
                return CMSPage.GetPageLinkByType("ShopCart") + "?step=" +
                    Actions[(CurrentStep - 1 < 0 ? 0 : CurrentStep - 1)].Replace("Cart", "").ToLower();

            }
        }
    }

    public partial class OrderAdress
    {
        private string[] props = new[] { "Town", "Street", "House", "Building", "BuildingPart", "Doorway", "Floor", "Flat" };
        private string[] names = new[] { "", "улица", "д.", "корп.", "стр.", "подъезд", "этаж", "кв." };

        public override string ToString()
        {
            return props.Select((x, i) => new { Val = this.GetPropertyValue(x), Index = i })
                        .Where(x => x.Val != null && x.Val.ToString().IsFilled())
                        .Select(x => string.Format("{0} {1}", names[x.Index], x.Val)).JoinToString(", ");
        }
    }


    public partial class ShopCart
    {

        private List<OrderAdress> _adresses;
        public List<OrderAdress> Adresses
        {
            get
            {
                return _adresses ??
                       (_adresses =
                        new DB().OrderAdresses.Where(x => x.UserID == HttpContext.Current.GetCurrentUserUID()).ToList());
            }
        }

        public OrderAdress SelectedAddressEntry
        {
            get
            {
                if (LastAddressID == 0)
                    return new OrderAdress() { Comment = "" };
                return Adresses.First(x => x.ID == LastAddressID);
            }
        }
        public string SelectedAddress
        {
            get
            {
                if (LastAddressID == 0)
                {
                    return "Самовывоз";
                }
                else
                {
                    var addr = Adresses.First(x => x.ID == LastAddressID);
                    return addr.ToString();
                }
            }
        }

        private int? _lastAddressID;
        public int LastAddressID
        {
            get
            {
                if (!_lastAddressID.HasValue)
                {
                    var fromStore = GetField<int>("LastAddressID");
                    if (fromStore > 0)
                    {
                        _lastAddressID = Adresses.Any(z => z.ID == fromStore)
                                             ? fromStore
                                             : 0;
                    }
                    else
                    {
                        if (HttpContext.Current.GetCurrentUser() == null)
                        {
                            _lastAddressID = 0;
                        }
                        else
                        {
                            var orders = HttpContext.Current.GetCurrentUser()
                                .Orders.OrderByDescending(x => x.CreateDate);
                            if (orders.Any())
                            {
                                _lastAddressID = Adresses.Any(z => z.ID == orders.First().OrderDetail.AddressID)
                                    ? orders.First().OrderDetail.AddressID
                                    : 0;
                            }
                            else _lastAddressID = 0;
                        }
                    }
                }
                return _lastAddressID.Value;
            }
        }

        public decimal SummaryWeight
        {
            get
            {
                return
                    ActiveProducts.Where(x => x.StoreProduct != null)
                               .Where(x => x.StoreProduct.Weight.HasValue)
                               .Sum(x => x.StoreProduct.Weight ?? 0);
            }
        }


        private OrderSteps _steps;
        public OrderSteps Steps
        {
            get { return _steps ?? (_steps = new OrderSteps()); }
        }

        public static List<MembershipUser> FindAllUsersByEmail(string mail)
        {
            var users = new List<MembershipUser>();
            users.AddRange(Membership.FindUsersByName(mail).Cast<MembershipUser>());
            users.AddRange(Membership.FindUsersByEmail(mail).Cast<MembershipUser>());
            return users;
        }

        public string RegisterUser(string name, string pass)
        {
            try
            {
                var db = new DB();
                var newUser = Membership.CreateUser(name, pass, name);
                Roles.AddUserToRole(name, "Client");
                FormsAuthentication.SetAuthCookie(name, true);
                InitCart().SetField("AuthType", 2);
                var profile = new UserProfile()
                {
                    UserID = (Guid)newUser.ProviderUserKey,
                    Name = "",
                    FromIP = HttpContext.Current.Request.UserHostAddress.ToIPInt(),
                    RegDate = DateTime.Now
                };
                db.UserProfiles.InsertOnSubmit(profile);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }

        public static string AuthorizeUser(string name, string pass)
        {
            var res = "Указан неверный пароль или Email.";
            var users = FindAllUsersByEmail(name);
            foreach (var us in users)
            {
                var uname = us.UserName;
                if (us.GetPassword() != pass) continue;
                FormsAuthentication.SetAuthCookie(uname, true);
                MergeShopCarts(us);
                res = "";
                break;
            }
            return res;
        }

        public int MaxAvailableStepForAuth
        {
            get
            {
                if (SelectedProvider == null)
                    return 2;
                return SelectedPayment == null ? 3 : 4;
            }
        }

        public int MaxAvailableStep
        {
            get
            {
                return !HttpContext.Current.User.Identity.IsAuthenticated ? 1 : MaxAvailableStepForAuth;
            }
        }


        //private static DB db = new DB();
        private OrderDeliveryRegion _selectedRegion;
        public OrderDeliveryRegion SelectedRegion
        {
            get
            {
                if (_selectedRegion == null)
                {
                    var rid = GetField<int>("DeliveryRegion");
                    if (rid == 0)
                        _selectedRegion = new OrderDeliveryRegion();
                    else
                    {
                        _selectedRegion = SelectedProvider.OrderDeliveryRegions.FirstOrDefault(x => x.ID == rid) ??
                                          new OrderDeliveryRegion();
                    }
                }
                return _selectedRegion;
            }
        }


        private OrderDeliveryGroup _selectedGroup;
        public OrderDeliveryGroup SelectedGroup
        {
            get
            {
                if (_selectedGroup == null)
                {
                    var gid = GetField<int>("DeliveryGroup");
                    if (gid == 0)
                        _selectedGroup = new OrderDeliveryGroup();
                    else
                    {
                        var db = new DB();
                        _selectedGroup = db.OrderDeliveryGroups.FirstOrDefault(x => x.ID == gid) ??
                                         new OrderDeliveryGroup();
                    }
                }
                return _selectedGroup;
            }
        }

        private OrderDeliveryProvider _selectedProvider;
        public OrderDeliveryProvider SelectedProvider
        {
            get
            {
                if (_selectedProvider == null)
                {
                    var pid = GetField<int>("DeliveryProvider");
                    if (pid == 0)
                        _selectedProvider = new OrderDeliveryProvider();
                    else
                    {
                        _selectedProvider = SelectedGroup.OrderDeliveryProviders.FirstOrDefault(x => x.ID == pid) ??
                                            new OrderDeliveryProvider();
                    }
                }
                return _selectedProvider;
            }
        }

        private OrderPaymentProvider _selectedPayment;
        public OrderPaymentProvider SelectedPayment
        {
            get
            {
                if (_selectedPayment == null)
                {
                    var pid = GetField<int>("DeliveryPayment");
                    if (pid == 0)
                        _selectedPayment = new OrderPaymentProvider();
                    else
                    {
                        var rel = SelectedProvider.OrderPaymentDeliveryRels.FirstOrDefault(x => x.PaymentProviderID == pid);
                        _selectedPayment = rel == null ? new OrderPaymentProvider() : rel.OrderPaymentProvider;
                    }
                }
                return _selectedPayment;
            }
        }

        public string UserMail
        {
            get
            {
                var mail = GetField<string>("UserMail");
                if (mail.IsNullOrEmpty() && HttpContext.Current.User.Identity.IsAuthenticated)
                    return Membership.GetUser().Email;
                return mail;
            }
        }

        public string UserPass
        {
            get
            {
                var pass = GetField<string>("UserPass");

                if (pass.IsNullOrEmpty() && HttpContext.Current.User.Identity.IsAuthenticated)
                    return Membership.GetUser().GetPassword();
                return pass;
            }
        }

        public int AuthType
        {
            get
            {
                var at = GetField<int>("AuthType");
                if (at == 0 && HttpContext.Current.User.Identity.IsAuthenticated)
                    at = 2;
                if (at == 0)
                    at = 1;
                return at;
            }
        }

        public T GetField<T>(string filedName)
        {
            var db = new DB();
            var field = db.ShopCartFields.FirstOrDefault(x => x.Name == filedName && x.ShopCartID == ID);
            if (field == null) return default(T);
            return (T)Convert.ChangeType(field.Value, typeof(T), CultureInfo.InvariantCulture);
        }

        public void SetField(string fieldName, object value)
        {
            var val = (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
            if (val == null) return;
            var db = new DB();
            var exist = db.ShopCartFields.FirstOrDefault(x => x.ShopCartID == ID && x.Name == fieldName);


            if (exist == null)
            {
                exist = new ShopCartField() { Name = fieldName, ShopCartID = ID, Value = val };
                db.ShopCartFields.InsertOnSubmit(exist);
            }
            else
                exist.Value = val;
            db.SubmitChanges();
        }

        public void AddItem(int id, int count, bool spec = false)
        {
            try
            {
                var db = new DB();
                var item =
                    db.ShopCartItems.FirstOrDefault(x => x.ProductID == id && x.ShopCartID == ID && x.IsSpec == spec);
                if (item == null)
                {
                    item = new ShopCartItem
                        {
                            ShopCartID = ID,
                            IsDelayed = false,
                            Count = count,
                            ProductID = id,
                            IsSpec = spec
                        };
                    db.ShopCartItems.InsertOnSubmit(item);
                }
                else
                {
                    item.Count += count;
                    item.IsDelayed = false;
                }
                if (item.Count == 0)
                {
                    db.ShopCartItems.DeleteOnSubmit(item);
                }
                db.SubmitChanges();
                Reset();
            }
            catch (Exception) { }

        }   
        
        public void SetItem(int id, int count, bool spec = false)
        {
            try
            {
                var db = new DB();
                var item =
                    db.ShopCartItems.FirstOrDefault(x => x.ProductID == id && x.ShopCartID == ID && x.IsSpec == spec);
                if (item == null)
                {
                    item = new ShopCartItem
                        {
                            ShopCartID = ID,
                            IsDelayed = false,
                            Count = count,
                            ProductID = id,
                            IsSpec = spec
                        };
                    db.ShopCartItems.InsertOnSubmit(item);
                }
                else
                {
                    item.Count = count;
                    item.IsDelayed = false;
                }
                if (item.Count == 0)
                {
                    db.ShopCartItems.DeleteOnSubmit(item);
                }
                db.SubmitChanges();
                Reset();
            }
            catch (Exception) { }

        }

        public int AllTypesCount
        {
            get
            {
                return ShopCartItems.Any() ? ShopCartItems.Sum(x => x.Count) : 0;
            }
        }

        public decimal TotalWeight
        {
            get
            {
                return ShopCartItems.Any()
                           ? ShopCartItems.Where(x => !x.IsDelayed && x.StoreProduct.Weight.HasValue)
                                          .Sum(x => x.StoreProduct.Weight ?? 0)
                           : 0;
            }
        }

        public decimal TotalVolume
        {
            get
            {
                return ShopCartItems.Any()
                           ? ShopCartItems.Where(x => !x.IsDelayed && x.StoreProduct.Volume.HasValue)
                                          .Sum(x => (x.StoreProduct.Volume ?? 0) * x.Count)
                           : 0;
            }
        }

        public int TotalCount
        {
            get
            {
                return ShopCartItems.Any() ? ShopCartItems.Where(x => !x.IsDelayed).Sum(x => x.Count) : 0;
            }
        }

        public decimal TotalSum
        {
            get
            {
                if (!ShopCartItems.Any()) return 0;
                return
                    ShopCartItems.Where(x => !x.IsDelayed).Sum(x => x.Count * x.StoreProduct.PriceModule.ShopCartPrice);
            }
        }
        public decimal TotalSumWithoutDiscount
        {
            get
            {
                if (!ShopCartItems.Any()) return 0;
                return
                    ShopCartItems.Where(x => !x.IsDelayed).Sum(x => x.Count * x.StoreProduct.PriceModule.SitePriceWithoutDiscount);
            }
        }
        public decimal TotalSumForOrder
        {
            get
            {
                if (!ShopCartItems.Any()) return 0;
                return
                    ShopCartItems.Where(x => !x.IsDelayed).Sum(x => x.Count * x.StoreProduct.PriceModule.ShopCartPrice);
            }
        }

        public void Reset()
        {
            _selectedGroup = null;
            _selectedProvider = null;
            _selectedRegion = null;
            if (HttpContext.Current.Items.Contains("ShopCart"))
                HttpContext.Current.Items.Remove("ShopCart");

        }

        private static void MergeShopCarts(MembershipUser us)
        {

            var uid = (Guid)us.ProviderUserKey;
            Guid? cKey = null;
            if (HttpContext.Current.Request.Cookies["ck"] != null)
                cKey = new Guid(HttpContext.Current.Request.Cookies["ck"].Value);
            if (!cKey.HasValue) return;

            DB db = new DB();
            var carts = db.ShopCarts.Where(x => x.UserID == uid || x.TemporaryKey == cKey);
            if (carts.Count() <= 1) return;
            //добавляем в старую корзину новые записи (отложенные) и заменяем активные

            //новая карта
            var last = carts.OrderByDescending(x => x.LastRequested).First();


            var first = carts.OrderBy(x => x.LastRequested).First(); //старая карта

            //удаляем старые записи (активные)
            db.ShopCartItems.DeleteAllOnSubmit(db.ShopCartItems.Where(x => x.ShopCartID == first.ID && !x.IsDelayed));
            db.SubmitChanges();


            //переносим отложенные
            foreach (var item in last.ShopCartItems)
            {
                var exist =
                    db.ShopCartItems.FirstOrDefault(
                        x => x.ProductID == item.ProductID && x.ShopCartID == first.ID);

                if (exist != null)
                    exist.Count += item.Count;
                else
                    item.ShopCartID = first.ID;
            }
            first.LastRequested = DateTime.Now;
            db.SubmitChanges();


            var forDel = db.ShopCarts.Where(x => (x.UserID == uid || x.TemporaryKey == cKey) && x.ID != first.ID);
            db.ShopCarts.DeleteAllOnSubmit(forDel);
            db.SubmitChanges();


            var cook = HttpContext.Current.Request.Cookies.Get("ck");

            if (cook != null && cook.Value.IsGuid())
            {
                cook.Expires = DateTime.Now.AddYears(1);
            }
            else
            {
                cook = new HttpCookie("ck", first.TemporaryKey.ToString()) { Expires = DateTime.Now.AddYears(1) };
            }
            cook.Value = first.TemporaryKey.ToString();
            HttpContext.Current.Response.Cookies.Add(cook);
        }


        public ShopCart InitCart()
        {
            if (HttpContext.Current.Items.Contains("ShopCart") &&
                HttpContext.Current.Items["ShopCart"] is ShopCart)
                return HttpContext.Current.Items["ShopCart"] as ShopCart;
            else
            {
                var br = LoadFromDB();
                if (HttpContext.Current.Items.Contains("ShopCart"))
                    HttpContext.Current.Items.Remove("ShopCart");

                HttpContext.Current.Items.Add("ShopCart", br);
                return br;
            }

        }

        public ShopCart LoadFromDB()
        {
            DB db = new DB();
            ShopCart shopcart = null;
            string cKey = "";
            if (HttpContext.Current.Request.Cookies["ck"] != null)
                cKey = HttpContext.Current.Request.Cookies["ck"].Value;
            if (cKey.IsNullOrEmpty() || !cKey.IsGuid())
                cKey = Guid.NewGuid().ToString();
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                shopcart = db.ShopCarts.FirstOrDefault(x => x.UserID == (Guid)Membership.GetUser().ProviderUserKey);
            }
            if (shopcart == null || !HttpContext.Current.Request.IsAuthenticated)
            {
                shopcart = db.ShopCarts.FirstOrDefault(x => x.TemporaryKey == new Guid(cKey));
            }

            if (shopcart == null)
            {
                shopcart = new ShopCart();
                shopcart.LastRequested = DateTime.Now;
                if (HttpContext.Current.Request.IsAuthenticated)
                    shopcart.UserID = (Guid)Membership.GetUser().ProviderUserKey;
                shopcart.TemporaryKey = new Guid(cKey);
                db.ShopCarts.InsertOnSubmit(shopcart);
                db.SubmitChanges();
            }

            
            if (shopcart.UserID == null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var uid = (Guid)Membership.GetUser().ProviderUserKey;
                var exist = db.ShopCarts.FirstOrDefault(x => x.UserID == uid);
                if (exist == null)
                    shopcart.UserID = uid;
                db.SubmitChanges();
            }

            var old = db.ShopCarts.Where(x => x.LastRequested < DateTime.Now.AddDays(-10));
            if (old.Any())
            {
                db.ShopCarts.DeleteAllOnSubmit(old); //?
                db.SubmitChanges();
            }
            var cook = HttpContext.Current.Request.Cookies.Get("ck");

            if (cook != null && cook.Value.IsGuid())
                cook.Expires = DateTime.Now.AddYears(1);
            else
            {
                cook = new HttpCookie("ck", cKey);
                cook.Expires = DateTime.Now.AddYears(1);

            }
            cook.Value = shopcart.TemporaryKey.ToString();
            try
            {
                HttpContext.Current.Response.Cookies.Add(cook);
            }
            catch { }
            return shopcart;
        }

        public IEnumerable<ShopCartItem> ActiveProducts
        {
            get
            {
                var db = new DB();
                return db.ShopCartItems.Where(x => x.StoreProduct != null && x.StoreProduct.IsActive && !x.IsDelayed && x.ShopCartID == ID);
            }
        }
        public IEnumerable<ShopCartItem> DelayesProducts
        {
            get
            {
                var db = new DB();
                return db.ShopCartItems.Where(x => x.StoreProduct != null && x.StoreProduct.IsActive && x.IsDelayed && x.ShopCartID == ID);
            }
        }
        public IEnumerable<ShopCartItem> AbsentProducts
        {
            get
            {
                var db = new DB();
                return db.ShopCartItems.Where(x => x.StoreProduct != null && !x.StoreProduct.IsActive && x.ShopCartID == ID);
            }
        }


        public bool FirstTime
        {
            get { return Membership.GetUser().UserEntity().Profile.Name.IsNullOrEmpty(); }
        }

        private List<KeyValuePair<string, string>> _relations;


        protected List<KeyValuePair<string, string>> Relations
        {
            get
            {
                return _relations ?? (_relations = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("UserFirstName", "Name"),
                        new KeyValuePair<string, string>("UserSurname", "Surname"),
                        new KeyValuePair<string, string>("UserSecName", "Patrinomic"),
                        new KeyValuePair<string, string>("UserPhone", "HomePhone"),
                        new KeyValuePair<string, string>("UserMobile", "MobilePhone"),
                        new KeyValuePair<string, string>("DeliveryIndex", "ZipCode"),
                        new KeyValuePair<string, string>("DeliveryTown", "Town"),
                        new KeyValuePair<string, string>("DeliveryStreet", "Street"),
                        new KeyValuePair<string, string>("DeliveryHouse", "House"),
                        new KeyValuePair<string, string>("DeliveryFlat", "Flat"),
                        new KeyValuePair<string, string>("DeliverySection", "Building"),
                        new KeyValuePair<string, string>("DeliveryDoorway", "Doorway"),
                        new KeyValuePair<string, string>("DeliveryFloor", "Floor"),
                        new KeyValuePair<string, string>("OrgName", "OrgName"),
                        new KeyValuePair<string, string>("OrgINN", "OrgINN"),
                        new KeyValuePair<string, string>("OrgKPP", "OrgKPP"),
                        new KeyValuePair<string, string>("OrgKS", "OrgKS"),
                        new KeyValuePair<string, string>("OrgRS", "OrgRS"),
                        new KeyValuePair<string, string>("OrgBank", "OrgBank"),
                        new KeyValuePair<string, string>("OrgBik", "OrgBik"),
                        new KeyValuePair<string, string>("OrgJurAddr", "OrgJurAddr"),
                        new KeyValuePair<string, string>("OrgFactAddr", "OrgFactAddr"),
                        new KeyValuePair<string, string>("OrgDirector", "OrgDirector"),
                        new KeyValuePair<string, string>("OrgAccountant", "OrgAccountant")
                    });
            }
        }



        public bool IsPersonalDataCorrect
        {
            get { return GetField<bool>("PersonalCorrect"); }
        }


        /*     public void InitFieldsIfEmpty()
             {
                 var db = new DB();
                 var profile = db.ShopCartProfiles.FirstOrDefault(x => x.UserID == (Guid)Membership.GetUser().ProviderUserKey);
                 if (profile == null || profile.Name.IsNullOrEmpty()) return;
                 var keys = Relations.Select(x => x.Key);
                 var fields = db.ShopCartFields.Where(x => keys.Contains(x.Name) && x.ShopCartID == ID).ToList();
                 foreach (var key in keys)
                 {
                     var field = fields.FirstOrDefault(x => x.Name == key);
                     var data = profile.GetPropertyValue(Relations.First(x => x.Key == key).Value);
                     if (data != null)
                     {
                         if (field == null || field.Value.IsNullOrEmpty() || field.Value != data.ToString())
                         {
                             SetField(key, data);
                         }
                     }
                 }
             }



             public void SaveFieldsInProfile(bool overwrite = false)
             {
                 var db = new DB();
                 var profile = db.ShopCartProfiles.FirstOrDefault(x => x.UserID == (Guid)Membership.GetUser().ProviderUserKey);
                 if (profile == null)
                 {
                     profile = new ShopCartProfile()
                         {
                             UserID = (Guid)Membership.GetUser().ProviderUserKey
                         };
                     db.ShopCartProfiles.InsertOnSubmit(profile);
                 }
                 var keys = Relations.Select(x => x.Key);
                 foreach (var key in keys)
                 {
                     var field = GetField<object>(key);
                     var data = profile.GetPropertyValue(Relations.First(x => x.Key == key).Value);
                     if (field != null && (data == null || data.ToString().IsNullOrEmpty() || overwrite))
                     {
                         profile.SetPropertyValue(Relations.First(x => x.Key == key).Value, field);
                     }
                 }

                 if (SelectedProvider.ShowRegions && profile.Region.IsNullOrEmpty())
                     profile.Region = SelectedRegion.Name;
                 else profile.Region = SelectedProvider.DefaultCity;

                 profile.Address = profile.FullAdress;
                 db.SubmitChanges();

             }*/


        public Order CreateOrder(FastOrderModel form = null)
        {
            Guid? userID = null;
            var db = new DB();
            var currUser = Membership.GetUser();
            if (currUser == null && form != null)
            {
                var mail = "";
                var phone = "";
                var name = "";
                if (form.Type == "Физическое лицо")
                {
                    mail = form.Email;
                    phone = form.Phone;
                    name = form.FullName;
                }
                else if (form.Type == "Регионы РФ:Физическое лицо")
                {
                    mail = form.EmailA;
                    phone = form.PhoneA;
                    name = form.FullNameA;
                }
                else if (form.Type == "Юридическое лицо")
                {
                    mail = form.OrgMail;
                    phone = form.OrgPhone;
                    name = form.OrgPerson;
                }
                else if (form.Type == "Регионы РФ:Юридическое лицо")
                {
                    mail = form.OrgMailB;
                    phone = form.OrgPhoneB;
                    name = form.OrgPersonB;
                }
/*
                bool emptyMail = !mail.IsFilled();
                if (!mail.IsFilled())
                {
                    mail = SiteSetting.Get<string>("DefaultOrderMail");
                }
*/


                if (mail.IsFilled())
                {
                    var users = Membership.FindUsersByEmail(mail);

                    //if (users.Count > 0)
                    {
                        if (users.Count > 0 && Roles.IsUserInRole(mail, "Client"))
                        {
                            //if (!emptyMail)
                            {
                                FormsAuthentication.SetAuthCookie(mail, true);
                            }
                            foreach (MembershipUser user in users)
                            {
                                userID = (Guid?)user.ProviderUserKey;
                                if (!Roles.IsUserInRole(user.UserName, "Client"))
                                    Roles.AddUserToRole(user.UserName, "Client");
                                string nn = name;
                                string sn = "";
                                if (nn.Trim().Contains(" "))
                                {
                                    var arr = nn.Split<string>(" ").ToList();
                                    if (arr.Count() == 2)
                                    {
                                        nn = arr.ElementAt(0);
                                        sn = arr.ElementAt(1);
                                    }
                                }
                                var profile = db.UserProfiles.FirstOrDefault(x => x.UserID == userID);
                                if (profile == null)
                                {
                                    profile = new UserProfile()
                                    {
                                        UserID = (Guid) user.ProviderUserKey,
                                        FromIP = HttpContext.Current.Request.GetRequestIP().ToIPInt(),
                                        RegDate = DateTime.Now,
                                        Email = mail,
                                        Name = nn,
                                        Patrinomic = "",
                                        Surname = sn,
                                        MobilePhone = phone
                                    };

                                    db.UserProfiles.InsertOnSubmit(profile);
                                }
                                else
                                {
                                    profile.Email = mail;
                                    profile.Name = nn;
                                    profile.Surname = sn;
                                    profile.MobilePhone = phone;

                                }
                                db.SubmitChanges();

                                break;
                            }

                        }
                        else
                        {
                            MembershipCreateStatus createStatus;
                            var pass = new Random(DateTime.Now.Millisecond).GeneratePassword(6);
                            var user = Membership.CreateUser(mail, pass, mail, null, null, true,
                                null, out createStatus);
                            if (createStatus == MembershipCreateStatus.Success)
                            {
                                userID = (Guid?)user.ProviderUserKey;
                                Roles.AddUserToRole(user.UserName, "Client");
                                string nn = name;
                                string sn = "";
                                if (nn.Trim().Contains(" "))
                                {
                                    var arr = nn.Split<string>(" ").ToList();
                                    if (arr.Count() == 2)
                                    {
                                        nn = arr.ElementAt(0);
                                        sn = arr.ElementAt(1);
                                    }
                                }
                                var profile = new UserProfile()
                                {
                                    UserID = (Guid)user.ProviderUserKey,
                                    FromIP = HttpContext.Current.Request.GetRequestIP().ToIPInt(),
                                    RegDate = DateTime.Now,
                                    Email = mail,
                                    Name = nn,
                                    Patrinomic = "",
                                    Surname = sn,
                                    MobilePhone = phone
                                };

                                db.UserProfiles.InsertOnSubmit(profile);
                                db.SubmitChanges();

                              /*  MailingList.Get("RegisterLetter")
                                    .WithReplacement(
                                        new MailReplacement("{PASSWORD}", pass)
                                    ).To(mail).Send();*/

                                FormsAuthentication.SetAuthCookie(mail, true);
                            }
                        }
                    }
                    
                }

            }
            else if (currUser != null)
            {
                userID = (Guid?) currUser.ProviderUserKey;
            }

            if (!userID.HasValue)
                return null;

            var order = new Order()
                {
                    CreateDate = DateTime.Now,
                    StatusID = OrderStatus.GetStatusID("Accepted"),
                    UserID = userID.Value
                };
            var details = new OrderDetail()
                {
                    Address = SelectedAddress,
                    AddressID = LastAddressID,
                    Volume = TotalVolume,
                    Weight = TotalWeight,
                    DiscountCost = TotalSumWithoutDiscount > TotalSumForOrder ? Math.Abs(TotalSumWithoutDiscount - TotalSumForOrder) : 0,
                    OrderCost = TotalSumForOrder,

                    DeliveryCost = 0, //SelectedRegion.OrderDeliveryCost,
                    DeliveryType = 0,
                    PaymentType = 0,
                    Order = order,
                    RegionID = null
                };


            db.Orders.InsertOnSubmit(order);
            db.OrderDetails.InsertOnSubmit(details);

            foreach (var x in ActiveProducts)
            {
                db.OrderedProducts.InsertOnSubmit(
                    new OrderedProduct()
                        {
                            Amount = x.Count,
                            SalePrice = x.StoreProduct.PriceModule.ShopCartPrice,
                            ProductID = x.ProductID,
                            ProductName = x.StoreProduct.Name,
                            Order = order
                        });

            }
            db.SubmitChanges();
            return order;
        }

        public void SendLetters(Order order)
        {
            var replacements = new List<MailReplacement>
                        {
                            new MailReplacement("{BOOKSLIST}", GetHtmlForLetterBookList(false)),
                            new MailReplacement("{ADDRESS}", HtmlForDelivery),
                            new MailReplacement("{ALLSUM}", TotalSum.ToNiceDigit()),
                            new MailReplacement("{COMMENT}", SelectedAddressEntry.Comment),
                            new MailReplacement("{DISCOUNT}", Math.Abs(TotalSumForOrder - TotalSum).ToNiceDigit()),
                            new MailReplacement("{DISCOUNTPERCENT}", StoreProduct.CommonDiscountForCart.ToString("f0")),
                            new MailReplacement("{FINALSUM}", TotalSumForOrder.ToNiceDigit()),
                            new MailReplacement("{ORDERDATE}", DateTime.Now.ToString("d MMMM yyyy")),
                            new MailReplacement("{PAYMENTTYPE}", "Оплата наличными"),
                            new MailReplacement("{USERDATA}", HTMLForUserDataReg),
                            new MailReplacement("{USERNAME}", User.Profile.FullName),
                            new MailReplacement("{ORDERNUM}", order.ID.ToString("d10"))
                        };

            MailingList.Get("OrderNotificationClient")
                       .To(UserMail)
                       .WithReplacements(replacements)
                       .Send();

            replacements = replacements.Where(x => x.Key != "{BOOKSLIST}").ToList();
            replacements.Add(new MailReplacement("{BOOKSLIST}", GetHtmlForLetterBookList(true)));

            MailingList.Get("OrderNotificationAdmin").WithReplacements(replacements).Send();


        }


        public void SendLetters(Order order, FastOrderModel form)
        {
            List<MailReplacement> replacements = null;
            var xls = CreateOrderXls(order);
            if (form.Type.Contains("Физическое лицо"))
            {
                replacements = new List<MailReplacement>
                        {
                            new MailReplacement("{BOOKSLIST}", GetHtmlForLetterBookList(false)),
                            new MailReplacement("{ADDRESS}", form.Type.Contains("Регионы") ? form.AddressA :form.Address),
                            new MailReplacement("{ALLSUM}", TotalSum.ToNiceDigit()),
                            new MailReplacement("{COMMENT}", ""),
                            new MailReplacement("{DISCOUNT}", Math.Abs(TotalSumForOrder - TotalSum).ToNiceDigit()),
                            new MailReplacement("{DISCOUNTPERCENT}", StoreProduct.CommonDiscountForCart.ToString("f0")),
                            new MailReplacement("{FINALSUM}", TotalSumForOrder.ToNiceDigit()),
                            new MailReplacement("{ORDERDATE}", DateTime.Now.ToString("d MMMM yyyy")),
                            new MailReplacement("{PAYMENTTYPE}", "Оплата наличными"),
                            new MailReplacement("{USERDATA}", HTMLForUserData(form)),
                            new MailReplacement("{USERNAME}",form.Type.Contains("Регионы")?form.FullNameA : form.FullName),
                            new MailReplacement("{ORDERNUM}", order.ID.ToString("d10")),
                            new MailReplacement("{XLS}", xls)
                        };
            }
            else if (form.Type.Contains("Юридическое лицо"))
            {
                replacements = new List<MailReplacement>
                        {
                            new MailReplacement("{BOOKSLIST}", GetHtmlForLetterBookList(false)),
                            new MailReplacement("{ADDRESS}",form.Type.Contains("Регионы")? form.OrgDelivAddrB : form.OrgDelivAddr),
                            new MailReplacement("{ALLSUM}", TotalSum.ToNiceDigit()),
                            new MailReplacement("{COMMENT}", ""),
                            new MailReplacement("{DISCOUNT}", Math.Abs(TotalSumForOrder - TotalSum).ToNiceDigit()),
                            new MailReplacement("{DISCOUNTPERCENT}", StoreProduct.CommonDiscountForCart.ToString("f0")),
                            new MailReplacement("{FINALSUM}", TotalSumForOrder.ToNiceDigit()),
                            new MailReplacement("{ORDERDATE}", DateTime.Now.ToString("d MMMM yyyy")),
                            new MailReplacement("{PAYMENTTYPE}", "Безналичный рассчет"),
                            new MailReplacement("{USERDATA}", HTMLForUserData(form)),
                            new MailReplacement("{USERNAME}",form.Type.Contains("Регионы")? form.OrgPersonB : form.OrgPerson),
                            new MailReplacement("{ORDERNUM}", order.ID.ToString("d10")),
                            new MailReplacement("{XLS}", xls)
                        };
            }



            MailingList.Get("OrderNotificationClient")
                .To(GetMailFromForm(form))
                       .WithReplacements(replacements)
                       .Send();

            replacements = replacements.Where(x => x.Key != "{BOOKSLIST}").ToList();
            replacements.Add(new MailReplacement("{BOOKSLIST}", GetHtmlForLetterBookList(true)));

            MailingList.Get("OrderNotificationAdmin").WithReplacements(replacements).Send();


        }

        private string GetMailFromForm(FastOrderModel form)
        {
            if (form.Type == "Физическое лицо")
            {
                return form.Email;
            }
            else if (form.Type == "Регионы РФ:Физическое лицо")
            {
                return form.EmailA;
            }
            else if (form.Type == "Юридическое лицо")
            {
                return form.OrgMail;
            }

            else
            {
                return form.OrgMailB;
            }

        }

        private string CreateOrderXls(Order order)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/content/Orders/")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/content/Orders/"));
            }
            string file = HttpContext.Current.Server.MapPath("/content/Orders/" + order.ID + ".xls");
            var stream = new FileStream(file, FileMode.Create);
            var workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("Order");

            int rowNum = 0;
            var header = worksheet.CreateRow(rowNum);
            header.CreateCell(0).SetCellValue("Артикл");
            header.CreateCell(1).SetCellValue("Название");
            header.CreateCell(2).SetCellValue("Цена");
            header.CreateCell(3).SetCellValue("Кол-во");
            header.CreateCell(4).SetCellValue("Сумма");

            rowNum++;

            foreach (ShopCartItem item in ActiveProducts)
            {
                var row = worksheet.CreateRow(rowNum);
                row.CreateCell(0).SetCellValue(item.StoreProduct.Article);
                row.CreateCell(1).SetCellValue(item.StoreProduct.NameOrDef);
                row.CreateCell(2).SetCellValue((item.StoreProduct.PriceModule.ShopCartPrice).ToNiceDigit());
                row.CreateCell(3).SetCellValue(item.Count);
                row.CreateCell(4).SetCellValue((item.Sum).ToNiceDigit());
                
                rowNum++;
            }
            workbook.Write(stream);
            stream.Close();

            var link = "http://" + HttpContext.Current.Request.Url.Host + "/content/Orders/" + order.ID + ".xls";

            return link;
        }


        public string GetHtmlForLetterBookList(bool includeArticles)
        {
            string message = "<table style=\"border-collapse: collapse; table-layout: fixed;width:100%\">";
            int mainLen = includeArticles ? 50 : 70;
            message += string.Join("",
                                   ActiveProducts.Select(
                                       x =>
                                       "<tr>{4}<td style=\"padding-right: 10px;width:{5}%\">{0}</td><td style=\"width: 10%;\">{1}</td><td style=\"width: 10%;\">{2}</td><td style=\"width: 10%;\">{3}</td></tr>"
                                           .FormatWith(
                                               x.StoreProduct.NameOrDef, x.Count.ToString(),
                                               (x.StoreProduct.PriceModule.ShopCartPrice).ToNiceDigit(),
                                               (x.Sum).ToNiceDigit(),
                                               includeArticles
                                                   ? "<td style=\"width: 20%; padding-right: 10px\">{0}</td>"
                                                         .FormatWith(x.StoreProduct.Article)
                                                   : "", mainLen)));
            message += "</table>";
            return message;
        }

        public string HTMLForUserDataReg
        {
            get
            {
                var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("ФИО", User.Profile.FullName),
                        new KeyValuePair<string, string>("E-mail", User.MembershipData.Email),
                        new KeyValuePair<string, string>("Телефон", User.Phone)
                    };

                return pairs.Select(x => "{0}:&nbsp;<b>{1}</b>".FormatWith(x.Key, x.Value)).JoinToString("</li><li>");
            }
        }
        public string HTMLForUserData(FastOrderModel form)
        {
            if (form.Type == "Физическое лицо")
            {
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Покупатель", form.Type),
                    new KeyValuePair<string, string>("ФИО", form.FullName),
                    new KeyValuePair<string, string>("E-mail", form.Email),
                    new KeyValuePair<string, string>("Телефон", form.Phone)

                };

                return pairs.Select(x => "{0}:&nbsp;<b>{1}</b>".FormatWith(x.Key, x.Value)).JoinToString("</li><li>");
            }
            else if (form.Type == "Регионы РФ:Физическое лицо")
            {
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Покупатель", form.Type),
                    new KeyValuePair<string, string>("ФИО", form.FullNameA),
                    new KeyValuePair<string, string>("E-mail", form.EmailA),
                    new KeyValuePair<string, string>("Телефон", form.PhoneA),
                    new KeyValuePair<string, string>("Жесткая обрешетка", form.TargetFixedA),
                    new KeyValuePair<string, string>("Ф.И.О. конечного грузополучателя", form.TargetFullNameA),
                    new KeyValuePair<string, string>("Паспортные данные", form.PhoneA),
                    new KeyValuePair<string, string>("Адрес регистрации", form.TargetAddressA),
                    new KeyValuePair<string, string>("Транспортная компания", form.TargetTransA)

                };

                return pairs.Select(x => "{0}:&nbsp;<b>{1}</b>".FormatWith(x.Key, x.Value)).JoinToString("</li><li>");
            }
            else if (form.Type == "Юридическое лицо")
            {
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Покупатель", form.Type),

                    new KeyValuePair<string, string>("Наименование организации", form.OrgName),
                    new KeyValuePair<string, string>("Контактное лицо", form.OrgPerson),
                    new KeyValuePair<string, string>("Телефон контактного лица", form.OrgPhone),
                    new KeyValuePair<string, string>("Email контактного лица", form.OrgMail),
                    new KeyValuePair<string, string>("ИНН", form.OrgINN),
                    new KeyValuePair<string, string>("КПП", form.OrgKPP),
                    new KeyValuePair<string, string>("ОКПО", form.OrgOKPO),
                    new KeyValuePair<string, string>("Корр счет", form.OrgKorr),
                    new KeyValuePair<string, string>("Расчетный счет", form.OrgAccount),
                    new KeyValuePair<string, string>("Наименование банка", form.OrgBankName),
                    new KeyValuePair<string, string>("БИК банка", form.OrgBik),
                    new KeyValuePair<string, string>("Юридический адрес", form.OrgJurAddr),
                    new KeyValuePair<string, string>("Фактический адрес", form.OrgFaktAddr),
                    /*new KeyValuePair<string, string>("Адрес доставки", form.OrgDelivAddr),*/


                };
                return pairs.Select(x => "{0}:&nbsp;<b>{1}</b>".FormatWith(x.Key, x.Value)).JoinToString("</li><li>");
            }


            {
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Покупатель", form.Type),

                    new KeyValuePair<string, string>("Наименование организации", form.OrgNameB),
                    new KeyValuePair<string, string>("Контактное лицо", form.OrgPersonB),
                    new KeyValuePair<string, string>("Телефон контактного лица", form.OrgPhoneB),
                    new KeyValuePair<string, string>("Email контактного лица", form.OrgMailB),
                    new KeyValuePair<string, string>("ИНН", form.OrgINNB),
                    new KeyValuePair<string, string>("КПП", form.OrgKPPB),
                    new KeyValuePair<string, string>("ОКПО", form.OrgOKPOB),
                    new KeyValuePair<string, string>("Корр счет", form.OrgKorrB),
                    new KeyValuePair<string, string>("Расчетный счет", form.OrgAccountB),
                    new KeyValuePair<string, string>("Наименование банка", form.OrgBankNameB),
                    new KeyValuePair<string, string>("БИК банка", form.OrgBikB),
                    new KeyValuePair<string, string>("Юридический адрес", form.OrgJurAddrB),
                    new KeyValuePair<string, string>("Фактический адрес", form.OrgFaktAddrB),
                    new KeyValuePair<string, string>("Ф.И.О. конечного грузополучателя", form.OrgTargetPersonB),
                    new KeyValuePair<string, string>("Паспортные данные", form.OrgTargetPassB),
                    new KeyValuePair<string, string>("Транспортная компания", form.OrgTargetTransB),
                    /*new KeyValuePair<string, string>("Адрес доставки", form.OrgDelivAddr),*/


                };
                return pairs.Select(x => "{0}:&nbsp;<b>{1}</b>".FormatWith(x.Key, x.Value)).JoinToString("</li><li>");
            }


        }

        public string HtmlForDelivery
        {
            get
            {
                return SelectedAddress;
            }
        }


        public static string TranslateToHtml(string xml, string parent)
        {
            XDocument document;
            try
            {
                document = XDocument.Parse(xml);
            }
            catch (Exception)
            {
                return "";
            }

            var message = "<table style=\"width:100%\">";
            var profile = new ProfileDataAnnotations();
            message += string.Join("",
                                   document.Descendants(parent).Elements().Select(
                                       x =>
                                       "<tr><td style=\"width:300px\">{0}:</td><td><b>{1}</b></td><tr>".FormatWith(
                                           profile.GetPropertyAttribute<DisplayNameAttribute>(x.Name.LocalName,
                                                                                              "DisplayName"), x.Value)));
            message += "</table><br>";
            return message;

        }

        public void ClearActive()
        {
            var db = new DB();
            db.ShopCartItems.DeleteAllOnSubmit(
                db.ShopCartItems.Where(x => x.ShopCartID == ID && !x.IsDelayed && x.StoreProduct.IsActive));
            db.SubmitChanges();
        }

        public void Clear()
        {
            var db = new DB();
            db.ShopCartItems.DeleteAllOnSubmit(
                db.ShopCartItems.Where(x => x.ShopCartID == ID && !x.IsDelayed));
            db.SubmitChanges();
        }

        public string OrderUserData
        {
            get
            {
                var profile = Membership.GetUser().UserEntity().Profile;
                var doc = new XDocument();
                var data = new XElement("UserData");
                doc.Add(data);
                data.Add(new XElement("Email", profile.Email));
                data.Add(new XElement("Surname", profile.Surname));
                data.Add(new XElement("Name", profile.Name));
                data.Add(new XElement("Patrinomic", profile.Patrinomic));
                data.Add(new XElement("HomePhone", profile.HomePhone));
                data.Add(new XElement("MobilePhone", profile.MobilePhone));
                return doc.ToString();
            }
        }

        public string OrderRegion
        {
            get
            {
                if (SelectedProvider.ShowRegions)
                    return SelectedRegion.Name;
                return SelectedProvider.DefaultCity;
            }
        }

        public string OrderOrgData
        {
            get
            {
                if (!GetField<bool>("ShowOrg")) return "";
                var keys = Relations.Where(x => x.Key.StartsWith("Org")).Select(x => x.Key);
                var doc = new XDocument();
                var data = new XElement("OrgData");
                doc.Add(data);
                foreach (var key in keys)
                {
                    data.Add(new XElement(key, GetField<string>(key)));
                }
                return doc.ToString();
            }
        }



        private decimal? _minSum;
        public decimal MinSum
        {
            get
            {
                if (!_minSum.HasValue)
                {
                    _minSum = SiteSetting.Get<int>("OrderMin");

                }
                return _minSum.Value;
            }
        }
    }
}