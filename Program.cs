using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FitnessTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddIdentity<FitnessUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            var serviceProvider = builder.Services.BuildServiceProvider();
            // // Create the "Admin" role
            // var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // var role = new IdentityRole { Name = "Admin" };
            // var roleExists = await roleManager.RoleExistsAsync("Admin");
            // if (!roleExists)
            // {
            //     await roleManager.CreateAsync(role);
            // 
            // verify role assigned to user
            //var userRoles2 = await userManager.GetRolesAsync(user);
            // // Create the "PremiumUser" role
            // var role2 = new IdentityRole { Name = "PremiumUser" };
            // var roleExists2 = await roleManager.RoleExistsAsync("PremiumUser");
            // if (!roleExists2)
            // {
            //     await roleManager.CreateAsync(role2);
            // }

            // // Assign the "Admin" role to the "admin@example.com" user
            // var userManager = serviceProvider.GetRequiredService<UserManager<FitnessUser>>();
            // var user = await userManager.FindByEmailAsync("admin@example.com");
            // if (user != null)
            // {
            //     var userRoles = await userManager.GetRolesAsync(user);
            //     if (!userRoles.Contains(role.Name))
            //     {
            //         await userManager.AddToRoleAsync(user, role.Name);
            //     }
            // }

            // // Restrict registration with admin email
            // builder.Services.Configure<IdentityOptions>(options =>
            // {
            //     options.User.RequireUniqueEmail = true;
            //     options.SignIn.RequireConfirmedEmail = true;
            // });

            builder.Services.AddScoped<WorkoutsSearchAPIController>();
            // // Add JWT authentication
            // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            //     {
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            //             ValidateIssuer = false,
            //             ValidateAudience = false
            //         };
            //     });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}