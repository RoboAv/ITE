using FirstTaskProj.Database;
using Microsoft.EntityFrameworkCore;
using FirstTaskProj.Database.Seeder;
using FirstTaskProj;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
    );

services.AddMvc(options => options.EnableEndpointRouting = false);

services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Request: {method} {url}",
        context.Request.Method,
        context.Request.Path
        );

    await next();
    logger.LogInformation("Response: {code}",
        context.Response.StatusCode);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    DbSeeder.Seed(context);
}

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

//app.UseSwaggerUi();

app.Run();