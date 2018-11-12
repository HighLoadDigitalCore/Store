using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Xml.Linq;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class CabinetController : Controller
    {
        private DB db = new DB();

        [HttpGet]
        [Authorize]
        public ActionResult ProfileFavorite()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AvatarDelete()
        {
            var user = db.UserProfiles.FirstOrDefault(x => x.UserID == HttpContext.GetCurrentUserUID());
            if (user == null)
            {
                return  new ContentResult(){Content = "fail"};
            }
            user.Avatar = null;
            db.SubmitChanges();
            return new ContentResult(){Content = user.GetAvatarLink(40)};
        }

        [HttpPost]
        public ActionResult ProfileAdressesDelete(int ID)
        {
            var a = db.OrderAdresses.FirstOrDefault(x => x.ID == ID);
            if (a != null)
            {
                db.OrderAdresses.DeleteOnSubmit(a);
                db.SubmitChanges();
            }
            return PartialView("ProfileAdresses");
        }

        [AuthorizeClient] 
        [ClientTemplate("Личный кабинет")]
        public PartialViewResult Index()
        {
            var p = db.UserProfiles.FirstOrDefault(x => x.UserID == (Guid) Membership.GetUser().ProviderUserKey);
            if (p!=null && p.IsMaster)
            {
                Response.Redirect("/cabmaster?view=profile");
            }

            return PartialView(new CabinetMenu());
        }

        [HttpPost]
        public PartialViewResult ProfileProfile(ProfileEdit profileEdit)
        {
            var user = db.UserProfiles.FirstOrDefault(x => x.UserID == HttpContext.GetCurrentUserUID());
            if (user != null)
            {
                var names = profileEdit.Name.Split<string>(" ").ToList();
                if (names.Count() > 1)
                {
                    user.Patrinomic = names.ElementAt(1);
                }
                user.Name = names.ElementAt(0);
                user.Surname = profileEdit.Surname;
                user.MobilePhone = profileEdit.Phone;
                if (user.Email != profileEdit.Email)
                {
                    user.MembershipUser.Email = profileEdit.Email;
                }
                if (profileEdit.OldPassword.IsFilled())
                {
                    if (profileEdit.NewPassword != profileEdit.NewPasswordConfirm)
                    {
                        ViewBag.Message = "Новый пароль и подтверждение пароля не совпадают";
                    }
                    else if (!profileEdit.NewPassword.IsFilled() || profileEdit.NewPassword.Length < 6)
                    {
                        ViewBag.Message = "Длина пароля должна быть не менее 6 символов";
                    }
                    else
                    {
                        if (!user.MembershipUser.ChangePassword(profileEdit.OldPassword, profileEdit.NewPassword))
                        {
                            ViewBag.Message = "Старый пароль указан неверно";
                        }
                    }
                }
            }
            if (ViewBag.Message == null)
            {
                ViewBag.Message = "Данные сохранены";
            }
            db.SubmitChanges();
            return PartialView();
        }

        [HttpGet]
        [AuthorizeClient]
        public PartialViewResult Common()
        {
            var order =
                db.Orders.Where(x => x.UserID == (Guid)Membership.GetUser().ProviderUserKey)
                  .OrderByDescending(x => x.CreateDate);
            var last = order.FirstOrDefault();
            if (last != null)
            {
                ViewBag.OrderNum = last.ID.ToString();
                ViewBag.Header = "Ваш последний заказ №S" + last.ID.ToString("d9");
            }
            return PartialView();
        }

 
        [HttpPost]
        [AuthorizeClient]
        public PartialViewResult Details(int? id, string header, string Message, FormCollection collection)
        {

            if (header.IsFilled())
            {
                ViewBag.Header = header;
            }
            var u = db.Orders.FirstOrDefault(x => x.ID == id);
            if (u == null)
            {
                ViewBag.Message = "Заказ не найден.";
                return Details(id, header);
            }
            if (Message.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Необходимо заполнить поле для сообщения.");
                return Details(id, header);
            }
            var now = DateTime.Now;
            now = now.Subtract(new TimeSpan(0, 0, 0, 0, now.Millisecond));
            var comment = new OrderComment() { Author = null, Comment = Message, Date = now, OrderID = u.ID };
            db.OrderComments.InsertOnSubmit(comment);
            db.SubmitChanges();

            return Details(id, header);
        }


        [HttpGet]
        [AuthorizeClient]
        public PartialViewResult Details(int? id, string header)
        {
            var u = db.Orders.FirstOrDefault(x => x.ID == id);
            if (header.IsFilled())
            {
                ViewBag.Header = header;
            }
            else if (u != null)
            {
                ViewBag.Header = "Информация о заказе №S" + u.ID.ToString("d9");
            }

            if (u == null)
            {
                ViewBag.Message = "Заказ не найден.";
                return PartialView(u);
            }
            if (Request.HttpMethod == "GET")
            {
                try
                {
                    XDocument data = XDocument.Load("http://www.sprinter.ru/obmen/?order_id=" + id);
                    //XDocument data = XDocument.Load("http://www.sprinter.ru/obmen/?order_id=1347276765");
                    var status = data.Descendants("status").ToList();
                    if (status.Any())
                    {
                        u.StatusID = OrderStatus.GetStatusIDByRUS(status.First().Value);
                    }
                    var comments = data.Descendants("comment").ToList();
                    if (comments.Any())
                    {
                        foreach (XElement comment in comments)
                        {
                            DateTime date;
                            if (DateTime.TryParseExact(comment.Attribute("date").Value, "yyyy-MM-dd HH:mm:ss",
                                                       CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                            {
                                var dbc = db.OrderComments.FirstOrDefault(x => x.OrderID == u.ID && x.Date == date);
                                if (dbc == null)
                                {
                                    dbc = new OrderComment()
                                        {
                                            Author = comment.Attribute("name").Value,
                                            Comment = comment.Value,
                                            OrderID = u.ID,
                                            Date = date
                                        };
                                    db.OrderComments.InsertOnSubmit(dbc);
                                }
                            }
                        }
                    }
                    db.SubmitChanges();

                }
                catch
                {

                }
            }
            string message = "";
            message = "<h3>Статус заказа:</h3>" + u.OrderStatus.Status + "<br><br>";

            message += "<h3>Заказанные товары:</h3><br><table>";
            message += "<tr><td><b>Название</b></td><td><b>Количество</b></td><td><b>Цена</b></td></tr>";
            message += string.Join("",
                                   u.OrderedProducts.Select(
                                       x =>
                                       "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>".FormatWith(
                                           x.StoreProduct.Name, x.Amount.ToString(),
                                           x.Sum.ForDisplaing())));
            message += "<tr><td colspan=\"3\"><b>Итого к оплате &nbsp;&mdash;&nbsp;" +
                        u.TotalSum + " руб.</b></td></tr> </table><br><br>";
            /*
                        message += "<h3>Информация о покупателе:</h3><br>";
                        message += ShopCart.TranslateToHtml(u.UserData, "UserData");
                        var orgData = u.OrderDetail.OrgData;
                        if (!string.IsNullOrEmpty(orgData))
                        {
                            message += "<h3>Информация о юридическом лице:</h3><br>";
                            message += ShopCart.TranslateToHtml(orgData, "OrgData");
                        }
            */
            if (u.OrderDetail.OrderDeliveryRegion != null)
            {
                message +=
                    "<h3>Информация о доставке:</h3><br><b>{0}, {1}, стоимость - {2} руб.</b><br>".FormatWith(
                        u.OrderDetail.OrderDeliveryRegion.OrderDeliveryProvider.Name,
                        u.OrderDetail.OrderDeliveryRegion.Name, u.OrderDetail.DeliveryCost.ForDisplaing());
            }
            message += ShopCart.TranslateToHtml(u.OrderDetail.Address ?? "", "Address");
            ViewBag.Data = message;
            return PartialView(u);
        }

        [HttpGet]
        [AuthorizeClient]
        public PartialViewResult Orders()
        {
            return PartialView(Membership.GetUser().UserEntity().Orders.OrderByDescending(x => x.CreateDate));
        }

        [HttpGet]
        public PartialViewResult UpperNav()
        {
            return PartialView();
        }  
        
        [HttpGet]
        public PartialViewResult HeadCount()
        {
            return PartialView();
        }
    }
}
