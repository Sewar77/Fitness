using System.ComponentModel.DataAnnotations.Schema;

namespace MyFitnessLife.Models
{
    public class UserLogins
    {
        public decimal Userid { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public decimal? Roleid { get; set; }

        public string? Fname { get; set; }

        public string? Lname { get; set; }

        public string? Email { get; set; }

        public string? Phonenumber { get; set; }

        // public DateTime? Createdat { get; set; }

        public string Gender { get; set; } = null!;

        public DateTime? Birthdate { get; set; }

       // public string? Imagepath { get; set; }

        //[NotMapped]
       // public virtual IFormFile? ImageFile { get; set; }
        public string? Status { get; set; }
    }
}
