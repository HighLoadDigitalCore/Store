using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Smoking.Models
{
    [XmlRoot("sitemapindex", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SiteMapIndex
    {
        public SiteMapIndex()
        {
            map = new ArrayList();
        }
        private ArrayList map;


        [XmlElement("sitemap")]
        public Location[] Locations
        {
            get
            {
                Location[] items = new Location[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                Location[] items = (Location[])value;
                map.Clear();
                foreach (Location item in items)
                    map.Add(item);
            }
        }
        public int Add(Location item)
        {
            return map.Add(item);
        }

        public void AddRange(IEnumerable<Location> items)
        {
            map.AddRange(items.ToArray());
        }
    }


    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]

    public class Sitemap
    {

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();


        private ArrayList map;

        public Sitemap()
        {
            map = new ArrayList();
            xmlns.Add("image", "http://www.google.com/schemas/sitemap-image/1.1");
        }

        [XmlElement("url")]
        public Location[] Locations
        {
            get
            {
                Location[] items = new Location[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                Location[] items = (Location[])value;
                map.Clear();
                foreach (Location item in items)
                    map.Add(item);
            }
        }

        public int Add(Location item)
        {
            return map.Add(item);
        }

        public void AddRange(IEnumerable<Location> items)
        {
            map.AddRange(items.ToArray());
        }
    }



    public class MapImage
    {
        public MapImage()
        {

        }

        [XmlElement("loc", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        public string loc { get; set; }
    }

    // Items in the shopping list
    public class Location
    {
        public Location()
        {

        }
        public Location(DateTime? lastMod = null)
        {
            if (lastMod.HasValue)
            {
                LastModified = new DateTime(lastMod.Value.Year, lastMod.Value.Month, lastMod.Value.Day,
                    lastMod.Value.Hour, lastMod.Value.Minute, lastMod.Value.Second,
                    DateTimeKind.Local);
            }
        }
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

        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("image", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        public MapImage[] Images { get; set; }


        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        [XmlElement("lastmod")]
        public string LastMod
        {
            get
            {
                return LastModified.HasValue ? LastModified.Value.ToString("O").Replace(".0000000", "") : "";
            }
            set
            {
                LastModified = DateTime.Parse(value, new CultureInfo("ru-RU"));
            }
        }


        private DateTime? _lastModified;
        [XmlIgnore]
        public DateTime? LastModified
        {
            get { return _lastModified; }
            set
            {
                if (value.HasValue)
                {
                    _lastModified = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day,
                        value.Value.Hour, value.Value.Minute, value.Value.Second,
                        DateTimeKind.Local);
                }
                else
                {
                    _lastModified = null;
                }
            }
        }


        public bool ShouldSerializeLastModified() { return _lastModified.HasValue; }



        [XmlElement("priority")]
        public double? Priority { get; set; }
        public bool ShouldSerializePriority() { return Priority.HasValue; }
    }
}