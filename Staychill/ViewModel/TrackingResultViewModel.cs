using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;

namespace Staychill.ViewModel
{
    public class TrackingResultViewModel
    {
        public string ShipmentCode { get; set; }
        public string Status { get; set; }
        public List<Product> Products { get; set; }
    }
}



