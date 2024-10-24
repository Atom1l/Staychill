using Staychill.Models.ProductModel;
using System.Collections.Generic;

namespace Staychill.ViewModel
{
    public class TrackingViewModel
    {
        public List<int> CartIds { get; set; } = new List<int>(); // For selected cart item IDs
        public List<Cart>? CartItemDetails { get; set; } // List of all cart items
        public float TotalAmount { get; set; } // Total amount for the cart
    }
}
