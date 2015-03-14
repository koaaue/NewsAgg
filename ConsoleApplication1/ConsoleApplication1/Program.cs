using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String article = "This text is used for detecting the keywords. The keywords of this text should be keywords.";
            article = article.ToLower();                                        //全部小写

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
