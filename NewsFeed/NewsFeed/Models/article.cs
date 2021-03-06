﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace NewsFeed.Models
{
    [Serializable()]
    public class item
    {
        public int Id { get; set; }//数据库主ID
        [System.Xml.Serialization.XmlElement("title")]
        public string title { get; set; }

        [System.Xml.Serialization.XmlElement("pubDate")]
        public string pubDate { get; set; }

        [System.Xml.Serialization.XmlElement("guid")]
        public string guid { get; set; }

        [System.Xml.Serialization.XmlElement("description")]
        public string description { get; set; }

        public DateTime? Date { get; set; }
        public int totalLike { get; set; }
        public string keyword { get; set; }
        //  public string category { get; set; }

        [ForeignKey("source")]
        public string srcName { get; set; }

        public item()
        {
        }

        public item(item i, DateTime d, string s, int t)
        {
            title = i.title;
            pubDate = i.pubDate;
            guid = i.guid;
            description = i.description;
            keyword = i.keyword;               //necessary?

            Date = d;
            srcName = s;
            totalLike = t;
        }

        public virtual source source { get; set; }
    }


    [Serializable()]
    [System.Xml.Serialization.XmlRoot("rss")]
    public class rss
    {
        [XmlArray("channel")]
        [XmlArrayItem("item", typeof(item))]
        public item[] item { get; set; }
    }
}