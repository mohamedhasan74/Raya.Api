using System.ComponentModel.DataAnnotations;

namespace Raya.Api.Dtos
{
    public class ProductAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
