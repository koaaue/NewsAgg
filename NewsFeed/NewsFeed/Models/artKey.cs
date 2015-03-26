/*********************************************
 * 此Table表示一篇文章里word的次数统计，用作TF
 * TFIDF默认0，TF除以keyword表格的IDF后获得
 *********************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewsFeed.Models
{
    public class artKey
    {
        [Key]
        [Column(Order = 0)]
        public int AId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string word { get; set; }

        public int TF { get; set; }                   //TF: the frequency of each word
        public double TFIDF { get; set; }


        public artKey()
        {
        }
        public artKey(int a, string w, int cnt)
        {
            AId = a;
            word = w;
            TF = cnt;
        }

    }
}