namespace MASA.EShop.Services.Ordering.Dto;

public class OrderSummaryDto
{
    public int OrderNumber { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = default!;
    public decimal Total { get; set; }
    public string ProductName { get; set; } = default!;
    public string PictureName { get; set; } = default!;

    public static OrderSummaryDto FromOrderSummary(OrderSummary orderSummary)
    {
        return new OrderSummaryDto
        {
            OrderNumber = orderSummary.OrderNumber,
            Date = orderSummary.OrderDate,
            Status = orderSummary.OrderStatus,
            Total = orderSummary.Total,
            ProductName = orderSummary.ProductName,
            PictureName = orderSummary.PictureFileName
        };
    }
}

