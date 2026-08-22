using System.ComponentModel.DataAnnotations;

namespace Billing.API.DTOs.InvoiceItems;

public class InvoiceItemCreateDto
{
    [Required(ErrorMessage = "O código do produto é obrigatório.")]
    public required string ProductCode { get; set; }

    [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public required string Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade do item deve ser de no mínimo 1.")]
    public required int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
    public required decimal UnitPrice { get; set; }
}