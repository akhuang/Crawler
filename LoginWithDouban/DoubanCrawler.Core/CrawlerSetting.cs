using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawler.Core
{
    public class CrawlerSetting
    {
        public List<string> Groups { get; set; }
        public string PeopleId { get; set; }

        public int Page { get; set; }
    }

}
