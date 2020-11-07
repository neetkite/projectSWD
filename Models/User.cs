using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class User
    {
        public User()
        {
            Designer = new HashSet<Designer>();
            Request = new HashSet<Request>();
            Wallet = new HashSet<Wallet>();
        }

        public int Username { get; set; }
        public string Fullname { get; set; }
        public string Password{get;set;}
        public int IdRole { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int Status {get; set;}
        public virtual Role IdRoleNavigation { get; set; }
        public virtual ICollection<Designer> Designer { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<Wallet> Wallet { get; set; }
    }
}
