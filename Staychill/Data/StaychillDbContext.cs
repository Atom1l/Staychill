using Microsoft.EntityFrameworkCore;
using Staychill.Models;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;
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
        public DbSet<Staychill.Models.ProductModel.CartItem> CartitemsDB { get; set; }
        public DbSet<Staychill.Models.ProductModel.RetainCarts> RetaincartsDB { get; set; }
        public DbSet<Staychill.Models.ProductModel.RetainCartItem> RetainCartItems { get; set; }

        // Payment DbSet ----- //
        public DbSet<Staychill.Models.BankModel.BankAccount> BankAccDB { get; set; }
        public DbSet<Staychill.Models.BankModel.BankTransfer> BankTransferDB { get; set; }
        public DbSet<Staychill.Models.BankModel.QRData> QRDataDB { get; set; }
        public DbSet<Staychill.Models.BankModel.CreditCard> CreditCardsDB { get; set; }
        public DbSet<Staychill.Models.BankModel.PaymentMethod> PaymentDB { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User to Address relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address) // A User has one Address
                .WithMany(a => a.UsersInfo) // An Address can have many Users
                .HasForeignKey(u => u.AddressId) // Foreign key in User
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // ProductImages to Product relationship
            modelBuilder.Entity<ProductImages>()
                .HasOne(p => p.Product) // A ProductImages has one Product
                .WithOne(p => p.Images) // A Product has one ProductImages (1-to-1 relationship)
                .HasForeignKey<ProductImages>(p => p.ProductId); // Foreign key in ProductImages

            // Cart to CartItem relationship
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems) // A Cart can have many CartItems
                .WithOne(ci => ci.Cart) // Each CartItem belongs to one Cart
                .HasForeignKey(ci => ci.CartId) // Foreign key in CartItem
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // CartItem to Product relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product) // Each CartItem has one Product
                .WithMany(p => p.CartItems) // A Product can be in many CartItems
                .HasForeignKey(ci => ci.ProductId) // Use ProductId as the foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Tracking to RetainCart relationship
            modelBuilder.Entity<Tracking>()
                .HasMany(t => t.RetainCarts)
                .WithOne(rc => rc.Tracking) // Assuming RetainCart has a property named Tracking
                .HasForeignKey(rc => rc.TrackingId) // Foreign key in RetainCart
                .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior

            // RetainCart to RetainCartItem relationship
            modelBuilder.Entity<RetainCarts>()
                .HasMany(rc => rc.RetainCartItems)
                .WithOne(rci => rci.RetainCart) // Assuming RetainCartItem has a property named RetainCart
                .HasForeignKey(rci => rci.RetainCartId) // Foreign key in RetainCartItem
                .OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior

            // One-to-one relationship between PaymentMethod and CreditCard
            modelBuilder.Entity<PaymentMethod>()
                .HasOne(p => p.CreditCard)
                .WithOne(c => c.PaymentMethod)
                .HasForeignKey<CreditCard>(c => c.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade); // Configure deletion behavior

            // One-to-one relationship between PaymentMethod and QRData
            modelBuilder.Entity<PaymentMethod>()
                .HasOne(p => p.QRData)
                .WithOne(q => q.PaymentMethod)
                .HasForeignKey<QRData>(q => q.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one relationship between PaymentMethod and BankTransfer
            modelBuilder.Entity<PaymentMethod>()
                .HasOne(p => p.BankTransfer)
                .WithOne(b => b.PaymentMethod)
                .HasForeignKey<BankTransfer>(b => b.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShipmentViewModel as a keyless entity
            modelBuilder.Entity<ShipmentViewModel>(entity =>
            {
                entity.HasNoKey(); // Mark as keyless
            });

            base.OnModelCreating(modelBuilder); // Call the base method
        }

        public DbSet<Staychill.ViewModel.UserViewModel> UserViewModel { get; set; } = default!;
        public DbSet<Staychill.ViewModel.ShipmentViewModel> ShipmentViewModel { get; set; } = default!;

    }
}
