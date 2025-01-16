using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Invoice
{
    public decimal Invoiceid { get; set; }

    public decimal Subscriptionid { get; set; }

    public decimal Userid { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Invoicedate { get; set; }

    public string? Pdfpath { get; set; }

    public virtual Subscriptions Subscription { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
