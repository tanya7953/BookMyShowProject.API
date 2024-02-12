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

namespace BookMyShowProject.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly IConfiguration _configuration;

        public string UsersPreferedTiming { get; private set; }
        public int SeatsNeeded { get; private set; }

        public MovieServices(IConfiguration configuration)
        {

            _configuration = configuration;

        }
        public List<Movies> GetMovies()
        {
            List<Movies> moviesList = new List<Movies>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Movies WHERE status = 1", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Movies movie = new Movies();
                        movie.ID = (int)dt.Rows[i]["ID"];
                        movie.MovieName = Convert.ToString(dt.Rows[i]["MovieName"]);
                        movie.DirectorName = Convert.ToString(dt.Rows[i]["DirectorName"]);
                        movie.TheatreName = Convert.ToString(dt.Rows[i]["TheatreName"]);
                        movie.status = (bool)dt.Rows[i]["status"];
                        movie.genre = Convert.ToString(dt.Rows[i]["genre"]);
                        movie.duration = (int)dt.Rows[i]["duration"];
                        moviesList.Add(movie);
                    }
                }
            }
            return moviesList;
        }

        public string AddMovie(Movies request)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                {
                    con.Open();
                    if (request.MovieName == "")
                    {
                        return "Enter Valid Movie Name";
                    }
                    if (request.DirectorName == "")
                    {
                        return "Enter Valid Director Name";
                    }
                    if (request.TheatreName == "")
                    {
                        return "Enter Valid Theater Name";
                    }
                    if (request.duration == 0)
                    {
                        return "Enter Duration Of Movie";
                    }
                    string addMovie = "INSERT INTO Movies(MovieName,DirectorName,TheatreName,status,genre,duration) VALUES('" + request.MovieName + "' , '" + request.DirectorName + "' , '" + request.TheatreName + "','" + request.status + "' ,'" + request.genre + "','" + request.duration + "')";
                    using (SqlCommand cmd = new SqlCommand(addMovie, con))
                    {
                        int result = cmd.ExecuteNonQuery();
                        con.Close();
                        if (result > 0)
                        {
                            return JsonConvert.SerializeObject(request);
                        }
                        return string.Empty;


                    }
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string AddMovieSchedule(Timing request)
        {
            try
            {
                if (CheckIfMovieExists(request.MovieName))
                {
                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                    {
                        con.Open();
                        
                        if (request.showTiming == "")
                        {
                            return "Enter Valid Show Timing Like Evening,Morning ,Night..";
                        }
                        


                        string addMovieSchedule = "INSERT INTO Timings(MovieName,showTiming,availableSeats) VALUES('" + request.MovieName + "' , '" + request.showTiming + "','" + request.availableSeats + "')";
                        using (SqlCommand cmd = new SqlCommand(addMovieSchedule, con))
                        {
                            int result = cmd.ExecuteNonQuery();
                            con.Close();
                            if (result > 0)
                            {
                                return JsonConvert.SerializeObject(request);
                            }
                            return string.Empty;


                        }
                    }
                }
                return "Movie Not Scheduled";

            }
            catch (Exception ex)
            {
                return ex + "";
            }


        }

        public string UpdateStatus(string movieName)
        {
            try
            {
                if (CheckIfMovieExists(movieName))
                {


                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                    {
                        con.Open();
                        string UpdateStatus = "UPDATE Movies SET status = 0 where MovieName = @movieName";

                        SqlCommand cmd = new SqlCommand(UpdateStatus, con);
                        cmd.Parameters.AddWithValue("@movieName", movieName);
                        int result = cmd.ExecuteNonQuery();
                        con.Close();
                        if (result > 0)
                        {
                            return "Updated Sucessfully"; ;
                        }
                        return "Updated UnSucessfully";
                    } }
                    return "Movie Don't Exist";
                   
                
            }
            catch (Exception ex)
            {
                return ex + "";
            }

        }

        public string deleteMovie(string movie)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                {
                    con.Open();
                   
                    SqlCommand cmd = new SqlCommand("DELETE FROM Movies WHERE MovieName= @MovieName", con);
                    cmd.Parameters.AddWithValue("@MovieName", movie);
                    int result = cmd.ExecuteNonQuery();
                    
                    con.Close();
                    if(result > 0)
                    {
                        return "Deleted Sucessfully";
                    }
                    return "This Movie Doesn't Exist";
                   
                }

            }
            catch (Exception ex)
            {
                return ex+"";
            }

        }



        private bool CheckIfMovieExists(string movieName)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Movies WHERE MovieName = @MovieName", con);
                cmd.Parameters.AddWithValue("@MovieName", movieName);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return count > 0;
            }
        }

        public string TicketBooking(string MovieName, string UserPreferedTiming, int NumberOfSeats)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                {
                    if (CheckIfMovieExists(MovieName))
                    {
                        string[] validTimes = { "Morning", "Afternoon", "Evening", "Night" };
                        string inputTime = UserPreferedTiming;

                        if (!validTimes.Contains(inputTime, StringComparer.OrdinalIgnoreCase))
                        {
                            return "Invalid Time Added";
                        }

                        con.Open();


                        SqlCommand cmd3 = new SqlCommand("SELECT availableSeats FROM Timings WHERE MovieName = @MovieName", con);
                        cmd3.Parameters.AddWithValue("@MovieName", MovieName);

                        int seatsLeft = Convert.ToInt32(cmd3.ExecuteScalar());
                        int seatsLeftAfterBooking = seatsLeft - NumberOfSeats;

                        if (seatsLeftAfterBooking < 0)
                        {
                            return "Not enough seats available";
                        }

                        SqlCommand cmd4 = new SqlCommand("SELECT status FROM Movies WHERE MovieName = @MovieName", con);
                        cmd4.Parameters.AddWithValue("@MovieName", MovieName);
                        bool status = Convert.ToBoolean(cmd4.ExecuteScalar());

                        if (!status)
                        {
                            return "Movie Currently InActive";
                        }

                      

                        SqlCommand cmd2 = new SqlCommand("UPDATE Timings SET availableSeats = @SeatsLeftAfterBooking WHERE MovieName = @MovieName and showTiming = @inputTime", con);
                        cmd2.Parameters.AddWithValue("@SeatsLeftAfterBooking", seatsLeftAfterBooking);
                        cmd2.Parameters.AddWithValue("@MovieName", MovieName);
                        cmd2.Parameters.AddWithValue("@inputTime", inputTime);
                        int rowsAffected_ = cmd2.ExecuteNonQuery();

                        con.Close();
                        return $"Movie '{MovieName}' Booking successful";
                    }
                }

                return "No Movie Found";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }


        }
    }
