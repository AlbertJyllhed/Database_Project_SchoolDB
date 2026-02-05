using System;
using System.Collections.Generic;

namespace Database_Project_SchoolDB.Models;

public partial class Grade
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int TeacherId { get; set; }

    public string Score { get; set; } = null!;

    public DateOnly GradingDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Employee Teacher { get; set; } = null!;
}
