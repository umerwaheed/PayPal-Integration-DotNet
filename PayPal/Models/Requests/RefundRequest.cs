using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPal.Models.Requests
{
    public class RefundRequest
    {
        public Amount amount { get; set; }
        public string invoice_number { get; set; }
    }

    public class Amount
    {
        public string total { get; set; }
        public string currency { get; set; }
    }
}
