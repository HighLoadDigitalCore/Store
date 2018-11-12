using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;
using ImageFormat = BotDetect.ImageFormat;

namespace Smoking.Controllers
{
    public class UniversalEditorController : Controller
    {
        [HttpPost]
        public virtual ActionResult UploadFile(string fileColumn)
        {
            HttpPostedFileBase myFile = Request.Files[fileColumn];
            bool isUploaded = false;
            string message = "Ошибка при загрузке изображения";
            var fileName = "";
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/content/temp");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        fileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        isUploaded = true;
                        message = "Изображение успешно загружено";
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Ошибка при загрузке изображения: {0}", ex.Message);
                    }
                }
            }
            return
                Json(
                    new
                        {
                            isUploaded = isUploaded,
                            message = message,
                            path = "/content/temp/" + fileName,
                            filedName = fileColumn
                        });
        }

        [HttpPost]
        public virtual ActionResult UploadFileToDisk(string path, string fileColumn)
        {
            HttpPostedFileBase myFile = Request.Files[fileColumn];
            bool isUploaded = false;
            string message = "Ошибка при загрузке изображения";
            var fileName = "";
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath(path);
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        fileName = myFile.FileName;// DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        isUploaded = true;
                        message = "Изображение успешно загружено";
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("Ошибка при загрузке изображения: {0}", ex.Message);
                    }
                }
            }
            return
                Json(
                    new
                        {
                            isUploaded = isUploaded,
                            message = message,
                            path = path + fileName,
                            filedName = fileColumn.Replace("_Input", "")
                        });
        }

        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: Need to process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [AuthorizeMaster]
        [HttpPost]
        [ValidateInput(false)]
        public PartialViewResult UniversalEdit(UniversalEditorPagedData ds, FormCollection collection)
        {
            ds.AddQueryParamsJoin = collection["AddQueryParamsJoin"];
            if (ds.AddQueryParams.Any())
            {

            }
            var model = GetModel(ds.CallerController, ds.CallerAction, null, ds.AddQueryParams);
            model.EditedRow.LoadPossibleProperties(new[] { "ID" }, collection);

            var imagePaths =
                collection.AllKeys.Where(x => x.EndsWith("_Path"))
                          .Select(x => new { Field = x.Replace("_Path", ""), Data = ReadFile(collection[x]) }).Where(x => x.Data != null && x.Data.Length > 0);


            foreach (var image in imagePaths)
            {
                model.EditedRow.SetPropertyValue(image.Field, new Binary(image.Data));
            }


            var errList = model.ErrorList;
            if (errList.IsFilled())
            {
                var list = errList.Split<string>("<br/>");
                foreach (var err in list)
                {
                    ModelState.AddModelError("", err);
                }
                //ModelState.AddModelError("", errList);
                return PartialView(model);
            }
            bool inserting = (int)model.EditedRow.GetPropertyValue("ID") == 0;
            string msg = "";
            var db = new DB();
            if (model.BeforeSaveRow != null)
            {
                msg = model.BeforeSaveRow(model.EditedRow, db, HttpContext);
            }
            if (model.SaveRow != null)
            {
                msg += model.SaveRow(model.EditedRow, db);
            }
            else
            {
                msg += model.Settings.UniversalTableSaver(model.EditedRow, model.Settings, db);

            }
            if (model.AfterSaveRow != null)
            {
                msg += model.AfterSaveRow(model.EditedRow, db);
            }
            if (model.CompleteSave != null)
            {
                model.CompleteSave(model, model.EditedRow);
            }
            ModelState.AddModelError("", msg.IsNullOrEmpty() ? "Данные сохранены" : msg);
            if (msg.IsNullOrEmpty())
                if (inserting)
                {
                    var routes = new RouteValueDictionary
                        {
                            {"Type", "List"},
                            {"Page", Request.QueryString["Page"].ToInt()}
                        };
                    var filterRoutes = model.FilterParams;
                    foreach (var route in filterRoutes.Where(route => Request.QueryString[route].IsFilled()))
                    {
                        routes.Add(route, Request.QueryString[route]);
                    }
                    if (ds.AddQueryParams != null)
                    {
                        foreach (var param in ds.AddQueryParams)
                        {
                            if (!routes.ContainsKey(param) && Request.QueryString[param].IsFilled())
                                routes.Add(param, Request.QueryString[param]);
                        }
                    }
                    model.RedirectURL = Url.Action(model.CallerAction, model.CallerController, routes);
                }
            return PartialView(model);
        }

        private byte[] ReadFile(string path)
        {
            ClearOldImages();

            if (string.IsNullOrEmpty(path))
                return null;

            try
            {
                using (var fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[fs.Length];
                    //TODO Check for large files
                    fs.Read(buffer, 0, (int)fs.Length);
                    fs.Close();
                    return buffer;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void ClearOldImages()
        {
            int timeout = 30;
            var di = new DirectoryInfo(Server.MapPath("/content/temp"));
            var files = di.GetFiles();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file.Name);
                DateTime nameDate;
                if (DateTime.TryParseExact(name, "ddMMyyyyHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None,
                                           out nameDate))
                {
                    if (nameDate.AddMinutes(timeout) < DateTime.Now)
                        try
                        {
                            file.Delete();
                        }
                        catch
                        {

                        }
                }
            }
        }

        [AuthorizeMaster]
        [ValidateInput(false)]

        private UniversalEditorPagedData GetModel(string callerController, string callerAction, object[] pa = null, string[] aa = null, string query = "")
        {
            NameValueCollection collection = new NameValueCollection();
            foreach (string param in Request.Params.Keys)
            {
                if (!collection.AllKeys.Contains(param))
                {
                    var o = Request.Params[param];
                    if (o.Contains(","))
                    {
                        var els = o.Split<string>(",").ToList();
                        if (els.Count() == 2 && els.ElementAt(0) == els.ElementAt(1))
                        {
                            o = els.ElementAt(0);
                        }
                        else if (els.Count == 1)
                        {
                            o = els.ElementAt(0);
                        }
                    }

                    collection.Add(param, o);
                }
            }
            //collection.Add(Request.Params);
            if (query.IsFilled())
            {
                var items =
                    query.Split<string>("&").Select(x => x.Split<string>("=").ToArray()).ToList();
                foreach (var item in items)
                {
                    if (collection[item.ElementAt(0)] == null)
                        collection.Add(item.ElementAt(0), item.Count() == 2 ? item.ElementAt(1) : "");
                }
            }
            var type = Type.GetType("Smoking.Controllers." + callerController + "Controller");
            var obj = Activator.CreateInstance(type);
            MethodInfo methodInfo = type.GetMethod(callerAction);
            object result = null;

            if (methodInfo != null)
            {
                var parametersArray = (pa ?? new object[]
                    {
                        collection["Type"], collection["Page"].ToInt(),
                        collection["UID"].ToInt()
                    }).ToList();

                if (aa != null)
                    parametersArray.AddRange(aa.Select(key => collection[key].ToSuitableType()));

                parametersArray = parametersArray.Select(x => x != null && x.Equals("undefined") ? null : x).ToList();

                result = methodInfo.Invoke(obj, parametersArray.ToArray());
            }
            if (result is ViewResult)
                return (UniversalEditorPagedData)((ViewResult)result).ViewData.Model;
            else return (UniversalEditorPagedData)((PartialViewResult)result).ViewData.Model;
        }

        [HttpPost]
        [AuthorizeMaster]
        public PartialViewResult UniversalDelete(UniversalEditorPagedData ds, FormCollection collection)
        {
            ds.AddQueryParamsJoin = collection["AddQueryParamsJoin"];
            var model = GetModel(ds.CallerController, ds.CallerAction, null, ds.AddQueryParams);
            var db = new DB();
            ITable table = null;
            try
            {
                table = db.GetTableByName(model.Settings.TableName);
            }
            catch
            {
                try
                {
                    table = db.GetTableByName(model.Settings.TableName + "s");
                }
                catch
                {

                }
            }
            if (table == null)
            {
                ModelState.AddModelError("", "Таблица не найдена");
                return PartialView(model);

            }
            object target = null;
            var uid = (int)model.EditedRow.GetPropertyValue(model.Settings.UIDColumnName);
            foreach (var item in table)
            {
                if ((int)item.GetPropertyValue(model.Settings.UIDColumnName) == uid)
                    target = item;
            }
            if (target == null)
            {
                ModelState.AddModelError("", "Объект не найден.");
                return PartialView(model);
            }
            if (model.BeforeDelFunc != null)
            {
                model.BeforeDelFunc(target, db);
            }
            try
            {
                if (model.DelFunc != null)
                {
                    model.DelFunc(target, db);
                }
                else
                {
                    table.DeleteOnSubmit(target);
                    db.SubmitChanges();
                }

            }
            catch (Exception ee)
            {
                ModelState.AddModelError("", ee.Message);
                return PartialView(model);
            }
            RouteValueDictionary dict = new RouteValueDictionary();
            dict.Add("Type", "List");
            dict.Add("Page", Request.QueryString["Page"].ToInt());
            if (ds.AddQueryParams != null)
            {
                foreach (var param in ds.AddQueryParams)
                {
                    if (!dict.ContainsKey(param) && Request.QueryString[param].IsFilled())
                        dict.Add(param, Request.QueryString[param]);
                }
            }
            model.RedirectURL = Url.Action(model.CallerAction, model.CallerController, dict);
            return PartialView(model);

        }

        /*[OutputCache(Duration = 86400, Location = OutputCacheLocation.Client)]*/
        public FileContentResult Image(string tableName, string uidName, string uidValue, string fieldName, int width = 0, int height = 0, string backColor = "", bool forDL = false, int nocache = 0)
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

            if (forDL)
            {
                var fileName = uidValue + ".jpg";

                if (tableName == "StoreCategories")
                {
                    string nSql = string.Format("select Slug from {1} where {2} = {3}", fieldName, tableName, uidName,
                        uidValue);
                    using (
                        var conn =
                            new SqlConnection(
                                ConfigurationManager.ConnectionStrings["MsSqlConnectionString"].ConnectionString))
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = nSql;
                            cmd.CommandType = CommandType.Text;
                            conn.Open();
                            object result = null;
                            try
                            {
                                result = cmd.ExecuteScalar();
                            }
                            catch
                            {
                            }
                            if (result != null && !(result is DBNull) && !result.ToString().IsNullOrEmpty())
                            {
                                fileName = result + ".jpg";
                            }

                        }
                    }
                }
                Response.Headers.Add("Content-Disposition", "attachment; filename=\"" +
                    HttpContext.Server.UrlEncode(fileName) + "\"");
            }

            string rawIfModifiedSince = Request.Headers.Get("If-Modified-Since");
            if (string.IsNullOrEmpty(rawIfModifiedSince) || nocache == 1)
            {
                Response.Cache.SetLastModified(DateTime.Now);
            }
            else
            {
                DateTime ifModifiedSince = DateTime.Parse(rawIfModifiedSince);
                if (ifModifiedSince.AddHours(1) >= DateTime.Now)
                {
                    Response.StatusCode = 304;

                    return new FileContentResult(new byte[0],
                                                 MIMETypeWrapper.GetMIMEForData(
                                                     new Binary(new byte[0])));
                }
            }

            if (uidValue == "0")
            {
                Response.StatusCode = 404;
            }
            string sql = string.Format("select {0} from {1} where {2} = {3}", fieldName, tableName, uidName, uidValue);
            using (
                var conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    object result = null;
                    try
                    {
                        result = cmd.ExecuteScalar();
                    }
                    catch
                    {
                        sql = string.Format("select {0} from {1} where {2} = {3}", fieldName,
                                            tableName.EndsWith("es")
                                                ? tableName.Substring(0, tableName.Length - 2)
                                                : tableName.Substring(0, tableName.Length - 1), uidName, uidValue);
                        cmd.CommandText = sql;
                        result = cmd.ExecuteScalar();
                    }
                    if (result == null || result is DBNull)
                    {
                        Response.StatusCode = 404;
                        return new FileContentResult(new byte[0], "image/jpeg");
                    }

                    /*Response.Cache.SetCacheability(HttpCacheability.Public);*/


                    if (height > 0 || width > 0)
                    {
                        using (var ms = new MemoryStream((byte[])result))
                        {
                            ms.Position = 0;
                            var image = System.Drawing.Image.FromStream(ms);



                            var resized = ResizeImage(width, height, image);
                            if (resized != null)
                            {

                                using (var rs = new MemoryStream())
                                {
                                    var path = Server.MapPath(SiteSetting.Get<string>("WatermarkText"));
                                    if (SiteSetting.Get<bool>("WatermarkShow"))
                                    {

                                        Image imgWatermark = new Bitmap(path);

                                        var bitmap = new Bitmap(width, height);


                                        var graphics = Graphics.FromImage(bitmap);

                                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        graphics.CompositingMode = CompositingMode.SourceCopy;

                                        graphics.DrawImage(resized, (float)0, (float)0, width, height);


                                        int wmWidth = /*imgWatermark.Width*/width / 10;
                                        int wmHeight = /*imgWatermark.Height*/height / 10;



                                        var bmWatermark = new Bitmap(bitmap);
                                        bmWatermark.SetResolution(graphics.DpiX, graphics.DpiY);

                                        var grWatermark =
                                            Graphics.FromImage(bmWatermark);


                                        ImageAttributes imageAttributes =
                                            new ImageAttributes();
                                        ColorMap colorMap = new ColorMap();

                                        colorMap.OldColor = Color.FromArgb(255, 255, 255, 255); //базовый фоновый цвет
                                        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                        ColorMap[] remapTable = { colorMap };

                                        imageAttributes.SetRemapTable(remapTable,
                                                                      ColorAdjustType.Bitmap);

                                        float[][] colorMatrixElements =
                            {
                                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                new float[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f},
                                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                            };

                                        var wmColorMatrix = new
                                            ColorMatrix(colorMatrixElements);

                                        imageAttributes.SetColorMatrix(wmColorMatrix,
                                                                       ColorMatrixFlag.Default,
                                                                       ColorAdjustType.Bitmap);

                                        int xPosOfWm = ((bitmap.Width - wmWidth) / 3) * 2;
                                        int yPosOfWm = ((bitmap.Height - wmHeight) / 4);
                                        /*int yPosOfWm = bitmap.Height - wmHeight - 10;*/

                                        grWatermark.DrawImage(imgWatermark,
                                                              new Rectangle(xPosOfWm, yPosOfWm, wmWidth,
                                                                            wmHeight),
                                                              0,
                                                              0,
                                                              imgWatermark.Width,
                                                              imgWatermark.Height,
                                                              GraphicsUnit.Pixel,
                                                              imageAttributes);


                                        yPosOfWm = ((bitmap.Height - wmHeight) / 4) * 3;
                                        grWatermark.DrawImage(imgWatermark,
                                                              new Rectangle(xPosOfWm, yPosOfWm, wmWidth,
                                                                            wmHeight),
                                                              0,
                                                              0,
                                                              imgWatermark.Width,
                                                              imgWatermark.Height,
                                                              GraphicsUnit.Pixel,
                                                              imageAttributes);


                                        bitmap = bmWatermark;
                                        graphics.Dispose();
                                        grWatermark.Dispose();


                                        bitmap.Save(rs, System.Drawing.Imaging.ImageFormat.Jpeg);

                                        bitmap.Dispose();
                                        imgWatermark.Dispose();



                                    }
                                    else
                                    {

                                        /*
                                                        if (SiteSetting.Get<bool>("ImageRotate"))
                                                        {
                                                            bitmap = RotateImg(bitmap, 1, Color.White);
                                                        }
                                        */
                                        resized.Save(rs, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                    return new FileContentResult(rs.ToArray(), MIMETypeWrapper.GetMIMEForData(
                                                new Binary(rs.ToArray())));



                                    /*
                                                                        resized.Save(rs, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                        return new FileContentResult(rs.ToArray(),
                                                                            MIMETypeWrapper.GetMIMEForData(
                                                                                new Binary(rs.ToArray())));
                                    */

                                }

                            }
                        }
                    }




                    return new FileContentResult((byte[])result,
                                                 MIMETypeWrapper.GetMIMEForData(
                                                     new Binary((byte[])result)));


                }
            }

        }
        //Overload for crop that default starts top left of the image.
        public static System.Drawing.Image CropImage(System.Drawing.Image Image, int Height, int Width)
        {
            return CropImage(Image, Height, Width, 0, 0);
        }

        //The crop image sub
        public static System.Drawing.Image CropImage(System.Drawing.Image Image, int Height, int Width, int StartAtX, int StartAtY)
        {
            Image outimage;
            MemoryStream mm = null;
            try
            {
                //check the image height against our desired image height
                if (Image.Height < Height)
                {
                    Height = Image.Height;
                }

                if (Image.Width < Width)
                {
                    Width = Image.Width;
                }

                //create a bitmap window for cropping
                Bitmap bmPhoto = new Bitmap(Width, Height);
                bmPhoto.SetResolution(72, 72);

                //create a new graphics object from our image and set properties
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //now do the crop
                grPhoto.DrawImage(Image, new Rectangle(0, 0, Width, Height), StartAtX, StartAtY, Width, Height, GraphicsUnit.Pixel);

                // Save out to memory and get an image from it to send back out the method.
                mm = new MemoryStream();
                bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                Image.Dispose();
                bmPhoto.Dispose();
                grPhoto.Dispose();
                outimage = Image.FromStream(mm);

                return outimage;
            }
            catch (Exception ex)
            {
                throw new Exception("Error cropping image, the error was: " + ex.Message);
            }
        }

        //Hard resize attempts to resize as close as it can to the desired size and then crops the excess
        public static System.Drawing.Image HardResizeImage(int Width, int Height, System.Drawing.Image Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            Image resized = null;
            if (Width > Height)
            {
                resized = ResizeImage(Width, Width, Image);
            }
            else
            {
                resized = ResizeImage(Height, Height, Image);
            }
            Image output = CropImage(resized, Height, Width);
            //return the original resized image
            return output;
        }

        //Image resizing
        public static Image ResizeImage(int maxWidth, int maxHeight, Image Image)
        {
            try
            {
                int width = Image.Width;
                int height = Image.Height;
                if (width > maxWidth || height > maxHeight)
                {
                    //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                    //from displaying as the thumbnail image later, in other words, we want a clean
                    //resize, not a grainy one.
                    Image.RotateFlip(RotateFlipType.Rotate180FlipX);
                    Image.RotateFlip(RotateFlipType.Rotate180FlipX);

                    float ratio = 0;
                    if (width > height)
                    {
                        ratio = (float)width / (float)height;
                        width = maxWidth;
                        height = Convert.ToInt32(Math.Round((float)width / ratio));
                    }
                    else
                    {
                        ratio = (float)height / (float)width;
                        height = maxHeight;
                        width = Convert.ToInt32(Math.Round((float)height / ratio));
                    }

                    //return the resized image
                    return Image.GetThumbnailImage(width, height, null, IntPtr.Zero);
                }
                return Image;
            }
            catch
            {
                return new Bitmap(maxWidth, maxHeight);
            }

            //return the original resized image

        }

        public ContentResult ClearImage(string tableName, string uidName, string uidValue, string fieldName)
        {

            string sql = string.Format("update {0} set {1} = null where {2} = {3}", tableName, fieldName, uidName,
                                       uidValue);
            using (
                var conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        //Response.StatusCode = 404;
                    }
                    return new ContentResult() { Content = "1" };

                }
            }
        }


        [HttpPost]
        [AuthorizeMaster]
        public PartialViewResult changeOrder(int id, int? value, int page, string tablename, string uidname, string ordername, string cc, string ca, string addqs, string query)
        {
            if (!value.HasValue)
                return PartialView();
            var db = new DB();
            var table = db.GetTableByName(tablename);
            object target = null;
            var all = new List<KeyValuePair<object, int>>();

            foreach (var item in table)
            {
                if ((int)item.GetPropertyValue(uidname) == id)
                    target = item;
                else all.Add(new KeyValuePair<object, int>(item, (int)item.GetPropertyValue(ordername)));
            }
            if (target == null)
                return PartialView();

            if (all.Any())
            {
                all = all.OrderBy(x => x.Value).ToList();
                int pos = value.Value - 1;
                if (pos <= 0) pos = 0;
                int max = all.Max(x => x.Value);
                if (pos >= max)
                    all.Add(new KeyValuePair<object, int>(target, max + 1));
                else
                    all.Insert(pos, new KeyValuePair<object, int>(target, pos));
                int counter = 1;
                foreach (var rec in all)
                {
                    rec.Key.SetPropertyValue(ordername, counter);
                    counter++;
                }
            }
            else
            {
                target.SetPropertyValue(ordername, 1);
            }
            db.Refresh(RefreshMode.KeepChanges);
            db.SubmitChanges();


            var qp = (addqs ?? "").Split<string>("&").ToList();


            var model = GetModel(cc, ca, new object[] { "List", page, id }, qp.Any() ? qp.ToArray() : null, query);
            CatalogBrowser.Init().ClearAllCaches();
            return PartialView("UniversalList", model);
        }

        [HttpPost]
        [AuthorizeMaster]
        public PartialViewResult changeOrderComplex(int id1, int id2, string uid1, string uid2, int? value, int page, string tablename, string ordername, string cc, string ca, string addqs, string query)
        {
            if (!value.HasValue)
                return PartialView();
            var db = new DB();
            var table = db.GetTableByName(tablename);
            object target = null;
            var all = new List<KeyValuePair<object, int>>();

            foreach (var item in table)
            {
                if ((int)item.GetPropertyValue(uid1) == id1 && (int)item.GetPropertyValue(uid2) == id2)
                    target = item;
                else if ((int)item.GetPropertyValue(uid2) == id2)
                    all.Add(new KeyValuePair<object, int>(item, (int)item.GetPropertyValue(ordername)));
            }
            if (target == null)
                return PartialView();

            if (all.Any())
            {
                all = all.OrderBy(x => x.Value).ToList();
                int pos = value.Value - 1;
                if (pos <= 0) pos = 0;
                int max = all.Max(x => x.Value);
                if (pos >= max)
                    all.Add(new KeyValuePair<object, int>(target, max + 1));
                else
                    all.Insert(pos, new KeyValuePair<object, int>(target, pos));
                int counter = 1;
                foreach (var rec in all)
                {
                    rec.Key.SetPropertyValue(ordername, counter);
                    counter++;
                }
            }
            else
            {
                target.SetPropertyValue(ordername, 1);
            }
            db.Refresh(RefreshMode.KeepChanges);
            db.SubmitChanges();


            var qp = (addqs ?? "").Split<string>("&").ToList();


            var model = GetModel(cc, ca, new object[] { "List", page, 0 }, qp.Any() ? qp.ToArray() : null, query);
            return PartialView("UniversalList", model);
        }

    }
}
