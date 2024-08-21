using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MolotokMvc.Data;
namespace MolotokMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MolotokDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MolotokDbContext") ?? throw new InvalidOperationException("Connection string 'MolotokDbContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //to be able to use session in code
            builder.Services.AddSession( options =>
            { 
                options.IdleTimeout= TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //to be able to use session in a view
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Login}/{id?}");

            app.Run();
        }
    }
}