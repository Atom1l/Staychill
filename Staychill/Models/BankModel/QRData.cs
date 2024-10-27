using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.Models.BankModel
{
    public class QRData
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public IFormFile? QRPic { get; set; }
        [NotMapped]
        public IFormFile? UserPic { get; set; }

        public byte[]? QRPicData { get; set; }
        public byte[]? UserUploadedData { get; set; }

        // Foreign key for PaymentMethod
        public int? PaymentMethodId { get; set; }

        // Navigation property to PaymentMethod
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
