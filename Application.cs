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
            EFMenu = 1
        }

        // Launches the application and starts the main program loop
        internal static void Launch()
        {
            SetupMenus();
            _activeMenuIndex = 0;

            while (_running)
            {
                GoToMenu(_activeMenuIndex);
            }
        }

        private static void SetupMenus()
        {
            _menus.AddRange([CreateMainMenu(), CreateEFMenu()]);
        }

        // Creates the main menu with its items and actions
        private static Menu CreateMainMenu()
        {
            var mainMenu = new Menu("Main Menu");

            mainMenu.AddMenuItem(new MenuItem("Entity Framework Operations", () =>
            {
                GoToMenu((int)MenuIndices.EFMenu);
            }, requiresConfirmation: false));

            // Exiting the application
            mainMenu.AddMenuItem(new MenuItem("Exit", () =>
            {
                _running = false;
            }, requiresConfirmation: false));

            return mainMenu;
        }

        // Creates the Entity Framework menu with its items and actions
        private static Menu CreateEFMenu()
        {
            var efMenu = new Menu("Entity Framework Menu");

            efMenu.AddMenuItem(new MenuItem("Fetch Students",
                DBManager.GetAllStudents));
            efMenu.AddMenuItem(new MenuItem("Fetch Teachers by Department",
                DBManager.GetTeachersByDepartment));
            efMenu.AddMenuItem(new MenuItem("Fetch Active Courses",
                DBManager.GetActiveCourses));

            // Returning to previous menu
            efMenu.AddMenuItem(new MenuItem("Back", () =>
            {
                GoToMenu(_previousMenuIndex);
            }, requiresConfirmation: false));

            return efMenu;
        }

        // Navigates to the specified menu index and displays it
        private static void GoToMenu(int index)
        {
            _previousMenuIndex = _activeMenuIndex;
            _activeMenuIndex = index;
            _menus[index].DisplayMenu();
        }
    }
}
