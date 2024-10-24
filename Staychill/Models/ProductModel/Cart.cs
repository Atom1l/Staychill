using Staychill.Models.ProductModel.TrackingModel;
using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.ProductModel
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; } // Primary key for the Cart

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public int GenerateCartId()
        {
            var random = new Random();
            // Generate a random integer between 1 and 99999999 (8 digits)
            int cartitemId = random.Next(1, 100000000);
            return cartitemId; // Return the generated integer | not the same with CartitemsId //
        }
    }
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; } // Primary key for CartItem

        [Required]
        public int ProductId { get; set; } // Foreign key to the Product

        [Required]
        public int Quantity { get; set; } // Quantity of the product in the cart

        [Required]
        public float UnitPrice { get; set; } // Unit price of the product

        public float TotalPrice => UnitPrice * Quantity; // Calculated total price

        public Product Product { get; set; } = null!; // Navigation property to Product

        public int CartId { get; set; } // Foreign key to Cart

        public Cart Cart { get; set; } = null!; // Navigation property to Cart


    }


}
