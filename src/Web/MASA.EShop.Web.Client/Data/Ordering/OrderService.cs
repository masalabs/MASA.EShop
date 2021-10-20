using MASA.EShop.Web.Client.Data.Ordering.Record;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MASA.EShop.Web.Client.Data.Ordering
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderService> _logger;

        private readonly string getOrdersUrl = "";
        private readonly string cancelOrderUrl = "";
        private readonly string shipOrderUrl = "";

        private string party = "/api/v1/orders/";

        public OrderService(HttpClient httpClient, IOptions<Settings> settings, ILogger<OrderService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.OrderingUrl);
            _logger = logger;

            getOrdersUrl = $"{party}list";
            cancelOrderUrl = $"{party}cancel";
            shipOrderUrl = $"{party}ship";
        }

        public async Task<List<OrderSummary>> GetMyOrders(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<OrderSummary>>($"{getOrdersUrl}?userId={userId}") ?? new List<OrderSummary>();
        }

        public async Task<Order> GetOrder(string userId, int orderNumber)
        {
            return await _httpClient.GetFromJsonAsync<Order>($"{party}{orderNumber}");
        }

        public async Task ShipOrder(int orderNumber)
        {
            var order = new
            {
                OrderNumber = orderNumber
            };

            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("orderNumber",orderNumber.ToString())
            });

            var response = await _httpClient.PutAsync(shipOrderUrl, stringContent);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task CancelOrder(int orderNumber)
        {
            var response = await _httpClient.PutAsync($"{cancelOrderUrl}/{orderNumber}", null);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
