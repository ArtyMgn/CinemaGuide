using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace CinemaGuide.Models.MoviesDatabases
{
    public class Tmdb : IMovieDatabase
    {
        private readonly TMDbClient сlient;

        public Tmdb()
        {
            var token = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            сlient = new TMDbClient(token);
        }

        public async Task<List<Movie>> SearchAsync(string query, string lang, bool allowAdult)
        {
            сlient.DefaultLanguage = lang;
            var searchContainer = await сlient.SearchMovieAsync(query, includeAdult: allowAdult);
            return searchContainer.Results
                .Select(CreateMovie)
                .ToList();
        }

        private Movie CreateMovie(SearchMovie movieInfo)
        {
            сlient.GetConfig();
            var posterUrl = сlient.GetImageUrl("original", movieInfo.PosterPath);
       
            return new Movie(
                movieInfo.OriginalTitle, 
                movieInfo.Title, 
                posterUrl,
                movieInfo.Overview
            );
        }
    
        public string Name => "TMDB";
    }
}