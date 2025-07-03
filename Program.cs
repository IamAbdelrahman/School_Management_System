using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;

namespace School_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ITIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("iticonfig")));


            //For register
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            


            // Register the DbContext with dependency injection
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

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

            app.Run();
        }
    }
}
