using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFitnessLife.Models;

public partial class Feedback
{
    public decimal Feedbackid { get; set; }

    public decimal Userid { get; set; }

    public string Feedbacktext { get; set; } = null!;

    public DateTime? Submittedat { get; set; }

    public bool? Approved { get; set; }

    [NotMapped]
    public string? Username { get; set; }

    public virtual User User { get; set; } = null!;
}
