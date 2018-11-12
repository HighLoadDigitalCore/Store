using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.XSSF.UserModel;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace Smoking.Controllers
{
    public class ExportController : Controller
    {

        private DB db = new DB();

        [AuthorizeMaster]
        [HttpGet]
        [MenuItem("Скачать каталог в XLS", 44, 4)]
        public ActionResult Index()
        {
            return PartialView();
        }

        /*
                [AuthorizeMaster]
                [HttpPost]
                public ActionResult Index(ExportInfo info, FormCollection collection)
                {
                    if (collection["categories"].IsFilled())
                        return ExportCategoriesToXLSX(info.NeedDescription);
                    return ExportProductsToXLSX(info.NeedDescription);
                }
        */



        [HttpGet]
        [AuthorizeMaster]
        public ActionResult ProductExport(int Category)
        {
            return View();
        }

        [AuthorizeMaster]
        [HttpPost]
        public FileResult ProductExport(FormCollection collection)
        {
            if (!collection["all"].IsNullOrEmpty())
            {

                var stream = new MemoryStream();
                var workbook = new HSSFWorkbook();
                var worksheet = workbook.CreateSheet("Catalog");



                var settings = new List<StoreImporter>()
                {
                    new StoreImporter()
                    {
                        ColumnName = "RecType",
                        ColumnNum = 1,
                        RowNum = 2,
                        Header = "Тип записи"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "FolderNum",
                        ColumnNum = 2,
                        RowNum = 2,
                        Header = "Номер папки"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "ProductFolders",
                        ColumnNum = 3,
                        RowNum = 2,
                        Header = "Родительская папка"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "ShortName",
                        ColumnNum = 4,
                        RowNum = 2,
                        Header = "Короткое название (модель)"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "Name",
                        ColumnNum = 5,
                        RowNum = 2,
                        Header = "Название полностью"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "URL",
                        ColumnNum = 6,
                        RowNum = 2,
                        Header = "URL"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "PageTitle",
                        ColumnNum = 7,
                        RowNum = 2,
                        Header = "Title страницы"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "PageKeywords",
                        ColumnNum = 8,
                        RowNum = 2,
                        Header = "Ключевые слова (Keywords)"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "PageDescription",
                        ColumnNum = 9,
                        RowNum = 2,
                        Header = "Описание (Description)"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "H1",
                        ColumnNum = 10,
                        RowNum = 2,
                        Header = "H1"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "H2",
                        ColumnNum = 11,
                        RowNum = 2,
                        Header = "H2"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "H3",
                        ColumnNum = 12,
                        RowNum = 2,
                        Header = "H3"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "TextUnderH2",
                        ColumnNum = 13,
                        RowNum = 2,
                        Header = "Описание под H2"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "TextUnderH3",
                        ColumnNum = 14,
                        RowNum = 2,
                        Header = "Описание под H3"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "Artikul",
                        ColumnNum = 15,
                        RowNum = 2,
                        Header = "Артикул"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "Weight",
                        ColumnNum = 16,
                        RowNum = 2,
                        Header = "Вес"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "Volume",
                        ColumnNum = 17,
                        RowNum = 2,
                        Header = "Объем"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "Discount",
                        ColumnNum = 18,
                        RowNum = 2,
                        Header = "Скидка, %"
                    },

                    new StoreImporter()
                    {
                        ColumnName = "Visible",
                        ColumnNum = 19,
                        RowNum = 2,
                        Header = "Отображать в каталоге (1 = виден, 0 = не виден)"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "Price",
                        ColumnNum = 20,
                        RowNum = 2,
                        Header = "Цена, руб"
                    },
                    new StoreImporter()
                    {
                        ColumnName = "SearchTags",
                        ColumnNum = 21,
                        RowNum = 2,
                        Header = "Поисковые слова (через ;)",


                    },
                };


                var category = db.StoreCategories.First(x => x.ID == collection["CategoryID"].ToInt());

                var chars = category.StoreProductsToCategories.Select(x => x.StoreProduct)
                    .Where(x => !x.Deleted)
                    .SelectMany(x => x.StoreCharacterToProducts)
                    .Distinct();

                var cc = 0;
                foreach (var c in chars)
                {
                    if (settings.All(x => x.Header != c.StoreCharacterValue.StoreCharacter.Name))
                    {
                        settings.Add(new StoreImporter()
                        {
                            ColumnName = /*c.StoreCharacterValue.StoreCharacter.Name*/ "Character",
                            ColumnNum = 22 + cc,
                            RowNum = 2,
                            Header = c.StoreCharacterValue.StoreCharacter.Name,
                            Priority = 1
                        });
                        cc++;
                    }
                }


                int rowNum = 0;
                var header = worksheet.CreateRow(rowNum);
                var req = settings.Count(x => x.Priority == 0);
                for (int i = 0; i < req; i++)
                {
                    header.CreateCell(i).SetCellValue("Обязательные параметры");
                }
                for (int i = req; i < settings.Count; i++)
                {
                    header.CreateCell(i).SetCellValue("Характеристики");
                }

                rowNum++;
                header = worksheet.CreateRow(rowNum);
                for (int i = 0; i < settings.Count; i++)
                {
                    header.CreateCell(i).SetCellValue(settings[i].Header);
                }


                var prods = category.StoreProductsToCategories.OrderBy(x => x.OrderNum).ToList();

                foreach (var productsToCategory in prods)
                {
                    rowNum++;
                    header = worksheet.CreateRow(rowNum);
                    foreach (var setting in settings)
                    {
                        //if (setting.Priority == 0)
                        {

                            var value = productsToCategory.StoreProduct.GetExportValue(setting);
                            if (setting.ColumnName == "URL")
                                value = productsToCategory.StoreProduct.SlugOrId;
                            header.CreateCell(setting.ColumnNum - 1)
                                .SetCellValue(value);
                        }
                    }
                }

                workbook.Write(stream);

                return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xls"),
                    "Catalog_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls");

            }
            else
            {
                var category = db.StoreCategories.First(x => x.ID == collection["CategoryID"].ToInt());
                var images =
                    category.StoreProductsToCategories.Select(x => x.StoreProduct)
                        .Where(x => x.StoreImages.Any())
                        .SelectMany(x => x.StoreImages).ToList();

                var virtPath = "/content/temp/Images_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".zip";
                using (var fs = new MemoryStream())
                {
                    var zf = new ZipOutputStream(fs);

                    foreach (var image in images)
                    {
                        if (System.IO.File.Exists(Server.MapPath(image.UrlPath)))
                        {
                            var entry = new ZipEntry(Path.GetFileName(image.UrlPath)) { DateTime = DateTime.Now };

                            zf.PutNextEntry(entry);
                            using (var ifs = new FileStream(Server.MapPath(image.UrlPath), FileMode.Open, FileAccess.Read))
                            {
                                StreamUtils.Copy(ifs, zf, new byte[4096]);
                            }
                            zf.CloseEntry();
                        }
                    }

                    zf.IsStreamOwner = false;
                    zf.Close();

                    fs.Position = 0;

                    return File(fs.ToArray(), MIMETypeWrapper.GetMIME("zip"), Path.GetFileName(virtPath));


                }

            }
        }

        [AuthorizeMaster]
        [HttpPost]
        public FileResult ProductExportPrice(FormCollection collection)
        {

            var stream = new MemoryStream();
            var workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("Catalog");



            var settings = new List<StoreImporter>()
                {
                       new StoreImporter()
                        {
                            ColumnName = "PriceBaseEUR",
                            ColumnNum = 0,
                            Header = "Базовая евро"
                        },new StoreImporter()
                        {
                            ColumnName = "PriceBaseRUR",
                            ColumnNum = 1,
                            Header = "Базовая руб."
                        },new StoreImporter()
                        {
                            ColumnName = "BuyingRate",
                            ColumnNum = 2,
                            Header = "Коэфф. Закупки"
                        }
                        ,new StoreImporter()
                        {
                            ColumnName = "ProfitRate",
                            ColumnNum = 3,
                            Header = "Коэфф. Наценки"
                        },
                        new StoreImporter()
                        {
                            ColumnName = "DiscountRate",
                            ColumnNum = 4,
                            Header = "Коэфф. Скидки"
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Article",
                            ColumnNum = 5,
                            Header = "Артикул"
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Name",
                            ColumnNum =6,
                            Header = "Название товара"
                        },
                        new StoreImporter()
                        {
                            ColumnName = "SitePrice",
                            ColumnNum =7,
                            Header = "Цена на сайте"
                        },

                
                };


            var category = db.StoreCategories.First(x => x.ID == collection["CategoryID"].ToInt());



            int rowNum = 0;
            var header = worksheet.CreateRow(rowNum);
            for (int i = 0; i < settings.Count; i++)
            {
                header.CreateCell(i).SetCellValue(settings[i].Header);
            }

            var child = CatalogBrowser.GetChildrenCategories(category.ID);

            var prods = db.StoreProductsToCategories.Where(x => !x.StoreProduct.Deleted && child.Contains(x.CategoryID)).OrderBy(x => x.CategoryID).ThenBy(x => x.OrderNum).ToList();


            foreach (var productsToCategory in prods)
            {
                rowNum++;
                header = worksheet.CreateRow(rowNum);
                foreach (var setting in settings)
                {
                    //if (setting.Priority == 0)
                    {

                        var value = productsToCategory.StoreProduct.GetExportValue(setting);
                        /*
                                                    if (setting.ColumnName == "URL")
                                                        value = productsToCategory.StoreProduct.SlugOrId;
                        */
                        header.CreateCell(setting.ColumnNum)
                            .SetCellValue(value);
                    }
                }
            }

            workbook.Write(stream);

            return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xls"),
                "CatalogPrices_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls");


        }

        [AuthorizeMaster]
        [HttpPost]
        public FileResult ProductExportRels(FormCollection collection)
        {

            var stream = new MemoryStream();
            var workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("Catalog");



            var settings = new List<StoreImporter>()
                {
                        new StoreImporter()
                        {
                            ColumnName = "SimilarArt",
                            ColumnNum = 0,
                            Header = "Похожие товары"
                        },
                      
                    new StoreImporter()
                        {
                            ColumnName = "RecommendArt",
                            ColumnNum = 1,
                            Header = "Рекомендуем купить"
                        }
                        ,new StoreImporter()
                        {
                            ColumnName = "RelatedArt",
                            ColumnNum = 2,
                            Header = "С этим товаром покупают"
                        },
                   
                        new StoreImporter()
                        {
                            ColumnName = "Article",
                            ColumnNum = 3,
                            Header = "Артикул"
                        },
                        new StoreImporter()
                        {
                            ColumnName = "Name",
                            ColumnNum =4,
                            Header = "Название товара"
                        },

                
                };


            var category = db.StoreCategories.First(x => x.ID == collection["CategoryID"].ToInt());



            int rowNum = 0;
            var header = worksheet.CreateRow(rowNum);
            for (int i = 0; i < settings.Count; i++)
            {
                header.CreateCell(i).SetCellValue(settings[i].Header);
            }


            var prods = category.StoreProductsToCategories.Where(x => !x.StoreProduct.Deleted).OrderBy(x => x.OrderNum).ToList();

            foreach (var productsToCategory in prods)
            {
                rowNum++;
                header = worksheet.CreateRow(rowNum);
                foreach (var setting in settings)
                {
                    //if (setting.Priority == 0)
                    {

                        var value = productsToCategory.StoreProduct.GetExportValue(setting);
                        /*
                                                    if (setting.ColumnName == "URL")
                                                        value = productsToCategory.StoreProduct.SlugOrId;
                        */
                        header.CreateCell(setting.ColumnNum)
                            .SetCellValue(value);
                    }
                }
            }

            workbook.Write(stream);

            return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xls"),
                "CatalogRels_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls");


        }



        [AuthorizeMaster]
        [HttpPost]
        public FileContentResult Index(FormCollection collection)
        {

            var stream = new MemoryStream();
            var workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("Catalog");


            int rowNum = 0;

            var settings = db.StoreImporters.OrderBy(x => x.ColumnNum).ToList();

            //Пустая строка
            var header = worksheet.CreateRow(rowNum);
            rowNum++;
            for (int i = 0; i < settings.Count; i++)
            {
                header.CreateCell(i).SetCellValue("");
            }
            //Пустая строка
            header = worksheet.CreateRow(rowNum);
            rowNum++;
            for (int i = 0; i < settings.Count; i++)
            {
                header.CreateCell(i).SetCellValue("");
            }
            //Заголовок
            header = worksheet.CreateRow(rowNum);
            rowNum++;
            foreach (var setting in settings)
            {
                if (setting.ColumnName == "Character")
                {
                    header.CreateCell(setting.ColumnNum - 1).SetCellValue(setting.Header);
                }
                else
                    header.CreateCell(setting.ColumnNum - 1)
                          .SetCellValue(StoreImporter.RequiredColumns.First(x => x.Key == setting.ColumnName).Name);
            }
            //Пустая строка
            header = worksheet.CreateRow(rowNum);
            rowNum++;
            for (int i = 0; i < settings.Count; i++)
            {
                header.CreateCell(i).SetCellValue("");
            }
            int startCatID = 2;
            var baseCat = db.StoreCategories.FirstOrDefault(x => x.ID == 1);
            baseCat.ExportID = startCatID;
            AppendChildren(baseCat, worksheet, settings, ref rowNum, ref startCatID);




            /*     for (int i = 0; i < CatalogXLSExportProductRow.ColumnNames.Length; i++)
                 {
                     header.CreateCell(i).SetCellValue(CatalogXLSExportProductRow.ColumnNames[i]);
                 }
                 rowNum++;
                 foreach (var product in prods)
                 {
                     var row = worksheet.CreateRow(rowNum);
                     for (int i = 0; i < CatalogXLSExportProductRow.ColumnNames.Length; i++)
                     {
                         var data = product.GetPropertyValue(CatalogXLSExportProductRow.PropNames[i]);
                         if (data is bool)
                             row.CreateCell(i).SetCellValue((bool)data ? "1" : "0");
                         else
                         {
                             if ((data ?? "").ToString().Length > 32767)
                                 data = "";
                             row.CreateCell(i).SetCellValue((data ?? "").ToString());
                         }

                     }
                     rowNum++;
                 }
     */

            workbook.Write(stream);

            return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xls"),
                        "Catalog_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls");


        }

        private void AppendChildren(StoreCategory baseCat, ISheet worksheet, List<StoreImporter> settings, ref int rowNum, ref int catNum)
        {
            var childrenCats = baseCat.Children;
            //int baseID = baseCat.ExportID;
            foreach (var cat in childrenCats.OrderBy(x => x.OrderNum))
            {
                var row = worksheet.CreateRow(rowNum);
                rowNum++;
                catNum++;
                //baseCat.ExportID = catNum;
                cat.ExportID = catNum;
                cat.ExportParentID = baseCat.ExportID;
                foreach (var setting in settings)
                {

                    if (setting.ColumnName == "ID")
                        row.CreateCell(setting.ColumnNum - 1).SetCellValue(cat.ExportID.ToString());
                    else if (setting.ColumnName == "ParentID")
                        row.CreateCell(setting.ColumnNum - 1)
                           .SetCellValue(cat.ExportParentID == 0 ? "" : cat.ExportParentID.ToString());
                    else
                        row.CreateCell(setting.ColumnNum - 1).SetCellValue(cat.GetExportValue(setting));
                }
                foreach (var prods in cat.StoreProductsToCategories.OrderBy(x => x.OrderNum))
                {
                    row = worksheet.CreateRow(rowNum);
                    rowNum++;
                    foreach (var setting in settings)
                    {
                        if (setting.ColumnName == "ParentID")
                            row.CreateCell(setting.ColumnNum - 1).SetCellValue(cat.ExportID.ToString());
                        else if (setting.ColumnName == "ProductOrder")
                            row.CreateCell(setting.ColumnNum - 1).SetCellValue(prods.OrderNum.ToString());
                        else
                            row.CreateCell(setting.ColumnNum - 1).SetCellValue(prods.StoreProduct.GetExportValue(setting));
                    }
                }
                AppendChildren(cat, worksheet, settings, ref rowNum, ref catNum);
            }

        }

        /*

                [AuthorizeMaster]
                public FileContentResult ExportProductsToXLSX(bool needDescription)
                {
                    var prods =
                        db.StoreProducts.ToList()
                          .OrderBy(x => x.StoreProductsToCategories.Any() ? x.StoreProductsToCategories.First().OrderNum : 10000)
                          .Select(
                              x =>
                              new CatalogXLSExportProductRow
                                  {
                                      Article = x.Article,
                                      Categories =
                                          x.StoreProductsToCategories.Select(z => z.StoreCategory.SlugOrId).JoinToString("; "),
                                      Description = x.Description,
                                      Favorite = x.IsFavorite,
                                      Visible = x.IsActive,
                                      Name = x.Name,
                                      Slug = x.SlugOrId,
                                      Price = x.Price,
                                      OldPrice = x.OldPrice,
                                      PageTitle = x.PageTitle,
                                      PageDescription = x.PageDescription,
                                      PageKeywords = x.PageKeywords,
                                      Manufacturer = "",
                                      Provider = "",
                                      Images =
                                          x.StoreImages.Select(z => "{0}{1}".FormatWith(AccessHelper.SiteUrl, z.UrlPath))
                                           .JoinToString("; "),
                                      DescrptionLower = x.DescrptionLower,
                                      PageH1 = x.PageH1,
                                      PageH2 = x.PageH2,
                                      PageH3 = x.PageH3,
                                      Param1 = x.Param1,
                                      Param2 = x.Param2,
                                      Param3 = x.Param3,
                                      Param4 = x.Param4,
                                      Param5 = x.Param5,
                                      Volume = x.Volume.ToString(),
                                      Weight = x.Weight.ToString()
                                  });


                    var stream = new MemoryStream();
                    var workbook = new XSSFWorkbook();
                    var worksheet = workbook.CreateSheet("Products");


                    int rowNum = 0;
                    var header = worksheet.CreateRow(rowNum);
                    for (int i = 0; i < CatalogXLSExportProductRow.ColumnNames.Length; i++)
                    {
                        header.CreateCell(i).SetCellValue(CatalogXLSExportProductRow.ColumnNames[i]);
                    }
                    rowNum++;
                    foreach (var product in prods)
                    {
                        var row = worksheet.CreateRow(rowNum);
                        for (int i = 0; i < CatalogXLSExportProductRow.ColumnNames.Length; i++)
                        {
                            var data = product.GetPropertyValue(CatalogXLSExportProductRow.PropNames[i]);

                            if (CatalogXLSExportProductRow.PropNames[i] == "Description" && !needDescription)
                            {
                                row.CreateCell(i).SetCellValue("");
                            }
                            else
                            {
                                if (data is bool)
                                    row.CreateCell(i).SetCellValue((bool)data ? "1" : "0");
                                else
                                {
                                    row.CreateCell(i).SetCellValue(new XSSFRichTextString((data ?? "").ToString()));
                                }
                            }

                        }
                        rowNum++;
                    }


                    workbook.Write(stream);

                    return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xlsx"),
                                "Products_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xlsx");


                }

                [AuthorizeMaster]
                public FileContentResult ExportCategoriesToXLSX(bool needDescription)
                {
                    var prods =
                        db.StoreCategories.OrderBy(x=> x.OrderNum).ToList()
                          .Select(
                              x =>
                              new CatalogXLSExportCategoryRow
                                  {
                                      Name = x.Name,
                                      PageTitle = x.PageTitle,
                                      PageDescription = x.PageDescription,
                                      PageKeywords = x.PageKeywords,
                                      SlugOrId = x.SlugOrId,
                                      ParentSlugOrId = x.Parent == null ? "" : x.Parent.SlugOrId,
                                      H1 = x.PageHeader,
                                      H2 = x.PageSubHeader,
                                      Description = x.Description,
                                      ImageURL = x.Image == null || x.Image.Length == 0
                                                     ? ""
                                                     : (AccessHelper.SiteUrl +
                                                        UniversalEditorPagedData.GetImageWrapper("StoreCategories", "ID",
                                                                                                 x.ID.ToString(),
                                                                                                 "Image")),
                                      H3 = x.PageHeaderH3,
                                      PageTextH3Lower = x.PageTextH3Lower
                                  });


                    var stream = new MemoryStream();
                    var workbook = new XSSFWorkbook();
                    var worksheet = workbook.CreateSheet("Categories");


                    int rowNum = 0;
                    var header = worksheet.CreateRow(rowNum);
                    for (int i = 0; i < CatalogXLSExportCategoryRow.ColumnNames.Length; i++)
                    {
                        header.CreateCell(i).SetCellValue(CatalogXLSExportCategoryRow.ColumnNames[i]);
                    }
                    rowNum++;
                    foreach (var product in prods)
                    {
                        var row = worksheet.CreateRow(rowNum);
                        for (int i = 0; i < CatalogXLSExportCategoryRow.ColumnNames.Length; i++)
                        {
                            var data = product.GetPropertyValue(CatalogXLSExportCategoryRow.PropNames[i]);

                            if (CatalogXLSExportCategoryRow.PropNames[i] == "Description" && !needDescription)
                            {
                                row.CreateCell(i).SetCellValue("");
                            }
                            else
                            {
                                if (data is bool)
                                    row.CreateCell(i).SetCellValue((bool)data ? "1" : "0");
                                else
                                {
                                    row.CreateCell(i).SetCellValue(new XSSFRichTextString((data ?? "").ToString()));
                                }
                            }

                        }
                        rowNum++;
                    }


                    workbook.Write(stream);

                    return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xlsx"),
                                "Categories_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xlsx");


                }
        */
        /*
                public ContentResult Orders(DateTime? from_date, int? from_id)
                {
                    /* Dictionary<string, int> Payments = new Dictionary<string, int>();
                     Payments.Add("", 0);
                     Payments.Add("Наличными", 1);
                     Payments.Add("Оплата через Банк", 2);
                     Payments.Add("Наложенным платежом", 3);
                     Payments.Add("Оплата банковской картой", 4);
                     Payments.Add("Оплата по безналичному расчету", 5);
                     Payments.Add("WebMoney", 6);
                     Payments.Add("Оплата через сервис Robokassa", 7);
                     Payments.Add("Yandex-деньги", 8);#1#

                    var payments = db.OrderPaymentProviders.Select(x => new { Key = x.SprinterUID, Value = x.Name }).ToList();

                    Dictionary<string, int> Deliveries = new Dictionary<string, int>();
                    Deliveries.Add("", 0);
                    Deliveries.Add("Самовывоз", 1);
                    Deliveries.Add("Курьером", 2);
                    Deliveries.Add("Почтой России", 3);
                    Deliveries.Add("Почтой России в дальнее зарубежье", 4);
                    Deliveries.Add("Пони-Экспресс", 5);

                    if (!from_date.HasValue && !from_id.HasValue) from_date = DateTime.Now.Date.AddDays(-1);
                    const int maxDayLimit = -90;
                    IQueryable<Order> records = null;
                    if (from_date.HasValue && from_date < DateTime.Now.AddDays(maxDayLimit).Date) from_date = DateTime.Now.AddDays(maxDayLimit).Date;
                    if (from_date.HasValue)
                        records =
                            db.Orders.Where(x => x.CreateDate > from_date || x.OrderComments.Any(z => z.Date > from_date))
                              .OrderByDescending(x => x.CreateDate);
                    if (from_id.HasValue)
                        records = db.Orders.Where(x => x.ID > from_id).OrderByDescending(x => x.CreateDate);


                    var list = records.ToList();
                    if (!list.Any()) list = new List<Order>();

                    XDocument doc = new XDocument();
                    var orders = new XElement("Orders");
                    doc.Add(orders);

                    foreach (Order order in list)
                    {
                        var xo = new XElement("Order");
                        xo.Add(new XAttribute("id", order.ID));
                        xo.Add(new XAttribute("date", order.CreateDate.ToString("yyyy-MM-ddTHH:mm:ss")));
                        orders.Add(xo);

                        var books = new XElement("Books");
                        xo.Add(books);
                        books.Add(order.OrderedBooks.Where(x => x.Partner != null).Select(x => new XElement("Book", new[]
                                {
                                    new XElement("Supplier", x.Partner == null ? -1 : x.Partner.ID),
                                    new XElement("SupplierName", x.Partner == null ? "" : x.Partner.Name),
                                    new XElement("SaledPrice", x.SalePrice),
                                    new XElement("Amount", x.Amount),
                                    new XElement("BookId", x.BookDescriptionCatalog.SprinterCode),
                                    new XElement("BookSupplierId",
                                                 x.BookDescriptionCatalog.BookSaleCatalogs.First(z => z.PartnerID == x.PartnerID)
                                                     .PartnerUID)

                                })
                                      ));
                        xo.Add(XDocument.Parse(order.UserData).Root);
                        var orgData = order.OrderDetail.OrgData;
                        if (orgData.IsFilled())
                            xo.Add(XDocument.Parse(orgData).Root);


                        var adress = order.OrderDetail.Address;
                        xo.Add(new XElement("Delivery", Deliveries.First(x => x.Value == order.OrderDetail.DeliveryType).Key));
                        xo.Add(new XElement("DeliveryCost", order.OrderDetail.DeliveryCost));
                        if (order.OrderDetail.OrderDeliveryRegion != null)
                            xo.Add(new XElement("DeliveryRegion", order.OrderDetail.OrderDeliveryRegion.Name));
                        xo.Add(new XElement("PaymentType", order.OrderDetail.PaymentType));
                        if (payments.Any(x => x.Key == order.OrderDetail.PaymentType))
                        {
                            xo.Add(new XElement("Payment", payments.First(x => x.Key == order.OrderDetail.PaymentType).Value));
                        }
                        var statusNode = new XElement("OrderStatus", order.OrderStatus.Status);
                        statusNode.Add(new XAttribute("name", order.OrderStatus.EngName));
                        xo.Add(statusNode);

                        var notifyNode = new XElement("NotifyMail", order.User.Profile.NotifyMail ?? order.User.MembershipData.Email);
                        notifyNode.Add(new XAttribute("enabled", order.User.Profile.NeedMailNotify ?? false));
                        xo.Add(notifyNode);

                        var notifySmsNode = new XElement("NotifyPhone", order.User.Profile.NotifyPhone ?? order.User.Profile.MobilePhone);
                        notifySmsNode.Add(new XAttribute("enabled", order.User.Profile.NeedPhoneNotify ?? false));
                        xo.Add(notifySmsNode);

                        if (order.OrderComments.Any())
                        {
                            var comments = new XElement("Comments",
                                                        order.OrderComments.ToList()
                                                             .Select(
                                                                 x =>
                                                                     {
                                                                         var comment = new XElement("Comment", x.Comment);
                                                                         comment.Add(new XAttribute("author",
                                                                                                    x.Author.IsNullOrEmpty()
                                                                                                        ? x.Order.User
                                                                                                           .UserProfile
                                                                                                           .FullName
                                                                                                        : x.Author),
                                                                                     new XAttribute("date",
                                                                                                    x.Date.ToString(
                                                                                                        "yyyy-MM-dd HH:mm:ss")),
                                                                                     new XAttribute("id", x.ID.ToString()));
                                                                         return comment;
                                                                     }
                                                            ));
                            xo.Add(comments);
                        }


                        if (adress.IsFilled())
                            xo.Add(XDocument.Parse(adress).Root);
                    }
                    ContentResult content = new ContentResult();
                    content.Content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";
                    content.Content += doc.ToString();
                    content.ContentType = "text/xml";
                    return content;
                }

                [HttpGet]
                public ActionResult Yandex()
                {
                    return View(new CatalogExportFilterModel());
                }

                [HttpPost]
                public ActionResult Yandex(CatalogExportFilterModel model)
                {
                    ParseringInfo.Reset("YandexExport");
                    var workingThread = new Thread(ThreadFuncExportCatalogToYandex);
                    workingThread.Start(new ThreadExportInfo() { Context = System.Web.HttpContext.Current, ExporterName = "YandexExport", ExportFilter = model, ProgressFunc = OnOrderFormedYandex });

                    return View(model);
                }


                private void ThreadFuncExportCatalogToYandex(object context)
                {
                    DB dbx = new DB();
                    var thi = (ThreadExportInfo)context;
                    System.Web.HttpContext.Current = thi.Context;
                    ParseringInfo info = ParseringInfo.Create(thi.ExporterName);
                    info.Created = info.Updated = info.Deleted = info.Dirs = info.Prepared = 0;
                    info.EndDate = null;
                    info.StartDate = DateTime.Now;
                    info.AddMessage(DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " - Начало формирования файла.");
                    var db = new DB();
                    var pageIDs = thi.ExportFilter.PageListPlain.Split<int>();
                    var pagesList = CMSPage.FullPageTable.Where(x => pageIDs.Contains(x.ID)).ToList();

                    //Ищем все подразделы
                    if (pagesList.Any())
                    {
                        int minLevel = pagesList.Min(x => x.TreeLevel);
                        int maxLevel = CMSPage.FullPageTable.Max(x => x.TreeLevel);
                        for (int i = minLevel; i < maxLevel; i++)
                        {
                            var thisLevel = pagesList.Where(x => x.TreeLevel == i).Select(x => x.ID).ToList();
                            var nextLevel = CMSPage.FullPageTable.Where(x => thisLevel.Contains(x.ParentID ?? 0)).ToList();
                            pagesList.AddRange(nextLevel);
                        }
                    }
                    var pagesListIdsPlain = string.Join(";", pagesList.Select(x => x.ID));

                    var pages = db.getIntListByJoinedString(pagesListIdsPlain, ";").Join(db.CMSPages.AsQueryable(), x => x.ID,
                                                                                         y => y.ID, (x, y) => y);
                    var partnersListIds = thi.ExportFilter.PartnerListPlain.Split<int>();

            
                    var filtered =
                        db.BookSaleCatalogs.Where(
                            x =>
                            (!thi.ExportFilter.AvailableOnly || x.IsAvailable) &&
                            (!thi.ExportFilter.MinPrice.HasValue ||
                             (x.Partner == null
                                  ? 0
                                  : ((x.PriceOverride.HasValue && x.PriceOverride > 0)
                                         ? x.PriceOverride.Value
                                         : x.PartnerPrice * (100 +
                                                           (x.Margin > 0
                                                                ? x.Margin
                                                                : (x.BookDescriptionCatalog.PublisherID.HasValue &&
                                                                   x.BookDescriptionCatalog.BookPublisher.BookPublisherMargins.
                                                                       Any(
                                                                           z => z.PartnerID == x.PartnerID) &&
                                                                   x.BookDescriptionCatalog.BookPublisher.BookPublisherMargins.
                                                                       First(
                                                                           z => z.PartnerID == x.PartnerID).Margin.HasValue
                                                                       ? (x.BookDescriptionCatalog.BookPublisher.
                                                                             BookPublisherMargins
                                                                             .First(
                                                                                 z => z.PartnerID == x.PartnerID).Margin.Value)
                                                                       : (x.Partner.Margin)))

                                                           - (x.BookDescriptionCatalog.PublisherID.HasValue &&
                                                              x.BookDescriptionCatalog.BookPublisher.BookPublisherMargins.Any(
                                                                  z => z.PartnerID == x.PartnerID) &&
                                                              x.BookDescriptionCatalog.BookPublisher.BookPublisherMargins.First(
                                                                  z => z.PartnerID == x.PartnerID).Discount.HasValue
                                                                  ? x.BookDescriptionCatalog.BookPublisher.BookPublisherMargins.
                                                                        First
                                                                        (
                                                                            z => z.PartnerID == x.PartnerID).Discount.Value
                                                                  : x.Partner.Discount)) / 100)) >= thi.ExportFilter.MinPrice) &&
                            (!thi.ExportFilter.MaxPrice.HasValue || (x.Partner == null
                                                                         ? 0
                                                                         : ((x.PriceOverride.HasValue && x.PriceOverride > 0)
                                                                                ? x.PriceOverride.Value
                                                                                : x.PartnerPrice * (100 +
                                                                                                  (x.Margin > 0
                                                                                                       ? x.Margin
                                                                                                       : (x.
                                                                                                              BookDescriptionCatalog
                                                                                                              .PublisherID.
                                                                                                              HasValue &&
                                                                                                          x.
                                                                                                              BookDescriptionCatalog
                                                                                                              .BookPublisher.
                                                                                                              BookPublisherMargins
                                                                                                              .Any(
                                                                                                                  z =>
                                                                                                                  z.PartnerID ==
                                                                                                                  x.PartnerID) &&
                                                                                                          x.
                                                                                                              BookDescriptionCatalog
                                                                                                              .BookPublisher.
                                                                                                              BookPublisherMargins
                                                                                                              .First(
                                                                                                                  z =>
                                                                                                                  z.PartnerID ==
                                                                                                                  x.PartnerID).
                                                                                                              Margin.HasValue
                                                                                                              ? (x.
                                                                                                                    BookDescriptionCatalog
                                                                                                                    .
                                                                                                                    BookPublisher
                                                                                                                    .
                                                                                                                    BookPublisherMargins
                                                                                                                    .First(
                                                                                                                        z =>
                                                                                                                        z.
                                                                                                                            PartnerID ==
                                                                                                                        x.
                                                                                                                            PartnerID)
                                                                                                                    .Margin.
                                                                                                                    Value)
                                                                                                              : (x.Partner.
                                                                                                                    Margin)))

                                                                                                  -
                                                                                                  (x.BookDescriptionCatalog.
                                                                                                       PublisherID.HasValue &&
                                                                                                   x.BookDescriptionCatalog.
                                                                                                       BookPublisher.
                                                                                                       BookPublisherMargins.Any(
                                                                                                           z =>
                                                                                                           z.PartnerID ==
                                                                                                           x.PartnerID) &&
                                                                                                   x.BookDescriptionCatalog.
                                                                                                       BookPublisher.
                                                                                                       BookPublisherMargins.
                                                                                                       First(
                                                                                                           z =>
                                                                                                           z.PartnerID ==
                                                                                                           x.PartnerID).Discount
                                                                                                       .HasValue
                                                                                                       ? x.
                                                                                                             BookDescriptionCatalog
                                                                                                             .BookPublisher.
                                                                                                             BookPublisherMargins
                                                                                                             .First
                                                                                                             (
                                                                                                                 z =>
                                                                                                                 z.PartnerID ==
                                                                                                                 x.PartnerID).
                                                                                                             Discount.Value
                                                                                                       : x.Partner.Discount)) /
                                                                                  100)) <= thi.ExportFilter.MaxPrice) &&
                            x.PartnerPrice > 0 &&
                            partnersListIds.Contains(x.PartnerID) && x.BookPageRels.Any(z => pages.Any(c=> c.ID == z.PageID))).
                            GroupBy(x => x.DescriptionID).
                            Select(x => new
                                {
                                    Priority =
                                            x.Min(
                                                z =>
                                                z.BookPageRels.First().CMSPage.PartnerPriorities.Any(
                                                    v => v.PartnerID == z.PartnerID)
                                                    ? z.BookPageRels.First().CMSPage.PartnerPriorities.First(
                                                        v => v.PartnerID == z.PartnerID).Priority
                                                    : z.Partner.SalePriority),
                                    Item = x
                                })
                            .Select(x => x.Item.Where(z=> z.PartnerPrice>0 && z.Partner.Enabled && z.IsAvailable && z.BookPageRels.Any()).FirstOrDefault(z => z.Partner.SalePriority == x.Priority ));

                    var exporter = new YmlExporter(filtered);

                    if (thi.ProgressFunc != null)
                        exporter.OnOrderFormed = thi.ProgressFunc;

                    var catalog = exporter.CreateYmlCatalog();
                    info.AddMessage("Запущено формирование YML структуры.");
                    var result = exporter.ExportToFile(catalog, thi.ExportFilter.UseZip);
                    info.AddMessage("Файл сформирован.");
                    info.AddMessage(DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " - Обработка завершена.");
                    info.AddMessage(
                        "<b style='font-size:14px'>Файл доступен по этой ссылке - <a target='_blank' href='{0}'>{0}</a></b>"
                            .FormatWith(
                                result));
                    info.EndDate = DateTime.Now;
                }

                protected void OnOrderFormedYandex(int current, int total)
                {
                    ParseringInfo info = ParseringInfo.Create("YandexExport");
                    info.Created = current;
                    info.Updated = total;
                }*/
    }
}
