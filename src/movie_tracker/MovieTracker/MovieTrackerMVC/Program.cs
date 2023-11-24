using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTrackerMVC.Data;
using MovieTrackerMVC.Models;

namespace MovieTrackerMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

            builder.Services.Configure<StorageOptions>((option) =>
            {
                option.StorageAddress = builder.Configuration["STORAGE:ADDRESS"] ?? throw new InvalidOperationException("The STORAGE:ADDRESS env variable is not set");
                option.StorageKey = builder.Configuration["STORAGE:API:KEY"] ?? throw new InvalidOperationException("The STORAGE:API:KEY env variable is not set");
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies().UseSqlite(connectionString));

            builder.Services.AddDefaultIdentity<User>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = !builder.Environment.IsDevelopment();
                })
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}