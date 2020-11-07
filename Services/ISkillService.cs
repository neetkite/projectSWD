using System.Collections.Generic;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;

namespace BeanlancerAPI2.Services{
    public interface ISkillService : IBaseService<Skill,int>{
        IEnumerable<Skill> getAllSkill();

        Skill GetSkillById(int id);
    }


    public class SkillService : BaseService<Skill, int>, ISkillService
    {
        public SkillService(BeanlancersContext context) : base(context)
        {
        }

        public IEnumerable<Skill> getAllSkill()
        {
            return GetAllNonPaging();
        }

        public Skill GetSkillById(int id)
        {
            return GetById(id);
        }
    }
}

