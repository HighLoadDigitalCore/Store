using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class SettingsController : Controller
    {
        private DB db = new DB();

        [MenuItem("Настройки", 99, Icon = "wrench")]
        public ActionResult IndexList()
        {
            return View();
        }
        [MenuItem("Настройки системы", 55, 4)]
        public ActionResult CatalogIndexList()
        {
            var list =
                db.SiteSettings.Where(x => x.GroupName == "Настройки для 1С" || x.GroupName == "Настройки для изображений" || x.GroupName == "Настройки оформления заказа" || x.GroupName == "Общие настройки каталога")
                  .OrderBy(x => x.OrderNum);
            foreach (var setting in list)
            {
                ViewData.Add(setting.Setting, setting.oValue);
            }
            ViewBag.TitleBlock = "Настройки системы";
            return View("../Settings/Index", list);
        }


        [HttpPost, AuthorizeMaster, ValidateInput(false)]
        public ActionResult CatalogIndexList(FormCollection collection)
        {
            

            var groups = SettingsController.SaveSettings(collection, ModelState, db);
            ModelState.AddModelError("", "Данные сохранены");
            return CatalogIndexList();

            

        }

        [HttpGet, AuthorizeMaster]

        public ActionResult BlockSettings(string blockname, int viewid)
        {
            var model = Models.BlockSettings.GetSettings(blockname, viewid);
            return View(model.Settings.OrderBy(x=> x.OrderNum).ToList());
        }


        [HttpPost, AuthorizeMaster]
        public ActionResult BlockSettings(string blockname, int viewid, FormCollection collection)
        {
            var model = Models.BlockSettings.GetSettings(blockname, viewid);
            
            foreach (var setting in model.Settings)
            {
                var dbItem =
                    db.CMSPageCellViewSettings.FirstOrDefault(x => x.Name == setting.Field && x.ViewID == viewid);

                if (dbItem == null)
                {
                    dbItem = new CMSPageCellViewSetting()
                    {
                        Name = setting.Field,
                        ViewID = viewid,
                        Value = collection[setting.Field]
                    };
                    db.CMSPageCellViewSettings.InsertOnSubmit(dbItem);


                }
                else
                {
                    dbItem.Value = collection[setting.Field];
                }

            }
            db.SubmitChanges();

            ModelState.AddModelError("", "Данные сохранены");

            model = Models.BlockSettings.GetSettings(blockname, viewid);
            return View(model.Settings.OrderBy(x=> x.OrderNum).ToList());
        }

        [HttpGet, AuthorizeMaster]
        [MenuItem("Курс валюты", 992, 99, Icon = "")]
        public ActionResult Currency()
        {
            
            ViewBag.TitleBlock = "Курс валюты";
            var sRate = SiteSetting.Get<string>("CurrencyRate");
            decimal rate = 0;
            decimal.TryParse(sRate, out rate);

            var auto = ViewBag.AutoCurrencyRate = SiteSetting.Get<bool>("AutoCurrencyRate");

            if (rate == 0 || auto)
            {
                rate = (decimal)new CBRF.DailyInfoSoapClient().GetCursOnDate(DateTime.Now).Tables[0].Rows.Cast<DataRow>()
                    .First(x => x[4].ToString() == "EUR")[2];
            }
            ViewBag.CurrencyRate = rate;
            
                
            return View();
        }   
        
        [HttpPost, AuthorizeMaster]
        public ActionResult Currency(FormCollection collection)
        {

            decimal rate = 0;

            var sRate = "";

            if (collection["CurrencyRate"].IsNullOrEmpty())
            {
                sRate =
                    new CBRF.DailyInfoSoapClient().GetCursOnDate(DateTime.Now).Tables[0].Rows.Cast<DataRow>()
                        .First(x => x[4].ToString() == "EUR")[2].ToString();
            }
            else
            {
                sRate = collection["CurrencyRate"].Replace(".", ",");
            }
            decimal.TryParse(sRate, out rate);
            if (rate == 0)
            {
                  rate = (decimal)new CBRF.DailyInfoSoapClient().GetCursOnDate(DateTime.Now).Tables[0].Rows.Cast<DataRow>()
                    .First(x => x[4].ToString() == "EUR")[2];
            }


            SiteSetting.Set("AutoCurrencyRate", collection["AutoCurrencyRate"].ToBool());
            
            SiteSetting.Set("CurrencyRate", rate);

            ViewBag.AutoCurrencyRate = SiteSetting.Get<bool>("AutoCurrencyRate");
            ViewBag.TitleBlock = "Курс валюты";
            ViewBag.CurrencyRate = rate;
            ModelState.AddModelError("", "Данные сохранены");
                
            return View();
        }


        [HttpGet, AuthorizeMaster]
        [MenuItem("Общие настройки", 991, 99, Icon = "wrench")]
        public ActionResult Index(string[] groups)
        {
            var exclude = new[] { "Настройки почтового сервера", "Настройки для 1С", "Настройки для изображений", "Настройки оформления заказа", "Настройки для квитанций", "Настройки онлайн-чата", "Общие настройки каталога", "Настройки шапки сайта" };
            var list = db.SiteSettings.Where(x => !exclude.Contains(x.GroupName)).OrderBy(x=> x.OrderNum);
            if (groups != null && groups.Any())
            {
                list =
                    list.Where(x => groups.Contains(x.GroupName))
                        
                        .OrderBy(x => x.OrderNum);
            }
            foreach (var setting in list)
            {
                ViewData.Add(setting.Setting, setting.oValue);
            }
            ViewBag.TitleBlock = "Общие настройки";
            return View(list);
        }


        [AuthorizeMaster]
        [MenuItem("Шапка", 82, 60, Icon = "settinghead")]
        public ActionResult HeadSettings()
        {
            var list = db.SiteSettings.Where(x => x.GroupName == "Настройки шапки сайта").OrderBy(x => x.OrderNum);
            foreach (var setting in list)
            {
                ViewData.Add(setting.Setting, setting.oValue);
            }
            
            return View("../Settings/Index", list);

        }

        [HttpPost, AuthorizeMaster, ValidateInput(false)]
        public ActionResult Index(FormCollection collection)
        {
            System.Web.HttpContext.Current.Cache.Remove("Redirects");
            var groups = SaveSettings(collection, ModelState, db);
            ModelState.AddModelError("", "Данные сохранены");
            return Index(groups.ToArray());
        }

        public static IEnumerable<string> SaveSettings(FormCollection collection, ModelStateDictionary state, DB db)
        {
            var groups = new List<string>();
            IValueProvider provider = collection.ToValueProvider();
            foreach (string key in collection.AllKeys)
            {
                var setting = (from x in db.SiteSettings
                               where x.Setting == key
                               select x).FirstOrDefault();
                if (setting != null)
                {
                    try
                    {
                        setting.Value = provider.GetValue(key).ConvertTo(Type.GetType(setting.ObjectType)).ToString();
                        if (!groups.Contains(setting.GroupName))
                            groups.Add(setting.GroupName);

                    }
                    catch (Exception)
                    {
                        setting.Value = "";
                    }
                }
                if (System.Web.HttpContext.Current.Cache[key] != null)
                {

                    System.Web.HttpContext.Current.Cache.Remove(key);
                }

            }
            db.SubmitChanges();

            foreach (string key in collection.AllKeys)
            {
                var setting = (from x in db.SiteSettings
                               where x.Setting == key
                               select x).FirstOrDefault();
                if (setting != null)
                {
                    try
                    {
                        if (setting.Editor == "Hidden" && setting.Title.IsFilled())
                        {
                            SiteSetting.ExecuteFunc(setting.Title);
                        }
                    }
                    catch (Exception e)
                    {
                        state.AddModelError("", e.Message);
                    }
                }

            }
            return groups;
        }
    }
}

