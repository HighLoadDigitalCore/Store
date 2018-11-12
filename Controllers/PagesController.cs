using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class PagesController : Controller
    {
        DB db = new DB();

        [HttpGet]

        [AuthorizeMaster]
        /*     [MenuItem("Структура сайта", -100, Icon = "home")]
        */
        public ActionResult Index(int? Page)
        {
            var pagedList = new PagedData<CMSPage>(db.CMSPages.OrderBy(x => x.OrderNum), Page ?? 0, 20, "MasterListPaged");
            return View(pagedList);
        }


        [AuthorizeMaster]
        public ActionResult UniversalModuls(int pageID)
        {
            var page = db.CMSPages.FirstOrDefault(x => x.ID == pageID);
            if (page != null)
            {
                page = page.LoadLangValues();
            }

            var user = Membership.GetUser("admin");
            if (user == null)
                user = Membership.GetUser(Roles.GetUsersInRole("Administrator").FirstOrDefault() ?? "");

            if (user == null)
                ViewBag.LastChange = new List<LastChangeModel>();
            else
            {
                var last = db.CMSPages.Where(x => !x.Deleted).OrderByDescending(x => x.LastMod).Take(50).ToList().Select(x => x.LoadLangValues()).ToList().Select(x => new LastChangeModel()
                {
                    ID = x.ID,
                    Action = "Редактирование страницы",
                    Date = x.LastMod,
                    Type = 2,
                    Link = x.FullUrl,
                    Name = x.PageName,
                    UserLink = "/Master/ru/Users/Edit?user=" + user.ProviderUserKey,
                    UserName = user.UserName
                }).ToList();

                last.AddRange(
                    db.StoreCategories.Where(x => !x.Deleted)
                        .OrderByDescending(x => x.LastMod)
                        .Take(50)
                        .Select(x => new LastChangeModel()
                        {
                            ID = x.ID,
                            Action = "Редактирование папки",
                            Date = x.LastMod,
                            Type = 0,
                            Link = x.FullUrl,
                            Name = x.Name,
                            UserLink = "/Master/ru/Users/Edit?user=" + user.ProviderUserKey,
                            UserName = user.UserName
                        }));

                last.AddRange(
                    db.StoreProducts.Where(x => !x.Deleted)
                        .OrderByDescending(x => x.LastMod)
                        .Take(50)
                        .Select(x => new LastChangeModel()
                        {
                            ID = x.ID,
                            Action = "Редактирование товара",
                            Date = x.LastMod,
                            Type = 1,
                            Link = x.FullUrl,
                            Name = x.Name,
                            UserLink = "/Master/ru/Users/Edit?user=" + user.ProviderUserKey,
                            UserName = user.UserName
                        }));

                last = last.OrderByDescending(x => x.Date).Take(50).ToList();
                ViewBag.LastChange = last;
            }
            return PartialView(page);
        }

        [HttpGet]
        [AuthorizeMaster]
        public PartialViewResult Edit(int? ID, int? ParentID, int? vtype)
        {
            CMSPage page = new CMSPage() { ParentID = 0, Visible = true, ViewMenu = false, Deleted = false, LastMod = DateTime.Now, };

            if (!ID.HasValue || ID == 0)
            {
                ViewBag.Header = "Создание нового раздела";

                var parent = CMSPage.FullPageTable.FirstOrDefault(x => x.ID == ParentID);
                if (parent != null)
                {
                    parent.RolesList = null;
                    page.RolesList = parent.RolesList;
                }
                else
                {
                    var roles = new List<RoleInfo>();
                    roles.AddRange(db.Roles.ToList().Select(x => new RoleInfo()
                    {
                        RoleName = x.Description.IsNullOrEmpty() ? x.RoleName : x.Description,
                        RoleID = x.RoleId,
                        Selected = true
                    }));
                    roles.Add(new RoleInfo()
                                    {
                                        RoleName = "Неавторизованные пользователи",
                                        RoleID = new Guid(),
                                        Selected = true
                                    });
                    page.RolesList = roles;
                }
            }
            else
            {
                ViewBag.Header = "Редактирование раздела";
                page = db.CMSPages.FirstOrDefault(x => x.ID == ID);

                if (page == null)
                {
                    RedirectToAction("Index");
                }
                page.LoadLangValues();
            }
            if (!vtype.HasValue)
                vtype = 2;
            var parents = CMSPage.FullPageTable.Where(x => x.ID != ID).ToList();
            parents.Insert(0, new CMSPage() { ID = 0, PageName = "Корневой раздел сайта", LastMod = DateTime.Now, });
            ViewBag.Parents = new SelectList(parents, "ID", "PageName", page.ParentID ?? 0);
            ViewBag.Types = new SelectList(db.PageTypes.Where(x => x.Enabled).OrderBy(x => x.Ordernum).AsEnumerable(), "ID", "Description");
            ViewBag.VType = vtype;
            return PartialView(page);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Edit(int? ID, int? vtype, FormCollection collection, CMSPage model)
        {
            var page = new CMSPage() { LastMod = DateTime.Now, };
            bool exist = false;
            bool isNew = false;
            if (model.PageName.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Необходимо указать название страницы");
                return PartialView(model);
            }
            if (model.URL.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Необходимо указать URL страницы");
                return PartialView(model);
            }
            if (ID.HasValue && ID > 0)
            {
                ViewBag.Header = "Редактирование раздела";
                page = db.CMSPages.FirstOrDefault(x => x.ID == ID);
                exist = true;
                if (page == null)
                {
                    ModelState.AddModelError("", "Страница не найдена");
                    return PartialView(model);
                }

            }
            else
            {
                isNew = true;
                ViewBag.Header = "Создание нового раздела";
                page.OrderNum = (db.CMSPages.Count() + 1) * 10;
                db.CMSPages.InsertOnSubmit(page);
            }

            var oldParent = page.ParentID;

            page.LoadPossibleProperties(model, new[] { "ID", "UrlPath", "BreadCrumbs", "LinkedBreadCrumbs", "ParentID" });
            if (page.FullName.IsNullOrEmpty())
                page.FullName = "";
            if (page.FullNameH2.IsNullOrEmpty())
                page.FullNameH2 = "";
            if (page.Title.IsNullOrEmpty())
                page.Title = "";
            if (page.Keywords.IsNullOrEmpty())
                page.Keywords = "";
            if (page.Description.IsNullOrEmpty())
                page.Description = "";
            page.LastMod = DateTime.Now;
            var parents = CMSPage.FullPageTable.Where(x => x.ID != ID).ToList();
            parents.Insert(0, new CMSPage() { ID = 0, PageName = "Корневой раздел сайта" });
            ViewBag.Parents = new SelectList(parents, "ID", "PageName", page.ParentID ?? 0);
            ViewBag.Types = new SelectList(db.PageTypes.Where(x => x.Enabled).OrderBy(x => x.Ordernum).AsEnumerable(), "ID", "Description");
            if (!vtype.HasValue)
                vtype = 1;

            if (model.ParentID == 0 || !model.ParentID.HasValue)
            {
                page.ParentID = null;
            }
            else
            {
                page.ParentID = model.ParentID;
            }
            if (page.ParentID == page.ID)
                page.ParentID = oldParent;
            if (page.ParentID == page.ID)
                page.ParentID = null;

            ViewBag.VType = vtype;
            try
            {
                db.SubmitChanges();
                page.SaveLangValues();

                /*

                                if (model.ParentID == 0 || !model.ParentID.HasValue)
                                    page.ParentID = null;
                                else
                                    page.Parent = db.CMSPages.FirstOrDefault(x => x.ID == model.ParentID);

                                db.SubmitChanges();
                */

                ModelState.AddModelError("", "Данные сохранены");



            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var roles =
                collection.AllKeys.Where(x => x.StartsWith("ID_"))
                          .Select(
                              x =>
                              new
                                  {
                                      Value = (bool)collection.GetValue(x).ConvertTo(typeof(bool)),
                                      ID = new Guid(x.Split<string>("_").ToArray()[1])
                                  });

            foreach (var role in roles)
            {
                var rel =
                    db.CMSPageRoleRels.FirstOrDefault(
                        x => (role.ID == new Guid() ? !x.RoleID.HasValue : x.RoleID == role.ID) && x.PageID == page.ID);
                if (role.Value && rel == null)
                {
                    db.CMSPageRoleRels.InsertOnSubmit(new CMSPageRoleRel()
                        {
                            RoleID = role.ID == new Guid() ? (Guid?)null : role.ID,
                            PageID = page.ID
                        });
                }
                if (!role.Value && rel != null)
                {
                    db.CMSPageRoleRels.DeleteOnSubmit(rel);
                }
            }
            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {

            }

            CMSPage.FullPageTable = null;

            if (isNew)
            {

                var another = db.CMSPages.FirstOrDefault(x => x.ID != page.ID && x.Type == page.Type && !x.Deleted);
                if (another != null)
                {
                    var views = db.CMSPageCellViews.Where(x => x.PageID == another.ID).ToList();
                    foreach (var view in views)
                    {
                        var v = new CMSPageCellView()
                        {
                            Action = view.Action,
                            Controller = view.Controller,
                            CellID = view.CellID,
                            Path = view.Path,
                            Description = view.Description,
                            OrderNum = view.OrderNum,
                            PageID = page.ID
                        };
                        db.CMSPageCellViews.InsertOnSubmit(v);
                    }
                    db.SubmitChanges();
                }
                return new ContentResult() { Content = "<script type='text/javascript'>resetSelectNode('" + "#x" + page.ID + "');</script>" };

            }

            if (exist)
                return PartialView(page);
            return PartialView(null);

            //            return RedirectToAction("Index");

        }

        [HttpGet]
        [AuthorizeMaster]
        public ActionResult GetSiteUrl(string uid)
        {
            if (uid.StartsWith("x"))
            {
                var page = CMSPage.Get(uid.Substring(1).ToInt());
                return new ContentResult() { Content = string.IsNullOrEmpty(page.FullUrl) ? "/" : page.FullUrl };
            }
            if (uid.StartsWith("c"))
            {
                var cat = db.StoreCategories.FirstOrDefault(x => x.ID == uid.Substring(1).ToInt());
                if (cat != null)
                    return new ContentResult() { Content = cat.FullUrl };
            }
            if (uid.StartsWith("p"))
            {
                var cat = db.StoreProducts.FirstOrDefault(x => x.ID == uid.Substring(1).ToInt());
                if (cat != null)
                    return new ContentResult() { Content = cat.FullUrl };
            }
            return new ContentResult() { Content = "/" };
        }

        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeData()
        {
            var treeData = new UniversalTreeDataSource() { LinkFunction = x => Url.Action("Edit", "Pages", new { ID = x }) };
            return treeData.Serialize(SerializationType.Pages);

        }



        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Delete(int? ID)
        {
            var page = db.CMSPages.FirstOrDefault(x => x.ID == ID);
            if (page == null)
                return RedirectToAction("Index");

            return PartialView(page.LoadLangValues());
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Delete(int? ID, FormCollection collection)
        {
            var page = db.CMSPages.FirstOrDefault(x => x.ID == ID);
            if (page == null)
                return PartialView(null);

            deleteRecursive(page);
            db.SubmitChanges();
            CMSPage.ClearAllCache();

            var main = db.CMSPages.FirstOrDefault(x => !x.ParentID.HasValue) ??
                       (db.CMSPages.FirstOrDefault() ?? new CMSPage() { LastMod = DateTime.Now, });

            return new ContentResult()
            {
                Content = "<script type='text/javascript'>resetSelectNode('" + "#x" + main.ID + "');</script>"
            };

        }

        private void deleteRecursive(CMSPage page)
        {
            if (page.Children.Any())
            {
                foreach (var child in page.Children)
                {
                    deleteRecursive(child);
                }
            }
            page.Deleted = true;
            /*db.CMSPages.DeleteOnSubmit(page);*/
        }

        [HttpPost]
        [AuthorizeMaster]
        public ContentResult saveNode(int nodeID, int targetID, string type)
        {
            var currentNode = db.CMSPages.FirstOrDefault(x => x.ID == nodeID);
            var targetNode = db.CMSPages.FirstOrDefault(x => x.ID == targetID);
            if (currentNode == null || (targetNode == null && targetID != 0)) return new ContentResult();
            var targetParent = targetNode == null ? null : (int?)targetNode.ID;
            switch (type)
            {
                //родитель меняется
                case "first":
                    currentNode.ParentID = targetParent;
                    var inLevelNodes = db.CMSPages.Where(x => targetParent == null ? !x.ParentID.HasValue : x.ParentID == targetParent);
                    if (inLevelNodes.Any())
                        currentNode.OrderNum = inLevelNodes.Min(x => x.OrderNum) - 20;
                    break;
                case "last":
                    currentNode.ParentID = targetParent;
                    var inLevelNodesA = db.CMSPages.Where(x => targetParent == null ? !x.ParentID.HasValue : x.ParentID == targetParent);
                    if (inLevelNodesA.Any())
                        currentNode.OrderNum = inLevelNodesA.Max(x => x.OrderNum) + 20;
                    break;
                //родитель не меняется ??
                case "before":
                    targetParent = targetNode == null ? null : (int?)targetNode.ParentID;
                    var prevInOrder =
                        db.CMSPages.Where(x => (targetParent == null ? !x.ParentID.HasValue : x.ParentID == targetParent) && x.OrderNum < targetNode.OrderNum);
                    foreach (var page in prevInOrder)
                    {
                        page.OrderNum -= 40;
                    }
                    currentNode.OrderNum = targetNode.OrderNum - 20;
                    currentNode.ParentID = targetParent;
                    break;
                case "after":
                    targetParent = targetNode == null ? null : (int?)targetNode.ParentID;
                    var nextInOrder =
                        db.CMSPages.Where(x => (targetParent == null ? !x.ParentID.HasValue : x.ParentID == targetParent) && x.OrderNum > targetNode.OrderNum);
                    foreach (var page in nextInOrder)
                    {
                        page.OrderNum += 40;
                    }
                    currentNode.OrderNum = targetNode.OrderNum + 20;
                    currentNode.ParentID = targetParent;
                    break;
            }

            db.SubmitChanges();
            //Обнуляем кеш
            CMSPage.FullPageTable = null;
            return new ContentResult() { Content = "1" };
        }


    }
}
