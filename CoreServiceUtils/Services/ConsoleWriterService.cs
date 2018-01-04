using System;
using CoreServiceUtils.Interfaces;

namespace CoreServiceUtils.Services
{
    public class ConsoleWriterService : BaseService, IHostWriter
    {
        

        public ConsoleWriterService() 
            : base(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString())
        {
            
        }

        public void Write(string msg, bool writeToLog = true)
        {
            if (writeToLog)
            {
                Log.Info(msg);
            }

            ConsoleColor originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(msg);

            Console.ForegroundColor = originalColor;
        }

        public void Warn(string msg, bool writeToLog = true)
        {
            if (writeToLog)
            {
                Log.Warn(msg);
            }

            ConsoleColor originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);

            Console.ForegroundColor = originalColor;
        }

        public void Error(string msg, bool writeToLog = true)
        {
            if (writeToLog)
            {
                Log.Error(msg);
            }
            
            ConsoleColor originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);

            Console.ForegroundColor = originalColor;
        }

        public void Success(string msg, bool writeToLog = true)
        {
            if (writeToLog)
            {
                Log.Info(msg);
            }

            ConsoleColor originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);

            Console.ForegroundColor = originalColor;
        }
        
    }
}
