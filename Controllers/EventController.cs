using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class EventController : Controller
    {
        private DB db = new DB();
        //
        // GET: /Event/
        /*[MenuItem("Содержимое страниц", 5, Icon = "book_alt2")]*/
        [HttpGet]
        [AuthorizeMaster]

        public ActionResult IndexList()
        {
            return View();
        }


        [HttpGet]
        [AuthorizeMaster]
        [MenuItem("Robots.txt", 985, 99, Icon = "robot")]
        public ActionResult Robots()
        {
            var path = Server.MapPath("~/robots.txt");
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

            
            return View();
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Robots(string FileContent, FormCollection collection)
        {
            var path = Server.MapPath("~/robots.txt");
            try
            {
                using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
                {
                    sw.Write(FileContent);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            ModelState.AddModelError("", "Данные сохранены");
            ViewBag.FileContent = FileContent;
            return View();
        }

        [HttpGet]
        [AuthorizeMaster]
        [MenuItem("Расписание цен", 980, 99, Icon = "book_alt")]
        public ActionResult Index()
        {
            ViewBag.ContainerStyle = " dashboard dashboard-widget-group";
            return
                View(db.EventCalendars.ToList()
                       .GroupBy(x => x.DayOfWeek)
                       .ToDictionary(x => x.Key, x => x.OrderBy(z => z.StartTime)));
        }


        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Edit(int? ID, int? DayOfWeek)
        {
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            var @event = new EventCalendar() { DayOfWeek = DayOfWeek ?? 0, Direction = 1, ShowDiscount = true };
            if (ID.HasValue && ID > 0)
            {
                @event = db.EventCalendars.FirstOrDefault(x => x.ID == ID);
            }
            if (@event == null)
                return RedirectToAction("Index");

            return View(@event);
        }
        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Edit(int? ID, FormCollection collection)
        {
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            var @event = new EventCalendar() { DayOfWeek = 0, Direction = 1, ShowDiscount = true };
            if (ID.HasValue && ID > 0)
            {
                @event = db.EventCalendars.FirstOrDefault(x => x.ID == ID);
            }
            else
            {
                db.EventCalendars.InsertOnSubmit(@event);
            }
            if (@event == null)
                return RedirectToAction("Index");

            

            try
            {
                UpdateModel(@event);

                db.SubmitChanges();
                ModelState.AddModelError("", "Данные сохранены");
                if (!ID.HasValue || ID == 0)
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(@event);
        }


        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Delete(int? ID)
        {
            var page = db.EventCalendars.FirstOrDefault(x => x.ID == ID);
            if (page == null)
                return RedirectToAction("Index");

            return View(page);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Delete(int? ID, FormCollection collection)
        {
            var page = db.EventCalendars.FirstOrDefault(x => x.ID == ID);
            if (page == null)
                return RedirectToAction("Index");
            db.EventCalendars.DeleteOnSubmit(page);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
