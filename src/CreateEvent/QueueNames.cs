namespace CreateEvent
{
    public static class QueueNames
    {
        public static string EmailQueue = "email-queue";
        public static string ExchangeName = "Order.Topic";
        public static string RoutingKey = "Order.Created";
        public static string ExchangeRoutingKey = "Order.#";
    }
}
