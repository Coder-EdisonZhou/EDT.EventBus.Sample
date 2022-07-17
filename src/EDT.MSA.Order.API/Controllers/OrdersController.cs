using AutoMapper;
using EDT.MSA.API.Shared.Utils;
using EDT.MSA.Ordering.API.Models;
using EDT.MSA.Ordering.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IList<OrderVO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(_mapper.Map<IList<OrderVO>>(orders));
        }

        [HttpGet("id")]
        public async Task<ActionResult<OrderVO>> GetOrder(string id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderVO>(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderVO>> CreateOrder(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            // 01.生成订单初始数据
            order.OrderId = SnowflakeGenerator.Instance().GetId().ToString();
            order.CreatedDate = DateTime.Now;
            order.Status = OrderStatus.Pending;
            // 02.订单数据提交
            await _orderService.CreateOrder(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, _mapper.Map<OrderVO>(order));
        }
    }
}
