using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRazorHelper.Helpers
{
    public static class CustomHtmlHelper
    {
        /// <summary>
        /// Iframe
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name">Id</param>
        /// <param name="src"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString Iframe(this HtmlHelper helper, string name, string src, object htmlAttributes = null)
        {
            var iframeBuilder = new TagBuilder("iframe");
            iframeBuilder.GenerateId(name);
            iframeBuilder.MergeAttribute("src", src);
            var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            iframeBuilder.MergeAttributes(attr);
            return new MvcHtmlString(iframeBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 空值使用&nbsp;取代
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString StringOrSpace(this HtmlHelper helper, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new HtmlString("&nbsp;");
            }
            return new HtmlString(helper.Encode(value));
        }

        /// <summary>
        /// AntiXss
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString AntiXssRaw(this HtmlHelper helper, string value)
        {
            return new HtmlString(Sanitizer.GetSafeHtmlFragment(value));
        }
    }
}