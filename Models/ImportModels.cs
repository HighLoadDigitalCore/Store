using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using NPOI.SS.UserModel;
using Smoking.Extensions;

namespace Smoking.Models
{
    /*    public delegate void PostProcessingDelegate(int saleID, ImportData args, Ex dl);
        public delegate void  PrepareRecordDelegate(ref ImportData import);

       */


    public class StoreImporterRow : List<StoreImporter>, IExportRow
    {
        public StoreImporterRow(IEnumerable<StoreImporter> list)
        {
            this.AddRange(list);
        }

        public string GetValueFor(string name)
        {
            var exist = this.Any(x => x.ColumnName == name);
            var rv = exist ? this.First(x => x.ColumnName == name).XlsValue : "";
            return rv;
        }
    }

    public class StoreImporterColumn
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public bool IsRequired { get; set; }
    }

    public partial class StoreImporter
    {

        public string XlsValue { get; set; }
        public StoreImporter ToImportColumn(IRow xlsRow)
        {

            //if (xlsRow.Cells.Count >= this.ColumnNum - 1)
            {
                var cell = xlsRow.GetCell(ColumnNum - 1);
                if (cell == null) XlsValue = "";
                else
                {

                    if (cell.CellType == CellType.Numeric)
                        XlsValue = cell.NumericCellValue.ToString();
                    else
                    {
                        try
                        {
                            if (cell.RichStringCellValue != null && cell.RichStringCellValue.String.IsFilled())
                                XlsValue = cell.RichStringCellValue.ToString();
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            XlsValue = cell.StringCellValue;
                        }
                        catch
                        {
                            XlsValue = "";
                        }
                    }
                }
            }

            if (XlsValue.IsNullOrEmpty())
                XlsValue = "";

            var rv = new StoreImporter();
            rv.LoadPossibleProperties(this);
            return rv;
        }

        private static List<StoreImporterColumn> _requiredColumns;
        public static List<StoreImporterColumn> RequiredColumns
        {
            get
            {
                return _requiredColumns ?? (_requiredColumns = new List<StoreImporterColumn>()
                {
                    new StoreImporterColumn()
                    {
                        Name = "Номер (ID)",
                        IsRequired = true,
                        Key = "ID"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Где находится? (родительский ID)",
                        IsRequired = true,
                        Key = "ParentID"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Сортировка папок",
                        IsRequired = true,
                        Key = "CategoryOrder"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Сортировка товаров",
                        IsRequired = true,
                        Key = "ProductOrder"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "URL",
                        IsRequired = true,
                        Key = "URL"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Title страницы",
                        IsRequired = true,
                        Key = "PageTitle"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Ключевые слова (Keywords)",
                        IsRequired = true,
                        Key = "PageKeywords"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Описание (Description)",
                        IsRequired = true,
                        Key = "PageDescription"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "H1",
                        IsRequired = true,
                        Key = "H1"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "H2",
                        IsRequired = true,
                        Key = "H2"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "H3",
                        IsRequired = true,
                        Key = "H3"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Описание под H2",
                        IsRequired = true,
                        Key = "TextUnderH2"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Описание под H3",
                        IsRequired = true,
                        Key = "TextUnderH3"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Артикул",
                        IsRequired = true,
                        Key = "Artikul"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Вес",
                        IsRequired = true,
                        Key = "Weight"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Объем",
                        IsRequired = true,
                        Key = "Volume"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Скидка",
                        IsRequired = true,
                        Key = "Discount"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Отображать в каталоге",
                        IsRequired = true,
                        Key = "Visible"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Цена",
                        IsRequired = true,
                        Key = "Price"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Характеристика",
                        IsRequired = false,
                        Key = "Character"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Название товара (модель)",
                        IsRequired = false,
                        Key = "Name"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Поисковые слова",
                        IsRequired = true,
                        Key = "SearchTags"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Список товаров для блока \"С этим товаром покупают\"",
                        IsRequired = false,
                        Key = "RelatedProductsSame"
                    },
                    new StoreImporterColumn()
                    {
                        Name = "Список товаров для блока \"Рекомендуем купить\"",
                        IsRequired = false,
                        Key = "RelatedProductsBuy"
                    }, 
                    new StoreImporterColumn()
                    {
                        Name = "Список товаров для блока \"Похожие товары\"",
                        IsRequired = false,
                        Key = "RelatedProductsSimilar"
                    },    
                    
                    new StoreImporterColumn()
                    {
                        Name = "Список категорий для левого меню",
                        IsRequired = false,
                        Key = "RelatedCategories"
                    },
                });
            }
        }
    }


    public delegate void PostProcessingDelegate(int saleID, IExportRow args, ImportInfo dl);
    public delegate void PrepareRecordDelegate(ref IExportRow import);

    public class ThreadCatalogParserInfo
    {
        public List<IExportRow> DataList { get; set; }
        public HttpContext Context { get; set; }
        public PrepareRecordDelegate PrepareRecordFunc { get; set; }
        public PostProcessingDelegate PostProcessingFunc { get; set; }
        public ImportInfo ImportInfo { get; set; }

    }

    public class ImportInfo
    {
        public bool DeleteExpired { get; set; }
        public string AdditionalPath { get; set; }
        public string Message { get; set; }

        public int? CategoryID { get; set; }
    }

    public class ThreadStartData
    {
        public HttpContext Context { get; set; }
        public int ProviderID { get; set; }
        public string URL { get; set; }
        public bool IsBook { get; set; }
    }

    [Serializable]
    public class ParseringInfo
    {
        public static ParseringInfo Create(string key)
        {
            if (HttpContext.Current.Application[key] == null)
                HttpContext.Current.Application[key] = new ParseringInfo();
            return HttpContext.Current.Application[key] as ParseringInfo;

        }
        public static ParseringInfo Reset(string key)
        {
            HttpContext.Current.Application[key] = null;
            return Create(key);
        }
        protected ParseringInfo()
        {
            Messages = new List<KeyValuePair<string, bool>>();
        }

        protected List<KeyValuePair<string, bool>> Messages { get; set; }
        public string MessageList
        {
            get { return string.Join("<br>", Messages.Select(x => x.Key).ToArray()); }
        }

        public string ErrorList
        {
            get { return string.Join("<br>", Messages.Where(x => x.Value).Select(x => x.Key).ToArray()); }
        }

        public void AddMessage(string message, bool error = false)
        {
            if (Messages.Count >= 100)
                Messages.RemoveRange(0, 10);
            Messages.Add(new KeyValuePair<string, bool>(message, error));
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [ScriptIgnore]
        public string ParseURL { get; set; }

        public int Deleted { get; set; }
        public int Dirs { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Errors { get; set; }
        public int Prepared { get; set; }
        public int Total { get { return Created + Updated + Errors; } }

        public bool Break { get; set; }


        protected List<int> ItemsProcessed = new List<int>();

        public void resetList()
        {
            ItemsProcessed.Clear();
        }

        public List<int> getProcessedList()
        {
            return ItemsProcessed;
        }

        public void AddProcessedItem(int itemID)
        {
            lock (ItemsProcessed)
            {
                ItemsProcessed.Add(itemID);
            }
        }
        public bool IsItemProcessed(int itemID)
        {

            lock (ItemsProcessed)
            {
                return ItemsProcessed.Contains(itemID);
            }
        }
    }



}