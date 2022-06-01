using Order = Masa.EShop.Services.Ordering.Entities.Order;

namespace Masa.EShop.Services.Ordering.Dto;

public class OrderDto
{
    public int ordernumber { get; set; }
    public DateTime date { get; set; }
    public string status { get; set; } = default!;
    public string description { get; set; } = default!;
    public string street { get; set; } = default!;
    public string city { get; set; } = default!;
    public string zipcode { get; set; } = default!;
    public string country { get; set; } = default!;
    public List<OrderItemDto> orderitems { get; set; } = default!;
    public decimal subtotal { get; set; }
    public decimal total { get; set; }

    public static OrderDto FromOrder(Order order)
    {
        return new OrderDto
        {
            ordernumber = order.OrderNumber,
            date = order.OrderDate,
            status = order.OrderStatus,
            description = order.Description ?? "",
            street = order.Address.Street,
            city = order.Address.City,
            zipcode = order.Address.ZipCode,
            country = order.Address.Country,
            orderitems = order.OrderItems
                .Select(OrderItemDto.FromOrderItem)
                .ToList(),
            subtotal = order.GetTotal(),
            total = order.GetTotal()
        };
    }
}

