using Microsoft.Data.SqlClient;

namespace Database_Project_SchoolDB
{
    internal static class ADOManager
    {
        private static readonly string _connectionString = "Server = localhost;" +
            "Database = SchoolDB;" +
            "Integrated Security = True;" +
            "Trust Server Certificate = True;";

        // Displays all teachers and which department they work in
        internal static void GetEmployees()
        {
            string query =
                "SELECT " +
                "e.FirstName + ' ' + e.LastName AS [Employee], " +
                "et.TypeName AS [Job Description], " +
                "d.DepartmentName AS [Department], " +
                "DATEDIFF(YEAR, e.DateOfHire, GETDATE()) AS [Years of Service] " +
                "FROM Employees e " +
                "JOIN EmployeeTypes et ON e.EmployeeTypeId = et.Id " +
                "JOIN Departments d ON e.DepartmentId = d.Id";

            ReadTable(query);
        }

        // Adds a new employee to the database
        internal static void AddNewEmployee(string firstName, string? lastName, 
            decimal salary, int employeeTypeId, int departmentId)
        {
            string query =
                "INSERT INTO Employees (FirstName, LastName, Salary, EmployeeTypeId, DepartmentId) " +
                "VALUES (@FirstName, @LastName, @Salary, @EmployeeTypeId, DepartmentId)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@EmployeeTypeId", employeeTypeId);
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} rows changed.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    connection.Close();
                }
            }
        }

        internal static void GetStudentGrades()
        {
            Utils.InputString("Please enter a search term to find student grades " +
                "(first name, last name, or personal number): ", out string searchTerm);

            string query =
                "SELECT " +
                "sub.SubjectName AS [Subject], " +
                "g.Score AS [Grade], " +
                "e.FirstName + ' ' + e.LastName AS [Teacher], " +
                "g.GradingDate AS [Date Graded] " +
                "FROM Grades g " +
                "JOIN Students s ON g.StudentId = s.Id " +
                "JOIN Courses c ON g.CourseId = c.Id " +
                "JOIN Subjects sub ON c.SubjectId = sub.Id " +
                "JOIN Employees e ON g.TeacherId = e.Id " +
                "WHERE s.FirstName LIKE @SearchTerm " +
                "OR s.LastName LIKE @SearchTerm " +
                "OR s.PersonalNumber LIKE @SearchTerm " +
                "ORDER BY g.GradingDate DESC";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var command =new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    try
                    {
                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string[] table = new string[reader.FieldCount];

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string column = reader.GetValue(i).ToString() ?? "Unknown";
                                        table[i] = column;
                                    }

                                    Utils.DisplayTable(table);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Student has not yet been graded.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    connection.Close();
                }
            }
        }

        internal static void GetSalaryPerDepartment()
        {
            string query =
                "SELECT " +
                "d.DepartmentName AS [Department], " +
                "SUM(e.Salary) AS [Total Salary] " +
                "FROM Departments d " +
                "JOIN Employees e ON d.Id = e.DepartmentId " +
                "GROUP BY d.DepartmentName";

            ReadTable(query);
        }

        internal static void GetMedianSalaryPerDepartment()
        {
            string query =
                "SELECT " +
                "d.DepartmentName AS [Department], " +
                "CAST(ROUND(AVG(e.Salary), 2) AS numeric(19, 2)) AS [Average Salary] " +
                "FROM Departments d " +
                "JOIN Employees e ON d.Id = e.DepartmentId " +
                "GROUP BY d.DepartmentName";

            ReadTable(query);
        }

        private static void ReadTable(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (var reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] table = new string[reader.FieldCount];

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string column = reader.GetValue(i).ToString() ?? "Unknown";
                                table[i] = column;
                            }

                            Utils.DisplayTable(table);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();
            }
        }
    }
}
