using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsFeed.Models
{
    public class source
    {
        //public int Id { get; set; }
        [Key]
        public string srcName { get; set; }
        public DateTime? newDate { get; set; }

        public source()
        {
        }
        public source(string s, DateTime d)
        {
            srcName = s;
            newDate = d;
        }

        //public virtual ICollection<item> items { get; set; }     //items是接口则无法序列化

    }
}