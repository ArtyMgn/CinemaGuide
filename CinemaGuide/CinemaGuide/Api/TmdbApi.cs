﻿using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Search;
using System.Collections.Generic;

namespace CinemaGuide.Api
{
    public class TmdbApi : ICinemaApi
    {
        public string Name => "TMDb";

        private readonly TMDbClient client;

        public TmdbApi(string token)
        {
            client = new TMDbClient(token);
        }

        public async Task<List<IMovieInfo>> SearchAsync(SearchConfig config)
        {
            client.DefaultLanguage = config.Language; // not thread safe

            var searchContainer = await client.SearchMovieAsync(config.Query, includeAdult: config.AllowAdult);

            return searchContainer.Results
                .Select(m => new MovieInfo(m, client))
                .Cast<IMovieInfo>()
                .ToList();
        }

        private struct MovieInfo : IMovieInfo
        {
            public int Id                { get; }
            public string Title          { get; }
            public string OriginalTitle  { get; }
            public Uri PosterUrl         { get; }
            public DateTime? ReleaseDate { get; }

            public MovieInfo(SearchMovie movie, TMDbClient client)
            {
                client.GetConfig();

                Id            = movie.Id;
                Title         = movie.Title;
                OriginalTitle = movie.OriginalTitle;
                PosterUrl     = client.GetImageUrl("original", movie.PosterPath);
                ReleaseDate   = movie.ReleaseDate;
            }
        }
    }
}