namespace EDT.MSA.Ordering.API.Models
{
    public class OrderDatabaseSettings : IOrderDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string OrderCollectionName { get; set; } = null!;
    }

    public interface IOrderDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string OrderCollectionName { get; set; }
    }
}
