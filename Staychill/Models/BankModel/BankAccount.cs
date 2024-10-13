using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.Models.BankModel
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? BankName { get; set; }

        [NotMapped]
        public IFormFile? BankPics { get; set; }

        public byte[]? BankPicsData { get; set; }


    }
}
