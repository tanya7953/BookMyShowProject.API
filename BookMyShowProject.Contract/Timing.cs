using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyShowProject.Contract
{
    public class Timing
    {
        
        public int Id { get; set; }   
        public string MovieName { get; set;}
        public string showTiming { get; set;}
        public int availableSeats {  get; set; }

    }
}
