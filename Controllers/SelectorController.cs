using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using Smoking.Extensions.Helpers;
using Smoking.Models;
using Smoking.Extensions;
namespace Smoking.Controllers
{
    public class SelectorController : Controller
    {


        public ActionResult Index(string url1, string url2, string url3, string url4, string url5, string url6, string url7, string url8, string url9,string url10,string url11,string url12,string url13,string url14,string url15, int? book, int? page)
        {
            if (AccessHelper.Repository.ContainsKey("Refreshed"))
                AccessHelper.Repository.Remove("Refreshed");
            var path = new List<string> { url1, url2, url3, url4, url5, url6, url7, url8, url9, url10, url11, url12, url13, url14, url15 };
            var jnp = "~/"+ string.Join("/", path.Where(x => !x.IsNullOrEmpty()).ToList());
            var mapped = Server.MapPath(jnp);
            if(System.IO.File.Exists(mapped) && Path.GetExtension(mapped)!=".cshtml")
            {
                return File(mapped, MIMETypeWrapper.GetMIME(Path.GetExtension(mapped)));
            }
            var info = CommonPageInfo.InitFromQueryParams(path.Where(x => !x.IsNullOrEmpty()).ToList());
            AccessHelper.CurrentPageInfo = info;
            ViewBag.CommonInfo = info;
            return View(info);
        }

      
    }
}
