namespace Stock.API.DTOs.Product
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Balance { get; set; }
    }
}
