using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;

namespace Cw.PayslipService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ServiceContext _context;

        public EmployeeService(ServiceContext context)
        {
            _context = context;
        }

        public int SubmitEmployee(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();

            return employee.Id;
        }
    }
}
