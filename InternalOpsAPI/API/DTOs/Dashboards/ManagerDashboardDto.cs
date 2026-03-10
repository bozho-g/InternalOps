namespace API.DTOs.Dashboards
{
    public class ManagerDashboardDto
    {
        public int PendingCount { get; set; }

        public int ApprovedToday { get; set; }

        public Dictionary<string, int> ByType { get; set; } = [];
    }
}
