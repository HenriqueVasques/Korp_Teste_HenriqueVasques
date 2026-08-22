namespace Billing.API.DTOs.InvoiceItems;

public class InvoiceItemResponseDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }
}