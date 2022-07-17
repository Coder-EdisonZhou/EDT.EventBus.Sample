using DotNetCore.CAP;
using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using EDT.MSA.Ordering.API.Models;
using EDT.MSA.Ordering.API.Services;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Events
{
    public class ProductStockDeductedEventService : IProductStockDeductedEventService, ICapSubscribe
    {
        private readonly IOrderService _orderService;

        public ProductStockDeductedEventService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [CapSubscribe(name: EventNameConstants.TOPIC_STOCK_DEDUCTED, Group = EventNameConstants.GROUP_STOCK_DEDUCTED)]
        public async Task MarkOrderStatus(EventData<ProductStockDeductedEvent> eventData)
        {
            if (eventData == null || eventData.MessageBody == null)
                return;

            var order = await _orderService.GetOrder(eventData.MessageBody.OrderId);
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

            await _orderService.UpdateOrder(order);
        }
    }
}
