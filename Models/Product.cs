using System.ComponentModel.DataAnnotations;

namespace CrudWithWebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}