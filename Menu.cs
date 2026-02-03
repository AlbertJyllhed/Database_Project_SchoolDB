namespace Database_Project_SchoolDB
{
    internal class Menu
    {
        private string Name { get; set; }
        private List<MenuItem> Items { get; set; }

        public Menu(string name)
        {
            Name = name;
            Items = new List<MenuItem>();
        }

        // Adds a new menu item to the menu
        public void AddMenuItem(MenuItem item)
        {
            Items.Add(item);
        }

        // Displays the menu items to the console
        public void DisplayMenu()
        {
            Console.WriteLine($"--- {Name} ---\n");
            int counter = 1;
            foreach (var item in Items)
            {
                Console.WriteLine($"{counter}. {item.Name}");
                counter++;
            }

            SelectMenuItem();
        }

        // Gets user input to select a menu item
        private void SelectMenuItem()
        {
            Console.Write("Select an option by number: ");
            string? input = Console.ReadLine();
            ResetMenu(waitForConfirmation: false);
            if (int.TryParse(input, out int choice))
            {
                ExecuteMenuItem(choice - 1);
            }
            else
            {
                DisplayErrorMessage("Invalid input. Please enter a number.");
            }
        }

        // Executes the action associated with the selected menu item
        private void ExecuteMenuItem(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                Items[index].Action();
                ResetMenu(Items[index].RequiresConfirmation);
            }
            else
            {
                DisplayErrorMessage("Invalid menu item index.");
            }
        }

        // Displays an error message to the console
        private void DisplayErrorMessage(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            ResetMenu();
        }

        // Resets the menu display after an action is executed
        private void ResetMenu(bool waitForConfirmation = true)
        {
            if (waitForConfirmation)
            {
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }

            Console.Clear(); // Clears the console
            Console.WriteLine("\x1b[3J"); // Clears the scrollback buffer
            Console.Clear(); // Clears cursor artifacts created by command above
        }
    }
}
