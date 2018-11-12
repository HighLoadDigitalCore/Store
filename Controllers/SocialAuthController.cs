using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{
    [Authorize]
    public class SocialAuthController : Controller
    {
        [MenuItem("Вид", 60, Icon = "themes")]
        [HttpGet]
        [AuthorizeMaster]
        public ActionResult IndexList()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Socials(bool needHide = false)
        {
            var result = SocialAuthResult.CheckAuth();
            result.NeedHide = needHide;
            return PartialView(result);
        }

    }
}
