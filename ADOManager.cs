using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
                "SELECT FirstName, LastName, DepartmentName, DateOfHire " +
                "FROM Employees e " +
                "JOIN EmployeeTypes et ON e.EmployeeTypeId = et.Id " +
                "JOIN Departments d ON e.DepartmentId = d.Id " +
                "WHERE et.TypeName = 'Teacher'";

            using (SqlConnection connection = new SqlConnection(_connectionString))
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

        // Adds a new employee to the database
        internal static void AddNewEmployee(string firstName, string lastName, decimal salary)
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

        // Finds a student by their first name, last name, or personal number and returns their ID
        internal static int FindStudent()
        {
            Utils.InputString("Please enter search term: ", out string student);
            int studentId = 0;

            string query =
                "SELECT Id FROM Students " +
                "WHERE FirstName LIKE @SearchTerm " +
                "OR LastName LIKE @SearchTerm " +
                "OR PersonalNumber LIKE @SearchTerm";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{student}%");

                    try
                    {
                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                studentId = reader.GetInt32(0);
                                Console.WriteLine($"Student found with ID: {studentId}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                connection.Close();
            }

            return studentId;
        }

        internal static void GetStudentGrades(int studentId)
        {
            string query =
                "SELECT * FROM Grades g " +
                "JOIN Students s ON g.StudentId = s.Id " +
                "WHERE s.Id = @StudentId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var command =new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    try
                    {
                        connection.Open();

                        using (var reader = command.ExecuteReader())
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

        internal static void GetSalaryPerDepartment()
        {
            Console.WriteLine("Getting Salary Per Department...");
        }

        internal static void GetMedianSalaryPerDepartment()
        {
            Console.WriteLine("Getting Median Salary Per Department...");
        }
    }
}
