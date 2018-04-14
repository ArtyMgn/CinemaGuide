using System;

namespace CinemaGuide.Api
{
    public interface IMovieInfo
    {
        int       Id            { get; }
        string    Title         { get; }
        string    OriginalTitle { get; }
        Uri       PosterUrl     { get; }
        DateTime? ReleaseDate   { get; }
    }
}
