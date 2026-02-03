using Microsoft.Data.SqlClient;

namespace Database_Project_SchoolDB
{
    internal static class ADOManager
    {
        private static readonly string _connectionString = "Server = localhost;" +
            "Database = SchoolDB;" +
            "Integrated Security = True;" +
            "Trust Server Certificate = True;";

        internal static void GetEmployees()
        {
            string query = "SELECT * FROM Employees";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (var reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine($"{reader.GetName(i)}: {reader.GetValue(i)}");
                            }
                            Console.WriteLine("----------------------------------------");
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

        internal static void AddNewEmployee()
        {
            string query = "INSERT INTO Employees (FirstName, LastName, Salary, EmployeeTypeId, DepartmentId) " +
                "VALUES (@FirstName, @LastName, @Email, @Age)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    //command.Parameters.AddWithValue("@FirstName", firstName);

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
            Console.WriteLine("Getting Student Grades...");
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
