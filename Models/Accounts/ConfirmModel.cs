﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webad_source.Models.Accounts
{
    public class ConfirmModel
    {
        [Required]
        [Display(Name ="Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Code is required")]
        public string Code { get; set; }

    }
}