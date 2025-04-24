using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence
{
    public class DefaultContext : IdentityDbContext<User>
    {
        public DefaultContext() { }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultContext).Assembly);

            modelBuilder.Entity("RO.DevTest.Domain.Entities.Sale")
                .HasOne("RO.DevTest.Domain.Entities.Customer", null)
                .WithMany()
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity("RO.DevTest.Domain.Entities.SaleItem")
                .HasOne("RO.DevTest.Domain.Entities.Product", null)
                .WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}