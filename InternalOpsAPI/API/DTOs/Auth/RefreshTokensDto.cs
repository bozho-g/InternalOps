namespace API.DTOs.Auth
{
    using API.Models;

    public class RefreshTokensDto
    {
        public required string TokenRaw { get; set; }
        public required RefreshToken RefreshToken { get; set; }
    }
}
