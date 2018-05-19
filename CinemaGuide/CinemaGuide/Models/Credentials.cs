using System.ComponentModel.DataAnnotations;

namespace CinemaGuide.Models
{
    public class Credentials
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Пароль должен содержать не менее 5 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}