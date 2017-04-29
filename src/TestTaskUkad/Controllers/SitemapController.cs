using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb;
using TestTaskUkad.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TestTaskUkad.Controllers
{
    public class SitemapController : Controller
    {
        readonly IDocumentCollection<SiteMap> _sitemaps;

        public SitemapController(IDocumentCollection<SiteMap> sitemaps)
        {
            _sitemaps = sitemaps;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {      
            return View();
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<IActionResult> Generate(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var sitemap = new SiteMap();
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
            {
                doc = _sitemaps.New();
                await sitemap.Generate(url);
                await doc.SyncUpdate(sitemap);
            }
            else
                sitemap = doc.Data;
            
            return View(sitemap);
        }


        // GET: /<controller>/<url>
        //[HttpGet]
        //[Route("api/[controller]/Dowmload")]
        [Produces("application/xml")]
        public SiteMap Download(string siteUrl)
        {
            var url = new Uri(siteUrl);
            var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url.AbsoluteUri);
            if (doc == null)
                return null;

            return doc.Data;
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public IEnumerable<SiteMap> Get()
        {
            return _sitemaps.Documents.Select(d => d.Data);
        }

        // GET: api/<controller>/5
        [HttpGet]
        [Route("api/[controller]/{url?}")]
        [Produces("application/json")]
        public SiteMap Get(string url)
        {
            var sm = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url);
            return sm != null ? sm.Data : null;
        }

        // POST: api/<controller>
        //[HttpPost]
        //[Route("api/[controller]/")]
        //[Produces("application/json")]
        //public async Task Generate([FromBody] string url)
        //{
        //    //url = "http://kinogo.club/";
        //    //var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url.AbsoluteUri == url);
        //    //if (doc != null)
        //    //    return;
        //    //doc = _sitemaps.New();
        //    //var sm = new SiteMap(url);
            
        //    //await doc.SyncUpdate(sm);
        //}

        // POST: api/<controller>
        [HttpPut]
        [Route("api/[controller]/{url?}")]
        [Produces("application/json")]
        public async Task Post([FromBody] SiteMap sitemap)
        {
            //var doc = _sitemaps.Documents.FirstOrDefault(d => d.Data.Url == url);
            //if (doc == null)
            //    doc = _sitemaps.New();          
            //var sm = doc.Data.Url != null ? doc.Data : new SiteMap(url);             
            //await sm.MapRequest();
            //await doc.SyncUpdate(sm);      
        }

    }
}
