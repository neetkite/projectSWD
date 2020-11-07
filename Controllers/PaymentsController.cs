using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanlancerAPI2.Controllers
{
    [Route("api/Payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly IRequestService _request;
        private readonly IWalletService _wallet;
        private readonly IRequestHistoryService _history;
        
        public PaymentsController(IPaymentService service, IRequestService request, IWalletService wallet, IRequestHistoryService history)
        {
            _service = service;
            _request = request;
            _wallet = wallet;
            _history = history;
            
        }

        // GET: api/<PaymentsController>
        [HttpGet("requestId")]
        public IActionResult Get([FromQuery] string requestId)
        {
            var result = _service.GetPaymentByRequest(requestId);
            return Ok(result);
        }




        [HttpPost]
        public IActionResult Post([FromBody]Payment payment)
        {
            var result = _service.CreatePayment(payment);
            _service.Commit();
            //-------------------------
            var req = _request.GetRequestByID(payment.IdRequest);
            if (req.Status == "Done")
            {
                req.Status = "Completed";
                _request.UpdateRequest(req, req.IdRequest);
                _request.Commit();
            
            //------------------------
            var wallet = _wallet.GetWalletByUsername(req.Username);
            wallet.BeanAmout = wallet.BeanAmout - req.BeanAmount;
            _wallet.UpdateWallet(wallet.IdWallet, wallet);
            _wallet.Commit();
            //----------------
            var hr = new RequestHistory
            {
                IdRequest = req.IdRequest,
                Action = "Completed Request"
            };
            _history.CreateHistoryRequest(hr);
            _history.Commit();

            return Ok(result);
            }
            return NotFound("Can't pay for this request anymore");
        }

    }
}
