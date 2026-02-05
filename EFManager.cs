using Database_Project_SchoolDB.Models;

namespace Database_Project_SchoolDB
{
    internal static class EFManager
    {
        // Displays all students and which class they're in
        internal static void GetAllStudents()
        {
            using (var context = new SchoolDbContext())
            {
                var students =
                    from s in context.Students
                    join c in context.Classes on s.ClassId equals c.Id
                    select new
                    {
                        Name = s.FirstName + " " + s.LastName,
                        s.PersonalNumber,
                        c.ClassName
                    };

                foreach (var student in students)
                {
                    Utils.DisplayTable([
                        student.Name,
                        student.PersonalNumber,
                        student.ClassName
                    ]);
                }
            }
        }

        // Displays all departments and the number of teachers in them
        internal static void GetTeachersByDepartment()
        {
            using (var context = new SchoolDbContext())
            {
                var teachersByDept =
                    from e in context.Employees
                    join d in context.Departments on e.DepartmentId equals d.Id
                    join et in context.EmployeeTypes on e.EmployeeTypeId equals et.Id
                    where et.TypeName == "Teacher"
                    group e by d.DepartmentName into g
                    select new
                    {
                        Department = g.Key,
                        Count = g.Count(),
                    };

                //var departmentName = context.Departments.EntityType.GetTableName();
                //var tableNames = context.Model.GetEntityTypes();
                //foreach (var table in tableNames)
                //{
                //    Console.WriteLine(table.GetTableName());
                //}

                foreach (var item in teachersByDept)
                {
                    Utils.DisplayTable([
                        item.Department,
                        item.Count.ToString()
                    ]);
                }
            }
        }

        // Displays all subjects that have students actively studying them
        internal static void GetActiveCourses()
        {
            using (var context = new SchoolDbContext())
            {
                var activeCourses =
                    from c in context.Courses
                    where c.StartDate < DateOnly.FromDateTime(DateTime.Today) &&
                    c.EndDate > DateOnly.FromDateTime(DateTime.Today)
                    select new
                    {
                        CourseName = c.Subject.SubjectName,
                        c.StartDate,
                        c.EndDate
                    };

                foreach (var course in activeCourses)
                {
                    Utils.DisplayTable([
                        course.CourseName,
                        course.StartDate.ToString(),
                        course.EndDate.ToString()
                    ]);
                }
            }
        }
    }
}
