namespace API.Mappers
{
    using API.DTOs;
    using API.Models;

    using Riok.Mapperly.Abstractions;

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class UserMapper
    {
        public partial UserDto ToUserDto(User user);

        public UserDto? MapToDtoNullable(User? user) => user != null ? ToUserDto(user) : null;

        private static string MapEmail(string? email) => email ?? string.Empty;
    }
}
