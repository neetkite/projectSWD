using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Request = new HashSet<Request>();
        }

        public int IdCategories { get; set; }
        public string NameCategories { get; set; }

        public virtual ICollection<Request> Request { get; set; }
    }
}
