using BookMyShowProject.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Azure.Core;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Azure;
using BookMyShowProject.API.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
namespace BookMyShowProject.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly DataContext _dbContext;

        public MovieServices(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<Movies>>> GetMovies()
        {
            try
            {
                var movies = await _dbContext.movies.Where(m => m.status == true).ToListAsync();
                return new APIResponse<List<Movies>>
                {
                    data= movies,
                    Status =HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Movies>>
                {
                    Error= new Error { errorMessage=ex.Message },
                    Status= HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> AddMovie(Movies request)
        {
            try
            {
                if (CheckIfMovieExists(request.MovieName))
                {
                    return new APIResponse<string> 
                    {
                        Error = new Error { errorMessage="Movie with the same name Already exist"},
                        Status= HttpStatusCode.BadRequest
                    };

                }
                _dbContext.movies.Add(request);
                await _dbContext.SaveChangesAsync();

                return new APIResponse<string>
                {
                    data="Movie added successfully",
                    Status=HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Error=new Error { errorMessage = ex.Message },
                    Status=HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> AddMovieSchedule(Timing request)
        {
            try
            {
                var existingSchedule = await _dbContext.Timings.FirstOrDefaultAsync(t => t.MovieName == request.MovieName && t.showTiming == request.showTiming);
                if (existingSchedule != null)
                {
                    return new APIResponse<string>
                    {
                        Error = new Error { errorMessage = "This schedule already exists for the same movie" },
                        Status = HttpStatusCode.BadRequest
                    };
                }
                if (CheckIfMovieExists(request.MovieName))
                {
            
                    _dbContext.Timings.Add(request);
                    await _dbContext.SaveChangesAsync();

                    return new APIResponse<string>
                    {
                        data= "Movie schedule added successfully",
                        Status= HttpStatusCode.OK
                    };
                }
                return new APIResponse<string>
                {
                    Error= new Error { errorMessage = "Movie is not Scheduled" },
                    Status= HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Error= new Error { errorMessage = ex.Message },
                    Status= HttpStatusCode.InternalServerError
                };
            }
        }


        public async Task<APIResponse<List<Timing>>> GetMoviesSchedule()
        {
            try
            {
                var timings = await _dbContext.Timings.ToListAsync();
                return new APIResponse<List<Timing>>
                {
                    data = timings,
                    Status = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Timing>>
                {
                    Error = new Error { errorMessage = ex.Message },
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> DeleteMovieSchedule(string MovieName, string showTiming)
        {
            try
            {
                var timing =await _dbContext.Timings.FirstOrDefaultAsync(t => t.MovieName == MovieName && t.showTiming == showTiming);
                if (timing != null)
                {
                    _dbContext.Timings.Remove(timing);
                    await _dbContext.SaveChangesAsync();
                    return new APIResponse<string>
                    { 
                        data= $" {MovieName} Scheduled for {showTiming} has been Cancelled",
                        Status = HttpStatusCode.OK
                    };

                }
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = $"{MovieName} is not available to delete OR {showTiming} Show for {MovieName} is not available" },
                    Status = HttpStatusCode.NotFound

                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = ex.Message },
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<string>> UpdateStatus(string movieName)
        {
            try
            {
                var movie =await _dbContext.movies.FirstOrDefaultAsync(m => m.MovieName == movieName);
                if (movie != null)
                {
                    movie.status = false;
                    await _dbContext.SaveChangesAsync();
                    return new APIResponse<string>
                    {
                        data = $"Status of {movieName} has been Updated Successfully",
                        Status = HttpStatusCode.OK
                    };
                }
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = $"{movieName} does not exist in database" },
                    Status = HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = ex.Message },
                    Status = HttpStatusCode.InternalServerError
                };

            }
        }

        public async Task<APIResponse<string>> DeleteMovie(string movie)
        {
            try
            {
                var movieEntity = await _dbContext.movies.FirstOrDefaultAsync(m => m.MovieName == movie);
                if (movieEntity != null)
                {
                    _dbContext.movies.Remove(movieEntity);
                    var timingsToDelete = _dbContext.Timings.Where(t => t.MovieName == movie).ToList();
                    _dbContext.Timings.RemoveRange(timingsToDelete);
                    await _dbContext.SaveChangesAsync();
                    return new APIResponse<string>
                    {
                        data = "Movie has been deleted",
                        Status = HttpStatusCode.OK
                    };
                }
                return new APIResponse<string>
                {
                    
                    Error = new Error { errorMessage = "This Movie Doesn't Exist" },
                    Status = HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    
                    Error = new Error { errorMessage = ex.Message },
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        private bool CheckIfMovieExists(string MovieName)
        {
            return _dbContext.movies.Any(m => m.MovieName == MovieName);
        }

        public async Task<APIResponse<string>> TicketBooking(string MovieName, string PreferedShowTiming, int NumberOfSeats)
        {
            try
            {
                var timing = await _dbContext.Timings.FirstOrDefaultAsync(t => t.MovieName == MovieName && t.showTiming == PreferedShowTiming);
                if (timing != null)
                {
                    if (timing.availableSeats >= NumberOfSeats)
                    {
                        timing.availableSeats -= NumberOfSeats;
                        await _dbContext.SaveChangesAsync();
                        return new APIResponse<string>
                        {
                            data = $"Movie '{MovieName}' Booking successful",
                            Status = HttpStatusCode.OK
                        };
                    }
                    return new APIResponse<string>
                    {
                        Error = new Error { errorMessage = "Enough Seats are Not Available for Booking" },
                        Status = HttpStatusCode.BadRequest
                    };
                }
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = $"NO Show for {MovieName} is available" },
                    Status = HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Error = new Error { errorMessage = ex.Message },
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
