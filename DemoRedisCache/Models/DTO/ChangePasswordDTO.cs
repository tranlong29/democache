﻿using System.ComponentModel.DataAnnotations;

namespace DemoRedisCache.Models.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? CurrentPassword { get; set; }
        [Required]
        public string? NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string? ConfirmNewPassword { get; set; }
    }
}
