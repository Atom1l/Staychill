using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.Models.BankModel
{
    public class StaychillQR
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public IFormFile? StoreQR { get; set; }

        public byte[]? StoreQRData { get; set; }

    }
}
