using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsFeed.Models
{
    public class keyword
    {
        //public int Id { get; set; }
        [Key]
        public string word { get; set; }
        public int cntAtl { get; set; }                             //the whole number of articles that have this keyword

        public keyword()
        {
        }
        public keyword(string w, int cnt)
        {
            word = w;      //foreign key can't use
            cntAtl = cnt;
        }

    }
}