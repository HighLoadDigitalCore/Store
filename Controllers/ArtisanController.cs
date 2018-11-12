using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BotDetect.Web.UI.Mvc;
using NPOI.SS.Formula.Functions;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class ArtisanController : Controller
    {
        private DB db = new DB();

        [ClientTemplate("Личный кабинет мастера")]
        [AuthorizeClient]
        public ActionResult Cabinet()
        {
            if (!Request.IsAuthenticated)
                return Redirect("/");


            var p = db.UserProfiles.FirstOrDefault(x => x.UserID == (Guid)Membership.GetUser().ProviderUserKey);

            return PartialView(p);
        }

        [HttpGet]
        [ClientTemplate("Профиль мастера")]
        public ActionResult Form(Guid? userID)
        {
            var profile = new UserProfile()
            {
                UserID = new Guid()
            };

            if (userID.HasValue)
            {
                var p = db.UserProfiles.FirstOrDefault(x => x.UserID == userID);
                if (p != null)
                    profile = p;
            }


            return PartialView(profile);
        }

        [HttpPost]
        public ActionResult UploadAvatar(Guid uid)
        {
            HttpPostedFileBase myFile = Request.Files.Count > 0 ? Request.Files[0] : null;
            bool isUploaded = false;
            string message = "Невозможно загрузить изображение";
            var fileName = "";
            var path = "";

            var usr = Membership.GetUser();
            if (uid.IsEmpty() && myFile != null && myFile.ContentLength != 0)
            {
                myFile.SaveAs(Server.MapPath("/content/temp/" + uid + ".jpg"));
                path = "/content/temp/" + uid + ".jpg?rnd=" + new Random(DateTime.Now.Millisecond).Next();
                isUploaded = true;
            }
            else if (myFile != null && myFile.ContentLength != 0 && usr != null)
            {
                var p = db.UserProfiles.FirstOrDefault(x => x.UserID == uid);
                p.Avatar = myFile.InputStream.ToBinary();
                path = p.GetAvatarLink(100) + "&rnd=" + new Random(DateTime.Now.Millisecond).Next();
                db.SubmitChanges();
                isUploaded = true;
            }

            return
                    Json(
                        new
                        {
                            isUploaded = isUploaded,
                            message = message,
                            path = path,
                        });


        }

        [HttpPost]
        public ActionResult RateUser(Guid user, int rate)
        {
            var ip = Request.GetRequestIP().ToIPInt() ?? 0;
            var exist =
                db.UserRates.Where(x => x.Date.Date == DateTime.Now.Date && x.IPAddress == ip && x.UserID == user);

            if (exist.Any())
                return new ContentResult() {Content = "Вы уже проголосовали"};


            db.UserRates.InsertOnSubmit(new UserRate()
            {
                UserID = user,
                Date = DateTime.Now,
                IPAddress = ip,
                Rate = rate
            });
            db.SubmitChanges();
            return new ContentResult();

        }

        [HttpGet]
        [AuthorizeClient]
        [ClientTemplate("Страница мастера")]
        public ActionResult Profile(Guid? uid)
        {

            if (!uid.HasValue)
                return PartialView();

            var m = db.UserProfiles.FirstOrDefault(x => x.UserID == uid);
            if (m != null)
            {
//                db.SubmitChanges();
            }
            return PartialView(m);
        }


        [HttpGet]
        public ActionResult Ask()
        {
            return PartialView();
        }


        [HttpPost]
        [CaptchaValidation("CaptchaCode", "SampleCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Ask(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Заполните правильно все поля формы";
            }
            return PartialView();
        }


        [HttpPost]
        public ActionResult AddView(Guid userID)
        {
            var m = db.UserProfiles.FirstOrDefault(x => x.UserID == userID);
            if (m != null)
            {
                m.ViewCount++;
                db.SubmitChanges();
            }
            return new ContentResult();
        }


        [HttpGet]
        [AuthorizeClient]
        public ActionResult Photo()
        {
            ViewBag.UserID = (Guid)Membership.GetUser().ProviderUserKey;
            return PartialView();
        }

        [HttpPost]
        public ActionResult DeletePhoto(int id)
        {
            var photo = db.UserImages.FirstOrDefault(x => x.ID == id);
            if (photo != null)
            {
                db.UserImages.DeleteOnSubmit(photo);
                db.SubmitChanges();
            }
            return new ContentResult();
        }


        [HttpPost]
        public ActionResult Photo(HttpPostedFileBase Image)
        {
            ViewBag.UserID = (Guid)Membership.GetUser().ProviderUserKey;
            if (Image == null)
            {
                ViewBag.Error = "Необходимо выбрать файл";
                //return PartialView("Photo");
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("/content/artisan/" + ViewBag.UserID)))
                {
                    Directory.CreateDirectory(Server.MapPath("/content/artisan/" + ViewBag.UserID));
                }
                var path = "/content/artisan/" + ViewBag.UserID + "/" + Image.FileName;
                Image.SaveAs(Server.MapPath(path));

                var img = new UserImage()
                {
                    CreateDate = DateTime.Now,
                    Name = "",
                    Path = path,
                    UserID = (Guid) ViewBag.UserID
                };

                db.UserImages.InsertOnSubmit(img);
                db.SubmitChanges();

            }

            return Redirect("/cabmaster?view=photo");
        }


        [HttpGet]
        [ClientTemplate("Список мастеров")]
        public ActionResult List(string town, int? page)
        {

            var profs = db.UserProfiles.Where(x => x.IsMaster);
            if (town.IsFilled())
            {
                profs = profs.Where(x => x.Region == town /*SqlMethods.Like(x.Town, "%" + town + "%")*/);
            }
            ViewBag.Town = town;
            var dict = new RouteValueDictionary();
            if(town.IsFilled())
                dict.Add("town", town);
            return PartialView(new PagedData<UserProfile>(profs, page ?? 0, 20, dict));
        }


        [HttpPost]
        public ActionResult Form(Guid userID, UserProfile p, FormCollection collection)
        {
            var redirect = false;

            var profile = new UserProfile()
            {
                UserID = new Guid()
            };

            if (p.Name.IsNullOrEmpty() || p.Surname.IsNullOrEmpty() || p.Email.IsNullOrEmpty() ||
                !p.Email.IsMailAdress() || p.Password.IsNullOrEmpty() || p.Password.Length < 6 ||
                (p.Password != collection["PasswordRepeat"] && userID.IsEmpty()))
            {
                ModelState.AddModelError("", "Необходимо правильно заполнить все поля формы");
                return PartialView(p);
            }

            if (userID.IsEmpty())
            {
                try
                {

                    var user = Membership.CreateUser(p.Email, p.Password, p.Email);

                    Roles.AddUserToRole(user.UserName, "Client");

                    userID = (Guid?)user.ProviderUserKey ?? new Guid();

                    profile.UserID = userID;
                    profile.FromIP = HttpContext.Request.GetRequestIP().ToIPInt();
                    profile.RegDate = DateTime.Now;
                    profile.Patrinomic = "";
                    profile.Avatar = new byte[0];

                    if (collection["AV"] == "1")
                    {
                        var pp = Server.MapPath("/content/temp/" + new Guid() + ".jpg");
                        if (System.IO.File.Exists(pp))
                        {
                            using (var fs = new FileStream(pp, FileMode.Open))
                            {
                                var buf = new byte[fs.Length];
                                fs.Read(buf, 0, (int)fs.Length);
                                profile.Avatar = buf;
                            }
                        }
                    }

                    db.UserProfiles.InsertOnSubmit(profile);
                    redirect = true;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    return Form(userID);
                }
            }
            else
            {
                profile = db.UserProfiles.FirstOrDefault(x => x.UserID == userID);
            }

            if (userID.IsEmpty())
            {
                ModelState.AddModelError("", "Ошибка сохранения данных");
                return Form(userID);
            }





            profile.LoadPossibleProperties(p, new[] { "UserID", "FromIP", "RegDate", "Patrinomic", "Avatar" });
            profile.IsMaster = true;

            db.SubmitChanges();


            if (!Request.IsAuthenticated || redirect)
            {
                FormsAuthentication.SetAuthCookie(profile.MembershipUser.Email, true);
            }


            if (redirect)
            {
                ViewBag.Redirect = "/cabmaster?view=first";
                //Redirect("/cabmaster?first=true");
            }
            ModelState.AddModelError("", "Данные успешно сохранены");
            return PartialView(profile);



        }

    }
}
