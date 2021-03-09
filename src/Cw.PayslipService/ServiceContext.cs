using Cw.PayslipService.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw.PayslipService
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Payslip> Payslips { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payslip>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Payslips)
                .IsRequired()
                .HasForeignKey(p => p.EmployeeId);
        }
    }
}
