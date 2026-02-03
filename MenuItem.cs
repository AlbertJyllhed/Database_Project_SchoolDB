namespace Database_Project_SchoolDB
{
    internal class MenuItem
    {
        public string Name { get; set; }
        public Action Action { get; set; }
        public bool RequiresConfirmation { get; set; }

        public MenuItem(string name, Action action, bool requiresConfirmation = true)
        {
            Name = name;
            Action = action;
            RequiresConfirmation = requiresConfirmation;
        }
    }
}
