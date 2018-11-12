using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Smoking.Models;
using Smoking.Extensions;

namespace Smoking.Controllers
{
    public class OrdersController : Controller
    {
        DB db = new DB();
        [AuthorizeMaster]
        [HttpGet]
        [MenuItem("Заказы", 15, Icon = "map_pin_stroke")]
        public ActionResult Index(string query, int? page, int? status)
        {
            var orders = db.Users.AsQueryable();

            if (!status.HasValue)
            {
                status = db.OrderStatus.First(x => x.EngName == "Accepted").ID;
            }

            

            if (!query.IsNullOrEmpty())
            {
                query = "%{0}%".FormatWith(query);
                orders =
                    orders.Where(
                        x =>
                        SqlMethods.Like(x.UserName.ToLower(), query.ToLower()) ||
                        SqlMethods.Like(x.MembershipData.Email.ToLower(), query.ToLower()) ||
                        SqlMethods.Like(x.UserProfile.Name.ToLower(), query.ToLower()) ||
                        SqlMethods.Like(x.UserProfile.Patrinomic.ToLower(), query.ToLower()) ||
                        SqlMethods.Like(x.UserProfile.Surname.ToLower(), query.ToLower()));
            }

            if (((page ?? 0 + 1) * 50) > orders.Count())
                page = 0;

            return
                View(new PagedData<Order>(orders.SelectMany(x => x.Orders).Where(x=> x.StatusID == status).OrderByDescending(x => x.CreateDate),
                                          page ?? 0, 50, "Master",
                                          new RouteValueDictionary(
                                              new {query = (query ?? "").Replace("%", ""), page = page,})));
        }

        [AuthorizeMaster]
        [HttpPost]
        public ActionResult Index( string query, int? page, int? status, FormCollection collection)
        {
            return RedirectToAction("Index", new {query = query, page = page, status = status});
        }

        [AuthorizeMaster]
        [HttpGet]
        public ActionResult Delete(int? order, string query, int? page)
        {
            var u = db.Orders.FirstOrDefault(x => x.ID == order);
            if (u == null) return RedirectToAction("Index", new { query = query, page = page});
            return View(u);
        }

        [AuthorizeMaster]
        [HttpPost]
        public ActionResult Delete(int? order, string query, int? page, FormCollection collection)
        {
            var u = db.Orders.FirstOrDefault(x => x.ID == order);
            if (u == null) return RedirectToAction("Index", new { query = query, page = page});
            db.Orders.DeleteOnSubmit(u);
            db.SubmitChanges();
            return RedirectToAction("Index", new { query = query, page = page});
        }

        [AuthorizeMaster]
        [HttpGet]
        public ActionResult Edit(int? order, string query, int? page)
        {
            var u = db.Orders.FirstOrDefault(x => x.ID == order);
            if (u == null) return RedirectToAction("Index", new { query = query, page = page });
            return View(u);
        }
        [AuthorizeMaster]
        [HttpPost]
        public ActionResult Edit(int? order, string query, int? page, FormCollection collection)
        {
            var u = db.Orders.FirstOrDefault(x => x.ID == order);
            if (u == null) return RedirectToAction("Index", new { query = query, page = page });
            u.StatusID = (int)collection.GetValue("StatusID").ConvertTo(typeof (int));
            db.SubmitChanges();
            ModelState.AddModelError("", "Данные сохранены");
            return View(u);
        }

    }
}
