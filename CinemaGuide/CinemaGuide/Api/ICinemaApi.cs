using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaGuide.Api
{
    public interface ICinemaApi
    {
        string Name { get; }

        Task<List<IMovieInfo>> SearchAsync(SearchConfig config);
    }
}
