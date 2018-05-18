using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGuide.Models.Db
{
    [Table("Profiles")]
    public class DbProfile
    {
        public int    Id   { get; set; }
        public int    Age  { get; set; }
        public string Role { get; set; }
        public DbUser User { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Surname can't be longer than 50 characters.")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
