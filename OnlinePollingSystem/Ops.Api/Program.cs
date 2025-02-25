using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ops.Api.Repositories;
using Ops.Api.Services;

namespace Ops.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PollContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IPollService, PollService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "CORS_POLICY_LOCALHOST",
                                  builder =>
                                  {
                                      builder
                                        .WithOrigins("http://localhost:5173") // specifying the allowed origin
                                        .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH") // defining the allowed HTTP method
                                        .AllowAnyHeader() // allowing any header to be sent
                                        .AllowCredentials();
                                  });
            });

            // Add Swagger generation
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Online Polling Service API",
                    Version = "v1",
                    Description = "API for managing polls and device information",
                    Contact = new OpenApiContact
                    {
                        Name = "Naveen Bathina",
                        Email = "naveen.bathina@yahoo.com"
                    }
                });

                // Optional: Include XML comments for richer documentation
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ops API v1");
                    c.RoutePrefix = string.Empty; // Makes Swagger UI available at root (/)
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("CORS_POLICY_LOCALHOST");

            app.MapControllers();

            app.Run();
        }
    }
}
