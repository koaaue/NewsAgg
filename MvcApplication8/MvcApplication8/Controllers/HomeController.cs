using MvcApplication8.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication8.Controllers
{
    public class HomeController : Controller
    {
        private CarContext db = new CarContext();
        public ActionResult Index()
        {
            var query = from item in db.items                                       //只提取一天内的新闻
                        where DbFunctions.DiffHours(item.Date, DateTime.Now) < 24
                        select item;

            
           

            return View(query.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            DateTime newtime = DateTime.ParseExact("Wed, 11 Feb 2015 10:35:48 EST", "ddd, dd MMM yyyy HH:mm:ss EST", new CultureInfo("en-US")).AddHours(10);
            ViewBag.Message = newtime;

            return View();
        }
    }
}
