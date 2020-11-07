using System.Collections.Generic;
using System.Linq;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;

namespace BeanlancerAPI2.Services
{
    public interface ICateOfDesignerService : IBaseService<CategoriesOfDesigner,int>{

        IEnumerable<CategoriesOfDesigner> GetDesignersByCategories(int id);

        IEnumerable<CategoriesOfDesigner> GetCateByDesigner(string id);

        IEnumerable<CategoriesOfDesigner> GetDesignersByCategoriesAndName(int id, string name);

    }

    public class CateOfDesignerService : BaseService<CategoriesOfDesigner, int>, ICateOfDesignerService
    {
  
       
        public CateOfDesignerService(BeanlancersContext context) : base(context)
        {
        }

        public IEnumerable<CategoriesOfDesigner> GetCateByDesigner(string id)
        {
            return GetAllNonPaging(a => a.IdDesigner == id);
        }

       

        public IEnumerable<CategoriesOfDesigner> GetDesignersByCategories(int id)
        {
            return GetAllNonPaging(a => a.IdCategories == id);
        }

        public IEnumerable<CategoriesOfDesigner> GetDesignersByCategoriesAndName(int id, string name)
        {
            throw new System.NotImplementedException();
        }
    }
}