using System;
using System.Collections.Generic;
namespace TestTaskUkad.Models
{
    public class PageInfo
    {
        public PageInfo(string url)
        {
            Url = url;
            RespondTimes = new List<int>();
        }
        public string Url { get; set; }
        public IEnumerable<int> RespondTimes { get; set; }
    }
}