using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPal.Models
{
    public class WebHookEvent
    {
        public string id { get; set; }
        public DateTime create_time { get; set; }
        public string resource_type { get; set; }
        public string event_type { get; set; }
        public string summary { get; set; }

        public Resource resource { get; set; }
    }

    public class Resource
    {
        // It's attributes can be different on event type
    }
}
