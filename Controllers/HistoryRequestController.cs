using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeanlancerAPI.Services;
using BeanlancerAPI2.DTOs.Request;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeanlancerAPI2.Controllers
{
    [Route("api/History-request")]
    [ApiController]
    public class HistoryRequestController : ControllerBase{
        
        private readonly IRequestHistoryService _service;

        public HistoryRequestController(IRequestHistoryService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetHistoryRequest(string id){
            var result = _service.GetRequestHistoriesByRequestID(id);
            if(result == null) return NotFound("No Request Found");
            return Ok(result);
        }
    }



}