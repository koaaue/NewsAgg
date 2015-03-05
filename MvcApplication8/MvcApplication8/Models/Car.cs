using System;
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

        public item(item i, DateTime d, string s, int t)//,string x)
        {
            title = i.title;
            pubDate = i.pubDate;
            guid = i.guid;
            Date = d;
            srcName = s;
            totalLike = t;
            //imgId = x;
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