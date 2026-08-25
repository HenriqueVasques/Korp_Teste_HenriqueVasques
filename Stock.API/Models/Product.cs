using System.ComponentModel.DataAnnotations;

namespace Stock.API.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O código do produto é obrigatório.")]
    [StringLength(50, ErrorMessage = "O código do produto deve ter no máximo 50 caracteres.")]
    public required string ProductCode { get; set; }

    [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição do produto deve ter no máximo 200 caracteres.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "O saldo do produto é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O saldo do produto não pode ser negativo.")]
    public required int Balance { get; set; }
    public bool IsDeleted { get; set; } = false;
}