using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestTaskUkad.Models
{
    public enum eChangeFrequency
    {
        always,
        hourly,
        daily,
        weekly,
        monthly,
        yearly,
        never
    }

    public class SiteMapLocation
    {
        string _stringUrl;
        Uri _url;

        public SiteMapLocation()
        {
            RequestsTimeLog = new List<int>();
        }

        [XmlElement("loc")]
        public string StringUrl {
            get
            {
                return _stringUrl;
            }
            set
            {
                _stringUrl = value;
            }
        }

        [XmlIgnore]
        public Uri Url {
            get
            {
                if (!_stringUrl.Contains("http") || _stringUrl == null)
                    return null;
                return (_url == null) ? new Uri(_stringUrl) : _url;
            }
        }

        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        [XmlElement("lastmod")]
        public DateTime? LastModified { get; set; }
        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

        [XmlElement("priority")]
        public double? Priority { get; set; }
        public bool ShouldSerializePriority() { return Priority.HasValue; }

        [XmlIgnore]
        public ICollection<int> RequestsTimeLog { get; set; }


        public async Task<int> GetRequestTimeAsync()
        {
            var stopWatch = Stopwatch.StartNew();
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(Url);
                return stopWatch.Elapsed.Milliseconds;
            }
        }
    }
}
