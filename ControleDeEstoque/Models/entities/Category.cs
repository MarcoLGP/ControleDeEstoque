using System.ComponentModel.DataAnnotations;

namespace ControleDeEstoque.Models.entities
{
    internal class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(100)]
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
