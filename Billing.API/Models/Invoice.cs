using Billing.API.Models;
using System.ComponentModel.DataAnnotations;
using static Billing.API.Enum.InvoiceStatusEnum;

public class Invoice
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O número da nota fiscal é obrigatório.")]
    public required int Number { get; set; } 

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    [Range(0.00, double.MaxValue, ErrorMessage = "O valor total da nota não pode ser negativo.")]
    public decimal TotalAmount { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Open;
    public List<InvoiceItem> Items { get; set; } = new();
}