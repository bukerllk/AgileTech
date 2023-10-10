﻿
using System.ComponentModel.DataAnnotations;

namespace AgileTech_Web.Models.Dto
{
    public class ClientCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
