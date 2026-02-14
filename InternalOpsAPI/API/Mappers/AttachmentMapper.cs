namespace API.Mappers
{
    using API.DTOs.Attachments;
    using API.Models;

    using Riok.Mapperly.Abstractions;

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class AttachmentMapper
    {
        public partial AttachmentDto MapToDto(RequestAttachment attachment);

        public partial IQueryable<AttachmentDto> ProjectToDto(IQueryable<RequestAttachment> attachments);
    }
}
