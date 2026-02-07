using Microsoft.Data.SqlClient;

namespace Database_Project_SchoolDB
{
    public enum IdType
    {
        Department,
        EmployeeType,
        Student,
        Course,
        Teacher
    }

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

            ExecuteQuery(ReadTable, query);
        }

        // Adds a new employee to the database
        internal static void AddNewEmployee(string firstName, string? lastName,
            decimal salary, int employeeTypeId, int departmentId)
        {
            string query =
                "INSERT INTO Employees (FirstName, LastName, DateOfHire, " +
                "Salary, EmployeeTypeId, DepartmentId) " +
                "VALUES (@FirstName, @LastName, @DateOfHire, " +
                "@Salary, @EmployeeTypeId, @DepartmentId)";

            ExecuteQuery(UpdateTable, query,
                [
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@LastName", lastName ?? "Unknown"),
                    new SqlParameter("@DateOfHire", DateOnly.FromDateTime(DateTime.Today)),
                    new SqlParameter("@Salary", salary),
                    new SqlParameter("@EmployeeTypeId", employeeTypeId),
                    new SqlParameter("@DepartmentId", departmentId)
                ]);
        }

        // Displays all grades for a student based on a search term
        internal static void GetStudentGrades(string searchTerm)
        {
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

            ExecuteQuery(ReadTable, query,
                [
                    new SqlParameter("@SearchTerm", $"%{searchTerm}%")
                ]);
        }

        // Displays the total salary for each department
        internal static void GetSalaryPerDepartment()
        {
            string query =
                "SELECT " +
                "d.DepartmentName AS [Department], " +
                "SUM(e.Salary) AS [Total Salary] " +
                "FROM Departments d " +
                "JOIN Employees e ON d.Id = e.DepartmentId " +
                "GROUP BY d.DepartmentName";

            ExecuteQuery(ReadTable, query);
        }

        // Displays the average salary for each department
        internal static void GetMedianSalaryPerDepartment()
        {
            string query =
                "SELECT " +
                "d.DepartmentName AS [Department], " +
                "CAST(ROUND(AVG(e.Salary), 2) AS numeric(19, 2)) AS [Average Salary] " +
                "FROM Departments d " +
                "JOIN Employees e ON d.Id = e.DepartmentId " +
                "GROUP BY d.DepartmentName";

            ExecuteQuery(ReadTable, query);
        }

        // Displays detailed information about a student based on their ID
        internal static void GetStudentInfo(int studentId)
        {
            string query = "EXEC dbo.GetStudentInfo @StudentId";

            ExecuteQuery(ReadTable, query,
                [
                    new SqlParameter("@StudentId", studentId)
                ]);
        }

        // Adds a grade for a student in a course taught by a specific teacher
        internal static void SetStudentGrade(int studentId, int courseId, 
            int teacherId, string score)
        {
            string query =
                "INSERT INTO Grades (StudentId, CourseId, TeacherId, Score, GradingDate) " +
                "VALUES (@StudentId, @CourseId, @TeacherId, @Score, CAST(GETDATE() AS Date))";

            ExecuteQuery(MakeTransaction, query,
                [
                    new SqlParameter("@StudentId", studentId),
                    new SqlParameter("@CourseId", courseId),
                    new SqlParameter("@TeacherId", teacherId),
                    new SqlParameter("@Score", score)
                ]);
        }

        // Helper method to execute a query and perform an action with the SqlCommand
        private static void ExecuteQuery(Action<SqlCommand> action,
            string query, SqlParameter[]? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        action(command);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                connection.Close();
            }
        }

        // Helper method to read a SqlCommand's results and display them in a table format
        private static void ReadTable(SqlCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                string[] table = new string[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    table[i] = columnName;
                }

                Utils.DisplayTable(table);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
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
                    Console.WriteLine("No results found.");
                }
            }
        }

        // Helper method to update a table and display the number of affected rows
        private static void UpdateTable(SqlCommand command)
        {
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} rows changed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Helper method to execute a command within a transaction
        private static void MakeTransaction(SqlCommand command)
        {
            using (var transaction = command.Connection.BeginTransaction())
            {
                command.Transaction = transaction;

                try
                {
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Transaction rolled back. Error: {ex.Message}");
                }
            }
        }

        // Helper method to display options from a table and
        // get a valid ID input from the user based on the type of ID needed
        internal static int ChooseID(IdType idType)
        {
            string query = string.Empty;

            switch (idType)
            {
                case IdType.Department:
                    query = "SELECT Id, DepartmentName FROM Departments";
                    break;
                case IdType.EmployeeType:
                    query = "SELECT Id, TypeName FROM EmployeeTypes";
                    break;
                case IdType.Student:
                    query = "SELECT Id, FirstName + ' ' + " +
                        "LastName AS StudentName FROM Students";
                    break;
                case IdType.Course:
                    query = "SELECT Id, SubjectName FROM Subjects";
                    break;
                case IdType.Teacher:
                    query = "SELECT e.Id, " +
                    "e.FirstName + ' ' + e.LastName AS FullName " +
                    "FROM Employees e " +
                    "JOIN EmployeeTypes et ON e.EmployeeTypeId = et.Id " +
                    "WHERE et.TypeName = 'Teacher'";
                    break;
            }

            return GetIdFromTable(query);
        }

        // Helper method to display options from a table and get a valid ID input from the user
        private static int GetIdFromTable(string query)
        {
            List<int> validIds = new List<int>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (var reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string departmentName = reader.GetString(1);

                            Console.WriteLine($"{id}. {departmentName}");

                            validIds.Add(id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();
            }

            Utils.InputInt("Select ID from table: ", out int choice);

            while (!validIds.Contains(choice))
            {
                Console.WriteLine("Invalid ID. Please try again.");
                Utils.InputInt("Select ID from table: ", out choice);
            }

            return choice;
        }
    }
}
