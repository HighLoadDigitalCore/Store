using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class ThemeController : Controller
    {
        //
        // GET: /Theme/
        private DB db = new DB();

        [HttpGet]
        public ActionResult Index(int themeID = 2)
        {
            var themesProps = db.ThemeProperties.Where(x => x.ThemeID == themeID).OrderBy(x => x.OrderNum).ToList();
            
            var text = themesProps.Where(x => x.IsFilled)
                       .Select(
                           x =>
                           CreateCss(x))
                       .JoinToString("\r\n");



            var cats = db.StoreCategories.Where(x => x.MenuImage != null && x.MenuImage.Length > 0 && !x.Deleted).ToList();
            if (cats.Any())
            {
                text += "\r\n";
                text += cats.Select(
                    x =>
                        string.Format(
                            "nav.hovered[data-megamenu] section[data-promo=\"{0}\"] {{background: url(\"{1}\") no-repeat scroll 100% 100% rgba(0, 0, 0, 0);}}",
                            x.ID, x.MenuImage))
                    .JoinToString("\r\n");
            }

        


            return new ContentResult() { Content = text, ContentEncoding = Encoding.UTF8, ContentType = "text/css" };
        }

        private string CreateCss(ThemeProperty x)
        {
            var vpnl = x.ValuePropName.Split<string>(",").ToList();
            if (vpnl.Count() > 1)
            {
                var vpnls = vpnl.Select(z => string.Format("{0}:{1}!important", z, string.Format(x.ValueTemplate, x.ValueString))).JoinToString(";\r\n");
                return string.Format("{0} {{{1}}}", x.ClassName, vpnls
                    );
            }
            return string.Format("{0} {{{1}:{2}!important;}}", x.ClassName, x.ValuePropName,
                string.Format(x.ValueTemplate, x.ValueString));
        }


        [HttpGet]
        [AuthorizeMaster]
        /*[MenuItem("Цвета и фон", ID = -80, Icon = "pen_alt_stroke", ParentID = 60)]*/
        public ActionResult PropertyList(int themeID = 2)
        {
            var themesProps = db.ThemeProperties.Where(x => x.ThemeID == themeID).OrderBy(x => x.OrderNum);
            return PartialView(themesProps);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult PropertyList(FormCollection collection, int themeID = 2)
        {
            var themesProps = db.ThemeProperties.Where(x => x.ThemeID == themeID).OrderBy(x => x.OrderNum);
            foreach (var prop in themesProps)
            {
                if (prop.Editor == "Color")
                {
                    prop.ValueText = collection.GetValue(prop.InputName).AttemptedValue;
                }
                else if (prop.Editor == "TextBox")
                {
                    prop.ValueText = collection.GetValue(prop.InputName).AttemptedValue;
                }
                else
                {
                    var newContent = collection.GetValue(prop.InputName + "_Path").AttemptedValue.GetFileContent();
                    if (newContent != null && newContent.Length > 0)
                        prop.ValueBinary = newContent;
                }
            }
            db.SubmitChanges();
            ModelState.AddModelError("", "Данные сохранены");
            return PartialView(themesProps);
        }


        public ActionResult Image(int id)
        {
            var xd = db.ThemeProperties.First(x => x.ID == id);
            if (!xd.IsNullImage)
            {

                var prop = xd.ValueBinary;
                return new FileContentResult(prop is DBNull || prop == null ? new byte[0] : prop.ToArray(),
                                             MIMETypeWrapper.GetMIMEForData(
                                                 new Binary(prop is DBNull ? new byte[0] : prop.ToArray())));
            }
            return new EmptyResult();

        }
    }
}
