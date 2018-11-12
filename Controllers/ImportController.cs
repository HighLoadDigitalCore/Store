using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Controllers
{
    public class ImportController : Controller
    {
        protected const int ImportParallelismDegree = 1;

        [HttpGet]
        [AuthorizeMaster]
        public JsonResult loadInfo(string name)
        {
            return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = ParseringInfo.Create(name.IsFilled() ? name : PartnerUID)
                };
        }
        #region Заливка универсальная

        public ActionResult ChangeOrder(string Type, int ProductID, int CategoryID)
        {
            var db = new DB();
            var prod = db.StoreProductsToCategories.FirstOrDefault(x => x.ProductID == ProductID);
            if (prod != null)
            {
                if (Type == "up")
                {
                    var prev =
                        db.StoreProductsToCategories.Where(x => x.CategoryID == CategoryID && !x.StoreProduct.Deleted)
                            .OrderBy(x => x.OrderNum).ToList()
                            .LastOrDefault(x => x.OrderNum < prod.OrderNum);
                    if (prev != null)
                    {
                        var prevOrder = prev.OrderNum;
                        prev.OrderNum = prod.OrderNum;
                        prod.OrderNum = prevOrder;
                    }
                }
                if (Type == "down")
                {
                    var next =
                        db.StoreProductsToCategories.Where(x => x.CategoryID == CategoryID && !x.StoreProduct.Deleted)
                            .OrderBy(x => x.OrderNum)
                            .FirstOrDefault(x => x.OrderNum > prod.OrderNum);
                    if (next != null)
                    {
                        var nextOrder = next.OrderNum;
                        next.OrderNum = prod.OrderNum;
                        prod.OrderNum = nextOrder;
                    }
                }
                db.SubmitChanges();
            }
            return new ContentResult();
        }

        public ActionResult SavePrice(int ID, int? Price)
        {
            if (!Price.HasValue)
                return new ContentResult() { Content = "0" };
            var db = new DB();
            var prod = db.StoreProducts.FirstOrDefault(x => x.ID == ID);
            if (prod == null)
                return new ContentResult() { Content = "0" };

            prod.Price = (decimal)Price;
            db.SubmitChanges();

            return new ContentResult() { Content = "1" };
        }


        [HttpGet]
        [AuthorizeMaster]
        public ActionResult ProductImport(int Category)
        {
            if (!string.IsNullOrEmpty((TempData["Error"] ?? "").ToString()))
            {
                ModelState.AddModelError("", TempData["Error"].ToString());
            }

            ViewBag.Products =
                CatalogBrowser.GetCategory(Category)
                    .ProductList;
            return View(new ImportInfo { AdditionalPath = "/content/Catalog/", DeleteExpired = false, CategoryID = Category });


        }



        public class JsonPriceEntry
        {
            public string name { get; set; }
            public decimal? data { get; set; }
            public bool needround { get; set; }
            public bool iseuro { get; set; }
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult SavePriceLine(int ProductID, string Name, string Value)
        {
            var db = new DB();
            var product = db.StoreProducts.FirstOrDefault(x => x.ID == ProductID);
            if (product != null)
            {

                if (Value.IsNullOrEmpty() || (Name.Contains("PriceBase") && Value.IsDecimal() && Value.ToDecimal() == 0))
                {
                    product.SetPropertyValue(Name, null);
                    product.SitePrice = product.PriceModule.SitePrice;
                    product.Price = product.PriceModule.SitePrice;
                    db.SubmitChanges();
                }
                else if (Value.IsDecimal())
                {
                    product.SetPropertyValue(Name, Value.ToDecimal());
                    product.SitePrice = product.PriceModule.SitePrice;
                    product.Price = product.PriceModule.SitePrice;
                    db.SubmitChanges();
                }

                return new JsonResult()
                {
                    Data =
                        new List<JsonPriceEntry>
                        {

                            new JsonPriceEntry() {name = "PriceBaseEUR", data = product.PriceBaseEUR, iseuro = true},
                            new JsonPriceEntry() {name = "PriceBaseRUR", data = product.PriceBaseRUR, needround = true},
                            new JsonPriceEntry() {name = "BuyingRate", data = product.PriceModule.BuyingRate},
                            new JsonPriceEntry() {name = "BuyingPrice", data = product.PriceModule.BuyingPrice, needround = true},
                            new JsonPriceEntry() {name = "ProfitRate", data = product.PriceModule.ProfitRate},
                            new JsonPriceEntry() {name = "DiscountRate", data = product.PriceModule.DiscountRate},
                            new JsonPriceEntry() {name = "SitePrice", data = product.PriceModule.SitePrice, needround = true},
                            new JsonPriceEntry() {name = "ProfitSum", data = product.PriceModule.ProfitSum, needround = true},
                            new JsonPriceEntry() {name = "Price", data = product.Price, needround = true}
                        }

                };
            }
            return new ContentResult();
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult DeleteProduct(int ProductID)
        {
            var db = new DB();
            var prod = db.StoreProducts.FirstOrDefault(x => x.ID == ProductID);
            if (prod != null)
            {
                prod.Deleted = true;
                db.SubmitChanges();
            }
            return new ContentResult() { Content = "" };
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult ProductImportPrice(HttpPostedFileBase prices, int Category,
            FormCollection collection)
        {
            ViewBag.Products = CatalogBrowser.GetCategory(Category).ProductList;
            var path = Server.CreateDir("/Temp/Universal/");
            string target = "";
            if (prices != null && prices.ContentLength > 0)
            {
                string filePath = Path.Combine(path, Path.GetFileName(prices.FileName));
                target = filePath;
                prices.SaveAs(filePath);
            }
            if (target.IsNullOrEmpty())
            {
                TempData["Error"] = "Необходимо выбрать файл";
                return RedirectToAction("ProductImport", new { Category = Category });
            }
            else
            {
                var unzipper = new WebUnzipper(path, "", target);
                bool success = unzipper.GetFile();
                if (!success)
                {
                    TempData["Error"] = unzipper.Info.ErrorList;
                    return RedirectToAction("ProductImport", new { Category = Category });
                }
                else
                {
                    string xlsPath = unzipper.ResultFileName;
                    if (Translitter.IsRussian(xlsPath))
                    {
                        xlsPath = Translitter.RenameFileToTranslit(xlsPath);
                    }
                    ParseringInfo info = ParseringInfo.Create(PartnerUID);
                    if (info.StartDate.HasValue)
                        info = ParseringInfo.Reset(PartnerUID);
                    info.AddMessage(string.Format("Запуск обработки в {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm")));


                    var il = ParseToPriceList(xlsPath);
                    UpdatePrices(il, null, null, null);



                }
            }



            return RedirectToAction("ProductImport", new { Category = Category, started = "true" });

        }
        [HttpPost]
        [AuthorizeMaster]
        public ActionResult ProductImportRels(HttpPostedFileBase rels, int Category,
            FormCollection collection)
        {
            ViewBag.Products = CatalogBrowser.GetCategory(Category).ProductList;
            var path = Server.CreateDir("/Temp/Universal/");
            string target = "";
            if (rels != null && rels.ContentLength > 0)
            {
                string filePath = Path.Combine(path, Path.GetFileName(rels.FileName));
                target = filePath;
                rels.SaveAs(filePath);
            }
            if (target.IsNullOrEmpty())
            {
                TempData["Error"] = "Необходимо выбрать файл";
                return RedirectToAction("ProductImport", new { Category = Category });
            }
            else
            {
                var unzipper = new WebUnzipper(path, "", target);
                bool success = unzipper.GetFile();
                if (!success)
                {
                    TempData["Error"] = unzipper.Info.ErrorList;
                    return RedirectToAction("ProductImport", new { Category = Category });
                }
                else
                {
                    string xlsPath = unzipper.ResultFileName;
                    if (Translitter.IsRussian(xlsPath))
                    {
                        xlsPath = Translitter.RenameFileToTranslit(xlsPath);
                    }
                    ParseringInfo info = ParseringInfo.Create(PartnerUID);
                    if (info.StartDate.HasValue)
                        info = ParseringInfo.Reset(PartnerUID);
                    info.AddMessage(string.Format("Запуск обработки в {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm")));



                    var il = ParseToRelsList(xlsPath);
                    UpdateRels(il, null, null, null);



                }
            }



            return RedirectToAction("ProductImport", new { Category = Category, started = "true" });

        }


        [HttpPost]
        [AuthorizeMaster]
        public ActionResult ProductImport(HttpPostedFileBase file, HttpPostedFileBase picts, ImportInfo di,
            FormCollection collection)
        {
            ViewBag.Products = CatalogBrowser.GetCategory(di.CategoryID ?? 1).ProductList;
            var path = Server.CreateDir("/Temp/Universal/");
            string target = "";
            if (!string.IsNullOrEmpty(collection["pict"]))
            {
                if (picts == null || picts.ContentLength == 0)
                {
                    ModelState.AddModelError("", "Необходимо выбрать файл");
                    return View(di);
                }
                if (picts.ContentLength > 0)
                {
                    path = Server.CreateDir("/content/Catalog/");
                    var list = WebUnzipper.UnzipAllTo("/content/Catalog/", picts);

                    var endings = new List<string>();
                    for (int i = 1; i <= 100; i++)
                    {
                        endings.Add("-" + i);
                    }
                    var orderedList = list.Distinct().ToDictionary(x => x, x => Path.GetFileNameWithoutExtension(x) ?? "")
                        .ToDictionary(
                            x => x.Key,
                                x => endings.Any(x.Value.EndsWith)
                                    ? int.Parse(endings.First(x.Value.EndsWith).Replace("-", ""))
                                    : 0);

                    


                    var db = new DB();
                    foreach (var item in orderedList.OrderBy(x=> x.Value).Select(x=> x.Key))
                    {
                        var slug = Path.GetFileNameWithoutExtension(item) ?? "";

                        var prod = db.StoreProducts.Where(x => x.Slug == slug).ToList();

                        if (!prod.Any())
                        {
                            if (endings.Any(x => slug.EndsWith(x)))
                            {
                                slug =
                                    slug.Substring(0, slug.Length - endings.First(x => slug.EndsWith(x)).Length);
                            }

/*
                            if (slug.EndsWith("-1") || slug.EndsWith("-2") || slug.EndsWith("-3") || slug.EndsWith("-4") ||
                                slug.EndsWith("-5") || slug.EndsWith("-6") || slug.EndsWith("-7") || slug.EndsWith("-8") ||
                                slug.EndsWith("-9"))
                            {
                                slug =
                                    slug.Substring(0, slug.Length - 2);
                            }
                            else if (slug.EndsWith("-10") || slug.EndsWith("-11") || slug.EndsWith("-12") || slug.EndsWith("-13") || slug.EndsWith("-14") ||
                                slug.EndsWith("-15") || slug.EndsWith("-16") || slug.EndsWith("-17") || slug.EndsWith("-18") ||
                                slug.EndsWith("-19") || slug.EndsWith("-20"))
                            {
                                slug =
                                    slug.Substring(0, slug.Length - 3);
                            }
*/


                            prod = db.StoreProducts.Where(x => x.Slug == slug).ToList();
                        }


                        if (prod.Any())
                        {
                            foreach (var product in prod)
                            {
                                var exist = false;
                                var existImages = db.StoreImages.Where(x => x.ProductID == product.ID).ToList();
                                foreach (var image in existImages)
                                {
                                    var imp = image.UrlPath.Split<string>("/");
                                    if (imp.Contains(item))
                                    {
                                        exist = true;
                                    }
                                }
                                if (!exist)
                                {
                                    var on = db.StoreImages.Count() + 1;
                                    var entry = new StoreImage()
                                    {
                                        Enabled = true,
                                        Description = product.NameOrDef,
                                        UrlPath = "/content/Catalog/" + item,
                                        UrlPathThumbs = "/content/Catalog/" + item,
                                        OrderNum = on,
                                        ProductID = product.ID,
                                        Alt = product.Name + " "+product.Article

                                    };
                                    db.StoreImages.InsertOnSubmit(entry);
                                    db.SubmitChanges();
                                }
                            }

                        }
                    }
                }
                if (target.IsNullOrEmpty())
                {
                    ModelState.AddModelError("", "Все файлы успешно загружены");
                    return View(di);
                }

            }


            if (file != null && file.ContentLength > 0)
            {
                string filePath = Path.Combine(path, Path.GetFileName(file.FileName));
                target = filePath;
                file.SaveAs(filePath);
            }
            if (target.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Необходимо выбрать файл");
                return View(di);
            }

            var unzipper = new WebUnzipper(path, "", target);
            bool success = unzipper.GetFile();
            if (!success)
            {
                ModelState.AddModelError("", unzipper.Info.ErrorList);
                return View(di);
            }


            /*
                        var req = StoreImporter.RequiredColumns.Where(x => x.IsRequired).ToList();
                        var exist = new DB().StoreImporters.ToList();
                        var join = req.Join(exist, x => x.Key, y => y.ColumnName, (x, y) => y);
                        if (join.Select(x => x.ColumnName).Distinct().Count() != req.Count)
                        {

                            ModelState.AddModelError("",
                                                     "Необходимо указать в настройках импорта соответствие для колонок: " +
                                                     req.Where(x => exist.All(z => z.ColumnName != x.Key))
                                                        .Select(x => x.Name)
                                                        .JoinToString(", "));
                            return View(di);
                        }
            */


            string xlsPath = unzipper.ResultFileName;
            if (Translitter.IsRussian(xlsPath))
            {
                xlsPath = Translitter.RenameFileToTranslit(xlsPath);
            }
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            if (info.StartDate.HasValue)
                info = ParseringInfo.Reset(PartnerUID);
            info.AddMessage(string.Format("Запуск обработки в {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm")));

            var il = ParseToUniversalProductList(xlsPath, di.AdditionalPath);
            UpdateCategory(il, null, null, di);


            return Redirect(HttpContext.Request.RawUrl + "&started=true");

            return View(di);
        }

        [HttpGet]
        [AuthorizeMaster]
        [MenuItem("Загрузка каталога", 45, 4)]
        public ActionResult Universal()
        {
            return View(new ImportInfo { AdditionalPath = "/content/Catalog/", DeleteExpired = false });
        }

        [HttpPost]
        [AuthorizeMaster]
        public ActionResult Universal(HttpPostedFileBase file, HttpPostedFileBase ZipImages, ImportInfo di, FormCollection collection)
        {
            var path = Server.CreateDir("/Temp/Universal/");
            string target = "";
            if (file != null && file.ContentLength > 0)
            {
                string filePath = Path.Combine(path, Path.GetFileName(file.FileName));
                target = filePath;
                file.SaveAs(filePath);
            }
            if (target.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Необходимо выбрать файл");
                return View(di);
            }

            if (ZipImages != null && ZipImages.ContentLength > 0)
            {
                WebUnzipper.UnzipAllTo(di.AdditionalPath, ZipImages);
            }

            var unzipper = new WebUnzipper(path, "", target);
            bool success = unzipper.GetFile();
            if (!success)
            {
                ModelState.AddModelError("", unzipper.Info.ErrorList);
                return View(di);
            }


            var req = StoreImporter.RequiredColumns.Where(x => x.IsRequired).ToList();
            var exist = new DB().StoreImporters.ToList();
            var join = req.Join(exist, x => x.Key, y => y.ColumnName, (x, y) => y);
            if (join.Select(x => x.ColumnName).Distinct().Count() != req.Count)
            {

                ModelState.AddModelError("",
                                         "Необходимо указать в настройках импорта соответствие для колонок: " +
                                         req.Where(x => exist.All(z => z.ColumnName != x.Key))
                                            .Select(x => x.Name)
                                            .JoinToString(", "));
                return View(di);
            }


            string xlsPath = unzipper.ResultFileName;
            if (Translitter.IsRussian(xlsPath))
            {
                xlsPath = Translitter.RenameFileToTranslit(xlsPath);
            }
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            if (info.StartDate.HasValue)
                info = ParseringInfo.Reset(PartnerUID);
            info.AddMessage(string.Format("Запуск обработки в {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm")));

            var il = ParseToUniversalList(xlsPath, di.AdditionalPath);
            UpdateCatalog(il, null, null, di);


            return View(di);

        }

        private List<IExportRow> ParseToUniversalList(string xlsPath, string additionalPath)
        {
            var list = new List<IExportRow>();
            using (var fs = new FileStream(xlsPath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var settings = new DB().StoreImporters.ToList();
                    for (int i = settings.First().RowNum - 1; i <= sheet.LastRowNum; i++)
                    {
                        var rows = settings.Select(x => x.ToImportColumn(sheet.GetRow(i))).ToList();
                        var ir = new StoreImporterRow(rows);
                        list.Add(ir);

                    }
                }
            }
            //var cats = list.Where(x => (x as StoreImporterRow).GetValueFor("ID").ToInt() > 0).ToList();
            return list;
        }

        private List<IExportRow> ParseToPriceList(string xlsPath)
        {
            var list = new List<IExportRow>();
            using (var fs = new FileStream(xlsPath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var settings = new List<StoreImporter>()
                    {

                        new StoreImporter()
                        {
                            ColumnName = "PriceBaseEUR",
                            ColumnNum = 1,
                        },new StoreImporter()
                        {
                            ColumnName = "PriceBaseRUR",
                            ColumnNum = 2,
                        },new StoreImporter()
                        {
                            ColumnName = "BuyingRate",
                            ColumnNum = 3,
                        }
                        ,new StoreImporter()
                        {
                            ColumnName = "ProfitRate",
                            ColumnNum = 4,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "DiscountRate",
                            ColumnNum = 5,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Article",
                            ColumnNum = 6,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Name",
                            ColumnNum =7,
                        }
                    };

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var line = sheet.GetRow(i);
                        if (line != null)
                        {
                            var rows = settings.Select(x => x.ToImportColumn(line)).ToList();
                            var ir = new StoreImporterRow(rows);
                            list.Add(ir);
                        }

                    }
                }
            }
            //var cats = list.Where(x => (x as StoreImporterRow).GetValueFor("ID").ToInt() > 0).ToList();
            return list.Where(x => (x as StoreImporterRow).GetValueFor("Article").IsFilled()).ToList();
        }
        private List<IExportRow> ParseToRelsList(string xlsPath)
        {
            var list = new List<IExportRow>();
            using (var fs = new FileStream(xlsPath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var settings = new List<StoreImporter>()
                    {

                        new StoreImporter()
                        {
                            ColumnName = "Recommend",
                            ColumnNum = 1,
                        },new StoreImporter()
                        {
                            ColumnName = "Related",
                            ColumnNum = 2,
                        },new StoreImporter()
                        {
                            ColumnName = "Similar",
                            ColumnNum = 3,
                        },
                     
                        new StoreImporter()
                        {
                            ColumnName = "Article",
                            ColumnNum = 4,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Name",
                            ColumnNum =5,
                        }
                    };

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var line = sheet.GetRow(i);
                        if (line != null)
                        {
                            var rows = settings.Select(x => x.ToImportColumn(line)).ToList();
                            var ir = new StoreImporterRow(rows);
                            list.Add(ir);
                        }

                    }
                }
            }
            //var cats = list.Where(x => (x as StoreImporterRow).GetValueFor("ID").ToInt() > 0).ToList();
            return list.Where(x => (x as StoreImporterRow).GetValueFor("Article").IsFilled()).ToList();
        }

        private List<IExportRow> ParseToUniversalProductList(string xlsPath, string additionalPath)
        {
            var list = new List<IExportRow>();
            using (var fs = new FileStream(xlsPath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var startDataNum = 5;
                    var settings = new List<StoreImporter>()
                    {

                        new StoreImporter()
                        {
                            ColumnName = "RecType",
                            ColumnNum = startDataNum-4,
                            RowNum = 2,
                        },new StoreImporter()
                        {
                            ColumnName = "FolderNum",
                            ColumnNum = startDataNum-3,
                            RowNum = 2,
                        },new StoreImporter()
                        {
                            ColumnName = "ProductFolders",
                            ColumnNum = startDataNum-2,
                            RowNum = 2,
                        }
                        ,new StoreImporter()
                        {
                            ColumnName = "ShortName",
                            ColumnNum = startDataNum -1,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Name",
                            ColumnNum = startDataNum,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "URL",
                            ColumnNum = startDataNum+1,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "PageTitle",
                            ColumnNum = startDataNum+2,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "PageKeywords",
                            ColumnNum = startDataNum+3,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "PageDescription",
                            ColumnNum = startDataNum+4,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "H1",
                            ColumnNum = startDataNum+5,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "H2",
                            ColumnNum = startDataNum+6,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "H3",
                            ColumnNum = startDataNum+7,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "TextUnderH2",
                            ColumnNum = startDataNum+8,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "TextUnderH3",
                            ColumnNum = startDataNum+9,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Artikul",
                            ColumnNum = startDataNum+10,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Weight",
                            ColumnNum = startDataNum+11,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Volume",
                            ColumnNum = startDataNum+12,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Discount",
                            ColumnNum = startDataNum+13,
                            RowNum = 2,
                        },

                        new StoreImporter()
                        {
                            ColumnName = "Visible",
                            ColumnNum = startDataNum+14,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Price",
                            ColumnNum = startDataNum+15,
                            RowNum = 2,
                        },
                        new StoreImporter()
                        {
                            ColumnName = "SearchTags",
                            ColumnNum = startDataNum+16,
                            RowNum = 2,
                        },
                    };
                    var header = sheet.GetRow(1);
                    for (int i = startDataNum + 16; i < 100; i++)
                    {
                        var cell = header.GetCell(i);
                        if (cell != null && !cell.StringCellValue.IsNullOrEmpty())
                        {
                            settings.Add(new StoreImporter()
                            {
                                ColumnName = "Character",
                                Header = cell.StringCellValue,
                                ColumnNum = i + 1,
                                RowNum = 2,
                            });
                        }
                        else
                        {
                            break;

                        }

                    }
                    for (int i = settings.First().RowNum; i <= sheet.LastRowNum; i++)
                    {
                        var line = sheet.GetRow(i);
                        if (line != null)
                        {
                            var rows = settings.Select(x => x.ToImportColumn(line)).ToList();
                            var ir = new StoreImporterRow(rows);
                            list.Add(ir);
                        }

                    }
                }
            }
            //var cats = list.Where(x => (x as StoreImporterRow).GetValueFor("ID").ToInt() > 0).ToList();
            return list.Where(x => (x as StoreImporterRow).GetValueFor("Name").IsFilled()).ToList();
        }

        private void UpdateCatalog(List<IExportRow> importList, PrepareRecordDelegate prepareRecordFunc = null, PostProcessingDelegate postProcessingFunc = null, ImportInfo dl = null)
        {
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.StartDate = DateTime.Now;

            Thread workingThread;
            if (!importList.Any()) return;
            workingThread = new Thread(ThreadFuncUpdate);

            workingThread.Start(new ThreadCatalogParserInfo()
                {
                    Context = System.Web.HttpContext.Current,
                    DataList = importList,
                    PrepareRecordFunc = prepareRecordFunc,
                    PostProcessingFunc = postProcessingFunc,
                    ImportInfo = dl
                });
        }



        private void UpdatePrices(List<IExportRow> importList, PrepareRecordDelegate prepareRecordFunc = null, PostProcessingDelegate postProcessingFunc = null, ImportInfo dl = null)
        {
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.StartDate = DateTime.Now;

            Thread workingThread;
            if (!importList.Any()) return;
            workingThread = new Thread(ThreadFuncUpdatePrices);

            workingThread.Start(new ThreadCatalogParserInfo()
            {
                Context = System.Web.HttpContext.Current,
                DataList = importList,
                PrepareRecordFunc = prepareRecordFunc,
                PostProcessingFunc = postProcessingFunc,
                ImportInfo = dl
            });
        }
        private void UpdateRels(List<IExportRow> importList, PrepareRecordDelegate prepareRecordFunc = null, PostProcessingDelegate postProcessingFunc = null, ImportInfo dl = null)
        {
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.StartDate = DateTime.Now;

            Thread workingThread;
            if (!importList.Any()) return;
            workingThread = new Thread(ThreadFuncUpdateRels);

            workingThread.Start(new ThreadCatalogParserInfo()
            {
                Context = System.Web.HttpContext.Current,
                DataList = importList,
                PrepareRecordFunc = prepareRecordFunc,
                PostProcessingFunc = postProcessingFunc,
                ImportInfo = dl
            });
        }



        private void UpdateCategory(List<IExportRow> importList, PrepareRecordDelegate prepareRecordFunc = null, PostProcessingDelegate postProcessingFunc = null, ImportInfo dl = null)
        {
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.StartDate = DateTime.Now;

            Thread workingThread;
            if (!importList.Any()) return;
            workingThread = new Thread(ThreadFuncUpdateCategory);

            workingThread.Start(new ThreadCatalogParserInfo()
                {
                    Context = System.Web.HttpContext.Current,
                    DataList = importList,
                    PrepareRecordFunc = prepareRecordFunc,
                    PostProcessingFunc = postProcessingFunc,
                    ImportInfo = dl
                });
        }


        public const string PartnerUID = "Undef";
        private void ThreadFuncUpdateCategory(object context)
        {
            const int startLevel = 2;
            const int ThreadDelay = 500;
            DB dbx = new DB();
            var thi = (ThreadCatalogParserInfo)context;
            System.Web.HttpContext.Current = thi.Context;
            // var processed = new List<BookSaleCatalog>();
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.Created = info.Updated = info.Deleted = info.Dirs = info.Prepared = 0;
            info.EndDate = null;
            info.StartDate = DateTime.Now;
            info.getProcessedList().Clear();


            var catalogRoot = dbx.StoreCategories.First(x => x.ID == thi.ImportInfo.CategoryID);

            var foldersList = new Dictionary<int, int>();


            thi.DataList.Where(x => (x as StoreImporterRow).GetValueFor("ID").ToInt() == 0).AsParallel().WithDegreeOfParallelism(ImportParallelismDegree).Select(d =>
            {
                try
                {
                    Thread.Sleep(ThreadDelay);


                    var data = (StoreImporterRow)d;
                    var dbt = new DB();

                    var rt = data.GetValueFor("RecType");
                    if (rt.ToLower().Trim() == "папка")
                    {
                        var folder = dbt.StoreCategories.FirstOrDefault(x => x.Slug == HttpUtility.UrlDecode(data.GetValueFor("URL")) /*&& x.ParentID == catalogRoot.ID*/);
                        if (folder == null)
                        {
                            folder = new StoreCategory()
                            {
                                LastMod = DateTime.Now,
                                Slug = data.GetValueFor("URL"),
                                ShowArticles = true,
                                ShowInCatalog = true,
                                ShowInBreadcrumb = true,
                                ShowInMenu = true,
                                ShowSlider = true,
                                ShowBigIcons = false


                            };
                            dbt.StoreCategories.InsertOnSubmit(folder);
                        }
                        folder.Deleted = false;
                        folder.ParentID = catalogRoot.ID;
                        folder.Name = data.GetValueFor("Name");
                        folder.PageTitle = data.GetValueFor("PageTitle");
                        folder.PageKeywords = data.GetValueFor("PageKeywords");
                        folder.PageDescription = data.GetValueFor("PageDescription");
                        folder.PageTextH3Upper = data.GetValueFor("TextUnderH2");
                        folder.PageTextH3Lower = data.GetValueFor("TextUnderH3");
                        folder.PageHeader = data.GetValueFor("H1");
                        folder.PageSubHeader = data.GetValueFor("H2");
                        folder.PageHeaderH3 = data.GetValueFor("H3");
                        dbt.SubmitChanges();
                        if (!string.IsNullOrEmpty(data.GetValueFor("FolderNum")))
                        {
                            var num = data.GetValueFor("FolderNum").ToInt();
                            if (num > 0 && !foldersList.ContainsKey(num))
                                foldersList.Add(num, folder.ID);
                        }
                    }

                    else
                    {
                        var saleItem =
                            dbt.StoreProducts.FirstOrDefault(
                                x => x.Article == data.GetValueFor("Artikul"));
                        if (thi.PrepareRecordFunc != null)
                        {
                            thi.PrepareRecordFunc(ref d);
                        }
                        if (saleItem == null)
                        {
                            info.Created++;
                            saleItem = new StoreProduct() { AddDate = DateTime.Now, LastMod = DateTime.Now };
                            dbt.StoreProducts.InsertOnSubmit(saleItem);
                        }
                        else
                        {
                            info.Updated++;
                        }
                        if (info.IsItemProcessed(saleItem.ID))
                        {
                            return false;
                        }
                        saleItem.Deleted = false;
                        saleItem.ViewCount = 0;
                        saleItem.Article = data.GetValueFor("Artikul");
                        saleItem.Name = data.GetValueFor("Name");
                        saleItem.ShortName = data.GetValueFor("ShortName");
                        saleItem.Price = data.GetValueFor("Price").ToDecimal();
                        saleItem.Discount = data.GetValueFor("Discount").ToInt();
                        saleItem.IsActive = data.GetValueFor("Visible").ToBool();
                        saleItem.Description = data.GetValueFor("TextUnderH2");
                        saleItem.Slug = data.GetValueFor("URL").LastSegment();
                        saleItem.PageTitle = data.GetValueFor("PageTitle"); //saleItem.Name;
                        saleItem.PageDescription = data.GetValueFor("PageDescription");
                        saleItem.PageKeywords = data.GetValueFor("PageKeywords");
                        saleItem.Volume = data.GetValueFor("Volume").ToDecimal();
                        saleItem.Weight = data.GetValueFor("Weight").ToDecimal();
                        saleItem.DescrptionLower = data.GetValueFor("TextUnderH3");
                        saleItem.PageH1 = data.GetValueFor("H1");
                        saleItem.PageH2 = data.GetValueFor("H2");
                        saleItem.PageH3 = data.GetValueFor("H3");
                        saleItem.SearchWords = data.GetValueFor("SearchTags");

                        var menu = (data.GetValueFor("RelatedCategories") ?? "");
                        saleItem.RelatedCategories = menu;


                        dbt.StoreProducts.Context.SubmitChanges();
                        info.AddProcessedItem(saleItem.ID);

                        var newRels = new[] { catalogRoot.ID };

                        var newRelsFromXLS = new List<int>();
                        var catNums = data.GetValueFor("ProductFolders").Trim().Split<int>(";").ToList();
                        if (catNums.Any())
                        {
                            newRelsFromXLS.AddRange(from catNum in catNums
                                                    where foldersList.ContainsKey(catNum)
                                                    select /*dbt.StoreCategories.FirstOrDefault(x => x.ID == foldersList[catNum])*/foldersList[catNum] into cat
                                                    where cat != null
                                                    select cat);
                        }
                        if (newRelsFromXLS.Any())
                            newRels = newRelsFromXLS.ToArray();


                        var rels = dbt.StoreProductsToCategories.Where(x => x.ProductID == saleItem.ID).ToList();
                        /*
                                                foreach (var rel in rels)
                                                {
                                                    rel.OrderNum = /*data.GetValueFor("ProductOrder").ToInt()#1# info.Created + info.Updated;
                                                }
                        */
                        var forDel = rels;//.Where(x => !newRels.Contains(x.StoreCategory.SlugOrId));
                        dbt.StoreProductsToCategories.DeleteAllOnSubmit(forDel);
                        dbt.StoreProductsToCategories.Context.SubmitChanges();
                        var forAdd =
                            newRels/*.Where(x => !rels.Select(z => z.StoreCategory.SlugOrId).Contains(x))*/
                                .Select(
                                    x =>
                                        new StoreProductsToCategory
                                        {
                                            CategoryID = x,
                                            ProductID = saleItem.ID,
                                            OrderNum = /*data.GetValueFor("ProductOrder").ToInt()*/
                                                info.Created + info.Updated
                                        });

                        dbt.StoreProductsToCategories.InsertAllOnSubmit(forAdd);
                        dbt.StoreProductsToCategories.Context.SubmitChanges();
                        dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreProductsToCategories);
                        /*
                                            var oldImages = (from storeImage in saleItem.StoreImages
                                                             let abs = thi.Context.Server.MapPath(storeImage.UrlPath)
                                                             where !System.IO.File.Exists(abs)
                                                             select storeImage).ToList();

                                            if (oldImages.Any())
                                            {
                                                dbt.StoreImages.DeleteAllOnSubmit(oldImages);
                                                dbt.StoreImages.Context.SubmitChanges();
                                                info.AddMessage("Удалены отсутсвующие изображение для товара " + saleItem.Name, false);

                                            }
                                            dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreImages);
                        */

                        /*
                                            var imss = new[]
                                                {
                                                    new CatalogImageSearcher(saleItem.Article, thi.ImportInfo.AdditionalPath, thi.Context),
                                                    new CatalogImageSearcher(saleItem.Slug, thi.ImportInfo.AdditionalPath, thi.Context)
                                                };

                                            foreach (var searcher in imss)
                                            {



                                                if (searcher.HasImage && !saleItem.StoreImages.ToList().Any(z => z.UrlPath.EndsWith(searcher.RelativeFile.LastSegment())))
                                                {
                                                    var img = new StoreImage()
                                                    {
                                                        Description = saleItem.Name,
                                                        Enabled = true,
                                                        UrlPath = searcher.RelativeFile,
                                                        UrlPathThumbs = searcher.RelativeFile,
                                                        OrderNum = dbt.StoreImages.Count() + 1,
                                                        ProductID = saleItem.ID
                                                    };
                                                    dbt.StoreImages.InsertOnSubmit(img);
                                                    dbt.StoreImages.Context.SubmitChanges();
                                                    info.AddMessage("Загружено новое изображение для товара " + saleItem.Name, false);
                                                }
                                            }*/
                        var chars = data.Where(x => x.ColumnName == "Character");

                        if (saleItem.StoreCharacterToProducts.Any())
                        {
                            dbt.StoreCharacterToProducts.DeleteAllOnSubmit(saleItem.StoreCharacterToProducts);
                            dbt.SubmitChanges();
                            dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreCharacterToProducts);
                        }


                        foreach (var c in chars)
                        {
                            if (c.XlsValue.IsNullOrEmpty())
                                continue;

                            var group =
                                dbt.StoreCharacters.FirstOrDefault(x => x.Name.ToLower() == c.Header.ToLower().Trim());
                            if (group == null)
                            {
                                group = new StoreCharacter() { Name = c.Header.ToLower().Trim()/*.ToNiceForm()*/ };
                                dbt.StoreCharacters.InsertOnSubmit(group);
                            }
                            var value =
                                group.StoreCharacterValues.FirstOrDefault(
                                    x => x.Value.ToLower() == c.XlsValue.ToLower().Trim());
                            if (value == null)
                            {
                                value = new StoreCharacterValue()
                                {
                                    StoreCharacter = group,
                                    Value = c.XlsValue.ToLower().Trim()/*.ToNiceForm()*/
                                };
                                dbt.StoreCharacterValues.InsertOnSubmit(value);
                            }
                            var rel = value.StoreCharacterToProducts.FirstOrDefault(x => x.ProductID == saleItem.ID);
                            if (rel == null)
                            {
                                rel = new StoreCharacterToProduct()
                                {
                                    ProductID = saleItem.ID,
                                    StoreCharacterValue = value
                                };
                                dbt.StoreCharacterToProducts.InsertOnSubmit(rel);
                            }
                            dbt.SubmitChanges();

                        }
                        dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreCharacterToProducts);
                        var tags = data.GetValueFor("SearchTags").Split<string>(";").Select(x => x.ToLower().Trim());
                        var tagsForDel =
                            saleItem.StoreProductTagRels.ToList()
                                .Where(x => !tags.Contains(x.StoreProductTag.Tag.ToLower()))
                                .ToList();
                        var tagsForAdd =
                            tags.Where(
                                x =>
                                    !saleItem.StoreProductTagRels.Select(z => z.StoreProductTag.Tag.ToLower())
                                        .Contains(x.ToLower())).ToList();

                        if (tagsForDel.Any())
                        {
                            dbt.StoreProductTagRels.DeleteAllOnSubmit(tagsForDel);
                            dbt.SubmitChanges();

                        }
                        foreach (var tag in tagsForAdd)
                        {
                            var exist = dbt.StoreProductTags.FirstOrDefault(x => x.Tag.ToLower() == tag.ToLower().Trim());
                            if (exist == null)
                            {
                                exist = new StoreProductTag() { Tag = tag.ToLower().Trim() };
                                dbt.StoreProductTags.InsertOnSubmit(exist);
                            }
                            dbt.StoreProductTagRels.InsertOnSubmit(new StoreProductTagRel()
                            {
                                StoreProduct = saleItem,
                                StoreProductTag = exist
                            });

                        }
                        dbt.SubmitChanges();




                        if (thi.PostProcessingFunc != null)
                        {
                            thi.PostProcessingFunc(saleItem.ID, data, thi.ImportInfo);
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    info.AddMessage(ex.Message);
                    info.AddMessage(ex.StackTrace);
                    return false;
                }
            }).Count();
            if (thi.ImportInfo.DeleteExpired)
            {
                var exists = dbx.getIntListByJoinedString(info.getProcessedList().JoinToString(","), ",")
                   .Join(dbx.StoreProducts, x => x.ID, y => y.ID, (x, y) => y);
                var prodsForDel =
                    dbx.StoreProducts.Where(
                        x =>
                            x.StoreProductsToCategories.Any(z => z.CategoryID == catalogRoot.ID) &&
                            !exists.Any(z => z.ID == x.ID)).ToList();

                foreach (var product in prodsForDel)
                {
                    var p = product;
                    var rels = dbx.StoreProductRelations.Where(x => x.BaseProductID == p.ID || x.RelatedProductID == p.ID);
                    if (rels.Any())
                    {
                        dbx.StoreProductRelations.DeleteAllOnSubmit(rels);
                        dbx.SubmitChanges();
                    }
                    var catrels = dbx.StoreCategoryRelations.Where(x => x.RelatedProductID == p.ID);
                    if (catrels.Any())
                    {
                        dbx.StoreCategoryRelations.DeleteAllOnSubmit(catrels);
                        dbx.SubmitChanges();
                    }
                }
                dbx.StoreProducts.DeleteAllOnSubmit(prodsForDel);
                dbx.SubmitChanges();
            }
            CatalogBrowser.Init().ClearAllCaches();
            info.AddMessage("Обработка завершена.");
            info.EndDate = DateTime.Now;
        }
        private void ThreadFuncUpdatePrices(object context)
        {
            const int startLevel = 2;
            const int ThreadDelay = 500;
            DB dbx = new DB();
            var thi = (ThreadCatalogParserInfo)context;
            System.Web.HttpContext.Current = thi.Context;
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.Created = info.Updated = info.Deleted = info.Dirs = info.Prepared = 0;
            info.EndDate = null;
            info.StartDate = DateTime.Now;
            info.getProcessedList().Clear();



            thi.DataList.AsParallel().WithDegreeOfParallelism(ImportParallelismDegree).Select(d =>
            {
                try
                {
                    Thread.Sleep(ThreadDelay);


                    var data = (StoreImporterRow)d;
                    var dbt = new DB();

                    var p = dbt.StoreProducts.FirstOrDefault(x => x.Article == data.GetValueFor("Article"));
                    if (p != null)
                    {
                        p.PriceBaseEUR = data.GetValueFor("PriceBaseEUR").ToNullableDecimal();
                        p.PriceBaseRUR = data.GetValueFor("PriceBaseRUR").ToNullableDecimal();
                        p.BuyingRate = data.GetValueFor("BuyingRate").IsDecimal()
                            ? data.GetValueFor("BuyingRate").ToDecimal()
                            : 1;
                        p.ProfitRate = data.GetValueFor("ProfitRate").IsDecimal()
                            ? data.GetValueFor("ProfitRate").ToDecimal()
                            : 1;
                        p.DiscountRate = data.GetValueFor("DiscountRate").IsDecimal()
                            ? data.GetValueFor("DiscountRate").ToDecimal()
                            : 1;
                        p.SitePrice = p.PriceModule.SitePrice;
                        p.Price = p.PriceModule.SitePrice;
                        info.Updated++;
                        dbt.SubmitChanges();

                    }

                    return true;
                }
                catch (Exception ex)
                {
                    info.AddMessage(ex.Message);
                    info.AddMessage(ex.StackTrace);
                    return false;
                }
            }).Count();

            CatalogBrowser.Init().ClearAllCaches();
            info.AddMessage("Обработка завершена.");
            info.EndDate = DateTime.Now;
        }
        private void ProcessRelation(IEnumerable<string> similar, string gname, DB db, StoreProduct p)
        {
            var rels =
             similar.Select(
                 slug =>
                     db.StoreProducts.FirstOrDefault(
                         x => x.Slug.ToLower() == slug.ToLower().Trim() || x.Article.Trim() == slug.Trim()))
                 .Where(s => s != null)
                 .ToList();

            var dbr = db.StoreProductRelations.Where(x => x.BaseProductID == p.ID && x.GroupName == gname).ToList();
            var forDel = dbr.Where(x => !rels.Select(z => z.ID).ToList().Contains(x.ID)).ToList();
            if (forDel.Any())
            {
                db.StoreProductRelations.DeleteAllOnSubmit(forDel);
                db.SubmitChanges();
            }
            var forAdd =
                rels.Select(x => x.ID)
                    .Where(x => !dbr.Select(z => z.BaseProductID).ToList().Contains(x))
                    .Join(rels, x => x, y => y.ID, (x, y) => y).ToList();

            if (forAdd.Any())
            {
                db.StoreProductRelations.InsertAllOnSubmit(forAdd.Select(x => new StoreProductRelation() { BaseProductID = p.ID, GroupName = gname, RelatedProductID = x.ID }));
                db.SubmitChanges();
            }

        }
        private void ThreadFuncUpdateRels(object context)
        {
            const int startLevel = 2;
            const int ThreadDelay = 500;
            DB dbx = new DB();
            var thi = (ThreadCatalogParserInfo)context;
            System.Web.HttpContext.Current = thi.Context;
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.Created = info.Updated = info.Deleted = info.Dirs = info.Prepared = 0;
            info.EndDate = null;
            info.StartDate = DateTime.Now;
            info.getProcessedList().Clear();



            thi.DataList.AsParallel().WithDegreeOfParallelism(ImportParallelismDegree).Select(d =>
            {
                try
                {
                    Thread.Sleep(ThreadDelay);


                    var data = (StoreImporterRow)d;
                    var dbt = new DB();

                    var p = dbt.StoreProducts.FirstOrDefault(x => x.Article == data.GetValueFor("Article"));
                    if (p != null)
                    {
                          
                        var similar = data.GetValueFor("Similar").Split<string>(";");
                        var recomend = data.GetValueFor("Recommend").Split<string>(";");
                        var related = data.GetValueFor("Related").Split<string>(";");
                        ProcessRelation(similar, "similar", dbt, p);
                        ProcessRelation(recomend, "recomend", dbt, p);
                        ProcessRelation(related, "related", dbt, p);

                        info.Updated++;
                        dbt.SubmitChanges();

                    }

                    return true;
                }
                catch (Exception ex)
                {
                    info.AddMessage(ex.Message);
                    info.AddMessage(ex.StackTrace);
                    return false;
                }
            }).Count();

            CatalogBrowser.Init().ClearAllCaches();
            info.AddMessage("Обработка завершена.");
            info.EndDate = DateTime.Now;
        }


        private void ThreadFuncUpdate(object context)
        {
            const int startLevel = 2;
            const int ThreadDelay = 500;
            DB dbx = new DB();
            var thi = (ThreadCatalogParserInfo)context;
            System.Web.HttpContext.Current = thi.Context;
            // var processed = new List<BookSaleCatalog>();
            ParseringInfo info = ParseringInfo.Create(PartnerUID);
            info.Created = info.Updated = info.Deleted = info.Dirs = info.Prepared = 0;
            info.EndDate = null;
            info.StartDate = DateTime.Now;
            info.getProcessedList().Clear();

            var cats =
                thi.DataList.Select(
                    x => new { Key = ((StoreImporterRow)x).GetValueFor("ID").ToInt(), Value = x as StoreImporterRow }).ToList()
                   .Where(x => x.Key > startLevel).OrderBy(x => x.Key).ToList();

            var catalogRoot = dbx.StoreCategories.First(x => !x.ParentID.HasValue);
            foreach (var cat in cats)
            {
                Thread.Sleep(ThreadDelay);
                var exist =
                    dbx.StoreCategories.FirstOrDefault(
                        x => x.Slug.ToLower() == cat.Value.GetValueFor("URL").LastSegment().ToLower());

                var pc =
                    cats.FirstOrDefault(
                        x => x.Value.GetValueFor("ID").ToInt() == cat.Value.GetValueFor("ParentID").ToInt());

                StoreCategory parent = null;
                if (pc != null)
                    parent =
                        dbx.StoreCategories.FirstOrDefault(
                            x => x.Slug.ToLower() == pc.Value.GetValueFor("URL").LastSegment());
                if (parent == null)
                    parent = catalogRoot;

                if (exist == null)
                {
                    exist = new StoreCategory()
                    {
                        LastMod = DateTime.Now,
                        ShowBigIcons = false
                    };
                    dbx.StoreCategories.InsertOnSubmit(exist);
                    info.Created++;
                }
                else
                {
                    info.Updated++;
                }
                exist.Deleted = false;
                exist.Parent = parent;
                exist.Name = cat.Value.GetValueFor("PageTitle");
                exist.PageTitle = exist.Name;
                exist.PageKeywords = cat.Value.GetValueFor("PageKeywords");
                exist.PageDescription = cat.Value.GetValueFor("PageDescription");
                exist.Slug = cat.Value.GetValueFor("URL").LastSegment().ToLower();
                exist.OrderNum = cat.Value.GetValueFor("CategoryOrder").ToInt();
                exist.PageHeader = cat.Value.GetValueFor("H1");
                exist.PageSubHeader = cat.Value.GetValueFor("H2");
                exist.PageHeaderH3 = cat.Value.GetValueFor("H3");
                exist.Description = cat.Value.GetValueFor("TextUnderH2");
                exist.PageTextH3Lower = cat.Value.GetValueFor("TextUnderH3");
                exist.OrderNum = cat.Value.GetValueFor("CategoryOrder").ToInt();
                var iss = new CatalogImageSearcher(exist.Slug, thi.ImportInfo.AdditionalPath);
                exist.Image = iss.HasImage ? iss.FileContent : null;
                dbx.SubmitChanges();

                var related = (cat.Value.GetValueFor("RelatedProductsSame") ?? "").Split<string>(";").ToList();
                if (related.Any())
                {
                    var cids =
                        related.Select(x => dbx.StoreProducts.FirstOrDefault(z => z.Slug == x || z.ID == x.ToInt()))
                            .Where(x => x != null && !x.Deleted).Select(x => x.ID).ToList();
                    var rels =

                                dbx.StoreCategoryRelations.Where(
                                    z => z.BaseCategoryID == exist.ID && z.GroupName == "related").ToList();


                    var forDel = rels.Where(x => !cids.Contains(x.RelatedProductID ?? 0) || x.StoreProduct == null);
                    var forAdd = cids.Where(x => rels.All(z => z.RelatedProductID != x)).ToList();
                    if (forDel.Any())
                    {
                        dbx.StoreCategoryRelations.DeleteAllOnSubmit(forDel);
                        dbx.SubmitChanges();
                    }
                    if (forAdd.Any())
                    {
                        dbx.StoreCategoryRelations.InsertAllOnSubmit(
                            forAdd.Select(
                                x =>
                                    new StoreCategoryRelation()
                                    {
                                        BaseCategoryID = exist.ID,
                                        GroupName = "related",
                                        RelatedProductID = x
                                    }));
                        dbx.SubmitChanges();
                    }
                }


                var similar = (cat.Value.GetValueFor("RelatedProductsSimilar") ?? "").Split<string>(";").ToList();
                if (similar.Any())
                {
                    var cids =
                      similar.Select(x => dbx.StoreProducts.FirstOrDefault(z => z.Slug == x || z.ID == x.ToInt()))
                          .Where(x => x != null && !x.Deleted).Select(x => x.ID).ToList();
                    var rels =

                                dbx.StoreCategoryRelations.Where(
                                    z => z.BaseCategoryID == exist.ID && z.GroupName == "similar").ToList();
                    var forDel = rels.Where(x => !cids.Contains(x.RelatedProductID ?? 0) || x.StoreProduct == null).ToList();
                    var forAdd = cids.Where(x => rels.All(z => z.RelatedProductID != x)).ToList();
                    if (forDel.Any())
                    {
                        dbx.StoreCategoryRelations.DeleteAllOnSubmit(forDel);
                        dbx.SubmitChanges();
                    }
                    if (forAdd.Any())
                    {
                        dbx.StoreCategoryRelations.InsertAllOnSubmit(
                            forAdd.Select(
                                x =>
                                    new StoreCategoryRelation()
                                    {
                                        BaseCategoryID = exist.ID,
                                        GroupName = "similar",
                                        RelatedProductID = x
                                    }));
                        dbx.SubmitChanges();
                    }
                }


                var recomend = (cat.Value.GetValueFor("RelatedProductsBuy") ?? "").Split<string>(";").ToList();
                if (recomend.Any())
                {
                    var cids =
                      recomend.Select(x => dbx.StoreProducts.FirstOrDefault(z => z.Slug == x || z.ID == x.ToInt()))
                          .Where(x => x != null && !x.Deleted).Select(x => x.ID).ToList();
                    var rels =

                                dbx.StoreCategoryRelations.Where(
                                    z => z.BaseCategoryID == exist.ID && z.GroupName == "recomend").ToList();
                    var forDel = rels.Where(x => !cids.Contains(x.RelatedProductID ?? 0) || x.StoreProduct == null).ToList();
                    var forAdd = cids.Where(x => rels.All(z => z.RelatedProductID != x)).ToList();
                    if (forDel.Any())
                    {
                        dbx.StoreCategoryRelations.DeleteAllOnSubmit(forDel);
                        dbx.SubmitChanges();
                    }
                    if (forAdd.Any())
                    {
                        dbx.StoreCategoryRelations.InsertAllOnSubmit(
                            forAdd.Select(
                                x =>
                                    new StoreCategoryRelation()
                                    {
                                        BaseCategoryID = exist.ID,
                                        GroupName = "recomend",
                                        RelatedProductID = x
                                    }));
                        dbx.SubmitChanges();
                    }
                }


                info.AddProcessedItem(exist.ID);
            }

            if (thi.ImportInfo.DeleteExpired)
            {
                var exists = dbx.getIntListByJoinedString(info.getProcessedList().JoinToString(","), ",")
                                .Join(dbx.StoreCategories, x => x.ID, y => y.ID, (x, y) => y);
                var prodsForDel = dbx.StoreCategories.Where(x => !exists.Any(z => z.ID == x.ID) && x.ID > 1);
                if (prodsForDel.Any())
                {
                    dbx.StoreCategories.DeleteAllOnSubmit(prodsForDel);
                    dbx.SubmitChanges();
                }
            }
            info.resetList();

            thi.DataList.Where(x => (x as StoreImporterRow).GetValueFor("ID").ToInt() == 0).AsParallel().WithDegreeOfParallelism(ImportParallelismDegree).Select(d =>
            {
                try
                {
                    Thread.Sleep(ThreadDelay);
                    var data = (StoreImporterRow)d;
                    var dbt = new DB();
                    var saleItem =
                        dbt.StoreProducts.FirstOrDefault(
                            x => x.Article == data.GetValueFor("Artikul"));
                    if (thi.PrepareRecordFunc != null)
                    {
                        thi.PrepareRecordFunc(ref d);
                    }
                    if (saleItem == null)
                    {
                        info.Created++;
                        saleItem = new StoreProduct() { AddDate = DateTime.Now, LastMod = DateTime.Now };
                        dbt.StoreProducts.InsertOnSubmit(saleItem);
                    }
                    else
                    {
                        info.Updated++;
                    }
                    if (info.IsItemProcessed(saleItem.ID))
                    {
                        return false;
                    }
                    saleItem.Deleted = false;
                    saleItem.ViewCount = 0;
                    saleItem.Article = data.GetValueFor("Artikul");
                    saleItem.Name = data.GetValueFor("Name");
                    saleItem.Price = data.GetValueFor("Price").ToDecimal();
                    saleItem.Discount = data.GetValueFor("Discount").ToInt();
                    saleItem.IsActive = data.GetValueFor("Visible").ToBool();
                    saleItem.Description = data.GetValueFor("TextUnderH2");
                    saleItem.Slug = data.GetValueFor("URL").LastSegment();
                    saleItem.PageTitle = saleItem.Name;
                    saleItem.PageDescription = data.GetValueFor("PageDescription");
                    saleItem.PageKeywords = data.GetValueFor("PageKeywords");
                    saleItem.Volume = data.GetValueFor("Volume").ToDecimal();
                    saleItem.Weight = data.GetValueFor("Weight").ToDecimal();
                    saleItem.DescrptionLower = data.GetValueFor("TextUnderH3");
                    saleItem.PageH1 = data.GetValueFor("H1");
                    saleItem.PageH2 = data.GetValueFor("H2");
                    saleItem.PageH3 = data.GetValueFor("H3");
                    var menu = (data.GetValueFor("RelatedCategories") ?? "");
                    saleItem.RelatedCategories = menu;

                    var category =
                        dbt.StoreCategories.FirstOrDefault(x => x.Slug == data.GetValueFor("URL").PreLastSegment()) ??
                        dbt.StoreCategories.First(x => x.ID == 1);


                    dbt.StoreProducts.Context.SubmitChanges();
                    info.AddProcessedItem(saleItem.ID);

                    var newRels = new[] { category.SlugOrId };

                    var rels = dbt.StoreProductsToCategories.Where(x => x.ProductID == saleItem.ID).ToList();
                    foreach (var rel in rels)
                    {
                        rel.OrderNum = data.GetValueFor("ProductOrder").ToInt();
                    }
                    var forDel = rels.Where(x => !newRels.Contains(x.StoreCategory.SlugOrId));
                    dbt.StoreProductsToCategories.DeleteAllOnSubmit(forDel);
                    dbt.StoreProductsToCategories.Context.SubmitChanges();
                    var forAdd =
                        newRels.Where(x => !rels.Select(z => z.StoreCategory.SlugOrId).Contains(x))
                               .Select(
                                   x =>
                                   new StoreProductsToCategory
                                       {
                                           StoreCategory =
                                               dbt.StoreCategories.FirstOrDefault(
                                                   z => z.Slug == x || z.ID.ToString() == x) ??
                                               new StoreCategory() { Name = x, Slug = x, LastMod = DateTime.Now, ShowBigIcons = false},
                                           StoreProduct = saleItem,
                                           OrderNum = data.GetValueFor("ProductOrder").ToInt()
                                       });

                    dbt.StoreProductsToCategories.InsertAllOnSubmit(forAdd);
                    dbt.StoreProductsToCategories.Context.SubmitChanges();
                    dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreProductsToCategories);
                    var oldImages = (from storeImage in saleItem.StoreImages
                                     let abs = thi.Context.Server.MapPath(storeImage.UrlPath)
                                     where !System.IO.File.Exists(abs)
                                     select storeImage).ToList();

                    if (oldImages.Any())
                    {
                        dbt.StoreImages.DeleteAllOnSubmit(oldImages);
                        dbt.StoreImages.Context.SubmitChanges();
                        info.AddMessage("Удалены отсутсвующие изображение для товара " + saleItem.Name, false);

                    }
                    dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreImages);


                    var imss = new[]
                        {
                            new CatalogImageSearcher(saleItem.Article, thi.ImportInfo.AdditionalPath, thi.Context),
                            new CatalogImageSearcher(saleItem.Slug, thi.ImportInfo.AdditionalPath, thi.Context)
                        };

                    foreach (var searcher in imss)
                    {



                        if (searcher.HasImage && !saleItem.StoreImages.ToList().Any(z => z.UrlPath.EndsWith(searcher.RelativeFile.LastSegment())))
                        {
                            var img = new StoreImage()
                            {
                                Description = saleItem.Name,
                                Enabled = true,
                                UrlPath = searcher.RelativeFile,
                                UrlPathThumbs = searcher.RelativeFile,
                                OrderNum = dbt.StoreImages.Count() + 1,
                                ProductID = saleItem.ID
                            };
                            dbt.StoreImages.InsertOnSubmit(img);
                            dbt.StoreImages.Context.SubmitChanges();
                            info.AddMessage("Загружено новое изображение для товара " + saleItem.Name, false);
                        }
                    }
                    var chars = data.Where(x => x.ColumnName == "Character");

                    if (saleItem.StoreCharacterToProducts.Any())
                    {
                        dbt.StoreCharacterToProducts.DeleteAllOnSubmit(saleItem.StoreCharacterToProducts);
                        dbt.SubmitChanges();
                        dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreCharacterToProducts);
                    }


                    foreach (var c in chars)
                    {
                        if (c.XlsValue.IsNullOrEmpty())
                            continue;

                        var group =
                            dbt.StoreCharacters.FirstOrDefault(x => x.Name.ToLower() == c.Header.ToLower().Trim());
                        if (group == null)
                        {
                            group = new StoreCharacter() { Name = c.Header.ToLower().Trim()/*.ToNiceForm()*/ };
                            dbt.StoreCharacters.InsertOnSubmit(group);
                        }
                        var value =
                            group.StoreCharacterValues.FirstOrDefault(
                                x => x.Value.ToLower() == c.XlsValue.ToLower().Trim());
                        if (value == null)
                        {
                            value = new StoreCharacterValue()
                                {
                                    StoreCharacter = group,
                                    Value = c.XlsValue.ToLower().Trim()/*.ToNiceForm()*/
                                };
                            dbt.StoreCharacterValues.InsertOnSubmit(value);
                        }
                        var rel = value.StoreCharacterToProducts.FirstOrDefault(x => x.ProductID == saleItem.ID);
                        if (rel == null)
                        {
                            rel = new StoreCharacterToProduct() { ProductID = saleItem.ID, StoreCharacterValue = value };
                            dbt.StoreCharacterToProducts.InsertOnSubmit(rel);
                        }
                        dbt.SubmitChanges();

                    }
                    dbt.Refresh(RefreshMode.OverwriteCurrentValues, saleItem.StoreCharacterToProducts);
                    var tags = data.GetValueFor("SearchTags").Split<string>(";").Select(x => x.ToLower().Trim());
                    var tagsForDel =
                        saleItem.StoreProductTagRels.ToList().Where(x => !tags.Contains(x.StoreProductTag.Tag.ToLower())).ToList();
                    var tagsForAdd =
                        tags.Where(
                            x =>
                            !saleItem.StoreProductTagRels.Select(z => z.StoreProductTag.Tag.ToLower())
                                     .Contains(x.ToLower())).ToList();

                    if (tagsForDel.Any())
                    {
                        dbt.StoreProductTagRels.DeleteAllOnSubmit(tagsForDel);
                        dbt.SubmitChanges();

                    }
                    foreach (var tag in tagsForAdd)
                    {
                        var exist = dbt.StoreProductTags.FirstOrDefault(x => x.Tag.ToLower() == tag.ToLower().Trim());
                        if (exist == null)
                        {
                            exist = new StoreProductTag() { Tag = tag.ToLower().Trim() };
                            dbt.StoreProductTags.InsertOnSubmit(exist);
                        }
                        dbt.StoreProductTagRels.InsertOnSubmit(new StoreProductTagRel()
                            {
                                StoreProduct = saleItem,
                                StoreProductTag = exist
                            });

                    }
                    dbt.SubmitChanges();

                    var related = (data.GetValueFor("RelatedProductsSame") ?? "").Split<string>(";").ToList();
                    if (related.Any())
                    {
                        var cids =
                            related.Select(x => dbt.StoreProducts.FirstOrDefault(z => z.Slug == x || z.ID == x.ToInt()))
                                .Where(x => x != null).Select(x => x.ID).ToList();
                        var relsA =
                            cids.Select(
                                x =>
                                    dbx.StoreProductRelations.FirstOrDefault(
                                        z => z.BaseProductID == x && z.GroupName == "related")).Where(x => x != null).ToList();


                        var forDelA = relsA.Where(x => !cids.Contains(x.RelatedProductID ?? 0) || x.BaseProduct == null).ToList();
                        var forAddA = cids.Where(x => relsA.All(z => z.RelatedProductID != x)).ToList();
                        if (forDelA.Any())
                        {
                            dbx.StoreProductRelations.DeleteAllOnSubmit(forDelA);
                            dbx.SubmitChanges();
                        }
                        if (forAddA.Any())
                        {
                            dbx.StoreProductRelations.InsertAllOnSubmit(
                                forAddA.Select(
                                    x =>
                                        new StoreProductRelation()
                                        {
                                            BaseProductID = saleItem.ID,
                                            GroupName = "related",
                                            RelatedProductID = x
                                        }));
                            dbx.SubmitChanges();
                        }
                    }
                    var similar = (data.GetValueFor("RelatedProductsSimilar") ?? "").Split<string>(";").ToList();
                    if (similar.Any())
                    {
                        var cids =
                            similar.Select(x => dbt.StoreProducts.FirstOrDefault(z => z.Slug == x || z.ID == x.ToInt()))
                                .Where(x => x != null).Select(x => x.ID).ToList();
                        var relsA =
                            cids.Select(
                                x =>
                                    dbx.StoreProductRelations.FirstOrDefault(
                                        z => z.BaseProductID == x && z.GroupName == "similar")).Where(x => x != null).ToList();


                        var forDelA = relsA.Where(x => !cids.Contains(x.RelatedProductID ?? 0) || x.BaseProduct == null).ToList();
                        var forAddA = cids.Where(x => relsA.All(z => z.RelatedProductID != x)).ToList();
                        if (forDelA.Any())
                        {
                            dbx.StoreProductRelations.DeleteAllOnSubmit(forDelA);
                            dbx.SubmitChanges();
                        }
                        if (forAddA.Any())
                        {
                            dbx.StoreProductRelations.InsertAllOnSubmit(
                                forAddA.Select(
                                    x =>
                                        new StoreProductRelation()
                                        {
                                            BaseProductID = saleItem.ID,
                                            GroupName = "similar",
                                            RelatedProductID = x
                                        }));
                            dbx.SubmitChanges();
                        }
                    }


                    var recomend = (data.GetValueFor("RelatedProductsBuy") ?? "").Split<string>(";").ToList();
                    if (recomend.Any())
                    {
                        var cids =
                            recomend.Select(x => dbt.StoreProducts.FirstOrDefault(z => z.Slug == x || z.ID == x.ToInt()))
                                .Where(x => x != null).Select(x => x.ID).ToList();
                        var relsA =
                            cids.Select(
                                x =>
                                    dbx.StoreProductRelations.FirstOrDefault(
                                        z => z.BaseProductID == x && z.GroupName == "recomend")).Where(x => x != null).ToList();


                        var forDelA = relsA.Where(x => !cids.Contains(x.RelatedProductID ?? 0) || x.BaseProduct == null).ToList();
                        var forAddA = cids.Where(x => relsA.All(z => z.RelatedProductID != x)).ToList();
                        if (forDelA.Any())
                        {
                            dbx.StoreProductRelations.DeleteAllOnSubmit(forDelA);
                            dbx.SubmitChanges();
                        }
                        if (forAddA.Any())
                        {
                            dbx.StoreProductRelations.InsertAllOnSubmit(
                                forAddA.Select(
                                    x =>
                                        new StoreProductRelation()
                                        {
                                            BaseProductID = saleItem.ID,
                                            GroupName = "recomend",
                                            RelatedProductID = x
                                        }));
                            dbx.SubmitChanges();
                        }
                    }


                    if (thi.PostProcessingFunc != null)
                    {
                        thi.PostProcessingFunc(saleItem.ID, data, thi.ImportInfo);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    info.AddMessage(ex.Message);
                    info.AddMessage(ex.StackTrace);
                    return false;
                }
            }).Count();
            if (thi.ImportInfo.DeleteExpired)
            {
                var exists = dbx.getIntListByJoinedString(info.getProcessedList().JoinToString(","), ",")
                   .Join(dbx.StoreProducts, x => x.ID, y => y.ID, (x, y) => y);
                var prodsForDel = dbx.StoreProducts.Where(x => !exists.Any(z => z.ID == x.ID)).ToList();

                foreach (var product in prodsForDel)
                {
                    var p = product;
                    var rels = dbx.StoreProductRelations.Where(x => x.BaseProductID == p.ID || x.RelatedProductID == p.ID);
                    if (rels.Any())
                    {
                        dbx.StoreProductRelations.DeleteAllOnSubmit(rels);
                        dbx.SubmitChanges();
                    }
                    var catrels = dbx.StoreCategoryRelations.Where(x => x.RelatedProductID == p.ID);
                    if (catrels.Any())
                    {
                        dbx.StoreCategoryRelations.DeleteAllOnSubmit(catrels);
                        dbx.SubmitChanges();
                    }
                }
                dbx.StoreProducts.DeleteAllOnSubmit(prodsForDel);
                dbx.SubmitChanges();
            }
            CatalogBrowser.Init().ClearAllCaches();
            info.AddMessage("Обработка завершена.");
            info.EndDate = DateTime.Now;
        }



        #endregion


    }

}