using Staychill.Models.ProductModel.DiscountModel;
using Staychill.Models.ProductModel;

namespace Staychill.ViewModel
{
    public class CartViewModel
    {
        public List<int> CartIds { get; set; } = new List<int>(); // For selected cart item IDs
        public List<Cart>? CartItemDetails { get; set; } // List of cart items
        public float TotalAmount { get; set; } // Total amount before discount
        public string? DiscountCode { get; set; } // User input for discount code
        public float DiscountAmount { get; set; }
        public float DiscountedTotal { get; set; } // Total after discount is applied

        public float CalculatedDiscountAmount => TotalAmount * (DiscountAmount / 100f);
    }
}
