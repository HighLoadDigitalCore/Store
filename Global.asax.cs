using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;
using Smoking.Models;
using Smoking.Extensions;

namespace Smoking
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        private Dictionary<string, string> Redirects
        {
            get
            {
                if (HttpContext.Current.Cache["Redirects"] is Dictionary<string, string>)
                {
                    return HttpContext.Current.Cache["Redirects"] as Dictionary<string, string>;
                }
                var r = SiteSetting.Get<string>("Redirects");
                var lines = r.Split(new string[] { "\r", "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                Dictionary<string, string> _redirects = lines.Select(x => x.Trim(' ', '\r', '\n', '\t').Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(z => z.Trim(' ', '\r', '\n', '\t')).ToArray()).Where(x => x != null && x.Length == 2).ToDictionary(x => x[0], x => x[1]);
                HttpContext.Current.Cache["Redirects"] = _redirects;
                return _redirects;
            }
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            if (Request.Url.ToString().Contains("/catalog/"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/catalog/", "/"));
            }


            if (Redirects.ContainsKey(Request.Url.ToString()))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Redirects[Request.Url.ToString()]);
            }

            if (HttpContext.Current.Request.RawUrl == "/favicon.ico")
            {
                var file = HttpContext.Current.Server.MapPath(SiteSetting.Get<string>("FavIcon"));
                if (File.Exists(file))
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "image/x-icon";
                    HttpContext.Current.Response.WriteFile(file, false);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
                else
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.StatusCode = 404;
                    HttpContext.Current.Response.End();
                }
            }


            if (Request.Url.ToString().EndsWith("/index.html"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/index.html", ""));
            }


            if (Request.Url.ToString().EndsWith("/index.php"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/index.php", ""));
            }

            if (Request.Url.ToString().EndsWith("/index.aspx"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/index.aspx", ""));
            }


            if (Request.Url.ToString().EndsWith("/default.html"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/default.html", ""));
            }


            if (Request.Url.ToString().EndsWith("/default.php"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/default.php", ""));
            }

            if (Request.Url.ToString().EndsWith("/default.aspx"))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("/default.aspx", ""));
            }


            if (Request.Url.ToString().Contains("www."))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("www.", ""));
            }


            if (Request.Url.ToString().Contains("http://") && !Request.IsLocal)
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().Replace("http://", "https://"));
            }

            

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var workingThread = new Thread(UpdatePrices);
            workingThread.Start();
        }

        protected void UpdatePrices()
        {
            if (Application["1C"] is bool && (bool) Application["1C"])
                return;

            Application["1C"] = true;
            if (SiteSetting.Get<bool>("ImportEnabled"))
            {
                var lastUpd = SiteSetting.Get<string>("ImportListUpdate").ToDate("dd.MM.yyyy HH:mm:ss");
                var period = SiteSetting.Get<int>("ImportTime");
                if ((lastUpd.HasValue && DateTime.Now.Subtract(lastUpd.Value).TotalHours > period) || !lastUpd.HasValue)
                {
                    var link = SiteSetting.Get<string>("ImportURL");
                    try
                    {
                        XDocument xd = XDocument.Load(link);
                        var prices = xd.Descendants("Позиция");
                        var db = new DB();
                        foreach (var price in prices)
                        {
                            var item = db.StoreProducts.FirstOrDefault(x => x.Article == price.Element("Артикул").Value);
                            if (item != null)
                            {
                                item.Price = price.Element("Цена").Value.ToDecimal();
                            }
                        }
                        db.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                    }
                    SiteSetting.Set("ImportListUpdate", DateTime.Now);
                }
            }
            Application["1C"] = false;
        }

    }
}