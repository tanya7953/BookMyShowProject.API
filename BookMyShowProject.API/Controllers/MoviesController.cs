using Microsoft.AspNetCore.Http;
using BookMyShowProject.Contract;
using BookMyShowProject.Services;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Net;
using Microsoft.EntityFrameworkCore;

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
        public async Task<APIResponse<List<Movies>>> GetMovies()
        {
            return await _movieService.GetMovies();
        }


        [HttpPost]
        [Route("AddMovie")]
        public async Task<APIResponse<string>> AddMovie(Movies request)
        {
            if (string.IsNullOrWhiteSpace(request.MovieName))
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = "Enter Valid Movie Name" },
                    Status = HttpStatusCode.BadRequest
                };
            if (string.IsNullOrWhiteSpace(request.DirectorName))
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = "Enter Valid Director Name" },
                    Status = HttpStatusCode.BadRequest
                };
            if (string.IsNullOrWhiteSpace(request.TheatreName))
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = "Enter Valid Theater Name" },
                    Status = HttpStatusCode.BadRequest
                };
            if (request.duration == 0)
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = "Enter Duration Of Movie" },
                    Status = HttpStatusCode.BadRequest
                };
            
            return await _movieService.AddMovie(request);
        }




        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<APIResponse<string>> UpdateStatus(string movieName)
        {
            return await _movieService.UpdateStatus(movieName);
        }


        [HttpDelete]
        [Route("DeleteMovie")]
        public async Task<APIResponse<string>> DeleteMovie(string movie)
        {
            return await _movieService.DeleteMovie(movie);
            
        }

        [HttpPost]
        [Route("AddMovieSchedule")]
        public async Task<APIResponse<string>> AddMovieSchedule(Timing request)
        {



            if (!string.Equals(request.showTiming, "Morning", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(request.showTiming, "Afternoon", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(request.showTiming, "Evening", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(request.showTiming, "Night", StringComparison.OrdinalIgnoreCase)) { 
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = "Enter Valid SHow Timing" },
                    Status = HttpStatusCode.BadRequest
                };
            }
            if (request.availableSeats == 0 || request.availableSeats > 501)
            {
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = "Enter Correct Number Of Seats" },
                    Status = HttpStatusCode.BadRequest
                };
            }




            return await _movieService.AddMovieSchedule(request);
        }

        [HttpGet]
        [Route("GetMoviesSchedule")]
        public async Task<APIResponse<List<Timing>>> GetMoviesSchedule()
        {
            return await _movieService.GetMoviesSchedule();
        }

        [HttpDelete]
        [Route("deleteMovieSchedule")]
        public async Task<APIResponse<string>> DeleteMovieSchedule(string MovieName, string showTiming)
        {
            return await _movieService.DeleteMovieSchedule(MovieName, showTiming);

        }

    }

    
}
 