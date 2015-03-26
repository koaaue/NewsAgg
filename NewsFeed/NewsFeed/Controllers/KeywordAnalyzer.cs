/*********************************************
 * 实现功能：1.删除trivial词
 *           2.去掉标点，建立word array
 *           3.用map计算每个word的frequency
 *           4.对artKey表写入TF，对keyword表写入IDF
 *           5.对artKey表计算并写入TF-IDF（可更新）
 *********************************************/

using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NewsFeed.Controllers
{
    class KeywordAnalyzer
    {
        private UsersContext db = new UsersContext();
        
        private string rmTrivial( string text ) {

            Regex non1 = new Regex(@"\b(a|aboard|about|above|absent|according\sto|across|after|against|ago|ahead\sof|ain't|all|along|alongside)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non2 = new Regex(@"\b(also|although|am|amid|amidst|among|amongst|an|and|anti|anybody|anyone|anything|apart|apart\sfrom|are|been)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non3 = new Regex(@"\b(aren't|around|as|as\sfar\sas|as\ssoon\sas|as\swell\sas|aside|at|atop|away|be|because|because\sof|before)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non4 = new Regex(@"\b(behind|below|beneath|beside|besides|between|betwixt|beyond|but|by|by\smeans\sof|by\sthe\stime|can|cannot)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non5 = new Regex(@"\b(circa|close\sto|com|concerning|considering|could|couldn't|cum|'d|despite|did|didn't|do|does|doesn't|don't)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non6 = new Regex(@"\b(down|due\sto|during|each_other|'em|even\sif|even\sthough|ever|every|every\stime|everybody|everyone)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non7 = new Regex(@"\b(everything|except|far\sfrom|few|first\stime|following|for|from|get|got|had|hadn't|has|hasn't|have)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non8 = new Regex(@"\b(haven't|he|hence|her|here|hers|herself|him|himself|his|how|i|if|in|in\saccordance\swith|in\saddition\sto|in\scase)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non9 = new Regex(@"\b(in\sfront\sof|in\slieu\sof|in\splace\sof|in\sspite\sof|in\sthe\sevent\sthat|in\sto|inside|inside\sof)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non10 = new Regex(@"\b(instead\sof|into|is|isn't|it|itself|just\sin\scase|like|'ll|lots|may|me|mid|might|mightn't|mine|more|most)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non11 = new Regex(@"\b(must|mustn't|myself|near|near\sto|nearest|new|no|no\sone|nobody|none|not|nothing|notwithstanding|now\sthat|of)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non12 = new Regex(@"\b(off|on|on\sbehalf\sof|on\sto|on\stop\sof|once|one|one\sanother|only\sif|onto|opposite|or|org|other|our|any)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non13 = new Regex(@"\b(ours|ourselves|out|out\sof|outside|outside\sof|over|past|per|plenty|plus|prior\sto|qua|re|'re|really|set)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non14 = new Regex(@"\b(regarding|round|'s|said|sans|save|say|says|shall|shan't|she|should|shouldn't|since|so|somebody|its|only)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non15 = new Regex(@"\b(someone|something|than|that|the|thee|their|theirs|them|themselves|there|these|they|thine|this|thou)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non16 = new Regex(@"\b(though|through|throughout|till|to|toward|towards|under|underneath|unless|unlike|until|unto|up|upon|using|even)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non17 = new Regex(@"\b(us|'ve|versus|via|was|wasn't|we|were|weren't|what|when|whenever|where|whereas|whether\sor\snot|things)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non18 = new Regex(@"\b(which|while|who|whoever|whom|why|will|with|with\sregard\sto|withal|within|without|won't|would|wouldn't|mere)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        	    Regex non19 = new Regex(@"\b(ya|ye|yes|you|your|yours|yourself)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));
        
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

            return text;
        }

        public void TFIDF( int id )
        {
            int TotalDoc = db.items.Count();                            //文档总数，TotalDoc和IDF都会实时改变

            var query = from artKey in db.artKeys
                        where artKey.AId == id                          //根据item id找到全部word，存在query结果中
                        select artKey
                        ;
            foreach(var artKey in query){
                double tf = artKey.TF;                                  //这里应该再除以frequency的最大值
                double idf = db.keywords.Find(artKey.word).IDF;         //查询每个word的伪IDF

                artKey.TFIDF = tf * Math.Log( TotalDoc / idf ) ;
            }

            db.SaveChanges();


            /*****************************
             * 把结果存进article表中
             *****************************/
            

        }

        public void analyze( Models.item item )
        {
            string text = item.title + item.description;
            text = text.ToLower();                                            //全部小写

            /******************************
             * replace trivial words by "" 
             ******************************/
            text = rmTrivial(text);


            /******************************
             * remove punctuation, split into word arry.  (Todo: use Regex)
             ******************************/
            char[] sp = new Char[] { ',', '.', ' ', '?', ':', '\'', '‘', '’' };
            string[] split = text.Split(sp, StringSplitOptions.RemoveEmptyEntries);      //建单词array


            /******************************
             * use dictionary to calculate the frequency of each word. 
             ******************************/
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
                * Process artKey table: Saving the TF of each word in this article.
                ******************************/
                string word = map.ElementAt(i).Key;

                Models.artKey artK = new Models.artKey(item.Id, word, map.ElementAt(i).Value);             //add new keyword
                db.artKeys.Add(artK);


               /******************************
                * Process keyword table: For each keyword that appear in this article, plus one for IDF.
                ******************************/
                if (db.keywords.Find(word) == null)                                         //search database
                {
                    Models.keyword kw = new Models.keyword(word, 1);
                    db.keywords.Add(kw);                                                    //add new keyword to DB
                }
                else
                {
                    db.keywords.Find(word).IDF++;
                }

            }

            db.SaveChanges();                 //here should save change to db
        }

    }

}
