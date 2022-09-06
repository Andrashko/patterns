using System;
namespace Structural.Proxy
{
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
            return "Real subject response";
        }
    }

    class Proxy : ISubject
    {
        private RealSubject _realSubject;
        public Proxy(RealSubject realSubject)
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
            else
                return "Proxy response";
        }

        private Random rnd = new Random(2);
        private bool CheckAccess()
        {
            return rnd.NextDouble() < 0.5;
        }

        private void LogAccess(string message)
        {
            Console.WriteLine($"Request was proxied: {message}");
        }
    }
}