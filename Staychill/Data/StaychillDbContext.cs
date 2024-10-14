using Microsoft.EntityFrameworkCore;
using Staychill.Models;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.UserModel;
using Staychill.ViewModel;

namespace Staychill.Data
{
    public class StaychillDbContext :DbContext
    {
        // Default Constructor //
        public StaychillDbContext() { }
        // Create a Constructor //
        public StaychillDbContext(DbContextOptions<StaychillDbContext> options) : base(options) { }

        // To use ...DB as a data instead of database for CRUD func //

        // User DbSet ----- //
        public DbSet<Staychill.Models.UserModel.User> UserDB { get; set; }
        public DbSet<Staychill.Models.UserModel.Address> AddressDB { get; set; }

        // Product DbSet ----- //
        public DbSet<Staychill.Models.ProductModel.Product> ProductDB { get; set; }
        public DbSet<Staychill.Models.ProductModel.ProductImages> ProductImagesDB { get; set; }
        public DbSet<Staychill.Models.ProductModel.DiscountModel.Discount> DiscountDB { get; set; }
        public DbSet<Staychill.Models.ProductModel.TrackingModel.Tracking> TrackingDB { get; set; }
        public DbSet<Staychill.Models.ProductModel.Cart> CartDB { get; set; }

        // Payment DbSet ----- //
        public DbSet<Staychill.Models.BankModel.BankAccount> BankAccDB { get; set; }
        public DbSet<Staychill.Models.BankModel.BankTransfer> BankTransferDB { get; set; }
        public DbSet<Staychill.Models.BankModel.QRData> QRDataDB { get; set; }
        public DbSet<Staychill.Models.BankModel.CreditCard> CreditCardsDB { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder) // to mapping Model and Database //
        { // Set the relationship between each table | use for linking data together //

            modelBuilder.Entity<User>() // Define User class from Model as a Entity to create relationship with Address //
                .HasOne(u => u.Address) // Only One Address //
                .WithMany(u => u.UsersInfo) // Can contain many user //
                .HasForeignKey(u => u.AddressId) // With AddressId as a Foreign Key to Link //
                .OnDelete(DeleteBehavior.Cascade); // Linking data if one got deleted the other got deleted too //

            modelBuilder.Entity<ProductImages>() // Define ProductImages class as a Entity to create relationship with Product //
                .HasOne(p => p.Product) // One ProductImages will have One Product //
                .WithOne(p => p.Images) // Declare One to One relationship with Product and ProductImages //
                .HasForeignKey<ProductImages>(p => p.ProductId); // Set Foreign Key of this relation as ProductId //



            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Staychill.ViewModel.UserViewModel> UserViewModel { get; set; } = default!;

    }
}
