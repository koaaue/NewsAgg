using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication8.Models
{
    public class ArticleKeyword
    {
       [Key]
       [Column(Order = 0)]
        public int AId { get; set; }
       [Key]
       [Column(Order = 1)]
        public string SingleKeyword { get; set; }

       public int frequency { get; set; }
        public ArticleKeyword()
        {
        }
        public ArticleKeyword(string s, int i)
        {
            this.SingleKeyword = s;
           this.AId=i;
           this.frequency=1;
        }
    }
}