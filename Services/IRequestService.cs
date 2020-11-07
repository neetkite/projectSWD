using BeanlancerAPI2.DTOs.Request;
using BeanlancerAPI2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeanlancerAPI.Services
{
    public interface IRequestService : IBaseService<Request, string>
    {
        Request CreateRequest(Request _entity);
        Request UpdateRequest(Request _entity,string _id);
        Request DeleteRequest(string _id);
        IEnumerable<Request> GetAllRequest(int numpage, int perpage);
        IEnumerable<Request> GetAllByUsername(int numpage, int perpage, int username);
        Request GetRequestByID(string id);
        IEnumerable<Request> DisableAllRequestByUsername(int _username);
        IEnumerable<Request> GetRequestByUsernameAndStatus(int username, string status);
        IEnumerable<Request> GetAllByNameAndStatusAndCate(int numpage, int _pagesize, string name, string status, int cate);
        IEnumerable<Request> GetByNameAndStatusAndCateNonPaging(string name, string status, int cate);
        IEnumerable<Request> GetRequestNonPaging();

    }
}
