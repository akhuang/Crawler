using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawler.Core
{
    public interface IApplication
    {
        void Run();
    }

    public class Application : IApplication
    {
        private ISearchGroup _searchGroup;
        public Application(ISearchGroup searchGroup)
        {
            _searchGroup = searchGroup;
        }
        public void Run()
        {
            _searchGroup.CrawlerGroup();
        }
    }

}
