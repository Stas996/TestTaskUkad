using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb;
using TestTaskUkad.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return NotFound();

            var sitemap = doc.Data;
            return View(sitemap);
        }

        // POST: /<controller>/Generate/<siteUrl>
        [HttpPost]
        public async Task<IActionResult> Generate(string siteUrl)
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
            else
                sitemap = doc.Data;
            
            return View("GetByUrl", sitemap);
        }

        // POST: /<controller>/GetRequestDiagnostic/<siteUrl>
        [HttpPost]
        public async Task<IActionResult> RequestDiagnostic(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return NotFound();

            var sitemap = doc.Data;
            await sitemap.MapRequest();
            await doc.SyncUpdate(sitemap);

            return View("GetByUrl", sitemap);
        }

        // GET: /<controller>/Download/<siteUrl>
        [HttpGet]
        public async Task<IActionResult> Download(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return NotFound();

            string fpath = Path.Combine(_hostingEnvironment.WebRootPath, "files", "sitemap.xml");
            await doc.Data.GenerateXmlAsync(fpath);

            return File("~/files/sitemap.xml", "application/xml");
        }

        // GET: /<controller>/GetByUrl/<siteUrl>
        [HttpGet]
        public IActionResult GetJsonByUrl(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return NotFound();

            var sitemap = doc.Data;
            return Json(sitemap);
        }

    }
}
