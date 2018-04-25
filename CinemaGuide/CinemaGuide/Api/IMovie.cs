namespace CinemaGuide.Api
{
    public interface IMovie : IMovieInfo
    {
        string Overview { get; }
    }
}
