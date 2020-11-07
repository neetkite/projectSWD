using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Applies
    {
        public string IdRequest { get; set; }
        public string IdDesigner { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }

        public virtual Designer IdDesignerNavigation { get; set; }
        public virtual Request IdRequestNavigation { get; set; }
    }
}
