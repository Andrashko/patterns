using System;
namespace Structural.Proxy
{
    /* приклад шаблону Проксі.
    Клас моделює надсилання запиту на ІР.
    Проксі якщо мережа доступна то логує надсилання запиту
    Якщо мережа недоступна - надсилає підготовану відповідь 
    */
    public interface ISubject
    {
        string Request();
    }

    class RealSubject : ISubject
    {
        private string Ip;

        public RealSubject(string Ip)
        {
            this.Ip = Ip;
        }
        public string Request()
        {
            return $"Real subject response from {this.Ip}";
        }
    }

    class Proxy : ISubject
    {
        private ISubject _realSubject;
        public Proxy(ISubject realSubject)
        {
            this._realSubject = realSubject;
        }
        public Proxy(string Ip)
        {
            this._realSubject = new RealSubject(Ip);
        }

        public string Request()
        {
            if (this.CheckAccess())
            {
                string response = this._realSubject.Request();
                this.LogAccess(response);
                return response;
            }
            
            return "Proxy response";
        }

        private Random rnd = new Random(2);
        private bool CheckAccess()
        {
            return rnd.NextDouble() < 0.5;
        }

        private void LogAccess(string message)
        {
            Console.WriteLine($"Request was handle by proxy: {message}");
        }
    }
}