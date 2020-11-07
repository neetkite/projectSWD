using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeanlancerAPI.Models;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Cache;
using BeanlancerAPI2.DTOs.User;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanlancerAPI2.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IRequestService _request;
        private readonly IMapper _map;
        private readonly IWalletService _WalletService;
        private readonly IRoleService _role;

        public UsersController(IUserService service, IMapper map, IWalletService walletService, IRequestService request, IRoleService role)
        {
            _service = service;
            _map = map;
            _WalletService = walletService;
            _request = request;
            _role = role;
        }


        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult GetPaging([FromQuery]int pagenum, [FromQuery]int pagesize,[FromQuery] string name)
        {
            var list = _service.GetAllUserPaging(pagenum, pagesize, name);
            var list2 = _service.GetAllUserNonPaging(name);
            int pagnum = list2.Count() / pagesize;
            if (list2.Count() % pagesize != 0) pagnum++;
            return Ok(new
            {
                Data = list,
                Paging = new
                {
                    pagenumber = pagnum
                }
            }); 
        }

        // GET api/<UsersController>/5
        [HttpGet("{username}")]
       // [Cached(600)]
        public IActionResult getUserById(int username)
        {
            var result = _service.GetUserByUsername(username);
            if(result == null)   return NotFound("Invalid Username");
            return Ok(result);
        }


        [HttpPost("Authenticate")]
        public IActionResult PostTestJwt([FromQuery]string token, [FromQuery]string fcm){
            try{
               UserResponse result = _service.Authenticate(token,fcm);
                if (_WalletService.GetWalletByUsername(result.userID) == null)
                {
                    _WalletService.CreateWallet(new Wallet
                    {
                        Username = result.userID
                    });
                    _WalletService.Commit();
                }

                return Ok(result);
            }catch(Exception e){
                return NotFound(e.Message);
            }
        }

        [HttpPost("Login")]
        public  IActionResult AdminLogin(LoginUsers user){
            try{
                var result = _service.loginUser(user);
                if(result == null) return NotFound("Wrong password or email");
                if(_role.getRoleById(result.IdRole) == "Admin"){
                    UserAuthorize au = new UserAuthorize
                    {
                        email = result.Email,
                        fullname = result.Fullname,
                        role = "Admin"
                    };
                return Ok( new {
                    Data = result,
                    TokenKey = _service.GenerateToken(au)
                });
                }
                return Unauthorized("You have not permission to grant access");
            }catch(Exception e){
                return NotFound("Invalid information");
            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            if(value == null) return NotFound("Invalid User");
            

            var result = _service.CreateUser(value);
            _service.Commit();

            Wallet wallet = new Wallet{
                Username = result.Username,
            };
            _WalletService.CreateWallet(wallet);
            _WalletService.Commit();

            return Ok(result);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            var userMap = _map.Map<User>(user);
            var check = _service.GetUserByUsername(id);
            if(check == null) return NotFound("The User is not Exist");
            userMap.Username = id;
            userMap.Email = check.Email;
            userMap.IdRole = check.IdRole;
            
            var result = _service.UpdateUser(userMap, id);;
            _service.Commit();
            return Ok(result);
        }

        [HttpPut("disable/{id}")]
        public IActionResult DisableUser(int id, [FromBody] User user){
            var userMap = _map.Map<User>(user);
            var check = _service.GetUserByUsername(id);
            if(check == null) return NotFound("The User is not Exist");
            userMap = check;
            userMap.Status = 0;
            var result = _service.UpdateUser(userMap,id);
            _service.Commit();
            //-----------------------
            _request.DisableAllRequestByUsername(userMap.Username);
            _request.Commit();
            return Ok(result);

        }    

        // DELETE api/<UsersController>/5
        
    }
}
