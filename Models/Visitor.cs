using System;
using System.Collections.Generic;

namespace MyFitnessLife.Models;

public partial class Visitor
{
    public decimal Visitorid { get; set; }

    public string? IpAddress { get; set; }

    public string? Useragent { get; set; }

    public string? Referralurl { get; set; }

    public DateTime? Visittime { get; set; }

    public string? Sessionid { get; set; }

    public string? Pagevisited { get; set; }

    public string? Referralsource { get; set; }

    public bool? Hasregistered { get; set; }

    public string? Devicetype { get; set; }
}
