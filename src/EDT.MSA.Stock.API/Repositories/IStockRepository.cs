using EDT.MSA.Stocking.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Repositories
{
    public interface IStockRepository
    {
        Task<IList<Stock>> GetAllStocks();
        Task<Stock> GetStock(string productId);
        Task CreateStock(Stock stock);
        Task UpdateStock(Stock stock);
    }
}
