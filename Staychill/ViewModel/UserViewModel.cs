using System.ComponentModel.DataAnnotations;

namespace Staychill.ViewModel
{ // Use ViewModel as a combiner from user and address bc We have seperate class in UserModel | ViewModel will help us show the specific data easier //
    public class UserViewModel
    {
        // Refer User class Info //
        public int Id { get; set; }
        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Lastname { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Password { get; set; }


        // Refer Address class Info //
        [Required]
        public string? HouseNumber { get; set; }

        [Required]
        public string? Alley { get; set; }

        [Required]
        public string? Road { get; set; }

        [Required]
        public string? Subdistrict { get; set; }

        [Required]
        public string? District { get; set; }

        [Required]
        public string? Province { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? ZipCode { get; set; }

    }
}
