using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMyShowProject.Contract;

namespace BookMyShowProject.Services
{
    public interface IMovieServices
    {
        List<Movie> GetMovies();
        string AddMovie(Movie request);
    }
}
