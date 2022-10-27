using System;
using System.Collections.Generic;

namespace RecordApp.Models
{
    public class SearchResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<SearchResponse>(myJsonResponse);

        public string query { get; set; }
        public Albums albums { get; set; }
        
        public class Albums
        {
            public int totalCount { get; set; }
            public List<Item> items { get; set; }
            public PagingInfo pagingInfo { get; set; }
        }

        public class Item
        {
            public Data data { get; set; }
            public string uri { get; set; }
            public Profile profile { get; set; }
        }

        public class Data
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Artists artists { get; set; }
            public CoverArt coverArt { get; set; }
            public Date date { get; set; }
        }

        public class Artists
        {
            public List<Item> items { get; set; }
        }

        public class CoverArt
        {
            public List<Source> sources { get; set; }
        }

        public class Date
        {
            public int year { get; set; }
        }

        public class Profile
        {
            public string name { get; set; }
        }

        public class Source
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class PagingInfo
        {
            public int nextOffset { get; set; }
            public int limit { get; set; }
        }

    }
}
