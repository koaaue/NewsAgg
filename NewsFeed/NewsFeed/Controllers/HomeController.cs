using NewsFeed.Models;
using NewsFeed.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace NewsFeed.Controllers
{
    public class HomeController : Controller
    {       
		private UsersContext db = new UsersContext();

        //
        // GET: /News/

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

            XmlReader reader = new XmlTextReader("http://rss.nytimes.com/services/xml/rss/nyt/US.xml");
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

            if (db.sources.Find("NYTimes") == null)
                //db.sources.Add(new Models.source("NYTimes", new DateTime(2000,1,1)));    //Initialize database
                db.sources.Add(new Models.source("NYTimes", DateTime.Parse("Sat, 07 Feb 2015 00:57:00 GMT")));
                //db.sources.Add(new source { srcName = "NYTimes", newDate = DateTime.Parse("Sat, 07 Feb 2015 00:57:00 GMT") });
                //db.SaveChanges();

            if (db.sources.Find("NYTimes").newDate.Value < DateTime.Parse("Sat, 07 Feb 2015 00:57:00 GMT"))
            {
                string httpTime = cars.item[cars.item.Length - 1].pubDate;
                newTime = DateTime.Parse(httpTime);
                Models.source src = new Models.source("NYTimes", newTime);
                db.sources.Add(src);
            }
            else
            {
                Models.source src = db.sources.Find("NYTimes");
                newTime = src.newDate.Value;
            }


            /******************************
             循环添加每一条新闻条目，只添加新条目
             ******************************/

            for (var i = cars.item.Length - 1; i >= 0; i--)    //old item store into database first
            {
                string httpTime = cars.item[i].pubDate;
                DateTime time = DateTime.Parse(httpTime);


                // 每次添加新条目前，先与source里的最新时间对比
                if (time <= newTime)       //time值小于最新时间，舍弃
                    continue;
                else
                {
                    Models.source src = db.sources.Find("NYTimes");
                    src.newDate = time;    //更新时间
                }

                // description里面会带有<和> 之间的多余内容，例如广告，使用正则表达式可以消除掉
                cars.item[i].description = Regex.Replace(cars.item[i].description, "<.*?>", string.Empty);


                Models.item item = new Models.item(cars.item[i], time, "NYTimes", 0);

                db.items.Add(item);               //item include 4 elements

                db.SaveChanges();                 //save DB before calling other function !!


                /**********************************
                 * 添加每篇文章同时对keyword表和artKey表进行统计
                 * ********************************/
                KeywordAnalyzer ka = new KeywordAnalyzer();

                ka.analyze(item);                   //这里保存的数据库结果，不会传到view的ToList里？TFIDF

                ka.TFIDF(item.Id);                  //随着数据越多，TFIDF效果会越来越精确


                /*****************************
                * 把结果存进article表中。不能放在子函数，否则传不进View？
                *****************************/
                var query2 = db.artKeys
                       .Where(x => x.AId == item.Id)
                       .OrderByDescending(x => x.TFIDF)
                       .Take(3);                                        //获得排序最高的三个关键词
                string str = "";
                foreach (var line in query2)
                {
                    str = str + line.word + ",";
                }

                db.items.Find(item.Id).keyword = str;
                db.SaveChanges();
            }


            //db.SaveChanges();                 
            return View(db.items.ToList());
        }



        /******************************
         * 点赞功能
         * action需要url访问激活，没必要建立点赞的url页面，需要AJAX？
         ******************************/
        [HttpPost]
        public ActionResult Index(Models.like Li)   //通过AJAX在后台访问
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                // 检查当前用户是否已为一个已登录用户
                //bool isAuthenticated = System.Web.HttpContext.Current.Request.IsAuthenticated;

                // 获取当前请求的用户名
                string userName = System.Web.HttpContext.Current.User.Identity.Name;

                Li.UserName=userName;
                db.likes.Add(Li);

                db.items.Find(Li.ItemId).totalLike+=1;
                db.SaveChanges();

            }


            return View();                      //?
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


        

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";


            KeywordAnalyzer ka = new KeywordAnalyzer();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
