namespace API.DTOs.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Length(6, 100, ErrorMessage = "Password must be between 6 and 100 characters long")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
