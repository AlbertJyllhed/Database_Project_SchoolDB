using System;
using System.Collections.Generic;

namespace Database_Project_SchoolDB.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string? SubjectName { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
