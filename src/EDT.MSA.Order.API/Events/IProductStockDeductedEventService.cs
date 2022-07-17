using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using System.Threading.Tasks;

namespace EDT.MSA.Ordering.API.Events
{
    public interface IProductStockDeductedEventService
    {
        Task MarkOrderStatus(EventData<ProductStockDeductedEvent> eventData);
    }
}
