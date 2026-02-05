using System;
using System.Collections.Generic;

namespace Database_Project_SchoolDB.Models;

public partial class EmployeeType
{
    public int Id { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
