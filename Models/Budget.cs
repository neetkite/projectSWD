using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Budget
    {
        public Budget()
        {
            Request = new HashSet<Request>();
        }

        public int IdBudget { get; set; }
        public string RangeBudget { get; set; }

        public virtual ICollection<Request> Request { get; set; }
    }
}
