using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicationRssTest.Models
{
    public static class RssFeeds
    {

        public static List<RssFeed> Feeds = new List<RssFeed>();


        public static void Init()
        {
            af("NASA - On the Station - Latest News", "http://www.nasa.gov/rss/dyn/onthestation_rss.rss");
            af("CNN Top Stories", "http://rss.cnn.com/rss/edition.rss");
            af("FOX News Latest", "http://feeds.foxnews.com/foxnews/latest");
            af("Google News", "https://news.google.com/news/rss");
            af("The Hill", "https://thehill.com/rss/syndicator/19110");
            af("NY Times", "https://www.nytimes.com/services/xml/rss/nyt/HomePage.xml");

            af("NASA - Image Of The Day", "https://www.nasa.gov/rss/dyn/lg_image_of_the_day.rss");
            af("NASA - Breaking News", "https://www.nasa.gov/rss/dyn/breaking_news.rss");
            af("NASA - International Space Station Reports", "https://blogs.nasa.gov/stationreport/feed/");

        }

        private static void af(string title, string url)
        {
            RssFeed feed = new RssFeed();
            feed.Title = title;
            feed.Url = url;
            Feeds.Add(feed);

        }


    }
}
