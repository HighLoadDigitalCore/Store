using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class ChatController : Controller
    {
        DB db = new DB();
        xChatDB cDB = new xChatDB();

        [MenuItem("Чат", 70, Icon = "mic")]
        [HttpGet]
        [AuthorizeMaster]
        public ActionResult IndexList()
        {
            return View();
        }


        [MenuItem("Список сообщений", 780, 70, Icon = "listchat")]
        [AuthorizeMaster]
        public ActionResult Edit()
        {

            return View(GetEditorFromQuery());
        }

        [AuthorizeMaster]
        [HttpGet]
        public PartialViewResult Editor()
        {

            return PartialView(GetEditorFromQuery());
        }

        private ChatEditor GetEditorFromQuery()
        {
            var editor = new ChatEditor(true);
            if (Request.QueryString.AllKeys.Contains("IsArchive"))
                editor.IsArchive = Request.QueryString["IsArchive"].ToBool();

            if (Request.QueryString.AllKeys.Contains("StartDate"))
                editor.StartDate = Request.QueryString["StartDate"].ToDate("dd.MM.yyyy HH:mm");
            if (Request.QueryString.AllKeys.Contains("EndDate"))
                editor.EndDate = Request.QueryString["EndDate"].ToDate("dd.MM.yyyy HH:mm");

            if (Request.QueryString.AllKeys.Contains("ChatID"))
                editor.ChatID = Request.QueryString["ChatID"].ToNullInt();
            editor.InitProps();
            return editor;

        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult UnreadCount()
        {
            return new ContentResult() { Content = cDB.ChatMessages.Count(x => !x.IsRead).ToString() };
        }

        [HttpPost]
        [AuthorizeMaster]
        public PartialViewResult Editor(FormCollection collection)
        {

            var editor = new ChatEditor(true);
            editor.IsPost = true;
            editor.IsArchive = collection["IsArchive"].ToBool();
            editor.StartDate = collection["StartDate"].ToDate("dd.MM.yyyy HH:mm");
            editor.EndDate = collection["EndDate"].ToDate("dd.MM.yyyy HH:mm");
            editor.InitProps();
            return PartialView(editor);
        }

        public JsonResult PostMessage(Guid session, string host, string message = "", string un = "", string um = "", string uf = "")
        {

            var chat = cDB.Chats.FirstOrDefault(x => x.ChatUID == session);
            if (message.IsFilled())
            {
                if (chat == null)
                {
                    chat = new Chat { ChatUID = session, Host = host, StartDate = DateTime.Now };
                    cDB.Chats.InsertOnSubmit(chat);
                    SendNotification(message, un, um, uf);
                }
                chat.IsClosed = false;
                Guid? userID = HttpContext.GetCurrentUserUID();
                if (db.Users.FirstOrDefault(x => x.UserId == userID) == null)
                {
                    userID = null;
                }

                if (userID == null && un != "" && um != "" && uf != "")
                {
                    var users = Membership.FindUsersByEmail(um);

                    if (users.Count > 0)
                    {
                        if (Roles.IsUserInRole(um, "Client"))
                        {
                            FormsAuthentication.SetAuthCookie(um, true);
                            foreach (MembershipUser user in users)
                            {
                                userID = (Guid?) user.ProviderUserKey;
                                break;
                            }

                        }
                    }
                    else
                    {
                        MembershipCreateStatus createStatus;
                        var pass = new Random(DateTime.Now.Millisecond).GeneratePassword(6);
                        var user = Membership.CreateUser(um, pass, um, null, null, true,
                            null, out createStatus);
                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            userID = (Guid?) user.ProviderUserKey;
                            Roles.AddUserToRole(user.UserName, "Client");
                            string name = un;
                            string surn = "";
                            if (name.Trim().Contains(" "))
                            {
                                var arr = name.Split<string>(" ").ToList();
                                if (arr.Count() == 2)
                                {
                                    name = arr.ElementAt(0);
                                    surn = arr.ElementAt(1);
                                }
                            }
                            var profile = new UserProfile()
                            {
                                UserID = (Guid) user.ProviderUserKey,
                                FromIP = HttpContext.Request.GetRequestIP().ToIPInt(),
                                RegDate = DateTime.Now,
                                Email = um,
                                Name = name,
                                Patrinomic = "",
                                Surname = surn,
                                MobilePhone = uf
                            };

                            db.UserProfiles.InsertOnSubmit(profile);
                            db.SubmitChanges();
                            try
                            {
                                MailingList.Get("RegisterLetter")
                                    .WithReplacement(
                                        new MailReplacement("{PASSWORD}", pass)
                                    ).To(um).Send();
                            }
                            catch
                            {

                            }
                            FormsAuthentication.SetAuthCookie(um, true);
                        }
                    }
                }
                

                var nm = new ChatMessage()
                    {
                        Chat = chat,
                        Date = DateTime.Now,
                        Text = message,
                        UserID = userID
                    };

                if (AccessHelper.IsMaster)
                    nm.IsRead = true;

                cDB.ChatMessages.InsertOnSubmit(nm);
                cDB.SubmitChanges();
            }

            var messages =
                cDB.ChatMessages.Where(x => x.Chat.ChatUID == session)
                    .OrderBy(x => x.Date).ToList()
                    .Select(
                        x =>
                            new JChatMessage()
                            {
                                ID = x.ID,
                                Message = x.Text,
                                Time = x.Date.ToString("HH:mm"),
                                Date = x.Date.ToString("d MMMMM HH:mm:ss"),
                                Author = x.AuthorName
                            })
                    .ToArray();

            return Json(messages);
        }

        private void SendNotification(string message, string un, string um, string uf)
        {
            try
            {
                MailingList.Get("ChatLetter")
                    .WithReplacements(
                        new List<MailReplacement>()
                        {
                            new MailReplacement("{MESSAGE}", message),
                            new MailReplacement("{NAME}", un),
                            new MailReplacement("{MAIL}", um),
                            new MailReplacement("{PHONE}", uf)
                        }
                    ).Send();
            }
            catch
            {
                
            }

        }

        [AuthorizeMaster]
        [MenuItem("Настройки чата", 790, 70, Icon = "settingchat")]
        public ActionResult ChatSettings()
        {
            var list = db.SiteSettings.Where(x => x.GroupName == "Настройки онлайн-чата").OrderBy(x => x.OrderNum);
            foreach (var setting in list)
            {
                ViewData.Add(setting.Setting, setting.oValue);
            }
            ViewBag.TitleBlock = "Настройки онлайн-чата";
            return View("../Settings/Index", list);

        }

        [HttpPost, AuthorizeMaster, ValidateInput(false)]
        public ActionResult ChatSettings(FormCollection collection)
        {
            var groups = SettingsController.SaveSettings(collection, ModelState, db);
            ModelState.AddModelError("", "Данные сохранены");
            return ChatSettings();
        }


        public ActionResult ChatWidget()
        {
            var dataChat = db.SiteSettings.Where(x => x.GroupName == "Настройки онлайн-чата").Skip(2).Select(x => x.Value).ToList();
            var colorList = new List<string>();
            foreach(var data in dataChat)
            {
                colorList.Add(data);
            }
            ViewBag.ColorChat = colorList;
            var forClose =
                cDB.ChatMessages.Where(x => !x.Chat.IsClosed && SqlMethods.DateDiffDay(x.Date, DateTime.Now) >= 1)
                   .Select(x => x.Chat)
                   .Distinct();
            if (forClose.Any())
            {
                foreach (var chat in forClose)
                {
                    chat.IsClosed = true;
                }
                cDB.SubmitChanges();
            }
            return PartialView();
        }
    }
}
