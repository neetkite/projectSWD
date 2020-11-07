using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Payment
    {
        public int IdPayment { get; set; }
        public string IdRequest { get; set; }
        public DateTime? Time { get; set; }

        public virtual Request IdRequestNavigation { get; set; }
    }
}
