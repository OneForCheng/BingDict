using System;
using System.Net;

namespace BingDict
{
    class Program
    {
        static void Usage(string fileName)
        {
            var spaces = new string(' ', 4);
            if (fileName != string.Empty)
            {
                if (fileName.EndsWith("_"))
                {
                    fileName = fileName.Substring(0, fileName.Length - 1);
                }
            }
            Console.WriteLine("{0}Bing在线单词翻译。{1}", Environment.NewLine, Environment.NewLine);
            Console.WriteLine("{0} Word [..]{1}", fileName, Environment.NewLine);
            Console.WriteLine("{0}Word    待翻译单词。", spaces);
           
        }

        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || args[0] == "/?")
                {
                    var fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
                    fileName = fileName.Substring(0, fileName.LastIndexOf('.')).ToUpper();
                    Usage(fileName);
                }
                else
                {
                    var str = string.Empty;
                    for (var i = 0; i < args.Length; i++)
                    {
                        if (i > 0) str += " ";
                        str += args[i];
                    }
                    var dict = new BingDictionary();
                    var result = dict.SearchWord(str);
                    if (result != null)
                    {
                        dict.PrintResult(result);
                    }
                    else
                    {
                        Console.WriteLine("无法翻译单词:{0}", str);
                    }
                }
            }
            catch (WebException)
            {
                Console.WriteLine("网络连接超时!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
            return 0;
        }
    }
}
