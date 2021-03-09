using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Moq;
using Xunit;

namespace Cw.PayslipService.Tests
{
    public class EmployeeServiceTests : IClassFixture<TestFixture>
    {
        private readonly IEmployeeService _employeeService;
        private readonly Mock<ServiceContext> _context;

        public EmployeeServiceTests(TestFixture fixture)
        {
            _employeeService = fixture.EmployeeService;
            _context = fixture.Context;
        }

        [Fact]
        public void SubmitEmployee()
        {
            _context.Invocations.Clear();

            var result = _employeeService.SubmitEmployee(new Employee() { Name = "test", Salary = 120000 });

            Assert.IsType<int>(result);

            var exception = Record.Exception(
                () =>
                {
                    _context.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once);
                    _context.Verify(x => x.SaveChanges(), Times.Once);
                }
            );

            Assert.Null(exception);
        }
    }
}
