﻿using System.ComponentModel.DataAnnotations;

namespace Cw.PayslipService.Dtos
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
