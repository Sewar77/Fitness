﻿namespace MyFitnessLife.Models
{
    public class TrainerWithUserandSubsecribtions
    {
        public int Userid { get; set; }
        public string USERNAME { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }

        public int SubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int MembershipPlanId { get; set; }
        
    }
}
