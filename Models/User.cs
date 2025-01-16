using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFitnessLife.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Roleid { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public DateTime? Createdat { get; set; }

    public string Gender { get; set; } = null!;

    public DateTime? Birthdate { get; set; }

    public bool? Isactive { get; set; }

    public string? Imagepath { get; set; }

    public string? Status { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Subscriptions> Subscriptions { get; set; } = new List<Subscriptions>();

    public virtual ICollection<Trainerassignment> TrainerassignmentMembers { get; set; } = new List<Trainerassignment>();

    public virtual ICollection<Trainerassignment> TrainerassignmentTrainers { get; set; } = new List<Trainerassignment>();

    public virtual ICollection<Workout> WorkoutMembers { get; set; } = new List<Workout>();

    public virtual ICollection<Workout> WorkoutTrainers { get; set; } = new List<Workout>();
}
