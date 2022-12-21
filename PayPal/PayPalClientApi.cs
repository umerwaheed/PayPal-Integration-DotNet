using System.Net;
using System.Net.Http.Headers;
using System.Text;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PayPal.Configuration;
using PayPal.Models.Requests;
using PayPal.Models.Responses;

namespace PayPal
{
    public class PayPalClientApi
    {
        private HttpClient _client;
        public PayPalClientApi()
        {
            CreateHttpClient();
        }

        private void CreateHttpClient()
        {
            _client = new HttpClient();
        }

        public async Task<AuthorizationResponseData> GetAuthorizationRequest()
        {
            EnsureHttpClientCreated();

            var byteArray = Encoding.ASCII.GetBytes($"{ConfigHelper.ClientId}:{ConfigHelper.ClientSecret}");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var keyValueParis = new List<KeyValuePair<string, string>>
                { new KeyValuePair<string, string>("grant_type", "client_credentials") };

            var response = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/oauth2/token", new FormUrlEncodedContent(keyValueParis));

            var responseAsString = await response.Content.ReadAsStringAsync();

            var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);

            return authorizationResponse;
        }

        public async Task<CreatePlanResponse> CreatePlan(CreatePlanRequest request)
        {
            EnsureHttpClientCreated();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/billing/plans", httpRequestMessage.Content);

            var responseAsString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CreatePlanResponse>(responseAsString);

            return result;
        }

        public async Task<CreatePlanResponse> GetPlanDetails(string planId)
        {
            EnsureHttpClientCreated();

            var response = await _client.GetAsync($"{ConfigHelper.BaseUrl}/v1/billing/plans/{planId}");

            var responseAsString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CreatePlanResponse>(responseAsString);

            return result;
        }

        public async Task<CreateSubscriptionResponse> CreateSubscription(CreateSubscriptionRequest request)
        {
            EnsureHttpClientCreated();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/billing/subscriptions", httpRequestMessage.Content);

            var responseAsString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CreateSubscriptionResponse>(responseAsString);

            return result;
        }

        public async Task<bool> ActiveSubscription(string id, SubscriptionStatusChangeRequest request)
        {
            EnsureHttpClientCreated();


            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/billing/subscriptions/{id}/activate", httpRequestMessage.Content);

            return response.StatusCode == HttpStatusCode.NoContent;
        }
        public async Task<bool> SuspendSubscription(string id, SubscriptionStatusChangeRequest request)
        {
            EnsureHttpClientCreated();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/billing/subscriptions/{id}/suspend", httpRequestMessage.Content);

            return response.StatusCode == HttpStatusCode.NoContent;
        }
        public async Task<bool> CancelSubscription(string id, SubscriptionStatusChangeRequest request)
        {
            EnsureHttpClientCreated();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/billing/subscriptions/{id}/cancel", httpRequestMessage.Content);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> RefundSubscriptionAmount(string refundUrl, RefundRequest request)
        {
            EnsureHttpClientCreated();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsync(refundUrl, httpRequestMessage.Content);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> VerifyEvent(string json, IHeaderDictionary headerDictionary)
        {
            // !!IMPORTANT!!
            // Without this direct JSON serialization, PayPal WILL ALWAYS return verification_status = "FAILURE".
            // This is probably because the order of the fields are different and PayPal does not sort them. 
            var paypalVerifyRequestJsonString = $@"{{
				""transmission_id"": ""{headerDictionary["PAYPAL-TRANSMISSION-ID"][0]}"",
				""transmission_time"": ""{headerDictionary["PAYPAL-TRANSMISSION-TIME"][0]}"",
				""cert_url"": ""{headerDictionary["PAYPAL-CERT-URL"][0]}"",
				""auth_algo"": ""{headerDictionary["PAYPAL-AUTH-ALGO"][0]}"",
				""transmission_sig"": ""{headerDictionary["PAYPAL-TRANSMISSION-SIG"][0]}"",
				""webhook_id"": ""6WC685942N447610S"",
				""webhook_event"": {json}
				}}";

            var content = new StringContent(paypalVerifyRequestJsonString, Encoding.UTF8, "application/json");

            var resultResponse = await _client.PostAsync($"{ConfigHelper.BaseUrl}/v1/notifications/verify-webhook-signature", content);

            var responseBody = await resultResponse.Content.ReadAsStringAsync();

            var verifyWebhookResponse = JsonConvert.DeserializeObject<WebHookVerificationResponse>(responseBody);

            if (verifyWebhookResponse.verification_status != "SUCCESS")
            {
                return false;
            }

            return true;
        }

        public void SetToken(string token)
        {
            _client.SetBearerToken(token);
        }
        private void EnsureHttpClientCreated()
        {
            if (_client == null)
            {
                CreateHttpClient();
            }
        }

    }
}