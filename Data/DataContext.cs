using eObrazci.Models;
using Microsoft.EntityFrameworkCore;

namespace eObrazci.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Naslov> Naslovi { get; set; }
        public DbSet<Izpit> Izpiti { get; set; }
        public DbSet<Obrazec> Obrazci { get; set; }


    }
}
