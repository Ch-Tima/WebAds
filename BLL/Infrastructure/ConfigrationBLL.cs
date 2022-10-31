
using DLL.Context;
using DLL.Repository;
using Microsoft.Extensions.Configuration;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Infrastructure
{
    public static class ConfigrationBLL
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            var t = configuration.GetValue<string>("DbConection");
            services.AddDbContext<AdDbContext>(opt => opt.UseSqlServer(configuration.GetValue<string>("DbConection")));

            services.AddTransient<AdRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddTransient<CityRepository>();
            services.AddTransient<CommentRepository>();
            services.AddTransient<UserRepository>();

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AdDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Identity/Account/Login";
            });
        }
    }
}
