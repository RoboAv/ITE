using Swashbuckle.AspNetCore.Swagger;

namespace FirstTaskProj
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
               });

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "/{controller=Home}/{action=Index}/{id?}");
                }
            );
        }
    }
}
