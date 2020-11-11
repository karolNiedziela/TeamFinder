using System;
using System.Collections.Generic;
using System.Text;

namespace TeamFinderDTO
{
    public class SearchResult
    {
        public SearchResultType Type { get; set; }
        
        public SessionResponse Session { get; set; }

        public PlayerResponse Player { get; set; }
    }

    public enum SearchResultType
    {
        Player,
        Session
    }
}
