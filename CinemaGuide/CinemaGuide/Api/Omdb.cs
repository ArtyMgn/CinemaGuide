using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CinemaGuide.Api
{
    [Name("OMDb")]
    public class Omdb : ICinemaApi
    {
        public const string BaseUrl = "http://www.omdbapi.com";

        private readonly string token;
        private readonly HttpClient client;

        public Omdb(string token)
        {
            this.token = token;
            client = new HttpClient();
        }

        public async Task<List<IMovieInfo>> SearchAsync(SearchConfig config)
        {
            var queryString = new QueryString()
                .Add("apikey", token)
                .Add("s", config.Query)
                .Add("type", "movie");

            var requestUri = new Uri($"{BaseUrl}{queryString.ToUriComponent()}");
            var response = await client.GetAsync(requestUri);
            var jsonContent = await response.Content.ReadAsStringAsync();

            return JObject.Parse(jsonContent)["Search"]
                .Children()
                .Select(item => item.ToObject<MovieInfo>())
                .Cast<IMovieInfo>()
                .ToList();
        }

        private struct MovieInfo : IMovieInfo
        {
            [JsonProperty("imdbID")]
            public string Id { get; set; }

            public string Title { get; set; }

            [JsonIgnore]
            public string OriginalTitle => Title;

            public int? Year { get; set; }

            [JsonProperty("Poster")]
            public Uri PosterUrl { get; set; }

            [JsonIgnore]
            public DateTime? ReleaseDate { get; set; }
        }
    }
}
