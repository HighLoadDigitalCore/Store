using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class FormsController : Controller
    {
        [HttpGet]
        public PartialViewResult FastOrderPopup()
        {
            var model = new FastOrderModel() { Type = "Физическое лицо" };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var db = new DB();
                var user = db.Users.FirstOrDefault(x => x.UserId == HttpContext.GetCurrentUserUID());
                if (user != null)
                {
                    model.Email = user.MembershipData.Email;
                    model.EmailA = user.MembershipData.Email;
                    model.OrgMail = user.MembershipData.Email;
                    model.OrgMailB = user.MembershipData.Email;

                    model.FullName = user.Profile.FullName;
                    model.FullNameA = user.Profile.FullName;
                    model.OrgPerson = user.Profile.FullName;
                    model.OrgPersonB = user.Profile.FullName;

                    model.Phone = user.Phone;
                    model.PhoneA = user.Phone;
                    model.OrgPhone = user.Phone;
                    model.OrgPhoneB = user.Phone;
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult FastOrderPopup(FastOrderModel form)
        {
            if (form.Type == "Физическое лицо")
            {
                if (form.FullName.IsNullOrEmpty() || form.Address.IsNullOrEmpty() || form.Phone.IsNullOrEmpty() || form.Email.IsNullOrEmpty())
                {
                    form.ErrorText = "Необходимо заполнить все поля, отмеченные *";
                    return PartialView(form);
                }
            }
            else if (form.Type == "Юридическое лицо")
            {
                if (form.OrgName.IsNullOrEmpty() || form.OrgPerson.IsNullOrEmpty() || form.OrgPhone.IsNullOrEmpty() || form.OrgMail.IsNullOrEmpty() || form.OrgINN.IsNullOrEmpty() || form.OrgKorr.IsNullOrEmpty() || form.OrgAccount.IsNullOrEmpty() || form.OrgBankName.IsNullOrEmpty() || form.OrgBik.IsNullOrEmpty() || form.OrgJurAddr.IsNullOrEmpty() || form.OrgFaktAddr.IsNullOrEmpty() || form.OrgDelivAddr.IsNullOrEmpty() || form.OrgMail.IsNullOrEmpty())
                {
                    form.ErrorText = "Необходимо заполнить все поля, отмеченные *";
                    return PartialView(form);
                }
            }
            else if (form.Type == "Регионы РФ:Физическое лицо")
            {
                if (form.FullNameA.IsNullOrEmpty() || form.AddressA.IsNullOrEmpty() || form.PhoneA.IsNullOrEmpty() || form.TargetFullNameA.IsNullOrEmpty() || form.TargetPassA.IsNullOrEmpty() || form.TargetAddressA.IsNullOrEmpty() || form.EmailA.IsNullOrEmpty())
                {
                    form.ErrorText = "Необходимо заполнить все поля, отмеченные *";
                    return PartialView(form);
                }
            }
            else
            {
                if (form.OrgNameB.IsNullOrEmpty() || form.OrgPersonB.IsNullOrEmpty() || form.OrgPhoneB.IsNullOrEmpty() || form.OrgMailB.IsNullOrEmpty() || form.OrgINNB.IsNullOrEmpty() || form.OrgKorrB.IsNullOrEmpty() || form.OrgAccountB.IsNullOrEmpty() || form.OrgBankNameB.IsNullOrEmpty() || form.OrgBikB.IsNullOrEmpty() || form.OrgJurAddrB.IsNullOrEmpty() || form.OrgFaktAddrB.IsNullOrEmpty() || form.OrgDelivAddrB.IsNullOrEmpty() || form.OrgTargetPersonB.IsNullOrEmpty() || form.OrgTargetPassB.IsNullOrEmpty() || form.OrgMailB.IsNullOrEmpty())
                {
                    form.ErrorText = "Необходимо заполнить все поля, отмеченные *";
                    return PartialView(form);
                }
            }

            var cart = new ShopCart().InitCart();
            var order = cart.CreateOrder(form);
            if (order == null)
            {
                form.ErrorText = "Для оформления заказа необходимо пройти авторизацию или зарегистрироваться";
                return PartialView(form);
            }
            cart.SendLetters(order, form);
            cart.Clear();
            cart.Reset();

            form.ErrorText = "<script type=\"text/javascript\">document.location.href = '/order?step=final'</script>";

            return PartialView(form);
        }


        [HttpGet]
        public PartialViewResult FeedBackPopup()
        {
            var model = new FeedBackModel() { Type = "offer" };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var db = new DB();
                var user = db.Users.FirstOrDefault(x => x.UserId == HttpContext.GetCurrentUserUID());
                if (user != null)
                {
                    model.Mail = user.MembershipData.Email;
                    model.Name = user.Profile.FullName;
                }
            }
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult FeedBackPopupV2()
        {
            var model = new FeedBackModel() { Type = "offer" };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var db = new DB();
                var user = db.Users.FirstOrDefault(x => x.UserId == HttpContext.GetCurrentUserUID());
                if (user != null)
                {
                    model.Mail = user.MembershipData.Email;
                    model.Name = user.Profile.FullName;
                    model.Phone = user.Profile.MobilePhone;
                }
            }
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult BackCallPopup()
        {
            var model = new BackCallModel() { Where = "Как можно быстрее" };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var db = new DB();
                var user = db.Users.FirstOrDefault(x => x.UserId == HttpContext.GetCurrentUserUID());
                if (user != null)
                {
                    model.Name = user.Profile.FullName;
                    model.Phone = user.Phone;
                }
            }
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult CalcPopup(bool hasMail = true, string url = "")
        {
            var model = new CalcModel() {HasMail = hasMail, FromProduct = url};
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var db = new DB();
                var user = db.Users.FirstOrDefault(x => x.UserId == HttpContext.GetCurrentUserUID());
                if (user != null)
                {
                    model.Name = user.Profile.FullName;
                    model.Phone = user.Phone;
                    model.Mail = user.Profile.Email;
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public virtual ActionResult UploadFormFile()
        {
            HttpPostedFileBase myFile = Request.Files.Count > 0 ? Request.Files[0] : null;
            bool isUploaded = false;
            string message = "";
            var fileName = "";
            try
            {
                if (myFile != null && myFile.ContentLength != 0)
                {
                    var path = "/content/temp/" + Path.GetFileName(myFile.FileName);
                    myFile.SaveAs(Server.MapPath(path));
                    fileName = path;
                    isUploaded = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return
                 Json(
                     new
                     {
                         isUploaded,
                         message,
                         path = fileName,
                     });





        }

        [HttpPost]
        public PartialViewResult CalcPopup(CalcModel form)
        {

            if (form.Name.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо указать ваше имя";
                return PartialView(form);
            }
            if (form.Phone.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо указать ваш телефон";
                return PartialView(form);
            }
            if (form.Mail.IsNullOrEmpty() && form.HasMail)
            {
                form.ErrorText = "Необходимо указать вашу эл. почту";
                return PartialView(form);
            }

            var replacements = new List<MailReplacement>();
            replacements.Add(new MailReplacement("{HEAD}", form.HasMail ? "на расчет":"на быстрый заказ"));
            replacements.Add(new MailReplacement("{NAME}", form.Name));
            replacements.Add(new MailReplacement("{PHONE}", form.Phone));
            replacements.Add(new MailReplacement("{MAIL}", form.Mail));
            replacements.Add(new MailReplacement("{FILE}",
                form.File.IsNullOrEmpty()
                    ? ""
                    : (HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + form.File)));
            replacements.Add(new MailReplacement("{LINK}", form.FromProduct));

            string result = MailingList.Get("CalcLetter")
                                       .WithReplacements(replacements)
                                       .Send();
            if (result.IsNullOrEmpty())
            {
                form.ErrorText =
                    "Ваше сообщение успешно отправлено.";

                
            }
            else
            {

                form.ErrorText = "Произошла ошибка при отправке сообщения. Попробуйте повторить попытку позже" + "<br>" +
                                 result;
            }

            form.NeedClose = true;

            return PartialView(form);
        }



        [HttpPost]
        public PartialViewResult BackCallPopup(BackCallModel form)
        {

            if (form.Name.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо указать ваше имя";
                return PartialView(form);
            }
            if (form.Phone.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо указать ваш телефон";
                return PartialView(form);
            }

            var replacements = new List<MailReplacement>();
            replacements.Add(new MailReplacement("{NAME}", form.Name));
            replacements.Add(new MailReplacement("{PHONE}", form.Phone));
            replacements.Add(new MailReplacement("{WHERE}", form.Where));
            replacements.Add(new MailReplacement("{COMMENT}", form.Comment));

            string result = MailingList.Get("BackCallLetter")
                                       .WithReplacements(replacements)
                                       .Send();
            if (result.IsNullOrEmpty())
            {
                form.ErrorText =
                    "Ваше сообщение успешно отправлено.";
            }
            else
            {
                form.ErrorText = "Произошла ошибка при отправке сообщения. Попробуйте повторить попытку позже" + "<br>" +
                                 result;
            }
            return PartialView(form);
        }


        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult FeedBackPopup(FeedBackModel form)
        {
            if (form.Mail.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо указать E-mail";
                return PartialView(form);
            }
            if (form.Text.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо заполнить текст сообщения";
                return PartialView(form);
            }

            var replacements = new List<MailReplacement>();
            replacements.Add(new MailReplacement("{TYPE}", form.TypeName));
            replacements.Add(new MailReplacement("{MAIL}", form.Mail));
            replacements.Add(new MailReplacement("{NAME}", form.Name));
            replacements.Add(new MailReplacement("{TEXT}", form.Text));

            string result = MailingList.Get("FeedBackLetter")
                                       .WithReplacements(replacements)
                                       .Send();
            if (result.IsNullOrEmpty())
            {
                form.ErrorText =
                    "Ваше сообщение успешно отправлено.";
            }
            else
            {
                form.ErrorText = "Произошла ошибка при отправке сообщения. Попробуйте повторить попытку позже";
            }
            return PartialView(form);
        }


        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult FeedBackPopupV2(FeedBackModel form)
        {
            if (form.Mail.IsNullOrEmpty() && form.Phone.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо указать E-mail или телефон";
                return PartialView(form);
            }
            if (form.Text.IsNullOrEmpty())
            {
                form.ErrorText = "Необходимо заполнить текст сообщения";
                return PartialView(form);
            }

            var replacements = new List<MailReplacement>();
            replacements.Add(new MailReplacement("{ORDER}", form.Order));
            replacements.Add(new MailReplacement("{MAIL}", form.Mail));
            replacements.Add(new MailReplacement("{NAME}", form.Name));
            replacements.Add(new MailReplacement("{PHONE}", form.Phone));
            replacements.Add(new MailReplacement("{TEXT}", form.Text));

            string result = MailingList.Get("FeedBackLetterV2")
                                       .WithReplacements(replacements)
                                       .Send();
            if (result.IsNullOrEmpty())
            {
                form.ErrorText =
                    "Ваше сообщение успешно отправлено.";
                form.Sent = true;
            }
            else
            {
                form.ErrorText = "Произошла ошибка при отправке сообщения. Попробуйте повторить попытку позже";
            }
            return PartialView(form);
        }

    }
}
