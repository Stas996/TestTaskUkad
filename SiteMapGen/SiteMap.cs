using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;

namespace SiteMapGen
{
    /// <summary>
    /// Sitemap generator
    /// </summary>
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SiteMap
    {
        Uri _url;
        int? _diagnosticOrder = null;

        /// <summary>
        /// Url of main page
        /// </summary>
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

        /// <summary>
        /// All urls from sitemap
        /// </summary>
        [XmlElement("url")]
        public SiteMapLocation [] Map { get; set; }

        /// <summary>
        /// Gets the diagnostic order of Map. If not all urls have diagnostic, return smallest length of RequestsTimeLog
        /// </summary>
        [XmlIgnore]
        public int? DiagnosticOrder
        {
            get
            {
                return (_diagnosticOrder == null) ? GetDiagnosticOrder() : _diagnosticOrder;
            }
        }

        public SiteMap()
        {
            
        }

        /// <summary>
        /// Generates sitemap asynchronously.
        /// </summary>
        /// <param name="url">The URL of site</param>
        /// <returns></returns>
        public async Task GenerateAsync(string url)
        {
            await GenerateAsync(new Uri(url));
        }

        /// <summary>
        /// Generates sitemap asynchronously.
        /// </summary>
        /// <param name="url">The URL of site</param>
        /// <returns></returns>
        public async Task GenerateAsync(Uri url)
        {
            Url = url;
            //if site contains file, deserialize it
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(Url.AbsoluteUri + "sitemap.xml");
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SiteMap));
                    var o = (SiteMap)ser.Deserialize(stream);
                    Map = o.Map;
                }
            }
            //else, parse document
            catch
            {
                Map = Crawler.GetUrlsFromHtml(Url)
                     .Where(u => u.AbsoluteUri.Contains(Url.Host) && u.Segments.Count() < 3     //if url contain host name and within the starting directory will be included
                     && !u.AbsoluteUri.ContainsAny(".jpg", ".png", "mailto:"))                  //and dont have ".jpg", ".png", "mailto:", add it to collection
                     .Select(u => new SiteMapLocation() { StringUrl = u.AbsoluteUri }).ToArray();
            }
        }

        /// <summary>
        /// Generates sitemap.xml asynchronously.
        /// </summary>
        /// <param name="path">The path to file.</param>
        public void GenerateXmlAsync(string path)
        {

            XmlSerializer ser = new XmlSerializer(typeof(SiteMap));
            using (var fs = new FileStream(path, FileMode.Create))
            {
                ser.Serialize(fs, this);
            }
        }

        /// <summary>
        /// Gets the diagnostic order of Map. If not all urls have diagnostic, return smallest length of RequestsTimeLog
        /// </summary>
        /// <returns></returns>
        public int GetDiagnosticOrder()
        {
            if (Map == null || Map.Count() == 0)
                return 0;
            for (int i = 0; i < Map.Count() - 1; i++)
            {
                int c1 = Map[i].RequestsTimeLog.Count;
                int c2 = Map[i + 1].RequestsTimeLog.Count;
                if (c1 != c2)
                    return (c1 < c2) ? c1 : c2;
            }
            return Map[0].RequestsTimeLog.Count;
        }
    }
}