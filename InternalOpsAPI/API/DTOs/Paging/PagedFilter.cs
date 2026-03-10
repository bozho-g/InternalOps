namespace API.DTOs.Paging
{
    public abstract class PagedFilter
    {
        private const int MaxPageSize = 50;

        public int PageNumber
        {
            get;
            set
            {
                if (value < 1)
                    value = 1;
                field = value;
            }
        } = 1;

        public int PageSize
        {
            get;
            set
            {
                field = Math.Clamp(value, 1, MaxPageSize);
            }
        } = 10;

        public string? Search { get; set; }
    }
}
