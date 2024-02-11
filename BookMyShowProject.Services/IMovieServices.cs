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
       

        List<Movies> GetMovies();
        string AddMovie(Movies request);
        string AddMovieSchedule(Timing request);
        string UpdateStatus(string MovieName);
        string deleteMovie(int id);
        //string TicketBooking(string MovieName, string showTiming, int NumberOfSeats);
        
    }
}
