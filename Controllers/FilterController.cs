using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smoking.Extensions;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class FilterController : Controller
    {


        private DB db = new DB();

        /*[MenuItem("Фильтры", ID = 50, ParentID = 4)]*/
        public ActionResult Index()
        {
            return View(db.Filters.ToList());
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Add(int Category)
        {
            var filter = db.Filters.FirstOrDefault(x => x.CatID == Category);
            if (filter == null)
            {
                filter = new Smoking.Models.Filter() {CatID = Category, Visible = true};
                db.Filters.InsertOnSubmit(filter);
                db.SubmitChanges();
            }

            var item = new FilterItem()
            {
                CharID = null,
                FilterID = filter.ID,
                Help = "",
                Img = null,
                IsPrice = false,
                Name = "",
                Type = 0,
                Visible = false,
                OrderNum = filter.FilterItems.Count + 1
            };
            db.FilterItems.InsertOnSubmit(item);
            db.SubmitChanges();
            return new ContentResult();
        }


        [HttpPost]
        [AuthorizeMaster]
        public ActionResult DeleteItem(int ID)
        {
            var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                db.FilterItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            return new ContentResult();
        }



        [HttpPost]
        [AuthorizeMaster]
        public virtual ActionResult DeleteIcon(int ID)
        {

            var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                item.Img = null;
                db.SubmitChanges();
            }
            return new ContentResult();
        }

   

        [HttpPost]
        [AuthorizeMaster]
        public virtual ActionResult UploadIcon(int ID)
        {
            HttpPostedFileBase myFile = null;
            if (Request.Files.Count > 0)
            {
                myFile = Request.Files[0];    
            }
            
            bool isUploaded = false;
            string message = "Ошибка при загрузке изображения";
            var fileName = "";
            if (myFile != null && myFile.ContentLength != 0)
            {
                using (var sr = new MemoryStream())
                {
                    
                    myFile.InputStream.CopyTo(sr);

                    var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
                    if (item != null)
                    {
                        item.Img = sr.ToArray();
                        db.SubmitChanges();
                        isUploaded = true;
                        message = "Изображение успешно загружено";
                    }
                    else
                    {
                        message = string.Format("Ошибка при загрузке изображения");
                    }

                    
                }
               
            }
            return
                Json(
                    new
                    {
                        isUploaded = isUploaded,
                        message = message,
                        path = "/Master/ru/UniversalEditor/Image?tableName=FilterItems&fieldName=Img&uidName=ID&width=50&height=50&uidValue=" + ID+"&rnd="+new Random(DateTime.Now.Millisecond).Next(1000, 9999),
                        id = ID
                    });
        }



        [HttpPost]
        [AuthorizeMaster]
        public ActionResult ChangeOrder(string Type, int ID, int Category)
        {
            var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                FilterItem pair = null;
                pair = Type == "up"
                    ? db.FilterItems.FirstOrDefault(x => x.FilterID == item.FilterID && x.OrderNum < item.OrderNum)
                    : db.FilterItems.FirstOrDefault(x => x.FilterID == item.FilterID && x.OrderNum > item.OrderNum);

                if (pair != null)
                {
                    var on = item.OrderNum;
                    item.OrderNum = pair.OrderNum;
                    pair.OrderNum = on;
                    db.SubmitChanges();
                }
            }
            return new ContentResult();
        }


        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Save(int ID, int Category, string Field, string Value)
        {
            var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                switch (Field)
                {
                    case "Name":
                        item.Name = Value;
                        break;
                    case "Help":
                        item.Help = Value;
                        break;
                    case "CharID":
                        if (Value == "Price")
                        {
                            item.CharID = null;
                            item.IsPrice = true;
                        }
                        else if (Value.IsFilled())
                        {
                            item.CharID = Value.ToInt();
                            item.IsPrice = false;
                        }
                        else
                        {
                            item.CharID = null;
                            item.IsPrice = false;
                        }
                        break;
                    case "Type":
                        item.Type = Value.ToBool() ? 1 : 0;
                        break;
                    case "Visible":
                        item.Visible = Value.ToBool();
                        break;

                }
                db.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Settings(int Category)
        {
            ViewBag.Category = Category;

            var cats = new List<StoreCategory>();
            var cat = db.StoreCategories.FirstOrDefault(x => x.ID == Category);
            GetAllChildren(ref cats, cat);

/*
            var items =
                cats.SelectMany(x => x.StoreProductsToCategories)
                    .Select(x => x.StoreProduct)
                    .SelectMany(x => x.StoreCharacterToProducts)
                    .Select(x => x.StoreCharacterValue)
                    .Select(x => x.StoreCharacter)
                    .Distinct().OrderBy(x=> x.Name);
*/


            IEnumerable<StoreCharacter> items = new List<StoreCharacter>();

            IEnumerable<StoreCharacter> result = cats.Aggregate(items,
                (current, category) =>
                    current.Concat(
                        db.StoreCharacters.Where(
                            x =>
                                x.StoreCharacterValues.Any(
                                    z =>
                                        z.StoreCharacterToProducts.Any(
                                            c =>
                                                c.StoreProduct.StoreProductsToCategories.Any(
                                                    b => b.CategoryID == category.ID))))));
            items = result.Distinct().OrderBy(x=> x.Name);


            var ddl = items.ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
            ddl.Insert(0, new SelectListItem(){Text = "Цена", Value = "Price"});
            ddl.Insert(0, new SelectListItem(){Text = "-не выбрано-", Value = ""});

            ViewBag.Items = ddl;
            var list = db.FilterItems.Where(x => x.Filter.CatID == Category).OrderBy(x=> x.OrderNum).ToList();
            return View(list);
        }

        private void GetAllChildren(ref List<StoreCategory> cats, StoreCategory cat)
        {
            if (cat.StoreProductsToCategories.Any())
            {
                cats.Add(cat);
            }
            foreach (var category in cat.Children)
            {
                GetAllChildren(ref cats, category);
            }
        }


        [HttpGet]
        [AuthorizeMaster]
        public JsonResult getTreeData(string SelectedSection)
        {
            var list = new List<string>();
            if (SelectedSection.IsFilled())
            {
                list.Add(SelectedSection);

            }
            var treeData = new UniversalTreeDataSource() { CheckedNodes = list };
            return treeData.Serialize(SerializationType.PagesAnsCategories);

        }

        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Edit(int? ID)
        {
            var filter = db.Filters.FirstOrDefault(x => x.ID == ID) ?? new Models.Filter() { Visible = true };
            return View(filter);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Edit(Models.Filter filter)
        {
            if (!filter.HasSection)
            {
                ViewBag.Message = "Необходимо выбрать раздел";
                return View(filter);
            }

            if (filter.ID == 0)
            {
                db.Filters.InsertOnSubmit(filter);
            }
            else
            {
                var dbf = db.Filters.First(x => x.ID == filter.ID);
                dbf.SelectedSection = filter.SelectedSection;
                dbf.Visible = filter.Visible;
            }
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeMaster]
        public ActionResult Delete(int ID)
        {
            var filter = db.Filters.FirstOrDefault(x => x.ID == ID);
            if (filter != null)
            {
                db.Filters.DeleteOnSubmit(filter);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        [AuthorizeMaster]
        public ActionResult Items(int FilterID)
        {
            ViewBag.FilterID = FilterID;
            var filt = db.FilterItems.Select(x => x).ToList();
            var filter = db.FilterItems.Where(x => x.FilterID == FilterID).ToList();
            return View(filter);

        }

        [AuthorizeMaster]
        [HttpGet]
        public ActionResult ItemEdit(int FilterID, int? ID)
        {
            FilterItem item = null;
            if (!ID.HasValue)
            {
                item
                    = new FilterItem()
                    {
                        FilterID = FilterID
                    };
            }
            else
            {
                item = db.FilterItems.FirstOrDefault(x => x.ID == ID) ?? new FilterItem() { FilterID = FilterID };
            }
            return View(item);
        }

        [AuthorizeMaster]
        [HttpPost]
        public ActionResult ItemEdit(int FilterID, int? ID, FilterItem item)
        {

            string err = "";
            if (item.Name.IsNullOrEmpty())
            {
                err = "Необходимо указать название фильтра";
            }
            if (err.IsFilled())
            {
                ModelState.AddModelError("", err);
                return View(item);
            }

            if (item.ID == 0)
            {
                db.FilterItems.InsertOnSubmit(item);
            }
            else
            {
                var dbi = db.FilterItems.FirstOrDefault(x => x.ID == ID);
                dbi.Visible = item.Visible;
                if (dbi != null)
                {
                    dbi.LoadPossibleProperties(item);
                }
            }
            if (item.Help == null)
                item.Help = "";
            db.SubmitChanges();

            return RedirectToAction("Items", new {FilterID = FilterID});
        }

        public ActionResult ItemDelete(int ID, int FilterID)
        {
            var item = db.FilterItems.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                db.FilterItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            return RedirectToAction("Items",new {FilterID});
        }

    }

    
}
