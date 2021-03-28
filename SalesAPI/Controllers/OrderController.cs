using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesLib.Models;
using SalesLib.Repositories;

namespace SalesAPI.Controllers
{
    [ApiController]
    [Route("Order/[action]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderController(ILogger<OrderController> logger,
                               IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [Route("{search?}")]
        public async Task<IActionResult> GetAll(string search)
        {
            return Ok(await _orderRepository.GetAll(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _orderRepository.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order model)
        {
            var response = new DataResponseModel<string>();
            await _orderRepository.Add(model);
            response.Success = true;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Order model)
        {
            var response = new DataResponseModel<string>();
            await _orderRepository.Edit(model);
            response.Success = true;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = new DataResponseModel<string>();
            if (id > 0)
            {
                await _orderRepository.Delete(id);
                response.Success = true;
            }
            return Ok(response);
        }
    }
}
