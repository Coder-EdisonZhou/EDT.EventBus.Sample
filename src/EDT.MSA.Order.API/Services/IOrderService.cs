using EDT.MSA.Ordering.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Services
{
    public interface IOrderService
    {
        Task<IList<Order>> GetAllOrders();
        Task<Order> GetOrder(string orderId);
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
    }
}
