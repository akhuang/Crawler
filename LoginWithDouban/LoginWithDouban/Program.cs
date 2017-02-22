using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoginWithDouban
{
    class Program
    {
        static void Main(string[] args)
        {
            CookieContainer cookiecontainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();

            string cookieStr = "bid=XoXs521sbzY; __utma=30149280.510386763.1478350594.1487680519.1487682619.3; __utmz=30149280.1478350594.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); ll=118282; __utmb=30149280.20.5.1487682730328; __utmt=1; push_noty_num=0; push_doumail_num=0; __utmv=30149280.9291; _vwo_uuid_v2=1AD03B168B9E17DA2D623066C0B97206|1fac2e1e2c70c153b1e206ee56eb1d78; __utmc=30149280; dbcl2=92912674:V4c1GtCEYcE; ck=qQQy; _pk_id.100001.8cb4=ca7086b5b18428f4.1478350577.3.1487682729.1487680516.; _pk_ses.100001.8cb4=*";

            string[] cookies = cookieStr.Split(';');
            foreach (string cookie in cookies)
                cookiecontainer.SetCookies(new Uri("http://www.douban.com"), cookie);

            handler.CookieContainer = cookiecontainer;
            handler.UseCookies = true;

            HttpClient client = new HttpClient(handler);
            var formContent = new FormUrlEncodedContent(
                new[]{
                new KeyValuePair<string, string>("form_email", "phoenix.fy24h@gmail.com"),
                new KeyValuePair<string, string>("form_password", "050195@hf"),
                new KeyValuePair<string, string>("login", "登录")
            });

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

            string pageSource = GetWebText("https://www.douban.com/doumail/157482762/");
            //var r1 = client.GetAsync("https://www.douban.com/doumail/157482762/");

            CleanPageSource(pageSource);

            //var r1Content = r1.Result.Content.ReadAsStringAsync().Result;
            //var c = HttpUtility.HtmlDecode(r1Content);
            //response.Result.Content

            //var r2 = client.GetAsync("http://www.douban.com/accounts/");
            //var r2Content = r2.Result.Content.ReadAsStringAsync().Result;

            Console.ReadKey();
        }

        private static string CleanPageSource(string source)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(source);

            HtmlNode node = doc.GetElementbyId("content");
            var singleNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'doumail-bd')]");
            //doc.lo
            return "";
        }

        private static string GetWebText(string url)
        {
            CookieContainer cookiecontainer = new CookieContainer();
            string cookieStr = "bid=XoXs521sbzY; __utma=30149280.510386763.1478350594.1487680519.1487682619.3; __utmz=30149280.1478350594.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); ll=118282; __utmb=30149280.20.5.1487682730328; __utmt=1; push_noty_num=0; push_doumail_num=0; __utmv=30149280.9291; _vwo_uuid_v2=1AD03B168B9E17DA2D623066C0B97206|1fac2e1e2c70c153b1e206ee56eb1d78; __utmc=30149280; dbcl2=92912674:V4c1GtCEYcE; ck=qQQy; _pk_id.100001.8cb4=ca7086b5b18428f4.1478350577.3.1487682729.1487680516.; _pk_ses.100001.8cb4=*";

            string[] cookies = cookieStr.Split(';');
            foreach (string cookie in cookies)
                cookiecontainer.SetCookies(new Uri("http://www.douban.com"), cookie);


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.CookieContainer = cookiecontainer;
            //request.
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string htmlText = reader.ReadToEnd();
            return htmlText;
        }
    }
}
