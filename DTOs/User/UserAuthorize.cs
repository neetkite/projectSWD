using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.DTOs.User
{
    public class UserAuthorize
    {
        public string email { get; set; }
        public string fullname { get; set; }
        public string role { get; set; }
    }
}
