using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.BankModel
{
    public class CreditCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? CardType { get; set; }
        public List<string> CardTypeOpt { get; } = new List<string> // List of CardType //
        {
            "VISA",
            "MasterCard",
            "TrueMoney",
        };

        public string? NameOnCard { get; set; } // Name of the holder of the card //

        [Required]
        [RegularExpression(@"\d{4}-\d{4}-\d{4}-\d{4}")]
        public string? CardNumber { get; set; } // Card Number include "-" //

        [Required]
        [RegularExpression(@"\d{2}/\d{2}")]
        public string? ExpiredDate { get; set; } // Expired Date include "/" //

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string? CVV { get; set; } // CVV of the card (3 digit) //
    }
}
