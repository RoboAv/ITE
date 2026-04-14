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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder
        //        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFDataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
        //        .UseSeeding((context, _) =>
        //        {
        //            var testBlog = context.Set<City>().FirstOrDefault(b => b.Name == "City 17");
        //            if (testBlog == null)
        //            {
        //                context.Set<Blog>().Add(new Blog { Url = "http://test.com" });
        //                context.SaveChanges();
        //            }
        //        })
        //        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        //        {
        //            var testBlog = await context.Set<Blog>().FirstOrDefaultAsync(b => b.Url == "http://test.com", cancellationToken);
        //            if (testBlog == null)
        //            {
        //                context.Set<Blog>().Add(new Blog { Url = "http://test.com" });
        //                await context.SaveChangesAsync(cancellationToken);
        //            }
        //        });

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}


