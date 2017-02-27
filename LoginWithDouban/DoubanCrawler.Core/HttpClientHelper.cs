using DoubanCrawler.Core.DbGroup;
using DoubanCrawler.Core.Mq;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoubanCrawler.Core
{
    public interface IHttpClientHelper : IDependency
    {
        string GetPageSource(string url);
        string CleanPageSource(string source);
        string CleanPageSourceForGroup(string source);
    }

    public class HttpClientHelper : IHttpClientHelper
    {
        private CrawlerSetting _setting;
        private IMqService _mqService;
        public HttpClientHelper(IMqService mqService, CrawlerSetting setting)
        {
            _mqService = mqService;
            _setting = setting;
        }
        public void GetAsync(string url)
        {
            string cookieStr = DoubanConfig.UserCookie;
            CookieContainer cookiecontainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            string[] cookies = cookieStr.Split(';');
            foreach (string cookie in cookies)
            {
                cookiecontainer.SetCookies(new Uri("http://www.douban.com"), cookie);
            }

            handler.CookieContainer = cookiecontainer;
            handler.UseCookies = true;
            using (HttpClient client = new HttpClient(handler))
            {
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

                var r1 = client.GetAsync("https://www.douban.com/doumail/157482762/");
                var r1Content = r1.Result.Content.ReadAsStringAsync().Result;
            }

        }

        public string GetPageSource(string url)
        {
            string cookieStr = DoubanConfig.UserCookie;
            //string cookieStr = "";
            CookieContainer cookiecontainer = new CookieContainer();

            string[] cookies = cookieStr.Split(';');
            foreach (string cookie in cookies)
                cookiecontainer.SetCookies(new Uri("https://www.douban.com"), cookie);


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

        public string CleanPageSource(string source)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(source);

            HtmlNode node = doc.GetElementbyId("content");
            var content = doc.DocumentNode.ClassToFind("doumail-bd").FirstOrDefault();

            if (content == null)
            {
                return "";
            }
            var chatNodes = content.SelectNodes("//div[contains(@class,'chat')]");
            foreach (var item in chatNodes)
            {

            }
            return "";
        }

        public string CleanPageSourceForGroup(string source)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(source);

            //HtmlNode node = doc.GetElementbyId("content");
            var content = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'olt')]");

            if (content == null)
            {
                return "";
            }
            var trNodes = content.SelectNodes("tr");
            int index = 0;
            foreach (var trItem in trNodes)
            {
                if (index == 0)
                {
                    index++;
                    continue;
                }

                index++;

                //to rabbitmq
                Topic topic = new Topic();
                var tdNodes = trItem.SelectNodes("td");

                if (tdNodes == null || tdNodes.Count <= 0)
                {
                    continue;
                }

                var titleNode = tdNodes[0];

                var lnk = titleNode.SelectSingleNode("a[@href]");
                topic.TopicUrl = lnk.Attributes["href"].Value;
                topic.Title = lnk.InnerText;

                var authorNode = tdNodes[1];
                var lnkAuthor = authorNode.SelectSingleNode("a[@href]");
                topic.AuthorHomeUrl = lnkAuthor.Attributes["href"].Value;
                topic.Author = lnkAuthor.InnerText;

                var timeNode = tdNodes[3];
                string timestr = timeNode.InnerText;
                var timeArray = timestr.Split(' ');
                if (timeArray[0].Length <= 5)
                {
                    timestr = DateTime.Now.Year + "-" + timestr;
                }
                topic.CreateDate = Convert.ToDateTime(timestr);

                var message = JsonConvert.SerializeObject(topic);

                Filter(topic);
                //File.AppendAllText()
                //_mqService.Produce(message);
            }
            //doc.lo
            return "done";
        }

        private void Filter(Topic topic)
        {
            if (topic.AuthorHomeUrl.Contains(_setting.PeopleId))
            {
                Console.ForegroundColor = ConsoleColor.Red; //设置前景色，即字体颜色
                Console.WriteLine("Find Sth::" + topic.Title);
                Console.ResetColor();
                File.AppendAllText(Environment.CurrentDirectory + "/GroupInfo.txt", JsonConvert.SerializeObject(topic));

            }
        }
    }
}
