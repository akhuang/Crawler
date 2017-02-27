using DoubanCrawler.Core;
using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace DoubanCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlerSetting config;
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                config = JsonConvert.DeserializeObject<CrawlerSetting>(json);
            }

            var container = AutofacConfig.Register(x => x.RegisterInstance(config));

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                app.Run();
            }

            Console.ReadKey();
        }

        static void Backup()
        {
            CookieContainer cookiecontainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();

            string cookieStr = DoubanConfig.UserCookie;

            string[] cookies = cookieStr.Split(';');
            foreach (string cookie in cookies)
                cookiecontainer.SetCookies(new Uri("http://www.douban.com"), cookie);

            handler.CookieContainer = cookiecontainer;
            handler.UseCookies = true;

            HttpClient client = new HttpClient(handler);
            //var formContent = new FormUrlEncodedContent(
            //    new[]{
            //    new KeyValuePair<string, string>("form_email", "xxx"),
            //    new KeyValuePair<string, string>("form_password", "xxxx"),
            //    new KeyValuePair<string, string>("login", "登录")
            //});

            //var uri = new Uri("https://accounts.douban.com/login");
            //var response = client.PostAsync(uri, formContent);

            //var content = response.Result.Content;

            //cookies.GetCookieHeader(uri);
            //var responseCookies = cookies.GetCookies(uri).Cast<Cookie>().ToList(); 
            //client.BaseAddress = new Uri("www.douban.com");
            client.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");

            //string pageSource = HttpClientHelper.GetPageSource("https://www.douban.com/doumail/57161284/");
            //var r1 = client.GetAsync("https://www.douban.com/doumail/157482762/");

            //HttpClientHelper.CleanPageSource(pageSource);

            //var r1Content = r1.Result.Content.ReadAsStringAsync().Result;
            //var c = HttpUtility.HtmlDecode(r1Content);
            //response.Result.Content

            //var r2 = client.GetAsync("http://www.douban.com/accounts/");
            //var r2Content = r2.Result.Content.ReadAsStringAsync().Result;

            //group 
            //SearchGroup.CrawlerGroup();
        }

    }
}
