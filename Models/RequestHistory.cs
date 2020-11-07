using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class RequestHistory
    {
        public string IdRequest { get; set; }
        public string Action { get; set; }
        public DateTime Time { get; set; }

        public virtual Request IdRequestNavigation { get; set; }
    }
}
