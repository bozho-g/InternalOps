namespace API.DTOs.Dashboards
{
    public class UserDashboardDto
    {
        public int MyPending { get; set; }

        public int MyApproved { get; set; }

        public int MyRejected { get; set; }

        public int MyCompleted { get; set; }
    }
}
