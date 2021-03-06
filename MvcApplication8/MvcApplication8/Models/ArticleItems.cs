﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MvcApplication8.Models
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
        public string kw1 { get; set; }
        public string kw2 { get; set; }
        public string kw3 { get; set; }
        public DateTime? Date { get; set; }
        public int totalLike { get; set; }
        //public string imgId { get; set; }
        [ForeignKey("source")]
        public string srcName { get; set; }
      //  public string category { get; set; }
      //  public string[] keyword { get; set; }
        public item()
        {
        }

        public item(item i, DateTime d, string s, int t,string k1,string k2,string k3)//,string x)
        {
            title = i.title;
            pubDate = i.pubDate;
            guid = i.guid;
            Date = d;
            description = i.description;
            srcName = s;
            totalLike = t;
            kw1 = k1;
            kw2 = k2;
            kw3 = k3;
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