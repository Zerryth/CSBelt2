using System.ComponentModel.DataAnnotations;

namespace CSBelt2.Models
{
    public class RegisterViewModel
    {
        [Required, Display(Name="Name")]
        [RegularExpression(@"^[a-zA-Z\s]*$")]
        [MinLength(3, ErrorMessage="First Name must be at least 3 characters.")]
        public string Name { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$")]
        [MinLength(3, ErrorMessage="Last Name must be at least 3 characters.")]
        public string Alias { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Range(0,0, ErrorMessage="A user account already exists with this email. Please enter a unique email or login if you have an account.")]
        public int UniqueEmail { get; set; }

        [Required, DataType(DataType.Password)]
        // [MinLength(8, ErrorMessage="Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage="Passwords must have at least 1 Letter, 1 Number, and 1 Special Character")]
        public string Password { get; set; }
        
        [Required, Display(Name="Confirm Password"), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }

        public User CreateUser(Belt2Context _context)
        {
            User newUser = new User
            {
                Name = this.Name,
                Alias = this.Alias,
                Email = this.Email,
                Password = this.Password   
            };

            _context.users.Add(newUser);
            _context.SaveChanges(); 

            return newUser;
        }
    }
}