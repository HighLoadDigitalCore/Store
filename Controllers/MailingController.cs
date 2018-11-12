using System.Linq;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{

    public class MailingController : Controller
    {
        private readonly DB db = new DB();


/*
        [AuthorizeMaster]
        [MenuItem("Настройка почты", 80, Icon = "mail")]
*/
        public ActionResult IndexList()
        {
            return View();
        }

        [AuthorizeMaster]
        [MenuItem("Почта", 83, 99, Icon = "email")]
        public ActionResult MailSettings()
        {
            var list = db.SiteSettings.Where(x => x.GroupName == "Настройки почтового сервера").OrderBy(x => x.OrderNum);
            foreach (var setting in list)
            {
                ViewData.Add(setting.Setting, setting.oValue);
            }
            ViewBag.TitleBlock = "Почта";
            return View("../Settings/Index", list);
            
        }


        [HttpPost, AuthorizeMaster, ValidateInput(false)]
        public ActionResult MailSettings(FormCollection collection)
        {
            var groups = SettingsController.SaveSettings(collection, ModelState, db);
            ModelState.AddModelError("", "Данные сохранены");
            return MailSettings();
        }

        [AuthorizeMaster]
        [HttpGet]
        [MenuItem("Шаблоны писем", 225, 90, Icon = "mail")]
        public ActionResult Index(int? mailingID)
        {
            var mailings =
                db.MailingLists.Where(x => x.Enabled).OrderBy(x => x.Name).AsEnumerable().ToList();
            mailings.Insert(0, new MailingList {ID = 0, Name = "--Выберите рассылку в списке--"});
            ViewBag.Mailings = new SelectList(mailings, "ID", "Name", mailingID ?? 0);
            var current = db.MailingLists.FirstOrDefault(x => x.ID == mailingID);
            return View(current);
        }

        [AuthorizeMaster]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(MailingList mailing, int? mailingID)
        {
            var mailings =
                db.MailingLists.Where(x=> x.Enabled).OrderBy(x=> x.Name).AsEnumerable().ToList();
            mailings.Insert(0, new MailingList { ID = 0, Name = "--Выберите рассылку в списке--" });
            ViewBag.Mailings = new SelectList(mailings, "ID", "Name", mailingID ?? 0);

            var mail = db.MailingLists.FirstOrDefault(x => x.ID == mailing.ID);
            if (mail == null)
            {
                ModelState.AddModelError("", "Ошибка сохранения! Рассылка не найдена в БД");
                return View(mailing);
            }
            mail.Header = mailing.Header;
            mail.Letter = mailing.Letter;
            mail.TargetMail = mailing.TargetMail;

            if (!mail.MailingReplacements.All(x => mailing.Letter.IndexOf(x.Replacement) >= 0))
            {
                ModelState.AddModelError("", "Ошибка! Данные не сохранены - Все подстановки из списка должны быть использованы в тексте письма");
                return View(mail);
            }

            db.SubmitChanges();

            ModelState.AddModelError("", "Данные сохранены");
            return View(mail);
        }

    }
}
