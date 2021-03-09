using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Cw.PayslipService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cw.PayslipService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //setup authentication scheme
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        //verify the user does exist in the system
                        var userService = context.HttpContext.RequestServices
                                                 .GetRequiredService<IUserService>();
                        var userId = Convert.ToInt32(context.Principal.Identity.Name);
                        var user = userService.GetUserById(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerDocument();
            services.AddDbContext<ServiceContext>(opt => opt.UseInMemoryDatabase("PaySlipDatabase"));

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPaySlipService, PaySlipService>();
            services.AddScoped<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            var context = serviceProvider.GetService<ServiceContext>();
            SeedData(context);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SeedData(ServiceContext context)
        {
            var EmployeeA = new Employee()
            {
                Id = 1,
                Name = "Employee A",
                Salary = 100000
            };

            EmployeeA.Payslips.Add(new Payslip
            {
                PayDate = DateTime.UtcNow.Date,
                Gross = 100000,
                Tax = 30000,
                NetIncome = 70000
            });

            EmployeeA.Payslips.Add(new Payslip
            {
                PayDate = DateTime.UtcNow.Date.AddMonths(-1),
                Gross = 100000,
                Tax = 30000,
                NetIncome = 70000
            });

            var EmployeeB = new Employee()
            {
                Id = 2,
                Name = "Employee B",
                Salary = 150000
            };

            var EmployeeC = new Employee()
            {
                Id = 3,
                Name = "Employee B",
                Salary = 180000
            };

            EmployeeC.Payslips.Add(new Payslip
            {
                PayDate = DateTime.UtcNow.Date,
                Gross = 180000,
                Tax = 72000,
                NetIncome = 108000
            });

            context.AddRange(new List<Employee>
            {
                EmployeeA,
                EmployeeB,
                EmployeeC
            });

            
            var user = new User()
            {
                Id = 1,
                UserName = "user",
                Password = "password",
                IsAdmin = false
            };

            var admin = new User()
            {
                Id = 2,
                UserName = "admin",
                Password = "password",
                IsAdmin = true
            };

            context.AddRange(new List<User>
            {
                user,
                admin
            });

            context.SaveChanges();
        }
    }
}
