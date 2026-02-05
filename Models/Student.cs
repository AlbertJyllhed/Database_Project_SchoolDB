using System;
using System.Collections.Generic;

namespace Database_Project_SchoolDB.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PersonalNumber { get; set; }

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
