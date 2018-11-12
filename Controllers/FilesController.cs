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
    public class FilesController : Controller
    {
        private DB db = new DB();

        public ActionResult Settings(int? Category, int? Product)
        {
            var files = new List<StoreFile>();
            if (Category.HasValue)
            {
                files = db.StoreFiles.Where(x => x.CategoryID == Category).OrderBy(x => x.OrderNum).ToList();
            }
            else if (Product.HasValue)
            {
                files = db.StoreFiles.Where(x => x.ProductID == Product).OrderBy(x => x.OrderNum).ToList();
            }
            ViewBag.Category = Category;
            ViewBag.Product = Product;


            return View(files);
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Add(int? Category, int? Product)
        {

            var item = new StoreFile()
            {
                Name = "",
                Link = "",
                Download = false,
                OrderNum = db.StoreFiles.Count() + 1
            };

            if (Category.HasValue)
            {
                item.CategoryID = Category;
            }
            else if (Product.HasValue)
            {
                item.ProductID = Product;
            }
            if (Category.HasValue || Product.HasValue)
            {
                db.StoreFiles.InsertOnSubmit(item);
                db.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult ChangeOrder(string Type, int ID)
        {
            var item = db.StoreFiles.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                var list = db.StoreFiles.AsQueryable();
                if (item.CategoryID.HasValue)
                {
                    list = list.Where(x => x.CategoryID == item.CategoryID);
                }
                else if (item.ProductID.HasValue)
                {
                    list = list.Where(x => x.ProductID == item.ProductID);
                }


                StoreFile pair = null;
                pair = Type == "up"
                    ? list.FirstOrDefault(x => x.OrderNum < item.OrderNum)
                    : list.FirstOrDefault(x => x.OrderNum > item.OrderNum);

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
        public ActionResult Save(int ID, string Field, string Value)
        {
            var item = db.StoreFiles.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                switch (Field)
                {
                    case "Name":
                        item.Name = Value;
                        break;
                    case "Download":
                        item.Download = Value.ToBool();
                        break;
            

                }
                db.SubmitChanges();
            }
            return new ContentResult();
        }



        [HttpPost]
        [AuthorizeMaster]
        public ActionResult DeleteItem(int ID)
        {
            var item = db.StoreFiles.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                if (item.Link.IsFilled())
                {
                    var path = Server.MapPath(item.Link);
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch
                        {
                            
                        }
                    }
                }

                db.StoreFiles.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpGet]
        [AuthorizeMaster]
        public ActionResult Download(int ID)
        {

            var item = db.StoreFiles.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                var mp = HttpContext.Server.MapPath(item.Link);

                Response.Headers.Add("Content-Disposition", "attachment; filename=\"" +
                                            HttpContext.Server.UrlPathEncode(Path.GetFileName(item.Link)) + "\"");

                using (var reader = new FileStream(mp, FileMode.Open))
                {
                    var buf = new byte[reader.Length];
                    reader.Read(buf, 0, (int)reader.Length);
                    return new FileContentResult(buf, "application/octet-stream");
                }

            }
            return new ContentResult();
        }  
        
        
        [HttpGet]
        
        public ActionResult DownloadClient(int ID)
        {

            var item = db.StoreFiles.FirstOrDefault(x => x.ID == ID);
            if (item != null)
            {
                var mp = HttpContext.Server.MapPath(item.Link);

                Response.Headers.Add("Content-Disposition", "attachment; filename=\"" +
                                            HttpContext.Server.UrlPathEncode(item.Name + Path.GetExtension(item.Link)) + "\"");

                using (var reader = new FileStream(mp, FileMode.Open))
                {
                    var buf = new byte[reader.Length];
                    reader.Read(buf, 0, (int)reader.Length);
                    return new FileContentResult(buf, "application/octet-stream");
                }

            }
            return new ContentResult();
        }



        [HttpPost]
        [AuthorizeMaster]
        public ActionResult UploadFile(int ID)
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


                var item = db.StoreFiles.FirstOrDefault(x => x.ID == ID);
                if (item != null)
                {
                    var dir = Server.MapPath("/content/Docs");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var path = dir + "\\" + myFile.FileName;

                    var fi = new FileInfo(path);
                    var i = 1;
                    while (fi.Exists)
                    {
                        path = dir + "\\" + Path.GetFileNameWithoutExtension(myFile.FileName) + "-" + i +
                               Path.GetExtension(myFile.FileName);
                        i++;
                        fi = new FileInfo(path);
                    }

                    myFile.SaveAs(path);




                    item.Link = "/content/Docs/" + Path.GetFileName(path);
                    db.SubmitChanges();
                    isUploaded = true;
                    message = "Файл успешно загружен";
                }
                else
                {
                    message = string.Format("Ошибка при загрузке файла");
                }



            }
            return
                Json(
                    new
                    {
                        isUploaded = isUploaded,
                        message = message,
                        id = ID
                    });
        }


    }
}
