using Database_Project_SchoolDB.Models;
using Microsoft.EntityFrameworkCore;

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
                    string table = $"|{Utils.CenterString(student.Name, 40)}|";

                    table += $"|{Utils.CenterString(student.PersonalNumber, 20)}|";

                    table += $"|{Utils.CenterString(student.ClassName, 6)}|";

                    string line = new string('-', table.Length);

                    Console.WriteLine($"{line}\n{table}\n{line}");
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
                    string table = string.Format("|{0,-30}|{1,-6}|",
                        Utils.CenterString(item.Department, 30),
                        Utils.CenterString(item.Count.ToString(), 6));

                    string line = new string('-', table.Length);

                    Console.WriteLine($"{line}\n{table}\n{line}");
                }
            }
        }

        // Displays all subjects that have students actively studying them
        internal static void GetActiveCourses()
        {
            using (var context = new SchoolDbContext())
            {
                var activeCourses =
                    from s in context.Subjects
                    where s.Students.Count() > 0
                    select new
                    {
                        CourseName = s.SubjectName,
                        Students = s.Students.Count()
                    };

                foreach (var course in activeCourses)
                {
                    string studentString = course.Students == 1 ? "student" : "students";
                    Console.WriteLine($"{course.Students} {studentString} studying the {course.CourseName} course.");
                }
            }
        }
    }
}
