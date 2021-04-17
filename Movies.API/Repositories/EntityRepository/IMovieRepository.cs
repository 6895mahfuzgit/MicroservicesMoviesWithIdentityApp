using Movies.API.BaseRepository.BaseRepository;
using Movies.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.API.Repositories.EntityRepository
{
    public interface IMovieRepository:IRepository<Movie>
    {

    }
}
