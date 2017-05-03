using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiteMapGen
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

    /// <summary>
    /// Location from sitemap
    /// </summary>
    public class SiteMapLocation
    {
        string _stringUrl;
        Uri _url;

        public SiteMapLocation()
        {
            RequestsTimeLog = new List<int>();
        }


        /// <summary>
        /// Gets or sets the string URL.
        /// </summary>
        /// <value>
        /// The string URL.
        /// </value>
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

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [XmlIgnore]
        public Uri Url {
            get
            {
                if (!_stringUrl.Contains("http") || _stringUrl == null)
                    return null;
                return (_url == null) ? new Uri(_stringUrl) : _url;
            }
        }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>
        /// The change frequency.
        /// </value>
        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>
        /// The last modified.
        /// </value>
        [XmlElement("lastmod")]
        public DateTime? LastModified { get; set; }
        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        [XmlElement("priority")]
        public double? Priority { get; set; }
        public bool ShouldSerializePriority() { return Priority.HasValue; }

        /// <summary>
        /// Gets or sets the requests time log collection.
        /// </summary>
        /// <value>
        /// The requests time log collection.
        /// </value>
        [XmlIgnore]
        public ICollection<int> RequestsTimeLog { get; set; }

        /// <summary>
        /// Gets the average request time of current location.
        /// </summary>
        /// <value>
        /// The average request time.
        /// </value>
        [XmlIgnore]
        public double AverageRequestTime {
            get {
                if (RequestsTimeLog == null || RequestsTimeLog.Count == 0)
                    return 0;
                return RequestsTimeLog.Sum() / RequestsTimeLog.Count;
            }
        }

        /// <summary>
        /// Gets the maximum request time of current location.
        /// </summary>
        /// <value>
        /// The maximum request time.
        /// </value>
        [XmlIgnore]
        public double MaxRequestTime
        {
            get
            {
                if (RequestsTimeLog == null || RequestsTimeLog.Count == 0)
                    return 0;
                return RequestsTimeLog.Max();
            }
        }

        /// <summary>
        /// Gets the minimum request time of current.
        /// </summary>
        /// <value>
        /// The minimum request time.
        /// </value>
        [XmlIgnore]
        public double MinRequestTime
        {
            get
            {
                if (RequestsTimeLog == null || RequestsTimeLog.Count == 0)
                    return 0;
                return RequestsTimeLog.Min();
            }
        }
    }
}
