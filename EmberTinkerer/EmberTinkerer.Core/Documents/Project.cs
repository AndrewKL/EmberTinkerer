using System;
using System.Collections.Generic;

namespace EmberTinkerer.Core.Documents
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Tags { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTimeOffset DateAdded { get; set; }

        public string Html { get; set; }
        public string Javascript { get; set; }
        

        public int GetIntId()
        {
            return int.Parse(Id.Substring(Id.IndexOf("/") + 1)); 
        }
    }
}