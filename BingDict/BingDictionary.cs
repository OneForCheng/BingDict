using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace BingDict
{
    public class BingDictionary
    {
        readonly int _samplesLimit;
        readonly ConsoleColor _oldFgColor;

        public BingDictionary(int samplesLimit = 5)
        {
            _samplesLimit = samplesLimit;
            _oldFgColor = Console.ForegroundColor;
        }

        public QueryResult SearchWord(string word)
        {
            var value = RequestWord(word);
            return value.StartsWith("An error occurs.") ? null : ParseJson(value);
        }

        #region 输出查询结果
        public void PrintResult(QueryResult result)
        {
            PrintWord(result);
            PrintPronunciation(result);
            PrintDefinitions(result);
            PrintSamples(result);
        }

        void PrintWord(QueryResult result)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result.word);
            Console.ForegroundColor = _oldFgColor;
           
        }

        void PrintPronunciation(QueryResult result)
        {
            if (result.pronunciation != null)
            {
                Console.WriteLine();
                foreach (var item in result.pronunciation)
                {
                    if (item.Key == "AmE")
                    {
                        Console.Write("US [{0}] ", item.Value);
                    }
                    else if (item.Key == "BrE")
                    {
                        Console.Write("UK [{0}] ", item.Value);
                    }
                }
                Console.WriteLine();
            }
        }

        void PrintDefinitions(QueryResult result)
        {
            
            if (result.defs != null)
            {
                Console.WriteLine();
                for (var i = 0; i < Math.Min(_samplesLimit, result.defs.Length); i++)
                {
                    foreach (var definition in result.defs[i])
                    {
                        if (definition.Key == "pos")
                        {
                            Console.Write($"[{definition.Value}] ".PadRight(8));
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(definition.Value);
                            Console.ForegroundColor = _oldFgColor;
                          
                        }
                    }
                }
            }
        }

        void PrintSamples(QueryResult result)
        {
            if (result.sams != null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Samples:");
                Console.ForegroundColor = _oldFgColor;
                for (var i = 0; i < Math.Min(_samplesLimit, result.sams.Length); i++)
                {
                    Console.Write("{0}. ", i + 1);
                    foreach (var sample in result.sams[i])
                    {
                        if (sample.Key != "mp3Url" && sample.Key != "mp4Url")
                        {
                            Console.WriteLine(sample.Value);
                        }
                    }
                }
            }
            Console.WriteLine();
        }
        #endregion

        string RequestWord(string word)
        {
            const string prefix = "http://xtk.azurewebsites.net/BingDictService.aspx?Word=";
            var url = prefix + HttpUtility.UrlEncode(word);
            var request = WebRequest.Create(url);
            request.Timeout = 1000 * 5;
            using (var response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            
        }

        QueryResult ParseJson(string value)
        {
            var result = JsonConvert.DeserializeObject<QueryResult>(value);
            return result;
        }
    }
}

