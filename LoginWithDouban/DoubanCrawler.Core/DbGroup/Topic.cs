using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawler.Core.DbGroup
{
    public class Topic
    {
        public string Title { get; set; }
        public string TopicUrl { get; set; }
        public string Author { get; set; }
        public string AuthorHomeUrl { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
