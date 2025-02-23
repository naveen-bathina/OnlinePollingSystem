
using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("CORS_POLICY_LOCALHOST");

            app.MapControllers();

            app.Run();
        }
    }
}
