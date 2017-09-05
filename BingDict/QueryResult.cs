using System.Collections.Generic;

namespace BingDict
{
    public class QueryResult
    {
        public string word;
        public Dictionary<string,string> pronunciation;
        public Dictionary<string,string>[] defs;
        public Dictionary<string,string>[] sams;
    }
}