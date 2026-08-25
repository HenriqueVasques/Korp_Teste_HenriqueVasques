using Billing.API.DTOs.InvoiceItems;
using System.ComponentModel.DataAnnotations;
using static Billing.API.Enum.InvoiceStatusEnum;

public class InvoiceResponseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O número da nota fiscal é obrigatório.")]
    public int Number { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Open;
    public List<InvoiceItemResponseDto> Items { get; set; } = new();
}