using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanlancerAPI2.Controllers
{
    [Route("api/Wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _service;
        public WalletsController(IWalletService service)
        {
            _service = service;
        }

        // GET: api/<WalletsController>
        [HttpGet("{username}")]
        public IActionResult Get(int username)
        {
            var result = _service.GetWalletByUsername(username);
            if (result == null) return NotFound("Not User found");
            
            return Ok(result);
        }

        // GET api/<WalletsController>/5
        
        // POST api/<WalletsController>
        
    }
}
