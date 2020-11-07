using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class AttachmentFile
    {
        public string Path { get; set; }
        public string IdRequest { get; set; }

        public virtual Request IdRequestNavigation { get; set; }
    }
}
