using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

using System.Web.Routing;
using Smoking.Extensions;



namespace Smoking.Models
{
    public class StaticContentInterceptor : IHttpModule
    {

        public void Init(HttpApplication context)
        {
        
        }

        

        public void Dispose() { }



    }

}
