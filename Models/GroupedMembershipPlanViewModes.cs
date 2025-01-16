namespace MyFitnessLife.Models
{
    public class GroupedMembershipPlanViewModes
    {
        public int Planid { get; set; }
        public string Planname { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal Durationinmonths { get; set; } 
        public List<TrainerWithUserandSubsecribtions> Subscriptions { get; set; }

    }
}
