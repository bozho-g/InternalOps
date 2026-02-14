namespace API.DTOs.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordDto
    {
        [Required]
        public required string CurrentPassword { get; set; }

        [Required]
        public required string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
