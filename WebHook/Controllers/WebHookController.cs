using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayPal;
using PayPal.Models;

namespace WebHook.Controllers
{
    public class WebHookController : Controller
    {
        [HttpPost("api/checkout/webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var headers = HttpContext.Request.Headers;

            var isValidEvent = await VerifyEvent(json, headers);

            if (isValidEvent)
            {
                var webhookEvent = JsonConvert.DeserializeObject<WebHookEvent>(json);

                switch (webhookEvent.event_type)
                {
                    case "BILLING.SUBSCRIPTION.ACTIVATED":
                        // Activate the subscription -- Use webhookEvent.resource to get event data
                        break;
                    case "BILLING.SUBSCRIPTION.CANCELLED":
                        // Cancel the subscription -- Use webhookEvent.resource to get event data
                        break;
                    case "PAYMENT.SALE.COMPLETED":
                        // Add Recurring Payment -- Use webhookEvent.resource to get event data
                        break;
                }
                //https://api.sandbox.paypal.com/v1/payments/sale/94B86548SC282320N/refund
                //I-TBHAH1F8HLEM
                return Ok("Success");
            }

            return BadRequest();

        }

        private async Task<bool> VerifyEvent(string json, IHeaderDictionary headerDictionary)
        {
            var paypalHelper = new PayPalClientApi();
            var response = await paypalHelper.GetAuthorizationRequest();
            paypalHelper.SetToken(response.access_token);

            var isValidEvent = await paypalHelper.VerifyEvent(json, headerDictionary);

            return isValidEvent;

        }
    }
}
