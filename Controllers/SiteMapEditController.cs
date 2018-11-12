using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;
using NPOI.SS.Formula.Functions;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;
using System.Text;

namespace Smoking.Controllers
{

    public class SiteMapEditController : Controller
    {
        private DB db = new DB();

        [HttpGet]
        [AuthorizeMaster]
        /*[MenuItem("Карта сайта", 992, 99, Icon = "sitemap")]*/
        [MenuItem("Карта сайта", 100)]
        public ActionResult Index()
        {
            var path = Server.MapPath("~/sitemap.xml");
            if (System.IO.File.Exists(path))
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    ViewBag.FileContent = sr.ReadToEnd();
                }
            }
            else
            {
                ViewBag.FileContent = "";
            }
            return PartialView();
        }


        public FileContentResult Download(string path = "/sitemap.xml")
        {
            var mp = HttpContext.Server.MapPath(path);
            if (!System.IO.File.Exists(mp))
            {
                Response.StatusCode = 404;
                Response.Status = "Not Found";
                return new FileContentResult(new byte[0], "text/xml");
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=\"" +
                                                        HttpContext.Server.UrlEncode(Path.GetFileName(path)) + "\"");

            using (var reader = new FileStream(mp, FileMode.Open))
            {
                var buf = new byte[reader.Length];
                reader.Read(buf, 0, (int)reader.Length);
                return new FileContentResult(buf, "text/xml");
            }



        }



        public FileContentResult SiteMap()
        {
            var sitemap = new Sitemap();
            var baseURL = AccessHelper.SiteUrl;
            sitemap.Add(new Location(DateTime.Now)
            {
                Url = baseURL,

                //LastModified = DateTime.UtcNow.AddDays(-1)
            });


            var mainType = CMSPage.GetTypes("MainPage").ToList();
            if (mainType.Any())
            {
                var pages =
                    CMSPage.FullPageTable.Where(x => x.Type != mainType.First())
                           .OrderBy(x => x.OrderNum)
                           .Select(x => new Location() { Url = baseURL + x.FullUrl, LastModified = x.LastMod });
                sitemap.AddRange(pages);
            }

            var cats =
                CatalogBrowser.CategoriesList.OrderBy(x => x.Value.OrderNum)
                              .Select(x => new Location() { Url = baseURL + x.Value.FullUrl, LastModified = x.Value.LastMod }).ToList();
            if (cats.Any())
                sitemap.AddRange(cats);

            var prods = db.StoreProducts.Where(x => !x.Deleted).ToList().Select(x => new Location() { Url = baseURL + x.FullUrl, LastModified = x.LastMod }).ToList();
            if (prods.Any())
                sitemap.AddRange(prods);


            sitemap.Locations = sitemap.Locations.OrderByDescending(x => x.LastMod).ToArray();
            var cnt = sitemap.Locations.Count();
            var counter = 0;
            foreach (var location in sitemap.Locations)
            {
                location.Priority = Math.Round(1.0 - (1.0 / cnt) * counter, 5);
                counter++;
            }


            using (var fs = new MemoryStream())
            {
                var xs = new XmlSerializer(typeof(Sitemap));
                xs.Serialize(fs, sitemap);
                fs.Position = 0;
                return new FileContentResult(fs.ToArray(), "text/xml");
            }
        }

        [HttpPost]
        [AuthorizeMaster]

        public ActionResult Index(string Path)
        {
            var sitemap = new Sitemap();
            var siteMapPath = Server.MapPath("/sitemap.xml");

            var baseURL = AccessHelper.SiteUrl;
            sitemap.Add(new Location(DateTime.Now)
            {
                Url = baseURL,

                //LastModified = DateTime.UtcNow.AddDays(-1)
            });


            var mainType = CMSPage.GetTypes("MainPage").ToList();
            if (mainType.Any())
            {
                var pages =
                    CMSPage.FullPageTable.Where(x => x.Type != mainType.First())
                           .OrderBy(x => x.OrderNum)
                           .Select(x => new Location() { Url = baseURL + x.FullUrl, LastModified = x.LastMod });
                sitemap.AddRange(pages);
            }

            var images = db.StoreImages.ToList();

            var cats =
                CatalogBrowser.CategoriesList.OrderBy(x => x.Value.OrderNum)
                    .Select(
                        x =>
                            new Location()
                            {
                                Url = baseURL + x.Value.FullUrl,
                                LastModified = x.Value.LastMod,
                                Images = new[] {new MapImage() {loc = System.Web.HttpContext.Current.Request.Url.Scheme+"://"+ System.Web.HttpContext.Current.Request.Url.Host+ x.Value.ImageUrl}}
                            })
                    .OrderBy(x => x.LastMod)
                    .ToList();
            if (cats.Any())
                sitemap.AddRange(cats);

            var prods =
                db.StoreProducts.Where(x => !x.Deleted) /*.Take(100)*/
                    .ToList()
                    .Select(
                        x =>
                            new Location()
                            {
                                Url = baseURL + x.FullUrl,
                                LastModified = x.LastMod,
                                Images =
                                    images.Where(c => c.ProductID == x.ID)
                                        .ToList()
                                        .Select(
                                            z =>
                                                new MapImage()
                                                {
                                                    loc =
                                                        System.Web.HttpContext.Current.Request.Url.Scheme + "://" +
                                                        System.Web.HttpContext.Current.Request.Url.Host +
                                                        x.GetImgURL(z, 450, 450)
                                                })
                                        .ToArray()
                            })
                    .ToList();


            if (prods.Any())
                sitemap.AddRange(prods);


            //sitemap.Locations = sitemap.Locations.OrderBy(x => x.LastMod).ToArray();
            /*
                        var cnt = sitemap.Locations.Count();
                        var counter = 0;
                        foreach (var location in sitemap.Locations)
                        {
                            location.Priority = Math.Round(1.0 - (1.0 / cnt) * counter, 5);
                            counter++;
                        }
            */
            var maxCnt = 2500;

            if (sitemap.Locations.Count() < maxCnt)
            {

                /*
                                using (var fs = new FileStream(siteMapPath, FileMode.Create, FileAccess.ReadWrite))
                                {
                */

                using (var sw = new StreamWriter(siteMapPath, false, new UTF8Encoding(false)))
                {
                    var ns = new XmlSerializerNamespaces();

                    //Add an empty namespace and empty value
                    ns.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    var xs = new XmlSerializer(typeof(Sitemap));
                    xs.Serialize(sw, sitemap, ns);

                }


                /*
                                }
                */
            }
            else
            {
                var parts = Math.Ceiling((decimal)sitemap.Locations.Count() / (decimal)maxCnt);

                var smi = new SiteMapIndex();

                for (int i = 1; i <= parts; i++)
                {
                    smi.Add(new Location() { Url = Request.Url.Scheme + "://" + Request.Url.Host + "/sitemap_" + i + ".xml", LastModified = DateTime.Now });
                }

                using (var sw = new StreamWriter(siteMapPath, false, new UTF8Encoding(false)))
                {
                    var ns = new XmlSerializerNamespaces();

                    //Add an empty namespace and empty value
                    ns.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    var xs = new XmlSerializer(typeof(SiteMapIndex));
                    xs.Serialize(sw, smi, ns);

                }


                for (int i = 1; i <= parts; i++)
                {
                    var sm = new Sitemap();
                    sm.AddRange(sitemap.Locations.Skip((i - 1) * maxCnt).Take(maxCnt));

                    using (var sw = new StreamWriter(Server.MapPath("~/sitemap_" + i + ".xml"), false, new UTF8Encoding(false)))
                    {
                        var ns = new XmlSerializerNamespaces();

                        ns.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");

                        var xs = new XmlSerializer(typeof(Sitemap));
                        xs.Serialize(sw, sm, ns);

                    }

                }



            }
            var path = Server.MapPath("~/sitemap.xml");
            if (System.IO.File.Exists(path))
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    ViewBag.FileContent = sr.ReadToEnd();
                }
            }
            else
            {
                ViewBag.FileContent = "";
            }

            ModelState.AddModelError("", "Карта сайта успешно обновлена");

            return PartialView();
            //return new FilePathResult(siteMapPath, "text/xml");

        }

    }
}
