using Microsoft.EntityFrameworkCore;
using Smalldatabaseapp.Models;

namespace Smalldatabaseapp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> People { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=people.db");
        }
    }
}