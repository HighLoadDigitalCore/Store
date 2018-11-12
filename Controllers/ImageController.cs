using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class ImageController : Controller
    {
        public FileContentResult Download(string path)
        {
            var mp = HttpContext.Server.MapPath(path);
            if (!System.IO.File.Exists(mp))
            {
                Response.StatusCode = 404;
                Response.Status = "Not Found";
                return new FileContentResult(new byte[0], "image/jpeg");
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=\"" +
                                                        HttpContext.Server.UrlEncode(Path.GetFileName(path)) + "\"");

            using (var reader = new FileStream(mp, FileMode.Open))
            {
                var buf = new byte[reader.Length];
                reader.Read(buf, 0, (int)reader.Length);
                return new FileContentResult(buf, "image/jpeg");
            }



        }

        [AuthorizeMaster]
        [HttpGet]
        public ActionResult Index(int ProductID)
        {
            var db = new DB();
            var prod = db.StoreProducts.FirstOrDefault(x => x.ID == ProductID);
            ViewBag.Product = prod;

            var list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();
            return View(list);
        }



        [AuthorizeMaster]
        [HttpPost]
        public ActionResult Index(int ProductID, FormCollection collection)
        {
            var db = new DB();
            var prod = db.StoreProducts.First(x => x.ID == ProductID);
            ViewBag.Product = prod;


            var list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();

            if (collection["Image_New_Icon"].IsFilled() || collection["Image_New"].IsFilled())
            {
                list.Add(new StoreImage() { Enabled = true/*, ID = 1, UrlPath = "/content/admin/apple.png"*/});
            }

            var add = collection.AllKeys.Any(x => x == "Apply_0");
            if (add)
            {
                if (Request.Files.AllKeys.All(x => x != "Upload_0"))
                {
                    ModelState.AddModelError("", "Необходимо выбрать изображение");
                }
                else
                {
                    var uf = Request.Files["Upload_0"];

                    var file = "/content/Catalog/" + prod.SlugOrId + Path.GetExtension(uf.FileName);
                    if (System.IO.File.Exists(Server.MapPath(file)))
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            file = "/content/Catalog/" + prod.SlugOrId + "-" + i + Path.GetExtension(uf.FileName);
                            if (!System.IO.File.Exists(Server.MapPath(file)))
                                break;
                        }
                    }
                    uf.SaveAs(Server.MapPath(file));
                    var ni = new StoreImage()
                    {
                        Alt = collection["Alt_0"],
                        Description = collection["Title_0"],
                        UrlPath = file,
                        /*Youtube = collection["Youtube_0"],*/
                        UrlPathThumbs = file,
                        ProductID = prod.ID,
                        Enabled = collection["Enabled_0"].ToBool(),
                        OrderNum = db.StoreImages.Count() + 1
                    };
                    db.StoreImages.InsertOnSubmit(ni);
                    db.SubmitChanges();
                    list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();
                }
            }
            var delete = collection.AllKeys.Any(x => x.StartsWith("Delete_") && x != "Delete_0");
            if (delete)
            {
                var id = collection.AllKeys.First(x => x.StartsWith("Delete_")).Replace("Delete_", "").ToInt();
                var ifd = db.StoreImages.FirstOrDefault(x => x.ID == id);
                if (ifd != null)
                {
                    db.StoreImages.DeleteOnSubmit(ifd);
                    db.SubmitChanges();
                    list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();
                }
            }
            var apply = collection.AllKeys.Any(x => x.StartsWith("Apply_") && x != "Apply_0");
            if (apply)
            {
                var id = collection.AllKeys.First(x => x.StartsWith("Apply_")).Replace("Apply_", "").ToInt();
                var ifu = db.StoreImages.FirstOrDefault(x => x.ID == id);
                if (ifu != null)
                {
                    if (Request.Files.AllKeys.Any(x => x == "Upload_" + id))
                    {
                        var file = Request.Files["Upload_" + id];
                        if (file != null && file.ContentLength > 0)
                            file.SaveAs(Server.MapPath(ifu.UrlPath));
                    }
                    /*ifu.Youtube = collection["Youtube_" + id];*/

                    if (Request.Files.AllKeys.Any(x => x == "UploadVideo_" + id))
                    {
                        var file = Request.Files["UploadVideo_" + id];
                        if (file != null && file.ContentLength > 0)
                        {
                            if (!Directory.Exists(Server.MapPath("/content/Video")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/content/Video"));
                            }
                            var path = "/content/Video/" + file.FileName;
/*
                            int cnt = 1;
                            while (System.IO.File.Exists(Server.MapPath(path)))
                            {
                                path = "/content/Video/" + Path.GetFileNameWithoutExtension(file.FileName) + "-" + cnt +
                                       Path.GetExtension(file.FileName);
                                cnt++;
                            }
*/
                            file.SaveAs(Server.MapPath(path));

                            ifu.Youtube = path;
                        }
                        
                    }

                    ifu.Description = collection["Title_" + id];
                    ifu.Alt = collection["Alt_" + id];
                    ifu.Enabled = collection["Enabled_" + id].ToBool();
                    db.SubmitChanges();
                    list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();
                }
            }

            var up = collection.AllKeys.Any(x => x.StartsWith("Up_") && x != "Up_0");
            if (up)
            {
                var id = collection.AllKeys.First(x => x.StartsWith("Up_")).Replace("Up_", "").ToInt();
                var if1 = db.StoreImages.FirstOrDefault(x => x.ID == id);
                var if2 = db.StoreImages.OrderBy(x => x.OrderNum).FirstOrDefault(x => x.OrderNum < if1.OrderNum);
                if (if2 != null && if1 != null)
                {
                    var if1o = if1.OrderNum;
                    if1.OrderNum = if2.OrderNum;
                    if2.OrderNum = if1o;
                    db.SubmitChanges();
                    list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();
                }

            }
            var down = collection.AllKeys.Any(x => x.StartsWith("Down_") && x != "Down_0");
            if (down)
            {
                var id = collection.AllKeys.First(x => x.StartsWith("Down_")).Replace("Down_", "").ToInt();
                var if1 = db.StoreImages.FirstOrDefault(x => x.ID == id);
                var if2 = db.StoreImages.OrderBy(x => x.OrderNum).FirstOrDefault(x => x.OrderNum > if1.OrderNum);
                if (if2 != null && if1 != null)
                {
                    var if1o = if1.OrderNum;
                    if1.OrderNum = if2.OrderNum;
                    if2.OrderNum = if1o;
                    db.SubmitChanges();
                    list = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum).ToList();
                }

            }

            return View(list);
        }

        public FileContentResult StripeCSS()
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

            string rawIfModifiedSince = Request.Headers.Get("If-Modified-Since");
            if (string.IsNullOrEmpty(rawIfModifiedSince))
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

            var cats =
                CatalogBrowser.CategoriesList.Where(x => x.Value.FullUrl.Split<string>("/").Count()<=4).
                    Where(x => x.Value != null/* && x.Value.Image != null && x.Value.Image.Length > 0*/).ToList();
            var count = cats.Count;
            var css = "";
            for (int i = 0; i < count; i++)
            {
                css += ".cat-i-" + cats[i].Value.ID + " {";
                css += "background: url(/Master/ru/Image/CatalogMenuSprite?group=" + (Math.Floor((decimal)i / 150)) + ");";
                css += "background-position: -" + ((i % 150) * 45) + "px; 0px";
                css += "} ";
            }
            return new FileContentResult(Encoding.UTF8.GetBytes(css), "text/css");
        }

        public FileContentResult CatalogMenuSprite(int group = 0)
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

            string rawIfModifiedSince = Request.Headers.Get("If-Modified-Since");
            if (string.IsNullOrEmpty(rawIfModifiedSince))
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

/*
            if (HttpContext.Cache.Get("CatalogMenuSprite" + group) is FileContentResult)
            {
                return HttpContext.Cache.Get("CatalogMenuSprite" + group) as FileContentResult;
            }
*/

            var defImage = System.IO.File.ReadAllBytes(Server.MapPath("~/content/noimage.jpg"));

            var cats = CatalogBrowser.CategoriesList.Where(x => x.Value.FullUrl.Split<string>("/").Count() <= 4).ToList();
            var images = cats.Select(x => new { ID = x.Value.ID, Value = x.Value })
                .Where(x => x.Value != null /* && x.Value.Image!=null*/)
                .OrderBy(x => x.ID).Skip(group * 150).Take(150) /*.AsParallel().WithDegreeOfParallelism(10)*/
                .Select(
                    x => new
                    {
                        x.ID,
                        Image =
                            UniversalEditorController.ResizeImage(45, 45,
                                Image.FromStream(
                                    new MemoryStream(x.Value.Image != null && x.Value.Image.Length > 0
                                        ? x.Value.Image.ToArray()
                                        : defImage)))
                    }).ToList().OrderBy(x => x.ID).ToList();


            var strip = new Bitmap(45 * images.Count(), 45);

            var graphics = Graphics.FromImage(strip);

            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;


            var counter = 0;
            foreach (var image in images)
            {
                graphics.DrawImage(image.Image, new Point(45 * counter, 0));
                counter++;
            }


            var memoryStream = new MemoryStream();

            strip.Save(memoryStream, ImageFormat.Jpeg);


            graphics.Dispose();
            strip.Dispose();
            var result = new FileContentResult(memoryStream.ToArray(), "img/jpeg");
/*
            HttpContext.Cache.Add("CatalogMenuSprite" + group, result, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, CACHE_DURATION, 0)
                              , CacheItemPriority.Normal, null);
*/

            return result;
        }

        private static int? _cacheDuration;
        private static int CACHE_DURATION
        {
            get
            {
                if (!_cacheDuration.HasValue)
                {
                    _cacheDuration = ConfigurationManager.AppSettings["CacheDuration"].ToNullInt() ?? 5;
                }
                return _cacheDuration.Value;
            }
        }

        /*[OutputCache(Duration = 86400, Location = OutputCacheLocation.Client)]*/
        public FileContentResult Resize(int maxWidth, int maxHeight, string filePath, int? padding, string brushColor, int? brushWidthData, string backColor, string aligh, string vertalign, bool skiplogo = true, bool skipRotate = false)
        {

            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

            string rawIfModifiedSince = Request.Headers.Get("If-Modified-Since");
            if (string.IsNullOrEmpty(rawIfModifiedSince))
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


            if (!padding.HasValue)
            {
                padding = 0;
            }
            if (brushColor.IsNullOrEmpty())
                brushColor = "#FFFFFF";
            if (!brushWidthData.HasValue)
                brushWidthData = 0;
            if (backColor.IsNullOrEmpty())
                backColor = "#FFFFFF";
            if (aligh.IsNullOrEmpty())
                aligh = "center";
            if (vertalign.IsNullOrEmpty())
                vertalign = "center";


            var fileContent = ImageBySize(filePath, maxWidth, maxHeight, padding.Value, brushColor,
                                          brushWidthData.ToString(), backColor, aligh, vertalign, skiplogo, skipRotate);
            return fileContent;
        }
        /// <summary>
        /// Контролер возвращает изображения вписаное в указаные размеры, если изображение меньше то дополняется любым цветом
        /// </summary>
        /// <param name="filepath">Путь к файлу изображения</param>
        /// <param name="maxWidth">Максимальная ширина изображения</param>
        /// <param name="maxHeight">Максимальная высота изображения</param>
        /// <param name="padding">Внутрение отступы</param>
        /// <param name="brushColor">Цвет кисти</param>
        /// <param name="brushWidthData">Данные по ширине кисти</param>
        /// <param name="backColor">Цвет заднего плана</param>
        /// <returns></returns>
        public FileContentResult ImageBySize(string filepath, int maxWidth, int maxHeight, int padding = 0, string brushColor = "#FFFFFF", string brushWidthData = "0",
         string backColor = "#FFFFFF", string aligh = "center", string vertalign = "center", bool skiplogo = false, bool skipRotate = false)
        {
            var brushWidth = new Thickness(brushWidthData);
            var freeWidth = maxWidth - ((padding * 2) + brushWidth.Left + brushWidth.Right);
            var freeHeight = maxHeight - ((padding * 2) + brushWidth.Top + brushWidth.Bottom);

            int newWidth;
            int newHeight;



            // формируем путь к изображению
            //var photoPath = HttpContext.Current.Server.MapPath("~/" + filepath);
            var photoPath = HttpContext.Server.MapPath((filepath.StartsWith("~/") ? "" : "~/") + filepath);
            if (photoPath == null)
                return null;
            if (!System.IO.File.Exists(photoPath))
            {
                photoPath = HttpContext.Server.MapPath("~/content/noimage.jpg");
            }
            var photo = new Bitmap(photoPath);
            #region размер картинки больше размера содержимого
            if (photo.Height > freeHeight && photo.Width > freeWidth)
            {
                if (photo.Height > photo.Width)
                {
                    // коэфициент
                    if (freeHeight <= 0) freeHeight = 1;
                    var koeff = freeHeight / (double)photo.Height;
                    // определяем ширину нового изображения 
                    newWidth = (int)(photo.Width * koeff);
                    // определяем высоту нового изображения
                    newHeight = (int)(photo.Height * koeff);



                    return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
                }
                else
                {
                    if (GetIntFromString(freeWidth.ToString()) <= 0) freeWidth = 1;
                    var koeff = freeWidth / (double)photo.Width;
                    // определяем ширину изображения
                    var imgWidth = photo.Width * koeff;
                    // определяем высоту изображения
                    var imgHeight = photo.Height * koeff;

                    if (GetIntFromString(imgHeight.ToString()) > freeHeight)
                    {
                        // коэфициент
                        if (freeHeight <= 0) freeHeight = 1;
                        koeff = freeHeight / imgHeight;
                        // определяем ширину нового изображения 
                        newWidth = (int)(imgWidth * koeff);
                        // определяем высоту нового изображения
                        newHeight = (int)(imgHeight * koeff);
                        return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
                    }
                    if (GetIntFromString(imgWidth.ToString()) > GetIntFromString(freeWidth.ToString()))
                    {
                        if (freeWidth <= 0) freeWidth = 1;
                        koeff = freeWidth / imgWidth;
                        // определяем ширину нового изображения 
                        newWidth = (int)(photo.Width * koeff);
                        // определяем высоту нового изображения
                        newHeight = (int)(photo.Height * koeff);
                        return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
                    }
                    // определяем ширину изображения
                    newWidth = (int)imgWidth;
                    // определяем высоту изображения
                    newHeight = (int)imgHeight;
                    return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
                }
            }
            #endregion

            #region размер картинки меньше размера содержимого
            if (photo.Height <= GetIntFromString(freeHeight.ToString()) &&
                photo.Width <= GetIntFromString(freeWidth.ToString()))
            {
                // определяем ширину изображения
                newWidth = photo.Width;
                // определяем высоту изображения
                newHeight = photo.Height;
                return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
            }
            #endregion

            #region ширина картинки больше свободной ширины а высота меньше или равна свободной стороне
            if (photo.Width > GetIntFromString(freeWidth.ToString()) &&
               photo.Height <= GetIntFromString(freeHeight.ToString()))
            {
                var koeff = freeWidth / (double)photo.Width;
                // определяем ширину изображения
                newWidth = (int)(photo.Width * koeff);
                // определяем высоту изображения
                newHeight = (int)(photo.Height * koeff);
                return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
            }
            #endregion

            #region высота картинки больше свободной области а ширина меньше или равно свободной области
            if (photo.Height > GetIntFromString(freeHeight.ToString()) &&
               photo.Width <= GetIntFromString(freeWidth.ToString()))
            {
                var koeff = freeHeight / (double)photo.Height;
                // определяем ширину изображения
                newWidth = (int)(photo.Width * koeff);
                // определяем высоту изображения
                newHeight = (int)(photo.Height * koeff);
                return GetResult(newWidth, newHeight, maxWidth, maxHeight, ref photo, padding, brushColor, brushWidth, backColor, aligh, vertalign, skiplogo, skipRotate);
            }
            #endregion

            return null;
        }


        public static Bitmap RotateImg(Bitmap bmp, float angle, Color bkColor, int newWidth, int newHeight)
        {
            int originHeight = bmp.Height;
            int originWidth = bmp.Width;

            angle = angle % 360;
            if (angle > 180)
                angle -= 360;

            PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);
            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }

            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImage(bmp, 0, 0, originWidth, originHeight); // draw the image at 0, 0
            g.Dispose();


            Rectangle cropRect = new Rectangle(0, 0, newImg.Width, newImg.Height);

            Bitmap target = new Bitmap(originWidth, originHeight);

            using (Graphics gr = Graphics.FromImage(target))
            {
                gr.DrawImage(newImg, cropRect, new Rectangle((newImg.Width - target.Width), (newImg.Height - target.Height), target.Width, target.Height),

                                GraphicsUnit.Pixel);
            }

            return target;
        }

        private FileContentResult GetResult(int newWidth, int newHeight, int maxWidth, int maxHeight, ref Bitmap photo,
          int padding = 0, string brushColor = "#FFFFFF", Thickness brushWidth = null, string backColor = "#FFFFFF", string aligh = "center", string vertalign = "center", bool skiplogo = false, bool skipRotate = false)
        {

            if (SiteSetting.Get<bool>("ImageRotate") && !skipRotate)
            {
                photo = RotateImg(photo, 1, Color.White, newWidth, newHeight);
            }


            const int marginAlign = 10;

            if (Math.Abs(newWidth - maxWidth) <= marginAlign)
                newWidth = maxWidth;
            if (Math.Abs(newHeight - maxHeight) <= marginAlign)
                newHeight = maxHeight;

            var memoryStream = new MemoryStream();

            if (brushWidth == null) brushWidth = new Thickness(0);
            var freeWidth = maxWidth - ((padding * 2) + brushWidth.Left + brushWidth.Right);
            var freeHeight = maxHeight - ((padding * 2) + brushWidth.Top + brushWidth.Bottom);



            var bitmap = new Bitmap(maxWidth, maxHeight);


            var graphics = Graphics.FromImage(bitmap);

            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;

            // рисуем границу прямоугольника
            var brush = HexToColor(backColor);
            graphics.FillRectangle(new SolidBrush(brush), 0, 0, maxWidth, maxHeight);


            // верхняя линия
            brush = HexToColor(brushColor);
            var pen = new Pen(brush, brushWidth.Top);
            var pt1 = new Point(0, brushWidth.Top / 2);
            var pt2 = new Point(maxWidth, brushWidth.Top / 2);
            graphics.DrawLine(pen, pt1, pt2);

            // правая линия
            pen = new Pen(brush, brushWidth.Right);
            var x = maxWidth - (brushWidth.Right / 2);
            var y = brushWidth.Top / 2;
            pt1 = new Point(x, y);
            y = maxHeight - (brushWidth.Bottom / 2);
            pt2 = new Point(x, y);
            graphics.DrawLine(pen, pt1, pt2);
            //graphics.DrawArc(pn, rect.Right - sz.Width, rect.Bottom - sz.Height,
            //  sz.Width, sz.Height, 0, 90);

            // нижняя линия
            pen = new Pen(brush, brushWidth.Bottom);
            x = 0;
            y = maxHeight - (brushWidth.Bottom / 2);
            pt1 = new Point(x, y);
            x = maxWidth;
            pt2 = new Point(x, y);
            graphics.DrawLine(pen, pt1, pt2);

            //graphics.DrawArc(pn, rect.Left, rect.Bottom - sz.Height,
            //  sz.Width, sz.Height, 90, 90);

            // левая линия
            pen = new Pen(brush, brushWidth.Left);
            x = brushWidth.Left / 2;
            y = 0;
            pt1 = new Point(x, y);
            y = maxHeight;
            pt2 = new Point(x, y);
            graphics.DrawLine(pen, pt1, pt2);

            //graphics.DrawArc(pn, rect.Left, rect.Top,
            //  sz.Width, sz.Height, 180, 90);

            // отрисовываем необходимое изображение
            // вычисляем координату смещения 
            var innerLeft = (freeWidth - newWidth) / 2;
            var leftWidth = (padding * 2) + brushWidth.Right + brushWidth.Left + (innerLeft * 2);
            var leftkoeff = leftWidth == 0 ? 0 : (padding + brushWidth.Left + innerLeft) / (double)leftWidth;
            var left = (maxWidth - (double)newWidth) * leftkoeff;

            var innerTop = (freeHeight - newHeight) / 2;
            var topWidth = (padding * 2) + brushWidth.Top + brushWidth.Bottom + (innerTop * 2);
            var topkoeff = topWidth == 0 ? 0 : (padding + brushWidth.Top + innerTop) / (double)topWidth;
            var top = ((maxHeight - (double)newHeight) * topkoeff);

            switch (vertalign)
            {
                case "middle":
                    break;
                case "top":
                    top = 0;
                    break;
                case "bottom":
                    top *= 2;
                    break;
            }
            //
            switch (aligh)
            {
                case "left":
                    graphics.DrawImage(photo, 0, (float)top, newWidth, newHeight);
                    break;
                case "right":
                    graphics.DrawImage(photo, (float)left * 2, (float)top, newWidth, newHeight);
                    break;
                default:
                    graphics.DrawImage(photo, (float)left, (float)top, newWidth, newHeight);
                    break;
            }

            var path = Server.MapPath(SiteSetting.Get<string>("WatermarkText"));
            if (SiteSetting.Get<bool>("WatermarkShow") && System.IO.File.Exists(path) && !skiplogo)
            {

                Image imgWatermark = new Bitmap(path);



                int wmWidth = /*imgWatermark.Width*/bitmap.Width / 10;
                int wmHeight = /*imgWatermark.Height*/bitmap.Height / 10;

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

                int xPosOfWm = ((bitmap.Width - wmWidth) / 3)*2;
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


                bitmap.Save(memoryStream, ImageFormat.Jpeg);

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
                bitmap.Save(memoryStream, ImageFormat.Jpeg);
            }
            return new FileContentResult(memoryStream.ToArray(), "image/jpeg");
        }


        #region КОНВЕРТАИЯ ЦВЕТОВ
        /// <summary>Метод преобразовывает цвет из формата HEX в RGB</summary>
        /// <param name="hexValue">Значение HEX</param>
        /// <returns></returns>
        private static Color HexToColor(string hexValue)
        {
            try
            {
                hexValue = hexValue.Replace("#", string.Empty);
                byte position = 0;
                byte alpha = Convert.ToByte("ff", 16);

                if (hexValue.Length == 8)
                {
                    // get the alpha channel value
                    alpha = Convert.ToByte(hexValue.Substring(position, 2), 16);
                    position = 2;
                }

                // get the red value
                var red = Convert.ToByte(hexValue.Substring(position, 2), 16);
                position += 2;

                // get the green value
                var green = Convert.ToByte(hexValue.Substring(position, 2), 16);
                position += 2;

                // get the blue value
                var blue = Convert.ToByte(hexValue.Substring(position, 2), 16);

                // create the Color object
                var color = Color.FromArgb(alpha, red, green, blue);

                // create the SolidColorBrush object
                return color;
            }
            catch
            {
                return Color.FromArgb(255, 255, 255, 255);
            }
        }

        /// <summary>Метод преобразовывает цвет в формат HEX</summary>
        /// <param name="color">Преобразовываемый цвет</param>
        /// <returns></returns>
        public static string ColorToHex(Color color)
        {
            return string.Format("#{0}{1}{2}",
                   color.R.ToString("X2"),
                   color.G.ToString("X2"),
                   color.B.ToString("X2"));
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        /// <summary>Метод возвращает целое число из или 0</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static int GetIntFromString(string text)
        {
            int result;

            Int32.TryParse(text, out result);
            //
            return result;
        }

        /// <summary>Структура для установки толщины линий по прямоуголнику </summary>
        public class Thickness
        {

            #region КОНСТРУКТОР
            /// <summary>Структура для установки толщины линий по прямоуголнику </summary>
            public Thickness()
            {
                Left = 0;
                Top = 0;
                Right = 0;
                Bottom = 0;
            }

            /// <summary>Структура для установки толщины линий по прямоуголнику. Конструктор устанавливает одинаковую толщину линий для всех сторон</summary>
            /// <param name="width">Толщина линии</param>
            public Thickness(int width)
            {
                Left = width;
                Top = width;
                Right = width;
                Bottom = width;
            }

            /// <summary>
            /// Структура для установки толщины линий по прямоуголнику.      
            /// </summary>
            /// <param name="start">Параметр устанавливает толщину линии для левой и правой стороны</param>
            /// <param name="end">Параметр устанавливает толщину линий для верхней и нижней стороны</param>
            public Thickness(int start, int end)
            {
                Left = start;
                Right = start;
                Top = end;
                Bottom = end;
            }

            /// <summary>
            /// Структура для установки толщины линий по прямоуголнику.
            /// </summary>
            /// <param name="left">Левая толщина линии</param>
            /// <param name="top">Верхняя толщина линии</param>
            /// <param name="right">Правая толщина линии</param>
            /// <param name="bottom">Нижняя толщина линии</param>
            public Thickness(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            /// <summary>
            /// Структура для установки толщины линий по прямоуголнику.      
            /// </summary>
            /// <param name="data">Строка содержащая данные</param>
            public Thickness(string data)
            {
                Left = 0;
                Top = 0;
                Right = 0;
                Bottom = 0;
                if (data.IndexOf('-') != -1)
                {
                    var arr = data.Split('-');
                    if (arr.Length == 2)
                    {
                        Left = GetIntFromString(arr[0]);
                        Right = GetIntFromString(arr[0]);
                        Top = GetIntFromString(arr[1]);
                        Bottom = GetIntFromString(arr[1]);
                    }
                    else if (arr.Length == 4)
                    {
                        Left = GetIntFromString(arr[0]);
                        Top = GetIntFromString(arr[1]);
                        Right = GetIntFromString(arr[2]);
                        Bottom = GetIntFromString(arr[3]);
                    }
                }
                else
                {
                    var width = GetIntFromString(data);
                    Left = width;
                    Top = width;
                    Right = width;
                    Bottom = width;
                }

            }
            #endregion

            #region СВОЙСТВА
            /// <summary>Левая толщина линии</summary>
            public int Left { get; set; }
            /// <summary>Верхняя толщина линии</summary>
            public int Top { get; set; }
            /// <summary>Правая толщина линии</summary>
            public int Right { get; set; }
            /// <summary>Нижняя толщина линии</summary>
            public int Bottom { get; set; }
            #endregion
        }
    }

}

