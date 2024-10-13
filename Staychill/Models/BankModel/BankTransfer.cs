using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.BankModel
{
    public class BankTransfer
    {
        [Key]
        public int Id { get; set; } // Primary key for BankInfo

        public ICollection<BankAccount>? Accounts { get; set; }

        [Required]
        [RegularExpression(@"\d{3}-\d{1}-\d{5}-\d{1}")]
        public string? BankNumber { get; set; } // Bank Account Number //
  
    }
}
