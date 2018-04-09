using System;

namespace CinemaGuide.Models
{
    public class Movie
    {
        public string Title;
        public string OriginalTitle;
        public Uri PosterUrl;
        public string Overview;

        public Movie(string originalTitle, string title, Uri posterUrl, string overview)
        {
            Title = title;
            OriginalTitle = originalTitle;
            PosterUrl = posterUrl;
            Overview = overview;
        }
    }
}