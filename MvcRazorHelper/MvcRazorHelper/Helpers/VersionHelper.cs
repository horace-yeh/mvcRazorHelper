using System.IO;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcRazorHelper.Helpers
{
    public static class VersionHelper
    {
        //Reference : https://blog.darkthread.net/blog/js-versioning-htmlhelper/

        private readonly static MemoryCache cache = MemoryCache.Default;

        /// <summary>
        /// script import hash version
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">Physical Path(ex:"~/xxx/xx.js")</param>
        /// <returns></returns>
        public static MvcHtmlString ScriptHash(this HtmlHelper helper, string src)
        {
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("src", GetPathWithHash(src));
            return new MvcHtmlString(scriptBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// css import hash version
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="href">Physical Path(ex:"~/xxx/xx.css")</param>
        /// <returns></returns>
        public static MvcHtmlString CssHash(this HtmlHelper helper, string href)
        {
            var linkBuilder = new TagBuilder("link");
            linkBuilder.MergeAttribute("rel", "stylesheet");
            linkBuilder.MergeAttribute("href", GetPathWithHash(href));
            return new MvcHtmlString(linkBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// img import hash version
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">Physical Path(ex:"~/xxx/xx.jpg")</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ImgHash(this HtmlHelper helper, string src, object htmlAttributes = null)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", GetPathWithHash(src));
            var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            imgBuilder.MergeAttributes(attr);
            return new MvcHtmlString(imgBuilder.ToString(TagRenderMode.SelfClosing));
        }

        #region method

        private static string GetFileHash(string path)
        {
            var physicalPath = HostingEnvironment.MapPath(path);
            if (!File.Exists(physicalPath)) return string.Empty;
            string cacheKey = $"__asset_hash__{path}";
            if (cache.Contains(cacheKey)) return cache[cacheKey] as string;
            using (SHA256 sha256 = SHA256.Create())
            {
                var hash = HttpServerUtility.UrlTokenEncode(sha256.ComputeHash(File.ReadAllBytes(physicalPath)));
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new string[] { physicalPath }));
                cache.Add(cacheKey, hash, policy);
                return hash;
            }
        }

        private static string GetPathWithHash(string path)
        {
            return $"{VirtualPathUtility.ToAbsolute(path)}?v={GetFileHash(path)}";
        }

        #endregion method

    }
}