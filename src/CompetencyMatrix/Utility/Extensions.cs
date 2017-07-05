using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Utility
{
    public static class Extensions
    {
        /// <summary>
        /// Reduce string to appropriate count chars and add '...' ellipse in the trail
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Reduce(this string source, int length)
        {
            const string reduceIndicator = "...";
            if (string.IsNullOrEmpty(source) || source.Length <= length + reduceIndicator.Length)
                return source;
            return string.Format("{0}{1}", source.Substring(0, length), reduceIndicator);
        }

        public static string AddAnchor(this string source, string url, string text)
        {
            string anchor = string.Format($"<a href = {url} class='navbar-link'> {text} </a>");
            return string.Format($"{source}{anchor}");
        }

        /// <summary>
        /// Generate link for request employee descriptions
        /// </summary>
        /// <param name="source"></param>
        /// <param name="function"> function for request data from server</param>
        /// <param name="param"> params for function</param>
        /// <param name="text">text that dispalyed in anchor</param>
        /// <returns></returns>
        public static string AddDescriptionAnchor(this string source, string function, string param, string text)
        {
            string anchor = string.Format($"<a href = javascript:{function}('{param}') class='navbar-link'> {text} </a>");
            return string.Format($"{source}{anchor}");
        }

        public static string AddDescriptionTooltip(this string source, string contentDescription, string text)
        {
            //string anchor = string.Format($"<a href = '#' class='navbar-link' data-toggle='tooltip' title='{contentDescription}'> {text} </a>");
            //return string.Format($"{source}{anchor}");
            return source;
        }

        public static string AddDescriptionPopover(this string source, string contentDescription, string text)
        {
            string anchor = string.Format($"<a href = '#' class='navbar-link' data-toggle='popover' data-content='{contentDescription}'  title='Project description'> {text} </a>");
            return string.Format($"{source}{anchor}");
        }
    }
}
