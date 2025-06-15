using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SStack.Models;
using System.Diagnostics;
using SStack.ApiModels;
using System.Xml.Linq;
using System.Web.Http;
using System.Text;
using System.Net;
namespace SStack.ApiServices
{
    public class OrdersService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public OrdersService(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<List<StoreProduct>> GetProductsAsync()
        {
            try
            {
                string url = $"{_baseUrl}/api/OrderService/products";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<StoreProduct>>(json);
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode}");
                    return new List<StoreProduct>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return new List<StoreProduct>();
            }
        }

        public async Task<List<StoreOrder>> GetOrdersAsync()
        {
            try
            {
                string url = $"{_baseUrl}/api/OrderService/orders";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<StoreOrder>>(json);
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode}");
                    return new List<StoreOrder>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return new List<StoreOrder>();
            }
        }


        public async Task<bool> UpdateOrderAsyc(UpdateOrder updateOrder)
        {
            string url = $"{_baseUrl}/api/OrderService/updateOrder";

            try
            {
                string json = JsonConvert.SerializeObject(updateOrder);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"* * * * ** * Server response: {result}");
                    return true;
                }
                else
                {
                    Debug.WriteLine($" = = = = == -= Error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"- - - - - --  Exception: {ex.Message + " :" + ex.InnerException}");
                return false;
            }
        }

        public async Task<bool> AddProductToOrder(PostOrder postOrder)
        {
            string url = $"{_baseUrl}/api/OrderService/PostOrder";
            string json = JsonConvert.SerializeObject(postOrder);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($" + + + + ++  ++ API Response: {result}");
                    return true;
                }
                else
                {
                    Debug.WriteLine($" - - - - - - - - - - - - Error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" = = = = = = = = = = Exception occurred: {ex.Message}{ex.InnerException}");
                return true;
            }
        }

        public async Task<List<ProductSale>> GetProductsSales()
        {
            try
            {
                string url = $"{_baseUrl}/api/OrderService/productsSales";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ProductSale>>(json);
                }
                else
                {
                    Debug.WriteLine($" - - - - Error: {response.StatusCode}");
                    return new List<ProductSale>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"= = = = = = Exception: {ex.Message}");
                return new List<ProductSale>();
            }
        }

    }
}