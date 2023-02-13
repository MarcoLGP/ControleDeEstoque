using ControleDeEstoque.Models.entities;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.Db
{
    internal class Context : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=sql10.freemysqlhosting.net;database=sql10597895;user=sql10597895;password=3FDkIBBJXt;port=3306");
        }
    }
}
