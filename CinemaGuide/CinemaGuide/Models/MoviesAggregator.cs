using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaGuide.Models.MoviesDatabases;

namespace CinemaGuide.Models
{
    public class MoviesAggregator
    {
        private readonly List<IMovieDatabase> sourses;
        private readonly AggregatorConfig config;
        
        public MoviesAggregator(IEnumerable<IMovieDatabase> sourses, AggregatorConfig config)
        {
            this.sourses = sourses.ToList();
            this.config = config;
        }

        public async Task<Dictionary<string, List<Movie>>> SearchAsync(string query)
        {
            var searchResult = new Dictionary<string, List<Movie>>();
            
            foreach (var sourse in sourses)
            {
                var foundMovies = await sourse.SearchAsync(query, config.DefaultLanguage, config.AllowAdultContent);
                if (!foundMovies.Any())
                {
                    continue;
                }
                
                searchResult.Add(sourse.Name, foundMovies);
            }
            
            return searchResult;
        }
    }
}