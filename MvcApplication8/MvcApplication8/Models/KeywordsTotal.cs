using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication8.Models
{
    public class KeywordsTotal
    {
        [Key]

        public string keyword { get; set; }

        public int keywordSum { get; set; }

        public KeywordsTotal()
        {
        }
        public KeywordsTotal(string keyword,int i)
        {
            this.keywordSum = i;
            this.keyword = keyword;
        }



    }
}