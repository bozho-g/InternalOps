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
        [MapProperty(nameof(Request), nameof(RequestDto.DeletedInfo))]
        public partial RequestDto MapToDto(Request request);

        [MapProperty(nameof(Request), nameof(RequestDto.DeletedInfo))]
        public partial RequestDetailDto MapToDetailDto(Request request);

        public partial IQueryable<RequestDto> ProjectToDto(IQueryable<Request> requests);

        private UserDto MapRequestedBy(User user) => userMapper.ToUserDto(user);

        private UserDto? MapHandledBy(User? user) => userMapper.MapToDtoNullable(user);

        private DeletedDto MapToDeleteDto(Request request) => new()
        {
            IsDeleted = request.IsDeleted,
            DeletedAt = request.DeletedAt,
            DeletedById = request.DeletedById,
            DeletedBy = userMapper.MapToDtoNullable(request.DeletedBy)
        };

        private CommentDto MapComment(RequestComment comment) => commentMapper.MapToDto(comment);

        private AttachmentDto MapAttachment(RequestAttachment attachment) => attachmentMapper.MapToDto(attachment);
    }
}
