namespace MASA.EShop.Services.Ordering.Actors;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }
    public string PictureFileName { get; set; } = default!;
}
