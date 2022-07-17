using EDT.MSA.Stocking.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Services
{
    public interface IStockService
    {
        Task<IList<Stock>> GetAllStocks();
        Task<Stock> GetStock(string productId);
        Task CreateStock(Stock stock);
        Task UpdateStock(Stock stock);
    }
}
