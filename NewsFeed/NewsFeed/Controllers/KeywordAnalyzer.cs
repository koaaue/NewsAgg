using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsFeed.Controllers
{
    class KeywordAnalyzer
    {
        private UsersContext db = new UsersContext();

        public void analyze( Models.item item)
        {
            string article = item.title;
            article = article.ToLower();                                            //全部小写
            
            /** 应该建立更复杂的split算法，得用Regex */
            string[] split = article.Split(new Char[] { ',', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);      //建单词array
            long totalWord = split.Length;

            Dictionary<string, int> map = new Dictionary<string, int>();        //Map结构, key是单词，value是词频

            for (int i = 0; i < split.Length; i++)
            {
                if (map.ContainsKey(split[i]))
                    map[split[i]]++;                                            //词频加1
                else
                    map.Add(split[i], 1);
            }


            for (int i = 0; i < map.Count; i++)
            {
                /******************************
                * Process keyword table:
                * For each keyword that appear in this article, plus one for IDF.
                ******************************/
                //Models.keyword keyLine = db.keywords.Find(map.ElementAt(i).Key);                //search database
                string word = map.ElementAt(i).Key;

                if (db.keywords.Find(word) == null)
                {
                    Models.keyword kw = new Models.keyword(word, 1);
                    db.keywords.Add(kw);                                                    //add new keyword to DB
                }
                else
                {
                    db.keywords.Find(word).cntAtl++;
                }

                /******************************
                * Process artCntKey table:
                * Saving the TF of each word in this article.
                ******************************/
                Models.artCntKey artK = new Models.artCntKey(item.Id, word, map.ElementAt(i).Value);             //add new keyword
                db.artCntKeys.Add(artK);


            }

            db.SaveChanges();                 //here should save change to db
        }
    }

}
