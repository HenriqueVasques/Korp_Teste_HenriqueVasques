using System.ComponentModel.DataAnnotations;
using Billing.API.Models;
using static Billing.API.Enum.InvoiceStatusEnum;

namespace Billing.API.DTOs.Invoices
{
    public class InvoiceResponseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O número da nota fiscal é obrigatório.")]
        [StringLength(50, ErrorMessage = "O número da nota deve ter no máximo 50 caracteres.")]
        public required int Number { get; set; } = 0;

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        [Range(0.00, double.MaxValue, ErrorMessage = "O valor total da nota não pode ser negativo.")]
        public decimal TotalAmount { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Open;
        public List<InvoiceItem> Items { get; set; } = new();
    }
}
