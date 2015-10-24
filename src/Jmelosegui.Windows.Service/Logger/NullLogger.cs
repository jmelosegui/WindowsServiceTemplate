using System;

namespace Jmelosegui.Windows.Service
{
    class NullLogger : ILogger
    {
        public void Debug(string message)
        {
            Console.WriteLine("Debug - {0}", message);
        }

        public void Info(string message)
        {
            Console.WriteLine("Info - {0}", message);
        }

        public void Warning(string message)
        {
            Console.WriteLine("Warning - {0}", message);
        }

        public void Error(string message)
        {
            Console.WriteLine("Error - {0}", message);
        }

        public void Error(Exception ex)
        {
            Console.WriteLine("Error - {0}", ex.Message);
        }
    }
}
