using System;
using System.Text.Json;

namespace Behavioral.ChainOfResponsibility
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(Request request);
    }

    class AbstractHandler : IHandler
    {
        private IHandler _nextHandler = null;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual object Handle(Request request)
        {
            if (this._nextHandler == null)
                return null;
            return this._nextHandler.Handle(request);
        }
    }

    public class Request
    {
        public String Login { get; set; }
        public String Password { get; set; }
        public int Count { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now;
        public Request(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    class LogHendler : AbstractHandler
    {
        public override object Handle(Request request)
        {
            Console.WriteLine("Log");
            Console.WriteLine(request);
            return base.Handle(request);
        }
    }

    class AuthorizeHendler : AbstractHandler
    {
        private bool Check(string Login, string Password)
        {
            return Login == "admin" && Password == "admin";
        }
        public override object Handle(Request request)
        {
            Console.WriteLine("Authorize");
            if (Check(request.Login, request.Password))
                return base.Handle(request);
            else
            {
                Console.WriteLine("Wrong login or password");
                return null;
            }
        }
    }
    class ResponceHendler : AbstractHandler
    {
        public override object Handle(Request request)
        {
            Console.WriteLine("Responce");
            return 42;
        }
    }

    class IncHendler : AbstractHandler
    {
        public override object Handle(Request request)
        {
            Console.WriteLine("Inc Count");
            request.Count++;
            return base.Handle(request);
        }
    }
}