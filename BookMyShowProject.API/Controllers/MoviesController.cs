using Microsoft.AspNetCore.Http;
using BookMyShowProject.Contract;
using BookMyShowProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShowProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        
        public class MovieController : ControllerBase
        {
            IMovieServices _movieService;

            public MovieController(IMovieServices movieService)
            {
                _movieService = movieService;
            }

            [HttpGet]
            [Route("GetMovies")]
            public List<Movie> GetMovies()
            {
                return _movieService.GetMovies();
            }
        }
    }
}
 