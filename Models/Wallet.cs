using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Wallet
    {
        public int IdWallet { get; set; }
        public int BeanAmout { get; set; }
        public int Username { get; set; }

        public virtual User UsernameNavigation { get; set; }
    }
}
