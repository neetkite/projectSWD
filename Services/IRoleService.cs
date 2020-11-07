using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;

namespace  BeanlancerAPI2.Services
{
    public interface IRoleService : IBaseService<Role, int>{
        
        string getRoleById(int id);
    }


    public class RoleService : BaseService<Role, int>, IRoleService
    {
        public RoleService(BeanlancersContext context) : base(context)
        {
        }

        public string getRoleById(int id)
        {
            return GetById(id).RoleName;
        }
    }
}