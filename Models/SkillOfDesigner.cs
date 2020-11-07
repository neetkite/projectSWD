using System;
using System.Collections.Generic;

namespace BeanlancerAPI2.Models
{
    public partial class SkillOfDesigner
    {
        public string IdDesigner { get; set; }
        public int IdSkill { get; set; }

        public virtual Designer IdDesignerNavigation { get; set; }
        public virtual Skill IdSkillNavigation { get; set; }
    }
}
