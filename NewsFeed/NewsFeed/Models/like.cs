using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewsFeed.Models
{
    public class like
    {
        //public int Id { get; set; }
        [Key]
        [Column(Order = 0)]
        public string UserName { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ItemId { get; set; }

        //public virtual UserProfile UserProfile { get; set; }      //cannot link to the UserProfile?

        public like()
        {
        }
        public like(string n, int iid)
        {
            UserName = n;      //foreign key can't use
            ItemId = iid;
        }
    }
}