using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Product>()
				.HasMany(p => p.Categories)
				.WithMany(c => c.Products)
				.UsingEntity(cp => cp.ToTable("CategoryProduct"));

			//modelBuilder.Entity<CategoryProduct>()
			//	.HasKey(pc => new { pc.ProductId, pc.CategoryId });
			
		}
	}
}
