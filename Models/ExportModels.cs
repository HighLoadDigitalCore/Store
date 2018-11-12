using System;
using System.IO;
using System.Web;
using Smoking.Extensions;
using Smoking.Models;

public class ExportInfo
{
    public bool NeedDescription { get; set; }
}

public interface IExportRow
{

}

public class CatalogImageSearcher
{
    private string _imagePath;
    private string _relativePath;
    public CatalogImageSearcher(string name, string path, HttpContext context = null)
    {
        if (!path.EndsWith("/"))
            path += "/";
        var di = new DirectoryInfo((context ?? HttpContext.Current).Server.MapPath(path));
        var files = di.GetFiles(name + ".*");
        if (files.Length > 0)
        {
            _imagePath = files[0].FullName;
            _relativePath = path + files[0].Name;
        }

    }
    public bool HasImage
    {
        get { return _relativePath.IsFilled(); }
    }
    public string RelativeFile
    {
        get { return _relativePath; }
    }
    public byte[] FileContent
    {
        get
        {
            try
            {
                using (var fs = new FileStream(_imagePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int) fs.Length);
                    return bytes;
                }
            }
            catch
            {
                return null;
            }
        }
    }

}/*

public class CatalogXLSExportCategoryRow : IExportRow
{
    public string Name { get; set; }
    public string SlugOrId { get; set; }
    public string ParentSlugOrId { get; set; }
    public string H1 { get; set; }
    public string H2 { get; set; }
    public string H3 { get; set; }
    public string PageTitle { get; set; }
    public string PageKeywords { get; set; }
    public string PageDescription { get; set; }
    public string Description { get; set; }
    public string PageTextH3Lower { get; set; }
    public string ImageURL { get; set; }
    public int Ordernum { get; set; }


    public static string[] ColumnNames = new[]
        {
            "Название", "URL раздела", "URL верхнего раздела", "Заголовок H1", "Заголовок H2", "Заголовок H3",
            "Title (SEO)",
            "Keywords (SEO)",
            "Description (SEO)", "Описание категории (над H3)", "Описание категории (под H3)", "Ссылка на изображение"
        };

    public static string[] PropNames = new string[]
        {
            "Name", "SlugOrId", "ParentSlugOrId", "H1", "H2", "H3", "PageTitle", "PageKeywords", "PageDescription",
            "Description", "PageTextH3Lower", "ImageURL"
        };

    public StoreCategory ToStoreCategory()
    {
        return new StoreCategory()
            {
                Name = Name,
                Slug = SlugOrId,
                PageTitle = PageTitle,
                PageKeywords = PageKeywords,
                PageDescription = PageDescription,
                Description = Description,
                PageHeader = H1,
                PageSubHeader = H2,
                PageHeaderH3 = H3,
                PageTextH3Lower = PageTextH3Lower
            };
    }

}

public class CatalogXLSExportProductRow : IExportRow
{
    public string Article { get; set; }
    public string Name { get; set; }
    public bool Favorite { get; set; }
    public bool Visible { get; set; }
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public string Description { get; set; }
    public string DescrptionLower { get; set; }
    public string Manufacturer { get; set; }
    public string Provider { get; set; }
    public string Categories { get; set; }
    public string Images { get; set; }
    public string PageTitle { get; set; }
    public string PageKeywords { get; set; }
    public string PageDescription { get; set; }
    public string Slug { get; set; }
    public string Volume { get; set; }
    public string Weight { get; set; }
    public string PageH1 { get; set; }
    public string PageH2 { get; set; }
    public string PageH3 { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
    public string Param4 { get; set; }
    public string Param5 { get; set; }
    public int Ordernum { get; set; }


    public static string[] ColumnNames = new[]
        {
            "Артикл", "Название", "Отображать в каталоге", "Активный товар", "Новая цена", "Старая цена",
            "Описание (верхнее)", "Описание (нижнее)",
            "Производитель", "Поставщик", "Категории", "Изображения", "Тайтл", "Ключевые слова",
            "Описание", "URL", "Объем", "Вес", "H1", "H2", "H3", "Param1", "Param2", "Param3", "Param4", "Param5"
        };

    public static string[] PropNames = new string[]
        {
            "Article", "Name", "Favorite", "Visible", "Price", "OldPrice", "Description", "DescrptionLower",
            "Manufacturer", "Provider",
            "Categories", "Images", "PageTitle", "PageKeywords", "PageDescription", "Slug", "Volume", "Weight", "PageH1"
            , "PageH2", "PageH3",
            "Доп. параметр 1", "Доп. параметр 2", "Доп. параметр 3", "Доп. параметр 4", "Доп. параметр 5"
        };


    public StoreProduct ToStoreProduct()
    {
        return new StoreProduct()
            {
                Article = Article,
                AddDate = DateTime.Now,
                Description = Description,
                DescrptionLower = DescrptionLower,
                IsActive = Visible,
                IsFavorite = Favorite,
                Name = Name,
                PageTitle = PageTitle,
                PageKeywords = PageKeywords,
                PageDescription = PageDescription,
                OldPrice = OldPrice,
                Slug = Slug,
                Price = Price,
                VoteCount = 0,
                VoteSum = 0,
                VoteOverage = 0,
                Review = "",
                PageH1 = PageH1,
                PageH2 = PageH2,
                PageH3 = PageH3,
                Param1 = Param1,
                Param2 = Param2,
                Param3 = Param3,
                Param4 = Param4,
                Param5 = Param5,
            };
    }
}*/