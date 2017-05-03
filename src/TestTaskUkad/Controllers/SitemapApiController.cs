using System;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb;
using SiteMapGen;

namespace TestTaskUkad.Controllers
{
    [Route("api/Sitemap")]
    public class SitemapApiController : Controller
    {
        readonly IDocumentCollection<SiteMap> _sitemaps;

        public SitemapApiController(IDocumentCollection<SiteMap> sitemaps)
        {
            _sitemaps = sitemaps;
        }

        // GET api/Sitemap/?siteUrl=""
        [HttpGet]
        [Route("{siteUrl?}")]
        [Produces("application/json")]
        public SiteMap Get(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return null;

            var sitemap = doc.Data;
            return sitemap;
        }

        // GET api/Sitemap/Time/?siteUrl=""
        [HttpGet]
        [Route("Time/{siteUrl?}")]
        [Produces("application/json")]
        public async Task<int> GetTime(string siteUrl)
        {
            var stopWatch = Stopwatch.StartNew();
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(siteUrl);
                return stopWatch.Elapsed.Milliseconds;
            }
        }
        
        // GET api/Sitemap/?siteUrl=""
        [HttpPut]
        [Route("{siteUrl?}")]
        [Produces("application/json")]
        public async Task Update(string siteUrl, [FromBody] SiteMap data)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return;

            await doc.SyncUpdate(data);
        }
    }
}
