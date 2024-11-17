using Staychill.Models.BankModel;
using Staychill.Models.ProductModel.TrackingModel;

namespace Staychill.ViewModel
{
    public class AdminPaymentViewModel
    {
        public List<PaymentMethod>? PaymentMethods { get; set; }
        public List<Tracking>? TrackingData { get; set; }
    }
}
