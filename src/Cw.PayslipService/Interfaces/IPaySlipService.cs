using Cw.PayslipService.Models;
using System.Collections.Generic;

namespace Cw.PayslipService.Interfaces
{
    public interface IPaySlipService
    {
        public IEnumerable<Payslip> GetPaySlipsByEmployeeId(int employeeId);
        public Payslip GenerateLastPaySlipByEmployeeId(int employeeId);
    }
}
