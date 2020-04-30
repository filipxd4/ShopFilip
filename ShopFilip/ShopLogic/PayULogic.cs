using Newtonsoft.Json;
using ShopFilip.IdentityModels;
using ShopFilip.Interfaces;
using ShopFilip.Models;
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

        public async Task<string> GeneratePayLink(ApplicationUser user, int price, List<Item> itemfromCart, string ip, string accessToken)
        {
            string ProperPrice = (price * 100).ToString();
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://secure.payu.com/api/v2_1/orders"))
                {
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken + "");
                    var stringToPayU = "{\n\"notifyUrl\": \"https://your.eshop.com/notify\"," +
                        "\n\"continueUrl\":\"https://localhost:44380/cart/sucess\"," +
                        "\n\"customerIp\":\"" + ip + "\"," +
                        "\n\"merchantPosId\": \"145227\"," +
                        "\n\"description\": \"Filip Shop\"," +
                        "\n\"currencyCode\": \"PLN\",\n\"totalAmount\": \"" + ProperPrice + "\",\n\"buyer\": {\n       " +
                        "\"email\": \"" + user.Email + "\",\n\"phone\": \"988909909\",\n\"firstName\": \"" + user.Name + "\",\n  " +
                        "\"lastName\": \"" + user.Surname + "\",\n\"language\": \"pl\"," +
                        "\n\"delivery\": {\n" +
                        "\"street\": \"" + user.Street + "\",\n\"postalCode\": \"" + user.PostalCode + "\",\n\"city\": \"" + user.Town + "\"\n}" +
                        "\n}," +
                        "\n\"products\": [\n";
                    if (itemfromCart.Count() == 1)
                        stringToPayU += "{\n\"name\": \"" + itemfromCart.First().Product.Name + "\",\n\"unitPrice\": \"" + itemfromCart.First().Product.Price + "\",\n \"quantity\": \"" + itemfromCart.First().Quantity + "\"\n}\n";
                    else
                        for (int i = 0; i < itemfromCart.Count(); i++)
                        {
                            if (i == itemfromCart.Count() - 1)
                                stringToPayU += "{\n\"name\": \"" + itemfromCart[i].Product.Name + " " + itemfromCart[i].Size + "\",\n\"unitPrice\": \"" + itemfromCart[i].Product.Price + "\",\n \"quantity\": \"" + itemfromCart[i].Quantity + "\"\n}\n";
                            else
                                stringToPayU += "{\n\"name\": \"" + itemfromCart[i].Product.Name + " " + itemfromCart[i].Size + "\",\n\"unitPrice\": \"" + itemfromCart[i].Product.Price + "\",\n \"quantity\": \"" + itemfromCart[i].Quantity + "\"\n},\n";
                        }

                    stringToPayU += "]\n}";

                    request.Content = new StringContent(stringToPayU, Encoding.UTF8, "application/json");
                    var response = await httpClient.SendAsync(request);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return jsonString;
                }
            }
        }
    }
}
