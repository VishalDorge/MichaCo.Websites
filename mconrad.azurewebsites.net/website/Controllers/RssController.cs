using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace mconrad.azurewebsites.net.Controllers
{
    public class RssController : Controller
    {
        private const string DateFormat = "{0:r}";   // Wed, 03 Jun 2015 18:06:50 GMT

        public ActionResult News()
        {
            var pubDate = new DateTime(2015, 6, 2);

            var feed = new Rss();
            feed.Channel = new RssChannel()
            {
                Title = "Michael's Development Website News",
                Description = "Latest articles and blogs from Michael's Development Website.",
                Link = "http://www.michaconrad.com/",
                TimeToLive = 1000,
                PublicationDate = string.Format(CultureInfo.InvariantCulture, DateFormat, pubDate),
                Items = new RssItem[]
                {
                    new RssItem()
                    {
                        Title = "Accessing Environment Variables in ASP.NET 5 Apps",
                        Description = "This is a quick walk-through of how to access environmental variables when writing applications using the ASP.NET 5 DNX execution environment",
                        Link = "http://michaconrad.com/Documentation/Blog/aspnet_5_accessing_environment_variables",
                    },
                    new RssItem()
                    {
                        Title = "Single Page Todo App with Cache Manager",
                        Description = "This is about creating a single page web site using an ASP.NET Web API Service which stores the data via Cache Manager.",
                        Link = "http://cachemanager.net/Documentation/Index/cachemanager_backed_todo_web_app",
                    },
                    new RssItem()
                    {
                        Title = "Cache Synchronization",
                        Description = "Cache synchronization in distributed scenarios",
                        Link = "http://cachemanager.net/Documentation/Index/cachemanager_synchronization",
                    },
                    new RssItem()
                    {
                        Title = "Cache Manager Configuration",
                        Description = "A big goal of Cache Manager is to make it easy to work with different cache systems, but at the same time it should be flexible to adopt to different needs",
                        Link = "http://cachemanager.net/Documentation/Index/cachemanager_configuration",
                    },
                    new RssItem()
                    {
                        Title = "Getting Started with Cache Manager",
                        Description = "Introduction to the CacheManager library.",
                        Link = "http://cachemanager.net/Documentation/Index/cachemanager_getting_started",
                    },
                    new RssItem()
                    {
                        Title = "Features and Architecture",
                        Description = "Overview of the features and design of the CacheManager library.",
                        Link = "http://cachemanager.net/Documentation/Index/cachemanager_architecture",
                    },
                    new RssItem()
                    {
                        Title = "Update Operations",
                        Description = @"Updating a cache item in a distributed cache is different from just changing the item within an in-process cache. 
With in-process caches, we can ensure thread safe writes, and with poco objects, the in-process cache will just keep the reference to that object and therefor always holds the same version for all threads.",
                        Link = "http://cachemanager.net/Documentation/Index/cachemanager_update",
                    }
                }
            };
            
            return new RssResult(feed);
        }
    }
    
    public class RssResult : ActionResult
    {
        private Rss feed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RssResult"/> class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        public RssResult(Rss feed)
        {
            this.feed = feed;
        }

        /// <summary>
        /// Gets the object to be serialized to XML.
        /// </summary>
        public Rss Feed
        {
            get { return this.feed; }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (this.feed != null)
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.ContentType = "text/xml";
                GetFeedData(context.HttpContext.Response.Output, this.feed);
            }
        }

        private static void GetFeedData(TextWriter stream, Rss feed)
        {
            var serializer = new XmlSerializer(typeof(Rss));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            serializer.Serialize(stream, feed, namespaces);
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "rss")]
    public class Rss
    {
        public Rss()
        {
            this.Version = "2.0";
        }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "channel")]
        public RssChannel Channel { get; set; }
    }

    [Serializable]
    public class RssChannel
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "generator")]
        public string Generator { get; set; }

        [XmlElement(ElementName = "pubDate")]
        public string PublicationDate { get; set; }

        [XmlElement("ttl")]
        public int TimeToLive { get; set; }

        [XmlElement("image")]
        public RssImage Image { get; set; }

        [XmlElement("item")]
        public RssItem[] Items { get; set; }
    }

    [Serializable]
    public class RssItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }
    }

    [Serializable]
    public class RssImage
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }
    }
}