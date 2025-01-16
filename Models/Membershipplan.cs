using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Membershipplan
{
    public decimal Planid { get; set; }

    public string Planname { get; set; } = null!;

    public decimal Durationinmonths { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Subscriptions> Subscriptions { get; set; } = new List<Subscriptions>();
    //public virtual ICollection<User> TrainerID { get; set; } = new List<User>();

}
