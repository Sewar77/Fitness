using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFitnessLife.Models;

public partial class Websiteinfo
{
    public decimal Websitetid { get; set; }

    public string? Title1 { get; set; }

    public string? Title2 { get; set; }

    public string? Openhour { get; set; }

    public string? Address { get; set; }

    public string? Image { get; set; }

    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }
}
