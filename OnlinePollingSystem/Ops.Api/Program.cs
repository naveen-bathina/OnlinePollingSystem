using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ops.Api.Models;
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
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost", "http://localhost:5173", "http://10.0.2.2")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
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

            builder.Services.AddSignalR();

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

            if (app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.MapControllers();

            app.MapHub<PollHub>("/pollHub");

            app.Run();
        }
    }
}
