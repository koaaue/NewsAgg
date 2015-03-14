using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewsFeed.Models
{
    public class artCntKey
    {
        [Key]
        [Column(Order = 0)]
        public int AId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string word { get; set; }

        public int cntKey { get; set; }


        public artCntKey()
        {
        }
        public artCntKey(int a, string w, int cnt)
        {
            AId = a;
            word = w;
            cntKey = cnt;
        }

    }
}