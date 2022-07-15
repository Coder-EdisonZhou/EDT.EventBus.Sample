using EDT.MSA.Ordering.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Repositories
{
    public interface IOrderRepository
    {
        Task<IList<Order>> GetAllOrders();
        Task<Order> GetOrder(string orderId);
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
    }
}
