using Autofac;
using DoubanCrawler.Core.Mq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawler.Core
{
    public class AutofacConfig
    {
        public static IContainer Register(Action<ContainerBuilder> reg)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<HttpClientHelper>().As<IHttpClientHelper>();
            builder.RegisterType<SearchGroup>().As<ISearchGroup>();
            builder.RegisterType<MqService>().As<IMqService>();

            reg(builder);

            var container = builder.Build();

            return container;
        }


    }
}
