using System;

namespace Cw.PayslipService.Dtos
{
    public class PaySlipDto
    {
        public int Id { get; set; }
        public DateTime PayDate { get; set; }
        public double Gross { get; set; }
        public double Tax { get; set; }
        public double NetIncome { get; set; }
        public int EmployeeId { get; set; }
    }
}
