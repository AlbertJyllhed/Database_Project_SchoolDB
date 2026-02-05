using System;
using System.Collections.Generic;

namespace Database_Project_SchoolDB.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfHire { get; set; }

    public decimal? Salary { get; set; }

    public int? EmployeeTypeId { get; set; }

    public int? DepartmentId { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department? Department { get; set; }

    public virtual EmployeeType? EmployeeType { get; set; }
}
