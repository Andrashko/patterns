using System;
using System.IO;

namespace Creational
{
    namespace Singleton
    {
        /* 
        Система має
            0. файл має бути один для всієї системи
            1. Метод дописати в кінець текстовго файлу повідомленя 
            2. метод вивести все  з файлу в консоль
        */

        class LogSystem
        {

            private static LogSystem instance = null;
            public static LogSystem getInstance(string fileName = "log.txt")
            {
                if (instance == null)
                {
                    instance = new LogSystem(fileName);
                }
                return instance;
            }
            private string fileName;
            private StreamWriter stream;

            private LogSystem(string fileName)
            {
                this.fileName = fileName;
                this.stream = new StreamWriter(fileName, true);
            }

            ~LogSystem()
            {
                this.stream.Close();
            }

            public void Log(string message)
            {
                this.stream.WriteLine($"{DateTime.Now} : {message}");
                this.stream.Flush();
            }

            public void ShowLog()
            {
                //тут потрібно реалізувати вивід логу з файлу в консоль
                Console.WriteLine("Not relised");
            }
        }
    }
}