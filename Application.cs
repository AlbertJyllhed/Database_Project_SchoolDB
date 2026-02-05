namespace Database_Project_SchoolDB
{
    internal static class Application
    {
        private static List<Menu> _menus = new List<Menu>();
        private static int _activeMenuIndex = 0;
        private static int _previousMenuIndex = 0;
        private static bool _running = true;

        private enum MenuIndices
        {
            MainMenu = 0,
            EFMenu = 1,
            ADOMenu = 2
        }

        // Launches the application and starts the main program loop
        internal static void Launch()
        {
            SetupMenus();
            _activeMenuIndex = 0;

            while (_running)
            {
                _menus[_activeMenuIndex].DisplayMenu();
            }
        }

        // Creates all necessary menus and adds them to the menu list
        private static void SetupMenus()
        {
            _menus.AddRange([CreateMainMenu(), CreateEFMenu(), CreateADOMenu()]);
        }

        // Creates the main menu with its items and actions
        private static Menu CreateMainMenu()
        {
            var mainMenu = new Menu("Main Menu");

            mainMenu.AddMenuItem(new MenuItem("Entity Framework Operations", () =>
            {
                GoToMenu((int)MenuIndices.EFMenu);
            }, 
            requiresConfirmation: false));

            mainMenu.AddMenuItem(new MenuItem("ADO.NET Operations", () =>
            {
                GoToMenu((int)MenuIndices.ADOMenu);
            },
            requiresConfirmation: false));

            // Exiting the application
            mainMenu.AddMenuItem(new MenuItem("Exit", () =>
            {
                _running = false;
            }, 
            requiresConfirmation: false));

            return mainMenu;
        }

        // Creates the Entity Framework menu with its items and actions
        private static Menu CreateEFMenu()
        {
            var efMenu = new Menu("Entity Framework Menu");

            efMenu.AddMenuItem(new MenuItem("Fetch Students",
                EFManager.GetAllStudents));
            efMenu.AddMenuItem(new MenuItem("Fetch Teachers by Department",
                EFManager.GetTeachersByDepartment));
            efMenu.AddMenuItem(new MenuItem("Fetch Active Courses",
                EFManager.GetActiveCourses));

            // Returning to previous menu
            efMenu.AddMenuItem(new MenuItem("Back", () =>
            {
                GoToMenu(_previousMenuIndex);
            }, 
            requiresConfirmation: false));

            return efMenu;
        }

        // Creates the ADO.NET menu with its items and actions
        private static Menu CreateADOMenu()
        {
            var adoMenu = new Menu("ADO.NET Menu");

            adoMenu.AddMenuItem(new MenuItem("Get Employees", 
                ADOManager.GetEmployees));

            adoMenu.AddMenuItem(new MenuItem("Add New Employee", () =>
            {
                Utils.InputString("Please enter the new employee's name: ", out string name);
                string[] nameParts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                ADOManager.AddNewEmployee(nameParts[0], nameParts[1], 20000.00m);
            }));

            adoMenu.AddMenuItem(new MenuItem("Get Student Grades", () =>
            {
                int studentId = ADOManager.FindStudent();
                ADOManager.GetStudentGrades(studentId);
            }));
            adoMenu.AddMenuItem(new MenuItem("Get Salary Per Department", 
                ADOManager.GetSalaryPerDepartment));
            adoMenu.AddMenuItem(new MenuItem("Get Median Salary Per Department", 
                ADOManager.GetMedianSalaryPerDepartment));

            // Returning to previous menu
            adoMenu.AddMenuItem(new MenuItem("Back", () =>
            {
                GoToMenu(_previousMenuIndex);
            },
            requiresConfirmation: false));

            return adoMenu;
        }

        // Navigates to the specified menu index and displays it
        private static void GoToMenu(int index)
        {
            _previousMenuIndex = _activeMenuIndex;
            _activeMenuIndex = index;
            _menus[_activeMenuIndex].DisplayMenu();
        }
    }
}
