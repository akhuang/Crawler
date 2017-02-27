using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoubanCrawler
{
    public static class HtmlAgilityPackUtility
    {
        public static HtmlNodeCollection ClassToFind(this HtmlNode node, string className)
        {
            return node.SelectNodes(string.Format("//div[contains(@class, '{0}')]", className));
        }
    }
}
