namespace API.DTOs.Dashboards
{
    public class AdminDashboardDto
    {
        public int TotalRequests { get; set; }

        public int TotalUsers { get; set; }

        public int DeletedRequests { get; set; }

        public Dictionary<string, int> ByStatus { get; set; } = [];

        public Dictionary<string, int> ByType { get; set; } = [];
    }
}
