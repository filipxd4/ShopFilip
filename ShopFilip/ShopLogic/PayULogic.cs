using Newtonsoft.Json;
using ShopFilip.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ShopFilip.Models.PayUModel;

namespace ShopFilip.ShopLogic
{
    public class PayULogic : IPayULogic
    {
        public async Task<string> GetStatusOfOrderAsync(string orderId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://private-anon-8f04126df6-payu21.apiary-proxy.com/api/v2_1/orders/" + orderId))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer 3e5cac39-7e38-4139-8fd6-30adc06a61bd");
                    var response = await httpClient.SendAsync(request);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var objResponse1 = JsonConvert.DeserializeObject<AccessModel>(jsonString);
                    return objResponse1.status.statusCode;
                }
            }
        }
        public async Task<string> GetAccessTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://secure.payu.com/pl/standard/user/oauth/authorize"))
                {
                    request.Headers.TryAddWithoutValidation("Host", "secure.payu.com");
                    request.Content = new StringContent("grant_type=client_credentials&client_id=145227&client_secret=12f071174cb7eb79d4aac5bc2f07563f", Encoding.UTF8, "application/x-www-form-urlencoded");
                    try
                    {
                        var response = await httpClient.SendAsync(request);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var parsedJson = JsonConvert.DeserializeObject<AccessModel>(jsonString);
                        return parsedJson.access_token;
                    }
                    catch
                    {
                        throw new InvalidOperationException("Nie można wykonac requestu.");
                    }
                }
            }
        }
    }
}
