using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMyShowProject.Contract;

namespace BookMyShowProject.Services
{
    public interface IMovieServices
    {


        Task<APIResponse<List<Movies>>> GetMovies();
        Task<APIResponse<string>> AddMovie(Movies request);
        Task<APIResponse<string>> AddMovieSchedule(Timing request);
        Task<APIResponse<List<Timing>>> GetMoviesSchedule();
        Task<APIResponse<string>> DeleteMovieSchedule(string MovieName, string showTiming);
        Task<APIResponse<string>> UpdateStatus(string movieName);
        Task<APIResponse<string>> DeleteMovie(string movie);
        Task<APIResponse<string>> TicketBooking(string MovieName, string showTiming, int NumberOfSeats);
        
    }
}
