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
    public class OmdbApi: ICinemaApi
    {
        public string Name => "OMDb";
        
        private readonly string token;
        private const string BASE_URL = "http://www.omdbapi.com";
        private readonly HttpClient client;

        public OmdbApi(string token)
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
            
            var requestUri = new Uri($"{BASE_URL}{queryString.ToUriComponent()}");
            
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
            public string    Id            { get; set; }
            public string    Title         { get; set; }

            [JsonIgnore]
            public string    OriginalTitle { get; set; }
     
            [JsonProperty("Poster")]
            public Uri       PosterUrl     { get; set; }

            public int Year { get; set; }

            [JsonIgnore]
            public DateTime? ReleaseDate => new DateTime(Year, 1, 1);
        }
    }
}
