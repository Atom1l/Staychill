using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Staychill.Models.UserModel
{
    // User Model //
    public class User
    {
        [Key]
        public int Id { get; set; } // User ID as a Primary Key //

        [Required]
        public string? Firstname { get; set; } // User Firstname //

        [Required]
        public string? Lastname { get; set; } // User Lastname //

        [Required]
        public string? Username { get; set; } // Username for Login //

        [Required]
        public string? Email { get; set; } // Email of user (to send pdf receipt) //

        [Required]
        public string? Password { get; set; } // Password for Login //

        [Required]
        public string? Phonenumber { get; set; } // Phonenumber //

        [ForeignKey("AddressId")]
        [Required]
        public int AddressId { get; set; } // Foreign Key linking to Address Table //

        public virtual Address? Address { get; set; }  // Navigation property to Address class //

    }

    // Address Model //
    public class Address
    {

        [Key]
        public int AddressId { get; set; } // Primary key of Address class //

        [Required]
        public string? Housenumber { get; set; } // House Number //

        [Required]
        public string? Alley { get; set; } // Alley //

        [Required]
        public string? Road { get; set; } // Road //

        [Required]
        public string? Subdistrict { get; set; } // Subdistrict //

        [Required]
        public string? District { get; set; } // District //

        [Required]
        public string? Province { get; set; } // Province //

        [Required]
        public string? Country { get; set; } // Country //

        [Required]
        public string? Zipcode { get; set; } // Zipcode/Postal Code //

        public virtual ICollection<User>? UsersInfo { get; set; } // Navigation property to UserModel class

    }
}

