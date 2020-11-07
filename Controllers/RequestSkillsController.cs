using System.Collections.Generic;
using System.Linq;
using BeanlancerAPI.Models;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeanlancerAPI2.Controllers
{
    [Route("api/RequestSkills")]
    [ApiController]
    public class RequestSkillsController : ControllerBase{
        private readonly IRequestSkillService _service;
        private readonly IRequestService _request;

        public RequestSkillsController(IRequestSkillService service, IRequestService request)
        {
            _request = request;
            _service = service;
        }


        [HttpGet("{idRequest}")]
        public IActionResult getSkillIdRequest(string idRequest)
        {
            var result = _service.getSkillByIdRequest(idRequest);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult CreateSkillRequest(List<RequestSkill> requestSkill){

            var checkreq = _request.GetRequestByID(requestSkill.ElementAt(0).IdRequest);
            if (checkreq == null) return NotFound("Request Not Found");
            _service.CreateRequestSkills(requestSkill);
            _service.Commit();
            
            if(requestSkill != null) 
                return Ok(_service.getSkillByIdRequest(requestSkill.ElementAt(0).IdRequest));
            return NotFound("No Skill Found");
        }

        [HttpDelete("{requestId}")]
        public IActionResult DeleteSkillRequest(string requestId){
            var result = _service.DeleteByRequestId(requestId);
            if (result != null) return NotFound("Delete skill Failed");
            _service.Commit();
            return Ok(result);
        }
        

    }
}