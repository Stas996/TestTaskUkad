using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
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

        public async Task GenerateAsync(string url)
        {
            await GenerateAsync(new Uri(url));
        }

        public async Task GenerateAsync(Uri url)
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
                     && !u.AbsoluteUri.ContainsAny(".jpg", ".png", "mailto:"))
                     .Select(u => new SiteMapLocation() { StringUrl = u.AbsoluteUri }).ToArray();
            }
        }

        public async Task GenerateXmlAsync(string path)
        {

            Mocoding.EasyDocDb.Xml.XmlSerializer ser = new Mocoding.EasyDocDb.Xml.XmlSerializer();
            using (var sw = new StreamWriter(path))
            {
                await sw.WriteLineAsync(ser.Serialize(this));
            }
        }

        public async Task MapRequest()
        {
            if (Map == null)
                return;
            var tasks = new List<Task<int>>();
            var map = Map.Count() > 300 ? Map.Take(300) : Map;

            foreach (var u in Map)
                tasks.Add(u.GetRequestTimeAsync());

            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Count; i++)
                Map[i].RequestsTimeLog.Add(tasks[i].Result);
        }

    }
}