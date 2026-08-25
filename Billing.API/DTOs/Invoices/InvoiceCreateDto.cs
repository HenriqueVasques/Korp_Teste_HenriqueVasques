using System.ComponentModel.DataAnnotations;
using Billing.API.DTOs.InvoiceItems;

namespace Billing.API.DTOs.Invoices;

public class InvoiceCreateDto
{
    [Required(ErrorMessage = "A fatura precisa conter pelo menos um item.")]
    [MinLength(1, ErrorMessage = "A fatura precisa ter no mínimo 1 item.")]
    public required List<InvoiceItemCreateDto> Items { get; set; } = new();
}