using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Subscriptions
{
    public decimal Subscriptionid { get; set; }

    public decimal Userid { get; set; }

    public decimal Planid { get; set; }

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public string? Paymentstatus { get; set; }

    public decimal? Amount { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Membershipplan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
