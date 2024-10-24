using Staychill.Models.ProductModel.TrackingModel;
using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.ProductModel
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; } // Primary Id for Product //

        [Required]
        public string? ProductName { get; set; } // Name of the Product //

        [Required]
        public string? ProductType { get; set; } // Type of Product //

        public List<string> Typeoptions { get; } = new List<string> // List of Type //
        {
            "None",
            "Mug",
            "Steel Flask",
            "Tumbler"
        };

        [Required]
        public string? Color { get; set; } // Color of Product //

        public List<string> Coloroptions { get; } = new List<string> // List of Color //
        {
            "None",
            "Yellow",
            "Pink",
            "Blue",
            "Green",
            "White",
            "Black"
        };

        [Required]
        public string? Description { get; set; } // Detail of the Product //

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public float? Price { get; set; } // Price of the Product //

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "In Stock cannot be negative.")]
        public int? Instock { get; set; } // Quantity of the product in stock //

        public ProductImages? Images { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class ProductImages
    {
        [Key]
        public int Id { get; set; } // This can be optional if you want to keep it tied directly to Product's Id

        public int ProductId { get; set; } // Foreign key to the Product

        // Properties for storing the images
        public byte[]? Image1 { get; set; }
        public byte[]? Image2 { get; set; }
        public byte[]? Image3 { get; set; }
        public byte[]? Image4 { get; set; }

        // Navigation property to associate with Product
        public Product Product { get; set; } = null!;
    }
}
