using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using System.Collections.Generic;
using Xunit;

namespace Cw.PayslipService.Tests
{
    public class PaySlipServiceTests : IClassFixture<TestFixture>
    {
        private readonly IPaySlipService _paySlipService;

        public PaySlipServiceTests(TestFixture fixture)
        {
            _paySlipService = fixture.PaySlipService;
        }

        [Fact]
        public void GetAllPaySlips_ValidEmployeeId()
        {
            var result = _paySlipService.GetPaySlipsByEmployeeId(TestFixture.TEST_EMPLOYEE_ID_1);
            Assert.IsType<List<Payslip>>(result);
        }

        [Fact]
        public void GetAllPaySlips_InvalidEmployeeId()
        {
            var exception = Assert.Throws<KeyNotFoundException>(
                () => _paySlipService.GetPaySlipsByEmployeeId(TestFixture.TEST_EMPLOYEE_ID_3)
            );

            Assert.Equal($"Employee with id: {TestFixture.TEST_EMPLOYEE_ID_3} is not found in DB.", exception.Message);
        }

        [Fact]
        public void GetLatestPaySlip_ValidEmployeeId()
        {
            var result = _paySlipService.GenerateLastPaySlipByEmployeeId(TestFixture.TEST_EMPLOYEE_ID_1);
            Assert.IsType<Payslip>(result);
            Assert.Equal(TestFixture.TEST_EMPLOYEE_ID_1, result.EmployeeId);
            Assert.Equal(TestFixture.TEST_SALARY_1, result.Gross);
            Assert.Equal(TestFixture.TEST_SALARY_1*0.3, result.Tax);
            Assert.Equal(TestFixture.TEST_SALARY_1*0.7, result.NetIncome);

            result = _paySlipService.GenerateLastPaySlipByEmployeeId(TestFixture.TEST_EMPLOYEE_ID_2);
            Assert.IsType<Payslip>(result);
            Assert.Equal(TestFixture.TEST_EMPLOYEE_ID_2, result.EmployeeId);
            Assert.Equal(TestFixture.TEST_SALARY_2, result.Gross);
            Assert.Equal(TestFixture.TEST_SALARY_2*0.4, result.Tax);
            Assert.Equal(TestFixture.TEST_SALARY_2*0.6, result.NetIncome);
        }
    }
}
