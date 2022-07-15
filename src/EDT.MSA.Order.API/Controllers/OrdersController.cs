using AutoMapper;
using DotNetCore.CAP;
using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using EDT.MSA.API.Shared.Utils;
using EDT.MSA.Ordering.API.Models;
using EDT.MSA.Ordering.API.Repositories;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICapPublisher _eventPublisher;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper, ICapPublisher eventPublisher)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IList<OrderVO>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return Ok(_mapper.Map<IList<OrderVO>>(orders));
        }

        [HttpGet("id")]
        public async Task<ActionResult<OrderVO>> GetOrder(string id)
        {
            var order = await _orderRepository.GetOrder(id);
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
            // 02.订单数据存入MongoDB
            await _orderRepository.CreateOrder(order);
            // 03.发布订单已生成事件消息
            await _eventPublisher.PublishAsync(
                name: EventNameConstants.TOPIC_ORDER_SUBMITTED,
                contentObj: new EventData<NewOrderSubmittedEvent>(new NewOrderSubmittedEvent(order.OrderId, order.ProductId, order.Quantity)),
                callbackName: EventNameConstants.TOPIC_STOCK_DEDUCTED
                );

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, _mapper.Map<OrderVO>(order));
        }
    }
}
