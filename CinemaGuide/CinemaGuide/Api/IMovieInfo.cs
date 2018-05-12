using System;

namespace CinemaGuide.Api
{
    public interface IMovieInfo
    {
        string    Id            { get; }
        string    Title         { get; }
        string    OriginalTitle { get; }
        Uri       PosterUrl     { get; }
        DateTime? ReleaseDate   { get; }
        int?       Year         { get; }
    }
}
