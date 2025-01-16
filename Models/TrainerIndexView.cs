namespace MyFitnessLife.Models
{
    public class TrainerIndexViewModel
    {
        public decimal TrainerId { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Workout> Workouts { get; set; }
        public IEnumerable<Membershipplan> MembershipPlans { get; set; }
        public IEnumerable<Trainerassignment> TrainerAssignments { get; set; }
        public IEnumerable<dynamic> TrainerPlans { get; set; } // Holds PlanId and PlanName
        public IEnumerable<dynamic> Members { get; set; } // Holds Member info
    }

}
