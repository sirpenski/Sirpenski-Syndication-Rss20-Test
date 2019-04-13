using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicationRssTest.Models
{
    public class RssFeed
    {
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";

        public RssFeed() { }

        public RssFeed(string title, string url)
        {
            Title = title;
            Url = url;
        }

    }
}
