using System.Collections.Generic;

namespace Cw.PayslipService.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }

        public virtual ICollection<Payslip> Payslips { get; private set; }

        public Employee()
        {
            Payslips = new List<Payslip>();
        }
    }
}
