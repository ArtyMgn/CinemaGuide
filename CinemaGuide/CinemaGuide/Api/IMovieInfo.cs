using System;

namespace CinemaGuide.Api
{
    public interface IMovieInfo
    {
        string    Id            { get; }
        string    Title         { get; }
        string    OriginalTitle { get; }
        int?      Year          { get; }
        Uri       PosterUrl     { get; }
        DateTime? ReleaseDate   { get; }
    }
}
