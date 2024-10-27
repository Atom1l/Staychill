using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.ViewModel
{
    public class PaymentViewModel
    {
        public List<Cart>? CartItemDetails { get; set; }
        public int? SelectedPaymentMethodId { get; set; } // Primary Key //
        public string? SelectedPaymentMethod { get; set; } // Selected Paymethod as String //
        public string? SelectedBankAccount { get; set; } // Selected Bank account //
        public string? BankNumber { get; set; } // Bank Number //
        public string? SelectCreditCard { get; set; }
        public string? CreditCardName { get; set; } 
        public string? CreditCardNumber { get; set; }
        public string? CreditCardExpiryDate { get; set; }
        public string? CreditCardCVV { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal DiscountedTotal { get; set; }

        [NotMapped]
        public IFormFile? UserQrPic { get; set; }
        public byte[]? UserQrPicByte { get; set; }

        public BankTransfer? BankTransfer { get; set; }
        public CreditCard? CreditCard { get; set; }
        public QRData? QRData { get; set; }
    }
}
