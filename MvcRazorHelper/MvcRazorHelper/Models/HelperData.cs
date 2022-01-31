using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcRazorHelper.Models
{
    public class HelperData
    {
    }

    public interface IWordCloudItem
    {
        string Text { get; set; }
        string Link { get; set; }
        decimal Percentage { get; set; }
    }
}