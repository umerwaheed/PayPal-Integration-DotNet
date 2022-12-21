    public class CreateSubscriptionResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public DateTime status_update_time { get; set; }
        public string plan_id { get; set; }
        public bool plan_overridden { get; set; }
        public DateTime start_time { get; set; }
        public string quantity { get; set; }
        public ShippingAmount shipping_amount { get; set; }
        public Subscriber subscriber { get; set; }
        public DateTime create_time { get; set; }
        public List<Link> links { get; set; }
    }
