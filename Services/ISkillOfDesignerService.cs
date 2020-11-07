using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Services
{
    public interface ISkillOfDesignerService : IBaseService<SkillOfDesigner, string>
    {
        IEnumerable<SkillOfDesigner> GetSkillByDesigner(string id);
    }

    public class SkillOfDesignerService : BaseService<SkillOfDesigner, string>, ISkillOfDesignerService
    {
        public SkillOfDesignerService(BeanlancersContext context) : base(context)
        {
        }

        public IEnumerable<SkillOfDesigner> GetSkillByDesigner(string id)
        {
            return GetAllNonPaging(a => a.IdDesigner == id);
           
        }
    }
}
