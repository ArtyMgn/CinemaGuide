using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGuide.Models
{
    [Table("Users")]
    public class DbUser
    {
        public string Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        public int       ProfileForeignKey { get; set; }
        public DbProfile Profile           { get; set; }
    }
}
