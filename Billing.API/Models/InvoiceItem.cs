using System.ComponentModel.DataAnnotations;

namespace Billing.API.Models;

public class InvoiceItem
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    [Required(ErrorMessage = "O código do produto é obrigatório.")]
    public required string ProductCode { get; set; }

    [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public required string Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade do item deve ser de no mínimo 1.")]
    public required int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
    public decimal UnitPrice { get; set; } = 0;

    public bool IsDeleted { get; set; } = false;

    public decimal Total => Quantity * UnitPrice;
}