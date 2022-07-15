namespace EDT.MSA.API.Shared.Events
{
    public class NewOrderSubmittedEvent
    {
        public NewOrderSubmittedEvent()
        { }

        public NewOrderSubmittedEvent(string orderId, string productId, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
