using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using HtmlAgilityPack;

namespace SiteMapGen
{
    /// <summary>
    /// Html parser, that get all urls from document
    /// </summary>
    public static class Crawler
    {

        /// <summary>
        /// Gets the urls from HTML.
        /// </summary>
        /// <param name="url">The URL of site.</param>
        /// <returns></returns>
        public static IEnumerable<Uri> GetUrlsFromHtml(string url)
        {
            return GetUrlsFromHtml(new Uri(url));
        }

        /// <summary>
        /// Gets the urls from HTML.
        /// </summary>
        /// <param name="url">The URL of site.</param>
        /// <returns></returns>
        public static IEnumerable<Uri> GetUrlsFromHtml(Uri url)
        {
            var client = new HttpClient();
            var tresponse = client.GetAsync(url);      //request to url
            tresponse.Wait();
            var tcontent = tresponse.Result.Content.ReadAsStringAsync();
            tcontent.Wait();

            HtmlDocument doc = new HtmlDocument();      //create html doc by url
            doc.LoadHtml(tcontent.Result);

            var xpath = new StringBuilder("//a[@href")
                .Append(" and not(@href='')")
                .Append(" and not(contains(@href,'#'))")
                .Append("]").ToString();

            try
            {
                var urls = doc.DocumentNode.SelectNodes(xpath)       //get all urls
                          .Select(p => p.Attributes["href"].Value)
                          .Distinct()
                          .Select(p =>
                          {
                              return (p[0] == '/' || p[0] == '?') ?
                                        new Uri(url, p) :   //if relative url
                                        new Uri(p);     //if absolute url
                          });

                return urls;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
