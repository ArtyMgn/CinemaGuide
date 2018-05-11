using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaGuide.Api
{
    public interface ICinemaApi
    {
        Task<List<IMovieInfo>> SearchAsync(SearchConfig config);
    }
}
