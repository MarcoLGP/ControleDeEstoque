using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeEstoque.Models.entities
{
    internal class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public double Price { get; set; }
        [Required, MaxLength(100)]
        public int Stock { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        public Category Category { get; set; }
    }
}
