using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeanlancerAPI2.Services{

public interface IDesignerService : IBaseService<Designer,string>{
    
   
    Designer GetDesignerById(string id);
    Designer GetDesignerByUsername(int id);
    
}

    public class DesignerService : BaseService<Designer, string>, IDesignerService
    {
        public DesignerService(BeanlancersContext context) : base(context)
        {
        }

        public Designer GetDesignerById(string id)
        {
            return GetById(id);
        }



        
        public Designer GetDesignerByUsername(int id)
        {
            return GetByExpress(a => a.Username == id);
        }
    }

}
