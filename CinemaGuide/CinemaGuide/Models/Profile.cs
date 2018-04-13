using CinemaGuide.Api;

namespace CinemaGuide.Models
{
    public class Profile
    {
        public bool IsAuth               { get; set; }
        public SearchConfig SearchConfig { get; set; }
    }
}
