namespace API.Mappers
{
    using API.DTOs;
    using API.DTOs.Comments;
    using API.Models;

    using Riok.Mapperly.Abstractions;

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class CommentMapper(UserMapper userMapper)
    {
        public partial CommentDto MapToDto(RequestComment comment);

        public partial IQueryable<CommentDto> ProjectToDto(IQueryable<RequestComment> comments);

        private UserDto MapUser(User user) => userMapper.ToUserDto(user);
    }
}
