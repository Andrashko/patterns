using System;
using System.IO;

namespace Creational
{
    /*
        Приклад використання шаблону Одинак  - система логування 
        Вимоги:
        0. файл логування має бути один для всієї системи
        1. Метод допису в кінець текстовго файлу повідомленя 
        2. метод виведення всього з файлу в консоль
      */
    namespace Singleton
    {


        class LogSystem
        {

            private static LogSystem _instance = null;
            public static LogSystem getInstance(string fileName = "log.txt")
            {
                if (_instance == null)
                {
                    _instance = new LogSystem(fileName);
                }
                return _instance;
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
                stream.Close();
            }

            public void Log(string message)
            {
                stream.WriteLine($"{DateTime.Now} : {message}");
                stream.Flush();
            }

            public void ShowLog()
            {
                //тут потрібно реалізувати вивід логу з файлу в консоль
                Console.WriteLine("Not relised");
            }
        }
    }
}