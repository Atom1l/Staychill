using Staychill.Models.ProductModel;
using System.ComponentModel.DataAnnotations.Schema;

// Declare Enum //
public enum ShipmentStatus
{
    WaitingForPayment,
    ConfirmedOrder,
    Delivered,
    Successfully,
    Cancelled
}

namespace Staychill.ViewModel
{
    public class ShipmentViewModel
    {
        public string? ShipmentCode {  get; set; }
        public ShipmentStatus? Status {  get; set; }
        public List<Cart>? CartItems { get; set; }
        public List<int> SelectedCartItemIds { get; set; } = new List<int>();

    }
}
