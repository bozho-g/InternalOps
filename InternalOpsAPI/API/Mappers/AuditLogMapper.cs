namespace API.Mappers
{
    using API.DTOs;
    using API.DTOs.AuditLogs;
    using API.Models;

    using Riok.Mapperly.Abstractions;

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class AuditLogMapper(UserMapper userMapper)
    {
        public partial AuditLogDto MapToDto(AuditLog log);

        public partial IQueryable<AuditLogDto> ProjectToDto(IQueryable<AuditLog> logs);

        private UserDto? MapChangedBy(User? user) => userMapper.MapToDtoNullable(user);
    }
}
