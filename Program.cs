using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using School_Management_System.Repositories;
using School_Management_System.Repositories.Implementations;
using School_Management_System.Repositories.Interfaces;
using System;

namespace School_Management_System
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

            builder.Services.AddScoped<IExamRepository, ExamRepository>();

            // Register the DbContext with dependency injection

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ITIContext>()
            .AddDefaultTokenProviders();

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
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DbInitializer.SeedAsync(services);
            }

            app.Run();
        }
    }
}
