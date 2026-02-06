namespace Database_Project_SchoolDB
{
    public static class Utils
    {
        public static void InputString(string prompt, out string result)
        {
            Console.Write(prompt);
            result = Console.ReadLine() ?? string.Empty;
            Console.Clear();
        }

        public static void InputInt(string prompt, out int result)
        {
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Invalid input. Please enter a valid integer: ");
            }
            Console.Clear();
        }

        public static void InputDecimal(string prompt, out decimal result)
        {
            Console.Write(prompt);
            while (!decimal.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Invalid input. Please enter a valid decimal number: ");
            }
            Console.Clear();
        }

        public static string CenterString(string s, int width)
        {
            if (s.Length >= width)
            {
                return s;
            }

            int leftPadding = (width - s.Length) / 2;
            int rightPadding = (width - s.Length - leftPadding);

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }

        public static void DisplayTable(string[] table)
        {
            string tableBox = string.Empty;
            foreach (string s in table)
            {
                int stringWidth = s.Length + 4; // Add padding for spaces
                tableBox += $"|{CenterString(s, stringWidth)}|";
            }
            string line = new string('-', tableBox.Length);
            Console.WriteLine($"{line}\n{tableBox}\n{line}");
        }
    }
}
