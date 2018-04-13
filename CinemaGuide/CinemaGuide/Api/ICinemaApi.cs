using System.Threading.Tasks;
using System.Collections.Generic;

namespace CinemaGuide.Api
{
    public interface ICinemaApi
    {
        string Name { get; }

        Task<List<IMovieInfo>> SearchAsync(SearchConfig config);
    }
}
