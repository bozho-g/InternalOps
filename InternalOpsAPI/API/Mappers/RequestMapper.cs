namespace API.Mappers
{
    using API.DTOs;
    using API.DTOs.Attachments;
    using API.DTOs.Comments;
    using API.DTOs.Requests;
    using API.Models;

    using Riok.Mapperly.Abstractions;

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class RequestMapper(CommentMapper commentMapper, UserMapper userMapper, AttachmentMapper attachmentMapper)
    {
        public partial RequestDto MapToDto(Request request);

        public partial RequestDetailDto MapToDetailDto(Request request);

        public partial IQueryable<RequestDto> ProjectToDto(IQueryable<Request> requests);

        private UserDto MapRequestedBy(User user) => userMapper.ToUserDto(user);

        private UserDto? MapHandledBy(User? user) => userMapper.MapToDtoNullable(user);

        private CommentDto MapComment(RequestComment comment) => commentMapper.MapToDto(comment);

        private AttachmentDto MapAttachment(RequestAttachment attachment) => attachmentMapper.MapToDto(attachment);
    }
}
