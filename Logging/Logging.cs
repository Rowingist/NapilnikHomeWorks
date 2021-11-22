using System;
using System.Collections.Generic;
using System.IO;

namespace Napilnik_homeworks
{
    public interface ILogger
    {
        void Find(string errorMessage);
    }

    public interface IWritable
    {
        void WriteError(string message);
    }

    public interface ISecureWritingPolicy
    {
        public bool TryGetSecurityAсcess();
    }

    class Program
    {
        static void Main(string[] args)
        {
            ILogger pathfinder = new PathFinder();

            pathfinder.Find("Error message");
        }
    }

    public class PathFinder : ILogger
    {
        private List<IWritable> _writers = new List<IWritable>();

        public PathFinder()
        {
            _writers.Add(new ConsoleLogWriter());
            _writers.Add(new FileLogWriter());
            _writers.Add(new SecureConsoleLogWritter(new ConsoleLogWriter()));
            _writers.Add(new SecureFileLogWriter(new FileLogWriter()));
            _writers.Add(new SecureFileAndConsoleLogWriter(new ConsoleLogWriter(), new SecureFileLogWriter(new FileLogWriter())));
        }

        public void Find(string errorMessage)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                _writers[i].WriteError(errorMessage);
            }
        }
    }

    class ConsoleLogWriter : IWritable
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWriter : IWritable
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class LogWriter : IWritable, ISecureWritingPolicy
    {
        private DayOfWeek _dayOfWeek;
        private IWritable _writer;

        public LogWriter(IWritable writer)
        {
            _dayOfWeek = DateTime.Now.DayOfWeek;
            _writer = writer;
        }

        public bool TryGetSecurityAсcess()
        {
            return _dayOfWeek == DayOfWeek.Friday;
        }

        public void WriteError(string message)
        {
            if (TryGetSecurityAсcess())
            {
                _writer.WriteError(message);
            }
            else
            {
                throw new InvalidOperationException("Access atempt declined. Wrong day of Week.");
            }
        }
    }

    class SecureConsoleLogWritter : IWritable
    {
        private IWritable _consoleLogWritter;

        public SecureConsoleLogWritter(ConsoleLogWriter consoleLogWritter)
        {
            _consoleLogWritter = new LogWriter(consoleLogWritter);
        }

        public void WriteError(string message)
        {
            _consoleLogWritter.WriteError(message);
        }
    }

    class SecureFileLogWriter : IWritable
    {
        private IWritable _fileLogWritter;

        public SecureFileLogWriter(FileLogWriter fileLogWritter)
        {
            _fileLogWritter = new LogWriter(fileLogWritter);
        }

        public void WriteError(string message)
        {
            _fileLogWritter.WriteError(message);
        }
    }

    class SecureFileAndConsoleLogWriter : IWritable
    {
        private IWritable _consoleLogWritter;
        private IWritable _secureFileLogWriter;

        public SecureFileAndConsoleLogWriter(ConsoleLogWriter consoleLogWritter, SecureFileLogWriter secureFileLogWriter)
        {
            _consoleLogWritter = consoleLogWritter;
            _secureFileLogWriter = new LogWriter(secureFileLogWriter);
        }

        public void WriteError(string message)
        {
            _consoleLogWritter.WriteError(message);
            _secureFileLogWriter.WriteError(message);
        }
    }
}