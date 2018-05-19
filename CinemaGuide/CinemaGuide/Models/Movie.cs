using CinemaGuide.Api;

namespace CinemaGuide.Models
{
    public class Movie
    {
        public IMovie Full { get; }
        public IMovieInfo Preview { get; }

        public Movie(IMovie full)
        {
            Full = full;
        }

        public Movie(IMovieInfo preview)
        {
            Preview = preview;
        }
    }
}
