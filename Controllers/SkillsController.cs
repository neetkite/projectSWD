using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeanlancerAPI2.Controllers
{
    [Route("api/Skills")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class SkillsController : ControllerBase{

        private readonly ISkillService _service;

        public SkillsController(ISkillService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllSkills(){
            var result = _service.getAllSkill();
            
            return Ok(result);
       }

        [HttpGet("{id}")]
        public IActionResult GetSkillId(int id){
            var result = _service.GetSkillById(id);
            return Ok(result);
        }

    }


}