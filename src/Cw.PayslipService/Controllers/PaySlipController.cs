using AutoMapper;
using Cw.PayslipService.Dtos;
using Cw.PayslipService.Interfaces;
using Cw.PayslipService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cw.PayslipService.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class PaySlipController : Controller
    {
        private readonly IPaySlipService _paySlipService;
        private readonly IMapper _mapper;

        public PaySlipController(IPaySlipService paySlipService, IMapper mapper)
        {
            _paySlipService = paySlipService ?? throw new ArgumentNullException(nameof(paySlipService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("allpayslips/{employeeId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public IActionResult GetPaySlipsByEmployeeId(int employeeId)
        {
            try
            {
                var paySlips = _paySlipService.GetPaySlipsByEmployeeId(employeeId);
                var paySlipDtos = _mapper.Map<IEnumerable<Payslip>, IEnumerable<PaySlipDto>>(paySlips);
                return Ok(paySlipDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("latestpayslip/{employeeId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public IActionResult GenerateLastPaySlipsByEmployeeId(int employeeId)
        {
            try
            {
                var paySlip = _paySlipService.GenerateLastPaySlipByEmployeeId(employeeId);
                var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
                return Ok(paySlipDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
