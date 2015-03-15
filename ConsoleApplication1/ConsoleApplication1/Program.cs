using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private static Regex non1 = new Regex(@"\b(a|aboard|about|above|absent|according\sto|across|after|against|ago|ahead\sof|ain't|all|along|alongside)\b", (RegexOptions.IgnoreCase | RegexOptions.Compiled));

        static void Main(string[] args)
        {
            String article = "Some nonsense word: about across, above, after. This text is used for detecting the keywords. The keywords of this text should be keywords.";
            article = article.ToLower();                                        //全部小写

            /****** removing trivial words *****/
            string text = article;
            text = non1.Replace(text, "");

            Console.WriteLine(text);




            /** 应该建立更复杂的split算法，得用Regex */
            string[] split = article.Split(new Char[] { ',', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);      //建单词array
            long totalWord = split.Length;

            Dictionary<string, int> dic = new Dictionary<string, int>();        //Map结构, key是单词，value是词频

            for (int i = 0; i < split.Length; i++)
            {
                if (dic.ContainsKey(split[i]))
                    dic[split[i]]++;                                            //词频加1
                else
                    dic.Add(split[i], 1);

                //Console.WriteLine(split[i]);
            }

            for (int i = 0; i < dic.Count; i++)
            {
                Console.WriteLine(dic.ElementAt(i).Key + " : " + dic.ElementAt(i).Value);
            }

            Console.ReadLine();                                                 //停在console
        }
    }
}
