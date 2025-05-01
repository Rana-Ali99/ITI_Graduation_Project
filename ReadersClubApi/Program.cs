using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadersClubApi.Services;
using ReadersClubCore.Data;
using ReadersClubCore.Models;

namespace ReadersClubApi
{
    public class Program
    {
        //F:\ITI-4months\Graduation Project\Dashboard\ReadersClubDashboard\
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ReadersClubContext>(
                options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
            );

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ReadersClubContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<StoryService>();

            // ”Ì«”… CORS  ”„Õ · ÿ»Ìﬁ Angular ⁄·Ï localhost:4200
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy => policy.WithOrigins("http://localhost:4200") // «·”„«Õ ·‹ Angular
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            var app = builder.Build();

            //  ›⁄Ì· CORS
            app.UseCors("AllowLocalhost");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseStaticFiles();


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
