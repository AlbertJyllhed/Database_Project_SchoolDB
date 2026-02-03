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
                    string table = string.Format("|{0,-40}|{1,-20}|{2,-5}|", 
                        Utils.CenterString(student.Name, 40), 
                        Utils.CenterString(student.PersonalNumber, 20), 
                        Utils.CenterString(student.ClassName, 5));

                    string bottom = new string('-', table.Length);

                    Console.WriteLine($"{table}\n{bottom}");
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

                foreach (var item in teachersByDept)
                {
                    string teacherString = item.Count == 1 ? "teacher" : "teachers";
                    Console.WriteLine($"The {item.Department} department has {item.Count} {teacherString}.");
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
