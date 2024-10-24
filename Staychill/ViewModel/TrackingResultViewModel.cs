using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;

namespace Staychill.ViewModel
{
    public class TrackingResultViewModel
    {
        public string? ShipmentCode { get; set; }
        public string? Status { get; set; }
        public List<RetainCartItemViewModel>? RetainCartItems { get; set; }
    }

    public class RetainCartItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice => Quantity * UnitPrice; // Calculated property
    }

}



