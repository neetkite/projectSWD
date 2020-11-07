using System;
using System.Collections.Generic;
using BeanlancerAPI2.Models;

namespace BeanlancerAPI.Models{
    
    public partial class RequestSkill{
        public int IdSkill{get; set;}
        public string IdRequest{get; set;}


        public virtual Request IdRequestNavigation {get; set;}
        public virtual Skill IdSkillNavigation{get; set;}
    }
}