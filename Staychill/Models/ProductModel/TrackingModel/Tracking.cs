using Staychill.Models.BankModel;
using Staychill.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staychill.Models.ProductModel.TrackingModel
{
    public class Tracking
    {
        [Key]
        public int Id { get; set; } // Id for Declartion //

        public int? UserId { get; set; } // Foreign Key //
        public User? User { get; set; }

        [Required]
        public string? ShipmentCode { get; set; } // ShipmentCode getting After Done Payment //

        [Required]
        public string? Status { get; set; } // Status of Shipping //

        public List<string> Statusoptions { get; } = new List<string> // List of Status //
        {
            "Pending",
            "Confirm order",
            "Delivering",
            "Sucessfully",
            "Cancelled"
        };

        public byte[]? Invoice { get; set; }

        public virtual ICollection<RetainCarts> RetainCarts { get; set; } = new List<RetainCarts>(); // Define RetainCarts to Include //

        public PaymentMethod? PaymentMethod { get; set; } // Define PaymentMethod to Include //


        // Create a function for generating a random code for ShipmentCode value //
        public static string GenerateShipmentCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // will random in A-Z a-z 0-9 //
            var random = new Random(); // Set variable random to contain new Random() //
            var Shipment = new char[8]; // Set the limit of the random letter //
            for (int i = 0; i < Shipment.Length; i++) // Loop untill meet the maximum length that we mentioned //
            {
                Shipment[i] = chars[random.Next(chars.Length)]; // Shipment[i] = letter by letter = Type of [chars] random till meet the requirements //
            }
            return new string(Shipment); // return as a new string by the value of Shipment //
        }

    }
}
