using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaGuide.Models.MoviesDatabases
{
    public interface IMovieDatabase
    {
        string Name { get; }
        Task<List<Movie>> SearchAsync(string query, string lang, bool allowAdult);
    }
}