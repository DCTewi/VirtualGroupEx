using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualGroupEx.Server.Data
{
    public class SearchResult
    {
        public SearchResultType Type { get; set; }

        public object Result { get; set; }
    }

    public enum SearchResultType
    {
        Mission,
        User,
        Discussion,
        Routine,
    }
}
