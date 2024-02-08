using BookMyShowProject.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyShowProject.Services
{
    public class MovieServices : IMovieServices
    {
        
        public List<Movie> GetMovies()
        {
            return new List<Movie>(){
                new Movie(){ MovieName="KGF",DirectorName="Abc",TheatreName="Abc",status=true,genre="Action" ,duration=190,},
                new Movie(){ MovieName="KGF2",DirectorName="Abc",TheatreName="Abc",status=true,genre="Action" ,duration=190,},
                new Movie(){ MovieName="Fighter",DirectorName="Abc",TheatreName="Abc",status=true,genre="Action" ,duration=190,}
            };
        }
        public string AddMovie(Movie request)
        {
            throw new NotImplementedException();
        }

       
        
    }
}
