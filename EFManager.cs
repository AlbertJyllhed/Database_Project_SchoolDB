using Database_Project_SchoolDB.Models;

namespace Database_Project_SchoolDB
{
    internal static class EFManager
    {
        internal static void GetTeachersByDepartment()
        {
            using (var context = new SchoolDbContext())
            {
                var teachersByDept = from e in context.Employees
                                     join d in context.Departments
                                     on e.DepartmentId equals d.Id
                                     join et in context.EmployeeTypes
                                     on e.EmployeeTypeId equals et.Id
                                     where et.TypeName == "Teacher"
                                     group e by d.DepartmentName into g
                                     select new
                                     {
                                         Department = g.Key,
                                         Count = g.Count(),
                                     };

                foreach (var item in teachersByDept)
                {
                    Console.WriteLine($"The {item.Department} department has {item.Count} teachers.");
                }
            }
        }

        internal static void GetAllStudents()
        {
            // Implementation for retrieving all students from the database
            Console.WriteLine("Getting Students...");
        }

        internal static void GetActiveCourses()
        {
            // Implementation for retrieving active courses from the database
            Console.WriteLine("Getting Active Courses...");
        }
    }
}
