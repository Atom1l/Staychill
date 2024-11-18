using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.BankModel
{
    public class CreditcardType
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? CreditcardOpt { get; set; }
    }
}
