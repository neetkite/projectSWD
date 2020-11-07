using BeanlancerAPI.Models;
using BeanlancerAPI.Services;
using BeanlancerAPI2.DTOs.User;
using BeanlancerAPI2.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BeanlancerAPI2.Services
{
    public class UserService : BaseService<User, int>, IUserService
    {
        public UserService(BeanlancersContext context) : base(context)
        {
        }


        public UserResponse Authenticate(string tokenId, string fcm)
        {
            var handler = new JwtSecurityTokenHandler();

            var idToken = handler.ReadJwtToken(tokenId).Claims.ToList();

            var email = idToken.Where(x => x.Type.Equals("email")).FirstOrDefault().Value;
            if (GetUserByEmail(email) != null)
            {
                var user = GetUserByEmail(email);
                UserAuthorize au = new UserAuthorize
                {
                    email = email,
                    fullname = user.Fullname, 
                };
                if (user.IdRole == 2) au.role = "User";
                
                UserResponse result = new UserResponse
                {
                    userID = user.Username,
                    fullname = user.Fullname,
                    email = email,
                    phone = user.Phone,
                    token = GenerateToken(au)
                };
                return result;
            }
            else {
                var user = CreateUser(new User
                {
                    Fullname = idToken.Where(x => x.Type.Equals("name")).FirstOrDefault().Value,
                    Email = email,
                });
                Commit();

                UserAuthorize au = new UserAuthorize
                {
                    email = email,
                    fullname = user.Fullname,
                };
                if (user.IdRole == 2) au.role = "User";

                UserResponse result = new UserResponse
                {
                    userID = user.Username,
                    fullname = user.Fullname,
                    email = email,
                    phone = user.Phone,
                    token = GenerateToken(au)
                };
            return result;
            }
        }

         public string GenerateToken(UserAuthorize userInfor){
             var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this_is_a_super_long_and_hard_Key_And_I_Am_Super_man$smesk.in"));
             var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            if(userInfor.role == "User") claims.Add(new Claim(ClaimTypes.Role, "User"));

            claims.Add(new Claim(ClaimTypes.Email, userInfor.email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
           
            var token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "readers",
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials : credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
         }        
        public User CreateUser(User _user)
        {
            _user.Password = "";
            _user.Status = 1;
            _user.IdRole = 2;
            return Create(_user);
        }

        public User DeleteUserById(int _username)
        {
            return DeleteById(_username);
        }

        public IEnumerable<User> GetAllUserNonPaging(string name)
        {
            if (string.IsNullOrEmpty(name)) return GetAllNonPaging();
            return GetAllNonPaging(a => a.Fullname.Contains(name));
        }

        public IEnumerable<User> GetAllUserPaging(int _numpage, int _pagesize,string name)
        {
            if(string.IsNullOrEmpty(name)) return GetAll(_numpage, _pagesize, a => a.IdRole != 1);
            return GetAll(_numpage, _pagesize, a => a.Fullname.Contains(name) && a.IdRole != 1);
        }

        public User GetUserByEmail(string _email)
        {
            return GetByExpress(a => a.Email.Equals(_email));
        }

        public User GetUserByUsername(int _username)
        {
            return GetById(_username);
        }

        public User UpdateUser(User _user, int _username)
        {
            return Update(_username, _user);
        }

        public User loginUser(LoginUsers users)
        {
            var admin = GetUserByEmail(users.email);
            if(admin.Password.Equals(users.password)){
                return admin;
            }
            return null;
        }

        public IEnumerable<User> GetUserByName(string name)
        {
            return GetAllNonPaging(a => a.Fullname.Contains(name));
        }

        public IEnumerable<RequestsResponse> getListRequestAndFullname(IEnumerable<Request> listRequest)
        {
            List<RequestsResponse> result = new List<RequestsResponse>();
            for (int i = 0; i < listRequest.Count(); i++)
            {
                result.Add(new RequestsResponse
                {
                    IdRequest = listRequest.ElementAt(i).IdRequest,
                    Username = listRequest.ElementAt(i).Username,
                    Fullname = GetUserByUsername(listRequest.ElementAt(i).Username).Fullname,
                    NameRequest = listRequest.ElementAt(i).NameRequest,
                    Description = listRequest.ElementAt(i).Description,
                    BeanAmount = listRequest.ElementAt(i).BeanAmount,
                    Requirement = listRequest.ElementAt(i).Requirement,
                    TimeExpired = listRequest.ElementAt(i).TimeExpired,
                    IdBudget = listRequest.ElementAt(i).IdBudget,
                    IdCategories = listRequest.ElementAt(i).IdCategories,
                    Status = listRequest.ElementAt(i).Status
                });
            }
            return result;
        }

        public RequestsResponse getRequestAndFullname(Request request)
        {
            RequestsResponse rq = new RequestsResponse
            {
                IdRequest = request.IdRequest,
                Username = request.Username,
                Fullname = GetUserByUsername(request.Username).Fullname,
                NameRequest = request.NameRequest,
                Description = request.Description,
                BeanAmount = request.BeanAmount,
                Requirement = request.Requirement,
                TimeExpired = request.TimeExpired,
                IdBudget = request.IdBudget,
                IdCategories = request.IdCategories,
                Status = request.Status
            };
            return rq;
        }
    }
}
