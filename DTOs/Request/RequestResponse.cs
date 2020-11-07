using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.DTOs.Request
{
    public class RequestResponse
    {
        public string Username { get; set; }
        public string NameRequest { get; set; }
        public string Description { get; set; }
        public int? BeanAmount { get; set; }
        public string Requirement { get; set; }
        public DateTime? TimeExpired { get; set; }
        public int? IdBudget { get; set; }
        public int? IdCategories { get; set; }
        public string Status { get; set; }
        public string IdDesigner { get; set; }
    }
}
