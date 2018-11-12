using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;


namespace Smoking.Models
{
    public class StaticContentInterceptor : IHttpModule
    {

        protected static string[] InterceptPaths = new[] { "~/content/", };
        protected static string[] Exts = new string[]{".jpg", ".png", ".gif"};

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            try
            {
                var cp = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;

                if (cp != null && InterceptPaths.Any(z => cp.StartsWith(z, StringComparison.CurrentCultureIgnoreCase)) && Exts.Any(z=> cp.EndsWith(z, StringComparison.CurrentCultureIgnoreCase)))
                {

                    var segs = cp.Split<string>("/").ToList();
                    if (!segs.Any())
                    {
                        HttpContext.Current.Response.StatusCode = 404;
                        return;
                    }

                    var chain = segs.Last().Split<string>("-", ".").ToList();

                    var ext = chain.Last();
                    chain = chain.Take(chain.Count - 1).ToList();


                    var clearChain = chain.Where(x => x != "nrt" && x != "nlg" ).ToList();


                    if (cp.Contains("/content/category/"))
                    {
                        clearChain = clearChain.Where(x => x != "ci" && x != "mi").ToList();
                    }

                    var size = clearChain.Last();
                    var containInfo = false;
                    var width = 0;

                    if (new Regex(@"w\d+").IsMatch(size))
                    {
                        containInfo = true;
                        width = int.Parse(size.Substring(1));
                    }


                    var rq = HttpContext.Current.Request.RequestContext;
                    var helper = new UrlHelper(rq);


                    if (cp.Contains("/content/category/"))
                    {
                        var col = chain.Contains("ci") ? "CategoryImage" : (chain.Contains("mi") ? "Image" : "");
                        if (col.IsNullOrEmpty() || !containInfo)
                        {
                            HttpContext.Current.Response.StatusCode = 404;
                            return;
                        }


                        var rw = UniversalEditorPagedData.GetImageWrapper("StoreCategories", "ID",
                            CatalogBrowser.CategoriesList[clearChain.Take(clearChain.Count - 1).JoinToString("-")].ID
                                .ToString(), col, width, width);

                        HttpContext.Current.RewritePath(rw);


                        return;
                    }


                    var path = containInfo
                        ? clearChain.Take(clearChain.Count - 1).JoinToString("-") + "." + ext
                        : segs.Last();


                    path = segs.Take(segs.Count - 1).JoinToString("/") + "/" + path;

                    var routeValues = new RouteValueDictionary
                    {
                        {"skiplogo", chain.Contains("nlg")},
                        {"filePath", path},
                        {"padding", 0},
                        {"maxWidth", width},
                        {"maxHeight", width},
                        {"vertalign", "center"},
                        {"skipRotate", chain.Contains("nrt")}
                    };



                    if (containInfo)
                    {
                        var url = UrlHelper.GenerateUrl("Master", "Resize", "Image", routeValues, helper.RouteCollection, rq, true);
                        HttpContext.Current.RewritePath(url);
                        return;
                    }





                 /*   var newPath = PathHelper.TransformPath(cp);

                    var ext = Path.GetExtension(newPath);
                    HttpContext.Current.Response.ContentType = MIMETypeWrapper.GetMIME(ext.Replace(".", ""));
                    if (!File.Exists(newPath))
                    {

                        newPath = HttpContext.Current.Server.MapPath(cp);
                        if (!File.Exists(newPath))
                        {
                            HttpContext.Current.Response.StatusCode = 404;
                            return;
                        }
                    }
                    HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMinutes(10));
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
                    HttpContext.Current.Response.WriteFile(newPath, false);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();*/

                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(ex.Message);
                //HttpContext.Current.Response.Write(ex.StackTrace);
            }
        }

        public void Dispose() { }



    }

}
