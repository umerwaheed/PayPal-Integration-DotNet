    public class CreateSubscriptionRequest
    {
        public string plan_id { get; set; }
        public string quantity { get; set; }
        public ShippingAmount shipping_amount { get; set; }
        public Subscriber subscriber { get; set; }
        public ApplicationContext application_context { get; set; }
    }
