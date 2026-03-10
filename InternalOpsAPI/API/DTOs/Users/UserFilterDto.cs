namespace API.DTOs.Users
{
    using API.DTOs.Paging;

    public class UserFilterDto : PagedFilter
    {
        public string? Role { get; set; }
        public bool Desc { get; set; }
    }
}
