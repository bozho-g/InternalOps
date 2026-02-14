namespace API.DTOs.Dashboards
{
    using API.DTOs.Requests;

    public class ManagerDashboardDto
    {
        public int PendingCount { get; set; }

        public int ApprovedToday { get; set; }

        public Dictionary<string, int> ByType { get; set; } = [];

        public List<RequestDto> PendingRequests { get; set; } = [];
    }
}
