using FirstTaskProj.Models;
using Microsoft.EntityFrameworkCore;
using FirstTaskProj.Database.Views;

namespace FirstTaskProj.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        //public DbSet<FullCountryView> FullCountryViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FullCountryView>()
                .HasNoKey()
                .ToView("FullCountryView");
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
    }
}


