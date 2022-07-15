namespace EDT.MSA.API.Shared.Events
{
    public class EventNameConstants
    {
        public const string TOPIC_ORDER_SUBMITTED = "mall.order.placed";
        public const string TOPIC_STOCK_DEDUCTED = "mall.stock.deducted";

        public const string GROUP_ORDER_SUBMITTED = "mall.order.placed.consumers";
        public const string GROUP_STOCK_DEDUCTED = "mall.stock.deducted.consumers";
    }
}
