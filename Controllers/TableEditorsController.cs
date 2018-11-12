using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Ajax.Utilities;
using Microsoft.JScript;
using NPOI.SS.Formula.Functions;
using Smoking.Extensions;
using Smoking.Extensions.Helpers;
using Smoking.Models;


namespace Smoking.Controllers
{
    public class TableEditorsController : Controller
    {
        private DB db = new DB();


        #region Редактор видео
        [AuthorizeMaster]
        /*[MenuItem("Видео блоки", 552, Icon = "play", ParentID = 5)]*/
        public ActionResult VideoEditor(string Type, int? Page, int? UID, int? LangID, int? CMSPageID, int? ViewID, int? CategoryID, int? ProductID)
        {
            if (!LangID.HasValue)
            {
                LangID = (db.Languages.FirstOrDefault(x => x.ShortName == "ru") ?? db.Languages.First()).ID;
            }


            var editedRow = (db.CMSPageVideos.FirstOrDefault(x => x.ID == UID) ??
                             new CMSPageVideo
                                 {
                                     LangID = LangID.Value,
                                     CMSPageID = !CMSPageID.HasValue || CMSPageID.Value == 0 ? (int?)null : CMSPageID.Value,
                                     ViewID = !ViewID.HasValue || ViewID.Value == 0 ? (int?)null : ViewID.Value,
                                     CategoryID = !CategoryID.HasValue || CategoryID.Value == 0 ? (int?)null : CategoryID.Value,
                                     AutoPlay = false,
                                     FilePath = "",
                                     Name = "",
                                     Width = 780,
                                     Height = 515,
                                     OrderNum = db.CMSPageVideos.Count() + 1,
                                     Visible = true
                                 });





            var settings = new UniversalEditorSettings
            {
                TableName = "CMSPageVideos",
                HasDeleteColumn = true,
                CanAddNew = true,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                    {
                        new UniversalListField
                        {
                            FieldName = "Name",
                            IsLinkToEdit = true,
                            HeaderText = "Название ролика"
                        },
                        new UniversalListField
                        {
                            FieldName = "Visible",
                            HeaderText = "Статус",
                            TextFunction = x => (bool) x ? "Отображается" : "Неактивен"
                        },
                        new UniversalListField
                        {
                            FieldName = "OrderNum",
                            IsLinkToEdit = false,
                            IsOrderColumn = true,
                            HeaderText = "Порядок"
                        }
                    },

                EditedFieldsList =
                    new List<UniversalEditorField>
                    {
                        new UniversalEditorField
                        {
                            FieldName = "LangID",
                            Hidden = true,
                            FieldType = UniversalEditorFieldType.DropDown,
                            InnerListDataSource = new UniversalDataSource
                            {
                                DefValue = AccessHelper.CurrentLang.ID,
                                HasEmptyDef = false,
                                KeyField = "ID",
                                ValueField = "Name",
                                Source = db.Languages.Where(x => x.Enabled),

                            },
                            HeaderText = "Язык",
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },
                        new UniversalEditorField
                        {

                            FieldName = "CMSPageID",
                            Hidden = true,

                            HeaderText = "Страница",
                        },
                        new UniversalEditorField
                        {
                            FieldName = "ViewID",
                            Hidden = true,

                            HeaderText = "Контейнер",
                        },
                        new UniversalEditorField
                        {
                            FieldName = "CategoryID",
                            Hidden = true,

                            HeaderText = "Категория",
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Name",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Название",
                            DataType = typeof (string),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Description",
                            FieldType = UniversalEditorFieldType.TextArea,
                            HeaderText = "Описание",
                            DataType = typeof (string),
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Visible",
                            FieldType = UniversalEditorFieldType.CheckBox,
                            HeaderText = "Отображать на сайте",
                            DataType = typeof (bool),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },
                        new UniversalEditorField
                        {
                            FieldName = "PreviewPath",
                            FieldType = UniversalEditorFieldType.FileImageUpload,
                            AdditionalData = "/content/Video/",
                            AdditionalTypeFlag = true,
                            HeaderText = "Превью",
                            DataType = typeof (string),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },

                                                new UniversalEditorField
                        {
                            FieldName = "ManualPath",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Путь к видео",
                            DataType = typeof (string),
                        },


                        new UniversalEditorField
                        {
                            FieldName = "FilePath",
                            FieldType = UniversalEditorFieldType.FileImageUpload,
                            AdditionalData = "/content/Video/",
                            AdditionalTypeFlag = false,
                            HeaderText = "Видео-файл",
                            DataType = typeof (string),
//                            Modificators =
//                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Width",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Ширина",
                            DataType = typeof (int),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },
                        
                        new UniversalEditorField
                        {
                            FieldName = "Height",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Высота",
                            DataType = typeof (int),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },

                        new UniversalEditorField
                        {
                            FieldName = "AutoPlay",
                            FieldType = UniversalEditorFieldType.CheckBox,
                            HeaderText = "Автостарт",
                            DataType = typeof (bool),
                            Hidden = true
                        },

                    }

            };



            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);

            IQueryable<CMSPageVideo> src = null;
            if (ViewID.HasValue && CMSPageID.HasValue && ViewID.Value > 0 && CMSPageID.Value > 0)
            {
                src = db.CMSPageVideos.Where(
                    x => x.ViewID == ViewID && x.LangID == LangID && x.CMSPageID == CMSPageID)
                    .OrderBy(x => x.OrderNum);
            }
            else if (CategoryID.HasValue)
            {
                src = db.CMSPageVideos.Where(
                    x => x.CategoryID == CategoryID && x.LangID == LangID)
                    .OrderBy(x => x.OrderNum);

            }
            /*
               else if (ProductID.HasValue)
               {
                   src = db.CMSPageSliders.Where(
                       x => x.ProductID == ProductID && x.LangID == LangID)
                       .OrderBy(x => x.OrderNum);

               }*/

            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageVideo>(
                              src, Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.AddQueryParams = new[] { "LangID", "CMSPageID", "ViewID", "CategoryID", "ProductID" };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageVideo)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "VideoEditor";
            data.EditorName = "Редактирование видео";
            data.EditorDescription = "";
            data.ShowInPopup = true;

            data.BeforeSaveRow = AfterSaveVideo;

            /*
                        settings.FilterDescription = "Выберите страницу и контейнер";
                        settings.AutoFilter = true;
                        settings.Filters = new List<FilterConfiguration>
                            {
                                new FilterConfiguration
                                    {
                                        FilterSource = new UniversalDataSource
                                            {
                                                DefValue = CMSPageID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "PageName",
                                                Source =


                                                    db.CMSPages.Where(
                                                        x => 
                                                        x.PageType.CMSPageCells.Any(
                                                            c =>
                                                            c.CMSPageCellViews.Any(
                                                                v => v.Action == "Index" && v.Controller == "VideoPage")))
                                                      .ToList()
                                                      .Select(x => x.LoadLangValues()),



                                            },
                                        HeaderText = "Страница",
                                        IsDropDown = true,
                                        QueryKey = "CMSPageID",
                                        Type = FilterType.Integer,
                                        MainFilter = true
                                    },
                                new FilterConfiguration
                                    {
                                        FilterSource = new UniversalDataSource
                                            {
                                                DefValue = ViewID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Description",
                                                Source = blocks
                                            },

                                        HeaderText = "Контейнер",
                                        IsDropDown = true,
                                        QueryKey = "ViewID",
                                        Type = FilterType.Integer,

                                    }
                            };


                        var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
                        var data = new UniversalEditorPagedData
                        {
                            PagedData =
                                type == CurrentEditorType.List
                                    ? new PagedData<CMSPageVideo>(
                                          db.CMSPageVideos.Where(
                                              x => x.ViewID == ViewID && x.LangID == LangID && x.CMSPageID == CMSPageID)
                                            .OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master", settings.Filters)
                                    : null,
                            Settings = settings,
                            CurrentType = type,
                            EditedRow =
                                type == CurrentEditorType.List
                                    ? null
                                    : editedRow
                        };
                        data.AddQueryParams = new[] { "CMSPageID", "LangID", "ViewID" };
                        data.IsAddingNew = data.EditedRow != null && ((CMSPageVideo)data.EditedRow).ID == 0;
                        data.CallerController = "TableEditors";
                        data.CallerAction = "VideoEditor";
                        data.EditorName = "Редактирование видео блоков";
                        data.EditorDescription = "На этой странице можно отредактировать все видео-блоки, размещенные на сайте";
                        data.ShowInPopup = true;
                        data.PreviewData = new PreviewData() { Type = 0, UID = CMSPageID.Value };*/
            return View("TableEditor", data);

        }

        private string AfterSaveVideo(object row, DB db, HttpContextBase context)
        {
            var r = (CMSPageVideo)row;
            if (r.ManualPath.IsFilled())
            {
                if (System.IO.File.Exists(context.Server.MapPath(r.ManualPath)))
                {
                    r.FilePath = r.ManualPath;
                }
                else if (System.IO.File.Exists(context.Server.MapPath("/content/Video/" + r.ManualPath)))
                {
                    r.FilePath = "/content/Video/" + r.ManualPath;
                }
                else if (System.IO.File.Exists(context.Server.MapPath("/content/Video/" + r.ManualPath + ".flv")))
                {
                    r.FilePath = "/content/Video/" + r.ManualPath + ".flv";
                }
            }

            if (r.ManualPath.IsNullOrEmpty() && r.FilePath.IsNullOrEmpty())
            {
                return "Необходимо выбрать или указать файл";
            }

            return "";
        }

        #endregion


        #region Редактор шаблонов чата
        [AuthorizeMaster]
        [MenuItem("Шаблонные сообщения", 790, 70, Icon = "templatechat")]
        public ActionResult ChatAnswerEditor(string Type, int? Page, int? UID)
        {

            var editedRow = (db.CharAnswerTemplates.FirstOrDefault(x => x.ID == UID) ??
                             new CharAnswerTemplate
                                 {
                                 });





            var settings = new UniversalEditorSettings
                {
                    TableName = "CharAnswerTemplates",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Answer",
                                        IsLinkToEdit = true,
                                        HeaderText = "Текст ответа"
                                    },
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                             
                                new UniversalEditorField
                                    {
                                        FieldName = "Answer",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Текст ответа",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                         

                            }

                };




            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CharAnswerTemplate>(
                              db.CharAnswerTemplates
                                .OrderBy(x => x.Answer), Page ?? 0, 10, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((CharAnswerTemplate)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "ChatAnswerEditor";
            data.EditorName = "Шаблонные сообщения";
            data.EditorDescription = ""; // "На этой странице можно отредактировать стандартный сообщения, которые используются для ответов в чате";
            return View("TableEditor", data);

        }

        #endregion


        public ActionResult Pages(string Type, int? Page, int? UID, int? ParentID)
        {

            var settings = new UniversalEditorSettings
            {
                TableName = "CMSPages",
                HasDeleteColumn = false,
                CanAddNew = false,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "PageName",
                                        IsLinkToEdit = false,
                                        HeaderText = "Название папки"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        HeaderText = "Порядковый номер",
                                        IsOrderColumn = true,
                                    }
                            },

                EditedFieldsList =
                    new List<UniversalEditorField>
                            {


                            }
            };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var ds =
                new PagedData<CMSPage>(
                    db.CMSPages.Where(
                        x => (ParentID.HasValue && ParentID.Value > 0 ? x.ParentID == ParentID.Value : !x.ParentID.HasValue) && !x.Deleted).AsEnumerable().Select(x => x.LoadLangValues()).AsQueryable()
                        .OrderBy(x => x.OrderNum), Page ?? 0, 30, "Master", null, true);

            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? ds
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow = null
            };
            data.AddQueryParams = new[] { "ParentID" };
            /*data.BeforeDelFunc = BeforeDelCategory;*/
            data.IsAddingNew = false;

            data.CallerController = "TableEditors";
            data.CallerAction = "Pages";
            data.EditorName = "Сортировка папок";
            data.HorizTabs = true;
            data.EditorUID = "Pages";

            //data.EditorDescription = "На этой странице вы можете отредактировать разделы каталога";
            data.IsPartial = true;

            data.EditorDescription = "";
            data.PreviewData = new PreviewData();
            return PartialView("TableEditorPartial", data);
        }


        #region Редактор категорий
        [AuthorizeMaster]
        /*[MenuItem("Разделы каталога", 41, 4)]*/
        public ActionResult Categories(string Type, int? Page, int? UID, int? ParentID)
        {
            StoreCategory editedRow = null;
            if (!UID.HasValue)
            {
                if (!ParentID.HasValue || ParentID.Value == 0)
                {
                    ParentID = 1;
                }
                editedRow = new StoreCategory()
                {
                    LastMod = DateTime.Now,
                    OrderNum = db.StoreCategories.Count() + 1,
                    ShowInMenu = true,
                    ShowSlider = true,
                    ShowArticles = true,
                    ShowInCatalog = true,
                    ShowInBreadcrumb = true,
                    ShowBigIcons = false
                };
            }
            else
            {
                editedRow = db.StoreCategories.FirstOrDefault(x => x.ID == UID) ?? new StoreCategory()
                {
                    LastMod = DateTime.Now,
                    OrderNum = db.StoreCategories.Count() + 1,
                    ShowInMenu = true,
                    ShowSlider = true,
                    ShowArticles = true,
                    ShowInCatalog = true,
                    ShowInBreadcrumb = true,
                    ShowBigIcons = false

                };
            }

            var parentRow = ParentID.HasValue
                ? db.StoreCategories.FirstOrDefault(x => x.ID == ParentID)
                : (editedRow.ID > 0 && editedRow.ParentID.HasValue
                    ? db.StoreCategories.FirstOrDefault(x => x.ID == editedRow.ParentID)
                    : new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false });

            var settings = new UniversalEditorSettings
                {
                    TableName = "StoreCategories",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название категории"
                                    },
                                    new UniversalListField()
                                        {
                                            FieldName = "ID",
                                            IsLinkToEdit = false,
                                            HeaderText = "Товары раздела",
                                            TextFunction = x=> "<a onclick='loadByLink(this); return false;' href='/Master/ru/TableEditors/Products?ParentID={0}'>перейти</a>".FormatWith(x)
                                        },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        HeaderText = "Порядковый номер",
                                        IsOrderColumn = true,
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                             



                                                                    new UniversalEditorField
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ShowArticles",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать артикулы",
                                        DataType = typeof (bool)
                                    },
                                         new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

/*
                                new UniversalEditorField
                                    {
                                        GroupName = "Основные данные",
                                        FieldName = "ParentID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Родительский раздел",
                                        DataType = typeof (int),
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = parentRow.ID,
                                                
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Name",
                                                Source =
                                                
                                                    db.StoreCategories.Where(x=> (x.ID != editedRow.ID && !x.Deleted) || x.ID == 1).OrderBy(x=> x.Name),
                                            },
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
*/                                new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "Slug",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "URL",
                                        DataType = typeof (string),
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "Slug",
                                                Source = db.StoreCategories.Where(x=> !x.Deleted)
                                            },
                                        Modificators =
                                            new List<IUniversalFieldModificator>
                                                {
                                                    new UniqueModificator(editedRow.ID),
                                                    new RequiredModificator()
                                                }
                                    },
                                          new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "PageTitle",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Title",
                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "PageKeywords",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Keywords",
                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "PageDescription",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Description",
                                    },
                                
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ShowSlider",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать фотогалерею",
                                        DataType = typeof(bool)
                                    },
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ShowInMenu",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать в левом меню",
                                        DataType = typeof(bool)
                                    },
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ShowInCatalog",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать в центре",
                                        DataType = typeof(bool)
                                    },
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ShowInBreadcrumb",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать в обр следе",
                                        DataType = typeof(bool)
                                    },
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ShowBigIcons",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать большие папки",
                                        DataType = typeof(bool)
                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ID",
                                        FieldType = UniversalEditorFieldType.CatalogSlider,
                                        HeaderText = "Слайдер",
                                        DataType = typeof (int)

                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ID",
                                        FieldType = UniversalEditorFieldType.CatalogVideo,
                                        HeaderText = "Видео",
                                        DataType = typeof (int)

                                    },
                             
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "StaticDescription",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 1",
                                        DataType = typeof (string)

                                    },



                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "StaticDescriptionLower",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 2",
                                        DataType = typeof (string)

                                    },                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "StaticDescriptionA",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 3",
                                        DataType = typeof (string)

                                    },                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "StaticDescriptionB",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 4",
                                        DataType = typeof (string)

                                    },                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "StaticDescriptionC",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 5",
                                        DataType = typeof (string)

                                    },

                                                                    new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim1",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Cвернуть/развернуть 1",
                                        DataType = typeof (bool)

                                    },


                                    
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim2",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Cвернуть/развернуть 2",
                                        DataType = typeof (bool)

                                    },

     new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim3",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Cвернуть/развернуть 3",
                                        DataType = typeof (bool)

                                    },

     new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim4",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Cвернуть/развернуть 4",
                                        DataType = typeof (bool)

                                    },

     new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim5",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Cвернуть/развернуть 5",
                                        DataType = typeof (bool)

                                    },



                                       new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "PageHeader",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Заголовок H1",
                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "PageSubHeader",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Заголовок H2",
                                    },
                                       new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "PageTextH3Upper",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Описание XLS 3",
                                        DataType = typeof (string)

                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "PageHeaderH3",
                                        FieldType = UniversalEditorFieldType.Hidden,
                                        HeaderText = "Заголовок H3",
                                    },
                                                            
                                    new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "PageTextH3Lower",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Описание XLS 4",
                                        DataType = typeof (string)

                                    },

                            

                                new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "Image",
                                        FieldType = UniversalEditorFieldType.DBImageUpload,
                                        HeaderText = "Фото для меню" /*,
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}*/
                                    },

                                       new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "MenuImageAlt",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Alt для SEO",
                                    },
                                    new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "MenuImageTitle",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Подсказка",
                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "ID",
                                        FieldType = UniversalEditorFieldType.Delimeter,
                                        HeaderText = "",
                                    },

                                new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "CategoryImage",
                                        FieldType = UniversalEditorFieldType.DBImageUpload,
                                        HeaderText = "Фото для папки (и меню без наведения)" /*,
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}*/
                                    },
                                          new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "CategoryImageAlt",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Alt для SEO",
                                    },
                                    new UniversalEditorField
                                    {
                                        GroupName = "Фото",
                                        FieldName = "CategoryImageTitle",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Подсказка",
                                    },

                              /*  new UniversalEditorField
                                    {
                                        GroupName = "Информация для меню",
                                        FieldName = "MenuUpperText",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание во всплавающем меню"
                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Информация для меню",
                                        FieldName = "MenuImage",
                                        FieldType = UniversalEditorFieldType.FileImageUpload,
                                        AdditionalTypeFlag = true,
                                        AdditionalData = "/content/Catalog/",
                                        HeaderText = "Фоновое изображение для всплывающего меню"
                                    },*/
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Товары",
                                        FieldName = "ID",
                                        HeaderText = "Загрузка товаров",
                                        FieldType = UniversalEditorFieldType.Custom,
                                        AdditionalData = "/Master/ru/Import/ProductImport?Category="+editedRow.ID
                                    },
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Фильтр",
                                        FieldName = "ID",
                                        HeaderText = "Настройки фильтра",
                                        FieldType = UniversalEditorFieldType.Custom,
                                        AdditionalData = "/Master/ru/Filter/Settings?Category="+editedRow.ID
                                    },
                                    new UniversalEditorField()
                                    {
                                        GroupName = "Файлы",
                                        FieldName = "ID",
                                        HeaderText = "Загрузка файлов",
                                        FieldType = UniversalEditorFieldType.Custom,
                                        AdditionalData = "/Master/ru/Files/Settings?Category="+editedRow.ID
                                    },
                                                                    
                                    new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "TagList",
                                        FieldType = UniversalEditorFieldType.TagBox,
                                        HeaderText = "Слова для поиска",
                                    },

                                  /*  new UniversalEditorField()
                                    {
                                        GroupName = "Выгрузка товаров",
                                        FieldName = "ID",
                                        HeaderText = "Выгрузка товаров",
                                        FieldType = UniversalEditorFieldType.Custom,
                                        AdditionalData = "/Master/ru/Export/ProductExport?Category="+editedRow.ID
                                    }*/
                                    
                                new UniversalEditorField
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "ParentID",
                                        FieldType = UniversalEditorFieldType.TreeEditor,
                                        HeaderText = "Родительский раздел",
                                        AdditionalDataObject = 1,
                                        DataType = typeof (int),
                                        TreeDataSource = new TreeDataSource
                                            {
                                                
                                                DataLink = "/Master/ru/TableEditors/CategoryTreeHandler?category=" + (parentRow == null ? "" :  parentRow.ID.ToString()),
                                                //Url.Action("CatalogTreeHandler", "TableEditors", new{productId = editedRow.ID}),
                                                Message = "Необходимо выбрать родительский раздел",
                                                Name = "CategoryTree",
                                                Values = parentRow == null ? "" : parentRow.ID.ToString()
                                            },
                                              Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                            }
                };

            settings.AutoFilter = true;
            settings.FilterDescription = "Выберите раздел каталога";
            /*
                        settings.Filters = new List<FilterConfiguration>
                            {
                                new FilterConfiguration
                                    {
                                        FilterSource = new UniversalDataSource
                                            {
                                                DefValue = ParentID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Name",
                                                Source = 
                                                    db.StoreCategories.OrderBy(x=> x.ParentID).ThenBy(x=> x.OrderNum),
                                            },
                                        HeaderText = "Родительская категория",
                                        IsDropDown = true,
                                        QueryKey = "ParentID",
                                        Type = FilterType.Integer,
                                        MainFilter = true
                                    }
                            };
            */

            /*    settings.Filters = new List<FilterConfiguration>
                    {
                        new FilterConfiguration
                            {
                                FilterSource = new UniversalTreeDataSource()
                                    {
                                        LinkFunction =
                                            x =>
                                            Request.GenerateURL(new List<KeyValuePair<string, string>>
                                                {
                                                    new KeyValuePair<string, string>("ParentID", x.ToString())
                                                }),
                                        Type = SerializationType.Categories
                                    }
                            }
                    };
    */

            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<StoreCategory>(db.StoreCategories.Where(x => x.ParentID == ParentID && !x.Deleted).OrderBy(x => x.ParentID).ThenBy(x => x.OrderNum), Page ?? 0, 30, "Master", null, true)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.AddQueryParams = new[] { "ParentID" };
            /*data.BeforeDelFunc = BeforeDelCategory;*/
            data.DelFunc = RecicleCategory;

            data.IsAddingNew = data.EditedRow != null && ((StoreCategory)data.EditedRow).ID == 0;
            data.PreviewData = data.IsAddingNew || data.EditedRow == null
                                   ? new PreviewData() { Type = 1, UID = ParentID ?? 1 }
                                   : new PreviewData() { Type = 1, UID = ((StoreCategory)data.EditedRow).ID };
            data.CallerController = "TableEditors";
            data.CallerAction = "Categories";
            data.EditorName = "Редактирование подразделов каталога";
            data.BeforeSaveRow = (row, db1, context) =>
            {
                if (((StoreCategory)row).ID == 1)
                {
                    ((StoreCategory)row).ParentID = null;
                }
                return "";
            };
            data.HorizTabs = true;
            data.EditorUID = "Categories";
            data.CompleteSave = (pagedData, row) =>
            {
                pagedData.NewNode = "#c" + ((StoreCategory)row).ID;
                return "";
            };
            data.AfterSaveRow = (row, db1, context) =>
                {
                    CatalogBrowser.Init().ClearAllCaches();
                    return "";
                };
            if (data.CurrentType == CurrentEditorType.List)
            {
                data.EditorName = "Подразделы в папке '{0}'".FormatWith(parentRow == null ? "": parentRow.Name);
            }
            else if (data.CurrentType == CurrentEditorType.Edit)
            {
                if (editedRow.ID == 0)
                {
                    data.EditorName = "Добавление новой папки";
                }
                else
                {
                    data.EditorName = "{0}".FormatWith(editedRow.Name);
                }
            }
            else if (data.CurrentType == CurrentEditorType.Delete)
            {
                data.EditorName = "Удаление папки";
            }
            //data.EditorDescription = "На этой странице вы можете отредактировать разделы каталога";
            data.IsPartial = true;
            /*
                        data.Settings.ListLinks = new List<EditorLink>()
                            {
                                new EditorLink(){IsPartial = true, Name = "Редактировать '{0}'".FormatWith(parentRow.Name), Link = "/Master/ru/TableEditors/Categories?Type=Edit&UID={0}&Page=0".FormatWith(parentRow.ID)},
                                new EditorLink(){IsPartial = true, Name = "Перейти к списку товаров папки '{0}'".FormatWith(parentRow.Name), Link = "/Master/ru/TableEditors/Products?ParentID={0}&Type=List".FormatWith(parentRow.ID)}
                            };
            */


            return PartialView("TableEditorPartial", data);
        }

        private void BeforeDelCategory(object row, DB db)
        {
            var tr = db.StoreCategories.FirstOrDefault(x => x.ID == ((StoreCategory)row).ID);
            if (tr == null)
                return;
            if (tr.Children.Any())
            {
                var arr = tr.Children.ToArray();
                foreach (var child in arr)
                {
                    BeforeDelCategory(child, db);
                }
            }
            db.Refresh(RefreshMode.KeepChanges, tr);
            db.StoreCategories.DeleteOnSubmit(tr);
            db.SubmitChanges();

            CatalogBrowser.Init().ClearAllCaches();
        }
        private void RecicleCategory(object row, DB db)
        {
            var tr = db.StoreCategories.FirstOrDefault(x => x.ID == ((StoreCategory)row).ID);
            if (tr == null)
                return;
            if (tr.Children.Any())
            {
                var arr = tr.Children.ToArray();
                foreach (var child in arr)
                {
                    RecicleCategory(child, db);
                }
            }
            db.Refresh(RefreshMode.KeepChanges, tr);
            tr.Deleted = true;

            var prods = db.StoreProductsToCategories.Where(x => x.CategoryID == tr.ID);
            foreach (var productsToCategory in prods)
            {
                productsToCategory.StoreProduct.Deleted = true;
            }

            db.SubmitChanges();

            CatalogBrowser.Init().ClearAllCaches();
        }

        #endregion

        #region Редактор товаров
        [AuthorizeMaster]
        /*[MenuItem("Товары каталога", 42, 4)]*/
        public ActionResult Products(string Type, int? Page, int? UID, int? ParentID)
        {


            var editedRow = db.StoreProducts.FirstOrDefault(x => x.ID == UID) ??
                            new StoreProduct
                            {
                                AddDate = DateTime.Now,
                                LastMod = DateTime.Now,
                                IsActive = true,
                                Discount = 0,
                                VoteCount = 0,
                                VoteOverage = 0,
                                VoteSum = 0,
                                //OrderNum = db.StoreCategories.Count() + 1
                            };

            editedRow.SimilarName =
                editedRow.StoreProductRelations.Where(x => x.GroupName == "similar")
                    .ToList()
                    .Select(
                        x =>
                            x.BaseProductReverse.Article.IsNullOrEmpty()
                                ? x.BaseProductReverse.SlugOrId
                                : x.BaseProductReverse.Article)
                    .JoinToString(";");
            editedRow.RecomendName =
                editedRow.StoreProductRelations.Where(x => x.GroupName == "recomend")
                    .ToList()
                    .Select(
                        x =>
                            x.BaseProductReverse.Article.IsNullOrEmpty()
                                ? x.BaseProductReverse.SlugOrId
                                : x.BaseProductReverse.Article)
                    .JoinToString(";");
            editedRow.SameName =
                editedRow.StoreProductRelations.Where(x => x.GroupName == "related")
                    .ToList()
                    .Select(
                        x =>
                            x.BaseProductReverse.Article.IsNullOrEmpty()
                                ? x.BaseProductReverse.SlugOrId
                                : x.BaseProductReverse.Article)
                    .JoinToString(";");


            var parentCat =
                ParentID.HasValue && ParentID.Value > 0 ? db.StoreCategories.First(x => x.ID == ParentID.Value) : (editedRow.StoreProductsToCategories.Any()
                    ? editedRow.StoreProductsToCategories.First().StoreCategory
                    : (db.StoreCategories.FirstOrDefault(x => x.ID == 1) ?? new StoreCategory() { LastMod = DateTime.Now, ShowBigIcons = false }));

            ParentID = parentCat.ID;


            var settings = new UniversalEditorSettings
                {
                    TableName = "StoreProducts",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название товара"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Price",
                                        IsLinkToEdit = false,
                                        HeaderText = "Цена",
                                        TextFunction = x => ((decimal) x).ToString("f0")
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "ID",
                                        IsLinkToEdit = false,
                                        HeaderText = "Фото",
                                        TextFunction =
                                            x => string.Format("<a href='/Master/ru/TableEditors/ImageGallery?Page=0&CategoryID={0}&ProductID={1}'>редактировать</a>", ParentID, x)
                                            
                                    },
/*
                                new UniversalListField
                                    {
                                        FieldName = "ID",
                                        IsLinkToEdit = false,
                                        HeaderText = "3D фото",
                                        TextFunction =
                                            x => string.Format("<a href='/Master/ru/ClientCatalog/Photo3DGallery?CategoryID={0}&ProductID={1}'>редактировать</a>", ParentID, x)
                                            
                                    },
*/

                                new UniversalListField
                                    {
                                        FieldName = "ID",
                                        IsOrderColumn = true,
                                        TextFunction =
                                            x =>
                                            db.StoreProductsToCategories.First(
                                                z => z.CategoryID == ParentID && z.ProductID == (int) x)
                                              .OrderNum.ToString(),

                                        ComplexReorder = new ComplexReorder
                                            {
                                                Key1 = "ProductID",
                                                Key2 = "CategoryID",
                                                TableName = "StoreProductsToCategories",
                                                OrderName = "OrderNum",
                                                TextFunction1 = x =>
                                                                db.StoreProductsToCategories.First(
                                                                    z =>
                                                                    z.CategoryID == ParentID && z.ProductID == (int) x)
                                                                  .ProductID.ToString(),
                                                TextFunction2 = x =>
                                                                db.StoreProductsToCategories.First(
                                                                    z =>
                                                                    z.CategoryID == ParentID && z.ProductID == (int) x)
                                                                  .CategoryID.ToString()
                                            }
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                
                                                                 new UniversalEditorField
                                    {
                                        GroupName = "Настройки",
                                        FieldName = "JoinedCats",
                                        FieldType = UniversalEditorFieldType.TreeEditor,
                                        HeaderText = "Выберите разделы",
                                        AdditionalDataObject = 0,
                                        TreeDataSource = new TreeDataSource
                                            {
                                                
                                                DataLink = "/Master/ru/TableEditors/CatalogTreeHandler?productId=" + editedRow.ID,
                                                //Url.Action("CatalogTreeHandler", "TableEditors", new{productId = editedRow.ID}),
                                                Message = "Необходимо выбрать хотя бы один раздел каталога для товара",
                                                Name = "CategoryTree",
                                                Values =
                                                    editedRow.StoreProductsToCategories.Where(x=> !x.StoreCategory.Deleted).Select(x => x.CategoryID)
                                                             .JoinToString(";")
                                            }
                                    },
                                     new UniversalEditorField
                                    {
                                        FieldName = "ShortName",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        GroupName = "SEO",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                     new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название полное",
                                        DataType = typeof (string),
                                        GroupName = "SEO",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                       new UniversalEditorField
                                    {
                                        FieldName = "Slug",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "URL",
                                        DataType = typeof (string),
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "Slug",
                                                Source = db.StoreProducts.AsEnumerable()
                                            },
                                        Modificators =
                                            new List<IUniversalFieldModificator>
                                                {
                                                    new UniqueModificator(editedRow.ID),
                                                    new RequiredModificator()
                                                }
                                    },
                                       new UniversalEditorField
                                    {
                                        FieldName = "PageTitle",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Title",
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "PageKeywords",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Keywords",
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "PageDescription",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Description",
                                    },
                                   
                                new UniversalEditorField
                                    {
                                        FieldName = "Article",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Артикул",
                                        GroupName = "Настройки",
                                        DataType = typeof (string)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Price",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Цена",
                                        GroupName = "Настройки",
                                        DataType = typeof (decimal),
                                        TextFunction = input => (((decimal)input).ToString("f0")),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                    new UniversalEditorField
                                    {
                                        FieldName = "Discount",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Скидка, %",
                                        DataType = typeof (int),
                                    },
   
/*
                                new UniversalEditorField
                                    {
                                        FieldName = "OldPrice",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Старая цена",
                                        DataType = typeof (decimal)
                                    },
*/
                                new UniversalEditorField
                                    {
                                        FieldName = "IsActive",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool)
                                    },
                             

                                new UniversalEditorField
                                    {
                                        FieldName = "AddDate",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.Calendar,
                                        HeaderText = "Дата создания",
                                        DataType = typeof (DateTime)

                                    },
                                        new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ID",
                                        FieldType = UniversalEditorFieldType.ProductSlider,
                                        HeaderText = "Слайдер",
                                        DataType = typeof (int)

                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "StaticDescription",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 1",
                                        DataType = typeof (string)

                                    },   
                      
                                           new UniversalEditorField
                                    {
                                        FieldName = "StaticDescriptionA",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 2",
                                        DataType = typeof (string)

                                    },          new UniversalEditorField
                                    {
                                        FieldName = "StaticDescriptionB",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 3",
                                        DataType = typeof (string)

                                    },          new UniversalEditorField
                                    {
                                        FieldName = "StaticDescriptionC",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 4",
                                        DataType = typeof (string)

                                    },          new UniversalEditorField
                                    {
                                        FieldName = "StaticDescriptionD",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Описание 5",
                                        DataType = typeof (string)

                                    },  
 
                                                           new UniversalEditorField
                                    {
                                        FieldName = "DeliveryPack",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Состав поставки",
                                        DataType = typeof (string)

                                    },
                                               
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim2",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Показать/скрыть 2",
                                        DataType = typeof (bool)

                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim3",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Показать/скрыть 3",
                                        DataType = typeof (bool)

                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim4",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Показать/скрыть 4",
                                        DataType = typeof (bool)

                                    },
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim5",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Показать/скрыть 5",
                                        DataType = typeof (bool)

                                    },


             

                                    
                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowDescrAnim",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Свертывать параметры",
                                        DataType = typeof (bool)

                                    },

                                new UniversalEditorField
                                    {
                                        GroupName = "Описание",
                                        FieldName = "ShowCompare",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать сравнение",
                                        DataType = typeof (bool)
                                    },




                                new UniversalEditorField
                                    {
                                        FieldName = "DescriptionShadow",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Описание XLS 2",
                                        DataType = typeof (string)

                                    },     
                                new UniversalEditorField
                                    {
                                        FieldName = "DescrptionLower",
                                        GroupName = "Описание",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Описание XLS 3",
                                        DataType = typeof (string)

                                    },     

                                new UniversalEditorField
                                    {
                                        FieldName = "Volume",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Объем (м3)",
                                        DataType = typeof (decimal)

                                    },     
                                new UniversalEditorField
                                    {
                                        FieldName = "Weight",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Вес (кг)",
                                        DataType = typeof (decimal)
                                    },     
                              
                             
   
                                    new UniversalEditorField()
                                        {
                                            GroupName = "Характеристики",
                                            FieldName = "JoinedChars",
                                            HeaderText = "Характеристики",
                                            AdditionalData = "/Master/ru/TableEditors/GetCharList/"+ editedRow.ID,
                                            FieldType = UniversalEditorFieldType.RelatedTable,
                                            DataType = typeof(string)
                                        },

                                    new UniversalEditorField()
                                        {
                                            GroupName = "Настройки",
                                            FieldName = "RelatedCategories",
                                            HeaderText = "Список категорий для левого меню",
                                            FieldType = UniversalEditorFieldType.Hidden,
                                            DataType = typeof(string)
                                        },

                                    new UniversalEditorField()
                                        {
                                            GroupName = "Похожие товары",
                                            FieldName = "RecomendName",
                                            HeaderText = "Товары для блока \"Рекомендуем купить\"",
                                            FieldType = UniversalEditorFieldType.TextBox,
                                            DataType = typeof(string)
                                        },

                                    new UniversalEditorField()
                                        {
                                            GroupName = "Похожие товары",
                                            FieldName = "SameName",
                                            HeaderText = "Товары для блока \"С этим товаром покупают\"",
                                            FieldType = UniversalEditorFieldType.TextBox,
                                            DataType = typeof(string)
                                        },

                                    new UniversalEditorField()
                                        {
                                            GroupName = "Похожие товары",
                                            FieldName = "SimilarName",
                                            HeaderText = "Товары для блока \"Похожие товары\"",
                                            FieldType = UniversalEditorFieldType.TextBox,
                                            DataType = typeof(string)
                                        },
                                    new UniversalEditorField()
                                        {
                                            GroupName = "Настройки",
                                            FieldName = "SearchWords",
                                            HeaderText = "Список похожих названий для товара (через пробел)",
                                            FieldType = UniversalEditorFieldType.Hidden,
                                            DataType = typeof(string)
                                        },
                                                                         new UniversalEditorField
                                    {
                                        FieldName = "VoteOverage",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.Hidden,
                                        HeaderText = "Рейтинг",
                                        DataType = typeof (decimal),
                                    },
                                    new UniversalEditorField
                                    {
                                        FieldName = "VoteCount",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.Hidden,
                                        HeaderText = "Кол-во проголосовавших",
                                        DataType = typeof (int),
                                    },
                                    new UniversalEditorField
                                    {
                                        FieldName = "VoteSum",
                                        GroupName = "Настройки",
                                        FieldType = UniversalEditorFieldType.Hidden,
                                        HeaderText = "Сумма баллов рейтига",
                                        DataType = typeof (int),
                                    },
                                      new UniversalEditorField
                                    {
                                        FieldName = "PageH1",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Заголовок H1",
                                        DataType = typeof (string)
                                    },     
                                new UniversalEditorField
                                    {
                                        FieldName = "PageH2",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Заголовок H2",
                                        DataType = typeof (string)
                                    },     
                                new UniversalEditorField
                                    {
                                        FieldName = "PageH3",
                                        GroupName = "SEO",
                                        FieldType = UniversalEditorFieldType.Hidden,
                                        HeaderText = "Заголовок H3",
                                        DataType = typeof (string)
                                    },     

                                                                    new UniversalEditorField
                                    {
                                        GroupName = "SEO",
                                        FieldName = "TagList",
                                        FieldType = UniversalEditorFieldType.TagBox,
                                        HeaderText = "Слова для поиска",
                                    },
                                        new UniversalEditorField()
                                    {
                                        GroupName = "Файлы",
                                        FieldName = "ID",
                                        HeaderText = "Загрузка файлов",
                                        FieldType = UniversalEditorFieldType.Custom,
                                        AdditionalData = "/Master/ru/Files/Settings?Product="+editedRow.ID
                                    },


                            }
                };

            settings.AutoFilter = true;
            settings.FilterDescription = "Выберите раздел каталога";
            /*            settings.Filters = new List<FilterConfiguration>
                            {
                                new FilterConfiguration
                                    {

                                        FilterSource = new UniversalDataSource
                                            {
                                                DefValue = ParentID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Name",
                                                Source =
                                                    db.StoreCategories.OrderBy(x => x.ParentID).ThenBy(x => x.OrderNum),
                                            },
                                        HeaderText = "Родительская категория",
                                        IsDropDown = true,
                                        QueryKey = "ParentID",
                                        Type = FilterType.Integer,
                                        MainFilter = true
                                    }
                            };*/
            /*  settings.Filters = new List<FilterConfiguration>
                  {
                      new FilterConfiguration
                          {
                              FilterSource = new UniversalTreeDataSource()
                                  {
                                      LinkFunction =
                                          x =>
                                          Request.GenerateURL(new List<KeyValuePair<string, string>>
                                              {
                                                  new KeyValuePair<string, string>("ParentID", x.ToString())
                                              }),
                                      Type = SerializationType.Categories
                                  }
                          }
                  };
  */

            /*
                        settings.ListLinks = new List<EditorLink>()
                            {
                                new EditorLink(){IsPartial = true, Name = "Редактировать раздел '{0}'".FormatWith(parentCat.Name), Link = "/Master/ru/TableEditors/Categories?Type=Edit&UID={0}&Page=0&ParentID={1}".FormatWith(parentCat.ID, parentCat.ParentID??0)},
                                new EditorLink(){IsPartial = true, Name = "Список подразделов для раздела '{0}'".FormatWith(parentCat.Name), Link = "/Master/ru/TableEditors/Categories?ParentID={0}".FormatWith(parentCat.ID)},
                            };
            */

            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
                {
                    PagedData =
                        type == CurrentEditorType.List
                            ? new PagedData<StoreProduct>(
                                  db.StoreProductsToCategories.Where(x => x.CategoryID == ParentID && !x.StoreCategory.Deleted)
                                    .OrderBy(x => x.OrderNum)
                                    .Select(x => x.StoreProduct).Where(x => !x.Deleted), Page ?? 0, 30, "Master", null, true)
                            : null,
                    Settings = settings,
                    CurrentType = type,
                    EditedRow =
                        type == CurrentEditorType.List
                            ? null
                            : editedRow,
                    AddQueryParams = new[] { "ParentID" }
                };
            data.HorizTabs = true;
            data.EditorUID = "Products";
            data.IsAddingNew = data.EditedRow != null && ((StoreProduct)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "Products";
            data.EditorName = "Редактирование товар";
            if (type == CurrentEditorType.List)
            {
                data.EditorName += "ов в разделе '{0}'".FormatWith(parentCat.Name);
            }
            else if (type == CurrentEditorType.Edit)
            {
                if (editedRow.ID == 0)
                {
                    data.EditorName = "Добавление нового товара";
                }
                else
                {
                    data.EditorName += "а '{0}'".FormatWith(editedRow.Name);
                }
            }
            else if (type == CurrentEditorType.Delete)
            {
                data.EditorName = "Удаление товара";
            }
            data.IsPartial = true;
            data.EditorDescription = "";//"На этой странице можно отредактировать все товары каталога";
            data.AfterSaveRow = SaveProductData;
            /*data.BeforeDelFunc = DeleteProductData;*/
            data.DelFunc = RecicleProductData;
            data.PreviewData = data.IsAddingNew || data.EditedRow == null
                       ? new PreviewData() { Type = 1, UID = ParentID.Value }
                       : new PreviewData() { Type = 2, UID = ((StoreProduct)data.EditedRow).ID };

            data.CompleteSave = (pagedData, row) =>
            {
                pagedData.NewNode = "#p" + ((StoreProduct)row).ID;
                return "";
            };

            //data.AfterSaveRow = AfterSaveRow;

            if (data.EditedRow != null)
            {
                /* data.Settings.EditLinks = new List<EditorLink>()
                 {
                     new EditorLink()
                     {
                         Name = "Добавить фото для товара '{0}'".FormatWith(((StoreProduct) data.EditedRow).Name),
                         Link =
                             "/Master/ru/TableEditors/ImageGallery?Type=Edit&ProductID={0}&CategoryID={1}&Page=0"
                                 .FormatWith(UID, ParentID)
                     },
                     new EditorLink()
                     {
                         Name =
                             "Редактировать фото-галерею для товара '{0}'".FormatWith(
                                 ((StoreProduct) data.EditedRow).Name),
                         Link =
                             "/Master/ru/TableEditors/ImageGallery?Type=List&ProductID={0}&CategoryID={1}&Page=0"
                                 .FormatWith(UID, ParentID)
                     },
                     new EditorLink()
                     {
                         Name = "Список комментариев для товара '{0}'".FormatWith(((StoreProduct) data.EditedRow).Name),
                         Link =
                             "/Master/ru/TableEditors/CommentProductEditor?Type=List&ProductID={0}&CategoryID={1}&Page=0"
                                 .FormatWith(UID, ParentID)
                     },

                 };*/
                data.Settings.EditLinks = new List<EditorLink>()
                {
                   
                    new EditorLink()
                    {
                        Name =
                            "Фото",
                        Link =
                            "/Master/ru/Image?ProductID={0}"
                                .FormatWith(UID, ParentID)
                    },
/*
                    new EditorLink()
                    {
                        Name = "Комментарии",
                        Link =
                            "/Master/ru/TableEditors/CommentProductEditor?Type=List&ProductID={0}&CategoryID={1}&Page=0"
                                .FormatWith(UID, ParentID)
                    },
*/

                };
            }

            return PartialView("TableEditorPartial", data);
        }

        private string AfterSaveRow(object row, DB db)
        {
            var p = (StoreProduct)row;
            var similar = p.SimilarName.Split<string>(";");
            var recomend = p.RecomendName.Split<string>(";");
            var related = p.SameName.Split<string>(";");
            ProcessRelation(similar, "similar", db, p);
            ProcessRelation(recomend, "recomend", db, p);
            ProcessRelation(related, "related", db, p);




            return "";
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

        private void DeleteProductData(object row, DB db)
        {
            var p = (StoreProduct)row;
            var rels = db.StoreProductRelations.Where(x => x.BaseProductID == p.ID || x.RelatedProductID == p.ID);
            if (rels.Any())
            {
                db.StoreProductRelations.DeleteAllOnSubmit(rels);
                db.SubmitChanges();
            }

            var catrels = db.StoreCategoryRelations.Where(x => x.RelatedProductID == p.ID);
            if (catrels.Any())
            {
                db.StoreCategoryRelations.DeleteAllOnSubmit(catrels);
                db.SubmitChanges();

            }
            CatalogBrowser.Init().ClearAllCaches();
        }

        private void RecicleProductData(object row, DB db)
        {
            var p = (StoreProduct)row;
            p.Deleted = true;
            db.SubmitChanges();



            CatalogBrowser.Init().ClearAllCaches();
        }

        private string SaveProductData(object row, DB db, HttpContextBase context)
        {
            try
            {
                var newRels = ((StoreProduct)row).JoinedCats.Split<int>(";");
                var rels = db.StoreProductsToCategories.Where(x => x.ProductID == ((StoreProduct)row).ID);
                var forDel = rels.Where(x => !newRels.Contains(x.CategoryID));
                if (forDel.Any())
                {
                    db.StoreProductsToCategories.DeleteAllOnSubmit(forDel);
                    db.SubmitChanges();
                }
                var forAdd =
                    newRels.Where(x => !rels.Select(z => z.CategoryID).Contains(x))
                           .Select(
                               x => new StoreProductsToCategory { CategoryID = x, ProductID = ((StoreProduct)row).ID }).ToList();
                if (forAdd.Any())
                {
                    db.StoreProductsToCategories.InsertAllOnSubmit(forAdd);
                    db.SubmitChanges();
                }


                var tags = ((StoreProduct)row).TagList.Split<string>(",").ToList();
                var tagRels = db.StoreProductTagRels.Where(x => x.ProductID == ((StoreProduct)row).ID);
                var tagsForDel = tagRels.Where(x => !tags.Contains(x.StoreProductTag.Tag));
                if (tagsForDel.Any())
                {
                    db.StoreProductTagRels.DeleteAllOnSubmit(tagsForDel);
                    db.SubmitChanges();
                }

                var empty = db.StoreProductTags.Where(x => !x.StoreProductTagRels.Any());
                if (empty.Any())
                {
                    db.StoreProductTags.DeleteAllOnSubmit(empty);
                    db.SubmitChanges();
                }

                var tagsForAdd =
                 tags.Where(x => !tagRels.Select(z => z.StoreProductTag.Tag).Contains(x));

                foreach (var ta in tagsForAdd)
                {
                    var exist = db.StoreProductTags.FirstOrDefault(x => x.Tag == ta.ToLower().Trim());
                    if (exist == null)
                    {
                        exist = new StoreProductTag() { Tag = ta.ToLower().Trim() };
                        db.StoreProductTags.InsertOnSubmit(exist);
                    }
                    db.StoreProductTagRels.InsertOnSubmit(new StoreProductTagRel()
                        {
                            ProductID = ((StoreProduct)row).ID,
                            StoreProductTag = exist
                        });
                }
                db.SubmitChanges();

                var serializer = new JavaScriptSerializer();
                var r = (StoreProduct)row;
                var chars = serializer.Deserialize<List<List<string>>>((r).JoinedChars);
                if (chars != null)
                {
                    List<string> names = chars.Select(z => z.ElementAt(0).Trim()).ToList();
                    List<string> renames = chars.Where(z => z.ElementAt(1).ToBool()).Select(z => z.ElementAt(7).Trim()).ToList();

                    var cfd =
                        db.StoreCharacterToProducts.Where(
                            x =>
                                x.ProductID == r.ID && !names.Contains(x.StoreCharacterValue.StoreCharacter.Name) &&
                                !renames.Contains(x.StoreCharacterValue.StoreCharacter.Name))
                            .ToList();
                    if (cfd.Any())
                    {
                        db.StoreCharacterToProducts.DeleteAllOnSubmit(cfd);
                    }
                    db.SubmitChanges();
                    foreach (List<string> line in chars)
                    {
                        var tooltip = line.ElementAt(5);

                        var newCharName = line.ElementAt(0).Trim();
                        var newCharValue = line.ElementAt(2).Trim();

                        var cdb =
                            db.StoreCharacterToProducts.FirstOrDefault(
                                x =>
                                    x.ProductID == r.ID &&
                                    x.StoreCharacterValue.StoreCharacter.Name == newCharName);
                        if (cdb != null)
                        {
                            if (newCharValue != cdb.StoreCharacterValue.Value)
                            {
                                //Все значения
                                if (line.ElementAt(3).ToBool())
                                {
                                    var olv =
                                        db.StoreCharacterValues.FirstOrDefault(
                                            x =>
                                                x.Value == newCharValue &&
                                                x.CharacterID == cdb.StoreCharacterValue.CharacterID);

                                    if (olv != null && olv.Value != newCharValue)
                                    {
                                        olv = null;
                                    }

                                    if (olv == null)
                                    {
                                        cdb.StoreCharacterValue.Value = newCharValue;

                                    }
                                    else
                                    {


                                        var lst =
                                            db.StoreCharacterToProducts.Where(
                                                x => x.CharacterValueID == cdb.CharacterValueID).ToList();
                                        foreach (var toProduct in lst)
                                        {
                                            toProduct.StoreCharacterValue = olv;
                                            db.SubmitChanges();
                                        }

                                    }
                                }
                                //Вся папка
                                else if (line.ElementAt(4).ToBool())
                                {
                                    var myCats =
                                        cdb.StoreProduct.StoreProductsToCategories.Select(x => x.CategoryID)
                                            .ToList();

                                    var olv =
                                        db.StoreCharacterValues.FirstOrDefault(
                                            x =>
                                                x.Value == newCharValue &&
                                                x.CharacterID == cdb.StoreCharacterValue.CharacterID/* &&
                                                x.StoreCharacterToProducts.Any(
                                                    z =>
                                                        z.StoreProduct.StoreProductsToCategories.Any(
                                                            c => cats.Contains(c.CategoryID)))*/);

                                    if (olv != null && olv.Value != newCharValue)
                                    {
                                        olv = null;
                                    }

                                    //Записи с таким значением нет в базе данных
                                    if (olv == null)
                                    {
                                        var allCats =
                                            db.StoreCategories.Where(
                                                x =>
                                                    x.StoreProductsToCategories.SelectMany(
                                                        c => c.StoreProduct.StoreCharacterToProducts)
                                                        .Any(v => v.CharacterValueID == cdb.CharacterValueID))
                                                .Select(x => x.ID)
                                                .ToList();

                                        if (!allCats.Except(myCats).Any())
                                        {

                                            cdb.StoreCharacterValue.Value = newCharValue;
                                        }
                                        else
                                        {
                                            var evv =
                                                db.StoreCharacterValues.FirstOrDefault(
                                                    z =>
                                                        z.CharacterID == cdb.StoreCharacterValue.CharacterID &&
                                                        z.Value == newCharValue);

                                            if (evv != null && evv.Value != newCharValue)
                                            {
                                                evv = null;
                                            }

                                            if (evv == null)
                                            {
                                                evv = new StoreCharacterValue()
                                                {
                                                    CharacterID = cdb.StoreCharacterValue.CharacterID,
                                                    Value = newCharValue
                                                };
                                                db.StoreCharacterValues.InsertOnSubmit(evv);
                                                db.SubmitChanges();
                                            }


                                            var charRels =
                                                db.StoreProducts.Where(
                                                    x =>
                                                        x.StoreProductsToCategories.Any(
                                                            z => myCats.Contains(z.CategoryID)))
                                                    .SelectMany(x => x.StoreCharacterToProducts)
                                                    .Where(x => x.CharacterValueID == cdb.CharacterValueID)
                                                    .ToList();

                                            if (charRels.Any())
                                            {


                                                foreach (var rel in charRels)
                                                {
                                                    rel.StoreCharacterValue = evv;
                                                }
                                                db.SubmitChanges();
                                            }

                                        }
                                    }
                                    else
                                    {
                                        var lst =
                                            db.StoreCharacterToProducts.Where(
                                                x =>
                                                    x.CharacterValueID == cdb.CharacterValueID &&
                                                    x.StoreProduct.StoreProductsToCategories.Any(
                                                        z => myCats.Contains(z.CategoryID))).ToList();
                                        foreach (var toProduct in lst)
                                        {
                                            toProduct.StoreCharacterValue = olv;

                                            db.SubmitChanges();
                                        }

                                    }
                                }
                                else
                                {

                                    var evv =
                                        db.StoreCharacterValues.FirstOrDefault(
                                            x =>
                                                x.CharacterID == cdb.StoreCharacterValue.CharacterID &&
                                                x.Value == newCharValue);

                                    if (evv!=null && evv.Value != newCharValue)
                                    {
                                        evv = null;
                                    }

                                    if (evv == null)
                                    {
                                        evv = new StoreCharacterValue()
                                        {
                                            CharacterID = cdb.StoreCharacterValue.CharacterID,
                                            Value = newCharValue
                                        };
                                        db.StoreCharacterValues.InsertOnSubmit(evv);
                                        cdb.StoreCharacterValue = evv;
                                    }
                                    else
                                    {
                                        cdb.CharacterValueID = evv.ID;    
                                    }

                                    
                                    


                                    db.SubmitChanges();
                                }


                            }
                            cdb.StoreCharacterValue.StoreCharacter.Tooltip = tooltip;
                            db.SubmitChanges();
                        }
                        else
                        {


                            if (!line.ElementAt(1).ToBool())
                            {
                                var edb =
                                   db.StoreCharacters.FirstOrDefault(
                                      x => x.Name == newCharName);
                                if (edb == null)
                                {
                                    edb = new StoreCharacter() { Name = newCharName, Tooltip = tooltip };
                                    db.StoreCharacters.InsertOnSubmit(edb);
                                }
                                var evv =
                                    edb.StoreCharacterValues.FirstOrDefault(
                                        z => z.CharacterID == edb.ID && z.Value == newCharValue);

                                if (evv != null && evv.Value != newCharValue)
                                {
                                    evv = null;
                                }

                                if (evv == null)
                                {
                                    evv = new StoreCharacterValue()
                                    {
                                        StoreCharacter = edb,
                                        Value = newCharValue
                                    };
                                    db.StoreCharacterValues.InsertOnSubmit(evv);
                                }
                                cdb = new StoreCharacterToProduct()
                                {
                                    ProductID = r.ID,
                                    StoreCharacterValue = evv
                                };
                                db.StoreCharacterToProducts.InsertOnSubmit(cdb);
                                db.SubmitChanges();

                                if (line.ElementAt(4).ToBool())
                                {
                                    var catsList = db.StoreProductsToCategories.Where(x => x.ProductID == r.ID).Select(x => x.CategoryID).ToList();
                                    var prods =
                                        db.StoreProductsToCategories.Where(
                                            x => catsList.Contains(x.CategoryID) && x.ProductID != r.ID)
                                            .Select(x => x.StoreProduct)
                                            .Where(
                                                x => x.StoreCharacterToProducts.All(z => z.CharacterValueID != evv.ID)).ToList();

                                    foreach (var product in prods)
                                    {
                                        var rel = new StoreCharacterToProduct()
                                        {
                                            StoreProduct = product,
                                            StoreCharacterValue = evv
                                        };
                                        db.StoreCharacterToProducts.InsertOnSubmit(rel);

                                        db.SubmitChanges();
                                    }
                                }

                            }
                            else
                            {
                                cdb = db.StoreCharacterToProducts.FirstOrDefault(
                                    x =>
                                        x.ProductID == r.ID &&
                                        x.StoreCharacterValue.StoreCharacter.Name ==
                                        line.ElementAt(7).Trim());
                                if (cdb != null)
                                {
                                    cdb.StoreCharacterValue.StoreCharacter.Name = newCharName;
                                    cdb.StoreCharacterValue.StoreCharacter.Tooltip = tooltip;
                                }

                            }
                        }
                        db.SubmitChanges();
                    }
                }


                var emptyVals = db.StoreCharacterValues.Where(x => !x.StoreCharacterToProducts.Any());
                if (emptyVals.Any())
                {
                    db.StoreCharacterValues.DeleteAllOnSubmit(emptyVals);
                }

                db.SubmitChanges();


                var emptyChars = db.StoreCharacters.Where(x => !x.StoreCharacterValues.Any());
                if (emptyChars.Any())
                {
                    db.StoreCharacters.DeleteAllOnSubmit(emptyChars);
                }

                db.SubmitChanges();

                AfterSaveRow(row, db);

                CatalogBrowser.Init().ClearAllCaches();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }

        [AuthorizeMaster]
        [HttpGet]
        public JsonResult CatalogTreeHandler(int productId)
        {
            /*var rels = db.StoreProductsToCategories.Where(x => x.ProductID == productId).ToList();

            JsTreeModel model;
            var result = new JsonResult();
            var rootPage = CatalogBrowser.CategoriesList.FirstOrDefault(x => x.Value.ID == 1).Value;

            var root = new JsTreeModel
            {
                data = rootPage.Name,
                attr = new JsTreeAttribute { id = "x" + rootPage.ID, href = "#", uid = rootPage.ID, @class = rels.Any(z => z.CategoryID == rootPage.ID) ? "jstree-checked" : "" },
                children = new List<JsTreeModel>()
            };
            FillModel(ref root, rootPage.ID, rels);
            model = root;

            result.Data = model;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.ContentType = "application/json";
            return result;*/
            var treeModel = new UniversalTreeDataSource()
                {
                    CheckedNodes = db.StoreProductsToCategories.Where(x => x.ProductID == productId).ToList()
                };
            return treeModel.Serialize(SerializationType.Categories);
        }

        [AuthorizeMaster]
        [HttpGet]
        public JsonResult CategoryTreeHandler(int? category)
        {
            /*var rels = db.StoreProductsToCategories.Where(x => x.ProductID == productId).ToList();

            JsTreeModel model;
            var result = new JsonResult();
            var rootPage = CatalogBrowser.CategoriesList.FirstOrDefault(x => x.Value.ID == 1).Value;

            var root = new JsTreeModel
            {
                data = rootPage.Name,
                attr = new JsTreeAttribute { id = "x" + rootPage.ID, href = "#", uid = rootPage.ID, @class = rels.Any(z => z.CategoryID == rootPage.ID) ? "jstree-checked" : "" },
                children = new List<JsTreeModel>()
            };
            FillModel(ref root, rootPage.ID, rels);
            model = root;

            result.Data = model;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.ContentType = "application/json";
            return result;*/

            var nodes = new List<StoreCategory>();
            if (category.HasValue)
            {
                nodes = db.StoreCategories.Where(x => x.ID == category).Where(x => x != null).ToList();
            }

            var treeModel = new UniversalTreeDataSource()
                {
                    CheckedNodes = nodes
                };
            return treeModel.Serialize(SerializationType.Categories);
        }

        /*
                private void FillModel(ref JsTreeModel model, int id, List<StoreProductsToCategory> rels)
                {

                    var pages = CatalogBrowser.CategoriesList.Where(x => x.Value.ParentID == id);
                    foreach (var cmsPage in pages)
                    {
                        var child = new JsTreeModel
                        {
                            data = cmsPage.Value.Name,
                            attr =
                                new JsTreeAttribute
                                {
                                    id = "x" + cmsPage.Value.ID,
                                    href = "#",
                                    uid = cmsPage.Value.ID,
                                    @class = rels.Any(z => z.CategoryID == cmsPage.Value.ID) ? "jstree-checked" : ""
                                },

                        };
                        if (model.children == null)
                            model.children = new List<JsTreeModel>();
                        model.children.Add(child);
                        FillModel(ref child, cmsPage.Value.ID, rels);
                    }
                }
        */
        #endregion

        #region Редактор комментариев к товарам


        [AuthorizeMaster]
        [MenuItem("Комментарии к товарам", 50, 4)]
        public ActionResult CommentProductEditor(string Type, int? Page, int? UID, int? CategoryID, int? ProductID)
        {
            CategoryID = CategoryID ?? 0;
            ProductID = ProductID ?? 0;


            var cats = db.StoreCategories.Where(x => !x.Deleted).OrderBy(x => x.Name).ToList();
            cats.Insert(0, new StoreCategory() { Name = "Все категории" });

            List<StoreProduct> prodList;
            if (CategoryID > 0)
            {
                prodList =
                    db.StoreProductsToCategories.Where(x => x.CategoryID == CategoryID && !x.StoreCategory.Deleted)
                        .OrderBy(x => x.OrderNum)
                        .Select(x => x.StoreProduct)
                        .ToList();
            }
            else
            {
                prodList = db.StoreProducts.Where(x => !x.Deleted).OrderBy(x => x.Name).ToList();
            }
            prodList.Insert(0, new StoreProduct() { Name = "Все продукты" });
            var editedRow = db.Comments.FirstOrDefault(x => x.ID == UID) ??
                            new Comment
                            {
                                UserID = AccessHelper.CurrentUserKey,
                                ID = 0,
                                StoreProductComments =
                                    new EntitySet<StoreProductComment>()
                                    {
                                        new StoreProductComment() {ProductID = ProductID ?? 0}
                                    }
                            };


            var settings = new UniversalEditorSettings
            {
                TableName = "Comments",
                HasDeleteColumn = true,
                CanAddNew = ProductID > 0,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Date",
                                        IsLinkToEdit = true,
                                        HeaderText = "Дата добавления",
                                        Template = "<span style='white-space: nowrap;'>{{0}}</span>"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "CommentText",
                                        IsLinkToEdit = false,
                                        HeaderText = "Комментарий",
                                        Width = 300
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "UserID",
                                        HeaderText = "Автор",
                                        IsLinkToEdit = false,
                                        TextFunction = x => UserProfile.Get((Guid) x).FullName,
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "ID",
                                        HeaderText = "Ссылка на страницу",
                                        IsLinkToEdit = false,
                                        TextFunction = x =>
                                            {
                                                var row = db.Comments.First(z => z.ID == (int) x);
                                                return string.Format("<a target=\"_blank\" href='{0}'>{1}</a>",
                                                                     (row.StoreProductComments.First().StoreProduct ?? new StoreProduct()).FullUrl,
                                                                     (row.StoreProductComments.First().StoreProduct?? new StoreProduct()).Name);
                                            }
                                    }
                            },

                EditedFieldsList =
                    new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Date",
                                        FieldType = UniversalEditorFieldType.Calendar,
                                        HeaderText = "Дата",
                                        DataType = typeof (DateTime),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "CommentText",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Комментарий",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },



                            },
                AutoFilter = true,

            };

            settings.FilterDescription = "Выберите раздел и товар";
            settings.AutoFilter = true;


            settings.Filters = new List<FilterConfiguration>
            {
                new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = CategoryID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Name",
                                    Source = cats
                                },
                            HeaderText = "Категория",
                            IsDropDown = true,
                            QueryKey = "CategoryID",
                            Type = FilterType.Integer,
                            MainFilter = true,
                            SkipInQuery = true
                        },
               /* new FilterConfiguration
                {
                    FilterSource = new UniversalTreeDataSource()
                    {
                        LinkFunction =
                            x =>
                                Request.GenerateURL(new List<KeyValuePair<string, string>>
                                {
                                    new KeyValuePair<string, string>("CategoryID", x.ToString())
                                }, true),
                        Type = SerializationType.Categories,
                        Source =
                            db.StoreCategories.OrderBy(x => x.ParentID).ThenBy(x => x.OrderNum),
                        HasEmptyDef = false,
                        KeyField = "ID",
                        ValueField = "Name",
                    },
                    QueryKey = "CategoryID",
                    Type = FilterType.Integer,
                    MainFilter = true,
                    SkipInQuery = true,
                    MaxHeight = 300,
                    HeaderText = "Категория",
                },
*/

                new FilterConfiguration
                {
                    FilterSource = new UniversalDataSource
                    {
                        DefValue = ProductID,
                        HasEmptyDef = false,
                        KeyField = "ID",
                        ValueField = "Name",
                        Source = prodList
                    },

                    HeaderText = "Выберите товар:",
                    IsDropDown = true,
                    QueryKey = "ProductID",
                    Type = FilterType.Integer,

                }
            };

            settings.ListLinks = new List<EditorLink>()
                {
                    new EditorLink(){Name = "Все комментарии", Link = "/Master/ru/TableEditors/CommentProductEditor"},
                };

            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var list = db.Comments.Where(x => x.StoreProductComments.Any());
            if (ProductID > 0)
            {
                list = list.Where(x => x.StoreProductComments.Any(z => z.ProductID == ProductID));
            }
            else if (CategoryID > 0)
            {
                list =
                    list.Where(
                        x =>
                            x.StoreProductComments.Any(
                                z => z.StoreProduct.StoreProductsToCategories.Any(c => c.CategoryID == CategoryID)));
            }
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<Comment>(list.OrderByDescending(x => x.Date), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };

            data.IsAddingNew = data.EditedRow != null && ((Comment)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "CommentProductEditor";
            data.AddQueryParams = new[] { "CategoryID", "ProductID" };
            data.EditorName = "Комментарии к товарам";
            data.EditorDescription = ""; //"На этой странице можно отредактировать все комментарии к товарам";
            return View("TableEditor", data);
        }

        #endregion


        #region Редактор комментариев


        [AuthorizeMaster]
        public ActionResult CommentEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.Comments.FirstOrDefault(x => x.ID == UID) ??
                            new Comment
                                {
                                    UserID = AccessHelper.CurrentUserKey,
                                    ID = 0,
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "Comments",
                    HasDeleteColumn = true,
                    CanAddNew = false,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Date",
                                        IsLinkToEdit = true,
                                        HeaderText = "Дата добавления",
                                        Template = "<span style='white-space: nowrap;'>{{0}}</span>"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "CommentText",
                                        IsLinkToEdit = false,
                                        HeaderText = "Комментарий",
                                        Width = 300
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "UserID",
                                        HeaderText = "Комментарий добавлен",
                                        IsLinkToEdit = false,
                                        TextFunction = x => UserProfile.Get((Guid) x).FullName,
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "ID",
                                        HeaderText = "Ссылка на страницу",
                                        IsLinkToEdit = false,
                                        TextFunction = x =>
                                            {
                                                var row = db.Comments.First(z => z.ID == (int) x);
                                                return string.Format("<a href='{0}'>{1}</a>",
                                                                     row.CommentedObjectLink,
                                                                     row.CommentedObject);
                                            }
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Date",
                                        FieldType = UniversalEditorFieldType.Calendar,
                                        HeaderText = "Дата",
                                        DataType = typeof (DateTime),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "CommentText",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Комментарий",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<Comment>(db.Comments.OrderByDescending(x => x.Date), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((Comment)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "CommentEditor";
            data.EditorName = "Редактор комментариев";
            return View("TableEditor", data);
        }

        #endregion

        #region Редактор меню


        [AuthorizeMaster]
        /*[MenuItem("Верхнее меню", 20, 2)]*/
        public ActionResult MenuEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.CMSPageMenuCustoms.FirstOrDefault(x => x.ID == UID) ??
                            new CMSPageMenuCustom
                                {
                                    UID = "upper",
                                    ID = 0,
                                    OrderNum = db.CMSPageMenuCustoms.Count() + 1,
                                    Visible = true
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "CMSPageMenuCustoms",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название раздела"
                                        
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        IsLinkToEdit = false,
                                        HeaderText = "Отображать на сайте",
                                        Width = 300
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Description",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Краткое описание",
                                        DataType = typeof (string)

                                    },


                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool)

                                    },


                                new UniversalEditorField
                                    {
                                        FieldName = "URL",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "URL",
                                        DataType = typeof (string)

                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageMenuCustom>(db.CMSPageMenuCustoms.Where(x => x.UID == "upper").OrderBy(x => x.OrderNum), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageMenuCustom)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "MenuEditor";
            data.EditorName = "Редактирование верхнего меню";
            data.EditorDescription = "На этой странице редактируется верхнее меню сайта";
            return View("TableEditor", data);
        }

        #endregion
        #region Редактор нижнего меню


        [AuthorizeMaster]
        /*[MenuItem("Меню в подвале сайта", 25, 2)]*/
        public ActionResult MenuBottomEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.CMSPageMenuCustoms.FirstOrDefault(x => x.ID == UID) ??
                            new CMSPageMenuCustom
                                {
                                    UID = "bottom",
                                    ID = 0,
                                    OrderNum = db.CMSPageMenuCustoms.Count() + 1,
                                    Visible = true
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "CMSPageMenuCustoms",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название раздела"
                                        
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        IsLinkToEdit = false,
                                        HeaderText = "Отображать на сайте",
                                        Width = 300
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "IsHeader",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать как заголовок",
                                        DataType = typeof (bool)

                                    },
/*
                                new UniversalEditorField
                                    {
                                        FieldName = "Description",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Краткое описание",
                                        DataType = typeof (string)

                                    },
*/


                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool)

                                    },


                                new UniversalEditorField
                                    {
                                        FieldName = "URL",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "URL",
                                        DataType = typeof (string)

                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageMenuCustom>(db.CMSPageMenuCustoms.Where(x => x.UID == "bottom").OrderBy(x => x.OrderNum), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageMenuCustom)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "MenuBottomEditor";
            data.EditorName = "Редактирование нижнего меню";
            data.EditorDescription = "На этой странице редактируется нижнее меню сайта";
            return View("TableEditor", data);
        }

        #endregion
        #region Редактор спецпредложений


        [AuthorizeMaster]
        /*[MenuItem("Меню спецпредложений", 21, 2)]*/
        public ActionResult SpecEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.CMSPageMenuCustoms.FirstOrDefault(x => x.ID == UID) ??
                            new CMSPageMenuCustom
                                {
                                    UID = "spec",
                                    ID = 0,
                                    OrderNum = db.CMSPageMenuCustoms.Count() + 1,
                                    Visible = true
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "CMSPageMenuCustoms",
                    HasDeleteColumn = true,
                    CanAddNew = db.CMSPageMenuCustoms.Count(x => x.UID == "spec") < 3,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название объявления"
                                        
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        IsLinkToEdit = false,
                                        HeaderText = "Отображать на сайте",
                                        Width = 300
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название объявления",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Description",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Краткое описание",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },


                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool)

                                    },


                                new UniversalEditorField
                                    {
                                        FieldName = "URL",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "URL",
                                        DataType = typeof (string)

                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Image",
                                        FieldType = UniversalEditorFieldType.DBImageUpload,
                                        HeaderText = "Изображение"
                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageMenuCustom>(db.CMSPageMenuCustoms.Where(x => x.UID == "spec").OrderBy(x => x.OrderNum), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageMenuCustom)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "SpecEditor";
            data.EditorName = "Редактирование меню со спецпредложениями";
            data.EditorDescription = "На этой странице редактируется меню со спецпредложениями, максимальное количество спецпредложений - 3";
            return View("TableEditor", data);
        }

        #endregion
        #region Горячие скидки

        [AuthorizeMaster]
        /*[MenuItem("Горячие скидки", 22, 2)]*/
        public ActionResult HotSpecEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.StoreProductBlocks.FirstOrDefault(x => x.ID == UID) ??
                            new StoreProductBlock
                                {
                                    GroupName = "hot",
                                    ID = 0,
                                    OrderNum = db.CMSPageMenuCustoms.Count() + 1
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "StoreProductBlocks",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "ProductID",
                                        TextFunction = x=> (db.StoreProducts.FirstOrDefault(z=> z.ID == x.ToString().ToInt())?? new StoreProduct()).Name,
                                        IsLinkToEdit = true,
                                        HeaderText = "Название объявления"

                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "ProductID",
                                        FieldType = UniversalEditorFieldType.ProductSelect,
                                        HeaderText = "Список товаров",
                                        DataType = typeof (int),
                                        TreeDataSource = new TreeProductDataSource
                                            {
                                                Branch = editedRow.ID == 0? 0: editedRow.StoreProduct.StoreProductsToCategories.First().CategoryID,
                                                Message = "Выберите товары, которые будут отображаться в блоке 'Горячие новинки'",
                                                GroupName = "hot",
                                                ItemsDataHandler = "/Master/ru/TableEditors/ProductListHandler",
                                                SaveDataHandler = "/Master/ru/TableEditors/SaveProductList",
                                                DataLink =
                                                    "/Master/ru/TableEditors/CatalogTreeHandler?productId=" +
                                                    editedRow.ID,
                                                Name = "CategoryTree"
                                            }

                                    },


                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<StoreProductBlock>(db.StoreProductBlocks.Where(x => x.GroupName == "hot").OrderBy(x => x.OrderNum), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.SaveRow = (row, db1, context) => "";
            data.IsAddingNew = data.EditedRow != null && ((StoreProductBlock)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "HotSpecEditor";
            data.EditorName = "Редактирование блока 'Горячие скидки'";
            data.EditorDescription = "На этой странице редактируется блок 'Горячие скидки'";
            return View("TableEditor", data);
        }
        [AuthorizeMaster]
        /*[MenuItem("Хиты сезона", 23, 2)]*/
        public ActionResult TrendSpecEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.StoreProductBlocks.FirstOrDefault(x => x.ID == UID) ??
                            new StoreProductBlock
                                {
                                    GroupName = "trend",
                                    ID = 0,
                                    OrderNum = db.CMSPageMenuCustoms.Count() + 1
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "StoreProductBlocks",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "ProductID",
                                        TextFunction = x=> (db.StoreProducts.FirstOrDefault(z=> z.ID == x.ToString().ToInt())?? new StoreProduct()).Name,
                                        IsLinkToEdit = true,
                                        HeaderText = "Название товара"

                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "ProductID",
                                        FieldType = UniversalEditorFieldType.ProductSelect,
                                        HeaderText = "Список товаров",
                                        DataType = typeof (int),
                                        TreeDataSource = new TreeProductDataSource
                                            {
                                                Branch = editedRow.ID == 0? 0: editedRow.StoreProduct.StoreProductsToCategories.First().CategoryID,
                                                Message = "Выберите товары, которые будут отображаться в блоке 'Горячие новинки'",
                                                GroupName = "trend",
                                                ItemsDataHandler = "/Master/ru/TableEditors/ProductListHandler",
                                                SaveDataHandler = "/Master/ru/TableEditors/SaveProductList",
                                                DataLink =
                                                    "/Master/ru/TableEditors/CatalogTreeHandler?productId=" +
                                                    editedRow.ID,
                                                Name = "CategoryTree"
                                            }

                                    },


                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<StoreProductBlock>(db.StoreProductBlocks.Where(x => x.GroupName == "trend").OrderBy(x => x.OrderNum), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.SaveRow = (row, db1, context) => "";
            data.IsAddingNew = data.EditedRow != null && ((StoreProductBlock)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "TrendSpecEditor";
            data.EditorName = "Редактирование блока 'Хиты сезона'";
            data.EditorDescription = "На этой странице редактируется блок 'Хиты сезона'";
            return View("TableEditor", data);
        }

        [HttpGet]
        [AuthorizeMaster]
        public JsonResult ProductListHandler(string group, int cat)
        {
            var db = new DB();
            var prods =
                db.StoreProducts.Where(x => x.StoreProductsToCategories.Any(z => z.CategoryID == cat) && !x.Deleted)
                  .OrderBy(x => x.Name);
            var gr = db.StoreProductBlocks.Where(x => x.GroupName == group);

            var list = prods.Select(
                x =>
                new TreeProductListItem()
                    {
                        Checked = gr.Any(z => z.ProductID == x.ID) ? "checked" : "",
                        ID = x.ID,
                        Name = x.Name
                    });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveProductList(string group, int item, int status)
        {
            var db = new DB();
            var p = db.StoreProductBlocks.FirstOrDefault(x => x.ProductID == item && x.GroupName == group);
            if (p == null && status == 1)
            {
                db.StoreProductBlocks.InsertOnSubmit(new StoreProductBlock()
                    {
                        GroupName = group,
                        ProductID = item,
                        OrderNum = db.StoreProductBlocks.Count() + 1
                    });
            }
            else if (p != null && status == 0)
            {
                db.StoreProductBlocks.DeleteOnSubmit(p);
            }
            db.SubmitChanges();
            return new ContentResult();
        }

        #endregion

        #region Редактор Настроек Импорта


        [AuthorizeMaster]
        [MenuItem("Настройки XLS импорта", ID = 60, ParentID = 4)]
        public ActionResult ImportEditor(string Type, int? Page, int? UID)
        {



            var editedRow = db.StoreImporters.FirstOrDefault(x => x.ID == UID) ??
                            new StoreImporter()
                                {
                                    ID = 0,
                                    ColumnName = StoreImporter.RequiredColumns.First().Key
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "StoreImporter",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "ColumnName",
                                        IsLinkToEdit = true,
                                        TextFunction = x => StoreImporter.RequiredColumns.First(z => z.Key == x.ToString()).Name,
                                        HeaderText = "Название столбца"
                                    },
                                    new UniversalListField()
                                        {
                                            FieldName = "ColumnNum",
                                            HeaderText = "Номер"
                                        },
                                    new UniversalListField()
                                        {
                                            FieldName = "Header",
                                            HeaderText = "Характеристика"
                                        },
                                        new UniversalListField()
                                        {
                                            HeaderText = "Исп. в фильтре",
                                            FieldName = "ShowInFilter"
                                            
                                        },
                                        new UniversalListField()
                                        {
                                            HeaderText = "Исп. в табл. товаров",
                                            FieldName = "ShowInList"
                                            
                                        }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "ColumnName",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Название столбца",
                                        DataType = typeof (string),
                                        InnerListDataSource = new UniversalDataSource()
                                            {
                                                DefValue = editedRow.ColumnName,
                                                HasEmptyDef = false,
                                                KeyField = "Key",
                                                ValueField = "Name",
                                                Source = StoreImporter.RequiredColumns,
                                            },
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ColumnNum",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Номер колонки в XLS файле",
                                        DataType = typeof (int),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "RowNum",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Номер первой строки с данными в XLS файле",
                                        DataType = typeof (int),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Header",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название характеристики (требуется для колонок с характеристиками)",
                                        DataType = typeof (string)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ShowInFilter",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Нужно ли отображать эту характеристику в фильтрах каталога?",
                                        DataType = typeof (bool)
                                    },                                new UniversalEditorField
                                    {
                                        FieldName = "ShowInList",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Нужно ли отображать таблице товаров?",
                                        DataType = typeof (bool)
                                    },                                new UniversalEditorField
                                    {
                                        FieldName = "Priority",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Порядковый номер характеристики для отображения в таблице",
                                        DataType = typeof (int)
                                    }

                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<StoreImporter>(db.StoreImporters.OrderBy(x => x.ID), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((StoreImporter)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "ImportEditor";
            data.EditorDescription = ""; //"Здесь вы можете изменить настройки XLS импорта";
            data.EditorName = "Настройка импорта";
            return View("TableEditor", data);
        }

        #endregion


        #region Редактор Счетчиков


        [AuthorizeMaster]
        [MenuItem("Счетчики", 981, 99, Icon = "clock")]
        public ActionResult CountersEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.SiteCounters.FirstOrDefault(x => x.ID == UID) ??
                            new SiteCounter()
                                {
                                    ID = 0,
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "SiteCounters",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название"
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },     new UniversalEditorField
                                    {
                                        FieldName = "Code",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Код счетчика",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    }

                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<SiteCounter>(db.SiteCounters.OrderBy(x => x.ID), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((SiteCounter)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "CountersEditor";
            data.EditorDescription = ""; /*"Здесь вы можете добавить счетчик для статистики посещаемости сайта";*/
            data.EditorName = "Счетчики";
            return View("TableEditor", data);
        }

        #endregion


        #region Редактор Объектов


        [AuthorizeMaster]
        public ActionResult ObjectEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.MapObjects.FirstOrDefault(x => x.ID == UID) ??
                            new MapObject
                                {
                                    ID = 0,
                                };


            var settings = new UniversalEditorSettings
                {
                    TableName = "MapObjects",
                    HasDeleteColumn = true,
                    CanAddNew = false,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название",
                                        Width = 150
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "CreateDate",
                                        HeaderText = "Дата добавления"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "CreatorID",
                                        HeaderText = "Автор",
                                        IsLinkToEdit = false,
                                        TextFunction = x => UserProfile.Get((Guid) x).FullName,
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "ID",
                                        HeaderText = "Тип объекта",
                                        TextFunction = x =>
                                            {
                                                var row = db.MapObjects.First(z => z.ID == (int) x);
                                                return row.MapObjectType.TypeName;
                                            }
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },     new UniversalEditorField
                                    {
                                        FieldName = "CreateDate",
                                        FieldType = UniversalEditorFieldType.Calendar,
                                        HeaderText = "Дата добавления",
                                        DataType = typeof (DateTime),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Address",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Адрес",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Description",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Описание",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<MapObject>(db.MapObjects.OrderByDescending(x => x.CreateDate), Page ?? 0, 50, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((MapObject)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "ObjectEditor";
            data.EditorName = "Редактор объектов";
            return View("TableEditor", data);
        }

        #endregion

        #region Редактор типов объектов на карте


        [AuthorizeMaster]
        public ActionResult MapObjectTypesEditor(string Type, int? Page, int? UID)
        {

            var editedRow = db.MapObjectTypes.FirstOrDefault(x => x.ID == UID) ??
                            new MapObjectType
                                {
                                    Icon = "",
                                    TypeName = "",
                                    OrderNum = db.MapObjectTypes.Count() + 1
                                };

            var settings = new UniversalEditorSettings
                {
                    TableName = "MapObjectTypes",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "TypeName",
                                        IsLinkToEdit = true,
                                        HeaderText = "Тип объекта"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Icon",
                                        IsLinkToEdit = false,
                                        HeaderText = "CSS класс",
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        HeaderText = "Порядковый номер",
                                        IsOrderColumn = true
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "TypeName",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Тип объекта",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Icon",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "CSS класс",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<MapObjectType>(db.MapObjectTypes.OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((MapObjectType)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "MapObjectTypesEditor";
            data.EditorName = "Редактор типов объектов на карте";
            return View("TableEditor", data);
        }

        #endregion

        #region Редактор городов


        [AuthorizeMaster]
        public ActionResult MapSelect(string Type, int? Page, int? UID)
        {

            var editedRow = db.MapSelects.FirstOrDefault(x => x.ID == UID) ??
                            new MapSelect
                                {
                                    Visible = true,
                                    OrderNum = db.MapSelects.Count() + 1
                                };

            var settings = new UniversalEditorSettings
                {
                    TableName = "MapSelects",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Name",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название города"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        IsLinkToEdit = false,
                                        HeaderText = "Отображать на сайте",
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        HeaderText = "Порядковый номер",
                                        IsOrderColumn = true
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "Name",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название города",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Lat",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Широта",
                                        DataType = typeof (decimal),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Lng",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Долгота",
                                        DataType = typeof (decimal),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Zoom",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Приближение",
                                        DataType = typeof (int),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}

                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool)
                                    },



                            },
                    AutoFilter = false,

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<MapSelect>(db.MapSelects.OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((MapSelect)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "MapSelect";
            data.EditorName = "Редактор списка городов";
            return View("TableEditor", data);
        }

        #endregion

        #region Текстовый редактор
        [AuthorizeMaster]
        /*[MenuItem("Текстовые блоки", 551, Icon = "book_alt", ParentID = 5)]*/
        public ActionResult TextEditor(string Type, int? Page, int? UID, int? LangID, int? CMSPageID, int? ViewID)
        {
            if (!LangID.HasValue)
            {
                LangID = (db.Languages.FirstOrDefault(x => x.ShortName == "ru") ?? db.Languages.First()).ID;
            }
            if (!CMSPageID.HasValue)
            {
                CMSPageID =
                    (db.CMSPages.FirstOrDefault(
                        x =>
                        x.PageType.CMSPageCells.Any(
                            c => c.CMSPageCellViews.Any(v => v.Action == "Index" && v.Controller == "TextPage"))) ??
                     new CMSPage { ID = 0, LastMod = DateTime.Now, }).ID;

            }

            var views =
                db.CMSPageCellViews.Where(x => x.Action == "Index" && x.Controller == "TextPage")
                  .Select(x => x.CMSPageCell)
                  .Intersect(db.CMSPages.Where(x => x.ID == CMSPageID).SelectMany(x => x.PageType.CMSPageCells))
                  .ToList()
                  .SelectMany(x => x.CMSPageCellViews.Where(c => c.Action == "Index" && c.Controller == "TextPage"))
                  .GroupBy(x => x.CMSPageCell).ToList();



            foreach (var view in views)
            {
                if (view.Count() > 1)
                {
                    for (int i = 1; i <= view.Count(); i++)
                    {
                        view.ElementAt(i - 1).Description += " - блок №" + i;
                    }
                }

            }

            var blocks = views.SelectMany(x => x).ToList();

            if (!ViewID.HasValue)
            {
                ViewID = (blocks.FirstOrDefault() ?? new CMSPageCellView()).ID;/*
                    (db.CMSPageCellViews.Where(x => x.Action == "Index" && x.Controller == "TextPage")
                       .Select(x => x.CMSPageCell)
                       .Intersect(db.CMSPages.Where(x => x.ID == CMSPageID).SelectMany(x => x.PageType.CMSPageCells))
                       .SelectMany(x => x.CMSPageCellViews.Where(x=> x.))
                       .FirstOrDefault() ?? new CMSPageCellView()).ID;*/
            }


            if (!UID.HasValue)
            {
                var target = db.CMSPageTextDatas.FirstOrDefault(x => x.ViewID == ViewID && x.CMSPageID == CMSPageID);
                if (target == null)
                {
                    target = new CMSPageTextData()
                    {
                        LangID = LangID.Value,
                        CMSPageID = CMSPageID.Value,
                        ViewID = ViewID.Value,
                        Text = "",
                        Visible = true
                    };
                    db.CMSPageTextDatas.InsertOnSubmit(target);
                    db.SubmitChanges();
                }
                Response.Redirect(string.Format("/Master/ru/TableEditors/TextEditor?CMSPageID={0}&ViewID={1}&Type=Edit&UID={2}",
                    CMSPageID.Value, ViewID.Value, target.ID));
            }


            var editedRow = (db.CMSPageTextDatas.FirstOrDefault(x => x.ID == UID) ??
                             new CMSPageTextData
                             {
                                 LangID = LangID.Value,
                                 CMSPageID = CMSPageID.Value,
                                 ViewID = ViewID.Value,
                                 Text = "",
                                 Visible = true
                             });




            var settings = new UniversalEditorSettings
            {
                TableName = "CMSPageTextDatas",
                HasDeleteColumn = true,
                CanAddNew = true,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Text",
                                        IsLinkToEdit = true,
                                        HeaderText = "Текст",
                                        TextFunction = x =>
                                            {
                                                var cleared = x.ToString().ClearHTML();
                                                if (cleared.Length > 100)
                                                    cleared = cleared.Substring(0, 100) + "...";
                                                return cleared;
                                            }
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        HeaderText = "Статус",
                                        TextFunction = x => (bool) x ? "Отображается" : "Неактивен"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                EditedFieldsList =
                    new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "LangID",
                                        Hidden = true,
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = AccessHelper.CurrentLang.ID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Name",
                                                Source = db.Languages.Where(x => x.Enabled),

                                            },
                                        HeaderText = "Язык",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "CMSPageID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = CMSPageID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "PageName",
                                                Source =
                                                    db.CMSPages.Where(
                                                        x =>
                                                        x.PageType.CMSPageCells.Any(
                                                            c =>
                                                            c.CMSPageCellViews.Any(
                                                                v => v.Action == "Index" && v.Controller == "TextPage")))
                                                      .ToList()
                                                      .Select(x => x.LoadLangValues()),

                                            },
                                        HeaderText = "Страница",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ViewID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = ViewID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Description",
                                                Source = blocks,

                                            },
                                        HeaderText = "Контейнер",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Text",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Текст",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    }
                            }

            };



            settings.FilterDescription = "Выберите страницу и контейнер";
            settings.AutoFilter = true;
            settings.Filters = new List<FilterConfiguration>
                {
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = CMSPageID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "PageName",
                                    Source =


                                        db.CMSPages.Where(
                                            x => 
                                            x.PageType.CMSPageCells.Any(
                                                c =>
                                                c.CMSPageCellViews.Any(
                                                    v => v.Action == "Index" && v.Controller == "TextPage")))
                                          .ToList()
                                          .Select(x => x.LoadLangValues()),



                                },
                            HeaderText = "Страница",
                            IsDropDown = true,
                            QueryKey = "CMSPageID",
                            Type = FilterType.Integer,
                            MainFilter = true
                        },
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = ViewID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Description",
                                    Source = blocks
                                },

                            HeaderText = "Контейнер",
                            IsDropDown = true,
                            QueryKey = "ViewID",
                            Type = FilterType.Integer,

                        }
                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageTextData>(
                              db.CMSPageTextDatas.Where(
                                  x => x.ViewID == ViewID && x.LangID == LangID && x.CMSPageID == CMSPageID)
                                .OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.AddQueryParams = new[] { "LangID", "CMSPageID", "ViewID" };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageTextData)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "TextEditor";
            data.SaveRow = SaveTextData;
            data.EditorName = "Редактирование текстовых блоков";
            data.EditorDescription = "На этой странице можно отредактировать все текстовые блоки, содержащиеся на сайте";

            data.PreviewData = new PreviewData() { Type = 0, UID = CMSPageID.Value };
            return View("TableEditor", data);

        }

        private string SaveTextData(object row, DB db, HttpContextBase context)
        {
            var r = (CMSPageTextData)row;
            try
            {
                if (r == null) return "Объект не найден";
                if (r.ID == 0)
                {
                    r.OrderNum = db.CMSPageTextDatas.Count() + 1;
                    db.CMSPageTextDatas.InsertOnSubmit(r);
                }
                else
                {
                    object entry = db.CMSPageTextDatas.FirstOrDefault(x => x.ID == r.ID);
                    if (entry != null)
                    {
                        r.Detach();
                        entry.LoadPossibleProperties(r, new string[] { "ID" });
                        db.Refresh(RefreshMode.KeepChanges, entry);
                    }
                }
                db.SubmitChanges();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        #region Редактор вспл. окон
        [AuthorizeMaster]
        /*[MenuItem("Всплывающие окна", 553, Icon = "info", ParentID = 5)]*/
        public ActionResult PopupEditor(string Type, int? Page, int? UID, int? LangID, int? CMSPageID, int? ViewID)
        {
            if (!LangID.HasValue)
            {
                LangID = (db.Languages.FirstOrDefault(x => x.ShortName == "ru") ?? db.Languages.First()).ID;
            }
            if (!CMSPageID.HasValue)
            {
                CMSPageID =
                    (db.CMSPages.FirstOrDefault(
                        x =>
                        x.PageType.CMSPageCells.Any(
                            c => c.CMSPageCellViews.Any(v => v.Action == "Popup" && v.Controller == "TextPage"))) ??
                     new CMSPage { ID = 0, LastMod = DateTime.Now, }).ID;

            }

            var views =
                db.CMSPageCellViews.Where(x => x.Action == "Popup" && x.Controller == "TextPage")
                  .Select(x => x.CMSPageCell)
                  .Intersect(db.CMSPages.Where(x => x.ID == CMSPageID).SelectMany(x => x.PageType.CMSPageCells))
                  .ToList()
                  .SelectMany(x => x.CMSPageCellViews.Where(c => c.Action == "Popup" && c.Controller == "TextPage"))
                  .GroupBy(x => x.CMSPageCell).ToList();


            foreach (var view in views)
            {
                if (view.Count() > 1)
                {
                    for (int i = 1; i <= view.Count(); i++)
                    {
                        view.ElementAt(i - 1).Description += " - блок №" + i;
                    }
                }

            }

            var blocks = views.SelectMany(x => x).ToList();

            if (!ViewID.HasValue)
            {
                ViewID = (blocks.FirstOrDefault() ?? new CMSPageCellView()).ID;/*
                    (db.CMSPageCellViews.Where(x => x.Action == "Index" && x.Controller == "TextPage")
                       .Select(x => x.CMSPageCell)
                       .Intersect(db.CMSPages.Where(x => x.ID == CMSPageID).SelectMany(x => x.PageType.CMSPageCells))
                       .SelectMany(x => x.CMSPageCellViews.Where(x=> x.))
                       .FirstOrDefault() ?? new CMSPageCellView()).ID;*/
            }


            var editedRow = (db.CMSPagePopupDatas.FirstOrDefault(x => x.ID == UID) ??
                             new CMSPagePopupData
                             {
                                 LangID = LangID.Value,
                                 CMSPageID = CMSPageID.Value,
                                 ViewID = ViewID.Value,
                                 Text = "",
                                 Visible = true
                             });





            var settings = new UniversalEditorSettings
            {
                TableName = "CMSPagePopupDatas",
                HasDeleteColumn = true,
                CanAddNew = true,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Text",
                                        IsLinkToEdit = true,
                                        HeaderText = "Текст",
                                        TextFunction = x =>
                                            {
                                                var cleared = x.ToString().ClearHTML();
                                                if (cleared.Length > 100)
                                                    cleared = cleared.Substring(0, 100) + "...";
                                                return cleared;
                                            }
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        HeaderText = "Статус",
                                        TextFunction = x => (bool) x ? "Отображается" : "Неактивен"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                EditedFieldsList =
                    new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "LangID",
                                        Hidden = true,
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = AccessHelper.CurrentLang.ID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Name",
                                                Source = db.Languages.Where(x => x.Enabled),

                                            },
                                        HeaderText = "Язык",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "CMSPageID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = CMSPageID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "PageName",
                                                Source =
                                                    db.CMSPages.Where(
                                                        x =>
                                                        x.PageType.CMSPageCells.Any(
                                                            c =>
                                                            c.CMSPageCellViews.Any(
                                                                v => v.Action == "Popup" && v.Controller == "TextPage")))
                                                      .ToList()
                                                      .Select(x => x.LoadLangValues()),

                                            },
                                        HeaderText = "Страница",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ViewID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = ViewID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Description",
                                                Source = blocks,

                                            },
                                        HeaderText = "Контейнер",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Text",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Текст",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    }
                            }

            };



            settings.FilterDescription = "Выберите страницу и контейнер";
            settings.AutoFilter = true;
            settings.Filters = new List<FilterConfiguration>
                {
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = CMSPageID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "PageName",
                                    Source =


                                        db.CMSPages.Where(
                                            x => 
                                            x.PageType.CMSPageCells.Any(
                                                c =>
                                                c.CMSPageCellViews.Any(
                                                    v => v.Action == "Popup" && v.Controller == "TextPage")))
                                          .ToList()
                                          .Select(x => x.LoadLangValues()),



                                },
                            HeaderText = "Страница",
                            IsDropDown = true,
                            QueryKey = "CMSPageID",
                            Type = FilterType.Integer,
                            MainFilter = true
                        },
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = ViewID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Description",
                                    Source = blocks
                                },

                            HeaderText = "Контейнер",
                            IsDropDown = true,
                            QueryKey = "ViewID",
                            Type = FilterType.Integer,

                        }
                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPagePopupData>(
                              db.CMSPagePopupDatas.Where(
                                  x => x.ViewID == ViewID && x.LangID == LangID && x.CMSPageID == CMSPageID)
                                .OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.AddQueryParams = new[] { "CMSPageID", "LangID", "ViewID" };
            data.IsAddingNew = data.EditedRow != null && ((CMSPagePopupData)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "PopupEditor";
            data.SaveRow = SavePopupData;
            data.EditorName = "Редактирование текстовых блоков";
            data.EditorDescription = "На этой странице можно отредактировать все всплывающие текстовые блоки, содержащиеся на сайте";

            data.PreviewData = new PreviewData() { Type = 0, UID = CMSPageID.Value };
            return View("TableEditor", data);

        }

        private string SavePopupData(object row, DB db, HttpContextBase context)
        {
            var r = (CMSPagePopupData)row;
            try
            {
                if (r == null) return "Объект не найден";
                if (r.ID == 0)
                {
                    r.OrderNum = db.CMSPagePopupDatas.Count() + 1;
                    db.CMSPagePopupDatas.InsertOnSubmit(r);
                }
                else
                {
                    object entry = db.CMSPagePopupDatas.FirstOrDefault(x => x.ID == r.ID);
                    if (entry != null)
                    {
                        r.Detach();
                        entry.LoadPossibleProperties(r, new string[] { "ID" });
                        db.Refresh(RefreshMode.KeepChanges, entry);
                    }
                }
                db.SubmitChanges();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        #region Слайдер
        [AuthorizeMaster]
        /*[MenuItem("Слайдеры", 555, Icon = "camera", ParentID = 5)]*/
        public ActionResult Slider(string Type, int? Page, int? UID, int? LangID, int? CMSPageID, int? ViewID, int? CategoryID, int? ProductID)
        {
            if (!LangID.HasValue)
            {
                LangID = (db.Languages.FirstOrDefault(x => x.ShortName == "ru") ?? db.Languages.First()).ID;
            }

            var editedRow = (db.CMSPageSliders.FirstOrDefault(x => x.ID == (UID ?? 0)) ??
                             new CMSPageSlider
                                 {
                                     LangID = LangID.Value,
                                     CMSPageID = !CMSPageID.HasValue || CMSPageID.Value == 0 ? (int?)null : CMSPageID.Value,
                                     ViewID = !ViewID.HasValue || ViewID.Value == 0 ? (int?)null : ViewID.Value,
                                     CategoryID = !CategoryID.HasValue || CategoryID.Value == 0 ? (int?)null : CategoryID.Value,
                                     ProductID = !ProductID.HasValue || ProductID.Value == 0 ? (int?)null : ProductID.Value,
                                     Text = "",
                                     Name = "",
                                     Alt = "",
                                     Link = "",
                                     Visible = true,
                                     OrderNum = db.CMSPageSliders.Count() + 1
                                 });





            var settings = new UniversalEditorSettings
            {

                TableName = "CMSPageSliders",
                HasDeleteColumn = true,
                CanAddNew = true,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                    {
                        new UniversalListField
                        {
                            FieldName = "Name",
                            IsLinkToEdit = true,
                            HeaderText = "Название"

                        },
                        new UniversalListField
                        {
                            FieldName = "Visible",
                            HeaderText = "Статус",
                            TextFunction = x => (bool) x ? "Отображается" : "Неактивен"
                        },
                        new UniversalListField
                        {
                            FieldName = "OrderNum",
                            IsLinkToEdit = false,
                            IsOrderColumn = true,
                            HeaderText = "Порядок"
                        }
                    },

                EditedFieldsList =
                    new List<UniversalEditorField>
                    {
                        new UniversalEditorField
                        {
                            FieldName = "LangID",
                            Hidden = true,
                            FieldType = UniversalEditorFieldType.DropDown,
                            InnerListDataSource = new UniversalDataSource
                            {
                                DefValue = AccessHelper.CurrentLang.ID,
                                HasEmptyDef = false,
                                KeyField = "ID",
                                ValueField = "Name",
                                Source = db.Languages.Where(x => x.Enabled),

                            },
                            HeaderText = "Язык",
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },
                        new UniversalEditorField
                        {

                            FieldName = "CMSPageID",
                            Hidden = true,

                            HeaderText = "Страница",
                        },
                        new UniversalEditorField
                        {
                            FieldName = "ViewID",
                            Hidden = true,

                            HeaderText = "Контейнер",
                        },
                        new UniversalEditorField
                        {
                            FieldName = "CategoryID",
                            Hidden = true,

                            HeaderText = "Категория",
                        },
                        new UniversalEditorField
                        {
                            FieldName = "ProductID",
                            Hidden = true,

                            HeaderText = "Товар",
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Visible",
                            FieldType = UniversalEditorFieldType.CheckBox,
                            HeaderText = "Отображать на сайте",
                            DataType = typeof (bool),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Name",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Название",
                            DataType = typeof (string),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },
                        new UniversalEditorField
                        {
                            FieldName = "Text",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Описание",
                            DataType = typeof (string),
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Link",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Ссылка",
                            /*Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}*/
                        },

                        new UniversalEditorField
                        {
                            FieldName = "Img",
                            FieldType = UniversalEditorFieldType.DBImageUpload,
                            HeaderText = "Изображение",
                            Modificators =
                                new List<IUniversalFieldModificator> {new RequiredModificator()}
                        },
                        new UniversalEditorField
                        {
                            FieldName = "Alt",
                            FieldType = UniversalEditorFieldType.TextBox,
                            HeaderText = "Alt"
                        },



                    }

            };




            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);

            IQueryable<CMSPageSlider> src = null;
            if (ViewID.HasValue && CMSPageID.HasValue && ViewID.Value > 0 && CMSPageID.Value > 0)
            {
                src = db.CMSPageSliders.Where(
                    x => x.ViewID == ViewID && x.LangID == LangID && x.CMSPageID == CMSPageID)
                    .OrderBy(x => x.OrderNum);
            }
            else if (CategoryID.HasValue)
            {
                src = db.CMSPageSliders.Where(
                    x => x.CategoryID == CategoryID && x.LangID == LangID)
                    .OrderBy(x => x.OrderNum);

            }
            else if (ProductID.HasValue)
            {
                src = db.CMSPageSliders.Where(
                    x => x.ProductID == ProductID && x.LangID == LangID)
                    .OrderBy(x => x.OrderNum);

            }

            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageSlider>(
                              src, Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.AddQueryParams = new[] { "LangID", "CMSPageID", "ViewID", "CategoryID", "ProductID" };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageSlider)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "Slider";
            data.EditorName = "Редактирование слайдера";
            data.EditorDescription = "";
            data.ShowInPopup = true;
            //data.IsPartial = true;
            //return PartialView("TableEditorPartial", data);
            return View("TableEditor", data);
        }

        #endregion

        #region Файлы подтверждения доступа

        [HttpGet, AuthorizeMaster]
        [MenuItem("Доступ", 991, 99, Icon = "keys")]
        public ActionResult AuthFiles(string Type, int? Page, int? UID)
        {

            var editedRow = (db.CMSAuthFiles.FirstOrDefault(x => x.ID == UID) ??
                             new CMSAuthFile()
                             {
                                 FileName = "",
                                 CreateDate = DateTime.Now,

                             });


            var settings = new UniversalEditorSettings
            {
                TableName = "CMSAuthFiles",
                HasDeleteColumn = true,
                CanAddNew = true,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "FileName",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название",
                                        

                                    },

                                new UniversalListField
                                    {
                                        FieldName = "CreateDate",
                                        HeaderText = "Дата загрузки",
                                    },
                                
                            },

                EditedFieldsList =
                    new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "FileName",
                                        DataType = typeof(string),
                                        AdditionalData = "/Temp/Universal/",
                                        AdditionalTypeFlag = false,

                                        FieldType = UniversalEditorFieldType.FileImageUpload,
                                        HeaderText = "Выберите файл",
                                        
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                           
                                
                            }

            };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);

            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSAuthFile>(
                              db.CMSAuthFiles.AsQueryable(), Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((CMSAuthFile)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "AuthFiles";
            data.AfterSaveRow = (row, db1, context) =>
            {
                try
                {
                    var file = (CMSAuthFile)row;
                    var source = System.Web.HttpContext.Current.Server.MapPath(file.FileName);
                    var target = System.Web.HttpContext.Current.Server.MapPath("/" + Path.GetFileName(file.FileName));
                    System.IO.File.Move(source, target);

                    var dbr = db1.CMSAuthFiles.First(x => x.ID == file.ID);
                    dbr.FileName = "/" + Path.GetFileName(file.FileName);
                    db1.SubmitChanges();
                    return "";
                }
                catch (Exception xxx)
                {
                    return xxx.Message + "<br>" + xxx.StackTrace;
                }

            };
            data.BeforeDelFunc = (row, db1) =>
            {
                var file = (CMSAuthFile)row;
                var path = System.Web.HttpContext.Current.Server.MapPath(file.FileName);
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
            };

            data.EditorName = "Доступ";

            return View("TableEditor", data);


        }
        #endregion

        #region Редактор изображений в каталоге
        [AuthorizeMaster]
        [MenuItem("Загрузка фото", 43, 4)]
        public ActionResult ImageGallery(string Type, int? Page, int? UID, int? CategoryID, int? ProductID)
        {
            CategoryID = CategoryID ?? 0;
            ProductID = ProductID ?? 0;


            var cats = db.StoreCategories.Where(x => !x.Deleted).OrderBy(x => x.Name).ToList();
            cats.Insert(0, new StoreCategory() { Name = "Все категории" });

            List<StoreProduct> prodList;
            if (CategoryID > 0)
            {
                prodList =
                    db.StoreProductsToCategories.Where(x => x.CategoryID == CategoryID)
                        .OrderBy(x => x.OrderNum)
                        .Select(x => x.StoreProduct)
                        .ToList();
            }
            else
            {
                prodList = db.StoreProducts.Where(x => !x.Deleted).OrderBy(x => x.Name).ToList();
            }
            prodList.Insert(0, new StoreProduct() { Name = "Все продукты" });



            var editedRow = (db.StoreImages.FirstOrDefault(x => x.ID == UID) ??
                             new StoreImage()
                             {
                                 ProductID = ProductID.Value,
                                 Enabled = true,
                                 OrderNum = db.StoreImages.Count() + 1
                             });





            var settings = new UniversalEditorSettings
                {
                    TableName = "StoreImages",
                    HasDeleteColumn = true,
                    CanAddNew = ProductID > 0,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Description",
                                        IsLinkToEdit = true,
                                        HeaderText = "Название",
                                        ImageField = "UrlPath"

                                    },

                                new UniversalListField
                                    {
                                        FieldName = "ProductID",
                                        HeaderText = "Товар",
                                        TextFunction = x => (db.StoreProducts.FirstOrDefault(z=> z.ID == (int)x) ?? new StoreProduct()).Name
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Enabled",
                                        HeaderText = "Статус",
                                        TextFunction = x => (bool) x ? "Отображается" : "Неактивно"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "ProductID",
                                        DataType = typeof(int),
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = editedRow.ProductID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Name",
                                                Source = db.StoreProducts.Where(x=> !x.Deleted).OrderBy(x=> x.Name)
                                            },
                                        HeaderText = "Товар в каталоге",
                                        
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                           
                                new UniversalEditorField
                                    {
                                        FieldName = "Enabled",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать на сайте",
                                        DataType = typeof (bool),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "Description",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Название",
                                        DataType = typeof (string),
                                        /*Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}*/
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "UrlPath",
                                        FieldType = UniversalEditorFieldType.FileImageUpload,
                                        AdditionalTypeFlag = true,
                                        HeaderText = "Изображение",
                                        AdditionalData = "/content/Catalog/",
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    }
                            }

                };



            settings.FilterDescription = "Выберите раздел и товар";
            settings.AutoFilter = true;
            settings.Filters = new List<FilterConfiguration>
                {
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = CategoryID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Name",
                                    Source =
                                    cats
                                },
                            HeaderText = "Категория",
                            IsDropDown = true,
                            QueryKey = "CategoryID",
                            Type = FilterType.Integer,
                            MainFilter = true,
                            SkipInQuery = true
                        },
                   /* new FilterConfiguration
                        {
                            FilterSource = new UniversalTreeDataSource()
                                {
                                    LinkFunction =
                                        x =>
                                        Request.GenerateURL(new List<KeyValuePair<string, string>>
                                            {
                                                new KeyValuePair<string, string>("CategoryID", x.ToString())
                                            }, true),
                                    Type = SerializationType.Categories
                                    },
                                    QueryKey = "CategoryID",
                                    Type = FilterType.Integer,
                                    MainFilter = true,
                                    SkipInQuery = true,
                                    MaxHeight = 300
                        },
*/

                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = ProductID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Name",
                                    Source = prodList
                                },

                            HeaderText = "Выберите товар:",
                            IsDropDown = true,
                            QueryKey = "ProductID",
                            Type = FilterType.Integer,
                            SkipInQuery = true

                        }
                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);

            IQueryable<StoreImage> lst;
            if (ProductID > 0)
            {
                lst = db.StoreImages.Where(x => x.ProductID == ProductID).OrderBy(x => x.OrderNum);
            }
            else if (CategoryID > 0)
            {
                lst =
                    db.StoreImages.Where(
                        x => x.StoreProduct.StoreProductsToCategories.Any(z => z.CategoryID == CategoryID))
                        .OrderBy(x => x.OrderNum);
            }
            else
            {
                lst = db.StoreImages.OrderBy(x => x.OrderNum);
            }
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<StoreImage>(
                              lst, Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.AddQueryParams = new[] { "CategoryID", "ProductID" };
            data.IsAddingNew = data.EditedRow != null && ((StoreImage)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "ImageGallery";

            /* var list = new List<EditorLink>()
                 {
                     new EditorLink(){Name = "Перейти к списку товаров раздела", Link = "/Master/ru/TableEditors/Products?ParentID="+CategoryID}
                    
                 };
             if (ProductID > 0)
             {
                 list.Add(new EditorLink()
                     {
                         Name = "Перейти к редактированию товара",
                         Link =
                             "/Master/ru/TableEditors/Products?Type=Edit&UID={0}&ParentID={1}&Page={2}".FormatWith(
                                 ProductID, CategoryID, "0")
                     });
             }*/
            /*   if (CategoryID > 0)
               {
                   data.Settings.ListLinks = list;
               }*/
            var cp = db.StoreProducts.FirstOrDefault(x => x.ID == ProductID);

            data.EditorName = "Загрузка изображений для товара";
            if (cp != null)
                data.EditorName += " '{0}'".FormatWith(cp.Name);

            data.EditorDescription = ""; // "На этой странице можно загрузить фото для любого товара";

            data.PreviewData = new PreviewData() { Type = 2, UID = ProductID.Value };

            return View("TableEditor", data);
        }

        #endregion

        #region Редактирование типов страниц
        [AuthorizeMaster]
        [MenuItem("Шаблоны разделов", 223, 90, Icon = "templateside")]
        public ActionResult TypeManager(string Type, int? Page, int? UID)
        {

            var editedRow = (db.PageTypes.FirstOrDefault(x => x.ID == UID) ??
                             new PageType()
                             {
                                 Ordernum = db.PageTypes.Count() + 1,
                                 Enabled = true
                             });

            var settings = new UniversalEditorSettings
            {
                TableName = "PageTypes",
                HasDeleteColumn = true,
                CanAddNew = true,
                UIDColumnName = "ID",
                ShowedFieldsInList =
                    new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Description",
                                        IsLinkToEdit = true,
                                        HeaderText = "Описание"
                                    },                                
                                    new UniversalListField
                                    {
                                        FieldName = "TypeName",
                                        HeaderText = "Идентификатор"
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "Ordernum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                EditedFieldsList =
                    new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "TemplateID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = editedRow.ID == 0 ? db.CMSPageTemplates.First(x => x.IsActive).ID:editedRow.TemplateID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "TemplateName",
                                                Source = db.CMSPageTemplates.Where(x => x.IsActive).OrderBy(x=> x.TemplateName)
                                            },
                                        HeaderText = "Шаблон страницы",
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Enabled",
                                        DataType = typeof (bool),
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Активен?",
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "TypeName",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Идентификатор (только латинские символы)",
                                        Modificators = new List<IUniversalFieldModificator>()
                                            {
                                                new RequiredModificator(),
                                                new EnglishLettersModificator()
                                            }
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Description",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Описание",
                                        Modificators = new List<IUniversalFieldModificator>()
                                            {
                                                new RequiredModificator(),
                                            }
                                    }
                            }

            };



            settings.AutoFilter = false;

            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<PageType>(db.PageTypes.OrderBy(x => x.Ordernum), Page ?? 0, 10, "Master")
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((PageType)data.EditedRow).ID == 0;
            data.SaveRow = SaveType;
            data.BeforeDelFunc = DeleteType;
            data.CallerController = "TableEditors";
            data.CallerAction = "TypeManager";
            data.EditorName = "Шаблоны разделов";
            data.EditorDescription = ""; // "В этом разделе перечислены все шаблоны страниц, которые встречаются на сайте";
            return View("TableEditor", data);
        }

        private void DeleteType(object row, DB dbx)
        {
            var tr = dbx.PageTypes.FirstOrDefault(x => x.ID == (row as PageType).ID);
            if (tr == null)
                return;

            if (tr.CMSPageCells.Any())
            {
                dbx.CMSPageCells.DeleteAllOnSubmit(tr.CMSPageCells);
                dbx.SubmitChanges();
            }
            dbx.Refresh(RefreshMode.KeepChanges, tr);
            dbx.PageTypes.DeleteOnSubmit(tr);
            dbx.SubmitChanges();
        }

        private string SaveType(object row, DB db, HttpContextBase context)
        {
            var r = (PageType)row;
            try
            {
                if (r == null) return "Объект не найден";
                if (r.ID == 0)
                {
                    var template = db.CMSPageTemplates.First(x => x.ID == r.TemplateID);
                    var cells = template.CMSPageTemplateCells.Select(
                        x =>
                        new CMSPageCell()
                            {
                                ColumnName = x.CellID,
                                Description = x.CellName,
                                Hidden = false,
                                CSS = "",
                                PageType = r
                            });
                    db.PageTypes.InsertOnSubmit(r);
                    db.CMSPageCells.InsertAllOnSubmit(cells);
                }
                else
                {
                    object entry = db.PageTypes.FirstOrDefault(x => x.ID == r.ID);
                    if (entry != null)
                    {
                        r.Detach();
                        entry.LoadPossibleProperties(r, new string[] { "ID" });
                        db.Refresh(RefreshMode.KeepChanges, entry);
                    }
                }
                db.SubmitChanges();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        #region Редактирование содержимого разделов
        [AuthorizeMaster]
        [MenuItem("Расположение модулей", 222, 90, Icon = "position")]
        public ActionResult ViewManager(string Type, int? Page, int? UID, int? CellID, int? TypeID)
        {
            if (!TypeID.HasValue)
            {
                TypeID = db.PageTypes.First(x => x.TypeName == "MainPage").ID;
            }
            if (!CellID.HasValue)
            {
                CellID = db.CMSPageCells.First(x => x.TypeID == TypeID).ID;
            }

            var editedRow = (db.CMSPageCellViews.FirstOrDefault(x => x.ID == UID) ??
                             new CMSPageCellView()
                             {
                                 Controller = "TextPage",
                                 Action = "Index",
                                 CellID = (int)CellID,
                                 OrderNum = db.CMSPageCellViews.Count() + 1
                             });



            var settings = new UniversalEditorSettings
                {
                    TableName = "CMSPageCellViews",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "Path",
                                        IsLinkToEdit = true,
                                        HeaderText = "Модуль",
                                        TextFunction =
                                            x =>
                                            (TemplateEditorController.BlockList.FirstOrDefault(
                                                z => z.Path == x.ToString()) ??
                                             new ClientTemplate {Name = "--Undef--"}).Name
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "OrderNum",
                                        IsLinkToEdit = false,
                                        IsOrderColumn = true,
                                        HeaderText = "Порядок"
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "CellID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = CellID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Description",
                                                Source = db.CMSPageCells.Where(x => x.TypeID == TypeID && !x.Hidden)
                                            },
                                        HeaderText = "Контейнер",
                                        DataType = typeof(int),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Path",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Модуль",
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue =
                                                    editedRow.ID == 0
                                                        ? (TemplateEditorController.BlockList.FirstOrDefault(
                                                            x => x.Controller == "TextPage") ??
                                                           TemplateEditorController.BlockList.First()).Path
                                                        : editedRow.Path,
                                                HasEmptyDef = false,
                                                KeyField = "Path",
                                                ValueField = "Name",
                                                Source = TemplateEditorController.BlockList.Where(x => x.IsModul)
                                                /*.GroupBy(x=> x.Controller).Select(x=> x.First()).Where(x=> x.Name!="Выберите блок")*/
                                            },
                                        DataType = typeof (string),
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },
                                /* new UniversalEditorField
                                    {
                                        FieldName = "OrderNum",
                                        DataType = typeof (int),
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Порядковый номер",
                                    }*/
                            }

                };



            settings.AutoFilter = true;
            settings.FilterDescription = "Выберите шаблон страницы и контейнер";
            settings.Filters = new List<FilterConfiguration>
                {
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = TypeID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Description",
                                    Source = db.PageTypes
                                },
                            HeaderText = "Выберите шаблон страницы:",
                            IsDropDown = true,
                            QueryKey = "TypeID",
                            Type = FilterType.Integer,
                            SkipInQuery = true,
                            MainFilter = true
                        },
                    new FilterConfiguration
                        {
                            FilterSource = new UniversalDataSource
                                {
                                    DefValue = CellID,
                                    HasEmptyDef = false,
                                    KeyField = "ID",
                                    ValueField = "Description",
                                    Source = db.CMSPageCells.Where(x => x.TypeID == TypeID && !x.Hidden)
                                },
                            HeaderText = "Выберите контейнер:",
                            IsDropDown = true,
                            QueryKey = "CellID",
                            Type = FilterType.Container,
                            PageTypeID = TypeID??0
                            
                        }
                };



            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
            {
                PagedData =
                    type == CurrentEditorType.List
                        ? new PagedData<CMSPageCellView>(db.CMSPageCellViews.OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master", settings.Filters)
                        : null,
                Settings = settings,
                CurrentType = type,
                EditedRow =
                    type == CurrentEditorType.List
                        ? null
                        : editedRow
            };
            data.IsAddingNew = data.EditedRow != null && ((CMSPageCellView)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "ViewManager";
            data.SaveRow = SaveCellView;
            data.AddQueryParams = new[] { "CellID", "TypeID" };
            data.EditorName = "Расположение модулей";
            data.EditorDescription = ""; //"Выберите список модулей, которые будут отображаться на всех страницах этого шаблона";

            return View("TableEditor", data);
        }

        private string SaveCellView(object row, DB db, HttpContextBase context)
        {
            var r = (CMSPageCellView)row;
            try
            {
                if (r == null) return "Объект не найден";

                var module = TemplateEditorController.BlockList.First(x => x.Path == r.Path);
                r.Controller = module.Controller;
                r.Action = module.Action;
                r.Path = module.Path;
                if (r.ID == 0)
                {
                    db.CMSPageCellViews.InsertOnSubmit(r);
                }
                else
                {
                    object entry = db.CMSPageCellViews.FirstOrDefault(x => x.ID == r.ID);
                    if (entry != null)
                    {
                        r.Detach();
                        entry.LoadPossibleProperties(r, new string[] { "ID" });
                        db.Refresh(RefreshMode.KeepChanges, entry);
                    }
                }
                db.SubmitChanges();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region Редактирование ленты событий и новостей

        [AuthorizeMaster]
        public ActionResult LentaEditor(string Type, int? Page, int? UID, int? PageID)
        {
            if (!PageID.HasValue)
            {
                var lentas = CMSPage.GetByType("Lenta").ToList();
                if (lentas.Any())
                    PageID = lentas.First().ID;
            }
            var editedRow = db.Lentas.FirstOrDefault(x => x.ID == UID) ??
                            new Lenta
                                {
                                    CreateDate = DateTime.Now,
                                    Visible = true,
                                    ShowInfo = false,
                                    ShowTime = false,
                                    ViewAmount = 0
                                };


            var cssList = new[]
                {
                    new {CSS = "stream-block", Name = "--Не выбрано--"},
                    new {CSS = "stream-block_2x", Name = "Двойная ширина"},
                    new {CSS = "stream-block1", Name = "Блок с фоновой картинкой (надпись по центру)"},
                    new {CSS = "stream-block1 title-l", Name = "Блок с фоновой картинкой (надпись слева по центру)"},
                    new {CSS = "stream-block1 title-lb", Name = "Блок с фоновой картинкой (надпись слева-снизу)"},
                    new {CSS = "cite", Name = "Блок в виде цитаты"},
                    new {CSS = "cite1", Name = "Блок в виде цитаты с фото"},
                    new {CSS = "cite cite-big", Name = "Блок в виде цитаты (крупные буквы)"},
                    new {CSS = "-lgreen", Name = "Зеленый фон"},
                    new {CSS = "-pink", Name = "Красный фон"},
                    new {CSS = "-peach", Name = "Персиковый фон"},
                    new {CSS = "title-blue", Name = "Голубой заголовок"}
                };

            var settings = new UniversalEditorSettings
                {
                    TableName = "Lenta",
                    HasDeleteColumn = true,
                    CanAddNew = true,
                    UIDColumnName = "ID",
                    ShowedFieldsInList =
                        new List<UniversalListField>
                            {
                                new UniversalListField
                                    {
                                        FieldName = "PageID",
                                        IsLinkToEdit = true,
                                        HeaderText = "Страница для поста",
                                        TextFunction = x => CMSPage.Get((int) x).PageName
                                    },
                                new UniversalListField
                                    {
                                        FieldName = "HeaderText",
                                        IsLinkToEdit = true,
                                        HeaderText = "Заголовок",
                                        TextFunction = x=> x.ToString().ClearHTML()
                                    },
                                new UniversalListField {FieldName = "CreateDate", HeaderText = "Дата создания"},
                                new UniversalListField
                                    {
                                        FieldName = "Visible",
                                        HeaderText = "Статус",
                                        TextFunction = x => (bool) x ? "Отображается" : "Скрыто"
                                    }
                            },

                    EditedFieldsList =
                        new List<UniversalEditorField>
                            {
                                new UniversalEditorField
                                    {
                                        FieldName = "PageID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Страница",
                                        DataType = typeof (int),
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = editedRow.PageID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "PageName",
                                                Source = CMSPage.GetByType("Lenta")
                                            },
                                        Modificators =
                                            new List<IUniversalFieldModificator> {new RequiredModificator()}
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "CellID",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Контейнер",
                                        DataType = typeof (int),
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                DefValue = editedRow.CellID,
                                                HasEmptyDef = false,
                                                KeyField = "ID",
                                                ValueField = "Description",
                                                Source =
                                                    db.CMSPageCells.Where(
                                                        x =>
                                                        x.PageType.TypeName == "Lenta" && x.Hidden).ToList()
                                            }
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "CategoryName",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Раздел",
                                        DataType = typeof (string)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "HeaderText",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Текст в заголовке",
                                        DataType = typeof (string)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Image",
                                        FieldType = UniversalEditorFieldType.DBImageUpload,
                                        HeaderText = "Фоновое изображение"
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Photo",
                                        FieldType = UniversalEditorFieldType.DBImageUpload,
                                        HeaderText = "Фото в рамке"
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Author",
                                        FieldType = UniversalEditorFieldType.TextBox,
                                        HeaderText = "Автор"
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Text",
                                        FieldType = UniversalEditorFieldType.TextArea,
                                        HeaderText = "Текст в ленте"
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ShowInfo",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать инфо-блок",
                                        DataType = typeof (bool)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ShowTime",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать дату",
                                        DataType = typeof (bool)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "Visible",
                                        FieldType = UniversalEditorFieldType.CheckBox,
                                        HeaderText = "Отображать в ленте",
                                        DataType = typeof (bool)
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "FullText",
                                        FieldType = UniversalEditorFieldType.TextEditor,
                                        HeaderText = "Полный текст"
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "ViewAmount",
                                        FieldType = UniversalEditorFieldType.Label,
                                        HeaderText = "Количество просмотров"
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "CreateDate",
                                        FieldType = UniversalEditorFieldType.Calendar,
                                        HeaderText = "Дата создания"
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "CSS1",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Стиль CSS 1",
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "CSS",
                                                ValueField = "Name",
                                                Source = cssList,
                                                DefValue = editedRow.CSS1.IsNullOrEmpty() ? "stream-block": editedRow.CSS1,
                                                HasEmptyDef = false
                                            }
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "CSS2",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Стиль CSS 2",
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "CSS",
                                                ValueField = "Name",
                                                Source = cssList,
                                                DefValue = editedRow.CSS2.IsNullOrEmpty() ? "stream-block": editedRow.CSS2
                                            }
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "CSS3",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Стиль CSS 3",
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "CSS",
                                                ValueField = "Name",
                                                Source = cssList,
                                                DefValue = editedRow.CSS3.IsNullOrEmpty() ? "stream-block": editedRow.CSS3
                                            }
                                    },
                                new UniversalEditorField
                                    {
                                        FieldName = "CSS4",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Стиль CSS 4",
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "CSS",
                                                ValueField = "Name",
                                                Source = cssList,
                                                DefValue = editedRow.CSS4.IsNullOrEmpty() ? "stream-block": editedRow.CSS4
                                            }
                                    },

                                new UniversalEditorField
                                    {
                                        FieldName = "CSS5",
                                        FieldType = UniversalEditorFieldType.DropDown,
                                        HeaderText = "Стиль CSS 5",
                                        InnerListDataSource = new UniversalDataSource
                                            {
                                                KeyField = "CSS",
                                                ValueField = "Name",
                                                Source = cssList,
                                                DefValue = editedRow.CSS5.IsNullOrEmpty() ? "stream-block": editedRow.CSS5
                                            }
                                    },


                            },
                    AutoFilter = true,
                    Filters = new List<FilterConfiguration>
                        {
                            new FilterConfiguration
                                {
                                    FilterSource = new UniversalDataSource
                                        {
                                            DefValue = editedRow.PageID == 0 ? PageID : editedRow.PageID,
                                            HasEmptyDef = false,
                                            KeyField = "ID",
                                            ValueField = "PageName",
                                            Source = CMSPage.GetByType("Lenta")
                                        },
                                    HeaderText = "Страница",
                                    QueryKey = "PageID",
                                    IsDropDown = true,
                                    Type = FilterType.Integer,


                                }
                        }

                };


            var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
            var data = new UniversalEditorPagedData
                {
                    PagedData =
                        type == CurrentEditorType.List
                            ? new PagedData<Lenta>(db.Lentas.OrderByDescending(x => x.CreateDate), Page ?? 0, 10, "Master", settings.Filters)
                            : null,
                    Settings = settings,
                    CurrentType = type,
                    EditedRow =
                        type == CurrentEditorType.List
                            ? null
                            : editedRow
                };
            data.AddQueryParams = new[] { "PageID" };
            data.IsAddingNew = data.EditedRow != null && ((Lenta)data.EditedRow).ID == 0;
            data.CallerController = "TableEditors";
            data.CallerAction = "LentaEditor";
            data.SaveRow = SaveLenta;

            data.EditorName = "Редактирование ленты событий и новостей";
            return View("TableEditor", data);
        }

        private string SaveLenta(object game, DB db, HttpContextBase context)
        {
            try
            {
                if (game == null) return "Объект не найден";
                var tGame = (Lenta)game;
                tGame.TypeClass =
                    new[] { tGame.CSS1, tGame.CSS2, tGame.CSS3, tGame.CSS4, tGame.CSS5, "stream-block" }.SelectMany(
                        x => x.Split<string>(" ")).Distinct().OrderBy(x => x).JoinToString(" ");

                if (tGame.ID == 0)
                {
                    db.Lentas.InsertOnSubmit(tGame);
                }
                else
                {
                    object entry = db.Lentas.FirstOrDefault(x => x.ID == (game as Lenta).ID);
                    if (entry != null)
                    {
                        game.Detach();
                        entry.LoadPossibleProperties(game, new string[] { "ID" });
                        db.Refresh(RefreshMode.KeepChanges, entry);
                    }
                }
                db.SubmitChanges();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        /* Старый код из проекта (оставлен, чтобы не забыть как пользоваться классом)
         * [AuthorizeMaster]
           public ActionResult GameEditor(string Type, int? Page, int? UID)
           {
               var settings = new UniversalEditorSettings
                   {
                       TableName = "Games",
                       HasDeleteColumn = true,
                       CanAddNew = true,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                               {
                                   new UniversalListField
                                       {
                                           FieldName = "Name",
                                           IsLinkToEdit = true,
                                           HeaderText = "Название"
                                       },
                                   new UniversalListField {FieldName = "SuperPrize", HeaderText = "Суперприз"},
                                   new UniversalListField
                                       {
                                           FieldName = "OrderNum",
                                           IsOrderColumn = true,
                                           HeaderText = "Порядок"
                                       }
                               },

                       EditedFieldsList =
                           new List<UniversalEditorField>
                               {
                                   new UniversalEditorField
                                       {
                                           FieldName = "Name",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Название",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "SuperPrize",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Суперприз",
                                           DataType = typeof (int),
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       }
                               }

                   };
               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var data = new UniversalEditorPagedData
                   {
                       PagedData =
                           type == CurrentEditorType.List
                               ? new PagedData<object>(db.Games.OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master")
                               : null,
                       Settings = settings,
                       CurrentType = type,
                       EditedRow =
                           type == CurrentEditorType.List
                               ? null
                               : (db.Games.FirstOrDefault(x => x.ID == UID) ?? new Game())
                   };
               data.IsAddingNew = data.EditedRow != null && ((Game)data.EditedRow).ID == 0;
               data.CallerController = "TableEditors";
               data.CallerAction = "GameEditor";
               data.SaveRow = SaveGame;

               data.EditorName = "Редактирование типов игр";
               return View("TableEditor", data);
           }

           private string SaveGame(object game)
           {
               try
               {
                   if (game == null) return "Объект не найден";
                   var tGame = (Game)game;
                   if (tGame.ID == 0)
                   {
                       db.Games.InsertOnSubmit(tGame);
                       tGame.OrderNum = db.Games.Count() + 1;
                   }
                   else
                   {
                       db.Refresh(RefreshMode.KeepCurrentValues, game);
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }

           [AuthorizeMaster]
           public ActionResult LuckyEditor(string Type, int? Page, int? UID)
           {
               var settings = new UniversalEditorSettings
                   {
                       TableName = "LuckyPeople",
                       HasDeleteColumn = false,
                       CanAddNew = true,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                             {
                                 new UniversalListField
                                     {
                                         FieldName = "ActionDate",
                                         IsLinkToEdit = true,
                                         HeaderText = "Дата розыгрыша"
                                     },
                                 new UniversalListField
                                     {
                                         FieldName = "IsCompleted",
                                         IsLinkToEdit = true,
                                         HeaderText = "Статус",
                                         TextFunction = StatusFunc
                                     }
                             },

                       EditedFieldsList =
                           new List<UniversalEditorField>
                             {
                                 new UniversalEditorField
                                     {
                                         FieldName = "ActionDate",
                                         FieldType = UniversalEditorFieldType.Calendar,
                                         DataType = typeof (DateTime),
                                         HeaderText = "Дата розыгрыша",
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "MoneyPrize1",
                                         FieldType = UniversalEditorFieldType.TextBox,
                                         HeaderText = "Приз за первое место в реальном режиме",
                                         DataType = typeof (decimal),
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "MoneyPrize2",
                                         FieldType = UniversalEditorFieldType.TextBox,
                                         HeaderText = "Приз за второе место в реальном режиме",
                                         DataType = typeof (decimal),
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "MoneyPrize3",
                                         FieldType = UniversalEditorFieldType.TextBox,
                                         HeaderText = "Приз за третье место в реальном режиме",
                                         DataType = typeof (decimal),
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "ChipPrize1",
                                         FieldType = UniversalEditorFieldType.TextBox,
                                         HeaderText = "Приз за первое место в тактическом режиме",
                                         DataType = typeof (decimal),
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "ChipPrize2",
                                         FieldType = UniversalEditorFieldType.TextBox,
                                         HeaderText = "Приз за второе место в тактическом режиме",
                                         DataType = typeof (decimal),
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "ChipPrize3",
                                         FieldType = UniversalEditorFieldType.TextBox,
                                         HeaderText = "Приз за третье место в тактическом режиме",
                                         DataType = typeof (decimal),
                                         Modificators =
                                             new List<IUniversalFieldModificator> {new RequiredModificator()}
                                     },
                                 new UniversalEditorField
                                     {
                                         FieldName = "IsCompleted",
                                         FieldType = UniversalEditorFieldType.CheckBox,
                                         HeaderText = "Игра завершена?",
                                         DataType = typeof (bool),
                                     }
                             }

                   };
               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var data = new UniversalEditorPagedData
                   {
                       PagedData =
                           type == CurrentEditorType.List
                               ? new PagedData<object>(db.LuckyPeoples.OrderByDescending(x => x.ActionDate), Page ?? 0, 10, "Master")
                               : null,
                       Settings = settings,
                       CurrentType = type,
                       EditedRow =
                           type == CurrentEditorType.List
                               ? null
                               : (db.LuckyPeoples.FirstOrDefault(x => x.ID == UID) ?? new LuckyPeople())
                   };
               data.IsAddingNew = data.EditedRow != null && ((LuckyPeople)data.EditedRow).ID == 0;
               data.CallerController = "TableEditors";
               data.CallerAction = "LuckyEditor";
               data.SaveRow = SaveLucky;

               data.AddView = new UniversalEditorAddViewInfo()
               {
                   InEditor = true,
                   Action = "LuckWinnersAddView",
                   Controller = "MainPage",
                   Routes = HttpContext == null || HttpContext.Request == null ||
                       HttpContext.Request.QueryString["UID"] == null
                           ? new RouteValueDictionary()
                           : new RouteValueDictionary() { { "ID", HttpContext.Request.QueryString["UID"] } }
               };

               data.EditorName = "Редактирование типов игр";
               return View("TableEditor", data);
           }

           private string StatusFunc(object input)
           {
               return input.ToTypedValue<bool>() ? "Завершено" : "Ожидание даты розыгрыша";
           }

           private string SaveLucky(object lucky)
           {
               try
               {
                   if (lucky == null) return "Объект не найден";
                   var tGame = (LuckyPeople)lucky;
                   if (tGame.ID == 0)
                   {
                       db.LuckyPeoples.InsertOnSubmit(tGame);
                   }
                   else
                   {
                       db.Refresh(RefreshMode.KeepCurrentValues, lucky);
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }

           [AuthorizeMaster]
           public ActionResult BetEditor(string Type, int? Page, int? UID)
           {
               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var er = type == CurrentEditorType.List
                            ? null
                            : (db.GameBets.FirstOrDefault(x => x.ID == UID) ?? new GameBet());
               var defGame = db.Games.OrderBy(x => x.OrderNum).FirstOrDefault();
               var gameDataSource = new UniversalDataSource
                   {
                       DefValue = er == null ? (defGame == null ? null : (object)defGame.ID) : er.GameID,
                       HasEmptyDef = false,
                       KeyField = "ID",
                       ValueField = "Name",
                       Source = db.Games.AsEnumerable()
                   };
               var settings = new UniversalEditorSettings
                   {
                       AutoFilter = true,
                       TableName = "GameBets",
                       HasDeleteColumn = true,
                       CanAddNew = true,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                               {
                                   new UniversalListField
                                       {
                                           FieldName = "DigitAmount",
                                           IsLinkToEdit = true,
                                           HeaderText = "Количество цифр"
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "Bet",
                                           HeaderText = "Цена ставки"
                                       },
                                   new UniversalListField {FieldName = "Chance", HeaderText = "Вероятность выигрыша"},
                                   new UniversalListField {FieldName = "CombiAmount", HeaderText = "Кол-во комбинаций"},
                                   new UniversalListField {FieldName = "PlayerAmount", HeaderText = "Количество игроков"},
                                   new UniversalListField
                                       {
                                           FieldName = "OrderNum",
                                           IsOrderColumn = true,
                                           HeaderText = "Порядок"
                                       }
                               },


                       EditedFieldsList =
                           new List<UniversalEditorField>
                               {
                                   new UniversalEditorField
                                       {
                                           FieldName = "GameID",
                                           FieldType = UniversalEditorFieldType.DropDown,
                                           DataType = typeof (int),
                                           HeaderText = "Тип игры",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()},
                                           InnerListDataSource = gameDataSource
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "DigitAmount",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (int),
                                           HeaderText = "Количество цифр",
                                           Modificators =
                                               new List<IUniversalFieldModificator>
                                                   {
                                                       new RequiredModificator(),
                                                       new RangeModificator(5, decimal.MaxValue)
                                                   }
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Bet",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Цена ставки",
                                           DataType = typeof (decimal),
                                           Modificators =
                                               new List<IUniversalFieldModificator>
                                                   {
                                                       new RequiredModificator(),
                                                       new RangeModificator(0, decimal.MaxValue)
                                                   }
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Chance",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Вероятность выигрыша",
                                           DataType = typeof (int),
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "CombiAmount",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Кол-во комбинаций",
                                           DataType = typeof (int),
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "PlayerAmount",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Количество игроков в синдикате",
                                           DataType = typeof (int),
                                           Modificators =
                                               new List<IUniversalFieldModificator>
                                                   {
                                                       new RequiredModificator(),
                                                       new RangeModificator(0, decimal.MaxValue)
                                                   }
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "IsSuper",
                                           FieldType = UniversalEditorFieldType.CheckBox,
                                           HeaderText = "Суперсиндикат",
                                           DataType = typeof (bool),
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       }

                               }

                   };


               var filters = new List<FilterConfiguration>
                   {
                       new FilterConfiguration
                           {
                               FilterSource = gameDataSource,
                               IsDropDown = true,
                               QueryKey = "GameID",
                               HeaderText = "Выберите игру",
                               Type = FilterType.Integer
                           }
                   };
               var data = new UniversalEditorPagedData
                   {
                       PagedData =
                           type == CurrentEditorType.List
                               ? new PagedData<GameBet>(db.GameBets.OrderBy(x => x.OrderNum), Page ?? 0, 15, "Master",
                                                        filters)
                               : null,
                       Settings = settings,
                       CurrentType = type,
                       EditedRow = er

                   };
               data.Settings.Filters = filters;

               data.IsAddingNew = data.EditedRow != null && ((GameBet)data.EditedRow).ID == 0;
               data.CallerController = "TableEditors";
               data.CallerAction = "BetEditor";
               data.SaveRow = SaveGameBet;
               data.EditorName = "Управление ставками";
               return View("TableEditor", data);
           }

           private string SaveGameBet(object bet)
           {
               try
               {
                   if (bet == null) return "Объект не найден";
                   var tBet = (GameBet)bet;

                   if (tBet.ID == 0)
                   {
                       db.GameBets.InsertOnSubmit(tBet);
                       tBet.OrderNum = db.GameBets.Count() + 1;
                   }
                   else
                   {
                       //db.GameBets.Attach(tBet);
                       db.Refresh(RefreshMode.KeepCurrentValues, tBet);
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }

           public string EditMoneyWins(object input)
           {
               return "<a href='{0}'>редактировать выигрыши</a>".FormatWith(Url.Action("WinsEditor", "MainPage", new { ID = input }));
           }

           [AuthorizeMaster]
           public ActionResult PlanEditor(string Type, int? Page, int? UID)
           {
               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var er = type == CurrentEditorType.List
                            ? null
                            : (db.GamePlans.FirstOrDefault(x => x.ID == UID) ?? new GamePlan());
               var defGame = db.Games.OrderBy(x => x.OrderNum).FirstOrDefault();
               var gameDataSource = new UniversalDataSource
                   {
                       DefValue = er == null ? (defGame == null ? null : (object)defGame.ID) : er.GameID,
                       HasEmptyDef = false,
                       KeyField = "ID",
                       ValueField = "Name",
                       Source = db.Games.AsEnumerable()
                   };
               var settings = new UniversalEditorSettings
                   {
                       AutoFilter = true,
                       TableName = "GamePlans",
                       HasDeleteColumn = true,
                       CanAddNew = true,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                               {
                                   new UniversalListField
                                       {
                                           FieldName = "Date",
                                           IsLinkToEdit = true,
                                           HeaderText = "Дата тиража",
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "Number",
                                           IsLinkToEdit = false,
                                           HeaderText = "Номер тиража"
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "WinnerCombination",
                                           IsLinkToEdit = false,
                                           HeaderText = "Выигрышная комбинация", 
                                           TextFunction = UniversalEditorSettings.DefaultTextCheckerNoReplace
                                       },

                                   new UniversalListField
                                       {
                                           FieldName = "ID",
                                           HeaderText = "",
                                           TextFunction = EditMoneyWins
                                       }
                               },


                       EditedFieldsList =
                           new List<UniversalEditorField>
                               {
                                   new UniversalEditorField
                                       {
                                           FieldName = "GameID",
                                           FieldType = UniversalEditorFieldType.DropDown,
                                           DataType = typeof (int),
                                           HeaderText = "Тип игры",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()},
                                           InnerListDataSource = gameDataSource
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Date",
                                           FieldType = UniversalEditorFieldType.Calendar,
                                           DataType = typeof (DateTime),
                                           HeaderText = "Дата тиража",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Number",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (string),
                                           HeaderText = "Номер тиража",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "SuperPrize",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (int),
                                           HeaderText = "Суперприз"
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "WinnerCombination",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (string),
                                           HeaderText = "Выигрышная комбинация (через ;)"
                                       }
                               }
                   };


               var filters = new List<FilterConfiguration>
                   {
                       new FilterConfiguration
                           {
                               FilterSource = gameDataSource,
                               IsDropDown = true,
                               QueryKey = "GameID",
                               HeaderText = "Выберите игру",
                               Type = FilterType.Integer
                           }
                   };
               var data = new UniversalEditorPagedData
                   {
                       PagedData =
                           type == CurrentEditorType.List
                               ? new PagedData<GamePlan>(db.GamePlans.OrderByDescending(x => x.Date), Page ?? 0, 15,
                                                         "Master", filters)
                               : null,
                       Settings = settings,
                       CurrentType = type,
                       EditedRow = er

                   };
               data.Settings.Filters = filters;

               data.IsAddingNew = data.EditedRow != null && ((GamePlan)data.EditedRow).ID == 0;
               data.CallerController = "TableEditors";
               data.CallerAction = "PlanEditor";
               data.SaveRow = SaveGamePlan;
               data.EditorName = "Управление расписанием";
               return View("TableEditor", data);
           }

           private string SaveGamePlan(object bet)
           {
               try
               {
                   if (bet == null) return "Объект не найден";
                   var tBet = (GamePlan)bet;

                   if (tBet.ID == 0)
                   {
                       db.GamePlans.InsertOnSubmit(tBet);
                   }
                   else
                   {
                       db.Refresh(RefreshMode.KeepCurrentValues, tBet);
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }


           [AuthorizeMaster]
           public ActionResult FAQEditor(string Type, int? Page, int? UID)
           {
               var settings = new UniversalEditorSettings
                   {
                       TableName = "FAQs",
                       HasDeleteColumn = true,
                       CanAddNew = true,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                               {
                                   new UniversalListField
                                       {
                                           FieldName = "Qst",
                                           IsLinkToEdit = true,
                                           HeaderText = "Вопрос"
                                       },
                                   new UniversalListField {FieldName = "Author", HeaderText = "Автор"},
                                   new UniversalListField
                                       {
                                           FieldName = "OrderNum",
                                           IsOrderColumn = true,
                                           HeaderText = "Порядок"
                                       }
                               },

                       EditedFieldsList =
                           new List<UniversalEditorField>
                               {
                                   new UniversalEditorField
                                       {
                                           FieldName = "LangID",
                                           FieldType = UniversalEditorFieldType.DropDown,
                                           HeaderText = "Язык",
                                           DataType = typeof(int),
                                           ReadOnly = true,
                                           InnerListDataSource = new UniversalDataSource
                                               {
                                                   DefValue = AccessHelper.CurrentLang.ID,
                                                   HasEmptyDef = false,
                                                
                                                   KeyField = "ID",
                                                   ValueField = "Name",
                                                   Source = db.Languages.ToList(),
                                                
                                               },
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },    
                                    
                                       new UniversalEditorField
                                       {
                                           FieldName = "Qst",
                                           FieldType = UniversalEditorFieldType.TextArea,
                                           HeaderText = "Вопрос",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Author",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Автор",

                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Theme",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Тема",

                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Section",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Раздел",

                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Ans",
                                           FieldType = UniversalEditorFieldType.TextArea,
                                           HeaderText = "Ответ",
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "CreateDate",
                                           FieldType = UniversalEditorFieldType.Calendar,
                                           HeaderText = "Дата",
                                           DataType = typeof (DateTime),
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       }
                               }

                   };
               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var data = new UniversalEditorPagedData
               {
                   PagedData =
                       type == CurrentEditorType.List
                           ? new PagedData<object>(db.FAQs.Where(x => x.LangID == AccessHelper.CurrentLang.ID).OrderBy(x => x.OrderNum), Page ?? 0, 10, "Master")
                           : null,
                   Settings = settings,
                   CurrentType = type,
                   EditedRow =
                       type == CurrentEditorType.List
                           ? null
                           : (db.FAQs.FirstOrDefault(x => x.ID == UID) ?? new FAQ())
               };
               data.IsAddingNew = data.EditedRow != null && ((FAQ)data.EditedRow).ID == 0;
               data.CallerController = "TableEditors";
               data.CallerAction = "FAQEditor";
               data.SaveRow = SaveFAQ;

               data.EditorName = "Редактирование вопросов-ответов";
               return View("TableEditor", data);
           }


           private string SaveFAQ(object bet)
           {
               try
               {
                   if (bet == null) return "Объект не найден";
                   var tBet = (FAQ)bet;

                   if (tBet.ID == 0)
                   {
                       tBet.LangID = AccessHelper.CurrentLang.ID;
                       db.FAQs.InsertOnSubmit(tBet);
                   }
                   else
                   {
                       db.Refresh(RefreshMode.KeepCurrentValues, tBet);
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }

           [AuthorizeMaster]
           public ActionResult Syndicates(string Type, int? Page, int? UID)
           {

               int syndType = Session == null ? 1 : (int)(Session["SyndicateType"] ?? 1);

               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var source = db.Syndicates/*.Where(
                   x =>
                   syndType == -1 || (syndType == 0
                                          ? x.GameType == (int) SyndicateType.Tactic
                                          : (x.GameType == (int) SyndicateType.Real || x.GameType == (int) SyndicateType.Vip)))#1#
                              .OrderBy(x => x.PlayDate.HasValue)
                              .ThenByDescending(x => x.PlayDate)
                              .ToList()
                              .Select(x => x.PlayDate)
                              .Distinct()
                              .Select(
                                  x =>
                                  new KeyValuePair<string, string>(
                                      x.HasValue ? x.Value.ToString("yyyy.MM.ddTHH:mm:ss.fff") : "",
                                      x.HasValue ? x.Value.ToString("d MMMMM yyyy, HH:mm") : "Тираж не определен"));

               var er = type == CurrentEditorType.List
                          ? null
                          : (db.Syndicates.FirstOrDefault(x => x.ID == UID) ?? new Models.Syndicate());

               var defGame = (object)null;
               var gameDataSource = new UniversalDataSource
                   {
                       DefValue = er == null || !er.PlayDate.HasValue ? (defGame == null ? null : (object)defGame) : er.PlayDate.Value.ToString("yyyy.MM.ddTHH:mm:ss.fff"),
                       HasEmptyDef = false,
                       KeyField = "Key",
                       ValueField = "Value",
                       Source = source
                   };


               var settings = new UniversalEditorSettings
                   {
                       AutoFilter = true,
                       TableName = "Syndicate",
                       HasDeleteColumn = false,
                       CanAddNew = false,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                               {
                                   new UniversalListField
                                       {
                                           FieldName = "ID",
                                           IsLinkToEdit = true,
                                           HeaderText = "№ синдиката",
                                           TextFunction = CreateNum
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "Status",
                                           HeaderText = "Текущий статус"
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "GameName",
                                           HeaderText = "Тип игры"
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "Combination",
                                           HeaderText = "Комбинация синдиката",
                                           TextFunction = UniversalEditorSettings.DefaultTextCheckerNoReplace
                                       },

                                   new UniversalListField
                                       {
                                           FieldName = "TicketNumber",
                                           HeaderText = "Номер билета",
                                           TextFunction = UniversalEditorSettings.DefaultTextCheckerNoReplace
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "ID",
                                           HeaderText = "",
                                           TextFunction = CancelTextFunc
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "GameType",
                                           HeaderText = "",
                                           TextFunction = GameTypeTextFunc
                                       }
                               },


                       EditedFieldsList =
                           new List<UniversalEditorField>
                               {
                                   new UniversalEditorField
                                       {
                                           FieldName = "ID",
                                           FieldType = UniversalEditorFieldType.Label,
                                           DataType = typeof (int),
                                           HeaderText = "Номер синдиката",
                                           TextFunction = CreateNum
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Bet",
                                           FieldType = UniversalEditorFieldType.Label,
                                           DataType = typeof (decimal),
                                           HeaderText = "Ставка",
                                       },

                                   new UniversalEditorField
                                       {
                                           FieldName = "TicketNumber",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (string),
                                           HeaderText = "Номер билета",
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "IsPlayed",
                                           FieldType = UniversalEditorFieldType.CheckBox,
                                           HeaderText = "Тираж состоялся",
                                           DataType = typeof (bool),
                                       },                               
                                
                                   new UniversalEditorField
                                       {
                                           FieldName = "CloseDate",
                                           FieldType = UniversalEditorFieldType.Label,
                                           HeaderText = "Дата формирования синдиката",
                                           DataType = typeof (DateTime),
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Combination",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           ReadOnly = true,
                                           HeaderText = "Комбинация синдиката"
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "PlayDate",
                                           FieldType = UniversalEditorFieldType.Calendar,
                                           HeaderText = "Дата тиража",
                                           DataType = typeof (DateTime),
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "WinnerCombination",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (DateTime),
                                           HeaderText = "Выигрышная комбинация"
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "OverageWin",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (decimal),
                                           HeaderText = "Выигрыш на человека"
                                       },

                               }

                   };


               var filters = new List<FilterConfiguration>
                   {
                       new FilterConfiguration
                           {
                               FilterSource = gameDataSource,
                               IsDropDown = true,
                               QueryKey = "PlayDate",
                               HeaderText = "Выберите дату тиража",
                               Type = FilterType.Date
                           }
                   };
               var data = new UniversalEditorPagedData
                   {
                       PagedData =
                           type == CurrentEditorType.List
                               ? new PagedData<Models.Syndicate>(
                                     db.Syndicates.Where(x => x.CloseDate.HasValue && (syndType == -1 || (syndType == 0
                                                                                                              ? x.GameType ==
                                                                                                                (int)
                                                                                                                SyndicateType
                                                                                                                    .Tactic
                                                                                                              : (x.GameType ==
                                                                                                                 (int)
                                                                                                                 SyndicateType
                                                                                                                     .Real ||
                                                                                                                 x.GameType ==
                                                                                                                 (int)
                                                                                                                 SyndicateType
                                                                                                                     .Vip))))
                                       .OrderBy(x => x.PlayDate), Page ?? 0, 15, "Master",
                                     filters)
                               : null,
                       Settings = settings,
                       CurrentType = type,
                       EditedRow = er

                   };
               data.Settings.Filters = filters;

               data.IsAddingNew = false;
               data.CallerController = "TableEditors";
               data.CallerAction = "Syndicates";
               data.SaveRow = SaveSyndicate;
               data.EditorName = "Список синдикатов";

               if (type == CurrentEditorType.List)
               {
                   data.AddView = new UniversalEditorAddViewInfo()
                       {
                           InEditor = false,
                           Action = "SyndicateGameAddView",
                           Controller = "MainPage",
                           Routes = HttpContext == null || HttpContext.Request == null ||
                                    HttpContext.Request.QueryString["PlayDate"] == null
                                        ? new RouteValueDictionary()
                                        : new RouteValueDictionary()
                                            {
                                                {"PlayDate", HttpContext.Request.QueryString["PlayDate"]}
                                            }
                       };
               }
               else if (type == CurrentEditorType.Edit && UID > 0)
               {
                   data.AddView = new UniversalEditorAddViewInfo()
                       {
                           InEditor = true,
                           Action = "SyndicateTicket",
                           Controller = "MainPage",
                           Routes =
                               new RouteValueDictionary()
                                   {
                                       {"SyndicateID", UID}
                                   }
                       };

               }


               return View("TableEditor", data);
           }

           private string GameTypeTextFunc(object input)
           {
               return string.Format("<input type='hidden' class='game-type' value='{0}'/>", input);
           }

           private string CancelTextFunc(object input)
           {
               var s = db.Syndicates.FirstOrDefault(x => x.ID == (int)input);
               if (s == null || s.IsPrizeDistributed) return "";
               if (s.Canceled == 1)
                   return "<b class='canceled'>Отменен</b>";
               if (s.Canceled == 2)
                   return "<b class='canceled'>Не состоялся</b>";
               return string.Format("<a href='#' arg='{0}' class='synd-cancel'>Отменить</a>", input);
           }

           private string CreateNum(object input)
           {
               return (input.ToTypedValue<int>()).ToString("d8");
           }

           private string SaveSyndicate(object syndicate)
           {
               try
               {
                   if (syndicate == null) return "Объект не найден";
                   var tBet = (Models.Syndicate)syndicate;

                   var exist = db.Syndicates.FirstOrDefault(x => x.ID == tBet.ID);

                   if (tBet.ID == 0)
                   {
                       db.Syndicates.InsertOnSubmit(tBet);
                   }
                   else
                   {
                       db.Refresh(RefreshMode.KeepCurrentValues, tBet);
                   }
                   if (!exist.IsPrizeDistributed && tBet.WinnerCombination.IsFilled() && tBet.OverageWin.HasValue)
                   {
                       exist.DistributePrize(tBet.OverageWin.Value, tBet.Combination);
                       tBet.IsPrizeDistributed = true;
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }



           /////////////////////////////
           ///  Х  Х   У  У   Й  /Й  ///
           ///   Х      У    Й / Й   ///
           /// Х  Х    У    Й/  Й    ///
           /// /////////////////////////
           [AuthorizeMaster]
           public ActionResult WithdrawalEditor(string Type, int? Page, int? UID)
           {
               var type = (CurrentEditorType)Enum.Parse(typeof(CurrentEditorType), Type ?? "List", true);
               var er = type == CurrentEditorType.List
                            ? null
                            : (db.MoneyWithdrawals.FirstOrDefault(x => x.ID == UID) ?? new MoneyWithdrawal());

               var typeDataSource = new UniversalDataSource
               {
                   DefValue = 0,
                   HasEmptyDef = false,

                   KeyField = "Key",
                   ValueField = "Value",
                   Source = new List<KeyValuePair<int, string>>()
                       {
                           {new KeyValuePair<int, string>(0, "В ожидании")},
                           {new KeyValuePair<int, string>(-1, "Отмененные")},
                           {new KeyValuePair<int, string>(1, "Завершенные")}
                       }
               };
               var settings = new UniversalEditorSettings
                   {
                       AutoFilter = true,
                       TableName = "MoneyWithdrawals",
                       HasDeleteColumn = false,
                       CanAddNew = false,
                       UIDColumnName = "ID",
                       ShowedFieldsInList =
                           new List<UniversalListField>
                               {
                                   new UniversalListField
                                       {
                                           FieldName = "ID",
                                           IsLinkToEdit = true,
                                           HeaderText = "Номер заявки",
                                           TextFunction = x => ((int) x).ToString("d10")
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "UserID",
                                           HeaderText = "Пользователь",
                                           TextFunction = val => string.Format("<a href='{0}'>{1}</a>",
                                                                               Url.Action("Edit", "Users",
                                                                                          new {user = val}),
                                                                               Membership.GetUser((Guid) val)
                                                                                         .GetProfile()
                                                                                         .FullName)
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "Sum",
                                           HeaderText = "Сумма"
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "RequestDate",
                                           HeaderText = "Дата заявки"
                                       },
                                   new UniversalListField
                                       {
                                           FieldName = "TransactionID",
                                           HeaderText = "Номер транзакции"
                                       }
                               },


                       EditedFieldsList =
                           new List<UniversalEditorField>
                               {
                                   new UniversalEditorField
                                       {
                                           FieldName = "Status",
                                           FieldType = UniversalEditorFieldType.DropDown,
                                           DataType = typeof (int),
                                        
                                           HeaderText = "Статус",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()},
                                           InnerListDataSource = typeDataSource
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Sum",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           DataType = typeof (decimal),
                                           HeaderText = "Сумма для снятия",
                                           Modificators =
                                               new List<IUniversalFieldModificator>
                                                   {
                                                       new RequiredModificator(),
                                                       new RangeModificator(0, SiteSetting.Get<int>("MaxWithdrawal")+1)
                                                   }
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "BankName",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Наименование и номер отделения банка",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}

                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "BankKorr",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Корреспондентский счет отделения банка",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "BankBik",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "БИК банка",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "BankINN",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "ИНН банка",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "BankKPP",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "КПП банка",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "BankAccount",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText = "Расчетный счет отделения банка для перечисления выигрыша"
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "Comment",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText =
                                               "Правильная формулировка о назначении платежа для отделения банка получателя",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "UserAccount",
                                           FieldType = UniversalEditorFieldType.TextBox,
                                           HeaderText =
                                               "Номер лицевого счета, либо номер счета пластиковой карты для зачисления выигрыша",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "RequestDate",
                                           FieldType = UniversalEditorFieldType.Calendar,
                                           HeaderText = "Дата подачи заявки",
                                           Modificators =
                                               new List<IUniversalFieldModificator> {new RequiredModificator()}
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "CloseDate",
                                           FieldType = UniversalEditorFieldType.Calendar,
                                           HeaderText = "Дата закрытия заявки"
                                       },
                                   new UniversalEditorField
                                       {
                                           FieldName = "TransactionID",
                                           FieldType = UniversalEditorFieldType.Label,
                                           HeaderText = "Номер транзакции списания денег в системе",
                                           DataType = typeof (int?),
                                           TextFunction = x => ((int?) x).HasValue ? x.ToString() : "&lt;NOT ASSIGNED&gt;"
                                       }

                               }

                   };


               var filters = new List<FilterConfiguration>
                   {
                       new FilterConfiguration
                           {
                               FilterSource = typeDataSource,
                               IsDropDown = true,
                               QueryKey = "Status",
                               HeaderText = "Статус заявки",
                               Type = FilterType.Integer
                           }
                   };
               var data = new UniversalEditorPagedData
               {
                   PagedData =
                       type == CurrentEditorType.List
                           ? new PagedData<MoneyWithdrawal>(db.MoneyWithdrawals.Where(x => x.Status == Request["Status"].ToInt()).OrderByDescending(x => x.RequestDate), Page ?? 0, 15, "Master",
                                                    filters)
                           : null,
                   Settings = settings,
                   CurrentType = type,
                   EditedRow = er

               };
               data.Settings.Filters = filters;
               data.IsAddingNew = data.EditedRow != null && ((MoneyWithdrawal)data.EditedRow).ID == 0;
               data.CallerController = "TableEditors";
               data.CallerAction = "WithdrawalEditor";
               data.SaveRow = SaveWithdrawal;
               data.EditorName = "Управление заявками на вывод средств";


               data.AddView = new UniversalEditorAddViewInfo()
                   {
                       Action = "CancelWithdrawal",
                       Controller = "Cabinet",
                       InEditor = true,
                       Routes = new RouteValueDictionary(
                           new
                               {
                                   RedirectURL = "/Master/ru/TableEditors/WithdrawalEditor",
                                   ID = ((MoneyWithdrawal)data.EditedRow ?? new MoneyWithdrawal()).ID
                               })

                   };
               return View("TableEditor", data);
           }

           private string SaveWithdrawal(object bet)
           {
               try
               {
                   if (bet == null) return "Объект не найден";
                   var tBet = (MoneyWithdrawal)bet;

                   if (tBet.ID == 0)
                   {
                       db.MoneyWithdrawals.InsertOnSubmit((MoneyWithdrawal)bet);
                   }
                   else
                   {
                       //db.GameBets.Attach(tBet);
                       db.Refresh(RefreshMode.KeepCurrentValues, tBet);
                   }
                   db.SubmitChanges();
                   return "";
               }
               catch (Exception e)
               {
                   return e.Message;
               }
           }
    */
        [AuthorizeMaster]
        [HttpGet]
        public JsonResult GetCharList(int id)
        {
            var chars = db.StoreCharacterToProducts.Where(x => x.ProductID == id).ToArray();
            var data =
                chars.Select(
                    x =>
                        new[]
                        {
                            x.StoreCharacterValue.StoreCharacter.Name, "checkbox", x.StoreCharacterValue.Value, "checkbox",
                            "checkbox", x.StoreCharacterValue.StoreCharacter.Tooltip ?? ""
                        })
                    .ToList();
            data.Insert(0, new[] { "Название характеристики", "Все назв.", "Значение характеристики", "Все знач.", "Вся папка", "Подсказка" });
            return Json(data.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}