using Movies.API.BaseRepository.BaseRepository;
using Movies.API.Context;
using Movies.API.Models;

namespace Movies.API.Repositories.EntityRepository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
