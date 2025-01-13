using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Trainerassignment
{
    public decimal Assignmentid { get; set; }

    public decimal Trainerid { get; set; }

    public decimal Memberid { get; set; }

    public DateTime? Assignedat { get; set; }

    public virtual User Member { get; set; } = null!;

    public virtual User Trainer { get; set; } = null!;
}
