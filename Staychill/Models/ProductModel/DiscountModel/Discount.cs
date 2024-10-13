using System.ComponentModel.DataAnnotations;

namespace Staychill.Models.ProductModel.DiscountModel
{
    public class Discount
    {
        [Key]
        public int Id { get; set; } // Id for declartion //
        
        [Required]
        public string DiscountCode { get; set; } // Code of the Discount //

        [Required]
        public int DiscountAmount { get; set; } // Amount of the percentage Discount give //
    }
}
