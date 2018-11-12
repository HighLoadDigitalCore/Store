<%@ WebHandler Language="C#" Class="FileSystemHandler" %>

using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Xml;


public class FileSystemHandler : IHttpHandler{
	

    #region IHttpHandler Members

    private HttpContext _context;
    private HttpContext Context
    {
        get
        {
            return _context;
        }
        set
        {
            _context = value;
        }
    }
    string _itemHandlerPath;
    Dictionary<string, string> mappedPathsInConfigFile;

    private void Initialize()
    {
        this._itemHandlerPath = "/FileSystemHandler.ashx";

        this.mappedPathsInConfigFile = new Dictionary<string, string>();
        var pairs = CustomFileSystemProvider.TelerikPathRelation;
        for (int i = 0; i < pairs.Length; i += 2)
        {
            this.mappedPathsInConfigFile.Add(PathHelper.RemoveEndingSlash(pairs[i], '/'), PathHelper.RemoveEndingSlash(pairs[i + 1], '\\'));
        }
    }


    public void ProcessRequest(HttpContext context){
        Context = context;
		 
		 if (Context.Request.QueryString["image-max-size"] != null && 
			 Context.Request.QueryString["image-file-path"] !=null){
		    var maxSize = Int32.Parse(Context.Request.QueryString["image-max-size"]);
		    var filePath = Context.Request.QueryString["image-file-path"];
		    var width = Context.Request.QueryString["image-width"] != null
		                   ? Int32.Parse(Context.Request.QueryString["image-width"])
		                   : 0;
			 var height = Context.Request.QueryString["image-height"] != null
					 ? Int32.Parse(Context.Request.QueryString["image-height"])
					 : 0;
          
		    if (GetPreviewImage(maxSize, filePath, width, height)){
		       return;
		    }
		 }
        if (Context.Request.QueryString["path"] == null)
        {
            return;
        }		 			

        Initialize();


        string virtualPathToFile = Context.Server.HtmlDecode(Context.Request.QueryString["path"]);
        string physicalPathToFile = "";
        foreach (KeyValuePair<string, string> mappedPath in mappedPathsInConfigFile)
        {
            if (virtualPathToFile.ToLower().StartsWith(mappedPath.Key.ToLower()))
            {
                // Build the physical path to the file ;
                physicalPathToFile = virtualPathToFile.Replace(mappedPath.Key, mappedPath.Value).Replace("/", "\\");

                break;// Brak the foreach loop ;
            }
        }
        try
        {
			//HttpUtility.UrlEncode
			// Handle files
			string contentType = "";
			string ext = Path.GetExtension(physicalPathToFile).ToLower();
            if (ext.Length > 0)
                MIMETypeWrapper.GetMIME(ext.Substring(1));
				//contentType = ApacheMimeTypes.GetType(ext.Substring(1));
			if (contentType.Length == 0)
				//Context.Response.WriteFile(physicalPathToFile);
                WriteFile(physicalPathToFile, "application/octet-stream", Context.Response);
			else
                
				WriteFile(physicalPathToFile, contentType, Context.Response);
        }
        catch
        {
            Context.Response.StatusCode = 404;
            Context.Response.End();
        }
    }


    /// <summary>
    /// Forces browser to download the file
    /// </summary>
    /// <param name="physicalPathToFile">Physical path to the file on the server </param>
    /// <param name="contentType">The file content type</param>
    private void WriteFile(string physicalPathToFile, string contentType, HttpResponse response)
    {
        response.Buffer = true;
        response.Clear();
        string ct = response.ContentType;
        response.ContentType = contentType == "" ? response.ContentType : contentType;
        string extension = Path.GetExtension(physicalPathToFile);

        if (extension != ".htm" && extension != ".html" && extension != ".xml")
        {
            response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(physicalPathToFile));
        }
        try
        {
            response.WriteFile(physicalPathToFile);
            response.Flush();
            response.End();

        }
        catch
        {
            response.StatusCode = 404;
            response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    
    #endregion

	#region

	/// <summary>Метод возвращает иконку изображения</summary>
	/// <returns></returns>
	 public bool GetPreviewImage(int maxSize, string filepath, int width = 0, int height = 0) {
		 if (maxSize != 0) {
			 return GetImageByWidth(maxSize, filepath);
		 }

		 if (height > 0 && width > 0) {
			 return SetImageInParams(width, height, filepath);
		 }
		 if (width > 0 && height == 0) {
			 return GetImageByWidth(width, filepath);
		 }

		 #region ЕСЛИ ОПУШЕННЫ  maxSize,width,height
		 var memoryStream = new MemoryStream();
		 // формируем путь к изображению
		 var photoPath = HttpContext.Current.Server.MapPath("~/" + filepath);

		 if (photoPath == null)
			 return false;
		 if (!File.Exists(photoPath)) {
			 photoPath = HttpContext.Current.Server.MapPath("~/noimage.jpg");
		 }
		 var photo = new Bitmap(photoPath);
		 using (var graphics = Graphics.FromImage(photo)) {
			 graphics.CompositingQuality = CompositingQuality.HighSpeed;
			 graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			 graphics.CompositingMode = CompositingMode.SourceCopy;
			 graphics.DrawImage(photo, 0, 0, photo.Width, photo.Height);
			 photo.Save(memoryStream, ImageFormat.Jpeg);
		 }
		 photo.Dispose();

         HttpContext.Current.Response.ContentType = "image/jpeg";
		 HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
		 HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
		 HttpContext.Current.Response.End(); 
		 return true;
		 #endregion
	}

	 private bool GetImageByWidth(int maxWidth, string filepath) {
		 var memoryStream = new MemoryStream();
		 var thumbnailSize = maxWidth;
       //var photoPath = string.Empty;
       //// формируем путь к изображению
       //if (HostingGateway.CloudEnabled){
       //  photoPath= PathHelper.TransformPath(filepath)
       //}
       //photoPath = HttpContext.Current.Server.MapPath("~/" + filepath);
		 var photoPath = PathHelper.TransformPath(filepath);
		 if (photoPath == null)
			 return false;
		 if (!File.Exists(photoPath)) {
			 photoPath = HttpContext.Current.Server.MapPath("~/noimage.png");
		 }
		 var photo = new Bitmap(photoPath);

		 double width2 = 0, height2 = 0, koeff;

		 if (photo.Width > thumbnailSize && photo.Height > thumbnailSize) {
			 if (photo.Width > photo.Height) {
				 koeff = (double)thumbnailSize / photo.Width;
			 }
			 else {
				 koeff = (double)thumbnailSize / photo.Height;
			 }
			 width2 = photo.Width * koeff;
			 height2 = photo.Height * koeff;
		 }
		 else if (photo.Width > thumbnailSize && photo.Height <= thumbnailSize) {
			 koeff = (double)thumbnailSize / photo.Width;
			 width2 = photo.Width * koeff;
			 height2 = photo.Height * koeff;
		 }
		 else if (photo.Width == thumbnailSize && photo.Height == thumbnailSize) {
			 width2 = thumbnailSize;
			 height2 = thumbnailSize;
		 }
		 else if (photo.Height > thumbnailSize && photo.Width <= thumbnailSize) {
			 koeff = (double)thumbnailSize / photo.Height;
			 width2 = photo.Width * koeff;
			 height2 = photo.Height * koeff;
		 }
		 else if (photo.Height < thumbnailSize && photo.Width < thumbnailSize) {
			 width2 = photo.Width;
			 height2 = photo.Height;
		 }

		 //if(filepath.IndexOf("fa5ef710-23c5-40b5-95d8-e42b78dc81ed") !=-1){
		 //   var tt = string.Empty;
		 //}
		 var target = new Bitmap((int)width2 == 0 ? photo.Width : (int)width2, (int)height2 == 0 ? photo.Height : (int)height2);
		 using (var graphics = Graphics.FromImage(target)) {
			 graphics.CompositingQuality = CompositingQuality.HighSpeed;
			 graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			 graphics.CompositingMode = CompositingMode.SourceCopy;
			 graphics.DrawImage(photo, 0, 0, (int)width2, (int)height2);
			 target.Save(memoryStream, ImageFormat.Jpeg);
		 }
		 photo.Dispose();

         HttpContext.Current.Response.ContentType = "image/jpeg";
		 HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
		 HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
		 HttpContext.Current.Response.End();
	    return true;
	 }

	 private bool SetImageInParams(int maxWidth, int maxHeight, string filepath) {

		 var freeWidth = maxWidth;
		 var freeHeight = maxHeight;
		 int newWidth;
		 int newHeight;

		 // формируем путь к изображению
		 var photoPath = HttpContext.Current.Server.MapPath("~/" + filepath);

		 if (photoPath == null)
			 return false;
		 if (!File.Exists(photoPath)) {
			 photoPath = HttpContext.Current.Server.MapPath("~/noimage.png");
		 }
		 var photo = new Bitmap(photoPath);

		 #region размер картинки больше размера содержимого
		 if (photo.Height > freeHeight && photo.Width > freeWidth) {
			 if (photo.Height > photo.Width) {
				 // коэфициент
				 if (freeHeight <= 0) freeHeight = 1;
				 var koeff = freeHeight / (double)photo.Height;
				 // определяем ширину нового изображения 
				 newWidth = (int)(photo.Width * koeff);
				 // определяем высоту нового изображения
				 newHeight = (int)(photo.Height * koeff);
				 return GetResult(newWidth, newHeight, ref photo);
			 }
			 else {
				 if (freeWidth <= 0) freeWidth = 1;
				 var koeff = freeWidth / (double)photo.Width;
				 // определяем ширину изображения
				 var imgWidth = photo.Width * koeff;
				 // определяем высоту изображения
				 var imgHeight = photo.Height * koeff;

				 if ((int)imgHeight > freeHeight) {
					 // коэфициент
					 if (freeHeight<= 0) freeHeight = 1;
					 koeff = freeHeight / imgHeight;
					 // определяем ширину нового изображения 
					 newWidth = (int)(imgWidth * koeff);
					 // определяем высоту нового изображения
					 newHeight = (int)(imgHeight * koeff);
					 return GetResult(newWidth, newHeight, ref photo);
				 }
				 if ((int)imgWidth > freeWidth) {
					 if (freeWidth <= 0) freeWidth = 1;
					 koeff = freeWidth / imgWidth;
					 // определяем ширину нового изображения 
					 newWidth = (int)(photo.Width * koeff);
					 // определяем высоту нового изображения
					 newHeight = (int)(photo.Height * koeff);
					 return GetResult(newWidth, newHeight, ref photo);
				 }
				 // определяем ширину изображения
				 newWidth = (int)imgWidth;
				 // определяем высоту изображения
				 newHeight = (int)imgHeight;
				 return GetResult(newWidth, newHeight, ref photo);
			 }
		 }
		 #endregion

		 #region размер картинки меньше размера содержимого
		 if (photo.Height <= freeHeight && photo.Width <= freeWidth) {
			 // определяем ширину изображения
			 newWidth = photo.Width;
			 // определяем высоту изображения
			 newHeight = photo.Height;
			 return GetResult(newWidth, newHeight, ref photo);
		 }
		 #endregion

		 #region ширина картинки больше свободной ширины а высота меньше или равна свободной стороне
		 if (photo.Width > freeHeight && photo.Height <= freeHeight) {
			 var koeff = freeWidth / (double)photo.Width;
			 // определяем ширину изображения
			 newWidth = (int)(photo.Width * koeff);
			 // определяем высоту изображения
          newHeight = (int)(photo.Height * koeff);
			 return GetResult(newWidth, newHeight, ref photo);
		 }
		 #endregion

		 #region высота картинки больше свободной области а ширина меньше или равно свободной области
		 if (photo.Height > freeHeight && photo.Width <= freeWidth) {
			 var koeff = freeHeight / (double)photo.Height;
			 // определяем ширину изображения
			 newWidth = (int)(photo.Width * koeff);
			 // определяем высоту изображения
			 newHeight = (int)(photo.Height * koeff);
			 return GetResult(newWidth, newHeight, ref photo);
		 }
		 #endregion

		 return false;
	 }

	 private static bool GetResult(int newWidth, int newHeight, ref Bitmap photo) {
		 var memoryStream = new MemoryStream();
		 var target = new Bitmap(newWidth, newHeight);
		 using (var graphics = Graphics.FromImage(target)) {
			 graphics.CompositingQuality = CompositingQuality.HighSpeed;
			 graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			 graphics.CompositingMode = CompositingMode.SourceCopy;
			 graphics.DrawImage(photo, 0, 0, newWidth, newHeight);
			 target.Save(memoryStream, ImageFormat.Jpeg);
		 }
		 photo.Dispose();

         HttpContext.Current.Response.ContentType = "image/jpeg";
		 HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
		 HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
		 HttpContext.Current.Response.End();
	    return true;
	 }
	#endregion
}