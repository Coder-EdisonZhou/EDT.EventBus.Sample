using EDT.MSA.Stocking.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IMongoCollection<Stock> _stocks;

        public StockRepository(IStockDatabaseSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _stocks = mongoDatabase.GetCollection<Stock>(settings.StockCollectionName);
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
