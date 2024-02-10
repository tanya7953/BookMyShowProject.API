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
        public MovieServices(IConfiguration configuration)
        {

            _configuration = configuration; 

        }
        public List<Movies> GetMovies() { 
            List<Movies> moviesList= new List<Movies>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Movies WHERE status = 1",con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0)
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
            catch(Exception ex)
            {
               return string.Empty;
            }   
        }

        public string AddMovieSchedule(Timing request)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                {
                    con.Open();
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
            catch(Exception ex)
            {
                return ex + "";
            }


        }

        public string UpdateStatus(string movieName)
        {
            try
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
                }
            }
            catch (Exception ex)
            {
                return ex+"";
            }
            
        }

        public string deleteMovie(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString()))
                {
                    con.Open();
                    string deleteMovie = "DELETE FROM Movies WHERE ID=" + id;
                    using (SqlCommand cmd = new SqlCommand(deleteMovie, con))
                    {
                        int result = cmd.ExecuteNonQuery();
                        con.Close();

                    }
                    return "Deleted Sucessfully";
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            
        }

        
    }
}
