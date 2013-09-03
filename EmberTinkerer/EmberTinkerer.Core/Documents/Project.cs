using System;
using System.Collections.Generic;

namespace EmberTinkerer.Core.Documents
{
    public class Project
    {
        public string Id { get; set; }
        public string name { get; set; }
        public ICollection<string> tags { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public int rating { get; set; }
        public int upvotes { get; set; }
        public int downvotes { get; set; }
        public DateTimeOffset dateAdded { get; set; }

        public string html { get; set; }
        public string javascript { get; set; }
        

        public int GetIntId()
        {
            return int.Parse(Id.Substring(Id.IndexOf("/") + 1)); 
        }
        public void SetId(int id)
        {
            Id = "Project/" + id;
        }
    }
}