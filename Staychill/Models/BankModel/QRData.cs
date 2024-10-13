using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.Models.BankModel
{
    public class QRData
    {
        [Key]
        public int Id {  get; set; }

        [NotMapped]
        public IFormFile? QRPic { get; set; }

        public byte[]? QRPicData { get; set; }
    }
}
