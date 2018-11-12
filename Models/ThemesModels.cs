using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Smoking.Extensions;

namespace Smoking.Models
{
    public partial class ThemeProperty
    {
        public string InputName
        {
            get { return Editor + "_" + ID; }
        }
        public bool IsNullImage
        {
            get
            {
/*
                if (ValueBinary == null || ValueBinary.Length == 0)
                {
                    var p = HttpContext.Current.Server.MapPath(ValueText);
                    if (File.Exists(p))
                    {
                        using (var fs = new FileStream(p, FileMode.Open, FileAccess.Read))
                        {
                            byte[] b = new byte[fs.Length];
                            fs.Read(b, 0, (int) fs.Length);
                            ValueBinary = b;
                        }
                    }
                }
*/
                return ValueBinary == null || ValueBinary.Length == 0;
            }
        }

        public string DeleteWrapper
        {
            get
            {
                
                return UniversalEditorPagedData.GetDeleteWrapper("ThemeProperties", "ID", ID.ToString(), "ValueBinary");

            }
        }

        public string ImageWrapper  
        {
            get
            {
                return "/Master/ru/Theme/Image/" + ID;
                return UniversalEditorPagedData.GetImageWrapper("ThemeProperties", "ID", ID.ToString(), "ValueBinary"); ;
            }
        }

        public string ValueString
        {
            get
            {
                if (Editor == "Color" || Editor == "TextBox")
                    return ValueText ?? "";


                if (ValueBinary == null || ValueBinary.Length == 0)
                    return "";

                return ImageWrapper;
            }
        }

        public bool IsFilled
        {
            get
            {
                if (Editor == "Color" || Editor == "TextBox")
                    return (ValueText ?? "").IsFilled();

                return (ValueBinary != null && ValueBinary.Length > 0);
            }
        }
    }
}