using Microsoft.EntityFrameworkCore;

namespace MyFitnessLife.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Feedback> Feedbacks { get; set; } = new List<Feedback>(); // Initialize with empty list
        public IEnumerable<Membershipplan> MembershipPlans { get; set; } = new List<Membershipplan>(); // Initialize with empty list

    }
}


