using System.ComponentModel.DataAnnotations;

namespace Billing.API.DTOs.InvoiceItems
{
    public class InvoiceItemUpdateDto
    {
        public required string ProductCode { get; set; }
        public required string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade do item deve ser de no mínimo 1.")]
        public required int Quantity { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public required decimal UnitPrice { get; set; }
    }
}
