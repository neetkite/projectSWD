using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeanlancerAPI2.Services{
    public interface IAppliesService : IBaseService<Applies,string>{
     IEnumerable<string> GetAppliesByIdRequest(string id);

     IEnumerable<Applies> GetAppliesById(string id);

     Applies CreateApplies(Applies applies);

    DateTime GetTimeAppliesByIdRequestAndIdDesigner(string idRequest, string idDesigner); 

     Applies AcceptedAppliesStatus(string idRequest,string idDesigner);   
    
    }


    public class AppliesService : BaseService<Applies, string>, IAppliesService
    {
        public AppliesService(BeanlancersContext context) : base(context)
        {
        }

        public Applies CreateApplies(Applies applies)
        {
            applies.Status = "On-queue";
            return Create(applies);
        }

        public IEnumerable<string> GetAppliesByIdRequest(string id)
        {
            var listApplies = GetAppliesById(id);
            List<string> listDes = new List<string>();
            for (int i = 0; i < listApplies.Count(); i++)
            {
                listDes.Add(listApplies.ElementAt(i).IdDesigner);
            }
            return listDes;
        //     List<int> listUsername = new List<int>();
        //     //tìm thg user trong bảng designer -> lấy username tìm ra User r trả về List User
        //     for (int i = 0; i < listDes.Count(); i++)
        //     {
        //         var desUser = _designer.GetDesignerById(listDes.ElementAt(i)).Username;
        //         listUsername.Add(desUser);
        //     }
        //     //---------- tim tới thg user
        //     List<User> listUser = new List<User>();
        //     for (int i = 0; i < listUsername.Count(); i++)
        //     {
        //         var user = _user.GetUserByUsername(listUsername.ElementAt(i));
        //         listUser.Add(user);
        //     }
        //     return listUser;
         }


        public Applies AcceptedAppliesStatus(string idRequest,string idDesigner)
        {
            DateTime time = DateTime.Now;
            var appliesAccept = GetByExpress(a => a.IdRequest.Equals(idRequest) && a.IdDesigner.Equals(idDesigner));
            appliesAccept.Status = "Accepted";
            appliesAccept.Time = time;
              
            var appliesList = GetAllNonPaging(b => b.IdRequest.Equals(idRequest));
            
            for(int i = 0 ; i < appliesList.Count(); i++){
                   DeleteByEntity(appliesList.ElementAt(i)); 
            }
            Commit();
            return Create(appliesAccept);
        }

        public IEnumerable<Applies> GetAppliesById(string id)
        {
            return GetAllNonPaging(a => a.IdRequest == id);
        }

        public DateTime GetTimeAppliesByIdRequestAndIdDesigner(string idRequest, string idDesigner)
        {
            return GetByExpress(a => a.IdDesigner == idDesigner && a.IdRequest == idRequest).Time;
        }
    }

}