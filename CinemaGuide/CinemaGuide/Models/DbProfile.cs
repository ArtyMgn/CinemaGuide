using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGuide.Models
{
    [Table("Profiles")]
    public class DbProfile
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Required]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "First surname be longer than 50 characters.")]
        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int    Age  { get; set; }
        public string Role { get; set; }

        public DbUser User { get; set; }
    }
}
