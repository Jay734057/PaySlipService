using System;
using System.Text.Json.Serialization;

namespace Cw.PayslipService.Models
{
    public class Payslip
    {
        public int Id { get; set; }
        public DateTime PayDate { get; set; }

        public double Gross { get; set; }
        public double Tax { get; set; }
        public double NetIncome { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
