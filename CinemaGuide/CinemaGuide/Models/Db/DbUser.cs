using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGuide.Models.Db
{
    [Table("Users")]
    public class DbUser
    {
        public int Id { get; set; }
        public int ProfileForeignKey { get; set; }
        public DbProfile Profile { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
