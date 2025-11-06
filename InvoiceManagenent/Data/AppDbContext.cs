using InvoiceManagenent.Models;
using Microsoft.EntityFrameworkCore;
using InvoiceManagenent.DTOs;

namespace InvoiceManagenent.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }


        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=Invoi_DB; Trusted_Connection=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData
                (
                new Category { CategoryId = 1, CatagoryName = "Electronics" },
                new Category { CategoryId = 2, CatagoryName = "Tools" },
                new Category { CategoryId = 3, CatagoryName = "Paper" }

                );

            modelBuilder.Entity<Product>().HasData
                (
                new Product { ProductId=1,CategoryId = 1, ProductName = "TCL TV 32`", Price = 16000.00m, Stock =3}

                );


        }
        public DbSet<InvoiceManagenent.DTOs.ProductDto> ProductDto { get; set; } = default!;


    }
}
