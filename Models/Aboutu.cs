using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFitnessLife.Models;

public partial class Aboutu
{
    public decimal Aboutid { get; set; }

    public string? Title { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Image { get; set; }


    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }
}
