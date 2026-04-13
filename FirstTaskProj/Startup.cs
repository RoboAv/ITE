using FirstTaskProj.Database;
using FirstTaskProj.Models;
using FirstTaskProj.Repositories;
using FirstTaskProj.Services;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureService (IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddMvc();
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            services.AddTransient<IGrowPopulationService, GrowPopulationService>();
            services.AddTransient<IBaseRepository<City>, BaseRepository<City>>();
            services.AddTransient<IBaseRepository<Region>, BaseRepository<Region>>();
            services.AddTransient<IBaseRepository<Country>, BaseRepository<Country>>();
        }

        public void Configure (IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
