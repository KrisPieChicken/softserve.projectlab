using API.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Customer related DbSets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BusinessCustomer> BusinessCustomers { get; set; }
        public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
        public DbSet<PremiumCustomer> PremiumCustomers { get; set; }
        public DbSet<LineOfCredit> LineOfCredits { get; set; }
        public DbSet<CreditTransaction> CreditTransactions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageItem> PackageItems { get; set; }
        public DbSet<PackageNote> PackageNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer entity
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id);

            // Configure inheritance for Customer types using TPH (Table-per-Hierarchy)
            modelBuilder.Entity<Customer>()
                .HasDiscriminator<string>("CustomerType")
                .HasValue<Customer>("Base")
                .HasValue<BusinessCustomer>("Business")
                .HasValue<IndividualCustomer>("Individual")
                .HasValue<PremiumCustomer>("Premium");

            // Configure DateOnly for EF Core
            modelBuilder.Entity<Customer>()
                .Property(c => c.BirthDate)
                .HasConversion(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d)
                );

            // Configure LineOfCredit relationship
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.LineOfCredit)
                .WithOne()
                .HasForeignKey<LineOfCredit>("CustomerId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configure LineOfCredit entity
            modelBuilder.Entity<LineOfCredit>()
                .HasKey(loc => loc.Id);

            // Configure CreditTransaction entity
            modelBuilder.Entity<CreditTransaction>()
                .HasKey(ct => ct.Id);

            // Configure relationship between LineOfCredit and CreditTransactions
            modelBuilder.Entity<CreditTransaction>()
                .HasOne(ct => ct.LineOfCredit)
                .WithMany(loc => loc.Transactions)
                .HasForeignKey(ct => ct.LineOfCreditId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Configure Cart entity
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany()
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configure CartItem entity with composite key
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartId, ci.ItemId });

            // Add Foreign Key for CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne<Cart>()
                .WithMany(c => c.Items)
                .HasForeignKey("CartId");

            // Configure Package entity
            modelBuilder.Entity<Package>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Package>()
                .HasOne(p => p.Customer)
                .WithMany()
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configure PackageItem entity with composite key
            modelBuilder.Entity<PackageItem>()
                .HasKey(pi => new { pi.PackageId, pi.ItemId });

            // Add Foreign Key for PackageItem
            modelBuilder.Entity<PackageItem>()
                .HasOne<Package>()
                .WithMany(p => p.Items)
                .HasForeignKey("PackageId");

            // Configure PackageNote entity
            modelBuilder.Entity<PackageNote>()
                .HasKey(pn => pn.Id);

            // Configure relationship between Package and PackageNote
            modelBuilder.Entity<PackageNote>()
                .HasOne<Package>()
                .WithMany(p => p.Notes)
                .HasForeignKey("PackageId")
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore calculated property
            modelBuilder.Entity<Package>()
                .Ignore(p => p.ContractEndDate);
        }
        
    }
}