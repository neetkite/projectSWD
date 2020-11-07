using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace  BeanlancerAPI2.Services
{
    public interface IRequestHistoryService : IBaseService<RequestHistory,string>{

        IEnumerable<RequestHistory> GetRequestHistoriesByRequestID(string id);

        RequestHistory CreateHistoryRequest(RequestHistory request);

        IEnumerable<RequestHistory> DeleteRequestHistoryByRequest(string id);
    }

    public class RequestHistoryService : BaseService<RequestHistory, string>, IRequestHistoryService
    {
        public RequestHistoryService(BeanlancersContext context) : base(context)
        {
        }

        public RequestHistory CreateHistoryRequest(RequestHistory request)
        {
            
            DateTime time = DateTime.Now;
            request.Time = time;
            return Create(request);
        }

        public IEnumerable<RequestHistory> DeleteRequestHistoryByRequest(string id)
        {
            var list = GetRequestHistoriesByRequestID(id);
            if(list != null)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    DeleteByEntity(list.ElementAt(i));
                }
                Commit();
                return GetRequestHistoriesByRequestID(id);
            }
            return null;
        }

        public IEnumerable<RequestHistory> GetRequestHistoriesByRequestID(string id)
        {
            return GetAllNonPaging(a => a.IdRequest.Equals(id));
        }
    }
}