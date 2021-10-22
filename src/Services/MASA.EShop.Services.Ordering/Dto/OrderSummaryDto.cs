namespace MASA.EShop.Services.Ordering.Dto;

public class OrderSummaryDto
{
    public int ordernumber { get; set; }
    public DateTime date { get; set; }
    public string status { get; set; } = default!;
    public decimal total { get; set; }

    public static OrderSummaryDto FromOrderSummary(OrderSummary orderSummary)
    {
        return new OrderSummaryDto
        {
            ordernumber = orderSummary.OrderNumber,
            date = orderSummary.OrderDate,
            status = orderSummary.OrderStatus,
            total = orderSummary.Total
        };
    }
}

