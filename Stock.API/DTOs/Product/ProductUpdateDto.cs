using System.ComponentModel.DataAnnotations;

namespace Stock.API.DTOs.Product
{
    public class ProductUpdateDto
    {
        [StringLength(50, ErrorMessage = "O código do produto deve ter no máximo 50 caracteres.")]
        public required string ProductCode { get; set; }
        [StringLength(200, ErrorMessage = "A descrição do produto deve ter no máximo 200 caracteres.")]
        public required string Description { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "O saldo do produto não pode ser negativo.")]
        public required int Balance { get; set; }
    }
}
