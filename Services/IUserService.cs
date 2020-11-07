using BeanlancerAPI.Models;
using BeanlancerAPI.Services;
using BeanlancerAPI2.DTOs.User;
using BeanlancerAPI2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Services
{
    public interface IUserService : IBaseService<User, int>
    {
        User CreateUser(User _user);
        User UpdateUser(User _user, int _username);
        User DeleteUserById(int _username);
        IEnumerable<User> GetAllUserPaging(int _numpage, int _pagesize, string name);
        IEnumerable<User> GetAllUserNonPaging(string name);
        User GetUserByEmail(string _email);
        User GetUserByUsername(int _username);
        UserResponse Authenticate(string uid, string mess);
        string GenerateToken(UserAuthorize au);
        IEnumerable<RequestsResponse> getListRequestAndFullname(IEnumerable<Request> listRequest);

        RequestsResponse getRequestAndFullname(Request request);
        IEnumerable<User> GetUserByName(string name);
            
        User loginUser(LoginUsers user);
    }

}
