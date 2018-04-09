namespace CinemaGuide.Models
{
    public class ProfileInfo
    {
        public string Language { get; }

        public ProfileInfo(string language = "ru")
        {
            Language = language;
        }
    }
}