using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Infrastructure
{
    public static class JavascriptExtension
    {
        public static HtmlString IncludeVersionedJs(this IHtmlHelper helper, string filename)
        {
            string version = GetVersion(helper, filename);
            return new HtmlString("<script type='text/javascript' src='" + filename + version + "'></script>");
        }

        private static string GetVersion(this IHtmlHelper helper, string filename)
        {
            var context = helper.ViewContext.HttpContext;

            var physicalPath = Startup.HostingEnvironment.WebRootPath + filename;

            var fi = new System.IO.FileInfo(physicalPath);

            var version = $"?v={fi.LastWriteTime.ToString("MMddHHmmss")}";

            return version;
        }
    }
}
