using System;
using System.ComponentModel.DataAnnotations;
namespace webad_source.Models.Accounts
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, ErrorMessage = "Password must be at least 6 charachters long")]
        [Display(Name = "Password")]
        public string Password {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password did not match")]
        public string ConfirmPassword {get; set;}

    }
}
