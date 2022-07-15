using DotNetCore.CAP;
using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using EDT.MSA.Ordering.API.Models;
using EDT.MSA.Ordering.API.Repositories;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Services
{
    public class ProductStockDeductedEventService : IProductStockDeductedEventService, ICapSubscribe
    {
        private readonly IOrderRepository _orderRepository;

        public ProductStockDeductedEventService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [CapSubscribe(name: EventNameConstants.TOPIC_STOCK_DEDUCTED, Group = EventNameConstants.GROUP_STOCK_DEDUCTED)]
        public async Task MarkOrderStatus(EventData<ProductStockDeductedEvent> eventData)
        {
            if (eventData == null || eventData.MessageBody == null)
                return;

            var order = await _orderRepository.GetOrder(eventData.MessageBody.OrderId);
            if (order == null)
                return;

            if (eventData.MessageBody.IsSuccess)
            {
                order.Status = OrderStatus.Succeed;
                // Todo: 一些额外的逻辑
            }
            else
            {
                order.Status = OrderStatus.Failed;
                // Todo: 一些额外的逻辑
            }

            await _orderRepository.UpdateOrder(order);
        }
    }
}
