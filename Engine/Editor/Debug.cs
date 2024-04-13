namespace WoopWoop
{
    /// <summary>
    /// Provides methods for logging debug messages with different colors.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Writes an error message to the console in red color.
        /// </summary>
        /// <param name="msg">The error message to write.</param>
        public static void WriteError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a warning message to the console in yellow color.
        /// </summary>
        /// <param name="msg">The warning message to write.</param>
        public static void WriteWarning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
