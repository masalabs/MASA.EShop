using OrderItem = MASA.EShop.Services.Ordering.Entities.OrderItem;

namespace MASA.EShop.Services.Ordering.Dto;

public class OrderItemDto
{
    public string productname { get; set; } = default!;
    public int units { get; set; }
    public decimal unitprice { get; set; }
    public string? pictureFileName { get; set; }

    public static OrderItemDto FromOrderItem(OrderItem orderItem)
    {
        return new OrderItemDto
        {
            productname = orderItem.ProductName,
            units = orderItem.Units,
            unitprice = orderItem.UnitPrice,
            pictureFileName = orderItem.PictureFileName
        };
    }
}

