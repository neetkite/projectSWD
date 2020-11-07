using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class CategoriesOfDesigner
    {
        public string IdDesigner { get; set; }
        public int IdCategories { get; set; }

        public virtual Categories IdCategoriesNavigation { get; set; }
        public virtual Designer IdDesignerNavigation { get; set; }
    }
}
