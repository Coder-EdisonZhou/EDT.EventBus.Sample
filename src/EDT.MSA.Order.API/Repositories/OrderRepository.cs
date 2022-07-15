using EDT.MSA.Ordering.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IOrderDatabaseSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _orders = mongoDatabase.GetCollection<Order>(settings.OrderCollectionName);
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
            await _orders.InsertOneAsync(order);
        }

        public async Task UpdateOrder(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.OrderId == order.OrderId, order);
        }
    }
}
