namespace EDT.MSA.API.Shared.Events
{
    public class ProductStockDeductedEvent
    {
        public ProductStockDeductedEvent()
        { }

        public ProductStockDeductedEvent(string orderId, bool isSuccess, string message = null)
        {
            OrderId = orderId;
            IsSuccess = isSuccess;
            Message = message;
        }

        public string OrderId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
