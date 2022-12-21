    public class BillingCycle
    {
        public Frequency frequency { get; set; }
        public string tenure_type { get; set; }
        public int sequence { get; set; }
        public int total_cycles { get; set; }
        public PricingScheme pricing_scheme { get; set; }
    }
