using AutoMapper;
using Cw.PayslipService.Dtos;
using Cw.PayslipService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw.PayslipService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeDto, Employee>(); 
            CreateMap<Payslip, PaySlipDto>(); 
        }
    }
}
