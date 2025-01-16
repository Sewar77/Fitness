using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Visitor
{
    public decimal Visitorid { get; set; }

    public string? IpAddress { get; set; }

    public DateTime? Visittime { get; set; }

    public string? Sessionid { get; set; }

    public bool? Hasregistered { get; set; }
}
