namespace Billing.API.DTOs.DeductStockItem;
public class DeductStockItemDto
{
    public required string ProductCode { get; set; }
    public int Quantity { get; set; }
}