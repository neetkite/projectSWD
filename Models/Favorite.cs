using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Favorite
    {
        public int Username { get; set; }
        public string IdDesigner { get; set; }

        public virtual Designer IdDesignerNavigation { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
