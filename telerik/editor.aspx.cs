using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Smoking.Extensions;


public partial class editor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        textBlock.ImageManager.ViewPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Images" };
        textBlock.ImageManager.UploadPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Images" };
        textBlock.ImageManager.DeletePaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Images" };

        textBlock.DocumentManager.ViewPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Docs" };
        textBlock.DocumentManager.UploadPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Docs" };
        textBlock.DocumentManager.DeletePaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Docs" };

        textBlock.FlashManager.ViewPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Flash" };
        textBlock.FlashManager.UploadPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Flash" };
        textBlock.FlashManager.DeletePaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Flash" };

        textBlock.MediaManager.ViewPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Media" };
        textBlock.MediaManager.UploadPaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Media" };
        textBlock.MediaManager.DeletePaths = new[] { ConfigurationManager.AppSettings["FileStoragePath"] + "Editor\\Media" };


        textBlock.ImageManager.SearchPatterns = new[] { "*.*" };
        //textBlock.ImageManager.EnableImageEditor = false;
        textBlock.ImageManager.ContentProviderTypeName = typeof(CustomFileSystemProvider).AssemblyQualifiedName;

        textBlock.DocumentManager.SearchPatterns = new[] { "*.*" };
        textBlock.DocumentManager.ContentProviderTypeName = typeof(CustomFileSystemProvider).AssemblyQualifiedName;
        textBlock.FlashManager.SearchPatterns = new[] { "*.*" };
        textBlock.FlashManager.ContentProviderTypeName = typeof(CustomFileSystemProvider).AssemblyQualifiedName;
        textBlock.MediaManager.SearchPatterns = new[] { "*.*" };
        textBlock.MediaManager.ContentProviderTypeName = typeof(CustomFileSystemProvider).AssemblyQualifiedName;


        var data = new DB();
        try
        {
            

            var db = new DB();
            var table = db.GetTableByName(Request["table"]);
            var all = new List<KeyValuePair<object, int>>();

            object target =
                Enumerable.Cast<object>(table)
                    .FirstOrDefault(
                        item => (int) item.GetPropertyValue(Request["searchcolumn"]) == int.Parse(Request["condition"]));
            if (target != null)
            {
                textBlock.Content = (target.GetPropertyValue(Request["targetcolumn"]) ?? "").ToString();
                /*      target.SetPropertyValue(ordername, 1);
            
                db.Refresh(RefreshMode.KeepChanges);
                db.SubmitChanges();*/

            }




        }

        catch (Exception ex)
        {
            textBlock.Content = ex.Message + "<br>" + ex.StackTrace;
        }
    }
}