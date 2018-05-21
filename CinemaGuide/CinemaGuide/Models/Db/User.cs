using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaGuide.Api;

namespace CinemaGuide.Models.Db
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
        
        [DisplayName("Имя")]
        [Required(ErrorMessage = "введите свое имя")]
        [StringLength(50, ErrorMessage = "Имя не дложно содержать более 50 символов")]
        public string Name { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "введите свою фамилию")]
        [StringLength(50, ErrorMessage = "Фамилия не дложно содержать более 50 символов")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string Email { get; set; }
        
        [DisplayName("Логин")]
        [Required(ErrorMessage = "введите логин")]
        [StringLength(50, ErrorMessage = "логин не должен содержать более 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "введите пароль")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Пароль должен содержать не менее 5 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "повторно введите пароль")]
        [Compare("Password", ErrorMessage = "пароли не совпадают")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
