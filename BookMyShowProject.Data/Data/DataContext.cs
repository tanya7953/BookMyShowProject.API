using BookMyShowProject.Contract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace BookMyShowProject.API.Data
{
    public class DataContext :DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Movies> movies { get; set; }
        public DbSet<Timing> Timings { get; set; }

    }
    
}
