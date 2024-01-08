namespace Sistema_Inventario_Manitos_Maravillosas.Filters
{
    public interface IFileLogger
    {
        void LogError(Exception ex);
    }

    public class FileLogger : IFileLogger
    {
        private readonly string _logDirectoryPath;

        public FileLogger()
        {
            // Define the log file path. You can change this path as needed.
            _logDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }

        public void LogError(Exception ex)
        {
            try
            {
                // Prepare the message to log
                string messageToLog = $"Timestamp: {DateTime.Now}\n" +
                                      $"Message: {ex.Message}\n" +
                                      $"InnerException: {ex.InnerException}\n" +
                                      $"StackTrace: {ex.StackTrace}\n" +
                                      $"---------------------------------------------\n";

                // Check if directory exists, if not, create it
                if (!Directory.Exists(_logDirectoryPath))
                {
                    Directory.CreateDirectory(_logDirectoryPath);
                }
                string logFilePath = Path.Combine(_logDirectoryPath, $"ErrorLog_{DateTime.Now:yyyyMMdd}.txt");

                // Append the message to the log file
                File.AppendAllText(logFilePath, messageToLog);
            }
            catch (Exception fileEx)
            {
                // Handle any exceptions that occur during logging
                // For example, you can output to console for debugging
                // Note: In production, you might want to handle this differently
                Console.WriteLine($"An error occurred while logging: {fileEx.Message}");
            }
        }
    }

}
