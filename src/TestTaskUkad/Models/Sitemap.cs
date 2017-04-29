using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using Mocoding.EasyDocDb.Xml;

namespace TestTaskUkad.Models
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SiteMap
    {
        Uri _url;

        [XmlIgnore]
        public Uri Url {
            get
            {
                if(_url == null)
                    return (Map == null) ? null : Map.FirstOrDefault(m => m.Url.LocalPath == "/").Url;
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        [XmlElement("url")]
        public SiteMapLocation [] Map { get; set; }

        public SiteMap()
        {
            
        }

        public async Task Generate(string url)
        {
            await Generate(new Uri(url));
        }

        public async Task Generate(Uri url)
        {
            Url = url;
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(Url.AbsoluteUri + "sitemap.xml");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Mocoding.EasyDocDb.Xml.XmlSerializer ser = new Mocoding.EasyDocDb.Xml.XmlSerializer();
                var o = ser.Deserialize<SiteMap>(content);
                Map = o.Map;
            }
            else
            {
                Map = Crawler.GetUrlsFromHtml(Url)
                     .Where(u => u.AbsoluteUri.Contains(Url.Host) && u.Segments.Count() < 3
                     && !u.AbsoluteUri.ContainsAny(".jpg", ".png", "mailto:", "/page/", "/pages/"))
                     .Select(u => new SiteMapLocation() { StringUrl = u.AbsoluteUri }).ToArray();
            }
        }

        public async Task MapRequest()
        {
            var tasks = new List<Task<int>>();

            //foreach (var u in Map)
               // tasks.Add(GetHttpWithTimingInfo(u.AbsoluteUri));

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task<int> GetHttpWithTimingInfo(string url)
        {
            var stopWatch = Stopwatch.StartNew();
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
                return stopWatch.Elapsed.Milliseconds;
            }
        }
    }
}