using DLL.Context;
using DLL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Infrastructure
{
    public static class ConfigrationBLL
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            var conDef = configuration.GetConnectionString("def");//appsettings
            var conMain = configuration.GetValue<string>("DbConection");//Azure secret key
            services.AddDbContext<AdDbContext>(opt => opt.UseSqlServer(conDef));

            //Add Repository
            services.AddTransient<AdRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddTransient<CityRepository>();
            services.AddTransient<CommentRepository>();
            services.AddTransient<UserRepository>();

            //Set setings Identity
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                //Password settings
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 4;

            }).AddEntityFrameworkStores<AdDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Identity/Account/Login";
            });
        }
    }
}
