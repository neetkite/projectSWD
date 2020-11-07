using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Designer
    {
        public string IdDesigner { get; set; }
        public int Username { get; set; }

        public virtual User UsernameNavigation { get; set; }
    }
}
