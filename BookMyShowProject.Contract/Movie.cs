using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyShowProject.Contract
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }
        public string MovieName { get; set; }
        public string DirectorName { get; set;}
        public string TheatreName { get; set; }
        public bool status {  get; set; }
        public string genre { get; set; }
        public int duration { get; set; }
        //public  ICollection<Timing> showTiming { get; set; }
    }
}
