using Microsoft.EntityFrameworkCore;
using Movies.API.Models;

namespace Movies.API.Context
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}
