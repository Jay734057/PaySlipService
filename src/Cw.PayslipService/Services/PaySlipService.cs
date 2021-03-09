using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cw.PayslipService.Services
{
    public class PaySlipService : IPaySlipService
    {
        private readonly ServiceContext _context;

        public PaySlipService(ServiceContext context)
        {
            _context = context;
        }

        public IEnumerable<Payslip> GetPaySlipsByEmployeeId(int employeeId)
        {
            var employee = _context.Employees.Include(e => e.Payslips).AsNoTracking().SingleOrDefault(e => e.Id == employeeId);

            if(employee == null)
                throw new KeyNotFoundException($"Employee with id: {employeeId} is not found in DB.");

            return employee.Payslips;
        }

        public Payslip GenerateLastPaySlipByEmployeeId(int employeeId)
        {
            var employee = _context.Employees.AsNoTracking().SingleOrDefault(e => e.Id == employeeId);

            if(employee == null)
                throw new KeyNotFoundException($"Employee with id: {employeeId} is not found in DB.");

            var newPaySlip = new Payslip
            {
                PayDate = DateTime.UtcNow.Date,
                EmployeeId = employeeId,
                Gross = employee.Salary,
                Tax = employee.Salary > 150000 ? employee.Salary*0.4 : employee.Salary*0.3
            };

            newPaySlip.NetIncome = newPaySlip.Gross - newPaySlip.Tax;

            _context.Add(newPaySlip);
            _context.SaveChanges();
            
            return newPaySlip;
        }
    }
}
