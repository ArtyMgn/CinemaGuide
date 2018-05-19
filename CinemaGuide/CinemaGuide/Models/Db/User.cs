using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGuide.Models.Db
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "Имя не дложно содержать более 50 символов")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Фамилия не дложно содержать более 50 символов")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "логин не должен содержать более 50 символов")]
        public string Login { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Пароль должен содержать не менее 5 символов")]
        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
