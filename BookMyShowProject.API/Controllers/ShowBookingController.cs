using BookMyShowProject.Contract;
using BookMyShowProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShowProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowBookingController : Controller
    {


        IMovieServices _movieService;
        public ShowBookingController(IMovieServices movieService)
        {
            _movieService = movieService;
        }
        [HttpPost]
            [Route("TicketBooking")]
            public async Task<APIResponse<string>> TicketBooking(string MovieName, string PreferedShowTiming, int NumberOfSeats)
            {
                return await _movieService.TicketBooking(MovieName, PreferedShowTiming, NumberOfSeats);
            }
        
    }
}
