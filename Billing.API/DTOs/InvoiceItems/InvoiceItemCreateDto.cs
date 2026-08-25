using System.ComponentModel.DataAnnotations;

namespace Billing.API.DTOs.InvoiceItems;

public class InvoiceItemCreateDto
{
    [Required(ErrorMessage = "O código do produto é obrigatório.")]
    public required string ProductCode { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de no mínimo 1.")]
    public required int Quantity { get; set; }

    public string Description { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; } = 0;
}