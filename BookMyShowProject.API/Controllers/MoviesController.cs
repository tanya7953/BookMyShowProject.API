using Microsoft.AspNetCore.Http;
using BookMyShowProject.Contract;
using BookMyShowProject.Services;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace BookMyShowProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MovieController : ControllerBase
    {
        IMovieServices _movieService;

        public MovieController(IMovieServices movieService)
        {
            _movieService = movieService;
        }
        

        [HttpGet]
        [Route("GetMovies")]
        public List<Movies> GetMovies()
        {
            return _movieService.GetMovies();
        }
       
        
        [HttpPost]
        [Route("AddMovie")]
        public string AddMovie(Movies request)
        {
            return _movieService.AddMovie(request);
        }


        [HttpPost]
        [Route("AddMovieSchedule")]
        public string AddMovieSchedule(Timing request)
        {
            return _movieService.AddMovieSchedule(request);
        }


        [HttpPut]
        [Route("UpdateStatus")]
        public string UpdateStatus(string movieName)
        {     
            return _movieService.UpdateStatus(movieName);
        }
        
        
        [HttpDelete]
        [Route("deleteMovie")]
        public string deleteMovie(int id) 
        {
            return _movieService.deleteMovie(id);
        }

        

        }

    
}
 