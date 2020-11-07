using BeanlancerAPI.Models;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeanlancerAPI2.Services
{
    public interface IRequestSkillService : IBaseService<RequestSkill,string>{
        IEnumerable<RequestSkill> getSkillByIdRequest(string id);
        IEnumerable<RequestSkill> GetAllSkillRequest();
        RequestSkill CreateRequestSkill(RequestSkill skill);

        IEnumerable<RequestSkill> CreateRequestSkills(List<RequestSkill> listSkill);
        IEnumerable<RequestSkill> DeleteByRequestId(string requestId);
        
    }

    public class RequestSkillService : BaseService<RequestSkill, string>, IRequestSkillService
    {
        public RequestSkillService(BeanlancersContext context) : base(context)
        {
        }


        public RequestSkill CreateRequestSkill(RequestSkill skill)
        {
            return Create(skill);
        }

        public IEnumerable<RequestSkill> CreateRequestSkills(List<RequestSkill> listSkill)
        {
            for (int i = 0; i < listSkill.Count(); i++)
            {
                Create(listSkill.ElementAt(i));
            }
            return getSkillByIdRequest(listSkill.ElementAt(0).IdRequest);
        }

        public IEnumerable<RequestSkill> DeleteByRequestId(string requestId)
        {
            var list = getSkillByIdRequest(requestId);
            if(list != null){
                for(int i = 0; i < list.Count(); i++){
                    DeleteByEntity(list.ElementAt(i));
                }
                Commit();
                return getSkillByIdRequest(requestId);
            }
            return null;
        }

        public IEnumerable<RequestSkill> GetAllSkillRequest()
        {
            return GetAllNonPaging();
        }

        public IEnumerable<RequestSkill> getSkillByIdRequest(string id)
        {
            return GetAllNonPaging(a => a.IdRequest == id);
        }
    }

}