using SmallRPGLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace SmallRPGLibrary.Services
{
    public class GameLogger
    {
        private static volatile GameLogger _instance;
        private static readonly object _root = new object();
        //private static string _filePath;

        public GameLogger()
        {
            //_filePath = GetLogFilePath();
        }

        public static GameLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_root)
                    {
                        if (_instance == null)
                        {
                            _instance = new GameLogger();
                        }
                    }
                    
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            Log(message, LogLevel.Info);
            
        }

        public void Log(string message, LogLevel level)
        {
            ChangeColor(level);
            Console.WriteLine(message);
            Console.ResetColor();
            //File.AppendAllLines(_filePath, new List<string>{ message }, Encoding.Unicode);
            
        }

        private string GetLogFilePath()
        {
            var logDirectoryPath = ConfigurationManager.AppSettings["GameLogDirectoryPath"];
            var fileName = "GameLog" + DateTime.Now.ToString("d") + ".txt";
            var logFilePath = logDirectoryPath + fileName;
            if(!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
            if (!File.Exists(logFilePath))
            {
                var stream = File.Create(logFilePath);
                stream.Dispose();
            }
            return logFilePath;
        }

        private void ChangeColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Heal:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Improve:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
           
        }
    }
}