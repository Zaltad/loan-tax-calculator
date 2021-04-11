using LoanTaxCalculator.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoanTaxCalculator.Repositories
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //dotnet ef migrations add InitialCreate
            //dotnet ef database update
            //DataSeed_
        }

        public DbSet<TaxType> TaxTypes { get; set; }

        public DbSet<PeriodicTax> PeriodicTaxes { get; set; }

        public DbSet<TaxMeasurement> TaxMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PeriodicTax>()
                .HasOne(periodicTax => periodicTax.Measurement)
                .WithOne(taxMeasurement => taxMeasurement.PeriodicTax)
                .HasForeignKey<TaxMeasurement>(taxMeasurement => taxMeasurement.PeriodicTaxId);

            modelBuilder.Entity<User>()
                .HasMany(user => user.PeriodicTaxes)
                .WithOne(periodicTax => periodicTax.User)
                .IsRequired();
        }
    }
}
