using CarRental.context;
using CarRental.Repository;
using CarRental.Repository.DAL;
using Microsoft.EntityFrameworkCore;

namespace AirportsDetail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var string1= builder.Configuration.GetConnectionString("default")?.ToString();
            builder.Services.AddDbContext<DbContextclass>(options => options.UseSqlServer(string1));
            builder.Services.AddTransient<ICarDAL, CarDAL>();
            builder.Services.AddTransient<ICustomerDAL, CustomerDAL>();
            builder.Services.AddTransient<IRentDAL, RentDAL>(); 
            builder.Services.AddMvcCore();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}