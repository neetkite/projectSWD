using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Services
{
    public interface ICategoriesService : IBaseService<Categories, int>
    {
        Categories GetCategoriesById(int id);
    }


    public class CategoriesService : BaseService<Categories, int>, ICategoriesService
    {
        public CategoriesService(BeanlancersContext context) : base(context)
        {
        }

        public Categories GetCategoriesById(int id)
        {
            return GetById(id);
        }
    }
}
