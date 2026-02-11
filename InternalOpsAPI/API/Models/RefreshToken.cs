namespace API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RefreshTokens")]
    public class RefreshToken
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public string TokenHash { get; set; } = null!;

        [Required]
        public DateTime Expires { get; set; }

        [Required]
        public bool IsRevoked { get; set; }

        [Required]
        public required string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
