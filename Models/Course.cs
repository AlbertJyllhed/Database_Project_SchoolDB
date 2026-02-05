using System;
using System.Collections.Generic;

namespace Database_Project_SchoolDB.Models;

public partial class Course
{
    public int Id { get; set; }

    public int? SubjectId { get; set; }

    public int? TeacherId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Employee? Teacher { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
