using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Infrastructure
{

    public static class HtmlExtensions
    {
        public static HtmlString Attr(this IHtmlHelper html, string name, string value, Func<bool> condition = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return HtmlString.Empty;
            }

            var render = condition != null ? condition() : true;

            return render ?
                new HtmlString(string.Format("{0}=\"{1}\"", name, value)) :
                HtmlString.Empty;
        }

        public static HtmlString Attr(this IHtmlHelper html, string name, string valueTrue, string valueFalse,  Func<bool> condition = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(valueTrue))
            {
                return HtmlString.Empty;
            }

            var render = condition != null ? condition() : true;

            return render ?
                new HtmlString(string.Format("{0}=\"{1}\"", name, valueTrue)) :
                new HtmlString(string.Format("{0}=\"{1}\"", name, valueFalse));
        }


    }
}
