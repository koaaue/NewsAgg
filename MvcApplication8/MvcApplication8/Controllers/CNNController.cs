using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication8.Models;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Xml;
using System.Globalization;

namespace MvcApplication8.Controllers
{
    public class CNNController : Controller
    {
        private CarContext db = new CarContext();

        //
        // GET: /Xml2Model/

        public ActionResult Index()
        {

            //Linq 语法 计算在likes 中ItemID文章的总like数目，也可用EF方法实现，比较麻烦
            // int x = db.likes.Count(like => like.ItemId ==101);

            /******************************
             访问rss的地址，读取xml数据
             ******************************/
            rss cars = null;
            //  System.Net.WebClient client = new WebClient();
            //  byte[] page = client.DownloadData("http://rss.nytimes.com/services/xml/rss/nyt/US.xml");
            // string path = System.Text.Encoding.UTF8.GetString(page);
            //string path = "cars.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(rss));

            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://rss.nytimes.com/services/xml/rss/nyt/US.xml");
            //使用Cookie设置AllowAutoRedirect属性为false,是解决“尝试自动重定向的次数太多。”的核心
            // request.CookieContainer = new CookieContainer();
            // request.AllowAutoRedirect = false;
            //  WebResponse response = (WebResponse)request.GetResponse();
            //  Stream sm = response.GetResponseStream();
            //  System.IO.StreamReader streamReader = new System.IO.StreamReader(sm);
            //将流转换为字符串
            // string html = streamReader.ReadToEnd();
            // streamReader.Close();

            //  TextReader reader = new StreamReader(path);

            XmlReader reader = new XmlTextReader("http://rss.cnn.com/rss/cnn_us.rss");
            cars = (rss)serializer.Deserialize(reader);

            /*var serializer = new XmlSerializer(typeof(rss));
              using (TextReader reader = new StringReader(html))
              {
                  cars = (rss)serializer.Deserialize(reader);
              }*/


            //  reader.Close();


            /******************************
              初始化最新时间newTime
             ******************************/
            DateTime newTime;
            if (db.sources.Find("CNN") == null)
            {
                string httpTime = cars.item[cars.item.Length - 1].pubDate;
                newTime = DateTime.ParseExact(httpTime, "ddd, dd MMM yyyy HH:mm:ss EST", new CultureInfo("en-US")).AddHours(-1);
                Models.source src = new Models.source("CNN", newTime);
                db.sources.Add(src);
            }
            else
            {
                Models.source src = db.sources.Find("CNN");
                newTime = src.newDate.Value;
            }


            /******************************
             循环添加每一条新闻条目，只添加新条目
             ******************************/
            for (var i = cars.item.Length - 1; i >= 0; i--)    //old item store into database first
            {
                string httpTime = cars.item[i].pubDate;
                DateTime time = DateTime.ParseExact(httpTime, "ddd, dd MMM yyyy HH:mm:ss EST", new CultureInfo("en-US")).AddHours(-1);


                // 每次添加新条目前，先与source里的最新时间对比
                if (time <= newTime)       //time值小于最新时间，舍弃
                    continue;
                else
                {
                    Models.source src = db.sources.Find("CNN");
                    src.newDate = time;    //更新时间
                }


                Models.item item = new Models.item(cars.item[i], time, "CNN", 0, "");

                db.items.Add(item);               //item include 4 elements
                //db.channel.Add(cars.item[i]);


            }
            db.SaveChanges();
            var query = from item in db.items
                        where item.imgId == ""
                        select item;

            foreach (Models.item item in query)
            {
                item.imgId = "i" + item.Id;
            }



            db.SaveChanges();
            return View();
        }



        /******************************
         * 点赞功能
         * action需要url访问激活，没必要建立点赞的url页面，需要AJAX？
         ******************************/
        [HttpPost]
        public ActionResult Index(Models.like Li)   //通过AJAX在后台访问
        {
            int x = 0;
            if (System.Web.HttpContext.Current == null) return Json(x);
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                // 检查当前用户是否已为一个已登录用户
                //bool isAuthenticated = System.Web.HttpContext.Current.Request.IsAuthenticated;



                // 获取当前请求的用户名
                string userName = System.Web.HttpContext.Current.User.Identity.Name;

                Li.UserName = userName;
                x = -1;
                var query = from like in db.likes
                            where (like.UserName == userName) && (like.ItemId == Li.ItemId)
                            select like;

                if (query.Count() == 0)
                {
                    x = 1;
                    db.likes.Add(Li);

                    db.items.Find(Li.ItemId).totalLike++;
                    db.SaveChanges();

                }

                return Json(x);
            }

            return Json(x);
            //?
        }

        //
        // POST: /Xml2Model/upvote
        /*public ActionResult upvote()
        {
            return View(db.likes.Find(id));     //去upvote页面，调用.AntiForgeryToken，再回到Index页面，费事
        }
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult upvoteConfirmed(int Uid, int Iid)
        {
            //Models.like Li = db.likes.Find(Uid,Iid);
            Models.like Li = new Models.like( Uid, Iid );
            db.likes.Add(Li);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */


    }
}
