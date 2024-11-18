using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Staychill.Models.BankModel
{
    public class CreditCard
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for PaymentMethod
        public int? PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        [Required]
        public string? CardType { get; set; }
        
        public List<CreditcardType>? CardTypeOpt { get; set; } = new List<CreditcardType>();

        public string? NameOnCard { get; set; } // Name of the holder of the card //

        // Backing field for CardNumber
        private string? _cardNumber;

        [Required]
        [RegularExpression(@"\d{4}-\d{4}-\d{4}-\d{4}", ErrorMessage = "Invalid card number format.")]
        public string? CardNumber
        {
            get => FormatCardNumber(_cardNumber);
            set => _cardNumber = value?.Replace("-", "").Trim(); // Store raw input
        }

        // Backing field for ExpiredDate
        private string? _expiredDate;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Invalid expiration date format. Use MM/YY.")]
        public string? ExpiredDate
        {
            get => FormatExpiredDate(_expiredDate);
            set => _expiredDate = value?.Replace("/", "").Trim(); // Store raw input
        }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string? CVV { get; set; } // CVV of the card (3 digit) //


        // === Method to format the card number with dashes === //
        private string? FormatCardNumber(string? cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return null;

            return Regex.Replace(cardNumber, ".{4}", "$0-").TrimEnd('-');
        }

        // Method to format the expiration date with "/"
        private string? FormatExpiredDate(string? expiredDate)
        {
            if (string.IsNullOrEmpty(expiredDate))
                return null;

            // Insert "/" after the first two characters
            return $"{expiredDate.Substring(0, 2)}/{expiredDate.Substring(2, 2)}";
        }
    }
}
