using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Cw.PayslipService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cw.PayslipService.Tests
{
    public class TestFixture
    {
        public const int TEST_EMPLOYEE_ID_1 = 1;
        public const int TEST_SALARY_1 = 100000;
        public const int TEST_EMPLOYEE_ID_2 = 2;
        public const int TEST_SALARY_2 = 180000;
        public const int TEST_EMPLOYEE_ID_3 = 3;
        public const string TEST_USERNAME_1 = "username 1";
        public const int TEST_USER_ID_1 = 1;
        public const string TEST_PASSWORD_1 = "password 1";
        public const int TEST_USER_ID_2 = 2;
        public const string TEST_USERNAME_2 = "username 2";
        public const string TEST_PASSWORD_2 = "password 2";

        public readonly Mock<ServiceContext> Context;
        public readonly IEmployeeService EmployeeService;
        public readonly IPaySlipService PaySlipService;
        public readonly IUserService UserService;

        public TestFixture()
        {
            var EmployeeDbSet = new Mock<DbSet<Employee>>();

            IQueryable<Employee> employees = new List<Employee>
            {
                new Employee() 
                { 
                    Id = TEST_EMPLOYEE_ID_1,
                    Name = "test employee 1",
                    Salary = TEST_SALARY_1
                },
                new Employee() 
                { 
                    Id = TEST_EMPLOYEE_ID_2,
                    Name = "test employee 2",
                    Salary = TEST_SALARY_2
                }
            }.AsQueryable();

            EmployeeDbSet.As<IQueryable<Employee>>()
                   .Setup(m => m.Provider)
                   .Returns(employees.Provider);
            EmployeeDbSet.As<IQueryable<Employee>>()
                   .Setup(m => m.Expression)
                   .Returns(employees.Expression);
            EmployeeDbSet.As<IQueryable<Employee>>()
                   .Setup(m => m.ElementType)
                   .Returns(employees.ElementType);
            EmployeeDbSet.As<IQueryable<Employee>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(employees.GetEnumerator());

            var paySlipDbSet = new Mock<DbSet<Payslip>>();

            IQueryable<Payslip> paySlips = new List<Payslip>
            {
                new Payslip() { Id = 1, EmployeeId = 1, NetIncome = 70000, Gross = 100000, Tax = 30000, PayDate = DateTime.UtcNow.Date.AddMonths(-1) }
            }.AsQueryable();

            paySlipDbSet.As<IQueryable<Payslip>>()
                         .Setup(m => m.Provider)
                         .Returns(paySlips.Provider);
            paySlipDbSet.As<IQueryable<Payslip>>()
                         .Setup(m => m.Expression)
                         .Returns(paySlips.Expression);
            paySlipDbSet.As<IQueryable<Payslip>>()
                         .Setup(m => m.ElementType)
                         .Returns(paySlips.ElementType);
            paySlipDbSet.As<IQueryable<Payslip>>()
                         .Setup(m => m.GetEnumerator())
                         .Returns(paySlips.GetEnumerator());

            var userDbSet = new Mock<DbSet<User>>();

            IQueryable<User> users = new List<User>
            {
                new User() { Id = TEST_USER_ID_1, UserName = TEST_USERNAME_1, Password = TEST_PASSWORD_1 }
            }.AsQueryable();

            userDbSet.As<IQueryable<User>>()
                         .Setup(m => m.Provider)
                         .Returns(users.Provider);
            userDbSet.As<IQueryable<User>>()
                         .Setup(m => m.Expression)
                         .Returns(users.Expression);
            userDbSet.As<IQueryable<User>>()
                         .Setup(m => m.ElementType)
                         .Returns(users.ElementType);
            userDbSet.As<IQueryable<User>>()
                         .Setup(m => m.GetEnumerator())
                         .Returns(users.GetEnumerator());

            //config database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("Test");
            Context = new Mock<ServiceContext>(dbContextOptionsBuilder.Options);

            Context.Setup(m => m.Employees)
                   .Returns(() => EmployeeDbSet.Object);
            Context.Setup(m => m.Payslips)
                   .Returns(() => paySlipDbSet.Object);
            Context.Setup(m => m.Users)
                   .Returns(() => userDbSet.Object);

            EmployeeService = new EmployeeService(Context.Object);
            PaySlipService = new PaySlipService(Context.Object);
            var settings = new AppSettings()
            {
                Secret = "15cb5668-a093-4026-b5aa-cdef304cbc81"
            };
            IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
            UserService = new UserService(Context.Object, appSettingsOptions);
        }
    }
}
