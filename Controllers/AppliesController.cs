using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;
using BeanlancerAPI.Services;
using System;
using BeanlancerAPI2.DTOs.Applies;

namespace BeanlancerAPI2.Controllers
{
    [Route("api/Applies")]
    [ApiController]
    public class AppliesController : ControllerBase{
        private readonly IAppliesService _service;
        private readonly IRequestService _request;
        private readonly IRequestHistoryService _history;
        private readonly IDesignerService _designer;
        private readonly IUserService _user;
        private readonly IMapper _mapper;

        public AppliesController(IAppliesService service, 
        IRequestHistoryService history, IMapper mapper, IRequestService request, IUserService user, IDesignerService designer)
        {
            _service = service;
            _history = history;
            _mapper = mapper;
            _request = request;
            _user = user;
            _designer = designer;
        }

        [HttpGet("{id}")]
        public IActionResult GetApplies(string id){
            var idDesigner = _service.GetAppliesByIdRequest(id);
            if (idDesigner == null) return NotFound("No Designer Applies yet!!");
     
            AppliesResponse a;
            List<AppliesResponse> result = new List<AppliesResponse>();    
            for (int i = 0; i < idDesigner.Count(); i++)
            {
                a = new AppliesResponse
                {
                    fullname = _user.GetUserByUsername(_designer.GetDesignerById(idDesigner.ElementAt(i)).Username).Fullname,
                    username = _designer.GetDesignerById(idDesigner.ElementAt(i)).Username,
                    idDesigner = idDesigner.ElementAt(i),
                    time = _service.GetTimeAppliesByIdRequestAndIdDesigner(id, idDesigner.ElementAt(i))
                };
                result.Add(a);
            }
            
            return Ok(result);
        }

        [HttpGet("number")]
        public IActionResult GetAppliesNumber([FromQuery]string id){
            int result = _service.GetAppliesByIdRequest(id).Count();
            return Ok(result);
        }
            
        [HttpPut("Accepted")]
        public IActionResult AccepTheApplies([FromQuery]string id,[FromQuery]string idDesigner){


            var result = _service.AcceptedAppliesStatus(id,idDesigner);
            _service.Commit();
            //------------------------
            var req = _request.GetRequestByID(id);
            // var reqMap = _mapper.Map<Request>(req);
            // //sua cho nay
             req.Status = "In-Process";
             req.IdDesigner = idDesigner;
            _request.Update(req.IdRequest, req);
            _request.Commit();

            //-----------------------------
            int username = _designer.GetDesignerById(idDesigner).Username;
            var user = _user.GetUserByUsername(username);
            RequestHistory hr = new RequestHistory{
                Action = user.Fullname + " has bees ACCEPTED for this request",
                IdRequest = result.IdRequest
            };
            _history.CreateHistoryRequest(hr);
            _history.Commit();

        
            return Ok(result);
        }
    }
}