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
using System.Text.RegularExpressions;
using System.Collections;

namespace MvcApplication8.Controllers
{
    public class Xml2ModelController : Controller
    {
        private NewsFeedContext db = new NewsFeedContext();

        //
        // GET: /Xml2Model/

        private static Regex non1 = new Regex(@"\b(a|aboard|about|above|absent|according\sto|across|after|against|ago|ahead\sof|ain't|all|along|alongside)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non2 = new Regex(@"\b(also|although|am|amid|amidst|among|amongst|an|and|anti|anybody|anyone|anything|apart|apart\sfrom|are|been)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non3 = new Regex(@"\b(aren't|around|as|as\sfar\sas|as\ssoon\sas|as\swell\sas|aside|at|atop|away|be|because|because\sof|before)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non4 = new Regex(@"\b(behind|below|beneath|beside|besides|between|betwixt|beyond|but|by|by\smeans\sof|by\sthe\stime|can|cannot)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non5 = new Regex(@"\b(circa|close\sto|com|concerning|considering|could|couldn't|cum|'d|despite|did|didn't|do|does|doesn't|don't)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non6 = new Regex(@"\b(down|due\sto|during|each_other|'em|even\sif|even\sthough|ever|every|every\stime|everybody|everyone)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non7 = new Regex(@"\b(everything|except|far\sfrom|few|first\stime|following|for|from|get|got|had|hadn't|has|hasn't|have)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non8 = new Regex(@"\b(haven't|he|hence|her|here|hers|herself|him|himself|his|how|i|if|in|in\saccordance\swith|in\saddition\sto|in\scase)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non9 = new Regex(@"\b(in\sfront\sof|in\slieu\sof|in\splace\sof|in\sspite\sof|in\sthe\sevent\sthat|in\sto|inside|inside\sof)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non10 = new Regex(@"\b(instead\sof|into|is|isn't|it|itself|just\sin\scase|like|'ll|lots|may|me|mid|might|mightn't|mine|more|most)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non11 = new Regex(@"\b(must|mustn't|myself|near|near\sto|nearest|new|no|no\sone|nobody|none|not|nothing|notwithstanding|now\sthat|of)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non12 = new Regex(@"\b(off|on|on\sbehalf\sof|on\sto|on\stop\sof|once|one|one\sanother|only\sif|onto|opposite|or|org|other|our|any)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non13 = new Regex(@"\b(ours|ourselves|out|out\sof|outside|outside\sof|over|past|per|plenty|plus|prior\sto|qua|re|'re|really|set)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non14 = new Regex(@"\b(regarding|round|'s|said|sans|save|say|says|shall|shan't|she|should|shouldn't|since|so|somebody|its|only)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non15 = new Regex(@"\b(someone|something|than|that|the|thee|their|theirs|them|themselves|there|these|they|thine|this|thou)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non16 = new Regex(@"\b(though|through|throughout|till|to|toward|towards|under|underneath|unless|unlike|until|unto|up|upon|using|even)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non17 = new Regex(@"\b(us|'ve|versus|via|was|wasn't|we|were|weren't|what|when|whenever|where|whereas|whether\sor\snot|things)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non18 = new Regex(@"\b(which|while|who|whoever|whom|why|will|with|with\sregard\sto|withal|within|without|won't|would|wouldn't|mere)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non19 = new Regex(@"\b(ya|ye|yes|you|your|yours|yourself)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        private static Regex non20 = new Regex(@"\b(a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        public ActionResult Index()
        {
           /* HashSet<string> hs = new HashSet<string>();
            var query = from KeywordsTotal in db.keywordsTotal
                        select KeywordsTotal;
            for (int i = 0; i < query.Count(); i++)
            {
                hs.Add(query.ElementAt(i).keyword);

            }*/

            //Linq 语法 计算在likes 中ItemID文章的总like数目，也可用EF方法实现，比较麻烦
            // int x = db.likes.Count(like => like.ItemId ==101);
            
            /******************************
             访问rss的地址，读取xml数据
             ******************************/
            rss newsItems = null;
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
            newsItems = (rss)serializer.Deserialize(reader);

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
            {
                string httpTime = newsItems.item[newsItems.item.Length - 1].pubDate;
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
            for (var i = newsItems.item.Length - 1; i >= 0; i--)    //old item store into database first
            {
                string httpTime = newsItems.item[i].pubDate;
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
                newsItems.item[i].description = Regex.Replace(newsItems.item[i].description, "<.*?>", string.Empty);
                string text = newsItems.item[i].title +" "+ newsItems.item[i].description;
                text  = text.ToLower();   

                text = non1.Replace(text, "");
                text = non2.Replace(text, "");
                text = non3.Replace(text, "");
                text = non4.Replace(text, "");
                text = non5.Replace(text, "");
                text = non6.Replace(text, "");
                text = non7.Replace(text, "");
                text = non8.Replace(text, "");
                text = non9.Replace(text, "");
                text = non10.Replace(text, "");
                text = non11.Replace(text, "");
                text = non12.Replace(text, "");
                text = non13.Replace(text, "");
                text = non14.Replace(text, "");
                text = non15.Replace(text, "");
                text = non16.Replace(text, "");
                text = non17.Replace(text, "");
                text = non18.Replace(text, "");
                text = non19.Replace(text, "");
                text = non20.Replace(text, "");
                char[] sp = new Char[] { ',', '.', ' ', '?', ':', '\'', '‘', '’','|'};
                string[] words = text.Split(sp, StringSplitOptions.RemoveEmptyEntries);  

               // string[] words = text.Split(' ');


                words[0] = Regex.Replace(words[0], "[\\s\\p{P}\n\r=<>$>+￥^]", "");
                words[1] = Regex.Replace(words[1], "[\\s\\p{P}\n\r=<>$>+￥^]", "");
                words[2] = Regex.Replace(words[2], "[\\s\\p{P}\n\r=<>$>+￥^]", "");


                Models.item item = new Models.item(newsItems.item[i], time, "NYTimes", 0, words[0], words[1], words[2]);// "");

                db.items.Add(item);               //item include 4 elements
                db.SaveChanges();


                for (int j = 0; j < 3; j++)
                {

                    db.articleKeyword.Add(new ArticleKeyword(words[j], item.Id));

                    if (db.keywordsTotal.Find(words[j]) != null)
                    {
                        db.keywordsTotal.Find(words[j]).keywordSum++;

                    }
                    else
                    {
                        db.keywordsTotal.Add(new KeywordsTotal(words[j], 1));

                    }
                    //db.SaveChanges();
                }

                //db.channel.Add(cars.item[i]);

                 
            }
            db.SaveChanges();

            /*var query = from item in db.items
                         where item.imgId == ""
                         select item;

            foreach (Models.item item in query)
            {
                item.imgId = "i" + item.Id;
            }
            db.SaveChanges();*/
            return View();
        }

       /* static List<item> selectedarticles = new List<item> { };

        [HttpPost]
        public List<item> search(string str)
        {
            var query2 = from item in db.items                                       //只提取一天内的新闻
                        where item.kw1==str || item.kw2==str ||item.kw3==str
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("title");
            dt.Columns.Add("link");
            DataRow row = null;
            foreach (var rowObj in query2)
            {
                row = dt.NewRow();
                dt.Rows.Add(rowObj.title,rowObj.guid);
            }
            return dt;
        }*/

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

                Li.UserName=userName;
                x = -1;
                var query = from like in db.likes
                            where (like.UserName == userName) && (like.ItemId==Li.ItemId)
                            select like;
                
                if (query.Count() == 0)
                {
                    x = 1;
                    db.likes.Add(Li);

                    db.items.Find(Li.ItemId).totalLike ++;
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
