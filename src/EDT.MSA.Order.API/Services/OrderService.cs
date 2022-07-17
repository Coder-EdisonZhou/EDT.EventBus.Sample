using DotNetCore.CAP;
using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using EDT.MSA.Ordering.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICapPublisher _eventPublisher;
        private readonly IMongoCollection<Order> _orders;
        private readonly IMongoClient _client;

        public OrderService(IOrderDatabaseSettings settings, ICapPublisher eventPublisher)
        {
            _client = new MongoClient(settings.ConnectionString);
            _orders = _client
                .GetDatabase(settings.DatabaseName)
                .GetCollection<Order>(settings.OrderCollectionName);
            _eventPublisher = eventPublisher;
        }

        public async Task<IList<Order>> GetAllOrders()
        {
            return await _orders.Find(o => true).ToListAsync();
        }

        public async Task<Order> GetOrder(string orderId)
        {
            return await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();
        }

        public async Task CreateOrder(Order order)
        {
            // 本地事务集成示例
            using (var session = _client.StartTransaction(_eventPublisher))
            {
                // 01.订单数据存入MongoDB
                _orders.InsertOne(order);
                // 02.发布订单已生成事件消息
                _eventPublisher.Publish(
                    name: EventNameConstants.TOPIC_ORDER_SUBMITTED,
                    contentObj: new EventData<NewOrderSubmittedEvent>(new NewOrderSubmittedEvent(order.OrderId, order.ProductId, order.Quantity)),
                    callbackName: EventNameConstants.TOPIC_STOCK_DEDUCTED
                    );
                // 03.提交事务
                await session.CommitTransactionAsync();
            }
        }

        public async Task UpdateOrder(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.OrderId == order.OrderId, order);
        }
    }
}
