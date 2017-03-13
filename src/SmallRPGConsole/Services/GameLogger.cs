using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace SmallRPG.Services
{
    public class GameLogger
    {
        private static volatile GameLogger _instance;
        private static readonly object _root = new object();
        private static string _filePath;

        public GameLogger()
        {
            _filePath = GetLogFilePath();
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
            Console.WriteLine(message);
            File.AppendAllLines(_filePath, new List<string>{ message }, Encoding.Unicode);
            
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
    }
}