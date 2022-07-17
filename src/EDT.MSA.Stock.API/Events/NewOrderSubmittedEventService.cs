using DotNetCore.CAP;
using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using EDT.MSA.API.Shared.Utils;
using EDT.MSA.Stocking.API.Services;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Events
{
    public class NewOrderSubmittedEventService : INewOrderSubmittedEventService, ICapSubscribe
    {
        private readonly IStockService _stockService;
        private readonly IMsgTracker _msgTracker;

        public NewOrderSubmittedEventService(IStockService stockService, IMsgTracker msgTracker)
        {
            _stockService = stockService;
            _msgTracker = msgTracker;
        }

        [CapSubscribe(name: EventNameConstants.TOPIC_ORDER_SUBMITTED, Group = EventNameConstants.GROUP_ORDER_SUBMITTED)]
        public async Task<EventData<ProductStockDeductedEvent>> DeductProductStock(EventData<NewOrderSubmittedEvent> eventData)
        {
            // 幂等性保障
            if(await _msgTracker.HasProcessed(eventData.Id))
                return null;

            // 产品Id合法性校验
            var productStock = await _stockService.GetStock(eventData.MessageBody.ProductId);
            if (productStock == null)
                return null;

            // 核心扣减逻辑
            EventData<ProductStockDeductedEvent> result;
            if (productStock.StockQuantity - eventData.MessageBody.Quantity >= 0)
            {
                // 扣减产品实际库存
                productStock.StockQuantity -= eventData.MessageBody.Quantity;
                // 提交至数据库
                await _stockService.UpdateStock(productStock);
                result = new EventData<ProductStockDeductedEvent>(new ProductStockDeductedEvent(eventData.MessageBody.OrderId, true));
            }
            else
            {
                // Todo: 一些额外的逻辑
                result = new EventData<ProductStockDeductedEvent>(new ProductStockDeductedEvent(eventData.MessageBody.OrderId, false, "扣减库存失败"));
            }

            // 幂等性保障
            await _msgTracker.MarkAsProcessed(eventData.Id);

            return result;
        }
    }
}
