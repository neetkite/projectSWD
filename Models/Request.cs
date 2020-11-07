using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class Request
    {
        public Request()
        {
            Payment = new HashSet<Payment>();
        }

        
        public string IdRequest { get; set; }
        public int Username { get; set; }
        public string NameRequest { get; set; }
        public string Description { get; set; }
        public int BeanAmount { get; set; }
        public string Requirement { get; set; }
        public DateTime TimeExpired { get; set; }
        public int IdBudget { get; set; }
        public int IdCategories { get; set; }
        public string Status { get; set; }
        public string IdDesigner { get; set; }

        public virtual Budget IdBudgetNavigation { get; set; }
        public virtual Categories IdCategoriesNavigation { get; set; }
        public virtual User UsernameNavigation { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
    }
}
