using DoubanCrawler.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoubanCrawler.Core
{
    public interface ISearchGroup
    {
        void CrawlerGroup();
    }
    public class SearchGroup : ISearchGroup
    {
        private IHttpClientHelper _httpclientHelper;
        private CrawlerSetting _setting;
        public SearchGroup(IHttpClientHelper httpclientHelper, CrawlerSetting setting)
        {
            _httpclientHelper = httpclientHelper;
            _setting = setting;
        }
        public string GroupName = "";

        public void CrawlerGroup()
        {
            var group = _setting.Groups[0];
            int page = _setting.Page;

            while (true)
            {
                Console.WriteLine("CurrentPage:" + page);
                string groupUrl = string.Format("{0}/discussion?start={1}", group, page);
                var pageSource = _httpclientHelper.GetPageSource(groupUrl);

                var result = _httpclientHelper.CleanPageSourceForGroup(pageSource);

                if (string.IsNullOrEmpty(result))
                {
                    //result is null
                    break;
                }

                page += 25;

                //sleep 1 second
                Thread.Sleep(3 * 1000);
            }

        }

    }
}
