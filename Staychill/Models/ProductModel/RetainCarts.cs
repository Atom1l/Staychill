using Staychill.Models.ProductModel.TrackingModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.Models.ProductModel
{
    public class RetainCarts
    {
        [Key]
        public int ReCartId { get; set; } // Primary key for the RetainCart

        public string? Username { get; set; }

        // Navigation property for related retain cart items
        public ICollection<RetainCartItem> RetainCartItems { get; set; } = new List<RetainCartItem>();

        public int? TrackingId { get; set; } // Nullable foreign key to Tracking

        public Tracking Tracking { get; set; } = null!; // Navigation property to Tracking

        public int GenerateCartId()
        {
            var random = new Random();
            // Generate a random integer between 1 and 99999999 (8 digits)
            int cartitemId = random.Next(1, 100000000);
            return cartitemId; // Return the generated integer
        }
    }

    public class RetainCartItem
    {
        [Key]
        public int ReCartItemId { get; set; } // Primary key for RetainCartItem

        [Required]
        public byte[]? ProductIMG { get; set; } // Product IMG

        [Required]
        public int ProductId { get; set; } // Foreign key to the Product

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? Color { get; set; }

        [Required]
        public int Quantity { get; set; } // Quantity of the product in the cart

        [Required]
        public float UnitPrice { get; set; } // Unit price of the product

        public float TotalPrice { get; set; } // Calculated total price
        public float TotalAmount { get; set; } // Total amount before calculate with discount

        public float DiscountAmount { get; set; }

        public float TotalDiscountedPrice { get; set; }

        public Product Product { get; set; } = null!; // Navigation property to Product

        public int RetainCartId { get; set; } // Nullable foreign key to RetainCart

        [ForeignKey("ReCartId")]
        public virtual RetainCarts? RetainCart { get; set; } // Navigation property to RetainCart
    }
}
