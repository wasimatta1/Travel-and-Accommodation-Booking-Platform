

using Application.Exceptions;
using Application.Mediator.Behavior;
using Application.Mediator.Handlers;
using Application.Profiles;
using Application.Service;
using Contracts.Interfaces;
using Domain.Entities;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbContextConnection"));
            });

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterHandler).Assembly));

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssembly(typeof(RegisterHandler).Assembly));

            builder.Services.AddAutoMapper(typeof(UserProfile));

            builder.Services.AddScoped<ITokenService, TokenService>();


            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.UseExceptionHandler();

            using (var scope = app.Services.CreateScope())
            {
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new string[] { "Admin", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManger.RoleExistsAsync(role))
                    {
                        await roleManger.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            //create the admin user
            using (var scope = app.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var email = "admin@admin.com";
                var password = "Admin123";

                if (await userManger.FindByEmailAsync(email) == null)
                {

                    var adminUser = new User
                    {
                        UserName = "admin",
                        NormalizedUserName = "ADMIN",
                        Email = "Admin@Admin.com",
                        NormalizedEmail = "ADMIN@ADMIN.COM",
                        FirstName = "Admin",
                        LastName = "User",
                        DateOfBirth = new DateTime(1990, 1, 1)
                    };

                    await userManger.CreateAsync(adminUser, password);

                    await userManger.AddToRoleAsync(adminUser, "Admin");
                }
            }
            app.Run();
        }
    }
}
