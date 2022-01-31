using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MvcRazorHelper.Models;

namespace MvcRazorHelper.Helpers
{
    public static class WordCloudHelper
    {

        public static IHtmlString WordCloudTagA(this HtmlHelper helper, IWordCloudItem wordCloudItem, decimal fontSize)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("target", "_blank");
            builder.MergeAttribute("href", wordCloudItem.Link);
            builder.MergeAttribute("style", $@"font-size: {fontSize}px;");
            builder.SetInnerText(wordCloudItem.Text);
            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString WordCloudTagSpan(this HtmlHelper helper, IHtmlString html)
        {
            var builder = new TagBuilder("span");
            builder.InnerHtml = html.ToHtmlString();
            builder.AddCssClass("mx-1");
            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString WordClouds(this HtmlHelper helper, IEnumerable<IWordCloudItem> wordCloudItems)
        {
            const decimal defaultFontSize = 12M;
            const decimal minZoom = 0.75m;
            const decimal maxZoom = 2.25m;

            var sortTemp = wordCloudItems.OrderByDescending(x => x.Percentage);
            var maxPerc = sortTemp.First().Percentage;
            var minPerc = sortTemp.Last().Percentage;

            wordCloudItems.ForEach(x =>
            {
                var perc = Decimal.Round((maxPerc * (x.Percentage - minPerc) / (maxPerc - minPerc)));
                perc = perc < 1m ? 1m : perc;
                x.Percentage = perc;
            });

            var maxLevel = wordCloudItems.Max(x => x.Percentage);

            wordCloudItems.ForEach(x =>
            {
                var perc = minZoom + Decimal.Round(x.Percentage / maxLevel, 1) * maxZoom;
                perc = Decimal.Round(perc * defaultFontSize);
                x.Percentage = perc;
            });

            var htmlTemp = wordCloudItems.Select(x => helper.WordCloudTagSpan(helper.WordCloudTagA(x, x.Percentage)));
            return ConcatMvcHtmlString(htmlTemp);
        }

        #region Mothod

        private static MvcHtmlString ConcatMvcHtmlString(IEnumerable<IHtmlString> items)
        {
            var sb = new StringBuilder();
            foreach (var item in items.Where(i => i != null))
                sb.Append(item.ToHtmlString());
            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion Mothod
    }
}