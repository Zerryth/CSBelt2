using System.ComponentModel.DataAnnotations;

namespace CSBelt2.Models
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        [Display(Name="Email")]
        public string LoginEmail { get; set; }

        [Range(0,0, ErrorMessage="Email input not found in database.")]
        public int EmailInDb { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword { get; set; }

        [Range(0,0, ErrorMessage="Password does not match password in database.")]
        public int LoginPasswordConfirmation { get; set; }
    }
}