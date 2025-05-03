using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ReadersClubApi.Helper;
using ReadersClubApi.Service;
using ReadersClubApi.Services;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ReadersClubApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
         

            // جلب الإعدادات من appsettings
            var secretKey = builder.Configuration["JWT:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            // إعداد المصادقة
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

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
