namespace Database_Project_SchoolDB
{
    public static class Utils
    {
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
    }
}
