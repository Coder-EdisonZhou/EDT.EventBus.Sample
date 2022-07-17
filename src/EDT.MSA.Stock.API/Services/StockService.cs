using EDT.MSA.Stocking.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Services
{
    public class StockService : IStockService
    {
        private readonly IMongoClient _client;
        private readonly IMongoCollection<Stock> _stocks;

        public StockService(IStockDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _stocks = _client
                .GetDatabase(settings.DatabaseName)
                .GetCollection<Stock>(settings.StockCollectionName);
        }

        public async Task<IList<Stock>> GetAllStocks()
        {
            return await _stocks.Find(o => true).ToListAsync();
        }

        public async Task<Stock> GetStock(string productId)
        {
            return await _stocks.Find(o => o.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task CreateStock(Stock stock)
        {
            await _stocks.InsertOneAsync(stock);
        }

        public async Task UpdateStock(Stock stock)
        {
            await _stocks.ReplaceOneAsync(o => o.ProductId == stock.ProductId, stock);
        }
    }
}
