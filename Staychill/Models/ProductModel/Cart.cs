using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.ProductModel
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; } // Primary key for the Cart

        [Required]
        public int ProductId { get; set; } // Foreign key to the Product

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Quantity of the product added to the cart

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public float UnitPrice { get; set; } // Unit price of the product

        public float TotalPrice => UnitPrice * Quantity; // Calculated total price

        // Navigation property to link the product
        public Product Product { get; set; } = null!;
    }
}
