using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Workout
{
    public decimal Workoutid { get; set; }

    public decimal Trainerid { get; set; }

    public decimal Memberid { get; set; }

    public string? Goals { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual User Member { get; set; } = null!;

    public virtual User Trainer { get; set; } = null!;
}
