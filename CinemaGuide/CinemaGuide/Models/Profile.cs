using CinemaGuide.Api;

namespace CinemaGuide.Models
{
    public class Profile
    {
        public SearchConfig SearchConfig { get; set; }
        public LoginModel UserCredentials { get; set; }
        public DbProfile UserProfile { get; set; }
    }
}
