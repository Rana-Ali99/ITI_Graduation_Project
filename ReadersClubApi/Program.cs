using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ReadersClubApi.Helper;
using ReadersClubApi.Service;
using ReadersClubApi.Services;
using ReadersClubCore.Data;
using ReadersClubCore.Models;

namespace ReadersClubApi
{
    public class Program
    {
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

            builder.Services.AddScoped<TokenConfiguration>();

            builder.Services.AddScoped<ReviewService>();
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<ReadersClubContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<StoryService>();
            //add channel service
            builder.Services.AddScoped<IChannelService, ChannelService>();


            // ✅ إعداد CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowDashboard", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            var env = app.Environment;
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(env.WebRootPath, "Uploads")), 
                RequestPath = "/Uploads"
            });

            app.UseCors("AllowDashboard");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
