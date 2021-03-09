using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Cw.PayslipService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
            services.AddSwaggerDocument();
            services.AddDbContext<ServiceContext>(opt => opt.UseInMemoryDatabase("PaySlipDatabase"));

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPaySlipService, PaySlipService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            var context = serviceProvider.GetService<ServiceContext>();
            SeedData(context);

            app.UseHttpsRedirection();

            app.UseRouting();

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

            context.SaveChanges();
        }
    }
}
