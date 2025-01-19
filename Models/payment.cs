namespace MyFitnessLife.Models
{
    public class payment
    {
        public virtual Bank? Bank { get; set; }
        public virtual User? user { get; set; }
        public virtual Membershipplan? memberplan { get; set; }
        public virtual Subscriptions? subscriptions { get; set; }
        public virtual Workout? workout { get; set; }

       
    }
}
