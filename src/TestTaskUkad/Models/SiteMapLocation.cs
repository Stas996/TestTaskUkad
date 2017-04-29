using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            //set
            //{
            //    _url = value;
            //    _stringUrl = _url.AbsoluteUri;
            //}
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
    }
}
