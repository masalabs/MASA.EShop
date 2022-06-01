namespace Masa.EShop.Services.Ordering.Dto;

public class OrderSummary
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = default!;
    public decimal Total { get; set; }
    public string ProductName { get; set; } = default!;
    public string PictureFileName { get; set; } = default!;
}

