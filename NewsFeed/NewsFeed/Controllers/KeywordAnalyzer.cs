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

        public void analyze( Models.item item)
        {
            string text = item.title + item.description;
            text = text.ToLower();                                            //全部小写

            /******************************
             * replace trivial words by "" 
             ******************************/
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



            /** 应该建立更复杂的split算法，得用Regex */
            char[] sp = new Char[] { ',', '.', ' ', '?', ':', '\'', '‘', '’' };
            string[] split = text.Split(sp, StringSplitOptions.RemoveEmptyEntries);      //建单词array

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
                string word = map.ElementAt(i).Key;

                if (db.keywords.Find(word) == null)                                         //search database
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

    }

}
