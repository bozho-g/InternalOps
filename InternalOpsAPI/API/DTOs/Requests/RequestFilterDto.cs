namespace API.DTOs.Requests
{
    using API.DTOs.Paging;
    using API.Models.Enums;

    public class RequestFilterDto : PagedFilter
    {
        public string? UserId { get; set; }
        public Status? Status { get; set; }
        public RequestType? Type { get; set; }
        public bool IncludeDeleted { get; set; }
        public DateOnly? CreatedFrom { get; set; }
        public DateOnly? CreatedTo { get; set; }
        public DateOnly? HandledFrom { get; set; }
        public DateOnly? HandledTo { get; set; }
    }
}
