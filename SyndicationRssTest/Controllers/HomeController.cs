using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyndicationRssTest.Models;
using Sirpenski.Syndication.Rss20;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SyndicationRssTest.Controllers
{
    public class HomeController : Controller
    {

        #region Methods, properties that are not changing




        // ****************************************************************************
        // DEFINE TESTS HERE
        // *****************************************************************************

        // test ids
        const int TEST_CREATE_CHANNEL_USING_OBJECTS = 1;
        const int TEST_CREATE_CHANNEL_USING_HELPER_METHODS = 2;
        const int TEST_CREATE_ONE_CORE_ITEM = 3;
        const int TEST_CREATE_ONE_CORE_ITEM_USING_HELPER_METHODS = 4;
        const int TEST_CREATE_CHANNEL_WITH_EXT_PROPS_USING_OBJECTS = 5;
        const int TEST_CREATE_CHANNEL_WITH_EXT_PROPS_USING_HELPER_METHODS = 6;
        const int TEST_CREATE_CHANNEL_WITH_EXT_PROPS_MEDIA_USING_OBJECTS = 7;
        const int TEST_CREATE_CHANNEL_WITH_EXT_PROPS_MEDIA_USING_HELPER_METHODS = 8;
        const int TEST_CREATE_CHANNEL_WITH_EXT_PLUS_ONE_EXT_ITEM_USING_OBJECTS = 9;
        const int TEST_CREATE_CHANNEL_WITH_EXT_PLUS_ONE_EXT_ITEM_WITH_MEDIA_USING_OBJECTS = 11;
        const int TEST_CREATE_ONE_ITEM_WITH_MEDIA_CONTENT = 13;
        const int TEST_CREATE_ONE_ITEM_WITH_MEDIA_CONTENT_AND_MEDIA_OPTIONS = 15;
        const int TEST_CREATE_ONE_ITEM_WITH_MEDIA_GROUP = 17;
        const int TEST_CREATE_ONE_ITEM_WITH_MEDIA_GROUP_WITH_MEDIA_OPTIONS = 19;



        // define the tests
        List<TestItem> lstTests = new List<TestItem>();

        // session variables
        PFSXSession<UserSessionVariables> session;


        /// <summary>
        /// Called Every Entry Into Controller
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)

        {


            // define a new user session
            session = new PFSXSession<UserSessionVariables>(context.HttpContext);

            // now load the user session
            session.Load(AutoInitialize: true);


            // execute the base.
            base.OnActionExecuting(context);


        }



        /// <summary>
        /// Helper Routine
        /// </summary>
        /// <param name="id">Test ID</param>
        /// <param name="title">Test Title</param>
        /// <param name="desc">Description</param>
        private void addTest(int id, string title, string desc)
        {
            TestItem obj = new TestItem(id, title, desc);
            lstTests.Add(obj);
        }




        /// <summary>
        /// Adds tests to the creates
        /// </summary>
        private void AddTests()
        {
            lstTests = new List<TestItem>();
            addTest(TEST_CREATE_CHANNEL_USING_OBJECTS, "Create Channel (Core Properties Only) Using Objects", "This test creates a channel using objects.  It does not add any items");

            addTest(TEST_CREATE_CHANNEL_USING_HELPER_METHODS, "Create Channel (Core Properties Only) Using Helper Methods",
                    "This test creates a simple channel only uisng the helper methods.  It does not add any items");

            addTest(TEST_CREATE_ONE_CORE_ITEM, "Create Channel With One Item (Core Properties Only)", "Creates a channel with a single core item");

            addTest(TEST_CREATE_ONE_CORE_ITEM_USING_HELPER_METHODS, "Creates Channel With One Item Using Helper Methods (Core Properties Only)",
                "Creates Channel With One Item (Core Properties Only) Using Helper Methods");

            addTest(TEST_CREATE_CHANNEL_WITH_EXT_PROPS_USING_OBJECTS, "Creates Channel With Ext Props Using Objects (No Media Options)",
                    "Creates Channel With Ext Props Using Objects (No Media)");


            addTest(TEST_CREATE_CHANNEL_WITH_EXT_PROPS_USING_HELPER_METHODS, "Creates Channel With Ext Props Using Helper Methods (No Media Options)",
                    "Creates Channel With Ext Props Using Helper Methods (No Media)");


            addTest(TEST_CREATE_CHANNEL_WITH_EXT_PROPS_MEDIA_USING_OBJECTS, "Creates Channel With Ext Props And Media Using Objects",
                    "Creates Channel With Ext Props And Media Using Objects");

            addTest(TEST_CREATE_CHANNEL_WITH_EXT_PROPS_MEDIA_USING_HELPER_METHODS, "Creates Channel With Ext Props And Media Using Helper Methods",
                    "Creates Channel With Ext Props And Media Using Helper Methods");


            addTest(TEST_CREATE_CHANNEL_WITH_EXT_PLUS_ONE_EXT_ITEM_USING_OBJECTS, "Creates Extended Channel With One Extended Item (No Media) Using Objects",
                    "Creates Extended Channel With One Extended Item(No Media) Using Objects");

            addTest(TEST_CREATE_CHANNEL_WITH_EXT_PLUS_ONE_EXT_ITEM_WITH_MEDIA_USING_OBJECTS, "Creates Extended Channel With One Extended Item With Media Using Objects",
                    "Creates Extended Channel With One Extended Item With Media Using Objects");

            addTest(TEST_CREATE_ONE_ITEM_WITH_MEDIA_CONTENT, "Create Channel With One Item With Media Content", "Creates Channel With One Item With Media Content");

            addTest(TEST_CREATE_ONE_ITEM_WITH_MEDIA_CONTENT_AND_MEDIA_OPTIONS, "Create Channel With One Item With Media Content And Media Options",
                "Creates Channel With One Item With Media Content And Media Options.");

            addTest(TEST_CREATE_ONE_ITEM_WITH_MEDIA_GROUP, "Create Channel With One Item With One Media Group",
                "Create Channel With One Item With One Media Group");

            addTest(TEST_CREATE_ONE_ITEM_WITH_MEDIA_GROUP_WITH_MEDIA_OPTIONS, "Create Channel With One Item With One Media Group With Media Options",
                "Create Channel With One Item With One Media Group With Media Options");


        }








        /// <summary>
        /// Default Method
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }




        /// <summary>
        /// Displays the Read Feed page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ReadFeed()
        {
            ViewData["SESSION"] = session;
            return View();
        }


        /// <summary>
        /// ReadFeedSubmit handles the submission
        /// </summary>
        /// <param name="COMBOBOX_FEED_URL"></param>
        /// <param name="OUTPUT_FORMAT"></param>
        /// <returns></returns>

        public async Task<IActionResult> ReadFeedSubmit(string COMBOBOX_FEED_URL, Nullable<int>OUTPUT_FORMAT)
        {

            IActionResult rslt = new BadRequestResult();

            int nOutputFormat = 0;
            if (OUTPUT_FORMAT.HasValue)
            {
                nOutputFormat = Convert.ToInt32(OUTPUT_FORMAT);
            }

            if (!string.IsNullOrEmpty(COMBOBOX_FEED_URL))
            {
                string url = COMBOBOX_FEED_URL.Trim();
                if (url.Length > 0)
                {
                    
      

                    // create a new feed
                    Rss20 rss = new Rss20();
                    rss.PersistResponseXml = true;
                    rss.SortItemsOnLoad = true;
                    HttpStatusCode r = await rss.LoadUrl(url);


                    // set the session variables
                    session.SessionVariables.LastReadUrl = url;
                    session.SessionVariables.LastReadOutputFormat = nOutputFormat;
                    session.Save();


                    if (rss.ResponseStatusCode == HttpStatusCode.OK)
                    {

                        switch (nOutputFormat)
                        {
                            case 0:
                                rslt = View("~/Views/Home/ReadFeedDisplay.cshtml", rss);
                                break;

                            case 1:
                                rslt = Content(rss.GetXml(), "application/xml");
                                break;

                            case 2:
                                rslt = Content(rss.ResponseXml, "application/xml");
                                break;

                            case 3:
                                rslt = Content(rss.ResponseRawXml, "application/xml");
                                break;


                            case 4:
                                string json = JsonConvert.SerializeObject(rss, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                                rslt = Content(json, "application/json");
                                break;



                        }   // switch

                    }

                }
            }

            return rslt;



        }


        /// <summary>
        /// Creates feed tests
        /// </summary>
        /// <returns></returns>     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CreateFeed()
        {
            // add the tests, they are defined at the top
            AddTests();


            ViewData["TESTS"] = lstTests;
            ViewData["SESSION"] = session;
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        #endregion





        /// <summary>
        /// Creates test feeds
        /// </summary>
        /// <param name="TEST_ID"></param>
        /// <param name="OUTPUT_FORMAT"></param>
        /// <returns></returns>

        public IActionResult CreateFeedSubmit(Nullable<int> TEST_ID, Nullable<int> OUTPUT_FORMAT)
        {
            IActionResult rslt = new BadRequestResult();

 


            // if id has test value, proceed
            if (TEST_ID.HasValue)
            {

                // get the int
                int nTestID = Convert.ToInt32(TEST_ID);

                // get the output value
                int nOutputFormat = 0;
                if (OUTPUT_FORMAT.HasValue)
                {
                    nOutputFormat = Convert.ToInt32(OUTPUT_FORMAT);
                }


                // save the session;
                session.SessionVariables.LastCreateTestID = nTestID;
                session.SessionVariables.LastCreateOutputFormat = nOutputFormat;
                session.Save();


                // create a new feed
                Rss20 rss = new Rss20();
                
                // switch the test
                switch(TEST_ID)
                {
                    case TEST_CREATE_CHANNEL_USING_OBJECTS:
                        CreateChannelUsingObjects(rss);
                        break;

                    case TEST_CREATE_CHANNEL_USING_HELPER_METHODS:
                        CreateChannelUsingHelperMethods(rss);
                        break;

                    case TEST_CREATE_ONE_CORE_ITEM:
                        CreateChannelWithOneCoreItem(rss);
                        break;

                    case TEST_CREATE_ONE_CORE_ITEM_USING_HELPER_METHODS:
                        CreateChannelWithOneCoreItemHelperMethods(rss);
                        break;

                    case TEST_CREATE_CHANNEL_WITH_EXT_PROPS_USING_OBJECTS:
                        CreateChannelWithExtendedPropertiesUsingObjects(rss);
                        break;

                    case TEST_CREATE_CHANNEL_WITH_EXT_PROPS_USING_HELPER_METHODS:
                        CreateChannelWithExtendedPropertiesUsingHelperMethods(rss);
                        break;

                    case TEST_CREATE_CHANNEL_WITH_EXT_PROPS_MEDIA_USING_OBJECTS:
                        CreateChannelWithExtendedPropertiesAndMediaUsingObjects(rss);
                        break;

                    case TEST_CREATE_CHANNEL_WITH_EXT_PROPS_MEDIA_USING_HELPER_METHODS:
                        CreateChannelWithExtendedPropertiesAndMediaUsingHelperMethods(rss);
                        break;

                    case TEST_CREATE_CHANNEL_WITH_EXT_PLUS_ONE_EXT_ITEM_USING_OBJECTS:
                        CreateChannelWithExtendedPropertiesAndOneItemWithExtendedPropertiesNoMediaUsingObjects(rss);
                        break;

                    case TEST_CREATE_CHANNEL_WITH_EXT_PLUS_ONE_EXT_ITEM_WITH_MEDIA_USING_OBJECTS:
                        CreateChannelWithExtendedPropertiesAndOneItemWithExtendedPropertiesAndMediaUsingObjeccts(rss);
                        break;

                    case TEST_CREATE_ONE_ITEM_WITH_MEDIA_CONTENT:
                        CreateOneItemWithMediaContent(rss);
                        break;

                    case TEST_CREATE_ONE_ITEM_WITH_MEDIA_CONTENT_AND_MEDIA_OPTIONS:
                        CreateOneItemWithMediaContentAndMediaOptions(rss);
                        break;

                    case TEST_CREATE_ONE_ITEM_WITH_MEDIA_GROUP:
                        CreateOneItemWithMediaGroup(rss);
                        break;

                    case TEST_CREATE_ONE_ITEM_WITH_MEDIA_GROUP_WITH_MEDIA_OPTIONS:
                        CreateOneItemWithMediaGroupAndMediaOptions(rss);
                        break;


                }



                // do the dump
                switch (nOutputFormat)
                {
                    case 0:
                        rslt = Content(rss.GetXml(), "application/xml");
                        break;

                    case 1:
                        string json = JsonConvert.SerializeObject(rss, Formatting.Indented, new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                        rslt = Content(json, "application/json");
                        break;

                    

                }   // switch



            }   // end if not string null or empty
                
            return rslt;
        }







 

 

        // *****************************************************************************
        // TEST CODE BEGINS HERE
        // *****************************************************************************


 


        #region CommonSetCoreChannelPropertiesUsingObjects
        public void CommonSetCoreChannelPropertiesUsingObjects(Rss20 rss, string optionalTitle = "")
        {
            RssChannel chan = rss.channel;

            string default_title = "TEST CHANNEL CREATED WITH HELPER METHODS";
            string title = optionalTitle.Length > 0 ? optionalTitle : default_title;



            chan.title = title;
            chan.link = "http://something.com/rss";
            chan.pubDate = DateTime.Now.ToUniversalTime();
            chan.lastBuildDate = chan.pubDate;
            chan.description = "This is a channel that is created for sirpenski.syndication.rss20 testing purposes";
            chan.language = "en";
            chan.copyright = "Copyright " + DateTime.Today.ToString("yyyy") + ". All Rights Reserved";
            chan.managingEditor = "MangingEditor@something.com";
            chan.webMaster = "webmaster@something.com";

            // add categories
            RssChannelCategory ctg = new RssChannelCategory();
            ctg.category = "TEST";
            ctg.domain = "http://something.com";
            chan.categories.Add(ctg);

            ctg = new RssChannelCategory();
            ctg.category = "RSS";
            ctg.domain = "";
            chan.categories.Add(ctg);

            // feed generator
            chan.generator = "sirpenski.syndication.rss20";

            // documentation url that defines spec for this channel
            chan.docs = "http://something.com/format_of_this_channel";

            // cloud
            RssChannelCloud c = new RssChannelCloud();
            c.domain = "http://something.com";
            c.path = "/path/to/service";
            c.port = 11092;
            c.protocol = "xml-rpc";
            c.registerProcedure = "notifyChannelUpdate";
            chan.cloud.Add(c);

            // time to live
            chan.ttl = 1440;

            // channel icon
            RssChannelImage img = new RssChannelImage();
            img.link = "http://something.com/images/rss/rss_channel_logo_88x88.gif";
            img.title = "Channel Logo";
            img.url = "http://something.com";
            img.width = 88;
            img.height = 88;
            chan.image = img;


            // skip hours ie don't look for updates to channel during these hours
            RssChannelSkipHours skipHours = new RssChannelSkipHours();
            skipHours.Hours.Add(1);
            skipHours.Hours.Add(2);
            skipHours.Hours.Add(3);
            chan.skipHours = skipHours;

            // skip days
            RssChannelSkipDays skipDays = new RssChannelSkipDays();
            skipDays.Days.Add(RssChannelSkipDays.DAY_SATURDAY);
            skipDays.Days.Add(RssChannelSkipDays.DAY_SUNDAY);
            chan.skipDays = skipDays;
        }
        #endregion



        #region CommonSetCoreChannelPropertiesUsingHelperMethods
        public void CommonSetCoreChannelPropertiesUsingHelperMethods(Rss20 rss, string optionalTitle = "")
        {
            RssChannel chan = rss.channel;

            string default_title = "TEST CHANNEL CREATED WITH HELPER METHODS";
            string title = optionalTitle.Length > 0 ? optionalTitle : default_title;



            chan.AddTitle(title);
            chan.AddLink("http://something.com/rss");
            chan.AddPubDate(DateTime.Today);
            chan.AddLastBuildDate(DateTime.Today);

            chan.AddDescription("This is a channel that is created for sirpenski.syndication.rss20 testing purposes");
            chan.AddLanguage("en");
            chan.AddCopyright("Copyright " + DateTime.Today.ToString("yyyy") + ". All Rights Reserved");
            chan.AddManagingEditor("MangingEditor@something.com");
            chan.AddWebMaster("webmaster@something.com");
            chan.AddCategory("TEST", "http://something.com");
            chan.AddCategory("RSS");
            chan.AddGenerator("sirpenski.syndication.rss20");
            chan.AddDocs("http://something.com/format_of_this_channel");
            chan.AddCloud("http://something.com", 11092, "/path/to/service", "notifyChannelUpdate", "xml-rpc");
            chan.AddTtl(1440);

            chan.AddImage("http://something.com/images/rss/rss_channel_logo_88x88.gif", "Channel Icon", "http://something.com", 88, 88);

            chan.AddSkipHour(1);
            chan.AddSkipHour(2);
            chan.AddSkipHour(3);

            chan.AddSkipDay(RssChannelSkipDays.DAY_SATURDAY);
            chan.AddSkipDay(RssChannelSkipDays.DAY_SUNDAY);

        }

        #endregion



        #region CommonSetExtendedChannelPropertiesUsingObjects

        public void CommonSetExtendedChannelPropertiesUsingObjects(Rss20 rss)
        {
            RssAtomLink lnk = new RssAtomLink();
            lnk.href = "http://something.com";
            lnk.hreflang = "en";
            lnk.title = rss.channel.title;
            lnk.type = "application/rss+xml";
            lnk.rel = "self";
            lnk.length = 102293;
            rss.channel.AddAtomLink(lnk);

            RssCreativeCommonsLicense lic = new RssCreativeCommonsLicense();
            lic.license = "http://creativecommons.org/useitall";
            rss.channel.AddCreativeCommonsLicense(lic);


        }


        #endregion



        #region CommonSetExtendedChannelPropertiesUsingHelperMethods
        public void CommonSetExtendedChannelPropertiesUsingHelperMethods(Rss20 rss)
        {
            RssChannel chan = rss.channel;
            chan.AddAtomLink("http://something.com", "self", "application/rss+xml", chan.title, "en", 102293);
            chan.AddCreativeCommonsLicense("http://creativecommons.org/useitall");
        }

        #endregion


        #region CommonSetMediaPropertiesUsingObjects
        public void CommonSetMediaPropertiesUsingObjects(IRssMediaExtension obj)
        {

         

            RssMediaRating mediaRating = new RssMediaRating();
            mediaRating.rating = "NC17";
            mediaRating.scheme = "http://search.yahoo.com/mrss/rating_schema";
            obj.AddMediaRating(mediaRating);

            RssMediaTitle mediaTitle = new RssMediaTitle();
            mediaTitle.title = "Flowers And More";
            mediaTitle.type = "text/html";
            obj.AddMediaTitle(mediaTitle);

            RssMediaDescription mediaDescription = new RssMediaDescription();
            mediaDescription.description = "This is a blog about pretty flowers";
            mediaDescription.type = "plain";
            obj.AddMediaDescription(mediaDescription);

            RssMediaKeywords mediaKeywords = new RssMediaKeywords();
            mediaKeywords.keywords = "Rose, Tulip, Lily";
            obj.AddMediaKeywords(mediaKeywords);

            RssMediaThumbnail mediaThumbnail = new RssMediaThumbnail();
            mediaThumbnail.url = "http://sirpenski.com/images/rss/channellogo.gif";
            mediaThumbnail.width = 60;
            mediaThumbnail.height = 60;
            obj.AddMediaThumbnail(mediaThumbnail);

            RssMediaCategory ctg = new RssMediaCategory();
            ctg.category = "PERENNIAL_FLOWERS";
            ctg.scheme = "http://search.yahoo.com/mrss/category_schema";
            ctg.label = "Perennials";
            obj.AddMediaCategory(ctg);

            ctg = new RssMediaCategory();
            ctg.category = "FLOWERS";
            ctg.scheme = "http://prettythings.com/category_schema";
            ctg.label = "Flowers";
            obj.AddMediaCategory(ctg);

  
            RssMediaHash hash = new RssMediaHash();
            hash.hash = "1902233933030303";
            hash.algo = "SHA256";
            obj.AddMediaHash(hash);

            RssMediaPlayer mediaPlayer = new RssMediaPlayer();
            mediaPlayer.url = "http://www.foo.com/player?id=1111";
            mediaPlayer.height = 200;
            mediaPlayer.width = 400;
            obj.AddMediaPlayer(mediaPlayer);

            RssMediaCredit mediaCredit = new RssMediaCredit();
            mediaCredit.credit = "entity name";
            mediaCredit.role = "producer";
            mediaCredit.scheme = "urn:ebu";
            obj.AddMediaCredit(mediaCredit);

            RssMediaCopyright mediaCopyright = new RssMediaCopyright();
            mediaCopyright.copyright = "Copyright 2019.  ACME Inc.";
            obj.AddMediaCopyright(mediaCopyright);

            RssMediaText mediaText = new RssMediaText();
            mediaText.text = "This is the first chapter";
            mediaText.start = new TimeSpan(0, 0, 20);
            mediaText.end = new TimeSpan(0, 0, 55);
            mediaText.lang = "en";
            mediaText.type = "text/plain";
            obj.AddMediaText(mediaText);

            RssMediaRestriction mediaRestriction = new RssMediaRestriction();
            mediaRestriction.relationship = "allow";
            mediaRestriction.restriction = "au us";
            mediaRestriction.type = "country";
            obj.AddMediaRestriction(mediaRestriction);

            RssMediaCommunity mediaCommunity = new RssMediaCommunity();
            mediaCommunity.starRatingAverage = 3.5;
            mediaCommunity.starRatingCount = 5000;
            mediaCommunity.starRatingMax = 5;
            mediaCommunity.starRatingMin = 0;
            mediaCommunity.statisticsFavorites = 99;
            mediaCommunity.statisticsViews = 2000;
            mediaCommunity.tags = "Weepy, Sleepy, Nod";
            obj.AddMediaCommunity(mediaCommunity);

            RssMediaComments mediaComments = new RssMediaComments();
            mediaComments.Add("Comment 1");
            mediaComments.Add("Comment 2");
            mediaComments.Add("Comment 3");
            obj.AddMediaComments(mediaComments);


            RssMediaEmbed mediaEmbed = new RssMediaEmbed();
            mediaEmbed.url = "http://youtube.com/mediaplayer";
            mediaEmbed.width = 400;
            mediaEmbed.height = 200;
            mediaEmbed.Add("type", "application/x-shockwave-flash");
            mediaEmbed.Add("allowFullScreen", "true");
            obj.AddMediaEmbed(mediaEmbed);


            RssMediaResponses mediaResponses = new RssMediaResponses();
            mediaResponses.responses.Add("Response 1");
            mediaResponses.responses.Add("Response 2");
            mediaResponses.responses.Add("Response 3");
            obj.AddMediaResponses(mediaResponses);

            RssMediaBacklinks links = new RssMediaBacklinks();
            links.backLinks.Add("http://backlink1.com");
            links.backLinks.Add("http://backlink2.com");
            obj.AddMediaBacklinks(links);


            RssMediaStatus mediaStatus = new RssMediaStatus();
            mediaStatus.state = "Blocked";
            mediaStatus.reason = "http://something.com/reason-for-blocking";
            obj.AddMediaStatus(mediaStatus);


            RssMediaPrice mediaPrice = new RssMediaPrice();
            mediaPrice.price = 30.00M;
            mediaPrice.currency = "USD";
            mediaPrice.type = "rent";
            mediaPrice.info = "http://something.com/pricing-info";
            obj.AddMediaPrice(mediaPrice);

            mediaPrice = new RssMediaPrice();
            mediaPrice.price = 60.00M;
            mediaPrice.currency = "GBP";
            mediaPrice.type = "rent";
            mediaPrice.info = "http://something.com/pricing-info";
            obj.AddMediaPrice(mediaPrice);

            RssMediaLicense mediaLicense = new RssMediaLicense();
            mediaLicense.license = "MIT";
            mediaLicense.type = "text/plain";
            mediaLicense.href = "http://something.com/license-text";
            obj.AddMediaLicense(mediaLicense);


            RssMediaPeerLink mediaPeerLink = new RssMediaPeerLink();
            mediaPeerLink.href = "http://something.com/location-of-item.torrent";
            mediaPeerLink.type = "application/x-bittorrent";
            obj.AddMediaPeerLink(mediaPeerLink);


            RssMediaLocation mediaLocation = new RssMediaLocation();
            mediaLocation.description = "Movie taken at my house";
            mediaLocation.start = new TimeSpan(0, 0, 0);
            mediaLocation.end = new TimeSpan(0, 4, 35);
            mediaLocation.latitude = 45.0;
            mediaLocation.longitude = 173.0;
            obj.AddMediaLocation(mediaLocation);

            mediaLocation = new RssMediaLocation();
            mediaLocation.description = "Neighbors House";
            mediaLocation.start = new TimeSpan(0, 4, 36);
            mediaLocation.end = new TimeSpan(0, 8, 59);
            mediaLocation.latitude = 46.0;
            mediaLocation.longitude = 174.0;
            obj.AddMediaLocation(mediaLocation);

            RssMediaRights mediaRights = new RssMediaRights();
            mediaRights.status = "userCreated";
            obj.AddMediaRights(mediaRights);


            RssMediaScenes mediaScenes = new RssMediaScenes();

            RssMediaScene scene = new RssMediaScene();
            scene.sceneDescription = "This is the beginning scene for the introduction";
            scene.sceneTitle = "Beginning Glory";
            scene.sceneStartTime = new TimeSpan(0, 0, 0);
            scene.sceneEndTime = new TimeSpan(0, 4, 0);
            mediaScenes.Add(scene);


            scene = new RssMediaScene();
            scene.sceneDescription = "This is the middle scene for the big war";
            scene.sceneTitle = "Gruesome War";
            scene.sceneStartTime = new TimeSpan(0, 4, 1);
            scene.sceneEndTime = new TimeSpan(0, 10, 0);
            mediaScenes.Add(scene);
            obj.AddMediaScenes(mediaScenes);

            RssDublinCoreValid dc = new RssDublinCoreValid();
            
            dc.start = new DateTime(2016, 01, 01);
            dc.end = new DateTime(2099, 12, 31);
            dc.scheme = "W3C-DTF";
            obj.AddMediaValid(dc);


        }
        #endregion


        #region CommonSetMediaPropertiesUsingHelperMethods
        public void CommonSetMediaPropertiesUsingHelperMethods(IRssMediaExtension obj)
        {
   

            obj.AddMediaRating("NC17", "http://search.yahoo.com/mrss/rating_schema");
            
            obj.AddMediaTitle("Flowers And More", "text/html");

            obj.AddMediaDescription("This is a blog about pretty flowers", "plain");

            obj.AddMediaKeyword("Rose");
            obj.AddMediaKeyword("Tulip");
            obj.AddMediaKeyword("Lily");

            obj.AddMediaThumbnail("http://sirpenski.com/images/rss/channellogo.gif", 60, 60);

            obj.AddMediaCategory("PERENNIAL_FLOWERS", "http://search.yahoo.com/mrss/category_schema", "Perennials");
            obj.AddMediaCategory("FLOWERS", "hhttp://prettythings.com/category_schema", "Flowers");

            obj.AddMediaHash("1902233933030303", "SHA256");

            obj.AddMediaPlayer("http://www.foo.com/player?id=111", 400, 200);

            obj.AddMediaCredit("entity name", "producer", "urn:ebu");

            obj.AddMediaCopyright("Copyright 2019. ACME Inc.");

            obj.AddMediaText("This is the first chapter", "text/plain", "en", new TimeSpan(0, 0, 20).Ticks, new TimeSpan(0, 0, 55).Ticks);

            obj.AddMediaRestriction("au us", "allow", "country");

            obj.AddMediaCommunity(3.5, 5000, 0, 5, 2000, 99, "Weepy, Sleepy, Nod");


            obj.AddMediaComment("Comment 1");
            obj.AddMediaComment("Comment 2");
            obj.AddMediaComment("Comment 3");

            obj.AddMediaEmbeddedResource("http://youtube.com/mediaplayer", 400, 200);
            obj.AddMediaEmbeddedResourceParameter("type", "application/x-shockwave-flash");
            obj.AddMediaEmbeddedResourceParameter("allowFullScreen", "true");

            obj.AddMediaResponse("Response 1");
            obj.AddMediaResponse("Response 2");
            obj.AddMediaResponse("Response 3");


            obj.AddMediaBacklink("http://backlink1.com");
            obj.AddMediaBacklink("http://backlink2.com");

            obj.AddMediaStatus("Blocked", "http://something.com/reason-for-blocking");

            obj.AddMediaPrice(30.00M, "USD", "rent", "http://something.com/pricing-info");
            obj.AddMediaPrice(60.00M, "GBP", "rent", "http://something.com/pricing-info");

            obj.AddMediaLicense("MIT", "http://something.com/license-text", "text/plain");

            obj.AddMediaPeerLink("http://something.com/location-of-item.torrent", "application/x-bittorrent");

            obj.AddMediaLocation("Movie Taken At My House", new TimeSpan(0, 0, 0).Ticks, new TimeSpan(0, 4, 35).Ticks, 45.0, 173.0);
            obj.AddMediaLocation("Neighbors House", new TimeSpan(0, 4, 36).Ticks, new TimeSpan(0, 8, 59).Ticks, 46.0, 174.0);

            obj.AddMediaRights("userCreated");

            obj.AddMediaScene("Beginning Glory", "This is the introduction sequence", new TimeSpan(0, 0, 0).Ticks, new TimeSpan(0, 4, 0).Ticks);
            obj.AddMediaScene("Gruesome War", "This is the middle scene for the big war", new TimeSpan(0, 4, 1).Ticks, new TimeSpan(0, 10, 0).Ticks);

            obj.AddMediaValid(new DateTime(2016, 01, 01), new DateTime(2099, 12, 31), "W3C-DTF");


        }

        #endregion



        #region CommonSetCoreItemPropertiesUsingObjects
        public void CommonSetCoreItemPropertiesUsingObjects(RssItem itm)
        {

            itm.title = "Item One Title";
            itm.description = "Item ONe Description Is a dandy";
            itm.link = "http://something.com/item1";
            itm.pubDate = DateTime.Now;
            itm.author = "p.sirpenski@something.com";

            RssItemCategory ctg = new RssItemCategory();
            ctg.category = "ITEM_CTG1";
            itm.categories.Add(ctg);

            ctg = new RssItemCategory();
            ctg.category = "ITEM_CTG2";
            ctg.domain = "something.com";
            itm.categories.Add(ctg);

            RssItemEnclosure enc = new RssItemEnclosure();
            enc.url = "http://something.com/crazy.mp4";
            enc.length = 0;
            enc.type = "video/mp4";
            itm.AddEnclosure(enc);

            RssItemGuid guid = new RssItemGuid();
            guid.guid = "http://something.com/item1";
            guid.isPermalink = true;
            itm.guid = guid;

            itm.comments = "http://something.com/item1/comments";

            RssItemSource src = new RssItemSource();
            src.source = "Blabbermouth Inc.";
            src.url = "http://blabbermouth.com/item1";
            itm.source = src;
        }

        #endregion



        #region CommonSetCoreItemPropertiesUsingHelperMethods
        public void CommonSetCoreItemPropertiesUsingHelperMethods(RssItem itm)
        {
            itm.AddTitle("Item One Title");
            itm.AddDescription("Item One Description Is A Dandy");
            itm.AddLink("http://something.com/item1");
            itm.AddPubDate(DateTime.Now);
            itm.AddAuthor("p.sirpenski@something.com");
            itm.AddCategory("ITEM_CTG1");
            itm.AddCategory("ITEM_CTG2", "something.com");
            itm.AddEnclosure("http://something.com/crazy.mp4", "video/mp4", 201928);
            itm.AddGuid("http://something.com/item1");
            itm.AddComment("http://something.com/item1/comments");
            itm.AddSource("http://blabbermouth.com", "Blabbermouth Inc.");
        }
        #endregion



        public void CommonSetExtendedItemPropertiesUsingObjects(RssItem itm)
        {
            RssDublinCoreCreator cr = new RssDublinCoreCreator();
            cr.creator = "Paul Sirpenski";
            itm.AddCreator(cr);

            cr = new RssDublinCoreCreator();
            cr.creator = "John Smith";
            itm.AddCreator(cr);

            RssContentEncoded enc = new RssContentEncoded();
            enc.encoded = WebUtility.HtmlEncode("THis is the funniest thing we need to do <today>");
            itm.AddContentEncoded(enc);



            RssAtomLink atomlnk = new RssAtomLink();
            atomlnk.href = "http://something.com/item1";
            atomlnk.type = "text/html";
            atomlnk.rel = "self";
            atomlnk.title = itm.title;
            atomlnk.hreflang = "en";
            atomlnk.length = 383933;
            itm.AddAtomLink(atomlnk);

            RssSlashComments slash = new RssSlashComments();
            slash.comments = 594;
            itm.AddSlashComments(slash);

            RssCreativeCommonsLicense ccl = new RssCreativeCommonsLicense();
            ccl.license = "http://creativecommons.org/my-fav-license.html";
            itm.AddCreativeCommonsLicense(ccl);

        }




        #region CreateChannelUsingObjects
        private void CreateChannelUsingObjects(Rss20 rss)
        {
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL (CORE PROPERTIES ONLY) USING OBJECTS");

        }
        #endregion



        #region CreateChannelUsingHelperMethods
        private void CreateChannelUsingHelperMethods(Rss20 rss)
        {
            CommonSetCoreChannelPropertiesUsingHelperMethods(rss, "CREATE CHANNEL (CORE PROPERTIES ONLY) USING HELPER METHODS");
        }
        #endregion



        #region CreateCoreChannelWithOneCoreItem
        private void CreateChannelWithOneCoreItem(Rss20 rss)
        {

            // create the base channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH ONE ITEM (CORE PROPERTIES ONLY) USING OBJECTS");


            RssItem itm = new RssItem();

            // set the core item properties
            CommonSetCoreItemPropertiesUsingObjects(itm);


            // add the item to the channel
            rss.channel.AddItem(itm);

        }
        #endregion



        #region CreateChannelWithOneCoreItemHelperMethods
        public void CreateChannelWithOneCoreItemHelperMethods(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingHelperMethods(rss, "CREATE CHANNEL WITH ONE ITEM (CORE PROPERTIES ONLY) USING HELPER METHODS");

            RssItem itm = new RssItem();

            // 
            CommonSetCoreItemPropertiesUsingHelperMethods(itm);

            rss.channel.AddItem(itm);
        }
        #endregion



        #region CreateChannelWithExtendedPropertiesUsingObjects
        public void CreateChannelWithExtendedPropertiesUsingObjects(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES USING OBJECTS");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

        }
        #endregion



        #region CreateChannelWithExtendedPropertiesUsingHelperMethods
        public void CreateChannelWithExtendedPropertiesUsingHelperMethods(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES USING HELPER METHODS");

            // set using extended properties helper methods, no media
            CommonSetExtendedChannelPropertiesUsingHelperMethods(rss);

        }
        #endregion


        #region CreateChannelWithExtendedPropertiesAndMediaUsingObjects
        public void CreateChannelWithExtendedPropertiesAndMediaUsingObjects(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES AND MEDIA USING OBJECTS");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // set the channel media properties
            CommonSetMediaPropertiesUsingObjects(rss.channel);


        }
        #endregion


        #region CreateChannelWithExtendedPropertiesAndMediaUsingHelperMethods
        public void CreateChannelWithExtendedPropertiesAndMediaUsingHelperMethods(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES AND MEDIA USING HELPER METHODS");

            // set using extended properties helper methods, no media
            CommonSetExtendedChannelPropertiesUsingHelperMethods(rss);

            // set te channel media properties
            CommonSetMediaPropertiesUsingHelperMethods(rss.channel);

        }
        #endregion


        #region CreateChannelWithExtendedPropertiesAndOneItemWithExtendedPropertiesNoMedia
        public void CreateChannelWithExtendedPropertiesAndOneItemWithExtendedPropertiesNoMediaUsingObjects(Rss20 rss)
        {

            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES AND ONE ITEM WITH EXTENDED PROPERTIES (NO MEDIA) USING OBJECTS");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // define new item
            RssItem itm = new RssItem();

            // add the core item
            CommonSetCoreItemPropertiesUsingObjects(itm);

            // add the extended item objects
            CommonSetExtendedItemPropertiesUsingObjects(itm);

            // add the item
            rss.channel.AddItem(itm);

        }
        #endregion


        #region CreateChannelWithExtendedPropertiesAndOneItemWithExtendedPropertiesAndMediaUsingObjeccts
        public void CreateChannelWithExtendedPropertiesAndOneItemWithExtendedPropertiesAndMediaUsingObjeccts(Rss20 rss)
        {

            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES PLUS ONE ITEM WITH EXTENSION AND MEDIA USING OBJECTS");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // define new item
            RssItem itm = new RssItem();

            // add the core item
            CommonSetCoreItemPropertiesUsingObjects(itm);

            // add the extended item objects
            CommonSetExtendedItemPropertiesUsingObjects(itm);

            CommonSetMediaPropertiesUsingObjects(itm);

            // add the item
            rss.channel.AddItem(itm);

        }


        #endregion


        public void CreateOneItemWithMediaContent(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES PLUS ONE ITEM WITH EXTENSION AND ONE MEDIA CONTENT ITEM");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // define new item
            RssItem itm = new RssItem();

            // add the core item
            CommonSetCoreItemPropertiesUsingObjects(itm);

            // add the extended item objects
            CommonSetExtendedItemPropertiesUsingObjects(itm);

            // Create the media content item
            RssMediaContent mc = new RssMediaContent();
            mc.url = "http://something.com/media/movie.mp4";
            mc.fileSize = 1000000;
            mc.type = "video/mp4";
            mc.medium = "video";
            mc.isDefault = true;
            mc.expression = "full";
            mc.bitrate = 128;
            mc.frameRate = 30;
            mc.samplingrate = 44.1;
            mc.channels = 2;
            mc.duration = 185;
            mc.width = 400;
            mc.height = 200;
            mc.lang = "en";
            itm.AddMediaContentItem(mc);




            // add the item
            rss.channel.AddItem(itm);
        }



        public void CreateOneItemWithMediaContentAndMediaOptions(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES PLUS ONE ITEM WITH EXTENSION AND ONE MEDIA CONTENT ITEM PLUS MEDIA OPTIONS");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // define new item
            RssItem itm = new RssItem();

            // add the core item
            CommonSetCoreItemPropertiesUsingObjects(itm);

            // add the extended item objects
            CommonSetExtendedItemPropertiesUsingObjects(itm);

            // Create the media content item
            RssMediaContent mc = new RssMediaContent();
            mc.url = "http://something.com/media/movie.mp4";
            mc.fileSize = 1000000;
            mc.type = "video/mp4";
            mc.medium = "video";
            mc.isDefault = true;
            mc.expression = "full";
            mc.bitrate = 128;
            mc.frameRate = 30;
            mc.samplingrate = 44.1;
            mc.channels = 2;
            mc.duration = 185;
            mc.width = 400;
            mc.height = 200;
            mc.lang = "en";

            CommonSetMediaPropertiesUsingObjects(mc);


            itm.AddMediaContentItem(mc);




            // add the item
            rss.channel.AddItem(itm);
        }


        public void CreateOneItemWithMediaGroup(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES PLUS ONE ITEM WITH EXTENSION AND ONE MEDIA GROUP");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // define new item
            RssItem itm = new RssItem();

            // add the core item
            CommonSetCoreItemPropertiesUsingObjects(itm);

            // add the extended item objects
            CommonSetExtendedItemPropertiesUsingObjects(itm);


            RssMediaGroup mg = new RssMediaGroup();




            // Create the media content item
            RssMediaContent mc = new RssMediaContent();
            mc.url = "http://something.com/media/movie.mp4";
            mc.fileSize = 1000000;
            mc.type = "video/mp4";
            mc.medium = "video";
            mc.isDefault = true;
            mc.expression = "full";
            mc.bitrate = 128;
            mc.frameRate = 30;
            mc.samplingrate = 44.1;
            mc.channels = 2;
            mc.duration = 185;
            mc.width = 400;
            mc.height = 200;
            mc.lang = "en";
            mg.AddMediaContentItem(mc);


            // Create the media content item
            mc = new RssMediaContent();
            mc.url = "http://something.com/media/a-better-movie.mp4";
            mc.fileSize = 2000000;
            mc.type = "video/mp4";
            mc.medium = "video";
            mc.isDefault = true;
            mc.expression = "full";
            mc.bitrate = 128;
            mc.frameRate = 30;
            mc.samplingrate = 44.1;
            mc.channels = 2;
            mc.duration = 185;
            mc.width = 400;
            mc.height = 200;
            mc.lang = "en";
            mg.AddMediaContentItem(mc);




            itm.AddMediaGroup(mg);




            // add the item
            rss.channel.AddItem(itm);
        }


        public void CreateOneItemWithMediaGroupAndMediaOptions(Rss20 rss)
        {
            // create the channel
            CommonSetCoreChannelPropertiesUsingObjects(rss, "CREATE CHANNEL WITH EXTENDED PROPERTIES PLUS ONE ITEM WITH EXTENSION AND ONE MEDIA GROUP WITH MEDIA OPTIONS");

            // set using extended properties objects, no media
            CommonSetExtendedChannelPropertiesUsingObjects(rss);

            // define new item
            RssItem itm = new RssItem();

            // add the core item
            CommonSetCoreItemPropertiesUsingObjects(itm);

            // add the extended item objects
            CommonSetExtendedItemPropertiesUsingObjects(itm);


            RssMediaGroup mg = new RssMediaGroup();




            // Create the media content item
            RssMediaContent mc = new RssMediaContent();
            mc.url = "http://something.com/media/movie.mp4";
            mc.fileSize = 1000000;
            mc.type = "video/mp4";
            mc.medium = "video";
            mc.isDefault = true;
            mc.expression = "full";
            mc.bitrate = 128;
            mc.frameRate = 30;
            mc.samplingrate = 44.1;
            mc.channels = 2;
            mc.duration = 185;
            mc.width = 400;
            mc.height = 200;
            mc.lang = "en";
            CommonSetMediaPropertiesUsingObjects(mc);
            mg.AddMediaContentItem(mc);


            // Create the media content item
            mc = new RssMediaContent();
            mc.url = "http://something.com/media/a-better-movie.mp4";
            mc.fileSize = 2000000;
            mc.type = "video/mp4";
            mc.medium = "video";
            mc.isDefault = true;
            mc.expression = "full";
            mc.bitrate = 128;
            mc.frameRate = 30;
            mc.samplingrate = 44.1;
            mc.channels = 2;
            mc.duration = 185;
            mc.width = 400;
            mc.height = 200;
            mc.lang = "en";
            CommonSetMediaPropertiesUsingObjects(mc);
            mg.AddMediaContentItem(mc);

     //       CommonSetMediaPropertiesUsingObjects(mg);



            itm.AddMediaGroup(mg);




            // add the item
            rss.channel.AddItem(itm);
        }

    }
}
