using Staychill.Models.ProductModel.DiscountModel;
using Staychill.Models.ProductModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Staychill.Models.BankModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Staychill.ViewModel
{
    public class CartViewModel
    {
        public string? UserEmail { get; set; }
        public List<int> CartIds { get; set; } = new List<int>(); // For selected cart item IDs
        public List<Cart>? CartItemDetails { get; set; } // List of cart items
        public float TotalAmount { get; set; } // Total amount before discount
        public string? DiscountCode { get; set; } // User input for discount code
        public float DiscountAmount { get; set; } // Discount Amount as a percentage //
        public float CalculatedDiscountAmount => TotalAmount * (DiscountAmount / 100f); // Calculate the TotalDiscountAmount //
        public float TotalDiscountAmount { get; set; } // Total Discount after calculate with unitprice  //\      
        public float DiscountedTotal { get; set; } // Total after discount is applied //     

        // Paymentmethod //
        public PaymentViewModel? PaymentViewModel { get; set; }
        public List<StaychillQR>? StaychillQR { get; set; }
    }
}
