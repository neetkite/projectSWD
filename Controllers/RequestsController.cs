using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeanlancerAPI.Models;
using BeanlancerAPI.Services;
using BeanlancerAPI2.DTOs.Request;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanlancerAPI.Controllers
{
    [Route("api/Requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _service;
        private readonly IMapper _mapper;
        private readonly IRequestHistoryService _history;
        private readonly IRequestSkillService _skill;
        private readonly IPaymentService _payment;
        private readonly IUserService _user;
        

        public RequestsController(IRequestService service, IMapper mapper, IRequestHistoryService history, IUserService user, IRequestSkillService skill, IPaymentService payment)
        {
            _service = service;
            _mapper = mapper;
            _history = history;
            _user = user;
            _skill = skill;
            _payment = payment;
        }
   

        [HttpGet("admin")]
        public IActionResult GetByCateAndNameAndStatus([FromQuery] int pagenumber, [FromQuery] int pagesize, [FromQuery]string status, [FromQuery]string name, [FromQuery]int cate){
            var listRequest = _service.GetAllByNameAndStatusAndCate(pagenumber,pagesize,name,status,cate);
            if (listRequest == null) return NotFound("No Request Found");

            var result = _user.getListRequestAndFullname(listRequest);
            // get total page
            var totalpage = _service.GetByNameAndStatusAndCateNonPaging(name, status, cate).Count() / pagesize;
            if (_service.GetByNameAndStatusAndCateNonPaging(name, status, cate).Count() % pagesize != 0) totalpage++;

            return Ok(new { 
                Data = result,
                Paging = new
                {
                    totalPage = totalpage
                }
            });
        }


//..................................................................................................................
        [HttpGet("user")]
        public IActionResult GetRequestByUsername([FromQuery]int username, [FromQuery] string status){
            
            var result = _service.GetRequestByUsernameAndStatus(username,status);
            if(result == null) return NotFound("This guy hadn't posted any request yet");
            return Ok(result);
        }


        

        

        // GET api/<RequestsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var request = _service.GetRequestByID(id);
            if (request == null) return NotFound("Request Not Found!!");
            var result = _user.getRequestAndFullname(request);

            return Ok(result);
        }

        // POST api/<RequestsController>
        [HttpPost]
        public IActionResult Post(Request request)
        {
            try
            {          
                //tao request vao database
                 var result = _service.CreateRequest(request);
                _service.Commit();

                //them vao history
                var user = _user.GetUserByUsername(result.Username);
                string action = user.Fullname + "has created request";
                RequestHistory history = new RequestHistory{
                    IdRequest = result.IdRequest,
                    Action = action
                };
                _history.CreateHistoryRequest(history);
                _history.Commit();

                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // PUT api/<RequestsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id,[FromBody]RequestRequest requestRequest)
        {
            try
            {
                //edit request trong database
                var request = _mapper.Map<Request>(requestRequest);
                var result = _service.UpdateRequest(request,id);
                _service.Commit();
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // DELETE api/<RequestsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {


            if (_service.GetRequestByID(id) == null) return NotFound();

            var check = _payment.DeletePaymentsByIdRequest(id);
            if (check.Count() != 0) return NotFound("Delete Payment Fail");
            
            var checkSkill = _skill.DeleteByRequestId(id);
            if (check.Count() != 0) return NotFound("Delete skill Fail");

            var checkHis = _history.DeleteRequestHistoryByRequest(id);
            if (checkHis.Count() != 0) return NotFound("Delete History fail");           

             _service.DeleteById(id);
            _service.Commit();
            return Ok("Deleted");
        }
    }
}
