using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb;
using SiteMapGen;

namespace TestTaskUkad.Controllers
{
    public class SitemapController : Controller
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IDocumentCollection<SiteMap> _sitemaps;

        public SitemapController(IHostingEnvironment hostingEnvironment, IDocumentCollection<SiteMap> sitemaps)
        {
            _hostingEnvironment = hostingEnvironment;
            _sitemaps = sitemaps;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var docs = _sitemaps.Documents;
            ViewBag.Sitemaps = docs.Count() != 0 ? docs.Select(d => d.Data.Url).ToArray() : null;
            return View();
        }

        // GET: /<controller>/GetByUrl/<siteUrl>
        [HttpGet]
        public IActionResult GetByUrl(string siteUrl)
        {
            ViewBag.SiteUrl = siteUrl;
            return View();
        }

        // POST: /<controller>/Generate/<siteUrl>
        [HttpPost]
        public async Task<IActionResult> Generate(string siteUrl)
        {
            try
            {
                var url = new Uri(siteUrl);
                var sitemap = new SiteMap();
                var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
                if (doc == null)
                {
                    doc = _sitemaps.New();
                    await sitemap.GenerateAsync(url);
                    await doc.SyncUpdate(sitemap);
                }
                return RedirectToAction("GetByUrl", new { siteUrl = siteUrl });
            }
            catch(UriFormatException)
            {
                TempData["ErrorMessage"] = "Invalid URL!";
                return RedirectToAction("Index");
            }
        }

        // GET: /<controller>/Download/<siteUrl>
        [HttpGet]
        public IActionResult Download(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return NotFound();

            string fpath = Path.Combine(_hostingEnvironment.WebRootPath, "files", "sitemap.xml");
            doc.Data.GenerateXmlAsync(fpath);

            return File("~/files/sitemap.xml", "application/xml");
        }
    }
}
