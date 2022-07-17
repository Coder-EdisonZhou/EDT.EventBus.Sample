namespace EDT.MSA.Stocking.API.Models
{
    public class StockDatabaseSettings : IStockDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string StockCollectionName { get; set; } = null!;
    }

    public interface IStockDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string StockCollectionName { get; set; }
    }
}
