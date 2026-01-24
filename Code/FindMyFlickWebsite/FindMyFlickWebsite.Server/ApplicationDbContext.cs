using Microsoft.EntityFrameworkCore;
using FindMyFlickWebsite.Server.Models;

namespace FindMyFlickWebsite.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        DbSet<Movies> Movie { get; set; }
        DbSet<Tags> Tag { get; set; }
    }
}